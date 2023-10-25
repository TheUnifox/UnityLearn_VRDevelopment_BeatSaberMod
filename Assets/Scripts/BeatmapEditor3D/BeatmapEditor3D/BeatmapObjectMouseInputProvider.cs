// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapObjectMouseInputProvider
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.InputSignals;
using System;
using UnityEngine;

namespace BeatmapEditor3D
{
  public class BeatmapObjectMouseInputProvider : MonoBehaviour
  {
    private int _initializedFrame;
    private BeatmapEditorObjectId _id;
    private BeatmapObjectCellData _cellData;

    public event Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType> pointerHoverEvent;

    public event Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType> pointerDownEvent;

    public event Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType> pointerUpEvent;

    public event Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType> pointerScrollEvent;

    public virtual void Initialize(BeatmapEditorObjectId id, BeatmapObjectCellData cellData)
    {
      this._initializedFrame = Time.frameCount;
      this._id = id;
      this._cellData = cellData;
    }

    public bool WasInitializedThisFrame() => this._initializedFrame == Time.frameCount;

    public void PointerHover(MouseInputType type)
    {
      Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType> pointerHoverEvent = this.pointerHoverEvent;
      if (pointerHoverEvent == null)
        return;
      pointerHoverEvent(this._id, this._cellData, type);
    }

    public void PointerDown(MouseInputType type)
    {
      Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType> pointerDownEvent = this.pointerDownEvent;
      if (pointerDownEvent == null)
        return;
      pointerDownEvent(this._id, this._cellData, type);
    }

    public void PointerUp(MouseInputType type)
    {
      Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType> pointerUpEvent = this.pointerUpEvent;
      if (pointerUpEvent == null)
        return;
      pointerUpEvent(this._id, this._cellData, type);
    }

    public void PointerScroll(MouseInputType type)
    {
      Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType> pointerScrollEvent = this.pointerScrollEvent;
      if (pointerScrollEvent == null)
        return;
      pointerScrollEvent(this._id, this._cellData, type);
    }

    protected void TriggerPointerDown(
      MouseInputType type,
      BeatmapEditorObjectId id,
      BeatmapObjectCellData cellData)
    {
      Action<BeatmapEditorObjectId, BeatmapObjectCellData, MouseInputType> pointerDownEvent = this.pointerDownEvent;
      if (pointerDownEvent == null)
        return;
      pointerDownEvent(id, cellData, type);
    }
  }
}
