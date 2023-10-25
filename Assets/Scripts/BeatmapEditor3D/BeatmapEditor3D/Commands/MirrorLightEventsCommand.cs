// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.MirrorLightEventsCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using BeatmapEditor3D.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class MirrorLightEventsCommand : IBeatmapEditorCommandWithHistory, IBeatmapEditorCommand
  {
    [Inject]
    private readonly EventsSelectionState _eventsSelectionState;
    [Inject]
    private readonly BeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly BeatmapBasicEventsDataModel _beatmapBasicEventsDataModel;
    [Inject]
    private readonly SignalBus _signalBus;
    private readonly List<BasicEventEditorData> _eventsToRemove = new List<BasicEventEditorData>();
    private readonly List<BasicEventEditorData> _eventsToAdd = new List<BasicEventEditorData>();

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      foreach (BasicEventEditorData basicEventEditorData in this._eventsSelectionState.events.Select<BeatmapEditorObjectId, BasicEventEditorData>((Func<BeatmapEditorObjectId, BasicEventEditorData>) (eventId => this._beatmapBasicEventsDataModel.GetBasicEventById(eventId))))
      {
        if (this._beatmapDataModel.environmentTrackDefinition[basicEventEditorData.type].trackToolbarType == TrackToolbarType.Lights)
          this._eventsToRemove.Add(basicEventEditorData);
      }
      if (this._eventsToRemove.Count == 0)
        return;
      this.shouldAddToHistory = true;
      foreach (BasicEventEditorData basicEventEditorData in this._eventsToRemove)
      {
        LightEventsPayload lightEventsPayload = new LightEventsPayload(basicEventEditorData.value, basicEventEditorData.floatValue);
        lightEventsPayload.color = lightEventsPayload.color == LightColor.Blue ? LightColor.Red : LightColor.Blue;
        this._eventsToAdd.Add(BasicEventEditorData.CreateNewWithId(basicEventEditorData.id, basicEventEditorData.type, basicEventEditorData.beat, lightEventsPayload.ToValue(), lightEventsPayload.ToAltValue()));
      }
      this.ReplaceEvents(this._eventsToAdd, this._eventsToRemove);
    }

    public void Undo() => this.ReplaceEvents(this._eventsToRemove, this._eventsToAdd);

    public void Redo() => this.ReplaceEvents(this._eventsToAdd, this._eventsToRemove);

    private void ReplaceEvents(
      List<BasicEventEditorData> eventsToAdd,
      List<BasicEventEditorData> eventsToRemove)
    {
      foreach (BasicEventEditorData basicEventToRemove in eventsToRemove)
        this._beatmapBasicEventsDataModel.Remove(basicEventToRemove);
      foreach (BasicEventEditorData basicEventToAdd in eventsToAdd)
        this._beatmapBasicEventsDataModel.Insert(basicEventToAdd);
      this._signalBus.Fire<EventsSelectionStateUpdatedSignal>();
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }
  }
}
