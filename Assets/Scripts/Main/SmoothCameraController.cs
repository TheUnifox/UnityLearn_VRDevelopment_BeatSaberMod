// Decompiled with JetBrains decompiler
// Type: SmoothCameraController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class SmoothCameraController : MonoBehaviour
{
  [SerializeField]
  protected MainSettingsModelSO _mainSettingsModel;
  [SerializeField]
  protected SmoothCamera _smoothCamera;

  public virtual void Start()
  {
    LivStaticWrapper.didActivateEvent += new System.Action(this.HandleDidActivate);
    LivStaticWrapper.didDeactivateEvent += new System.Action(this.HandleDidDeactivate);
    this.ActivateSmoothCameraIfNeeded();
  }

  public virtual void OnDestroy()
  {
    LivStaticWrapper.didActivateEvent -= new System.Action(this.HandleDidActivate);
    LivStaticWrapper.didDeactivateEvent -= new System.Action(this.HandleDidDeactivate);
  }

  public virtual void HandleDidActivate()
  {
    if (!this._smoothCamera.enabled)
      return;
    this._smoothCamera.enabled = false;
  }

  public virtual void HandleDidDeactivate() => this.ActivateSmoothCameraIfNeeded();

  public virtual void ActivateSmoothCameraIfNeeded()
  {
    if (LivStaticWrapper.isActive || !(bool) (ObservableVariableSO<bool>) this._mainSettingsModel.smoothCameraEnabled || this._smoothCamera.enabled)
      return;
    this._smoothCamera.enabled = true;
    this._smoothCamera.Init((float) (ObservableVariableSO<float>) this._mainSettingsModel.smoothCameraFieldOfView, (float) (ObservableVariableSO<float>) this._mainSettingsModel.smoothCameraPositionSmooth, (float) (ObservableVariableSO<float>) this._mainSettingsModel.smoothCameraRotationSmooth, (bool) (ObservableVariableSO<bool>) this._mainSettingsModel.smoothCameraThirdPersonEnabled, (Vector3) (ObservableVariableSO<Vector3>) this._mainSettingsModel.smoothCameraThirdPersonPosition, (Vector3) (ObservableVariableSO<Vector3>) this._mainSettingsModel.smoothCameraThirdPersonEulerAngles);
  }
}
