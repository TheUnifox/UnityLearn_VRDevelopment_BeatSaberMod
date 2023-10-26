// Decompiled with JetBrains decompiler
// Type: HMUI.IconSegmentedControl
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

namespace HMUI
{
  public class IconSegmentedControl : SegmentedControl, SegmentedControl.IDataSource
  {
    [SerializeField]
    protected float _iconSize = 4f;
    [SerializeField]
    protected bool _overrideCellSize;
    [DrawIf("_overrideCellSize", true, DrawIfAttribute.DisablingType.DontDraw)]
    [SerializeField]
    protected float _padding = 4f;
    [SerializeField]
    protected bool _hideCellBackground;
    [Space]
    [SerializeField]
    protected IconSegmentedControlCell _firstCellPrefab;
    [SerializeField]
    protected IconSegmentedControlCell _lastCellPrefab;
    [SerializeField]
    protected IconSegmentedControlCell _middleCellPrefab;
    [SerializeField]
    protected IconSegmentedControlCell _singleCellPrefab;
    [Inject]
    protected readonly DiContainer _container;
    protected IconSegmentedControl.DataItem[] _dataItems;
    protected bool _isInitialized;

    public virtual void Init()
    {
      if (this._isInitialized)
        return;
      this._isInitialized = true;
      this.dataSource = (SegmentedControl.IDataSource) this;
    }

    public virtual void SetData(IconSegmentedControl.DataItem[] dataItems)
    {
      this.Init();
      this._dataItems = dataItems;
      this.ReloadData();
    }

    public virtual int NumberOfCells() => this._dataItems == null ? 0 : this._dataItems.Length;

    public virtual SegmentedControlCell CellForCellNumber(int cellNumber)
    {
      IconSegmentedControlCell segmentedControlCell = this._dataItems.Length != 1 ? (cellNumber != 0 ? (cellNumber != this._dataItems.Length - 1 ? this.InstantiateCell((Object) this._middleCellPrefab) : this.InstantiateCell((Object) this._lastCellPrefab)) : this.InstantiateCell((Object) this._firstCellPrefab)) : this.InstantiateCell((Object) this._singleCellPrefab);
      segmentedControlCell.sprite = this._dataItems[cellNumber].icon;
      segmentedControlCell.hintText = this._dataItems[cellNumber].hintText;
      segmentedControlCell.hideBackgroundImage = this._hideCellBackground;
      if (this._overrideCellSize)
      {
        RectTransform transform = (RectTransform) segmentedControlCell.transform;
        segmentedControlCell.iconSize = this._iconSize;
        transform.sizeDelta = new Vector2(this._iconSize + 2f * this._padding, this._iconSize + 2f * this._padding);
      }
      return (SegmentedControlCell) segmentedControlCell;
    }

    public virtual IconSegmentedControlCell InstantiateCell(Object prefab)
    {
      GameObject gameObject = this._container.InstantiatePrefab(prefab, Vector3.zero, Quaternion.identity, this.transform);
      gameObject.transform.localScale = Vector3.one;
      return gameObject.GetComponent<IconSegmentedControlCell>();
    }

    public class DataItem
    {
      [CompilerGenerated]
      protected Sprite icon_k__BackingField;
      [CompilerGenerated]
      protected string hintText_k__BackingField;

      public Sprite icon
      {
        get => this.icon_k__BackingField;
        private set => this.icon_k__BackingField = value;
      }

      public string hintText
      {
        get => this.hintText_k__BackingField;
        private set => this.hintText_k__BackingField = value;
      }

      public DataItem(Sprite icon, string hintText)
      {
        this.icon = icon;
        this.hintText = hintText;
      }
    }
  }
}
