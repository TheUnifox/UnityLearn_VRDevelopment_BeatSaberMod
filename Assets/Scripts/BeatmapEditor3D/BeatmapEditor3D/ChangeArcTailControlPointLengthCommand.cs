// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.ChangeArcTailControlPointLengthCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using Zenject;

namespace BeatmapEditor3D
{
  public class ChangeArcTailControlPointLengthCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly ChangeArcTailControlPointLengthSignal _signal;
    [Inject]
    private readonly BeatmapObjectsState _readonlyBeatmapObjectsState;
    [Inject]
    private readonly SignalBus _signalBus;

    public void Execute()
    {
      this._readonlyBeatmapObjectsState.arcTailControlPointLengthMultiplier = this._signal.length;
      this._signalBus.Fire<ArcControlPointLengthChangedSignal>();
    }
  }
}
