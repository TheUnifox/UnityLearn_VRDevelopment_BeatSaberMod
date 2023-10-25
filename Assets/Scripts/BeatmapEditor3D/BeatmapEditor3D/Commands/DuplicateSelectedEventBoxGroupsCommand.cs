// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.DuplicateSelectedEventBoxGroupsCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class DuplicateSelectedEventBoxGroupsCommand : 
    IBeatmapEditorCommandWithHistory,
    IBeatmapEditorCommand,
    IBeatmapEditorCommandMessageProvider
  {
    [Inject]
    private readonly EventBoxGroupsSelectionState _selectionState;
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;
    [Inject]
    private readonly BeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly EventBoxGroupsState _eventBoxGroupsState;
    [Inject]
    private readonly EventBoxGroupsClipboardHelper _eventBoxGroupsClipboardHelper;
    [Inject]
    private readonly SignalBus _signalBus;
    private List<EventBoxGroupEditorData> _toDeleteEventBoxGroups;
    private Dictionary<BeatmapEditorObjectId, List<EventBoxEditorData>> _toDeleteEventBoxes;
    private Dictionary<BeatmapEditorObjectId, List<BaseEditorData>> _toDeleteBaseDataLists;
    private List<EventBoxGroupEditorData> _toInsertEventBoxGroups;
    private Dictionary<BeatmapEditorObjectId, List<EventBoxEditorData>> _toInsertEventBoxes;
    private Dictionary<BeatmapEditorObjectId, List<BaseEditorData>> _toInsertBaseDataLists;

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      if (this._selectionState.eventBoxGroups.Count == 0)
        return;
      List<EventBoxGroupEditorData> list1 = this._selectionState.eventBoxGroups.Select<BeatmapEditorObjectId, EventBoxGroupEditorData>((Func<BeatmapEditorObjectId, EventBoxGroupEditorData>) (id => this._beatmapEventBoxGroupsDataModel.GetEventBoxGroupById(id))).ToList<EventBoxGroupEditorData>();
      bool flag = list1.GroupBy<EventBoxGroupEditorData, int>((Func<EventBoxGroupEditorData, int>) (e => e.groupId)).Count<IGrouping<int, EventBoxGroupEditorData>>() == 1;
      int originEventBoxGroupId = list1[0].groupId;
      EnvironmentTracksDefinitionSO.EventBoxGroupTrackInfo trackInfo = this._beatmapDataModel.environmentTrackDefinition.eventBoxGroupPageInfos[this._eventBoxGroupsState.currentPage].eventBoxGroupTrackInfos.First<EnvironmentTracksDefinitionSO.EventBoxGroupTrackInfo>((Func<EnvironmentTracksDefinitionSO.EventBoxGroupTrackInfo, bool>) (i => i.lightGroup.groupId == originEventBoxGroupId));
      if (!trackInfo.enableDuplicate)
        this._signalBus.Fire<CommandMessageSignal>(new CommandMessageSignal(CommandMessageType.Normal, "Cannot duplicate EventBoxGroups. This lane has duplication disabled."));
      else if (!flag)
      {
        this._signalBus.Fire<CommandMessageSignal>(new CommandMessageSignal(CommandMessageType.Normal, "Cannot duplicate EventBoxGroups. Multiple groupIds selected."));
      }
      else
      {
        List<int> list2 = this._beatmapDataModel.environmentTrackDefinition.eventBoxGroupPageInfos[this._eventBoxGroupsState.currentPage].eventBoxGroupTrackInfos.Where<EnvironmentTracksDefinitionSO.EventBoxGroupTrackInfo>((Func<EnvironmentTracksDefinitionSO.EventBoxGroupTrackInfo, bool>) (t =>
        {
          if (!t.enableDuplicate)
            return false;
          return trackInfo.targetLightGroups == null || trackInfo.targetLightGroups.Length == 0 ? t.lightGroup.groupId != originEventBoxGroupId : ((IEnumerable<LightGroupSO>) trackInfo.targetLightGroups).Any<LightGroupSO>((Func<LightGroupSO, bool>) (targetLightGroup => targetLightGroup.groupId == t.lightGroup.groupId));
        })).Select<EnvironmentTracksDefinitionSO.EventBoxGroupTrackInfo, int>((Func<EnvironmentTracksDefinitionSO.EventBoxGroupTrackInfo, int>) (t => t.lightGroup.groupId)).ToList<int>();
        this._toDeleteEventBoxGroups = new List<EventBoxGroupEditorData>();
        foreach (int num in list2)
        {
          int groupId = num;
          this._toDeleteEventBoxGroups.AddRange(list1.Select<EventBoxGroupEditorData, EventBoxGroupEditorData>((Func<EventBoxGroupEditorData, EventBoxGroupEditorData>) (e => this._beatmapEventBoxGroupsDataModel.GetEventBoxGroupAt(groupId, e.type, e.beat))).Where<EventBoxGroupEditorData>((Func<EventBoxGroupEditorData, bool>) (e => e != (EventBoxGroupEditorData) null)));
        }
        this._eventBoxGroupsClipboardHelper.GetAllDataForEventBoxGroups(this._toDeleteEventBoxGroups, out this._toDeleteEventBoxes, out this._toDeleteBaseDataLists);
        Dictionary<BeatmapEditorObjectId, List<EventBoxEditorData>> eventBoxes1;
        Dictionary<BeatmapEditorObjectId, List<BaseEditorData>> baseDataLists;
        this._eventBoxGroupsClipboardHelper.GetAllDataForEventBoxGroups(list1, out eventBoxes1, out baseDataLists);
        this._toInsertEventBoxGroups = new List<EventBoxGroupEditorData>();
        this._toInsertEventBoxes = new Dictionary<BeatmapEditorObjectId, List<EventBoxEditorData>>();
        this._toInsertBaseDataLists = new Dictionary<BeatmapEditorObjectId, List<BaseEditorData>>();
        foreach (int num in list2)
        {
          int groupId = num;
          List<EventBoxGroupEditorData> list3 = list1.Select<EventBoxGroupEditorData, EventBoxGroupEditorData>((Func<EventBoxGroupEditorData, EventBoxGroupEditorData>) (e => EventBoxGroupEditorData.CreateNew(e.beat, groupId, e.type))).ToList<EventBoxGroupEditorData>();
          Dictionary<BeatmapEditorObjectId, BeatmapEditorObjectId> dictionary = new Dictionary<BeatmapEditorObjectId, BeatmapEditorObjectId>(list3.Count);
          for (int index = 0; index < list3.Count; ++index)
            dictionary[list3[index].id] = list1[index].id;
          this._toInsertEventBoxGroups.AddRange((IEnumerable<EventBoxGroupEditorData>) list3);
          foreach (EventBoxGroupEditorData boxGroupEditorData in list3)
          {
            BeatmapEditorObjectId key = dictionary[boxGroupEditorData.id];
            List<EventBoxEditorData> eventBoxes2 = eventBoxes1[key];
            List<EventBoxEditorData> newEventBoxes = new List<EventBoxEditorData>(eventBoxes2.Count);
            this._eventBoxGroupsClipboardHelper.CopyEventBoxesWithBaseLists(eventBoxes2, (IReadOnlyDictionary<BeatmapEditorObjectId, List<BaseEditorData>>) baseDataLists, newEventBoxes, this._toInsertBaseDataLists);
            this._toInsertEventBoxes[boxGroupEditorData.id] = newEventBoxes;
          }
        }
        this.shouldAddToHistory = true;
        this.Redo();
      }
    }

    public void Undo()
    {
      this._eventBoxGroupsClipboardHelper.RemoveEventBoxGroupData(this._toInsertEventBoxGroups, this._toInsertEventBoxes, this._toInsertBaseDataLists);
      this._eventBoxGroupsClipboardHelper.InsertEventBoxGroupData(this._toDeleteEventBoxGroups, this._toDeleteEventBoxes, this._toDeleteBaseDataLists);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public void Redo()
    {
      this._eventBoxGroupsClipboardHelper.RemoveEventBoxGroupData(this._toDeleteEventBoxGroups, this._toDeleteEventBoxes, this._toDeleteBaseDataLists);
      this._eventBoxGroupsClipboardHelper.InsertEventBoxGroupData(this._toInsertEventBoxGroups, this._toInsertEventBoxes, this._toInsertBaseDataLists);
      this._selectionState.Clear();
      this._selectionState.AddRange(this._toInsertEventBoxGroups.Select<EventBoxGroupEditorData, BeatmapEditorObjectId>((Func<EventBoxGroupEditorData, BeatmapEditorObjectId>) (e => e.id)));
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public (CommandMessageType, string) GetMessage() => (CommandMessageType.Normal, string.Format("Duplicated {0} EventBoxGroups.", (object) this._toInsertEventBoxGroups?.Count));
  }
}
