// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.SelectMultipleEventsCommand
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
  public class SelectMultipleEventsCommand : IBeatmapEditorCommand
  {
    private const float kMinBeatDistance = 0.00390625f;
    [Inject]
    private readonly SelectMultipleEventsSignal _signal;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly IBeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly IReadonlyBeatmapEventsDataModel _beatmapEventsDataModel;
    [Inject]
    private readonly EventsSelectionState _eventsSelectionState;

    public void Execute()
    {
      if (!this._signal.commit)
      {
        this._eventsSelectionState.Clear();
      }
      else
      {
        if (!this._signal.addToSelection)
          this._eventsSelectionState.Clear();
        float startBeat = this._eventsSelectionState.startBeat;
        float endBeat = this._eventsSelectionState.endBeat;
        for (int startTrackIndex = this._eventsSelectionState.startTrackIndex; startTrackIndex <= this._eventsSelectionState.endTrackIndex; ++startTrackIndex)
        {
          foreach (BasicEventEditorData basicEventData in this._beatmapEventsDataModel.GetBasicEventsInterval(this._beatmapDataModel.environmentTrackDefinition.visibleTrackInfos[startTrackIndex].basicBeatmapEventType, startBeat - 1f / 256f, endBeat + 1f / 256f))
            this.AddSingle(basicEventData);
        }
        BasicEventEditorData[] array = this._eventsSelectionState.events.Select<BeatmapEditorObjectId, BasicEventEditorData>((Func<BeatmapEditorObjectId, BasicEventEditorData>) (id => this._beatmapEventsDataModel.GetBasicEventById(id))).ToArray<BasicEventEditorData>();
        this._eventsSelectionState.startBeat = ((IEnumerable<BasicEventEditorData>) array).Select<BasicEventEditorData, float>((Func<BasicEventEditorData, float>) (evt => evt.beat)).Prepend<float>(float.MaxValue).Min();
        this._eventsSelectionState.allEventsSame = ((IEnumerable<BasicEventEditorData>) array).GroupBy<BasicEventEditorData, BasicBeatmapEventType>((Func<BasicEventEditorData, BasicBeatmapEventType>) (evt => evt.type)).Count<IGrouping<BasicBeatmapEventType, BasicEventEditorData>>() < 2;
        this._signalBus.Fire<EventsSelectionStateUpdatedSignal>();
      }
    }

    private void AddSingle(BasicEventEditorData basicEventData)
    {
      if (basicEventData == (BasicEventEditorData) null || this._eventsSelectionState.IsSelected(basicEventData.id))
        return;
      this._eventsSelectionState.Add(basicEventData.id);
    }
  }
}
