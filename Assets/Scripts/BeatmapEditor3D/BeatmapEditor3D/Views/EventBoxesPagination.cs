// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.EventBoxesPagination
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using HMUI;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Views
{
  public class EventBoxesPagination : MonoBehaviour
  {
    [SerializeField]
    private TextSegmentedControl _pageTextSegmentedControl;
    [Inject]
    private readonly EventBoxGroupsState _eventBoxGroupsState;
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;
    [Inject]
    private readonly BeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly SignalBus _signalBus;
    private EventBoxGroupEditorData _eventBoxGroupEditorData;
    private List<EnvironmentTracksDefinitionSO.EventBoxGroupTrackInfo> _trackInfos;

    protected void OnEnable()
    {
      this._pageTextSegmentedControl.didSelectCellEvent += new Action<SegmentedControl, int>(this.HandlePageTextSegmentedControlDidSelectCell);
      this._eventBoxGroupEditorData = this._eventBoxGroupsState.eventBoxGroupContext;
      this.SpawnPaginationButtons();
    }

    protected void OnDisable() => this._pageTextSegmentedControl.didSelectCellEvent -= new Action<SegmentedControl, int>(this.HandlePageTextSegmentedControlDidSelectCell);

    private void HandlePageTextSegmentedControlDidSelectCell(SegmentedControl _, int index)
    {
      EventBoxGroupEditorData eventBoxGroupAt = this._beatmapEventBoxGroupsDataModel.GetEventBoxGroupAt(this._trackInfos[index / 2].lightGroup.groupId, (EventBoxGroupEditorData.EventBoxGroupType) (index % 2), this._eventBoxGroupEditorData.beat);
      if (!(eventBoxGroupAt != (EventBoxGroupEditorData) null))
        return;
      this._signalBus.Fire<EditEventBoxGroupSignal>(new EditEventBoxGroupSignal(eventBoxGroupAt.id));
    }

    private void SpawnPaginationButtons()
    {
      this._trackInfos = this._beatmapDataModel.environmentTrackDefinition.eventBoxGroupPageInfos[this._eventBoxGroupsState.currentPage].eventBoxGroupTrackInfos;
      List<string> texts = new List<string>();
      foreach (EnvironmentTracksDefinitionSO.EventBoxGroupTrackInfo trackInfo in this._trackInfos)
      {
        texts.Add(trackInfo.groupName + " Color");
        texts.Add(trackInfo.groupName + " Rotation");
      }
      this._pageTextSegmentedControl.SetTexts((IReadOnlyList<string>) texts);
      for (int index = 0; index < this._pageTextSegmentedControl.cells.Count; ++index)
      {
        EventBoxGroupEditorData eventBoxGroupAt = this._beatmapEventBoxGroupsDataModel.GetEventBoxGroupAt(this._trackInfos[index / 2].lightGroup.groupId, (EventBoxGroupEditorData.EventBoxGroupType) (index % 2), this._eventBoxGroupEditorData.beat);
        this._pageTextSegmentedControl.cells[index].interactable = eventBoxGroupAt != (EventBoxGroupEditorData) null;
      }
    }
  }
}
