// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.ArcMouseInputProvider
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.InputSignals;
using BeatmapEditor3D.Visuals;
using System;
using UnityEngine;

namespace BeatmapEditor3D
{
  public class ArcMouseInputProvider : BeatmapObjectMouseInputProvider
  {
    [SerializeField]
    private ArcView _arcView;
    [Header("Head")]
    [SerializeField]
    private BeatmapObjectMouseInputProvider _headMoveControlPointProvider;
    [SerializeField]
    private BeatmapObjectMouseInputProvider _headControlPointProvider;
    [SerializeField]
    private Transform _headControlPointOrigin;
    [Header("Tail")]
    [SerializeField]
    private BeatmapObjectMouseInputProvider _tailMoveControlPointProvider;
    [SerializeField]
    private BeatmapObjectMouseInputProvider _tailControlPointProvider;
    [SerializeField]
    private Transform _tailControlPointOrigin;
    [Header("Mid Point")]
    [SerializeField]
    private BeatmapObjectMouseInputProvider _midPointControlPointProvider;
    [SerializeField]
    private Transform _midPointOrigin;

    public event Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType, NoteCutDirection, float, Vector3, bool> headControlPointHandlePointerDownEvent;

    public event Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType> headControlPointerUpEvent;

    public event Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType> headControlPointerDownEvent;

    public event Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType> headControlPointerScrollEvent;

    public event Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType, NoteCutDirection, float, Vector3, bool> tailControlPointHandlePointerDownEvent;

    public event Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType> tailControlPointerUpEvent;

    public event Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType> tailControlPointerDownEvent;

    public event Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType> tailControlPointerScrollEvent;

    public event Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType, SliderMidAnchorMode, Vector3> midControlPointerDownEvent;

    public void Initialize(
      BeatmapEditorObjectId id,
      BeatmapObjectCellData cellData,
      BeatmapObjectCellData tailCellData)
    {
      this.Initialize(id, cellData);
      this._headMoveControlPointProvider.Initialize(id, cellData);
      this._headControlPointProvider.Initialize(id, cellData);
      this._tailMoveControlPointProvider.Initialize(id, tailCellData);
      this._tailControlPointProvider.Initialize(id, tailCellData);
      this._midPointControlPointProvider.Initialize(id, cellData);
      this._headMoveControlPointProvider.pointerDownEvent += new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType>(this.HandleHeadMoveControlPointProviderPointerDown);
      this._headMoveControlPointProvider.pointerUpEvent += new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType>(this.HandleHeadMoveControlPointProviderPointerUp);
      this._headControlPointProvider.pointerDownEvent += new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType>(this.HandleHeadControlPointProviderPointerDown);
      this._headControlPointProvider.pointerScrollEvent += new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType>(this.HandleHeadControlPointProviderPointerScroll);
      this._tailMoveControlPointProvider.pointerDownEvent += new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType>(this.HandleTailMoveControlPointProviderPointerDown);
      this._tailMoveControlPointProvider.pointerUpEvent += new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType>(this.HandleTailMoveControlPointProviderPointerUp);
      this._tailControlPointProvider.pointerDownEvent += new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType>(this.HandleTailControlPointProviderPointerDown);
      this._tailControlPointProvider.pointerScrollEvent += new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType>(this.HandleTailControlPointProviderPointerScroll);
      this._midPointControlPointProvider.pointerDownEvent += new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType>(this.HandleMidPointControlPointProviderPointerDown);
    }

    public void DeInitialize()
    {
      this._headMoveControlPointProvider.pointerDownEvent -= new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType>(this.HandleHeadMoveControlPointProviderPointerDown);
      this._headMoveControlPointProvider.pointerUpEvent -= new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType>(this.HandleHeadMoveControlPointProviderPointerUp);
      this._headControlPointProvider.pointerDownEvent -= new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType>(this.HandleHeadControlPointProviderPointerDown);
      this._headControlPointProvider.pointerScrollEvent -= new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType>(this.HandleHeadControlPointProviderPointerScroll);
      this._tailMoveControlPointProvider.pointerDownEvent -= new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType>(this.HandleTailMoveControlPointProviderPointerDown);
      this._tailMoveControlPointProvider.pointerUpEvent -= new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType>(this.HandleTailMoveControlPointProviderPointerUp);
      this._tailControlPointProvider.pointerDownEvent -= new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType>(this.HandleTailControlPointProviderPointerDown);
      this._tailControlPointProvider.pointerScrollEvent -= new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType>(this.HandleTailControlPointProviderPointerScroll);
      this._midPointControlPointProvider.pointerDownEvent -= new Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType>(this.HandleMidPointControlPointProviderPointerDown);
    }

