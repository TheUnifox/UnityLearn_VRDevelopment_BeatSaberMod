// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapEditorCameraController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using HMUI;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace BeatmapEditor3D
{
  public class BeatmapEditorCameraController : MonoBehaviour
  {
    [Inject]
    private readonly SignalBus _signalBus;
    private readonly MouseBinder _mouseBinder = new MouseBinder();

    protected void OnEnable()
    {
      this._mouseBinder.AddButtonBinding(MouseBinder.ButtonType.Secondary, MouseBinder.MouseEventType.ButtonDown, new UnityAction(this.HandleCameraStartMovement));
      this._mouseBinder.AddButtonBinding(MouseBinder.ButtonType.Secondary, MouseBinder.MouseEventType.ButtonUp, new UnityAction(this.HandleEndCameraMovement));
    }

    protected void OnDisable() => this._mouseBinder.ClearBindings();

    protected void Update() => this._mouseBinder.ManualUpdate();

    private void HandleCameraStartMovement() => this._signalBus.Fire<ToggleCameraMovementSignal>(new ToggleCameraMovementSignal(true));

    private void HandleEndCameraMovement() => this._signalBus.Fire<ToggleCameraMovementSignal>(new ToggleCameraMovementSignal(false));
  }
}
