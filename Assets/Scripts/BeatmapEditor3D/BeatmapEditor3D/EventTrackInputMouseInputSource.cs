// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.EventTrackInputMouseInputSource
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.InputSignals;
using System;
using UnityEngine;

namespace BeatmapEditor3D
{
  public class EventTrackInputMouseInputSource : MonoBehaviour
  {
    [SerializeField]
    private EventObjectSelectionCellView _eventObjectSelectionCellView;

    public event Action<MouseInputType> gridPointerDownEvent;

    public event Action<MouseInputType> gridPointerUpEvent;

    public event Action<MouseInputType> gridPointerHoverEvent;

    public event Action<MouseInputType, BeatmapEditorObjectId> objectPointerDownEvent;

    public event Action<MouseInputType, BeatmapEditorObjectId> objectPointerUpEvent;

    public event Action<MouseInputType, BeatmapEditorObjectId> objectPointerHoverEvent;

    public event Action<MouseInputType, BeatmapEditorObjectId> objectPointerScrollEvent;

    public void Initialize() => this._eventObjectSelectionCellView.SetHighlighted(false);

    public void SubscribeToMouseEvents(BeatmapEditorObjectId id, Component component)
    {
      EventObjectMouseInputController component1 = component.GetComponent<EventObjectMouseInputController>();
      component1.Initialize(id);
      component1.pointerEnterEvent += new Action<BeatmapEditorObjectId>(this.HandleEventObjectMouseInputControllerPointerEnter);
      component1.pointerExitEvent += new Action<BeatmapEditorObjectId>(this.HandleEventObjectMouseInputControllerPointerExit);
      component1.pointerDownEvent += new Action<BeatmapEditorObjectId, MouseInputType>(this.HandleEventObjectMouseInputControllerPointerDown);
      component1.pointerUpEvent += new Action<BeatmapEditorObjectId, MouseInputType>(this.HandleEventObjectMouseInputControllerPointerUp);
      component1.pointerScrollEvent += new Action<BeatmapEditorObjectId, MouseInputType>(this.HandleEventObjectMouseInputControllerPointerScroll);
    }

    public void UnsubscribeFromMouseEvents(Component component)
    {
      EventObjectMouseInputController component1 = component.GetComponent<EventObjectMouseInputController>();
      component1.pointerEnterEvent -= new Action<BeatmapEditorObjectId>(this.HandleEventObjectMouseInputControllerPointerEnter);
      component1.pointerExitEvent -= new Action<BeatmapEditorObjectId>(this.HandleEventObjectMouseInputControllerPointerExit);
      component1.pointerDownEvent -= new Action<BeatmapEditorObjectId, MouseInputType>(this.HandleEventObjectMouseInputControllerPointerDown);
      component1.pointerUpEvent -= new Action<BeatmapEditorObjectId, MouseInputType>(this.HandleEventObjectMouseInputControllerPointerUp);
      component1.pointerScrollEvent -= new Action<BeatmapEditorObjectId, MouseInputType>(this.HandleEventObjectMouseInputControllerPointerScroll);
    }

    protected void OnEnable()
    {
      this._eventObjectSelectionCellView.pointerEnterEvent += new Action<int>(this.HandleEventObjectSelectionCellViewPointerEnter);
      this._eventObjectSelectionCellView.pointerExitEvent += new Action<int>(this.HandleEventObjectSelectionCellViewPointerExit);
      this._eventObjectSelectionCellView.pointerDownEvent += new Action<int>(this.HandleEventObjectSelectionCellViewPointerDown);
      this._eventObjectSelectionCellView.pointerUpEvent += new Action<int>(this.HandleEventObjectSelectionCellViewPointerUp);
    }

    protected void OnDisable()
    {
      this._eventObjectSelectionCellView.pointerEnterEvent -= new Action<int>(this.HandleEventObjectSelectionCellViewPointerEnter);
      this._eventObjectSelectionCellView.pointerExitEvent -= new Action<int>(this.HandleEventObjectSelectionCellViewPointerExit);
      this._eventObjectSelectionCellView.pointerDownEvent -= new Action<int>(this.HandleEventObjectSelectionCellViewPointerDown);
      this._eventObjectSelectionCellView.pointerUpEvent -= new Action<int>(this.HandleEventObjectSelectionCellViewPointerUp);
    }

    private void HandleEventObjectSelectionCellViewPointerEnter(int _)
    {
      this._eventObjectSelectionCellView.SetHighlighted(true);
      Action<MouseInputType> pointerHoverEvent = this.gridPointerHoverEvent;
      if (pointerHoverEvent == null)
        return;
      pointerHoverEvent(MouseInputType.Enter);
    }

    private void HandleEventObjectSelectionCellViewPointerExit(int _)
    {
      this._eventObjectSelectionCellView.SetHighlighted(false);
      Action<MouseInputType> pointerHoverEvent = this.gridPointerHoverEvent;
      if (pointerHoverEvent == null)
        return;
      pointerHoverEvent(MouseInputType.Exit);
    }

    private void HandleEventObjectSelectionCellViewPointerDown(int _)
    {
      Action<MouseInputType> pointerDownEvent = this.gridPointerDownEvent;
      if (pointerDownEvent == null)
        return;
      pointerDownEvent(MouseInputType.Left);
    }

    private void HandleEventObjectSelectionCellViewPointerUp(int _)
    {
      Action<MouseInputType> gridPointerUpEvent = this.gridPointerUpEvent;
      if (gridPointerUpEvent == null)
        return;
      gridPointerUpEvent(MouseInputType.Left);
    }

    private void HandleEventObjectMouseInputControllerPointerEnter(
      BeatmapEditorObjectId beatmapEditorObjectId)
    {
      Action<MouseInputType, BeatmapEditorObjectId> pointerHoverEvent = this.objectPointerHoverEvent;
      if (pointerHoverEvent == null)
        return;
      pointerHoverEvent(MouseInputType.Enter, beatmapEditorObjectId);
    }

    private void HandleEventObjectMouseInputControllerPointerExit(
      BeatmapEditorObjectId beatmapEditorObjectId)
    {
      Action<MouseInputType, BeatmapEditorObjectId> pointerHoverEvent = this.objectPointerHoverEvent;
      if (pointerHoverEvent == null)
        return;
      pointerHoverEvent(MouseInputType.Exit, beatmapEditorObjectId);
    }

    private void HandleEventObjectMouseInputControllerPointerDown(
      BeatmapEditorObjectId beatmapEditorObjectId,
      MouseInputType mouseInputType)
    {
      Action<MouseInputType, BeatmapEditorObjectId> pointerDownEvent = this.objectPointerDownEvent;
      if (pointerDownEvent == null)
        return;
      pointerDownEvent(mouseInputType, beatmapEditorObjectId);
    }

    private void HandleEventObjectMouseInputControllerPointerUp(
      BeatmapEditorObjectId beatmapEditorObjectId,
      MouseInputType mouseInputType)
    {
      Action<MouseInputType, BeatmapEditorObjectId> objectPointerUpEvent = this.objectPointerUpEvent;
      if (objectPointerUpEvent == null)
        return;
      objectPointerUpEvent(mouseInputType, beatmapEditorObjectId);
    }

    private void HandleEventObjectMouseInputControllerPointerScroll(
      BeatmapEditorObjectId beatmapEditorObjectId,
      MouseInputType mouseInputType)
    {
      Action<MouseInputType, BeatmapEditorObjectId> pointerScrollEvent = this.objectPointerScrollEvent;
      if (pointerScrollEvent == null)
        return;
      pointerScrollEvent(mouseInputType, beatmapEditorObjectId);
    }
  }
}
