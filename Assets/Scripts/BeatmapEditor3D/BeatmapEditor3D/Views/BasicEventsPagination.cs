// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.BasicEventsPagination
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
  public class BasicEventsPagination : MonoBehaviour
  {
    [SerializeField]
    private TextSegmentedControl _pageTextSegmentedControl;
    [Inject]
    private readonly BasicEventsState _basicBasicEventsState;
    [Inject]
    private readonly SignalBus _signalBus;

    protected void OnEnable()
    {
      this._signalBus.Subscribe<EventsPageChangedSignal>(new Action(this.HandleEventsPageChanged));
      this._pageTextSegmentedControl.didSelectCellEvent += new Action<SegmentedControl, int>(this.HandlePageTextSegmentedControlDidSelectCell);
      this.SpawnPaginationButtons();
      this.UpdatePage();
    }

    protected void OnDisable()
    {
      this._pageTextSegmentedControl.didSelectCellEvent -= new Action<SegmentedControl, int>(this.HandlePageTextSegmentedControlDidSelectCell);
      this._signalBus.TryUnsubscribe<EventsPageChangedSignal>(new Action(this.HandleEventsPageChanged));
    }

    private void HandleEventsPageChanged() => this.UpdatePage();

    private void HandlePageTextSegmentedControlDidSelectCell(SegmentedControl _, int idx) => this._signalBus.Fire<ChangeEventsPageSignal>(new ChangeEventsPageSignal());

    private void SpawnPaginationButtons() => this._pageTextSegmentedControl.SetTexts((IReadOnlyList<string>) new string[2]
    {
      "Page 1",
      "Page 2"
    });

    private void UpdatePage() => this._pageTextSegmentedControl.SelectCellWithNumber((int) this._basicBasicEventsState.currentEventsPage);
  }
}
