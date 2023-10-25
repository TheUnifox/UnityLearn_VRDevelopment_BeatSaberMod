// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapEditorRuntimeLogger
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands;
using System;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class BeatmapEditorRuntimeLogger : IInitializable, IDisposable
  {
    [Inject]
    private readonly SignalBus _signalBus;

    public void Initialize() => Application.logMessageReceived += new Application.LogCallback(this.HandleApplicationLogMessageReceived);

    public void Dispose() => Application.logMessageReceived -= new Application.LogCallback(this.HandleApplicationLogMessageReceived);

    private void HandleApplicationLogMessageReceived(
      string logString,
      string stacktrace,
      LogType type)
    {
      this._signalBus.Fire<CommandMessageSignal>(new CommandMessageSignal(CommandMessageType.Normal, logString));
    }
  }
}
