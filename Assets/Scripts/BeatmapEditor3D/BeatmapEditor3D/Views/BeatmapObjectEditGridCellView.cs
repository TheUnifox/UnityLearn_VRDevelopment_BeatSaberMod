// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.BeatmapObjectEditGridCellView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BeatmapEditor3D.Views
{
  public class BeatmapObjectEditGridCellView : 
    MonoBehaviour,
    IPointerDownHandler,
    IEventSystemHandler,
    IPointerUpHandler,
    IPointerEnterHandler,
    IPointerExitHandler
  {
    public Image _image;
    public int _column;
    public int _row;

    public event Action<int, int> pointerDownEvent;

    public event Action<int, int> pointerUpEvent;

    public event Action<int, int> pointerEnterEvent;

    public event Action<int, int> pointerExitEvent;

    private void Start() => this.SetHighlighted(false);

    public void OnPointerDown(PointerEventData eventData)
    {
      if (eventData.pointerId != -1)
        return;
      Action<int, int> pointerDownEvent = this.pointerDownEvent;
      if (pointerDownEvent == null)
        return;
      pointerDownEvent(this._column, this._row);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
      if (eventData.pointerId != -1)
        return;
      Action<int, int> pointerUpEvent = this.pointerUpEvent;
      if (pointerUpEvent == null)
        return;
      pointerUpEvent(this._column, this._row);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
      Action<int, int> pointerEnterEvent = this.pointerEnterEvent;
      if (pointerEnterEvent == null)
        return;
      pointerEnterEvent(this._column, this._row);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
      Action<int, int> pointerExitEvent = this.pointerExitEvent;
      if (pointerExitEvent == null)
        return;
      pointerExitEvent(this._column, this._row);
    }

    public void SetHighlighted(bool highlighted) => this._image.color = this._image.color with
    {
      a = highlighted ? 0.2f : 0.02f
    };
  }
}
