// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.ArcObjectsMouseInputSource
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.InputSignals;
using System;
using UnityEngine;

namespace BeatmapEditor3D
{
  public class ArcObjectsMouseInputSource : MonoBehaviour
  {
    public event Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)> objectPointerDownEvent;

    public event Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)> objectPointerUpEvent;

    public event Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)> objectPointerHoverEvent;

    public event Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)> arcObjectHeadMoveControlPointUpEvent;

    public event Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)> arcObjectHeadMoveControlPointDownEvent;

    public event Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)> arcObjectTailMoveControlPointUpEvent;

    public event Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)> arcObjectTailMoveControlPointDownEvent;

    public event Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData, NoteCutDirection, float, Vector3, bool)> arcObjectControlPointChangeEvent;

    public event Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData, SliderMidAnchorMode, Vector3)> arcObjectMidControlPointDownEvent;

    public void SubscribeToMouseEvents(
      BeatmapEditorObjectId id,
      BeatmapObjectCellData cellData,
      BeatmapObjectCellData tailCellData,
      Component component)
    {
      ArcMouseInputProvider component1 = component.GetComponent<ArcMouseInputProvider>();
      component1.Initialize(id, cellData, tailCellData);
      component1.pointerDownEvent += new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType>(this.HandleBeatmapObjectMouseInputControllerPointerDown);
      component1.pointerUpEvent += new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType>(this.HandleBeatmapObjectMouseInputControllerPointerUp);
      component1.pointerHoverEvent += new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType>(this.HandleBeatmapObjectMouseInputControllerPointerHover);
      component1.headControlPointHandlePointerDownEvent += new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType, NoteCutDirection, float, Vector3, bool>(this.HandleArcMouseInputControllerControlPointHandlePointHandlePointerDown);
      component1.headControlPointerUpEvent += new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType>(this.HandleArcMouseInputControllerHeadControlPointerUp);
      component1.headControlPointerDownEvent += new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType>(this.HandleArcMouseInputControllerHeadControlPointerDown);
      component1.tailControlPointHandlePointerDownEvent += new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType, NoteCutDirection, float, Vector3, bool>(this.HandleArcMouseInputControllerControlPointHandlePointHandlePointerDown);
      component1.tailControlPointerUpEvent += new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType>(this.HandleArcMouseInputControllerTailControlPointerUp);
      component1.tailControlPointerDownEvent += new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType>(this.HandleArcMouseInputControllerTailControlPointerDown);
      component1.midControlPointerDownEvent += new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType, SliderMidAnchorMode, Vector3>(this.HandleArcMouseInputControllerMidControlPointerDown);
    }

    public void UnsubscribeFromMouseEvents(Component component)
    {
      ArcMouseInputProvider component1 = component.GetComponent<ArcMouseInputProvider>();
      component1.DeInitialize();
      component1.pointerDownEvent -= new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType>(this.HandleBeatmapObjectMouseInputControllerPointerDown);
      component1.pointerUpEvent -= new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType>(this.HandleBeatmapObjectMouseInputControllerPointerUp);
      component1.pointerHoverEvent -= new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType>(this.HandleBeatmapObjectMouseInputControllerPointerHover);
      component1.headControlPointHandlePointerDownEvent -= new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType, NoteCutDirection, float, Vector3, bool>(this.HandleArcMouseInputControllerControlPointHandlePointHandlePointerDown);
      component1.headControlPointerUpEvent -= new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType>(this.HandleArcMouseInputControllerHeadControlPointerUp);
      component1.headControlPointerDownEvent -= new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType>(this.HandleArcMouseInputControllerHeadControlPointerDown);
      component1.tailControlPointHandlePointerDownEvent -= new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType, NoteCutDirection, float, Vector3, bool>(this.HandleArcMouseInputControllerControlPointHandlePointHandlePointerDown);
      component1.tailControlPointerUpEvent -= new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType>(this.HandleArcMouseInputControllerTailControlPointerUp);
      component1.tailControlPointerDownEvent -= new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType>(this.HandleArcMouseInputControllerTailControlPointerDown);
      component1.midControlPointerDownEvent -= new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType, SliderMidAnchorMode, Vector3>(this.HandleArcMouseInputControllerMidControlPointerDown);
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

    private void HandleArcMouseInputControllerControlPointHandlePointHandlePointerDown(
      BeatmapEditorObjectId id,
      BeatmapObjectCellData cellData,
      MouseInputType mouseInputType,
      NoteCutDirection cutDirection,
      float controlPointLength,
      Vector3 worldSpaceOrigin,
      bool flipDirection)
    {
      Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData, NoteCutDirection, float, Vector3, bool)> pointChangeEvent = this.arcObjectControlPointChangeEvent;
      if (pointChangeEvent == null)
        return;
      pointChangeEvent(mouseInputType, (id, cellData, cutDirection, controlPointLength, worldSpaceOrigin, flipDirection));
    }

    private void HandleArcMouseInputControllerHeadControlPointerUp(
      BeatmapEditorObjectId id,
      BeatmapObjectCellData cellData,
      MouseInputType mouseInputType)
    {
      Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)> controlPointUpEvent = this.arcObjectHeadMoveControlPointUpEvent;
      if (controlPointUpEvent == null)
        return;
      controlPointUpEvent(mouseInputType, (id, cellData));
    }

    private void HandleArcMouseInputControllerHeadControlPointerDown(
      BeatmapEditorObjectId id,
      BeatmapObjectCellData cellData,
      MouseInputType mouseInputType)
    {
      Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)> controlPointDownEvent = this.arcObjectHeadMoveControlPointDownEvent;
      if (controlPointDownEvent == null)
        return;
      controlPointDownEvent(mouseInputType, (id, cellData));
    }

    private void HandleArcMouseInputControllerMidControlPointerDown(
      BeatmapEditorObjectId id,
      BeatmapObjectCellData cellData,
      MouseInputType mouseInputType,
      SliderMidAnchorMode midAnchorMode,
      Vector3 worldSpaceOrigin)
    {
      Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData, SliderMidAnchorMode, Vector3)> controlPointDownEvent = this.arcObjectMidControlPointDownEvent;
      if (controlPointDownEvent == null)
        return;
      controlPointDownEvent(mouseInputType, (id, cellData, midAnchorMode, worldSpaceOrigin));
    }

    private void HandleArcMouseInputControllerTailControlPointerUp(
      BeatmapEditorObjectId id,
      BeatmapObjectCellData cellData,
      MouseInputType mouseInputType)
    {
      Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)> controlPointUpEvent = this.arcObjectTailMoveControlPointUpEvent;
      if (controlPointUpEvent == null)
        return;
      controlPointUpEvent(mouseInputType, (id, cellData));
    }

    private void HandleArcMouseInputControllerTailControlPointerDown(
      BeatmapEditorObjectId id,
      BeatmapObjectCellData cellData,
      MouseInputType mouseInputType)
    {
      Action<MouseInputType, (BeatmapEditorObjectId, BeatmapObjectCellData)> controlPointDownEvent = this.arcObjectTailMoveControlPointDownEvent;
      if (controlPointDownEvent == null)
        return;
      controlPointDownEvent(mouseInputType, (id, cellData));
    }
  }
}
