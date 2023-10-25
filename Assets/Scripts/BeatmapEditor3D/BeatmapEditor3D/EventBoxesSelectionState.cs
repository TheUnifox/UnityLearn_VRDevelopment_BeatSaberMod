// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.EventBoxesSelectionState
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
  public class EventBoxesSelectionState : IInitializable, IDisposable
  {
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;
    [Inject]
    private readonly SignalBus _signalBus;
    private readonly BeatmapEditorObjectsCollection<(BeatmapEditorObjectId eventBoxId, BeatmapEditorObjectId eventId)> _events = new BeatmapEditorObjectsCollection<(BeatmapEditorObjectId, BeatmapEditorObjectId)>();

    public IReadOnlyList<(BeatmapEditorObjectId eventBoxId, BeatmapEditorObjectId eventId)> events => this._events.items;

    public bool showSelection { get; set; }

    public EventBoxGroupEditorData.EventBoxGroupType eventBoxGroupType { get; set; }

    public int startEventBoxIndex { get; set; }

    public float startBeat { get; set; }

    public int endEventBoxIndex { get; set; }

    public float endBeat { get; set; }

    public int tempStartEventBoxIndex { get; set; }

    public float tempStartBeat { get; set; }

    public int tempEndEventBoxIndex { get; set; }

    public float tempEndBeat { get; set; }

    public void Initialize() => this._signalBus.Subscribe<BeatmapLevelUpdatedSignal>(new Action(this.HandleBeatmapLevelUpdated));

    public void Dispose() => this._signalBus.TryUnsubscribe<BeatmapLevelUpdatedSignal>(new Action(this.HandleBeatmapLevelUpdated));

    public void AddOrRemoveIfSelected(
      BeatmapEditorObjectId eventBoxId,
      BeatmapEditorObjectId eventId)
    {
      if (this.IsSelected(eventBoxId, eventId))
        this._events.Remove((eventBoxId, eventId));
      else
        this._events.Add((eventBoxId, eventId));
    }

    public void Add(BeatmapEditorObjectId eventBoxId, BeatmapEditorObjectId eventId) => this._events.Add((eventBoxId, eventId));

    public void AddRange(
      IEnumerable<(BeatmapEditorObjectId eventBoxId, BeatmapEditorObjectId eventId)> items)
    {
      this._events.AddRange(items);
    }

    public void Remove(BeatmapEditorObjectId eventBoxId, BeatmapEditorObjectId eventId) => this._events.Remove((eventBoxId, eventId));

    public void Clear() => this._events.Clear();

    public bool IsSelected(BeatmapEditorObjectId eventBoxId, BeatmapEditorObjectId eventId) => this._events.Contains((eventBoxId, eventId));

    public bool IsAnythingSelected() => this._events.count > 0;

    private void HandleBeatmapLevelUpdated()
    {
      if (!this.IsAnythingSelected())
        return;
      bool flag = false;
      foreach ((BeatmapEditorObjectId eventBoxId, BeatmapEditorObjectId eventId) tuple in this._events.items.ToList<(BeatmapEditorObjectId, BeatmapEditorObjectId)>())
      {
        if (this._beatmapEventBoxGroupsDataModel.GetBaseEditorDataById(tuple.eventBoxId, tuple.eventId).Item1 == null)
        {
          this.Remove(tuple.eventBoxId, tuple.eventId);
          flag = true;
        }
      }
      if (!flag)
        return;
      this._signalBus.Fire<EventBoxesSelectionStateUpdatedSignal>();
    }
  }
}
