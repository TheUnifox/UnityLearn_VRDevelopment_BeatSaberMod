// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.MoveEventToTrackCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class MoveEventToTrackCommand : 
    IBeatmapEditorCommandWithHistoryMergeable,
    IBeatmapEditorCommandWithHistory,
    IBeatmapEditorCommand
  {
    [Inject]
    private readonly MoveEventToTrackSignal _signal;
    [Inject]
    private readonly BeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly IBeatmapEventsDataModel _beatmapEventsDataModel;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly BasicEventsState _basicBasicEventsState;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly EventsSelectionState _eventsSelectionState;
    private BasicEventEditorData _prevBasicEventData;
    private BasicEventEditorData _newBasicEventData;

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      BasicBeatmapEventType beatmapEventType = this._beatmapDataModel.environmentTrackDefinition[this._basicBasicEventsState.currentEventsPage][this._signal.pageTrackId].basicBeatmapEventType;
      if (this._signal.basicEventEditorData == (BasicEventEditorData) null || !this.CanMoveEvt(this._signal.basicEventEditorData.type, beatmapEventType) || this._beatmapEventsDataModel.GetBasicEventAt(beatmapEventType, this._beatmapState.beat) != (BasicEventEditorData) null)
        return;
      this._prevBasicEventData = this._signal.basicEventEditorData;
      this._newBasicEventData = BasicEventEditorData.CreateNewWithId(this._prevBasicEventData.id, beatmapEventType, this._beatmapState.beat, this._prevBasicEventData.value, this._prevBasicEventData.floatValue);
      this._eventsSelectionState.draggedBasicEventData = this._newBasicEventData;
      this.shouldAddToHistory = true;
      this.Redo();
    }

    public void Undo()
    {
      this._beatmapEventsDataModel.Remove(this._newBasicEventData);
      this._beatmapEventsDataModel.Insert(this._prevBasicEventData);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public void Redo()
    {
      this._beatmapEventsDataModel.Remove(this._prevBasicEventData);
      this._beatmapEventsDataModel.Insert(this._newBasicEventData);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public bool ShouldMergeWith(
      IBeatmapEditorCommandWithHistoryMergeable previousCommand)
    {
      return previousCommand is MoveEventToTrackCommand eventToTrackCommand && this._prevBasicEventData.id == eventToTrackCommand._newBasicEventData.id;
    }

    public void MergeWith(
      IBeatmapEditorCommandWithHistoryMergeable previousCommand)
    {
      this._prevBasicEventData = ((MoveEventToTrackCommand) previousCommand)._prevBasicEventData;
    }

    private bool CanMoveEvt(BasicBeatmapEventType evtType, BasicBeatmapEventType trackType) => (Object) this._beatmapDataModel.environmentTrackDefinition[evtType].trackDefinition == (Object) this._beatmapDataModel.environmentTrackDefinition[trackType].trackDefinition;
  }
}
