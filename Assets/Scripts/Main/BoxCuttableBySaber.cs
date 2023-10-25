// Decompiled with JetBrains decompiler
// Type: BoxCuttableBySaber
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class BoxCuttableBySaber : CuttableBySaber
{
  [SerializeField]
  protected BoxCollider _collider;
  protected bool _canBeCut;
  protected float _radius;

  public override float radius => this._radius;

  public override bool canBeCut
  {
    set
    {
      this._collider.enabled = value;
      this._canBeCut = value;
    }
    get => this._canBeCut;
  }

  public Vector3 colliderSize
  {
    set
    {
      this._collider.size = value;
      this.RefreshRadius();
    }
    get => this._collider.size;
  }

  public Vector3 colliderCenter
  {
    set => this._collider.center = value;
    get => this._collider.center;
  }

  public virtual void Awake()
  {
    this._canBeCut = this._collider.enabled;
    this.RefreshRadius();
  }

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

  public virtual void SetColliderCenterAndSize(Vector3 center, Vector3 size)
  {
    this._collider.center = center;
    this._collider.size = size;
  }

  public virtual void RefreshRadius()
  {
    Vector3 max = this._collider.bounds.max;
    this._radius = Mathf.Max(Mathf.Max(max.x, max.y), max.z);
  }
}
