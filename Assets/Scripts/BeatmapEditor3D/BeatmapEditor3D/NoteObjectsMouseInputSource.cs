// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.NoteObjectsMouseInputSource
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.InputSignals;
using System;
using UnityEngine;

namespace BeatmapEditor3D
{
  public class NoteObjectsMouseInputSource : MonoBehaviour
  {
    public event Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)> objectPointerDownEvent;

    public event Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)> objectPointerUpEvent;

    public event Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)> objectPointerHoverEvent;

    public event Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)> objectPointerScrollEvent;

    public void SubscribeToMouseEvents(
      BeatmapEditorObjectId id,
      BeatmapObjectCellData cellData,
      Component component)
    {
      BeatmapObjectMouseInputProvider component1 = component.GetComponent<BeatmapObjectMouseInputProvider>();
      component1.Initialize(id, cellData);
      component1.pointerDownEvent += new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType>(this.HandleBeatmapObjectMouseInputControllerPointerDown);
      component1.pointerUpEvent += new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType>(this.HandleBeatmapObjectMouseInputControllerPointerUp);
      component1.pointerHoverEvent += new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType>(this.HandleBeatmapObjectMouseInputControllerPointerHover);
      component1.pointerScrollEvent += new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType>(this.HandleBeatmapObjectMouseInputControllerPointerScroll);
    }

    public void UnsubscribeFromMouseEvents(Component component)
    {
      BeatmapObjectMouseInputProvider component1 = component.GetComponent<BeatmapObjectMouseInputProvider>();
      component1.pointerDownEvent -= new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType>(this.HandleBeatmapObjectMouseInputControllerPointerDown);
      component1.pointerUpEvent -= new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType>(this.HandleBeatmapObjectMouseInputControllerPointerUp);
      component1.pointerHoverEvent -= new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType>(this.HandleBeatmapObjectMouseInputControllerPointerHover);
      component1.pointerScrollEvent -= new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType>(this.HandleBeatmapObjectMouseInputControllerPointerScroll);
    }

    private void HandleBeatmapObjectMouseInputControllerPointerDown(
      BeatmapEditorObjectId id,
      BeatmapObjectCellData cellData,
      MouseInputType mouseInputType)
    {
      Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)> pointerDownEvent = this.objectPointerDownEvent;
      if (pointerDownEvent == null)
        return;
      pointerDownEvent(mouseInputType, (id, cellData));
    }

    private void HandleBeatmapObjectMouseInputControllerPointerUp(
      BeatmapEditorObjectId id,
      BeatmapObjectCellData cellData,
      MouseInputType mouseInputType)
    {
      Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)> objectPointerUpEvent = this.objectPointerUpEvent;
      if (objectPointerUpEvent == null)
        return;
      objectPointerUpEvent(mouseInputType, (id, cellData));
    }

    private void HandleBeatmapObjectMouseInputControllerPointerHover(
      BeatmapEditorObjectId id,
      BeatmapObjectCellData cellData,
      MouseInputType mouseInputType)
    {
      Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)> pointerHoverEvent = this.objectPointerHoverEvent;
      if (pointerHoverEvent == null)
        return;
      pointerHoverEvent(mouseInputType, (id, cellData));
    }

    private void HandleBeatmapObjectMouseInputControllerPointerScroll(
      BeatmapEditorObjectId id,
      BeatmapObjectCellData cellData,
      MouseInputType mouseInputType)
    {
      Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)> pointerScrollEvent = this.objectPointerScrollEvent;
      if (pointerScrollEvent == null)
        return;
      pointerScrollEvent(mouseInputType, (id, cellData));
    }
  }
}
