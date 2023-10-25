// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.BeatmapLivePreviewDataModel
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels.Events.Conversion;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.DataModels
{
  public class BeatmapLivePreviewDataModel : IInitializable, ITickable, IDisposable
  {
    private const int kMaxUpdateQueueSize = 20;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly BeatmapBasicEventsDataModel _beatmapBasicEventsDataModel;
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;
    [Inject]
    private readonly BeatmapData _livePreviewBeatmapData;
    private BpmDataBeatToTimeConvertor _timeConvertor;
    private readonly QueueSet<EventBoxGroupEditorData> _livePreviewEventGroupUpdateQueue = new QueueSet<EventBoxGroupEditorData>();
    private readonly IDictionary<BeatmapEditorObjectId, BeatmapEventData> _livePreviewEvents = (IDictionary<BeatmapEditorObjectId, BeatmapEventData>) new Dictionary<BeatmapEditorObjectId, BeatmapEventData>();
    private readonly Dictionary<BeatmapEditorObjectId, LinkedListNode<BeatmapEventDataBoxGroup>> _eventBoxGroupListNodeById = new Dictionary<BeatmapEditorObjectId, LinkedListNode<BeatmapEventDataBoxGroup>>();
    private BeatmapEventDataBoxGroupLists _beatmapEventDataBoxGroupLists;
    private bool _prevUpdateDataOnInsert;

    public void Initialize()
    {
      this._signalBus.Subscribe<BeatmapProjectManagerSignals.BeatmapLevelDataModelLoaded>(new Action(this.HandleBeatmapLevelDataModelLoaded));
      this._signalBus.Subscribe<BeatmapProjectManagerSignals.BeatmapLevelWillCloseSignal>(new Action(this.HandleBeatmapLevelWillClose));
    }

    public void Dispose()
    {
      this._beatmapBasicEventsDataModel.didAddBasicEvent -= new Action<BasicEventEditorData>(this.HandleBeatmapBasicEventsDataModelDidAddBasicEvent);
      this._beatmapBasicEventsDataModel.didRemoveBasicEvent -= new Action<BasicEventEditorData>(this.HandleBeatmapBasicEventsDAtaModelDidRemoveBasicEvent);
      this._beatmapEventBoxGroupsDataModel.didUpdateEventBoxGroup -= new Action<EventBoxGroupEditorData>(this.HandleBeatmapEventBoxGroupDataModelDidUpdateEventBoxGroup);
      this._signalBus.TryUnsubscribe<BeatmapProjectManagerSignals.BeatmapLevelDataModelLoaded>(new Action(this.HandleBeatmapLevelDataModelLoaded));
      this._signalBus.TryUnsubscribe<BeatmapProjectManagerSignals.BeatmapLevelWillCloseSignal>(new Action(this.HandleBeatmapLevelWillClose));
    }

    private void HandleBeatmapBasicEventsDataModelDidAddBasicEvent(BasicEventEditorData evt) => this.AddLivePreviewBasicEvent(evt);

    private void HandleBeatmapBasicEventsDAtaModelDidRemoveBasicEvent(BasicEventEditorData evt) => this.RemoveLivePreviewBasicEvent(evt);

    private void HandleBeatmapEventBoxGroupDataModelDidUpdateEventBoxGroup(EventBoxGroupEditorData e) => this._livePreviewEventGroupUpdateQueue.Enqueue(e);

    private void HandleBeatmapLevelDataModelLoaded()
    {
      this._prevUpdateDataOnInsert = false;
      this._livePreviewBeatmapData.updateAllBeatmapDataOnInsert = false;
      this._timeConvertor = new BpmDataBeatToTimeConvertor();
      this._beatmapEventDataBoxGroupLists = new BeatmapEventDataBoxGroupLists(this._livePreviewBeatmapData, (IBeatToTimeConvertor) this._timeConvertor, false);
      foreach (BasicEventEditorData allEventsAs in this._beatmapBasicEventsDataModel.GetAllEventsAsList())
        this.AddLivePreviewBasicEvent(allEventsAs);
      foreach (BasicEventEditorData evt in this._beatmapBasicEventsDataModel.GetAllDataIn(BasicBeatmapEventType.Event5))
        this.AddLivePreviewBasicEvent(evt);
      foreach (EventBoxGroupEditorData eventBoxGroupsAs in this._beatmapEventBoxGroupsDataModel.GetAllEventBoxGroupsAsList())
        this.UpdateLivePreviewEventBoxes(eventBoxGroupsAs, false);
      this._beatmapEventDataBoxGroupLists.SyncWithBeatmapData();
      this._livePreviewBeatmapData.ProcessAndSortBeatmapData();
      this._livePreviewBeatmapData.updateAllBeatmapDataOnInsert = true;
      this._beatmapBasicEventsDataModel.didAddBasicEvent += new Action<BasicEventEditorData>(this.HandleBeatmapBasicEventsDataModelDidAddBasicEvent);
      this._beatmapBasicEventsDataModel.didRemoveBasicEvent += new Action<BasicEventEditorData>(this.HandleBeatmapBasicEventsDAtaModelDidRemoveBasicEvent);
      this._beatmapEventBoxGroupsDataModel.didUpdateEventBoxGroup += new Action<EventBoxGroupEditorData>(this.HandleBeatmapEventBoxGroupDataModelDidUpdateEventBoxGroup);
    }

    private void HandleBeatmapLevelWillClose()
    {
      this._beatmapBasicEventsDataModel.didAddBasicEvent -= new Action<BasicEventEditorData>(this.HandleBeatmapBasicEventsDataModelDidAddBasicEvent);
      this._beatmapBasicEventsDataModel.didRemoveBasicEvent -= new Action<BasicEventEditorData>(this.HandleBeatmapBasicEventsDAtaModelDidRemoveBasicEvent);
      this._beatmapEventBoxGroupsDataModel.didUpdateEventBoxGroup -= new Action<EventBoxGroupEditorData>(this.HandleBeatmapEventBoxGroupDataModelDidUpdateEventBoxGroup);
      foreach (EventBoxGroupEditorData eventBoxGroupsAs in this._beatmapEventBoxGroupsDataModel.GetAllEventBoxGroupsAsList())
      {
        LinkedListNode<BeatmapEventDataBoxGroup> nodeToDelete;
        if (this._eventBoxGroupListNodeById.TryGetValue(eventBoxGroupsAs.id, out nodeToDelete))
        {
          if (nodeToDelete != null)
            this._beatmapEventDataBoxGroupLists.Remove(eventBoxGroupsAs.groupId, nodeToDelete);
          else
            break;
        }
        else
          break;
      }
      foreach (KeyValuePair<BeatmapEditorObjectId, BeatmapEventData> livePreviewEvent in (IEnumerable<KeyValuePair<BeatmapEditorObjectId, BeatmapEventData>>) this._livePreviewEvents)
      {
        if (livePreviewEvent.Value != null)
          this._livePreviewBeatmapData.RemoveBeatmapEventData(livePreviewEvent.Value);
      }
      this._livePreviewBeatmapData.updateAllBeatmapDataOnInsert = false;
      this._livePreviewEvents.Clear();
      this._livePreviewEventGroupUpdateQueue.Clear();
      this._eventBoxGroupListNodeById.Clear();
    }

    private void AddLivePreviewBasicEvent(BasicEventEditorData evt)
    {
      BeatmapEventData beatmapEventData = BeatmapBasicEventConverter.ConvertBasicEvent(evt, (IBeatToTimeConvertor) this._timeConvertor);
      this._livePreviewEvents[evt.id] = beatmapEventData;
      this._livePreviewBeatmapData.InsertBeatmapEventData(beatmapEventData);
    }

    private void RemoveLivePreviewBasicEvent(BasicEventEditorData evt)
    {
      this._livePreviewBeatmapData.RemoveBeatmapEventData(this._livePreviewEvents[evt.id]);
      this._livePreviewEvents.Remove(evt.id);
    }

    private void UpdateLivePreviewEventBoxes(
      EventBoxGroupEditorData eventBoxGroup,
      bool updateDataOnInsert)
    {
      if (this._prevUpdateDataOnInsert != updateDataOnInsert)
      {
        this._beatmapEventDataBoxGroupLists.ToggleUpdateBeatmapDataOnInsert(updateDataOnInsert);
        this._prevUpdateDataOnInsert = updateDataOnInsert;
      }
      int groupSize;
      if (!this._beatmapEventBoxGroupsDataModel.TryGetGroupSizeByEventBoxGroupId(eventBoxGroup.groupId, out groupSize))
        return;
      LinkedListNode<BeatmapEventDataBoxGroup> nodeToDelete;
      if (this._eventBoxGroupListNodeById.TryGetValue(eventBoxGroup.id, out nodeToDelete))
      {
        this._eventBoxGroupListNodeById.Remove(eventBoxGroup.id);
        this._beatmapEventDataBoxGroupLists.Remove(eventBoxGroup.groupId, nodeToDelete);
      }
      if (this._beatmapEventBoxGroupsDataModel.GetEventBoxGroupById(eventBoxGroup.id) == (EventBoxGroupEditorData) null)
        return;
      List<EventBoxEditorData> byEventBoxGroupId = this._beatmapEventBoxGroupsDataModel.GetEventBoxesByEventBoxGroupId(eventBoxGroup.id);
      List<List<BaseEditorData>> list = byEventBoxGroupId.Select<EventBoxEditorData, List<BaseEditorData>>((Func<EventBoxEditorData, List<BaseEditorData>>) (b => this._beatmapEventBoxGroupsDataModel.GetBaseEditorDataListByEventBoxId(b.id))).ToList<List<BaseEditorData>>();
      BeatmapEventDataBoxGroup beatmapEventDataBoxGroup = BeatmapEditorEventBoxGroupRuntimeConverter.ConvertBoxGroup(eventBoxGroup, byEventBoxGroupId, list, groupSize);
      if (beatmapEventDataBoxGroup == null)
        Debug.LogError((object) string.Format("Unable to create group: {0}, Beat: {1}, GroupSize: {2}", (object) eventBoxGroup.groupId, (object) eventBoxGroup.beat, (object) groupSize));
      else
        this._eventBoxGroupListNodeById[eventBoxGroup.id] = this._beatmapEventDataBoxGroupLists.Insert(eventBoxGroup.groupId, beatmapEventDataBoxGroup);
    }

    public void Tick()
    {
      if (this._livePreviewEventGroupUpdateQueue.Count == 0)
        return;
      if (this._livePreviewEventGroupUpdateQueue.Count > 20)
      {
        while (this._livePreviewEventGroupUpdateQueue.Count > 0)
          this.UpdateLivePreviewEventBoxes(this._livePreviewEventGroupUpdateQueue.Dequeue(), false);
        this._beatmapEventDataBoxGroupLists.SyncWithBeatmapData();
      }
      else
        this.UpdateLivePreviewEventBoxes(this._livePreviewEventGroupUpdateQueue.Dequeue(), true);
    }
  }
}
