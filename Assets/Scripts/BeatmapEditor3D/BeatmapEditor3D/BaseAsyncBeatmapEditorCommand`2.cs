// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BaseAsyncBeatmapEditorCommand`2
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public abstract class BaseAsyncBeatmapEditorCommand<TInput, TResult> : IBeatmapEditorCommand
  {
    [Inject]
    private readonly RunningAsyncBeatmapEditorCommandCache _runningAsyncBeatmapEditorCommandCache;

    protected abstract BaseAsyncBeatmapEditorCommand<TInput, TResult>.MultipleSameTasksRunPolicy multipleSameTasksRunPolicy { get; }

    public async void Execute()
    {
      BaseAsyncBeatmapEditorCommand<TInput, TResult> beatmapEditorCommand1 = this;
      Type taskType = beatmapEditorCommand1.GetType();
      if (beatmapEditorCommand1.multipleSameTasksRunPolicy == BaseAsyncBeatmapEditorCommand<TInput, TResult>.MultipleSameTasksRunPolicy.CancelRunning)
        beatmapEditorCommand1.CancelRunningOldTask(taskType);
      if (beatmapEditorCommand1.multipleSameTasksRunPolicy == BaseAsyncBeatmapEditorCommand<TInput, TResult>.MultipleSameTasksRunPolicy.DontRunNew && beatmapEditorCommand1._runningAsyncBeatmapEditorCommandCache.IsTypeTaskRunning(taskType))
        return;
      try
      {
        BaseAsyncBeatmapEditorCommand<TInput, TResult> beatmapEditorCommand = beatmapEditorCommand1;
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        beatmapEditorCommand1._runningAsyncBeatmapEditorCommandCache.AddTaskToRunning(taskType, cancellationTokenSource);
        TResult result = await Task.Run<TResult>((Func<TResult>) (() => beatmapEditorCommand.ExecuteAsync(beatmapEditorCommand.GatherTaskData(), cancellationTokenSource.Token)), cancellationTokenSource.Token);
        if (cancellationTokenSource.IsCancellationRequested)
          return;
        beatmapEditorCommand1.TaskFinished(result);
      }
      catch (TaskCanceledException)
      {
      }
      catch (Exception ex)
      {
        Debug.LogWarning((object) string.Format("Async command {0} threw exception {1}", (object) taskType, (object) ex));
      }
      finally
      {
        CancellationTokenSource cancellationTokenSource;
        if (beatmapEditorCommand1._runningAsyncBeatmapEditorCommandCache.TryGetTypeTaskRunning(taskType, out cancellationTokenSource))
        {
          cancellationTokenSource?.Dispose();
          beatmapEditorCommand1._runningAsyncBeatmapEditorCommandCache.RemoveTaskFromRunning(taskType);
        }
      }
    }

    protected virtual TInput GatherTaskData() => default (TInput);

    protected virtual void TaskFinished(TResult result)
    {
    }

    protected abstract TResult ExecuteAsync(TInput input, CancellationToken cancellationToken);

    private void CancelRunningOldTask(Type type)
    {
      CancellationTokenSource cancellationTokenSource;
      if (!this._runningAsyncBeatmapEditorCommandCache.TryGetTypeTaskRunning(type, out cancellationTokenSource))
        return;
      cancellationTokenSource?.Cancel();
      cancellationTokenSource?.Dispose();
      this._runningAsyncBeatmapEditorCommandCache.RemoveTaskFromRunning(type);
    }

    protected enum MultipleSameTasksRunPolicy
    {
      RunAll,
      CancelRunning,
      DontRunNew,
    }
  }
}
