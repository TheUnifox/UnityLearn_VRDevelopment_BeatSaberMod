// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.MoveHoveredBeatmapObjectOnGridCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Types;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class MoveHoveredBeatmapObjectOnGridCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly MoveHoveredBeatmapObjectOnGridSignal _signal;
    [Inject]
    private readonly BeatmapObjectsSelectionState _beatmapObjectsSelectionState;
    [Inject]
    private readonly SignalBus _signalBus;

    public void Execute()
    {
      switch (this._beatmapObjectsSelectionState.draggedBeatmapObjectType)
      {
        case BeatmapObjectType.Note:
          this._signalBus.Fire<MoveNoteOnGridSignal>(new MoveNoteOnGridSignal(this._beatmapObjectsSelectionState.draggedBeatmapObjectId, this._signal.position));
          break;
        case BeatmapObjectType.Bomb:
          this._signalBus.Fire<MoveNoteOnGridSignal>(new MoveNoteOnGridSignal(this._beatmapObjectsSelectionState.draggedBeatmapObjectId, this._signal.position));
          break;
        case BeatmapObjectType.Obstacle:
          this._signalBus.Fire<MoveObstacleOnGridSignal>(new MoveObstacleOnGridSignal(this._beatmapObjectsSelectionState.draggedBeatmapObjectId, this._signal.position));
          break;
        case BeatmapObjectType.Arc:
          this._signalBus.Fire<MoveArcOnGridSignal>(new MoveArcOnGridSignal(this._beatmapObjectsSelectionState.draggedBeatmapObjectId, this._signal.position, this._beatmapObjectsSelectionState.draggedBeatmapObjectCellData));
          break;
        case BeatmapObjectType.Chain:
          this._signalBus.Fire<MoveChainOnGridSignal>(new MoveChainOnGridSignal(this._beatmapObjectsSelectionState.draggedBeatmapObjectId, this._signal.position, this._beatmapObjectsSelectionState.draggedBeatmapObjectCellData));
          break;
      }
    }
  }
}
