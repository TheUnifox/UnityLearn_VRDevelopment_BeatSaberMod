// Decompiled with JetBrains decompiler
// Type: SphereCuttableBySaber
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class SphereCuttableBySaber : CuttableBySaber
{
  [SerializeField]
  protected SphereCollider _collider;
  protected bool _canBeCut;

  public override float radius => this._collider.radius;

  public override bool canBeCut
  {
    set
    {
      this._collider.enabled = value;
      this._canBeCut = value;
    }
    get => this._canBeCut;
  }

  public virtual void Awake() => this._canBeCut = this._collider.enabled;

  public override void Cut(
    Saber saber,
    Vector3 cutPoint,
    Quaternion orientation,
    Vector3 cutDirVec)
  {
    if (!this._canBeCut)
      return;
    this.CallWasCutBySaberEvent(saber, cutPoint, orientation, cutDirVec);
  }
}
