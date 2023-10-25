// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.PlaceEventBoxGroupCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Controller;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using System.Collections.Generic;
using Zenject;

namespace BeatmapEditor3D
{
  public class PlaceEventBoxGroupCommand : IBeatmapEditorCommandWithHistory, IBeatmapEditorCommand
  {
    [Inject]
    private readonly PlaceEventBoxGroupSignal _signal;
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly ISongPreviewController _songPreviewController;
    [Inject]
    private readonly EventBoxGroupsState _eventBoxGroupsState;
    [Inject]
    private readonly BeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly SignalBus _signalBus;
    private EventBoxGroupEditorData _eventBoxGroup;
    private readonly List<EventBoxEditorData> _eventBoxes = new List<EventBoxEditorData>();
    private readonly List<(BeatmapEditorObjectId id, BaseEditorData data)> _baseEditorData = new List<(BeatmapEditorObjectId, BaseEditorData)>();

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      int groupId = this._signal.groupId;
      float beat = this._beatmapState.beat;
      if (this._songPreviewController.isPlaying)
        beat = AudioTimeHelper.RoundToBeat(beat, this._beatmapState.subdivision);
      if (this._beatmapEventBoxGroupsDataModel.GetEventBoxGroupAt(groupId, this._signal.type, beat) != (EventBoxGroupEditorData) null)
        return;
      this._eventBoxGroup = EventBoxGroupEditorData.CreateNew(beat, groupId, this._signal.type);
      if (this._eventBoxGroupsState.eventBoxGroupExtension)
        this.CreateExtensionEventBoxGroup();
      else
        this.CreateEventBoxGroup(groupId);
      this.shouldAddToHistory = true;
      this.Redo();
    }