    private void HandleHeadMoveControlPointProviderPointerDown(
      BeatmapEditorObjectId id,
      BeatmapObjectCellData cellData,
      MouseInputType mouseInputType)
    {
      Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType> pointerDownEvent = this.headControlPointerDownEvent;
      if (pointerDownEvent != null)
        pointerDownEvent(id, cellData, mouseInputType);
      this.TriggerPointerDown(mouseInputType, id, cellData);
    }

    private void HandleTailMoveControlPointProviderPointerDown(
      BeatmapEditorObjectId id,
      BeatmapObjectCellData cellData,
      MouseInputType mouseInputType)
    {
      Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType> pointerDownEvent = this.tailControlPointerDownEvent;
      if (pointerDownEvent != null)
        pointerDownEvent(id, cellData, mouseInputType);
      this.TriggerPointerDown(mouseInputType, id, cellData);
    }

    private void HandleHeadMoveControlPointProviderPointerUp(
      BeatmapEditorObjectId id,
      BeatmapObjectCellData cellData,
      MouseInputType mouseInputType)
    {
      Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType> controlPointerUpEvent = this.headControlPointerUpEvent;
      if (controlPointerUpEvent == null)
        return;
      controlPointerUpEvent(id, cellData, mouseInputType);
    }

    private void HandleTailMoveControlPointProviderPointerUp(
      BeatmapEditorObjectId id,
      BeatmapObjectCellData cellData,
      MouseInputType mouseInputType)
    {
      Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType> controlPointerUpEvent = this.tailControlPointerUpEvent;
      if (controlPointerUpEvent == null)
        return;
      controlPointerUpEvent(id, cellData, mouseInputType);
    }

    private void HandleHeadControlPointProviderPointerDown(
      BeatmapEditorObjectId id,
      BeatmapObjectCellData cellData,
      MouseInputType inputType)
    {
      Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType, NoteCutDirection, float, Vector3, bool> pointerDownEvent = this.headControlPointHandlePointerDownEvent;
      if (pointerDownEvent == null)
        return;
      pointerDownEvent(id, cellData, inputType, this._arcView.arcData.cutDirection, this._arcView.arcData.controlPointLengthMultiplier, this._headControlPointOrigin.position, false);
    }

    private void HandleHeadControlPointProviderPointerScroll(
      BeatmapEditorObjectId id,
      BeatmapObjectCellData cellData,
      MouseInputType inputType)
    {
      Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType> pointerScrollEvent = this.headControlPointerScrollEvent;
      if (pointerScrollEvent == null)
        return;
      pointerScrollEvent(id, cellData, inputType);
    }

    private void HandleTailControlPointProviderPointerDown(
      BeatmapEditorObjectId id,
      BeatmapObjectCellData cellData,
      MouseInputType inputType)
    {
      Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType, NoteCutDirection, float, Vector3, bool> pointerDownEvent = this.tailControlPointHandlePointerDownEvent;
      if (pointerDownEvent == null)
        return;
      pointerDownEvent(id, cellData, inputType, this._arcView.arcData.tailCutDirection, this._arcView.arcData.tailControlPointLengthMultiplier, this._tailControlPointOrigin.position, true);
    }

    private void HandleTailControlPointProviderPointerScroll(
      BeatmapEditorObjectId id,
      BeatmapObjectCellData cellData,
      MouseInputType inputType)
    {
      Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType> pointerScrollEvent = this.tailControlPointerScrollEvent;
      if (pointerScrollEvent == null)
        return;
      pointerScrollEvent(id, cellData, inputType);
    }

    private void HandleMidPointControlPointProviderPointerDown(
      BeatmapEditorObjectId id,
      BeatmapObjectCellData cellData,
      MouseInputType inputType)
    {
      Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType, SliderMidAnchorMode, Vector3> pointerDownEvent = this.midControlPointerDownEvent;
      if (pointerDownEvent == null)
        return;
      pointerDownEvent(id, cellData, inputType, this._arcView.arcData.midAnchorMode, this._midPointOrigin.position);
    }
  }
}
