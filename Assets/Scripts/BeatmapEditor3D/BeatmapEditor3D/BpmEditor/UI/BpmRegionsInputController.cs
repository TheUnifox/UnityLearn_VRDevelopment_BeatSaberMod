// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BpmEditor.UI.BpmRegionsInputController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BeatmapEditor3D.BpmEditor.UI
{
  public class BpmRegionsInputController : 
    MonoBehaviour,
    IPointerEnterHandler,
    IEventSystemHandler,
    IPointerExitHandler,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler,
    IPointerDownHandler,
    IPointerUpHandler,
    IPointerClickHandler
  {
    [SerializeField]
    private RectTransform _containerTransform;
    private bool _mouseOver;

    public event Action<int> mouseEnterEvent;

    public event Action<int> mouseOverEvent;

    public event Action<int> mouseExitEvent;

    public event Action<int> mouseBeginDragEvent;

    public event Action<int> mouseDragEvent;

    public event Action<int> mouseEndDragEvent;

    public event Action<int> mouseDownEvent;

    public event Action<int> mouseUpEvent;

    public event Action<int> mouseClickEvent;

    public RectTransform containerTransform => this._containerTransform;

    public int startSample { get; private set; }

    public int endSample { get; private set; }

    protected void Update()
    {
      if (!this._mouseOver)
        return;
      this.InvokeAction((Vector2) Input.mousePosition, this.mouseOverEvent);
    }

    public void SetBounds(int startSample, int endSample)
    {
      this.startSample = startSample;
      this.endSample = endSample;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
      this._mouseOver = true;
      if (eventData.button != PointerEventData.InputButton.Left)
        return;
      this.InvokeAction(eventData.position, this.mouseEnterEvent);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
      this._mouseOver = false;
      if (eventData.button != PointerEventData.InputButton.Left)
        return;
      this.InvokeAction(eventData.position, this.mouseExitEvent);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
      if (eventData.button != PointerEventData.InputButton.Left)
        return;
      this.InvokeAction(eventData.position, this.mouseBeginDragEvent);
    }

    public void OnDrag(PointerEventData eventData)
    {
      if (eventData.button != PointerEventData.InputButton.Left)
        return;
      this.InvokeAction(eventData.position, this.mouseDragEvent);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
      if (eventData.button != PointerEventData.InputButton.Left)
        return;
      this.InvokeAction(eventData.position, this.mouseEndDragEvent);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
      if (eventData.button != PointerEventData.InputButton.Left)
        return;
      this.InvokeAction(eventData.position, this.mouseDownEvent);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
      if (eventData.button != PointerEventData.InputButton.Left)
        return;
      this.InvokeAction(eventData.position, this.mouseUpEvent);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
      if (eventData.button != PointerEventData.InputButton.Left)
        return;
      this.InvokeAction(eventData.position, this.mouseClickEvent);
    }

    private void InvokeAction(Vector2 screenPosition, Action<int> action)
    {
      int sample = this.ScreenPositionToSample(screenPosition);
      if (action == null)
        return;
      action(sample);
    }

    private int ScreenPositionToSample(Vector2 screenPosition)
    {
      Vector2 localPoint;
      RectTransformUtility.ScreenPointToLocalPointInRectangle(this._containerTransform, screenPosition, (Camera) null, out localPoint);
      Rect rect = this._containerTransform.rect;
      return (int) Mathf.Lerp((float) this.startSample, (float) this.endSample, Mathf.InverseLerp(rect.x, rect.x + rect.width, localPoint.x));
    }
  }
}
