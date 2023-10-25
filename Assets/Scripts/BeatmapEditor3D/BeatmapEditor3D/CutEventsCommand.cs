// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.CutEventsCommand
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
  public class CutEventsCommand : 
    IBeatmapEditorCommandWithHistory,
    IBeatmapEditorCommand,
    IBeatmapEditorCommandMessageProvider
  {
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly IBeatmapEventsDataModel _beatmapEventsDataModel;
    [Inject]
    private readonly EventsClipboardState _eventsClipboardState;
    [Inject]
    private readonly EventsSelectionState _eventsSelectionState;
    private List<BasicEventEditorData> _clipboardEvents;

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      if (this._eventsSelectionState.events.Count == 0)
        return;
      this._eventsClipboardState.Clear();
      this._eventsClipboardState.AddRange(this._eventsSelectionState.events.Select<BeatmapEditorObjectId, BasicEventEditorData>((Func<BeatmapEditorObjectId, BasicEventEditorData>) (eventId => this._beatmapEventsDataModel.GetBasicEventById(eventId))));
      this._eventsClipboardState.startBeat = this._eventsSelectionState.startBeat;
      this._eventsClipboardState.allEventsSame = this._eventsSelectionState.allEventsSame;
      this._eventsSelectionState.Clear();
      this._clipboardEvents = this._eventsClipboardState.events.ToList<BasicEventEditorData>();
      this._signalBus.Fire<EventsSelectionStateUpdatedSignal>();
      this._signalBus.Fire<EventsClipboardStateUpdatedSignal>();
      this.shouldAddToHistory = true;
      this.Redo();
    }

    public void Undo()
    {
      this._beatmapEventsDataModel.Insert((IEnumerable<BasicEventEditorData>) this._clipboardEvents);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public void Redo()
    {
      this._beatmapEventsDataModel.Remove((IEnumerable<BasicEventEditorData>) this._clipboardEvents);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public (CommandMessageType, string) GetMessage() => (CommandMessageType.Normal, string.Format("Cut {0} events", (object) this._clipboardEvents?.Count));
  }
}
