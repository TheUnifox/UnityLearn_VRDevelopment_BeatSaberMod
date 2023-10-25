// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.LevelEditor.ChangeHoverCellDataCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using Zenject;

namespace BeatmapEditor3D.LevelEditor
{
  public class ChangeHoverCellDataCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly BeatmapObjectsState _readonlyBeatmapObjectsState;
    [Inject]
    private readonly ChangeHoverSignal _signal;
    [Inject]
    private readonly SignalBus _signalBus;

    public void Execute()
    {
      if (this._signal.hoveredCellOrigin == ChangeHoverSignal.HoverOrigin.Grid)
        this._readonlyBeatmapObjectsState.hoveredGridCellData = this._signal.hoveredCellData;
      if (this._signal.hoveredCellOrigin == ChangeHoverSignal.HoverOrigin.Object)
      {
        this._readonlyBeatmapObjectsState.hoveredBeatmapObjectId = this._signal.hoveredId;
        this._readonlyBeatmapObjectsState.hoveredNoteCellData = this._signal.hoveredCellData;
      }
      this._signalBus.Fire<HoverChangedSignal>();
    }
  }
}
