// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.CopyEventsCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System;
using System.Linq;
using Zenject;

namespace BeatmapEditor3D
{
  public class CopyEventsCommand : IBeatmapEditorCommand, IBeatmapEditorCommandMessageProvider
  {
    [Inject]
    private readonly EventsSelectionState _eventsSelectionState;
    [Inject]
    private readonly EventsClipboardState _eventsClipboardState;
    [Inject]
    private readonly IReadonlyBeatmapEventsDataModel _beatmapEventsDataModel;
    [Inject]
    private readonly SignalBus _signalBus;

    public void Execute()
    {
      this._eventsClipboardState.Clear();
      this._eventsClipboardState.AddRange(this._eventsSelectionState.events.Select<BeatmapEditorObjectId, BasicEventEditorData>((Func<BeatmapEditorObjectId, BasicEventEditorData>) (eventId => this._beatmapEventsDataModel.GetBasicEventById(eventId))));
      this._eventsClipboardState.startBeat = this._eventsSelectionState.startBeat;
      this._eventsClipboardState.allEventsSame = this._eventsSelectionState.allEventsSame;
      this._signalBus.Fire<EventsClipboardStateUpdatedSignal>();
    }

    public (CommandMessageType, string) GetMessage() => (CommandMessageType.Normal, string.Format("Copied {0} events", (object) this._eventsSelectionState.events?.Count));
  }
}
