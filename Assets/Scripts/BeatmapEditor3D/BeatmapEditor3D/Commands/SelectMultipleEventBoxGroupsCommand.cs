// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.SelectMultipleEventBoxGroupsCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System.Collections.Generic;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class SelectMultipleEventBoxGroupsCommand : IBeatmapEditorCommand
  {
    private const float kMinBeatDistance = 0.00390625f;
    [Inject]
    private readonly SelectMultipleEventBoxGroupsSignal _signal;
    [Inject]
    private readonly BeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly EventBoxGroupsSelectionState _selectionState;
    [Inject]
    private readonly EventBoxGroupsState _eventBoxGroupsState;
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;
    [Inject]
    private readonly SignalBus _signalBus;

    public void Execute()
    {
      if (!this._signal.commit)
      {
        this._selectionState.Clear();
      }
      else
      {
        if (!this._signal.addToSelection)
          this._selectionState.Clear();
        float startBeat = this._selectionState.startBeat;
        float endBeat = this._selectionState.endBeat;
        List<EnvironmentTracksDefinitionSO.EventBoxGroupTrackInfo> boxGroupTrackInfos = this._beatmapDataModel.environmentTrackDefinition.eventBoxGroupPageInfos[this._eventBoxGroupsState.currentPage].eventBoxGroupTrackInfos;
        for (int startPageIndex = this._selectionState.startPageIndex; startPageIndex <= this._selectionState.endPageIndex; ++startPageIndex)
        {
          this.AddGroupToSelection(boxGroupTrackInfos[startPageIndex].lightGroup.groupId, EventBoxGroupEditorData.EventBoxGroupType.Color, startBeat, endBeat);
          this.AddGroupToSelection(boxGroupTrackInfos[startPageIndex].lightGroup.groupId, EventBoxGroupEditorData.EventBoxGroupType.Rotation, startBeat, endBeat);
          this.AddGroupToSelection(boxGroupTrackInfos[startPageIndex].lightGroup.groupId, EventBoxGroupEditorData.EventBoxGroupType.Translation, startBeat, endBeat);
        }
        this._signalBus.Fire<EventBoxGroupsSelectionStateUpdatedSignal>();
      }
    }

    private void AddGroupToSelection(
      int groupId,
      EventBoxGroupEditorData.EventBoxGroupType groupType,
      float startBeat,
      float endBeat)
    {
      foreach (EventBoxGroupEditorData data in this._beatmapEventBoxGroupsDataModel.GetEventBoxGroupsInterval(groupId, groupType, startBeat - 1f / 256f, endBeat + 1f / 256f))
        this.AddSingle(data);
    }

    private void AddSingle(EventBoxGroupEditorData data)
    {
      if (this._selectionState.IsSelected(data.id))
        return;
      this._selectionState.Add(data.id);
    }
  }
}
