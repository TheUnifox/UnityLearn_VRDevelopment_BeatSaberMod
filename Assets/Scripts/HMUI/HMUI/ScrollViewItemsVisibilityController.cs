// Decompiled with JetBrains decompiler
// Type: HMUI.ScrollViewItemsVisibilityController
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HMUI
{
  public class ScrollViewItemsVisibilityController : MonoBehaviour
  {
    [SerializeField]
    protected RectTransform _viewport;
    [SerializeField]
    protected RectTransform _contentRectTransform;
    protected ScrollViewItemForVisibilityController[] _items;
    protected float _lastContentAnchoredPositionY;
    protected Vector3[] _viewportWorldCorners = new Vector3[4];
    protected Tuple<ScrollViewItemForVisibilityController, float>[] _upperItemsCornes;
    protected Tuple<ScrollViewItemForVisibilityController, float>[] _lowerItemsCornes;
    protected int _lowerLastVisibleIndex;
    protected int _upperLastVisibleIndex;
    protected float _contentMaxY;
    protected float _contentMinY;

    public virtual void Start()
    {
      this._viewport.GetWorldCorners(this._viewportWorldCorners);
      float y1 = this._viewport.InverseTransformPoint(this._viewportWorldCorners[0]).y;
      float y2 = this._viewport.InverseTransformPoint(this._viewportWorldCorners[2]).y;
      this._contentMinY = Mathf.Min(y1, y2);
      this._contentMaxY = Mathf.Max(y1, y2);
      this._items = this.GetComponentsInChildren<ScrollViewItemForVisibilityController>(true);
      this._upperItemsCornes = new Tuple<ScrollViewItemForVisibilityController, float>[this._items.Length];
      this._lowerItemsCornes = new Tuple<ScrollViewItemForVisibilityController, float>[this._items.Length];
      Vector3[] fourCornersArray = new Vector3[4];
      for (int index = 0; index < this._items.Length; ++index)
      {
        this._items[index].GetWorldCorners(fourCornersArray);
        float a = this._viewport.InverseTransformPoint(fourCornersArray[0]).y - this._contentRectTransform.anchoredPosition.y;
        float b = this._viewport.InverseTransformPoint(fourCornersArray[2]).y - this._contentRectTransform.anchoredPosition.y;
        this._lowerItemsCornes[index] = Tuple.Create<ScrollViewItemForVisibilityController, float>(this._items[index], Mathf.Min(a, b));
        this._upperItemsCornes[index] = Tuple.Create<ScrollViewItemForVisibilityController, float>(this._items[index], Mathf.Max(a, b));
      }
      this._upperItemsCornes = ((IEnumerable<Tuple<ScrollViewItemForVisibilityController, float>>) this._upperItemsCornes).OrderBy<Tuple<ScrollViewItemForVisibilityController, float>, float>((Func<Tuple<ScrollViewItemForVisibilityController, float>, float>) (item => item.Item2)).ToArray<Tuple<ScrollViewItemForVisibilityController, float>>();
      this._lowerItemsCornes = ((IEnumerable<Tuple<ScrollViewItemForVisibilityController, float>>) this._lowerItemsCornes).OrderBy<Tuple<ScrollViewItemForVisibilityController, float>, float>((Func<Tuple<ScrollViewItemForVisibilityController, float>, float>) (item => item.Item2)).ToArray<Tuple<ScrollViewItemForVisibilityController, float>>();
      this._lowerLastVisibleIndex = this._items.Length - 1;
      this._upperLastVisibleIndex = 0;
      this.UpdateVisibilityUpDirection(0.0f);
    }

    public virtual void Update()
    {
      if ((double) Mathf.Abs(this._lastContentAnchoredPositionY - this._contentRectTransform.anchoredPosition.y) <= 1.0 / 1000.0)
        return;
      if ((double) this._lastContentAnchoredPositionY < (double) this._contentRectTransform.anchoredPosition.y)
        this.UpdateVisibilityDownDirection(this._contentRectTransform.anchoredPosition.y);
      else
        this.UpdateVisibilityUpDirection(this._contentRectTransform.anchoredPosition.y);
      this._lastContentAnchoredPositionY = this._contentRectTransform.anchoredPosition.y;
    }

    public virtual void UpdateVisibilityUpDirection(float newContentAnchoredPositionY)
    {
      int lastVisibleIndex1;
      for (lastVisibleIndex1 = this._upperLastVisibleIndex; lastVisibleIndex1 < this._lowerItemsCornes.Length; ++lastVisibleIndex1)
      {
        Tuple<ScrollViewItemForVisibilityController, float> lowerItemsCorne = this._lowerItemsCornes[lastVisibleIndex1];
        if ((double) lowerItemsCorne.Item2 + (double) newContentAnchoredPositionY < (double) this._contentMaxY)
          lowerItemsCorne.Item1.gameObject.SetActive(true);
        else
          break;
      }
      this._upperLastVisibleIndex = Math.Min(lastVisibleIndex1, this._lowerItemsCornes.Length - 1);
      int lastVisibleIndex2;
      for (lastVisibleIndex2 = this._lowerLastVisibleIndex; lastVisibleIndex2 < this._upperItemsCornes.Length; ++lastVisibleIndex2)
      {
        Tuple<ScrollViewItemForVisibilityController, float> upperItemsCorne = this._upperItemsCornes[lastVisibleIndex2];
        if ((double) upperItemsCorne.Item2 + (double) newContentAnchoredPositionY < (double) this._contentMinY)
          upperItemsCorne.Item1.gameObject.SetActive(false);
        else
          break;
      }
      this._lowerLastVisibleIndex = Math.Min(lastVisibleIndex2, this._upperItemsCornes.Length - 1);
    }

    public virtual void UpdateVisibilityDownDirection(float newContentAnchoredPositionY)
    {
      int lastVisibleIndex1;
      for (lastVisibleIndex1 = this._lowerLastVisibleIndex; lastVisibleIndex1 >= 0; --lastVisibleIndex1)
      {
        Tuple<ScrollViewItemForVisibilityController, float> upperItemsCorne = this._upperItemsCornes[lastVisibleIndex1];
        if ((double) upperItemsCorne.Item2 + (double) newContentAnchoredPositionY > (double) this._contentMinY)
          upperItemsCorne.Item1.gameObject.SetActive(true);
        else
          break;
      }
      this._lowerLastVisibleIndex = Math.Max(lastVisibleIndex1, 0);
      int lastVisibleIndex2;
      for (lastVisibleIndex2 = this._upperLastVisibleIndex; lastVisibleIndex2 >= 0; --lastVisibleIndex2)
      {
        Tuple<ScrollViewItemForVisibilityController, float> lowerItemsCorne = this._lowerItemsCornes[lastVisibleIndex2];
        if ((double) lowerItemsCorne.Item2 + (double) newContentAnchoredPositionY > (double) this._contentMaxY)
          lowerItemsCorne.Item1.gameObject.SetActive(false);
        else
          break;
      }
      this._upperLastVisibleIndex = Math.Max(lastVisibleIndex2, 0);
    }
  }
}
