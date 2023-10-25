// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.EventBoxGroupsClipboardState
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System.Collections.Generic;

namespace BeatmapEditor3D
{
  public class EventBoxGroupsClipboardState
  {
    private readonly BeatmapEditorObjectsCollection<EventBoxGroupEditorData> _eventBoxGroups = new BeatmapEditorObjectsCollection<EventBoxGroupEditorData>();
    private readonly Dictionary<BeatmapEditorObjectId, List<EventBoxEditorData>> _eventBoxes = new Dictionary<BeatmapEditorObjectId, List<EventBoxEditorData>>();
    private readonly Dictionary<BeatmapEditorObjectId, List<BaseEditorData>> _baseEditorDataLists = new Dictionary<BeatmapEditorObjectId, List<BaseEditorData>>();

    public float startBeat { get; set; }

    public EventBoxGroupEditorData.EventBoxGroupType eventBoxGroupType { get; set; }

    public IReadOnlyList<EventBoxGroupEditorData> eventBoxGroups => this._eventBoxGroups.items;

    public IReadOnlyDictionary<BeatmapEditorObjectId, List<EventBoxEditorData>> eventBoxes => (IReadOnlyDictionary<BeatmapEditorObjectId, List<EventBoxEditorData>>) this._eventBoxes;

    public IReadOnlyDictionary<BeatmapEditorObjectId, List<BaseEditorData>> baseEditorDataLists => (IReadOnlyDictionary<BeatmapEditorObjectId, List<BaseEditorData>>) this._baseEditorDataLists;

    public void AddRange(IEnumerable<EventBoxGroupEditorData> items) => this._eventBoxGroups.AddRange(items);

    public void AddRange(
      BeatmapEditorObjectId eventBoxGroupId,
      IEnumerable<EventBoxEditorData> eventBoxes)
    {
      List<EventBoxEditorData> eventBoxEditorDataList;
      this._eventBoxes.TryGetValue(eventBoxGroupId, out eventBoxEditorDataList);
      if (eventBoxEditorDataList == null)
      {
        eventBoxEditorDataList = new List<EventBoxEditorData>();
        this._eventBoxes[eventBoxGroupId] = eventBoxEditorDataList;
      }
      foreach (EventBoxEditorData eventBox in eventBoxes)
        eventBoxEditorDataList.Add(eventBox);
    }

    public void AddRange(BeatmapEditorObjectId eventBoxId, IEnumerable<BaseEditorData> baseList)
    {
      List<BaseEditorData> baseEditorDataList;
      this._baseEditorDataLists.TryGetValue(eventBoxId, out baseEditorDataList);
      if (baseEditorDataList == null)
      {
        baseEditorDataList = new List<BaseEditorData>();
        this._baseEditorDataLists[eventBoxId] = baseEditorDataList;
      }
      foreach (BaseEditorData baseEditorData in baseList)
        baseEditorDataList.Add(baseEditorData);
    }

    public void Clear()
    {
      this._eventBoxGroups.Clear();
      this._eventBoxes.Clear();
      this._baseEditorDataLists.Clear();
    }
  }
}
