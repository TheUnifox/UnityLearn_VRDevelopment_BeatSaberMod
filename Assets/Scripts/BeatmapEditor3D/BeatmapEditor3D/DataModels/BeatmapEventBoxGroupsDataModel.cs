// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.BeatmapEventBoxGroupsDataModel
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using IntervalTree;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace BeatmapEditor3D.DataModels
{
  public class BeatmapEventBoxGroupsDataModel
  {
    [Inject]
    private readonly BeatmapDataModel _beatmapDataModel;
    private readonly IDictionary<(int, EventBoxGroupEditorData.EventBoxGroupType), IIntervalTree<float, EventBoxGroupEditorData>> _eventBoxGroupsByGroupIdAndType = (IDictionary<(int, EventBoxGroupEditorData.EventBoxGroupType), IIntervalTree<float, EventBoxGroupEditorData>>) new Dictionary<(int, EventBoxGroupEditorData.EventBoxGroupType), IIntervalTree<float, EventBoxGroupEditorData>>();
    private readonly IDictionary<BeatmapEditorObjectId, EventBoxGroupEditorData> _eventBoxGroupsById = (IDictionary<BeatmapEditorObjectId, EventBoxGroupEditorData>) new Dictionary<BeatmapEditorObjectId, EventBoxGroupEditorData>();
    private readonly IDictionary<BeatmapEditorObjectId, List<EventBoxEditorData>> _eventBoxesById = (IDictionary<BeatmapEditorObjectId, List<EventBoxEditorData>>) new Dictionary<BeatmapEditorObjectId, List<EventBoxEditorData>>();
    private readonly Dictionary<BeatmapEditorObjectId, BeatmapEditorObjectId> _eventBoxGroupIdByEventBoxId = new Dictionary<BeatmapEditorObjectId, BeatmapEditorObjectId>();
    private readonly IDictionary<BeatmapEditorObjectId, List<BaseEditorData>> _eventBoxBaseDataListsByEventBoxId = (IDictionary<BeatmapEditorObjectId, List<BaseEditorData>>) new Dictionary<BeatmapEditorObjectId, List<BaseEditorData>>();
    private readonly Dictionary<int, int> _groupIdToGroupSize = new Dictionary<int, int>();
    private bool _isDirty;
    private readonly object _updateDataLock = new object();

    public bool isDirty => this._isDirty;

    public event Action<EventBoxGroupEditorData> didUpdateEventBoxGroup;

    public void UpdateWith(
      List<BeatmapEditorEventBoxGroupInput> eventBoxGroups)
    {
      foreach (LightGroupSO lightGroupSo in this._beatmapDataModel.environment.lightGroups.lightGroupSOList)
        this._groupIdToGroupSize[lightGroupSo.groupId] = lightGroupSo.numberOfElements;
      this.InsertEventBoxGroupsWithData(eventBoxGroups);
    }

    public void ClearDirty() => this._isDirty = false;

    public void Clear()
    {
      foreach (KeyValuePair<BeatmapEditorObjectId, EventBoxGroupEditorData> keyValuePair in (IEnumerable<KeyValuePair<BeatmapEditorObjectId, EventBoxGroupEditorData>>) this._eventBoxGroupsById)
      {
        EventBoxGroupEditorData boxGroupEditorData = keyValuePair.Value;
      }
      this._eventBoxGroupsByGroupIdAndType.Clear();
      this._eventBoxGroupsById.Clear();
      this._eventBoxesById.Clear();
      this._eventBoxBaseDataListsByEventBoxId.Clear();
      this._eventBoxGroupIdByEventBoxId.Clear();
    }

    public List<EventBoxGroupEditorData> GetEventBoxGroupsInterval(
      int groupId,
      EventBoxGroupEditorData.EventBoxGroupType type,
      float from,
      float to)
    {
      IIntervalTree<float, EventBoxGroupEditorData> intervalTree;
      return this._eventBoxGroupsByGroupIdAndType.TryGetValue((groupId, type), out intervalTree) ? intervalTree.QueryWithCount(from, to) : new List<EventBoxGroupEditorData>();
    }

    public EventBoxGroupEditorData GetEventBoxGroupAt(
      int groupId,
      EventBoxGroupEditorData.EventBoxGroupType type,
      float time)
    {
      IIntervalTree<float, EventBoxGroupEditorData> intervalTree;
      return !this._eventBoxGroupsByGroupIdAndType.TryGetValue((groupId, type), out intervalTree) ? (EventBoxGroupEditorData) null : intervalTree.QueryWithCount(time - 0.00190624991f, time + 0.00190624991f).FirstOrDefault<EventBoxGroupEditorData>((Func<EventBoxGroupEditorData, bool>) (evt => evt.IsEventValidAtTime(time)));
    }

    public IEnumerable<EventBoxGroupEditorData> GetAllEventBoxGroupsAsList() => (IEnumerable<EventBoxGroupEditorData>) this._eventBoxGroupsByGroupIdAndType.Values.Aggregate<IIntervalTree<float, EventBoxGroupEditorData>, List<EventBoxGroupEditorData>>(new List<EventBoxGroupEditorData>(), (Func<List<EventBoxGroupEditorData>, IIntervalTree<float, EventBoxGroupEditorData>, List<EventBoxGroupEditorData>>) ((acc, tree) =>
    {
      acc.AddRange(tree.Values);
      return acc;
    }));

    public IEnumerable<BeatmapEditorEventBoxGroupInput> GetAllEventBoxGroups()
    {
      List<EventBoxGroupEditorData> boxGroupEditorDataList = this._eventBoxGroupsByGroupIdAndType.Values.Aggregate<IIntervalTree<float, EventBoxGroupEditorData>, List<EventBoxGroupEditorData>>(new List<EventBoxGroupEditorData>(), (Func<List<EventBoxGroupEditorData>, IIntervalTree<float, EventBoxGroupEditorData>, List<EventBoxGroupEditorData>>) ((acc, tree) =>
      {
        acc.AddRange(tree.Values);
        return acc;
      }));
      List<BeatmapEditorEventBoxGroupInput> allEventBoxGroups = new List<BeatmapEditorEventBoxGroupInput>();
      foreach (EventBoxGroupEditorData eventBoxGroup in boxGroupEditorDataList)
      {
        List<EventBoxEditorData> list;
        if (this._eventBoxesById.TryGetValue(eventBoxGroup.id, out list))
        {
          list = list.ToList<EventBoxEditorData>();
          allEventBoxGroups.Add(new BeatmapEditorEventBoxGroupInput(eventBoxGroup, list, list.Select<EventBoxEditorData, (BeatmapEditorObjectId, List<BaseEditorData>)>((Func<EventBoxEditorData, (BeatmapEditorObjectId, List<BaseEditorData>)>) (eventBox => (eventBox.id, this._eventBoxBaseDataListsByEventBoxId[eventBox.id]))).ToList<(BeatmapEditorObjectId, List<BaseEditorData>)>()));
        }
      }
      return (IEnumerable<BeatmapEditorEventBoxGroupInput>) allEventBoxGroups;
    }

    public EventBoxGroupEditorData GetEventBoxGroupById(BeatmapEditorObjectId eventBoxGroupId)
    {
      EventBoxGroupEditorData boxGroupEditorData;
      return !this._eventBoxGroupsById.TryGetValue(eventBoxGroupId, out boxGroupEditorData) ? (EventBoxGroupEditorData) null : boxGroupEditorData;
    }

    public List<EventBoxEditorData> GetEventBoxesByEventBoxGroupId(
      BeatmapEditorObjectId eventBoxGroupId)
    {
      lock (this._updateDataLock)
      {
        if (!this._eventBoxesById.ContainsKey(eventBoxGroupId))
          this._eventBoxesById[eventBoxGroupId] = new List<EventBoxEditorData>();
      }
      return this._eventBoxesById[eventBoxGroupId];
    }

    public List<TBaseEditorData> GetBaseEditorDataListByEventBoxId<TBaseEditorData>(
      BeatmapEditorObjectId eventBoxId)
      where TBaseEditorData : BaseEditorData
    {
      List<BaseEditorData> source;
      if (this._eventBoxBaseDataListsByEventBoxId.TryGetValue(eventBoxId, out source))
        return source.Cast<TBaseEditorData>().ToList<TBaseEditorData>();
      this._eventBoxBaseDataListsByEventBoxId[eventBoxId] = new List<BaseEditorData>();
      return this._eventBoxBaseDataListsByEventBoxId[eventBoxId].Cast<TBaseEditorData>().ToList<TBaseEditorData>();
    }

    public List<BaseEditorData> GetBaseEditorDataListByEventBoxId(BeatmapEditorObjectId eventBoxId)
    {
      lock (this._updateDataLock)
      {
        if (!this._eventBoxBaseDataListsByEventBoxId.ContainsKey(eventBoxId))
          this._eventBoxBaseDataListsByEventBoxId[eventBoxId] = new List<BaseEditorData>();
      }
      return this._eventBoxBaseDataListsByEventBoxId[eventBoxId];
    }

    public (BaseEditorData, int) GetBaseEditorDataById(
      BeatmapEditorObjectId eventBoxId,
      BeatmapEditorObjectId id)
    {
      List<BaseEditorData> listByEventBoxId = this.GetBaseEditorDataListByEventBoxId(eventBoxId);
      int index = listByEventBoxId.FindIndex((Predicate<BaseEditorData>) (e => e.id == id));
      return index != -1 ? (listByEventBoxId[index], index) : ((BaseEditorData) null, -1);
    }

    public (TBaseEditorData, int) GetBaseEditorDataById<TBaseEditorData>(
      BeatmapEditorObjectId eventBoxId,
      BeatmapEditorObjectId id)
      where TBaseEditorData : BaseEditorData
    {
      List<BaseEditorData> listByEventBoxId = this.GetBaseEditorDataListByEventBoxId(eventBoxId);
      int index = listByEventBoxId.FindIndex((Predicate<BaseEditorData>) (e => e.id == id));
      return index == -1 || !(listByEventBoxId[index] is TBaseEditorData) ? (default (TBaseEditorData), -1) : ((TBaseEditorData) listByEventBoxId[index], index);
    }

    public BaseEditorData GetBaseEditorDataAt(BeatmapEditorObjectId eventBoxId, float beat) => this.GetBaseEditorDataListByEventBoxId(eventBoxId).FirstOrDefault<BaseEditorData>((Func<BaseEditorData, bool>) (data => AudioTimeHelper.IsBeatSame(data.beat, beat)));

    public bool TryGetGroupSizeByEventBoxGroupId(int groupId, out int groupSize) => this._groupIdToGroupSize.TryGetValue(groupId, out groupSize);

    public void InsertEventBoxGroupsWithData(
      List<BeatmapEditorEventBoxGroupInput> eventBoxGroupInputs)
    {
      lock (this._updateDataLock)
      {
        foreach (BeatmapEditorEventBoxGroupInput eventBoxGroupInput in eventBoxGroupInputs)
        {
          EventBoxGroupEditorData eventBoxGroup = eventBoxGroupInput.eventBoxGroup;
          List<EventBoxEditorData> eventBoxes = eventBoxGroupInput.eventBoxes;
          List<(BeatmapEditorObjectId, List<BaseEditorData>)> baseLists = eventBoxGroupInput.baseLists;
          this.GetEventBoxGroupTree(eventBoxGroup.groupId, eventBoxGroup.type).Add(eventBoxGroup.beat, eventBoxGroup.beat, eventBoxGroup);
          this._eventBoxGroupsById[eventBoxGroup.id] = eventBoxGroup;
          this._eventBoxesById[eventBoxGroup.id] = eventBoxes;
          foreach (EventBoxEditorData eventBoxEditorData in eventBoxes)
            this._eventBoxGroupIdByEventBoxId[eventBoxEditorData.id] = eventBoxGroup.id;
          foreach ((BeatmapEditorObjectId, List<BaseEditorData>) tuple in baseLists)
            this._eventBoxBaseDataListsByEventBoxId[tuple.Item1] = tuple.Item2;
          Action<EventBoxGroupEditorData> updateEventBoxGroup = this.didUpdateEventBoxGroup;
          if (updateEventBoxGroup != null)
            updateEventBoxGroup(eventBoxGroup);
        }
      }
    }

    public void RemoveEventBoxGroupsWithData(
      List<BeatmapEditorEventBoxGroupInput> eventBoxGroupInputs)
    {
      lock (this._updateDataLock)
      {
        foreach (BeatmapEditorEventBoxGroupInput eventBoxGroupInput in eventBoxGroupInputs)
        {
          EventBoxGroupEditorData eventBoxGroup = eventBoxGroupInput.eventBoxGroup;
          List<EventBoxEditorData> eventBoxes = eventBoxGroupInput.eventBoxes;
          List<(BeatmapEditorObjectId, List<BaseEditorData>)> baseLists = eventBoxGroupInput.baseLists;
          this.GetEventBoxGroupTree(eventBoxGroup.groupId, eventBoxGroup.type).Remove(eventBoxGroup);
          this._eventBoxGroupsById.Remove(eventBoxGroup.id);
          this._eventBoxesById.Remove(eventBoxGroup.id);
          foreach (EventBoxEditorData eventBoxEditorData in eventBoxes)
            this._eventBoxGroupIdByEventBoxId.Remove(eventBoxEditorData.id);
          foreach ((BeatmapEditorObjectId, List<BaseEditorData>) tuple in baseLists)
            this._eventBoxBaseDataListsByEventBoxId.Remove(tuple.Item1);
          Action<EventBoxGroupEditorData> updateEventBoxGroup = this.didUpdateEventBoxGroup;
          if (updateEventBoxGroup != null)
            updateEventBoxGroup(eventBoxGroup);
        }
      }
    }

    public void InsertEventBoxGroup(EventBoxGroupEditorData eventBoxGroupToAdd)
    {
      if (!(this.GetEventBoxGroupAt(eventBoxGroupToAdd.groupId, eventBoxGroupToAdd.type, eventBoxGroupToAdd.beat) == (EventBoxGroupEditorData) null))
        return;
      lock (this._updateDataLock)
      {
        this.GetEventBoxGroupTree(eventBoxGroupToAdd.groupId, eventBoxGroupToAdd.type).Add(eventBoxGroupToAdd.beat, eventBoxGroupToAdd.beat, eventBoxGroupToAdd);
        this._eventBoxGroupsById[eventBoxGroupToAdd.id] = eventBoxGroupToAdd;
        Action<EventBoxGroupEditorData> updateEventBoxGroup = this.didUpdateEventBoxGroup;
        if (updateEventBoxGroup != null)
          updateEventBoxGroup(eventBoxGroupToAdd);
        this._isDirty = true;
      }
    }

    public void InsertEventBoxGroups(
      IEnumerable<EventBoxGroupEditorData> eventBoxGroupsToAdd)
    {
      foreach (EventBoxGroupEditorData eventBoxGroupToAdd in eventBoxGroupsToAdd)
        this.InsertEventBoxGroup(eventBoxGroupToAdd);
    }

    public void InsertEventBox(
      BeatmapEditorObjectId eventBoxGroupId,
      EventBoxEditorData eventBoxEditorData,
      int index = -1)
    {
      EventBoxGroupEditorData eventBoxGroupById = this.GetEventBoxGroupById(eventBoxGroupId);
      lock (this._updateDataLock)
      {
        List<EventBoxEditorData> byEventBoxGroupId = this.GetEventBoxesByEventBoxGroupId(eventBoxGroupId);
        if (index == -1)
          byEventBoxGroupId.Add(eventBoxEditorData);
        else
          byEventBoxGroupId.Insert(index, eventBoxEditorData);
        this._eventBoxGroupIdByEventBoxId[eventBoxEditorData.id] = eventBoxGroupById.id;
      }
      Action<EventBoxGroupEditorData> updateEventBoxGroup = this.didUpdateEventBoxGroup;
      if (updateEventBoxGroup != null)
        updateEventBoxGroup(eventBoxGroupById);
      this._isDirty = true;
    }

    public void InsertEventBoxes(
      BeatmapEditorObjectId eventBoxGroupId,
      IEnumerable<EventBoxEditorData> eventBoxEditorData)
    {
      foreach (EventBoxEditorData eventBoxEditorData1 in eventBoxEditorData)
        this.InsertEventBox(eventBoxGroupId, eventBoxEditorData1);
    }

    public void InsertBaseEditorData(
      BeatmapEditorObjectId eventBoxId,
      BaseEditorData data,
      int index = -1)
    {
      List<BaseEditorData> listByEventBoxId = this.GetBaseEditorDataListByEventBoxId(eventBoxId);
      lock (this._updateDataLock)
      {
        if (index == -1)
          listByEventBoxId.Add(data);
        else
          listByEventBoxId.Insert(index, data);
      }
      Action<EventBoxGroupEditorData> updateEventBoxGroup = this.didUpdateEventBoxGroup;
      if (updateEventBoxGroup != null)
        updateEventBoxGroup(this._eventBoxGroupsById[this._eventBoxGroupIdByEventBoxId[eventBoxId]]);
      this._isDirty = true;
    }

    public void InsertBaseEditorDataAtBeat(BeatmapEditorObjectId eventBoxId, BaseEditorData data)
    {
      List<BaseEditorData> listByEventBoxId = this.GetBaseEditorDataListByEventBoxId(eventBoxId);
      int index = listByEventBoxId.FindIndex((Predicate<BaseEditorData>) (e => (double) e.beat > (double) data.beat));
      lock (this._updateDataLock)
      {
        if (index == -1)
        {
          listByEventBoxId.Add(data);
        }
        else
        {
          if (index > 0 && AudioTimeHelper.IsBeatSame(listByEventBoxId[index - 1].beat, data.beat))
            return;
          listByEventBoxId.Insert(index, data);
        }
      }
      Action<EventBoxGroupEditorData> updateEventBoxGroup = this.didUpdateEventBoxGroup;
      if (updateEventBoxGroup != null)
        updateEventBoxGroup(this._eventBoxGroupsById[this._eventBoxGroupIdByEventBoxId[eventBoxId]]);
      this._isDirty = true;
    }

    public void InsertBaseEditorDataList(
      BeatmapEditorObjectId eventBoxId,
      IEnumerable<BaseEditorData> baseDataList)
    {
      lock (this._updateDataLock)
        this._eventBoxBaseDataListsByEventBoxId[eventBoxId] = baseDataList.ToList<BaseEditorData>();
      Action<EventBoxGroupEditorData> updateEventBoxGroup = this.didUpdateEventBoxGroup;
      if (updateEventBoxGroup != null)
        updateEventBoxGroup(this._eventBoxGroupsById[this._eventBoxGroupIdByEventBoxId[eventBoxId]]);
      this._isDirty = true;
    }

    public void RemoveEventBoxGroup(EventBoxGroupEditorData eventBoxGroupToRemove)
    {
      EventBoxGroupEditorData boxGroupEditorData = this._eventBoxGroupsById[eventBoxGroupToRemove.id];
      this.GetEventBoxesByEventBoxGroupId(boxGroupEditorData.id);
      IIntervalTree<float, EventBoxGroupEditorData> eventBoxGroupTree = this.GetEventBoxGroupTree(boxGroupEditorData.groupId, boxGroupEditorData.type);
      lock (this._updateDataLock)
      {
        eventBoxGroupTree.Remove(boxGroupEditorData);
        this._eventBoxGroupsById.Remove(boxGroupEditorData.id);
      }
      Action<EventBoxGroupEditorData> updateEventBoxGroup = this.didUpdateEventBoxGroup;
      if (updateEventBoxGroup != null)
        updateEventBoxGroup(boxGroupEditorData);
      this._isDirty = true;
    }

    public void RemoveEventBoxGroups(
      IEnumerable<EventBoxGroupEditorData> eventBoxGroupsToRemove)
    {
      foreach (EventBoxGroupEditorData eventBoxGroupToRemove in eventBoxGroupsToRemove)
        this.RemoveEventBoxGroup(eventBoxGroupToRemove);
    }

    public void RemoveEventBox(
      BeatmapEditorObjectId eventBoxGroupId,
      EventBoxEditorData eventBoxEditorData)
    {
      this.GetEventBoxGroupById(eventBoxGroupId);
      List<EventBoxEditorData> byEventBoxGroupId = this.GetEventBoxesByEventBoxGroupId(eventBoxGroupId);
      if (byEventBoxGroupId == null)
        return;
      EventBoxEditorData eventBoxEditorData1 = byEventBoxGroupId.Find((Predicate<EventBoxEditorData>) (eventBox => eventBox.Equals((object) eventBoxEditorData)));
      lock (this._updateDataLock)
      {
        if (eventBoxEditorData1 != null)
          byEventBoxGroupId.Remove(eventBoxEditorData1);
        this._eventBoxGroupIdByEventBoxId.Remove(eventBoxEditorData.id);
        Action<EventBoxGroupEditorData> updateEventBoxGroup = this.didUpdateEventBoxGroup;
        if (updateEventBoxGroup != null)
          updateEventBoxGroup(this._eventBoxGroupsById[eventBoxGroupId]);
      }
      this._isDirty = true;
    }

    public void RemoveEventBoxes(
      BeatmapEditorObjectId eventBoxGroupId,
      IEnumerable<EventBoxEditorData> eventBoxEditorDataList)
    {
      foreach (EventBoxEditorData eventBoxEditorData in eventBoxEditorDataList)
        this.RemoveEventBox(eventBoxGroupId, eventBoxEditorData);
    }

    public void RemoveBaseEditorData(
      BeatmapEditorObjectId eventBoxId,
      BaseEditorData baseEditorData,
      int index = -1)
    {
      List<BaseEditorData> listByEventBoxId = this.GetBaseEditorDataListByEventBoxId(eventBoxId);
      if (index == -1)
        index = listByEventBoxId.FindIndex((Predicate<BaseEditorData>) (e => e.id == baseEditorData.id));
      lock (this._updateDataLock)
        listByEventBoxId.RemoveAt(index);
      Action<EventBoxGroupEditorData> updateEventBoxGroup = this.didUpdateEventBoxGroup;
      if (updateEventBoxGroup != null)
        updateEventBoxGroup(this._eventBoxGroupsById[this._eventBoxGroupIdByEventBoxId[eventBoxId]]);
      this._isDirty = true;
    }

    public void RemoveBaseEditorDataList(BeatmapEditorObjectId eventBoxId)
    {
      if (!this._eventBoxBaseDataListsByEventBoxId.ContainsKey(eventBoxId))
        return;
      lock (this._updateDataLock)
        this._eventBoxBaseDataListsByEventBoxId.Remove(eventBoxId);
      Action<EventBoxGroupEditorData> updateEventBoxGroup = this.didUpdateEventBoxGroup;
      if (updateEventBoxGroup != null)
        updateEventBoxGroup(this._eventBoxGroupsById[this._eventBoxGroupIdByEventBoxId[eventBoxId]]);
      this._isDirty = true;
    }

    private IIntervalTree<float, EventBoxGroupEditorData> GetEventBoxGroupTree(
      int groupId,
      EventBoxGroupEditorData.EventBoxGroupType type)
    {
      if (!this._eventBoxGroupsByGroupIdAndType.ContainsKey((groupId, type)))
        this._eventBoxGroupsByGroupIdAndType[(groupId, type)] = (IIntervalTree<float, EventBoxGroupEditorData>) new IntervalTree.IntervalTree<float, EventBoxGroupEditorData>();
      return this._eventBoxGroupsByGroupIdAndType[(groupId, type)];
    }
  }
}
