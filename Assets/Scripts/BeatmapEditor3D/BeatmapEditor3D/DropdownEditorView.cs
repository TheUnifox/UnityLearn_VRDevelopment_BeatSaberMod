// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DropdownEditorView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using HMUI;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BeatmapEditor3D
{
  public class DropdownEditorView : MonoBehaviour
  {
    [SerializeField]
    private Button _button;
    [SerializeField]
    private TableView _tableView;
    [SerializeField]
    private int _numberOfVisibleCell = 5;
    [SerializeField]
    private TableView.ScrollPositionType _scrollPositionType = TableView.ScrollPositionType.Center;

    public event Action<DropdownEditorView, int> didSelectCellWithIdxEvent;

    public int selectedIndex { get; private set; }

    public void Init(TableView.IDataSource tableViewDataSource) => this._tableView.SetDataSource(tableViewDataSource, true);

    public void ReloadData()
    {
      this._tableView.ReloadData();
      this.RefreshSize(this._tableView.dataSource);
    }

    public virtual void SelectCellWithIdx(int idx)
    {
      this.selectedIndex = idx;
      this._tableView.SelectCellWithIdx(idx);
    }

    protected virtual void Awake()
    {
      this._button.onClick.AddListener(new UnityAction(this.HandleButtonOnClick));
      this._tableView.didSelectCellWithIdxEvent += new Action<TableView, int>(this.HandleTableViewDidSelectCellWithIdx);
      this._tableView.gameObject.SetActive(false);
    }

    protected virtual void OnDestroy()
    {
      if ((UnityEngine.Object) this._tableView != (UnityEngine.Object) null)
        this._tableView.didSelectCellWithIdxEvent -= new Action<TableView, int>(this.HandleTableViewDidSelectCellWithIdx);
      if (!((UnityEngine.Object) this._button != (UnityEngine.Object) null))
        return;
      this._button.onClick.RemoveListener(new UnityAction(this.HandleButtonOnClick));
    }

    private void RefreshSize(TableView.IDataSource dataSource)
    {
      float num = (float) Mathf.Min(this._numberOfVisibleCell, this._tableView.numberOfCells) * dataSource.CellSize();
      float height1 = this._tableView.viewportTransform.rect.height;
      float height2 = ((RectTransform) this._tableView.transform).rect.height;
      this._tableView.ChangeRectSize(RectTransform.Axis.Vertical, num + (height2 - height1));
    }

    private void Show()
    {
      if (!this.isActiveAndEnabled)
        return;
      this._button.enabled = false;
      this._tableView.gameObject.SetActive(true);
      this._tableView.ScrollToCellWithIdx(this.selectedIndex, this._scrollPositionType, false);
    }

    private void Hide()
    {
      this._tableView.gameObject.SetActive(false);
      this._button.enabled = true;
    }

    private void HandleButtonOnClick() => this.Show();

    private void HandleTableViewDidSelectCellWithIdx(TableView tableView, int idx)
    {
      this.selectedIndex = idx;
      Action<DropdownEditorView, int> cellWithIdxEvent = this.didSelectCellWithIdxEvent;
      if (cellWithIdxEvent != null)
        cellWithIdxEvent(this, idx);
      this.Hide();
    }
  }
}
