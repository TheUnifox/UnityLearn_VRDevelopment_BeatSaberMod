// Decompiled with JetBrains decompiler
// Type: MoveAndRotateWithMainCamera
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class MoveAndRotateWithMainCamera : MonoBehaviour
{
  [Inject]
  protected readonly MainCamera _mainCamera;
  protected Quaternion _rotationOffset;
  protected Vector3 _positionOffset;
  protected Transform _transform;

  public virtual void Awake()
  {
    this._transform = this.transform;
    this._rotationOffset = this._transform.rotation;
    this._positionOffset = this._transform.position;
  }

  public virtual void LateUpdate() => this._transform.SetPositionAndRotation(this._mainCamera.position + this._positionOffset, this._mainCamera.rotation * this._rotationOffset);
}
