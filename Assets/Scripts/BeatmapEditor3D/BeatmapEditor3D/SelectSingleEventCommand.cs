// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.SelectSingleEventCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace BeatmapEditor3D
{
  public class SelectSingleEventCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly SelectSingleEventSignal _signal;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly IReadonlyBeatmapEventsDataModel _beatmapEventsDataModel;
    [Inject]
    private readonly EventsSelectionState _eventsSelectionState;

    public void Execute()
    {
      if (this._eventsSelectionState.events.Count == 1 && this._signal.basicEventEditorData.id == this._eventsSelectionState.events[0])
      {
        this._eventsSelectionState.Clear();
        this._signalBus.Fire<EventsSelectionStateUpdatedSignal>();
      }
      else
      {
        if (!this._signal.addToSelection)
          this._eventsSelectionState.Clear();
        this.AddSingle(this._signal.basicEventEditorData);
        BasicEventEditorData[] array = this._eventsSelectionState.events.Select<BeatmapEditorObjectId, BasicEventEditorData>((Func<BeatmapEditorObjectId, BasicEventEditorData>) (id => this._beatmapEventsDataModel.GetBasicEventById(id))).ToArray<BasicEventEditorData>();
        this._eventsSelectionState.startBeat = ((IEnumerable<BasicEventEditorData>) array).Select<BasicEventEditorData, float>((Func<BasicEventEditorData, float>) (evt => evt.beat)).Prepend<float>(float.MaxValue).Min();
        this._eventsSelectionState.allEventsSame = ((IEnumerable<BasicEventEditorData>) array).GroupBy<BasicEventEditorData, BasicBeatmapEventType>((Func<BasicEventEditorData, BasicBeatmapEventType>) (evt => evt.type)).Count<IGrouping<BasicBeatmapEventType, BasicEventEditorData>>() < 2;
        this._signalBus.Fire<EventsSelectionStateUpdatedSignal>();
      }
    }

    private void AddSingle(BasicEventEditorData basicEventData)
    {
      if (basicEventData == (BasicEventEditorData) null)
        return;
      if (this._eventsSelectionState.IsSelected(basicEventData.id))
        this._eventsSelectionState.Remove(basicEventData.id);
      else
        this._eventsSelectionState.Add(basicEventData.id);
    }
  }
}
