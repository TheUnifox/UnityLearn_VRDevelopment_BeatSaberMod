// Decompiled with JetBrains decompiler
// Type: Saber
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class Saber : MonoBehaviour
{
  [SerializeField]
  protected Transform _saberBladeTopTransform;
  [SerializeField]
  protected Transform _saberBladeBottomTransform;
  [SerializeField]
  protected Transform _handleTransform;
  [SerializeField]
  protected SaberTypeObject _saberType;
  protected readonly SaberMovementData _movementData = new SaberMovementData();
  protected Vector3 _saberBladeTopPos;
  protected Vector3 _saberBladeBottomPos;
  protected Vector3 _handlePos;
  protected Quaternion _handleRot;

  public SaberType saberType => this._saberType.saberType;

  public Vector3 saberBladeTopPos => this._saberBladeTopPos;

  public Vector3 saberBladeBottomPos => this._saberBladeBottomPos;

  public Vector3 handlePos => this._handlePos;

  public Quaternion handleRot => this._handleRot;

  public float bladeSpeed => this._movementData.bladeSpeed;

  public SaberMovementData movementData => this._movementData;

  public virtual void ManualUpdate()
  {
    if (!this.gameObject.activeInHierarchy)
      return;
    this._handlePos = this._handleTransform.position;
    this._handleRot = this._handleTransform.rotation;
    this._saberBladeTopPos = this._saberBladeTopTransform.position;
    this._saberBladeBottomPos = this._saberBladeBottomTransform.position;
    this._movementData.AddNewData(this._saberBladeTopPos, this._saberBladeBottomPos, TimeHelper.time);
  }

  public virtual void OverridePositionAndRotation(Vector3 pos, Quaternion rot) => this.transform.SetPositionAndRotation(pos, rot);
}
