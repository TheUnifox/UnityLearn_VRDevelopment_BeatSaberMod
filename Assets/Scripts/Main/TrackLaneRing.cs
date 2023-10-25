// Decompiled with JetBrains decompiler
// Type: TrackLaneRing
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class TrackLaneRing : MonoBehaviour
{
  protected float _prevRotZ;
  protected float _rotZ;
  protected float _destRotZ;
  protected float _rotationSpeed;
  protected float _prevPosZ;
  protected float _posZ;
  protected float _destPosZ;
  protected float _moveSpeed;
  protected Vector3 _positionOffset;
  protected Transform _transform;

  public float destRotZ => this._destRotZ;

  public virtual void Init(Vector3 position, Vector3 positionOffset)
  {
    this._transform = this.transform;
    this._positionOffset = positionOffset;
    this._transform.localPosition = position + positionOffset;
    this._posZ = position.z + positionOffset.z;
  }

  public virtual void FixedUpdateRing(float fixedDeltaTime)
  {
    this._prevRotZ = this._rotZ;
    this._rotZ = Mathf.Lerp(this._rotZ, this._destRotZ, fixedDeltaTime * this._rotationSpeed);
    this._prevPosZ = this._posZ;
    this._posZ = Mathf.Lerp(this._posZ, this._positionOffset.z + this._destPosZ, fixedDeltaTime * this._moveSpeed);
  }

  public virtual void LateUpdateRing(float interpolationFactor)
  {
    this._transform.localEulerAngles = new Vector3(0.0f, 0.0f, this._prevRotZ + (this._rotZ - this._prevRotZ) * interpolationFactor);
    this._transform.localPosition = new Vector3(this._positionOffset.x, this._positionOffset.y, this._prevPosZ + (this._posZ - this._prevPosZ) * interpolationFactor);
  }

  public virtual void SetDestRotation(float destRotZ, float rotateSpeed)
  {
    this._destRotZ = destRotZ;
    this._rotationSpeed = rotateSpeed;
  }

  public virtual float GetRotation() => this._rotZ;

  public virtual float GetDestinationRotation() => this._destRotZ;

  public virtual void SetPosition(float destPosZ, float moveSpeed)
  {
    this._destPosZ = destPosZ;
    this._moveSpeed = moveSpeed;
  }
}