    public void Undo()
    {
      if (this._baseEditorData != null)
      {
        foreach ((BeatmapEditorObjectId id, BaseEditorData data) tuple in this._baseEditorData)
          this._beatmapEventBoxGroupsDataModel.RemoveBaseEditorData(tuple.id, tuple.data);
      }
      foreach (EventBoxEditorData eventBox in this._eventBoxes)
        this._beatmapEventBoxGroupsDataModel.RemoveEventBox(this._eventBoxGroup.id, eventBox);
      this._beatmapEventBoxGroupsDataModel.RemoveEventBoxGroup(this._eventBoxGroup);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public void Redo()
    {
      this._beatmapEventBoxGroupsDataModel.InsertEventBoxGroup(this._eventBoxGroup);
      foreach (EventBoxEditorData eventBox in this._eventBoxes)
        this._beatmapEventBoxGroupsDataModel.InsertEventBox(this._eventBoxGroup.id, eventBox);
      if (this._baseEditorData != null)
      {
        foreach ((BeatmapEditorObjectId id, BaseEditorData data) tuple in this._baseEditorData)
          this._beatmapEventBoxGroupsDataModel.InsertBaseEditorData(tuple.id, tuple.data);
      }
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    private void CreateExtensionEventBoxGroup()
    {
      switch (this._signal.type)
      {
        case EventBoxGroupEditorData.EventBoxGroupType.Color:
          LightColorEventBoxEditorData extension1 = LightColorEventBoxEditorData.CreateExtension();
          this._eventBoxes.Add((EventBoxEditorData) extension1);
          this._baseEditorData.Add((extension1.id, (BaseEditorData) LightColorBaseEditorData.CreateExtension(0.0f)));
          break;
        case EventBoxGroupEditorData.EventBoxGroupType.Rotation:
          LightRotationEventBoxEditorData extension2 = LightRotationEventBoxEditorData.CreateExtension(LightAxis.X);
          LightRotationEventBoxEditorData extension3 = LightRotationEventBoxEditorData.CreateExtension(LightAxis.Y);
          LightRotationEventBoxEditorData extension4 = LightRotationEventBoxEditorData.CreateExtension(LightAxis.Z);
          this._eventBoxes.Add((EventBoxEditorData) extension2);
          this._eventBoxes.Add((EventBoxEditorData) extension3);
          this._eventBoxes.Add((EventBoxEditorData) extension4);
          this._baseEditorData.Add((extension2.id, (BaseEditorData) LightRotationBaseEditorData.CreateExtension(0.0f)));
          this._baseEditorData.Add((extension3.id, (BaseEditorData) LightRotationBaseEditorData.CreateExtension(0.0f)));
          this._baseEditorData.Add((extension4.id, (BaseEditorData) LightRotationBaseEditorData.CreateExtension(0.0f)));
          break;
        case EventBoxGroupEditorData.EventBoxGroupType.Translation:
          LightTranslationEventBoxEditorData extension5 = LightTranslationEventBoxEditorData.CreateExtension(LightAxis.X);
          LightTranslationEventBoxEditorData extension6 = LightTranslationEventBoxEditorData.CreateExtension(LightAxis.Y);
          LightTranslationEventBoxEditorData extension7 = LightTranslationEventBoxEditorData.CreateExtension(LightAxis.Z);
          this._eventBoxes.Add((EventBoxEditorData) extension5);
          this._eventBoxes.Add((EventBoxEditorData) extension6);
          this._eventBoxes.Add((EventBoxEditorData) extension7);
          this._baseEditorData.Add((extension5.id, (BaseEditorData) LightTranslationBaseEditorData.CreateExtension(0.0f)));
          this._baseEditorData.Add((extension6.id, (BaseEditorData) LightTranslationBaseEditorData.CreateExtension(0.0f)));
          this._baseEditorData.Add((extension7.id, (BaseEditorData) LightTranslationBaseEditorData.CreateExtension(0.0f)));
          break;
      }
    }

    private void CreateEventBoxGroup(int groupId)
    {
      switch (this._signal.type)
      {
        case EventBoxGroupEditorData.EventBoxGroupType.Color:
          LightColorEventBoxEditorData newDefault1 = LightColorEventBoxEditorData.CreateNewDefault();
          this._eventBoxes.Add((EventBoxEditorData) newDefault1);
          this._baseEditorData.Add((newDefault1.id, (BaseEditorData) LightColorBaseEditorData.CreateNew(0.0f, this._eventBoxGroupsState.lightColorBrightness, this._eventBoxGroupsState.lightColorTransitionType, this._eventBoxGroupsState.lightColorType, this._eventBoxGroupsState.lightStrobeBeatFrequency)));
          break;
        case EventBoxGroupEditorData.EventBoxGroupType.Rotation:
          LightRotationEventBoxEditorData newDefault2 = LightRotationEventBoxEditorData.CreateNewDefault(this.GetLightAxis(groupId, EventBoxGroupEditorData.EventBoxGroupType.Rotation));
          this._eventBoxes.Add((EventBoxEditorData) newDefault2);
          this._baseEditorData.Add((newDefault2.id, (BaseEditorData) LightRotationBaseEditorData.CreateNew(0.0f, this._eventBoxGroupsState.lightRotationEaseType, this._eventBoxGroupsState.lightRotationLoopCount, this._eventBoxGroupsState.lightRotation, false, this._eventBoxGroupsState.lightRotationDirection)));
          break;
        case EventBoxGroupEditorData.EventBoxGroupType.Translation:
          LightTranslationEventBoxEditorData newDefault3 = LightTranslationEventBoxEditorData.CreateNewDefault(this.GetLightAxis(groupId, EventBoxGroupEditorData.EventBoxGroupType.Translation));
          this._eventBoxes.Add((EventBoxEditorData) newDefault3);
          this._baseEditorData.Add((newDefault3.id, (BaseEditorData) LightTranslationBaseEditorData.CreateNew(0.0f, this._eventBoxGroupsState.lightTranslationEaseType, this._eventBoxGroupsState.lightTranslation, false)));
          break;
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
