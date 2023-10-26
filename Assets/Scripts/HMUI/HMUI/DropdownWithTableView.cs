// Decompiled with JetBrains decompiler
// Type: HMUI.DropdownWithTableView
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HMUI
{
  [RequireComponent(typeof (RectTransform))]
  public class DropdownWithTableView : MonoBehaviour
  {
    [SerializeField]
    protected Button _button;
    [SerializeField]
    protected TableView _tableView;
    [SerializeField]
    protected ModalView _modalView;
    [SerializeField]
    protected int _numberOfVisibleCells = 5;
    [CompilerGenerated]
    protected int selectedIndex_k__BackingField;

    public event Action<DropdownWithTableView, int> didSelectCellWithIdxEvent;

    public TableView.IDataSource tableViewDataSource => this._tableView.dataSource;

    public int selectedIndex
    {
      get => this.selectedIndex_k__BackingField;
      private set => this.selectedIndex_k__BackingField = value;
    }

    public virtual void Init(TableView.IDataSource tableViewDataSource) => this._tableView.SetDataSource(tableViewDataSource, true);

    public virtual void ReloadData()
    {
      this._tableView.ReloadData();
      this.RefreshSize(this.tableViewDataSource);
    }

    public virtual void SelectCellWithIdx(int idx)
    {
      this.selectedIndex = idx;
      this._tableView.SelectCellWithIdx(idx);
    }

    protected virtual void Awake()
    {
      this._button.onClick.AddListener(new UnityAction(this.OnButtonClick));
      this._modalView.blockerClickedEvent += new Action(this.HandleModalViewBlockerClicked);
      this._tableView.didSelectCellWithIdxEvent += new Action<TableView, int>(this.HandleTableViewDidSelectCellWithIdx);
      this._tableView.gameObject.SetActive(false);
    }

    public virtual void OnDisable() => this.Hide(false);

    protected virtual void OnDestroy()
    {
      if ((UnityEngine.Object) this._tableView != (UnityEngine.Object) null)
        this._tableView.didSelectCellWithIdxEvent -= new Action<TableView, int>(this.HandleTableViewDidSelectCellWithIdx);
      if ((UnityEngine.Object) this._button != (UnityEngine.Object) null)
        this._button.onClick.RemoveListener(new UnityAction(this.OnButtonClick));
      if (!((UnityEngine.Object) this._modalView != (UnityEngine.Object) null))
        return;
      this._modalView.blockerClickedEvent -= new Action(this.HandleModalViewBlockerClicked);
    }

    public virtual void RefreshSize(TableView.IDataSource dataSource)
    {
      float num = (float) Mathf.Min(this._numberOfVisibleCells, this._tableView.numberOfCells) * dataSource.CellSize();
      float height1 = this._tableView.viewportTransform.rect.height;
      float height2 = ((RectTransform) this._tableView.transform).rect.height;
      this._tableView.ChangeRectSize(RectTransform.Axis.Vertical, num + (height2 - height1));
    }

    public virtual void OnButtonClick() => this.Show(true);

    public virtual void HandleTableViewDidSelectCellWithIdx(TableView tableView, int idx)
    {
      this.selectedIndex = idx;
      Action<DropdownWithTableView, int> cellWithIdxEvent = this.didSelectCellWithIdxEvent;
      if (cellWithIdxEvent != null)
        cellWithIdxEvent(this, idx);
      this.Hide(true);
    }

    public virtual void Hide(bool animated)
    {
      this._modalView.Hide(animated);
      this._button.enabled = true;
    }

    public virtual void Show(bool animated)
    {
      if (!this.isActiveAndEnabled)
        return;
      this._button.enabled = false;
      this._modalView.Show(animated);
      this._tableView.ScrollToCellWithIdx(this.selectedIndex, TableView.ScrollPositionType.Center, false);
    }

    public virtual void HandleModalViewBlockerClicked() => this.Hide(true);
  }
}
