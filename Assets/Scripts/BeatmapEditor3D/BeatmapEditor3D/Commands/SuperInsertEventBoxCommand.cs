// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.SuperInsertEventBoxCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using System;
using System.Collections.Generic;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class SuperInsertEventBoxCommand : IBeatmapEditorCommandWithHistory, IBeatmapEditorCommand
  {
    [Inject]
    private readonly EventBoxGroupsState _eventBoxGroupsState;
    [Inject]
    private readonly BeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;
    [Inject]
    private readonly SignalBus _signalBus;
    private BeatmapEditorObjectId _eventBoxGroupId;
    private List<EventBoxEditorData> _previousEventBoxesEditorData;
    private readonly Dictionary<BeatmapEditorObjectId, List<BaseEditorData>> _previousLightColorBaseLists = new Dictionary<BeatmapEditorObjectId, List<BaseEditorData>>();
    private List<EventBoxEditorData> _eventBoxesEditorData;

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      if (this._eventBoxGroupsState.eventBoxGroupContext == (EventBoxGroupEditorData) null)
        return;
      this._eventBoxGroupId = this._eventBoxGroupsState.eventBoxGroupContext.id;
      this._previousEventBoxesEditorData = new List<EventBoxEditorData>((IEnumerable<EventBoxEditorData>) this._beatmapEventBoxGroupsDataModel.GetEventBoxesByEventBoxGroupId(this._eventBoxGroupId));
      foreach (EventBoxEditorData eventBoxEditorData in this._previousEventBoxesEditorData)
        this.GetLightBaseData(eventBoxEditorData);
      EventBoxEditorData defaultEventBox = this.CreateDefaultEventBox(this._eventBoxGroupsState.eventBoxGroupContext.groupId, this._eventBoxGroupsState.eventBoxGroupContext.type);
      int groupSize;
      if (!this._beatmapEventBoxGroupsDataModel.TryGetGroupSizeByEventBoxGroupId(this._eventBoxGroupsState.eventBoxGroupContext.groupId, out groupSize))
        return;
      this._eventBoxesEditorData = new List<EventBoxEditorData>(groupSize);
      for (int offset = 0; offset < groupSize; ++offset)
        this._eventBoxesEditorData.Add(SuperInsertEventBoxCommand.CreateCopyWithIndexFilter(defaultEventBox, IndexFilterEditorData.CreateStepIndexFilter(offset, 0, false)));
      this.shouldAddToHistory = true;
      this.Redo();
    }

    public void Undo()
    {
      this._beatmapEventBoxGroupsDataModel.InsertEventBoxes(this._eventBoxGroupId, (IEnumerable<EventBoxEditorData>) this._previousEventBoxesEditorData);
      foreach (KeyValuePair<BeatmapEditorObjectId, List<BaseEditorData>> lightColorBaseList in this._previousLightColorBaseLists)
        this._beatmapEventBoxGroupsDataModel.InsertBaseEditorDataList(lightColorBaseList.Key, (IEnumerable<BaseEditorData>) lightColorBaseList.Value);
      this._beatmapEventBoxGroupsDataModel.RemoveEventBoxes(this._eventBoxGroupId, (IEnumerable<EventBoxEditorData>) this._eventBoxesEditorData);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
      this._signalBus.Fire<EventBoxesUpdatedSignal>(new EventBoxesUpdatedSignal(0));
    }

    public void Redo()
    {
      foreach (KeyValuePair<BeatmapEditorObjectId, List<BaseEditorData>> lightColorBaseList in this._previousLightColorBaseLists)
        this._beatmapEventBoxGroupsDataModel.RemoveBaseEditorDataList(lightColorBaseList.Key);
      this._beatmapEventBoxGroupsDataModel.RemoveEventBoxes(this._eventBoxGroupId, (IEnumerable<EventBoxEditorData>) this._previousEventBoxesEditorData);
      this._beatmapEventBoxGroupsDataModel.InsertEventBoxes(this._eventBoxGroupId, (IEnumerable<EventBoxEditorData>) this._eventBoxesEditorData);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
      this._signalBus.Fire<EventBoxesUpdatedSignal>(new EventBoxesUpdatedSignal(0));
    }

    private void GetLightBaseData(EventBoxEditorData eventBoxEditorData) => this._previousLightColorBaseLists[eventBoxEditorData.id] = new List<BaseEditorData>((IEnumerable<BaseEditorData>) this._beatmapEventBoxGroupsDataModel.GetBaseEditorDataListByEventBoxId(eventBoxEditorData.id));

    private EventBoxEditorData CreateDefaultEventBox(
      int groupId,
      EventBoxGroupEditorData.EventBoxGroupType type)
    {
      switch (type)
      {
        case EventBoxGroupEditorData.EventBoxGroupType.Color:
          return (EventBoxEditorData) LightColorEventBoxEditorData.CreateNewDefault();
        case EventBoxGroupEditorData.EventBoxGroupType.Rotation:
          return (EventBoxEditorData) LightRotationEventBoxEditorData.CreateNewDefault(this.GetLightAxis(groupId, type));
        case EventBoxGroupEditorData.EventBoxGroupType.Translation:
          return (EventBoxEditorData) LightTranslationEventBoxEditorData.CreateNewDefault(this.GetLightAxis(groupId, type));
        default:
          throw new ArgumentException();
      }
    }

    private static EventBoxEditorData CreateCopyWithIndexFilter(
      EventBoxEditorData eventBox,
      IndexFilterEditorData indexFilter)
    {
      switch (eventBox)
      {
        case LightColorEventBoxEditorData eventBoxEditorData1:
          return (EventBoxEditorData) LightColorEventBoxEditorData.CreateNew(indexFilter, eventBoxEditorData1.beatDistributionParamType, eventBoxEditorData1.beatDistributionParam, eventBoxEditorData1.brightnessDistributionParamType, eventBoxEditorData1.brightnessDistributionParam, eventBoxEditorData1.brightnessDistributionShouldAffectFirstBaseEvent, eventBoxEditorData1.brightnessDistributionEaseType);
        case LightRotationEventBoxEditorData eventBoxEditorData2:
          return (EventBoxEditorData) LightRotationEventBoxEditorData.CreateNew(indexFilter, eventBoxEditorData2.beatDistributionParamType, eventBoxEditorData2.beatDistributionParam, eventBoxEditorData2.rotationDistributionParamType, eventBoxEditorData2.rotationDistributionParam, eventBoxEditorData2.rotationDistributionShouldAffectFirstBaseEvent, eventBoxEditorData2.rotationDistributionEaseType, eventBoxEditorData2.axis, eventBoxEditorData2.flipRotation);
        case LightTranslationEventBoxEditorData eventBoxEditorData3:
          return (EventBoxEditorData) LightTranslationEventBoxEditorData.CreateNew(indexFilter, eventBoxEditorData3.beatDistributionParamType, eventBoxEditorData3.beatDistributionParam, eventBoxEditorData3.gapDistributionParamType, eventBoxEditorData3.gapDistributionParam, eventBoxEditorData3.gapDistributionShouldAffectFirstBaseEvent, eventBoxEditorData3.gapDistributionEaseType, eventBoxEditorData3.axis, eventBoxEditorData3.flipTranslation);
        default:
          throw new ArgumentException();
      }
    }

    private LightAxis GetLightAxis(
      int groupId,
      EventBoxGroupEditorData.EventBoxGroupType eventBoxGroupType)
    {
      return EnvironmentTracksHelper.GetLightAxis(this._beatmapDataModel.environmentTrackDefinition.groupIdToTrackInfo[groupId], eventBoxGroupType);
    }
  }
}
