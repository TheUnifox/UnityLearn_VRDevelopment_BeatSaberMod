// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.PasteEventsCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace BeatmapEditor3D
{
  public class PasteEventsCommand : 
    IBeatmapEditorCommandWithHistory,
    IBeatmapEditorCommand,
    IBeatmapEditorCommandMessageProvider
  {
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly IBeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly IBeatmapEventsDataModel _beatmapEventsDataModel;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly IBasicEventsState _basicEventsState;
    [Inject]
    private readonly BeatmapObjectsClipboardState _beatmapObjectsClipboardState;
    [Inject]
    private readonly EventsClipboardState _eventsClipboardState;
    [Inject]
    private readonly EventsSelectionState _eventsSelectionState;
    private List<BasicEventEditorData> _newEvents;
    private bool _canPasteToDifferentType;

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      EnvironmentTracksDefinitionSO.BasicEventTrackInfo track = this._beatmapDataModel.environmentTrackDefinition.visibleTrackInfos[this._basicEventsState.currentHoverVisibleTrackId];
      float beat = this._beatmapState.beat;
      float startBeat = this._eventsClipboardState.startBeat;
      List<BasicEventEditorData> list;
      if (this._eventsClipboardState.events.Count == 0)
      {
        (int value, float floatValue) = this._basicEventsState.GetSelectedBeatmapTypeValue(track.trackToolbarType);
        list = this._beatmapObjectsClipboardState.notes.Select<NoteEditorData, BasicEventEditorData>((Func<NoteEditorData, BasicEventEditorData>) (note => BasicEventEditorData.CreateNew(track.basicBeatmapEventType, note.beat, value, floatValue))).ToList<BasicEventEditorData>();
        this._canPasteToDifferentType = false;
      }
      else
      {
        list = this._eventsClipboardState.events.ToList<BasicEventEditorData>();
        this._canPasteToDifferentType = this._eventsClipboardState.allEventsSame;
      }
      this._newEvents = new List<BasicEventEditorData>();
      if (this._canPasteToDifferentType)
      {
        foreach (BasicEventEditorData basicEventEditorData in list)
          this._newEvents.Add(BasicEventEditorData.CreateNew(track.basicBeatmapEventType, beat + basicEventEditorData.beat - startBeat, basicEventEditorData.value, basicEventEditorData.floatValue));
      }
      else
      {
        foreach (BasicEventEditorData basicEventEditorData in list)
          this._newEvents.Add(BasicEventEditorData.CreateNew(basicEventEditorData.type, beat + basicEventEditorData.beat - startBeat, basicEventEditorData.value, basicEventEditorData.floatValue));
      }
      this._newEvents = this._newEvents.Where<BasicEventEditorData>((Func<BasicEventEditorData, bool>) (evt => this._beatmapEventsDataModel.GetBasicEventAt(evt.type, evt.beat) == (BasicEventEditorData) null)).ToList<BasicEventEditorData>();
      if (this._newEvents.Count == 0)
        return;
      this.shouldAddToHistory = true;
      this.Redo();
    }

    public void Undo()
    {
      this._beatmapEventsDataModel.Remove((IEnumerable<BasicEventEditorData>) this._newEvents);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public void Redo()
    {
      this._eventsSelectionState.Clear();
      this._beatmapEventsDataModel.Insert((IEnumerable<BasicEventEditorData>) this._newEvents);
      this._eventsSelectionState.AddRange(this._newEvents.Select<BasicEventEditorData, BeatmapEditorObjectId>((Func<BasicEventEditorData, BeatmapEditorObjectId>) (evt => evt.id)));
      this._signalBus.Fire<EventsSelectionStateUpdatedSignal>();
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public (CommandMessageType, string) GetMessage() => (CommandMessageType.Normal, string.Format("Pasted {0} events", (object) this._newEvents?.Count));
  }
}
