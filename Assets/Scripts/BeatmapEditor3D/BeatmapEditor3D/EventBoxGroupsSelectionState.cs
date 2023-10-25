// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.EventBoxGroupsSelectionState
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
  public class EventBoxGroupsSelectionState : IInitializable, IDisposable
  {
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;
    [Inject]
    private readonly SignalBus _signalBus;
    private readonly BeatmapEditorObjectsCollection<BeatmapEditorObjectId> _eventBoxGroups = new BeatmapEditorObjectsCollection<BeatmapEditorObjectId>();

    public IReadOnlyList<BeatmapEditorObjectId> eventBoxGroups => this._eventBoxGroups.items;

    public BeatmapEditorObjectsCollection<BeatmapEditorObjectId> eventBoxGroupsCollection => this._eventBoxGroups;

    public bool showSelection { get; set; }

    public int startPageIndex { get; set; }

    public float startBeat { get; set; }

    public int endPageIndex { get; set; }

    public float endBeat { get; set; }

    public int tempStartPageIndex { get; set; }

    public float tempStartBeat { get; set; }

    public int tempEndPageIndex { get; set; }

    public float tempEndBeat { get; set; }

    public void Initialize() => this._signalBus.Subscribe<BeatmapLevelUpdatedSignal>(new Action(this.HandleBeatmapLevelUpdated));

    public void Dispose() => this._signalBus.TryUnsubscribe<BeatmapLevelUpdatedSignal>(new Action(this.HandleBeatmapLevelUpdated));

    public void AddOrRemoveIfSelected(BeatmapEditorObjectId item)
    {
      if (this.IsSelected(item))
        this._eventBoxGroups.Remove(item);
      else
        this._eventBoxGroups.Add(item);
    }

    public void Add(BeatmapEditorObjectId item) => this._eventBoxGroups.Add(item);

    public void AddRange(IEnumerable<BeatmapEditorObjectId> items) => this._eventBoxGroups.AddRange(items);

    public void Remove(BeatmapEditorObjectId item) => this._eventBoxGroups.Remove(item);

    public void Clear() => this._eventBoxGroups.Clear();

    public bool IsSelected(BeatmapEditorObjectId item) => this._eventBoxGroups.Contains(item);

    public bool IsAnythingSelected() => this._eventBoxGroups.count > 0;

    private void HandleBeatmapLevelUpdated()
    {
      if (!this.IsAnythingSelected())
        return;
      bool flag = false;
      foreach (BeatmapEditorObjectId eventBoxGroupId in this._eventBoxGroups.items.ToList<BeatmapEditorObjectId>())
      {
        if (this._beatmapEventBoxGroupsDataModel.GetEventBoxGroupById(eventBoxGroupId) == (EventBoxGroupEditorData) null)
        {
          this.Remove(eventBoxGroupId);
          flag = true;
        }
      }
      if (!flag)
        return;
      this._signalBus.Fire<EventBoxGroupsSelectionStateUpdatedSignal>();
    }
  }
}
