// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.MoveEventsSelectionCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class MoveEventsSelectionCommand : IBeatmapEditorCommandWithHistory, IBeatmapEditorCommand
  {
    [Inject]
    private readonly MoveEventsSelectionSignal _signal;
    [Inject]
    private readonly EventsSelectionState _eventsSelectionState;
    [Inject]
    private readonly BeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly BeatmapBasicEventsDataModel _beatmapBasicEventsDataModel;
    [Inject]
    private readonly SignalBus _signalBus;
    private IEnumerable<BasicEventEditorData> _originalEvents;
    private List<BasicEventEditorData> _movedEvents;

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      if (this._eventsSelectionState.events.Count == 0)
        return;
      this._originalEvents = (IEnumerable<BasicEventEditorData>) this._eventsSelectionState.events.Select<BeatmapEditorObjectId, BasicEventEditorData>((Func<BeatmapEditorObjectId, BasicEventEditorData>) (id => this._beatmapBasicEventsDataModel.GetBasicEventById(id))).OrderBy<BasicEventEditorData, float>((Func<BasicEventEditorData, float>) (e => e.beat)).ThenBy<BasicEventEditorData, float>((Func<BasicEventEditorData, float>) (e => this.GetEventOrderByValue(this._signal.moveDirection, e))).ToList<BasicEventEditorData>();
      this._movedEvents = new List<BasicEventEditorData>(this._eventsSelectionState.events.Count);
      bool flag1 = false;
      foreach (BasicEventEditorData originalEvent in this._originalEvents)
      {
        BasicBeatmapEventType newEventPosition = this.GetNewEventPosition(this._signal.moveDirection, originalEvent.type);
        if (originalEvent.type == newEventPosition)
        {
          this._movedEvents.Add(originalEvent);
        }
        else
        {
          BasicEventEditorData basicEventEditorData1 = originalEvent.hasEndTime ? BasicEventEditorData.CreateNewWithId(originalEvent.id, newEventPosition, originalEvent.beat, originalEvent.value, originalEvent.floatValue) : BasicEventEditorData.CreateNewWithId(originalEvent.id, newEventPosition, originalEvent.beat, originalEvent.value, originalEvent.floatValue, originalEvent.endBeat, originalEvent.endValue, originalEvent.endFloatValue);
          BasicEventEditorData basicEventEditorData2 = this._beatmapBasicEventsDataModel.GetBasicEventAt(basicEventEditorData1.type, basicEventEditorData1.beat);
          int num = !(basicEventEditorData2 != (BasicEventEditorData) null) ? 0 : (this._originalEvents.Any<BasicEventEditorData>(new Func<BasicEventEditorData, bool>(basicEventEditorData2.PositionEquals)) ? 1 : 0);
          bool flag2 = this._movedEvents.Any<BasicEventEditorData>(new Func<BasicEventEditorData, bool>(basicEventEditorData1.PositionEquals));
          if (num != 0 && !flag2)
            basicEventEditorData2 = (BasicEventEditorData) null;
          flag1 |= basicEventEditorData2 == (BasicEventEditorData) null;
          this._movedEvents.Add(basicEventEditorData2 == (BasicEventEditorData) null ? basicEventEditorData1 : originalEvent);
        }
      }
      if (!flag1)
        return;
      this.shouldAddToHistory = true;
      this.Redo();
    }

    public void Undo()
    {
      this._eventsSelectionState.Clear();
      this._beatmapBasicEventsDataModel.Remove((IEnumerable<BasicEventEditorData>) this._movedEvents);
      this._beatmapBasicEventsDataModel.Insert(this._originalEvents);
      this._eventsSelectionState.AddRange(this._originalEvents.Select<BasicEventEditorData, BeatmapEditorObjectId>((Func<BasicEventEditorData, BeatmapEditorObjectId>) (evt => evt.id)));
      this._signalBus.Fire<EventsSelectionStateUpdatedSignal>();
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public void Redo()
    {
      this._eventsSelectionState.Clear();
      this._beatmapBasicEventsDataModel.Remove(this._originalEvents);
      this._beatmapBasicEventsDataModel.Insert((IEnumerable<BasicEventEditorData>) this._movedEvents);
      this._eventsSelectionState.AddRange(this._movedEvents.Select<BasicEventEditorData, BeatmapEditorObjectId>((Func<BasicEventEditorData, BeatmapEditorObjectId>) (evt => evt.id)));
      this._signalBus.Fire<EventsSelectionStateUpdatedSignal>();
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    private BasicBeatmapEventType GetNewEventPosition(
      MoveEventsSelectionSignal.MoveDirection moveDirection,
      BasicBeatmapEventType type)
    {
      EnvironmentTracksDefinitionSO.BasicEventTrackInfo environmentTrackInfo = this._beatmapDataModel.environmentTrackDefinition[type];
      EnvironmentTracksDefinitionSO.BasicEventTrackInfo[] array = this._beatmapDataModel.environmentTrackDefinition[environmentTrackInfo.trackDefinition];
      int index = Array.FindIndex<EnvironmentTracksDefinitionSO.BasicEventTrackInfo>(array, (Predicate<EnvironmentTracksDefinitionSO.BasicEventTrackInfo>) (track => track.basicBeatmapEventType == environmentTrackInfo.basicBeatmapEventType));
      return moveDirection != MoveEventsSelectionSignal.MoveDirection.Left ? array[Mathf.Clamp(index + 1, 0, array.Length - 1)].basicBeatmapEventType : array[Mathf.Clamp(index - 1, 0, array.Length - 1)].basicBeatmapEventType;
    }

    private float GetEventOrderByValue(
      MoveEventsSelectionSignal.MoveDirection moveDirection,
      BasicEventEditorData basicEventEditorData)
    {
      EnvironmentTracksDefinitionSO.BasicEventTrackInfo environmentTrackInfo = this._beatmapDataModel.environmentTrackDefinition[basicEventEditorData.type];
      return (float) (Array.FindIndex<EnvironmentTracksDefinitionSO.BasicEventTrackInfo>(this._beatmapDataModel.environmentTrackDefinition[environmentTrackInfo.trackDefinition], (Predicate<EnvironmentTracksDefinitionSO.BasicEventTrackInfo>) (track => track.basicBeatmapEventType == environmentTrackInfo.basicBeatmapEventType)) * (moveDirection == MoveEventsSelectionSignal.MoveDirection.Left ? 1 : -1));
    }
  }
}
