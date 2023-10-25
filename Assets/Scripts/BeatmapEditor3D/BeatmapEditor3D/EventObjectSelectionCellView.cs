// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.EventObjectSelectionCellView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace BeatmapEditor3D
{
  public class EventObjectSelectionCellView : 
    MonoBehaviour,
    IPointerDownHandler,
    IEventSystemHandler,
    IPointerUpHandler,
    IPointerEnterHandler,
    IPointerExitHandler
  {
    [SerializeField]
    private Image _image;
    private int _index;

    public event Action<int> pointerDownEvent;

    public event Action<int> pointerUpEvent;

    public event Action<int> pointerEnterEvent;

    public event Action<int> pointerExitEvent;

    protected void Start() => this.SetHighlighted(false);

    public void OnPointerDown(PointerEventData eventData)
    {
      if (eventData.pointerId != -1)
        return;
      Action<int> pointerDownEvent = this.pointerDownEvent;
      if (pointerDownEvent == null)
        return;
      pointerDownEvent(this._index);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
      if (eventData.pointerId != -1)
        return;
      Action<int> pointerUpEvent = this.pointerUpEvent;
      if (pointerUpEvent == null)
        return;
      pointerUpEvent(this._index);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
      Action<int> pointerEnterEvent = this.pointerEnterEvent;
      if (pointerEnterEvent == null)
        return;
      pointerEnterEvent(this._index);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
      Action<int> pointerExitEvent = this.pointerExitEvent;
      if (pointerExitEvent == null)
        return;
      pointerExitEvent(this._index);
    }

    public void SetHighlighted(bool highlighted) => this._image.color = this._image.color with
    {
      a = highlighted ? 0.2f : 0.02f
    };

    private void Reinitialize(int index) => this._index = index;

    public class Pool : MonoMemoryPool<int, EventObjectSelectionCellView>
    {
      protected override void Reinitialize(int index, EventObjectSelectionCellView item) => item.Reinitialize(index);
    }
  }
}
