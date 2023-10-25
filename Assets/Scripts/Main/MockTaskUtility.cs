// Decompiled with JetBrains decompiler
// Type: MockTaskUtility
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using BGNet.Core;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class MockTaskUtility : ITaskUtility
{
  protected readonly List<(float time, TaskCompletionSource<int> taskSource, CancellationToken cancellationToken)> _delayedTasks = new List<(float, TaskCompletionSource<int>, CancellationToken)>();

  public virtual void Tick() => this.AdvanceDelayedTasks(Time.time);

  public virtual void AdvanceDelayedTasks(float time)
  {
    for (int index = this._delayedTasks.Count - 1; index >= 0; --index)
    {
      (float time, TaskCompletionSource<int> taskSource, CancellationToken cancellationToken) delayedTask = this._delayedTasks[index];
      if (delayedTask.cancellationToken.IsCancellationRequested)
      {
        delayedTask.taskSource.TrySetCanceled(delayedTask.cancellationToken);
        this._delayedTasks.RemoveAt(index);
      }
      else if ((double) time >= (double) delayedTask.time)
      {
        delayedTask.taskSource.TrySetResult(0);
        this._delayedTasks.RemoveAt(index);
      }
    }
  }

  public virtual Task Delay(TimeSpan timeSpan, CancellationToken cancellationToken = default (CancellationToken))
  {
    TaskCompletionSource<int> completionSource = new TaskCompletionSource<int>();
    this._delayedTasks.Add((Time.time + (float) timeSpan.TotalSeconds, completionSource, cancellationToken));
    return (Task) completionSource.Task;
  }

  public virtual Task<T2> ContinueWith<T1, T2>(Task<T1> task, Func<Task<T1>, Task<T2>> continuation) => task.ContinueWith<Task<T2>>(continuation).Unwrap<T2>();

  public virtual void Wait(Task task) => throw new NotImplementedException();

  public virtual T Wait<T>(Task<T> task) => throw new NotImplementedException();

  public virtual Task Run(System.Action action, CancellationToken cancellationToken = default (CancellationToken)) => throw new NotImplementedException();

  public virtual Task Run(Func<Task> func, CancellationToken cancellationToken = default (CancellationToken)) => throw new NotImplementedException();

  public virtual Task<T> Run<T>(Func<T> func, CancellationToken cancellationToken = default (CancellationToken)) => throw new NotImplementedException();

  public virtual CancellationToken CancellationTokenWithDelay(TimeSpan timeSpan) => throw new NotImplementedException();
}
