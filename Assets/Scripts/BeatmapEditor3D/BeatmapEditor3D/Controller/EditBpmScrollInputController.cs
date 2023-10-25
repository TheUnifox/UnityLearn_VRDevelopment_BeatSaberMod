// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Controller.EditBpmScrollInputController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using HMUI;
using System;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace BeatmapEditor3D.Controller
{
  public class EditBpmScrollInputController : ITickable
  {
    [Inject]
    private readonly IBeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly ISongPreviewController _songPreviewController;
    private bool _enabled;
    private readonly BeatmapPreviewData _previewData = new BeatmapPreviewData();
    private readonly MouseBinder _mouseBinder = new MouseBinder();

    public event Action<float> changeSubdivisionEvent;

    public event Action<float> updatePlayHeadEvent;

    public event Action<float> zoomPreviewEvent;

    public void Enable()
    {
      if (this._enabled)
        return;
      this._enabled = true;
      this._previewData.Init(this._beatmapDataModel.audioClip.samples);
      this._mouseBinder.AddScrollBinding(new UnityAction<float>(this.HandleMouseScroll));
    }

    public void Disable()
    {
      if (!this._enabled)
        return;
      this._enabled = false;
      this._mouseBinder.ClearBindings();
    }

    public void Tick() => this._mouseBinder.ManualUpdate();

    private void HandleMouseScroll(float delta)
    {
      if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
      {
        Action<float> subdivisionEvent = this.changeSubdivisionEvent;
        if (subdivisionEvent == null)
          return;
        subdivisionEvent(delta);
      }
      else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
      {
        Action<float> zoomPreviewEvent = this.zoomPreviewEvent;
        if (zoomPreviewEvent == null)
          return;
        zoomPreviewEvent(delta);
      }
      else
      {
        if (this._songPreviewController.isPlaying)
          return;
        Action<float> updatePlayHeadEvent = this.updatePlayHeadEvent;
        if (updatePlayHeadEvent == null)
          return;
        updatePlayHeadEvent(delta);
      }
    }
  }
}
