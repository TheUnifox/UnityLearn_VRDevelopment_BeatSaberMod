// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DeleteSelectedEventsCommand
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
  public class DeleteSelectedEventsCommand : 
    IBeatmapEditorCommandWithHistory,
    IBeatmapEditorCommand,
    IBeatmapEditorCommandMessageProvider
  {
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly IBeatmapEventsDataModel _beatmapEventsDataModel;
    [Inject]
    private readonly EventsSelectionState _eventsSelectionState;
    private List<BasicEventEditorData> _selectedEvents;

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      this._selectedEvents = this._eventsSelectionState.events.Select<BeatmapEditorObjectId, BasicEventEditorData>((Func<BeatmapEditorObjectId, BasicEventEditorData>) (eventId => this._beatmapEventsDataModel.GetBasicEventById(eventId))).ToList<BasicEventEditorData>();
      this.shouldAddToHistory = this._selectedEvents.Count > 0;
      if (!this.shouldAddToHistory)
        return;
      this._eventsSelectionState.Clear();
      this._signalBus.Fire<EventsSelectionStateUpdatedSignal>();
      this.Redo();
    }

    public void Undo()
    {
      this._beatmapEventsDataModel.Insert((IEnumerable<BasicEventEditorData>) this._selectedEvents);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public void Redo()
    {
      this._beatmapEventsDataModel.Remove((IEnumerable<BasicEventEditorData>) this._selectedEvents);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public (CommandMessageType, string) GetMessage() => (CommandMessageType.Normal, string.Format("Delete {0} events", (object) this._selectedEvents?.Count));
  }
}
