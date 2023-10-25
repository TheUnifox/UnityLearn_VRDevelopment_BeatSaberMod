// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.SelectSingleEventBoxesEventCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class SelectSingleEventBoxesEventCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly SelectSingleEventBoxesEventSignal _signal;
    [Inject]
    private readonly EventBoxesSelectionState _selectionState;
    [Inject]
    private readonly EventBoxGroupsState _eventBoxGroupsState;
    [Inject]
    private readonly SignalBus _signalBus;

    public void Execute()
    {
      if (this._selectionState.events.Count == 1)
      {
        BeatmapEditorObjectId eventBoxId1 = this._signal.eventBoxId;
        BeatmapEditorObjectId eventId = this._signal.eventId;
        (BeatmapEditorObjectId eventBoxId, BeatmapEditorObjectId eventId) tuple = this._selectionState.events[0];
        BeatmapEditorObjectId eventBoxId2 = tuple.eventBoxId;
        if (eventBoxId1 == eventBoxId2 && eventId == tuple.eventId)
        {
          this._selectionState.Clear();
          this._signalBus.Fire<EventBoxesSelectionStateUpdatedSignal>();
          return;
        }
      }
      if (!this._signal.addToSelection)
        this._selectionState.Clear();
      this._selectionState.AddOrRemoveIfSelected(this._signal.eventBoxId, this._signal.eventId);
      this._selectionState.eventBoxGroupType = this._eventBoxGroupsState.currentHoverGroupType;
      this._signalBus.Fire<EventBoxesSelectionStateUpdatedSignal>();
    }
  }
}
