// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.RunningAsyncBeatmapEditorCommandCache
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using System.Collections.Generic;
using System.Threading;

namespace BeatmapEditor3D
{
  public class RunningAsyncBeatmapEditorCommandCache
  {
    private readonly Dictionary<Type, CancellationTokenSource> _runningTasks = new Dictionary<Type, CancellationTokenSource>();

    public bool IsTypeTaskRunning(Type type) => this._runningTasks.ContainsKey(type);

    public bool TryGetTypeTaskRunning(
      Type type,
      out CancellationTokenSource cancellationTokenSource)
    {
      return this._runningTasks.TryGetValue(type, out cancellationTokenSource);
    }

    public void RemoveTaskFromRunning(Type type) => this._runningTasks.Remove(type);

    public void AddTaskToRunning(Type type, CancellationTokenSource cancellationTokenSource) => this._runningTasks.Add(type, cancellationTokenSource);
  }
}
