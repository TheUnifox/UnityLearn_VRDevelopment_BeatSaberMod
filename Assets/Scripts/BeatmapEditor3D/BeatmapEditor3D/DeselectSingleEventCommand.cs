// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DeselectSingleEventCommand
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
  public class DeselectSingleEventCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly DeselectSingleEventSignal _signal;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly IReadonlyBeatmapEventsDataModel _beatmapEventsDataModel;
    [Inject]
    private readonly EventsSelectionState _eventsSelectionState;

    public void Execute()
    {
      this._eventsSelectionState.Remove(this._signal.basicEventEditorData.id);
      BasicEventEditorData[] array = this._eventsSelectionState.events.Select<BeatmapEditorObjectId, BasicEventEditorData>((Func<BeatmapEditorObjectId, BasicEventEditorData>) (id => this._beatmapEventsDataModel.GetBasicEventById(id))).ToArray<BasicEventEditorData>();
      this._eventsSelectionState.startBeat = ((IEnumerable<BasicEventEditorData>) array).Select<BasicEventEditorData, float>((Func<BasicEventEditorData, float>) (evt => evt.beat)).Prepend<float>(float.MaxValue).Min();
      this._eventsSelectionState.allEventsSame = ((IEnumerable<BasicEventEditorData>) array).GroupBy<BasicEventEditorData, BasicBeatmapEventType>((Func<BasicEventEditorData, BasicBeatmapEventType>) (evt => evt.type)).Count<IGrouping<BasicBeatmapEventType, BasicEventEditorData>>() < 2;
      this._signalBus.Fire<EventsSelectionStateUpdatedSignal>();
    }
  }
}
