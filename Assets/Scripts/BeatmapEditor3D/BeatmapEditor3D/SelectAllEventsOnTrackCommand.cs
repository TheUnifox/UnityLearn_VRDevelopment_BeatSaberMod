// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.SelectAllEventsOnTrackCommand
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
  public class SelectAllEventsOnTrackCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly SelectAllEventsOnTrackSignal _signal;
    [Inject]
    private readonly EventsSelectionState _eventsSelectionState;
    [Inject]
    private readonly IReadonlyBeatmapEventsDataModel _beatmapEventsDataModel;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly BeatmapDataModel _beatmapDataModel;

    public void Execute()
    {
      BasicEventEditorData[] array = this._beatmapEventsDataModel.GetAllDataIn(this._beatmapDataModel.environmentTrackDefinition.visibleTrackInfos[this._signal.visibleTrackId].basicBeatmapEventType).ToArray<BasicEventEditorData>();
      this._eventsSelectionState.Clear();
      this._eventsSelectionState.AddRange(((IEnumerable<BasicEventEditorData>) array).Select<BasicEventEditorData, BeatmapEditorObjectId>((Func<BasicEventEditorData, BeatmapEditorObjectId>) (evt => evt.id)));
      this._eventsSelectionState.startBeat = ((IEnumerable<BasicEventEditorData>) array).Select<BasicEventEditorData, float>((Func<BasicEventEditorData, float>) (evt => evt.beat)).Prepend<float>(float.MaxValue).Min();
      this._eventsSelectionState.allEventsSame = true;
      this._signalBus.Fire<EventsSelectionStateUpdatedSignal>();
    }
  }
}
