// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.EventObjectMouseInputController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.InputSignals;
using System;
using UnityEngine;

namespace BeatmapEditor3D
{
  public class EventObjectMouseInputController : MonoBehaviour
  {
    private BeatmapEditorObjectId _id;
    private int _initializedFrame;

    public event Action<BeatmapEditorObjectId> pointerEnterEvent;

    public event Action<BeatmapEditorObjectId> pointerExitEvent;

    public event Action<BeatmapEditorObjectId, MouseInputType> pointerDownEvent;

    public event Action<BeatmapEditorObjectId, MouseInputType> pointerUpEvent;

    public event Action<BeatmapEditorObjectId, MouseInputType> pointerScrollEvent;

    public void Initialize(BeatmapEditorObjectId id)
    {
      this._id = id;
      this._initializedFrame = Time.frameCount;
    }

    public bool WasInitializedThisFrame() => this._initializedFrame == Time.frameCount;

    public void PointerEnter()
    {
      Action<BeatmapEditorObjectId> pointerEnterEvent = this.pointerEnterEvent;
      if (pointerEnterEvent == null)
        return;
      pointerEnterEvent(this._id);
    }

    public void PointerExit()
    {
      Action<BeatmapEditorObjectId> pointerExitEvent = this.pointerExitEvent;
      if (pointerExitEvent == null)
        return;
      pointerExitEvent(this._id);
    }

    public void PointerDown(MouseInputType type)
    {
      Action<BeatmapEditorObjectId, MouseInputType> pointerDownEvent = this.pointerDownEvent;
      if (pointerDownEvent == null)
        return;
      pointerDownEvent(this._id, type);
    }

    public void PointerUp(MouseInputType type)
    {
      Action<BeatmapEditorObjectId, MouseInputType> pointerUpEvent = this.pointerUpEvent;
      if (pointerUpEvent == null)
        return;
      pointerUpEvent(this._id, type);
    }

    public void PointerScroll(MouseInputType type)
    {
      Action<BeatmapEditorObjectId, MouseInputType> pointerScrollEvent = this.pointerScrollEvent;
      if (pointerScrollEvent == null)
        return;
      pointerScrollEvent(this._id, type);
    }
  }
}
