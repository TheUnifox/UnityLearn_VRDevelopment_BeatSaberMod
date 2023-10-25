// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.EventBoxGroupsPagination
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands;
using BeatmapEditor3D.DataModels;
using HMUI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Views
{
  public class EventBoxGroupsPagination : MonoBehaviour
  {
    [SerializeField]
    private TextSegmentedControl _pageTextSegmentedControl;
    [Inject]
    private readonly EventBoxGroupsState _eventBoxGroupsState;
    [Inject]
    private readonly BeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly SignalBus _signalBus;
    private EnvironmentTracksDefinitionSO.EventBoxGroupPageInfo[] _pages;

    protected void OnEnable()
    {
      this._signalBus.Subscribe<EventBoxGroupsPageChangedSignal>(new Action(this.HandleEventBoxGroupsPageChanged));
      this._pageTextSegmentedControl.didSelectCellEvent += new Action<SegmentedControl, int>(this.HandlePageTextSegmentedControlDidSelectCell);
      this.SpawnPaginationButtons();
      this.UpdatePage();
    }

    protected void OnDisable()
    {
      this._pageTextSegmentedControl.didSelectCellEvent -= new Action<SegmentedControl, int>(this.HandlePageTextSegmentedControlDidSelectCell);
      this._signalBus.TryUnsubscribe<EventBoxGroupsPageChangedSignal>(new Action(this.HandleEventBoxGroupsPageChanged));
    }

    private void HandleEventBoxGroupsPageChanged() => this.UpdatePage();

    private void HandlePageTextSegmentedControlDidSelectCell(SegmentedControl _, int index) => this._signalBus.Fire<ChangeEventBoxGroupsPageSignal>(new ChangeEventBoxGroupsPageSignal(index));

    private void SpawnPaginationButtons()
    {
      this._pages = this._beatmapDataModel.environmentTrackDefinition.eventBoxGroupPageInfos;
      this._pageTextSegmentedControl.SetTexts((IReadOnlyList<string>) ((IEnumerable<EnvironmentTracksDefinitionSO.EventBoxGroupPageInfo>) this._pages).Select<EnvironmentTracksDefinitionSO.EventBoxGroupPageInfo, string>((Func<EnvironmentTracksDefinitionSO.EventBoxGroupPageInfo, string>) (page => page.eventBoxGroupPageName)).ToList<string>());
    }

    private void UpdatePage() => this._pageTextSegmentedControl.SelectCellWithNumber(this._eventBoxGroupsState.currentPage);
  }
}
