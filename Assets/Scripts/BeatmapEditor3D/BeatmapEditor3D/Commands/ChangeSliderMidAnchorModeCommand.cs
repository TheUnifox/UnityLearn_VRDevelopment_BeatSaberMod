// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.ChangeSliderMidAnchorModeCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using BeatmapEditor3D.Types;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class ChangeSliderMidAnchorModeCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly ChangeArcMidAnchorModeSignal _signal;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly BeatmapObjectsState _beatmapObjectsState;

    public void Execute()
    {
      if (this._beatmapObjectsState.beatmapObjectType != BeatmapObjectType.Arc)
        this._signalBus.Fire<ChangeBeatmapObjectTypeSignal>(new ChangeBeatmapObjectTypeSignal(BeatmapObjectType.Arc));
      this._beatmapObjectsState.arcMidAnchorMode = this._signal.sliderMidAnchorMode;
      this._signalBus.Fire<ArcMidAnchorModeChangedSignal>(new ArcMidAnchorModeChangedSignal(this._signal.sliderMidAnchorMode));
    }
  }
}
