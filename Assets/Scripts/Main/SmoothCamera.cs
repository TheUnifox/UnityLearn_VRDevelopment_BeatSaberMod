// Decompiled with JetBrains decompiler
// Type: SmoothCamera
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class SmoothCamera : MonoBehaviour
{
  [SerializeField]
  protected Camera _camera;
  [Inject]
  protected MainCamera _mainCamera;
  protected Vector3 _thirdPersonPosition;
  protected Vector3 _thirdPersonEulerAngles;
  protected bool _thirdPersonEnabled;
  protected float _rotationSmooth;
  protected float _positionSmooth;

  public virtual void Init(
    float fieldOfView,
    float positionSmooth,
    float rotationSmooth,
    bool thirdPersonEnabled,
    Vector3 thirdPersonPosition,
    Vector3 thirdPersonEulerAngles)
  {
    this._camera.fieldOfView = fieldOfView / this._camera.aspect;
    this._camera.depthTextureMode = this._mainCamera.camera.depthTextureMode;
    this._camera.nearClipPlane = this._mainCamera.camera.nearClipPlane;
    this._camera.farClipPlane = this._mainCamera.camera.farClipPlane;
    this._thirdPersonPosition = thirdPersonPosition;
    this._thirdPersonEnabled = thirdPersonEnabled;
    this._rotationSmooth = rotationSmooth;
    this._positionSmooth = positionSmooth;
    this._thirdPersonEulerAngles = thirdPersonEulerAngles;
    this.transform.SetPositionAndRotation(this._mainCamera.position, this._mainCamera.rotation);
  }

  public virtual void OnEnable() => this._camera.enabled = true;

  public virtual void OnDisable() => this._camera.enabled = false;

  public virtual void LateUpdate()
  {
    if (this._thirdPersonEnabled)
      this.transform.SetPositionAndRotation(this._thirdPersonPosition, new Quaternion()
      {
        eulerAngles = this._thirdPersonEulerAngles
      });
    else
      this.transform.SetPositionAndRotation(Vector3.Lerp(this.transform.position, this._mainCamera.position, Time.deltaTime * this._positionSmooth), Quaternion.Slerp(this.transform.rotation, this._mainCamera.rotation, Time.deltaTime * this._rotationSmooth));
  }
}
