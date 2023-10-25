// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.InsertEventBoxCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using System;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class InsertEventBoxCommand : IBeatmapEditorCommandWithHistory, IBeatmapEditorCommand
  {
    [Inject]
    private readonly EventBoxGroupsState _eventBoxGroupsState;
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;
    [Inject]
    private readonly BeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly SignalBus _signalBus;
    private int _groupId;
    private BeatmapEditorObjectId _eventBoxGroupId;
    private EventBoxEditorData _eventBoxEditorData;
    private int _eventBoxCount;

    public bool shouldAddToHistory => true;

    public void Execute()
    {
      this._groupId = this._eventBoxGroupsState.eventBoxGroupContext.groupId;
      this._eventBoxGroupId = this._eventBoxGroupsState.eventBoxGroupContext.id;
      this._eventBoxEditorData = this.CreateEventBoxEditorData(this._eventBoxGroupsState.eventBoxGroupContext.type);
      this._eventBoxCount = this._beatmapEventBoxGroupsDataModel.GetEventBoxesByEventBoxGroupId(this._eventBoxGroupId).Count;
      this.Redo();
    }

    public void Undo()
    {
      this._beatmapEventBoxGroupsDataModel.RemoveEventBox(this._eventBoxGroupId, this._eventBoxEditorData);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
      this._signalBus.Fire<EventBoxesUpdatedSignal>(new EventBoxesUpdatedSignal(this._eventBoxCount - 1));
    }

    public void Redo()
    {
      this._beatmapEventBoxGroupsDataModel.InsertEventBox(this._eventBoxGroupId, this._eventBoxEditorData);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
      this._signalBus.Fire<EventBoxesUpdatedSignal>(new EventBoxesUpdatedSignal(this._eventBoxCount));
    }

    private EventBoxEditorData CreateEventBoxEditorData(
      EventBoxGroupEditorData.EventBoxGroupType type)
    {
      switch (type)
      {
        case EventBoxGroupEditorData.EventBoxGroupType.Color:
          return (EventBoxEditorData) LightColorEventBoxEditorData.CreateNewDefault();
        case EventBoxGroupEditorData.EventBoxGroupType.Rotation:
          return (EventBoxEditorData) LightRotationEventBoxEditorData.CreateNewDefault(this.GetLightAxis(this._groupId, type));
        case EventBoxGroupEditorData.EventBoxGroupType.Translation:
          return (EventBoxEditorData) LightTranslationEventBoxEditorData.CreateNewDefault(this.GetLightAxis(this._groupId, type));
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
