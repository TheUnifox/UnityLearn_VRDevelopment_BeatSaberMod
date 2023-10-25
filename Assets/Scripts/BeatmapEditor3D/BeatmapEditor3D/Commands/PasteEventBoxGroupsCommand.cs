// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.PasteEventBoxGroupsCommand
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
  public class PasteEventBoxGroupsCommand : 
    IBeatmapEditorCommandWithHistory,
    IBeatmapEditorCommand,
    IBeatmapEditorCommandMessageProvider
  {
    [Inject]
    private readonly EventBoxGroupsClipboardState _clipboardState;
    [Inject]
    private readonly EventBoxGroupsSelectionState _selectionState;
    [Inject]
    private readonly BeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;
    [Inject]
    private readonly EventBoxGroupsState _eventBoxGroupsState;
    [Inject]
    private readonly EventBoxGroupsClipboardHelper _eventBoxGroupsClipboardHelper;
    [Inject]
    private readonly SignalBus _signalBus;
    private List<EventBoxGroupEditorData> _newEventBoxGroups;
    private readonly Dictionary<BeatmapEditorObjectId, List<EventBoxEditorData>> _newEventBoxes = new Dictionary<BeatmapEditorObjectId, List<EventBoxEditorData>>();
    private readonly Dictionary<BeatmapEditorObjectId, List<BaseEditorData>> _newBaseDataLists = new Dictionary<BeatmapEditorObjectId, List<BaseEditorData>>();

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      if (this._clipboardState.eventBoxGroups.Count == 0)
        return;
      float pasteStartBeat = this._beatmapState.beat;
      float clipboardStartBeat = this._clipboardState.startBeat;
      int currentPage = this._eventBoxGroupsState.currentPage;
      bool differentPage = this.CanPasteToDifferentPage();
      bool canPasteToDifferentGroupId = this.CanPasteToDifferentGroupId();
      int groupIdToPage = this._beatmapDataModel.environmentTrackDefinition.groupIdToPageMap[this._clipboardState.eventBoxGroups[0].groupId];
      if (currentPage != groupIdToPage)
      {
        if (differentPage)
        {
          EnvironmentTracksDefinitionSO.EventBoxGroupPageInfo boxGroupPageInfo1 = this._beatmapDataModel.environmentTrackDefinition.eventBoxGroupPageInfos[groupIdToPage];
          EnvironmentTracksDefinitionSO.EventBoxGroupPageInfo boxGroupPageInfo2 = this._beatmapDataModel.environmentTrackDefinition.eventBoxGroupPageInfos[currentPage];
          Dictionary<int, int> groupIdMapping = new Dictionary<int, int>();
          for (int index = 0; index < boxGroupPageInfo1.eventBoxGroupTrackInfos.Count && index != boxGroupPageInfo2.eventBoxGroupTrackInfos.Count; ++index)
          {
            groupIdMapping[boxGroupPageInfo1.eventBoxGroupTrackInfos[index].lightGroup.groupId] = boxGroupPageInfo2.eventBoxGroupTrackInfos[index].lightGroup.groupId;
            this._newEventBoxGroups = this._clipboardState.eventBoxGroups.Where<EventBoxGroupEditorData>((Func<EventBoxGroupEditorData, bool>) (e => groupIdMapping.ContainsKey(e.groupId))).Select<EventBoxGroupEditorData, EventBoxGroupEditorData>((Func<EventBoxGroupEditorData, EventBoxGroupEditorData>) (e => EventBoxGroupEditorData.CreateNew(pasteStartBeat + e.beat - clipboardStartBeat, canPasteToDifferentGroupId ? this._eventBoxGroupsState.currentHoverGroupId : groupIdMapping[e.groupId], e.type))).ToList<EventBoxGroupEditorData>();
          }
        }
        else
        {
          this._signalBus.Fire<CommandMessageSignal>(new CommandMessageSignal(CommandMessageType.Warning, "Cannot paste EventBoxGroups to different page. Multiple pages selected."));
          return;
        }
      }
      else
        this._newEventBoxGroups = canPasteToDifferentGroupId ? this._clipboardState.eventBoxGroups.Select<EventBoxGroupEditorData, EventBoxGroupEditorData>((Func<EventBoxGroupEditorData, EventBoxGroupEditorData>) (e => EventBoxGroupEditorData.CreateNew(pasteStartBeat + e.beat - clipboardStartBeat, this._eventBoxGroupsState.currentHoverGroupId, e.type))).ToList<EventBoxGroupEditorData>() : this._clipboardState.eventBoxGroups.Select<EventBoxGroupEditorData, EventBoxGroupEditorData>((Func<EventBoxGroupEditorData, EventBoxGroupEditorData>) (e => EventBoxGroupEditorData.CreateNew(pasteStartBeat + e.beat - clipboardStartBeat, e.groupId, e.type))).ToList<EventBoxGroupEditorData>();
      Dictionary<BeatmapEditorObjectId, BeatmapEditorObjectId> dictionary = new Dictionary<BeatmapEditorObjectId, BeatmapEditorObjectId>(this._newEventBoxGroups.Count);
      for (int index = 0; index < this._newEventBoxGroups.Count; ++index)
        dictionary[this._newEventBoxGroups[index].id] = this._clipboardState.eventBoxGroups[index].id;
      this._newEventBoxGroups = this._newEventBoxGroups.Where<EventBoxGroupEditorData>((Func<EventBoxGroupEditorData, bool>) (e => this.CanPasteToTrack(e.groupId, e.type))).Where<EventBoxGroupEditorData>((Func<EventBoxGroupEditorData, bool>) (e => this._beatmapEventBoxGroupsDataModel.GetEventBoxGroupAt(e.groupId, e.type, e.beat) == (EventBoxGroupEditorData) null)).ToList<EventBoxGroupEditorData>();
      if (!this._newEventBoxGroups.Any<EventBoxGroupEditorData>())
      {
        this._signalBus.Fire<CommandMessageSignal>(new CommandMessageSignal(CommandMessageType.Normal, "Cannot paste EventBoxGroups, another EventBoxGroups already present."));
      }
      else
      {
        foreach (EventBoxGroupEditorData newEventBoxGroup in this._newEventBoxGroups)
        {
          List<EventBoxEditorData> eventBox = this._clipboardState.eventBoxes[dictionary[newEventBoxGroup.id]];
          List<EventBoxEditorData> newEventBoxes = new List<EventBoxEditorData>(eventBox.Count);
          this._eventBoxGroupsClipboardHelper.CopyEventBoxesWithBaseLists(eventBox, this._clipboardState.baseEditorDataLists, newEventBoxes, this._newBaseDataLists);
          this._newEventBoxes[newEventBoxGroup.id] = newEventBoxes;
        }
        this.shouldAddToHistory = true;
        this.Redo();
      }
    }

    public void Undo()
    {
      this._eventBoxGroupsClipboardHelper.RemoveEventBoxGroupData(this._newEventBoxGroups, this._newEventBoxes, this._newBaseDataLists);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public void Redo()
    {
      this._eventBoxGroupsClipboardHelper.InsertEventBoxGroupData(this._newEventBoxGroups, this._newEventBoxes, this._newBaseDataLists);
      this._selectionState.Clear();
      this._selectionState.AddRange(this._newEventBoxGroups.Select<EventBoxGroupEditorData, BeatmapEditorObjectId>((Func<EventBoxGroupEditorData, BeatmapEditorObjectId>) (e => e.id)));
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public (CommandMessageType, string) GetMessage() => (CommandMessageType.Normal, string.Format("Pasted {0} event box groups", (object) this._newEventBoxGroups?.Count));

    private bool CanPasteToDifferentPage() => this._clipboardState.eventBoxGroups.GroupBy<EventBoxGroupEditorData, int>((Func<EventBoxGroupEditorData, int>) (eventBoxGroup => this._beatmapDataModel.environmentTrackDefinition.groupIdToPageMap[eventBoxGroup.groupId])).Count<IGrouping<int, EventBoxGroupEditorData>>() == 1;

    private bool CanPasteToDifferentGroupId() => this._clipboardState.eventBoxGroups.GroupBy<EventBoxGroupEditorData, int>((Func<EventBoxGroupEditorData, int>) (eventBoxGroup => eventBoxGroup.groupId)).Count<IGrouping<int, EventBoxGroupEditorData>>() == 1;

    private bool CanPasteToTrack(int groupId, EventBoxGroupEditorData.EventBoxGroupType type)
    {
      if (type == EventBoxGroupEditorData.EventBoxGroupType.Rotation)
        return this._beatmapDataModel.environmentTrackDefinition.groupIdToTrackInfo[groupId].showRotationTrack;
      return type == EventBoxGroupEditorData.EventBoxGroupType.Translation ? this._beatmapDataModel.environmentTrackDefinition.groupIdToTrackInfo[groupId].showTranslationTrack : this._beatmapDataModel.environmentTrackDefinition.groupIdToTrackInfo[groupId].showColorTrack;
    }
  }
}
