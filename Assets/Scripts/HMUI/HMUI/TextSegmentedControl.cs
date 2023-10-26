// Decompiled with JetBrains decompiler
// Type: HMUI.TextSegmentedControl
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace HMUI
{
  public class TextSegmentedControl : SegmentedControl, SegmentedControl.IDataSource
  {
    [SerializeField]
    protected float _fontSize = 4f;
    [SerializeField]
    protected bool _overrideCellSize;
    [DrawIf("_overrideCellSize", true, DrawIfAttribute.DisablingType.DontDraw)]
    [SerializeField]
    protected float _padding = 4f;
    [SerializeField]
    protected bool _hideCellBackground;
    [Space]
    [SerializeField]
    protected TextSegmentedControlCell _firstCellPrefab;
    [SerializeField]
    protected TextSegmentedControlCell _lastCellPrefab;
    [SerializeField]
    protected TextSegmentedControlCell _singleCellPrefab;
    [SerializeField]
    protected TextSegmentedControlCell _middleCellPrefab;
    [Inject]
    protected readonly DiContainer _container;
    protected IReadOnlyList<string> _texts;

    public virtual void SetTexts(IReadOnlyList<string> texts)
    {
      this._texts = texts;
      if (this.dataSource == null)
        this.dataSource = (SegmentedControl.IDataSource) this;
      else
        this.ReloadData();
    }

    public virtual int NumberOfCells() => this._texts == null ? 0 : ((IReadOnlyCollection<string>) this._texts).Count;

    public virtual SegmentedControlCell CellForCellNumber(int cellNumber)
    {
      TextSegmentedControlCell segmentedControlCell = ((IReadOnlyCollection<string>) this._texts).Count != 1 ? (cellNumber != 0 ? (cellNumber != ((IReadOnlyCollection<string>) this._texts).Count - 1 ? this.InstantiateCell((Object) this._middleCellPrefab) : this.InstantiateCell((Object) this._lastCellPrefab)) : this.InstantiateCell((Object) this._firstCellPrefab)) : this.InstantiateCell((Object) this._singleCellPrefab);
      segmentedControlCell.fontSize = this._fontSize;
      segmentedControlCell.text = this._texts[cellNumber];
      segmentedControlCell.hideBackgroundImage = this._hideCellBackground;
      if (this._overrideCellSize)
        ((RectTransform) segmentedControlCell.transform).sizeDelta = new Vector2(segmentedControlCell.preferredWidth + 2f * this._padding, 1f);
      return (SegmentedControlCell) segmentedControlCell;
    }

    public virtual TextSegmentedControlCell InstantiateCell(Object prefab)
    {
      GameObject gameObject = this._container.InstantiatePrefab(prefab, Vector3.zero, Quaternion.identity, this.transform);
      gameObject.transform.localScale = Vector3.one;
      return gameObject.GetComponent<TextSegmentedControlCell>();
    }
  }
}
