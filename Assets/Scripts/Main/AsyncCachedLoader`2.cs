// Decompiled with JetBrains decompiler
// Type: AsyncCachedLoader`2
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class AsyncCachedLoader<TKey, TValue>
{
  protected List<AsyncCachedLoader<TKey, TValue>.ScheduledTask> _scheduledTaskList = new List<AsyncCachedLoader<TKey, TValue>.ScheduledTask>();
  protected HMCache<TKey, TValue> _cache;
  protected Func<TKey, CancellationToken, Task<TValue>> _resultValueFunc;
  protected bool _isLoading;

  public virtual void ClearCache() => this._cache.Clear();

  public AsyncCachedLoader(
    int maxNumberCachedElements,
    Func<TKey, CancellationToken, Task<TValue>> resultValueFunc)
  {
    this._resultValueFunc = resultValueFunc;
    this._cache = new HMCache<TKey, TValue>(maxNumberCachedElements);
  }

  public virtual async Task<TValue> LoadAsync(TKey keyId, CancellationToken cancellationToken)
  {
    AsyncCachedLoader<TKey, TValue>.ScheduledTask scheduledTask = new AsyncCachedLoader<TKey, TValue>.ScheduledTask(keyId, new TaskCompletionSource<TValue>(), cancellationToken);
    this._scheduledTaskList.Add(scheduledTask);
    if (!this._isLoading)
      this.LoadAllAsync();
    return await scheduledTask.taskCompletionSource.Task;
  }

  public virtual async void LoadAllAsync()
  {
    if (this._isLoading)
      return;
    this._isLoading = true;
    while (this._scheduledTaskList.Count > 0)
    {
      AsyncCachedLoader<TKey, TValue>.ScheduledTask scheduledTask = this._scheduledTaskList[this._scheduledTaskList.Count - 1];
      this._scheduledTaskList.Remove(scheduledTask);
      TValue obj = default (TValue);
      if (scheduledTask.cancellationToken.IsCancellationRequested)
        scheduledTask.taskCompletionSource.TrySetCanceled();
      else if (this._cache.IsInCache(scheduledTask.keyId))
      {
        scheduledTask.taskCompletionSource.TrySetResult(this._cache.GetFromCache(scheduledTask.keyId));
      }
      else
      {
        TValue result;
        try
        {
          result = await this._resultValueFunc(scheduledTask.keyId, scheduledTask.cancellationToken);
        }
        catch (OperationCanceledException ex)
        {
          scheduledTask.taskCompletionSource.TrySetCanceled();
          continue;
        }
        if ((object) result != null)
          this._cache.PutToCache(scheduledTask.keyId, result);
        scheduledTask.taskCompletionSource.TrySetResult(result);
        scheduledTask = new AsyncCachedLoader<TKey, TValue>.ScheduledTask();
      }
    }
    this._isLoading = false;
  }

  public struct ScheduledTask
  {
    public TKey keyId;
    public TaskCompletionSource<TValue> taskCompletionSource;
    public CancellationToken cancellationToken;

    public ScheduledTask(
      TKey keyId,
      TaskCompletionSource<TValue> taskCompletionSource,
      CancellationToken cancellationToken)
    {
      this.keyId = keyId;
      this.taskCompletionSource = taskCompletionSource;
      this.cancellationToken = cancellationToken;
    }
  }
}
