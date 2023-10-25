// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.EventsSelectionState
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
  public class EventsSelectionState : IReadonlyEventsSelectionState, IInitializable, IDisposable
  {
    [Inject]
    private readonly BeatmapBasicEventsDataModel _basicEventsDataModel;
    [Inject]
    private readonly SignalBus _signalBus;
    private readonly BeatmapEditorObjectsCollection<BeatmapEditorObjectId> _events = new BeatmapEditorObjectsCollection<BeatmapEditorObjectId>();

    public IReadOnlyList<BeatmapEditorObjectId> events => this._events.items;

    public BeatmapEditorObjectsCollection<BeatmapEditorObjectId> eventsCollection => this._events;

    public bool showSelection { get; set; }

    public int startTrackIndex { get; set; }

    public float startBeat { get; set; }

    public int endTrackIndex { get; set; }

    public float endBeat { get; set; }

    public int tempStartTrackIndex { get; set; }

    public float tempStartBeat { get; set; }

    public int tempEndTrackIndex { get; set; }

    public float tempEndBeat { get; set; }

    public bool allEventsSame { get; set; }

    public BasicEventEditorData draggedBasicEventData { get; set; }

    public void Initialize() => this._signalBus.Subscribe<BeatmapLevelUpdatedSignal>(new Action(this.HandleBeatmapLevelUpdated));

    public void Dispose() => this._signalBus.TryUnsubscribe<BeatmapLevelUpdatedSignal>(new Action(this.HandleBeatmapLevelUpdated));

    public void Add(BeatmapEditorObjectId item) => this._events.Add(item);

    public void AddRange(IEnumerable<BeatmapEditorObjectId> items) => this._events.AddRange(items);

    public void Remove(BeatmapEditorObjectId item) => this._events.Remove(item);

    public void Clear() => this._events.Clear();

    public bool IsSelected(BeatmapEditorObjectId item) => this._events.Contains(item);

    public bool IsAnythingSelected() => this._events.count > 0;

    private void HandleBeatmapLevelUpdated()
    {
      if (!this.IsAnythingSelected())
        return;
      bool flag = false;
      foreach (BeatmapEditorObjectId id in this._events.items.ToList<BeatmapEditorObjectId>())
      {
        if (this._basicEventsDataModel.GetBasicEventById(id) == (BasicEventEditorData) null)
        {
          this.Remove(id);
          flag = true;
        }
      }
      if (!flag)
        return;
      this._signalBus.Fire<EventBoxGroupsSelectionStateUpdatedSignal>();
    }
  }
}
