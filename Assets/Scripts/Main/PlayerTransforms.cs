// Decompiled with JetBrains decompiler
// Type: PlayerTransforms
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class PlayerTransforms : MonoBehaviour
{
  [SerializeField]
  protected Transform _headTransform;
  [SerializeField]
  protected Transform _originTransform;
  [SerializeField]
  protected Transform _leftHandTransform;
  [SerializeField]
  protected Transform _rightHandTransform;
  protected bool _overrideHeadPos;
  protected Vector3 _overriddenHeadPos;
  protected Vector3 _headWorldPos;
  protected Quaternion _headWorldRot;
  protected Vector3 _headPseudoLocalPos;
  protected Quaternion _headPseudoLocalRot;
  protected Vector3 _leftHandPseudoLocalPos;
  protected Quaternion _leftHandPseudoLocalRot;
  protected Vector3 _rightHandPseudoLocalPos;
  protected Quaternion _rightHandPseudoLocalRot;
  protected Transform _originParentTransform;
  protected bool _useOriginParentTransformForPseudoLocalCalculations;

  public Vector3 headWorldPos => this._headWorldPos;

  public Quaternion headWorldRot => this._headWorldRot;

  public Vector3 headPseudoLocalPos => this._headPseudoLocalPos;

  public Quaternion headPseudoLocalRot => this._headPseudoLocalRot;

  public Vector3 leftHandPseudoLocalPos => this._leftHandPseudoLocalPos;

  public Quaternion leftHandPseudoLocalRot => this._leftHandPseudoLocalRot;

  public Vector3 rightHandPseudoLocalPos => this._rightHandPseudoLocalPos;

  public Quaternion rightHandPseudoLocalRot => this._rightHandPseudoLocalRot;

  public virtual void Awake()
  {
    this._originParentTransform = this._originTransform.parent;
    this._useOriginParentTransformForPseudoLocalCalculations = (Object) this._originParentTransform != (Object) null;
  }

  public virtual void OverrideHeadPos(Vector3 pos)
  {
    this._headPseudoLocalPos = pos;
    this._headWorldPos = pos;
    this._overrideHeadPos = true;
  }

  public virtual void Update()
  {
    if (this._overrideHeadPos)
      return;
    this._headWorldPos = this._headTransform.position;
    this._headWorldRot = this._headTransform.rotation;
    if (this._useOriginParentTransformForPseudoLocalCalculations)
    {
      this._headPseudoLocalPos = this._originParentTransform.InverseTransformPoint(this._headWorldPos);
      this._headPseudoLocalRot = this._originParentTransform.InverseTransformRotation(this._headWorldRot);
      this._rightHandPseudoLocalPos = this._originParentTransform.InverseTransformPoint(this._rightHandTransform.position);
      this._rightHandPseudoLocalRot = this._originParentTransform.InverseTransformRotation(this._rightHandTransform.rotation);
      this._leftHandPseudoLocalPos = this._originParentTransform.InverseTransformPoint(this._leftHandTransform.position);
      this._leftHandPseudoLocalRot = this._originParentTransform.InverseTransformRotation(this._leftHandTransform.rotation);
    }
    else
    {
      this._headPseudoLocalPos = this._headTransform.localPosition;
      this._headPseudoLocalRot = this._headTransform.localRotation;
      this._rightHandPseudoLocalPos = this._rightHandTransform.localPosition;
      this._rightHandPseudoLocalRot = this._rightHandTransform.localRotation;
      this._leftHandPseudoLocalPos = this._leftHandTransform.localPosition;
      this._leftHandPseudoLocalRot = this._leftHandTransform.localRotation;
    }
  }

  public virtual float MoveTowardsHead(
    float start,
    float end,
    Quaternion noteInverseWorldRotation,
    float t)
  {
    float headOffsetZ = this.HeadOffsetZ(noteInverseWorldRotation);
    return this.GetZPos(start, end, headOffsetZ, t);
  }

  public virtual float GetZPosOffsetByHeadPosAtTime(float start, float end, float t) => this.GetZPos(start, end, this._headPseudoLocalPos.z, t);

  public virtual float GetZPos(float start, float end, float headOffsetZ, float t) => Mathf.LerpUnclamped(start + headOffsetZ * Mathf.Min(1f, t * 2f), end + headOffsetZ, t);

  public virtual float HeadOffsetZ(Quaternion noteInverseWorldRotation) => (noteInverseWorldRotation * this._headPseudoLocalPos).z;
}
