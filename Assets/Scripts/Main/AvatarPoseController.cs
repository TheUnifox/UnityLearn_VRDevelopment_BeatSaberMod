// Decompiled with JetBrains decompiler
// Type: AvatarPoseController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;
using UnityEngine;

public class AvatarPoseController : MonoBehaviour
{
  [SerializeField]
  protected Transform _headTransform;
  [SerializeField]
  protected Transform _leftHandTransform;
  [SerializeField]
  protected Transform _rightHandTransform;
  [SerializeField]
  protected Transform _bodyTransform;
  [SerializeField]
  protected HeadBodyOffsetSO _headBodyOffset;
  [CompilerGenerated]
  protected AvatarPoseController.PositionsWillBeSetDelegate m_CearlyPositionsWillBeSetCallback;
  [CompilerGenerated]
  protected AvatarPoseController.LatePositionsWillBeSetDelegate m_ClatePositionsWillBeSetCallback;
  [CompilerGenerated]
  protected AvatarPoseController.RotationsWillBeSetDelegate m_CearlyRotationsWillBeSetCallback;

  public AvatarPoseController.PositionsWillBeSetDelegate earlyPositionsWillBeSetCallback
  {
    get => this.m_CearlyPositionsWillBeSetCallback;
    set => this.m_CearlyPositionsWillBeSetCallback = value;
  }

  public AvatarPoseController.LatePositionsWillBeSetDelegate latePositionsWillBeSetCallback
  {
    get => this.m_ClatePositionsWillBeSetCallback;
    set => this.m_ClatePositionsWillBeSetCallback = value;
  }

  public AvatarPoseController.RotationsWillBeSetDelegate earlyRotationsWillBeSetCallback
  {
    get => this.m_CearlyRotationsWillBeSetCallback;
    set => this.m_CearlyRotationsWillBeSetCallback = value;
  }

  public event System.Action<Vector3> didUpdatePoseEvent;

  public virtual void UpdateTransforms(
    Vector3 headPosition,
    Vector3 leftHandPosition,
    Vector3 rightHandPosition,
    Quaternion headRotation,
    Quaternion leftHandRotation,
    Quaternion rightHandRotation)
  {
    AvatarPoseController.PositionsWillBeSetDelegate willBeSetCallback1 = this.earlyPositionsWillBeSetCallback;
    if (willBeSetCallback1 != null)
      willBeSetCallback1(headPosition, leftHandPosition, rightHandPosition, out headPosition, out leftHandPosition, out rightHandPosition);
    AvatarPoseController.RotationsWillBeSetDelegate willBeSetCallback2 = this.earlyRotationsWillBeSetCallback;
    if (willBeSetCallback2 != null)
      willBeSetCallback2(headRotation, leftHandRotation, rightHandRotation, out headRotation, out leftHandRotation, out rightHandRotation);
    AvatarPoseController.LatePositionsWillBeSetDelegate willBeSetCallback3 = this.latePositionsWillBeSetCallback;
    if (willBeSetCallback3 != null)
      willBeSetCallback3(headRotation, headPosition, leftHandPosition, rightHandPosition, out headPosition, out leftHandPosition, out rightHandPosition);
    this._headTransform.SetLocalPositionAndRotation(headPosition, headRotation);
    this._leftHandTransform.SetLocalPositionAndRotation(leftHandPosition, leftHandRotation);
    this._rightHandTransform.SetLocalPositionAndRotation(rightHandPosition, rightHandRotation);
    this.UpdateBodyPosition();
    System.Action<Vector3> didUpdatePoseEvent = this.didUpdatePoseEvent;
    if (didUpdatePoseEvent == null)
      return;
    didUpdatePoseEvent(headPosition);
  }

  public virtual void UpdateBodyPosition()
  {
    Quaternion localRotation = this._headTransform.localRotation;
    this._bodyTransform.SetLocalPositionAndRotation(this._headTransform.localPosition + localRotation * this._headBodyOffset.headNeckOffset + Vector3.down * this._headBodyOffset.verticalOffset, Quaternion.Euler(0.0f, localRotation.eulerAngles.y, 0.0f));
  }

  public delegate void PositionsWillBeSetDelegate(
    Vector3 headPosition,
    Vector3 leftHandPosition,
    Vector3 rightHandPosition,
    out Vector3 newHeadPosition,
    out Vector3 newLeftHandPosition,
    out Vector3 newRightHandPosition);

  public delegate void LatePositionsWillBeSetDelegate(
    Quaternion headRotation,
    Vector3 headPosition,
    Vector3 leftHandPosition,
    Vector3 rightHandPosition,
    out Vector3 newHeadPosition,
    out Vector3 newLeftHandPosition,
    out Vector3 newRightHandPosition);

  public delegate void RotationsWillBeSetDelegate(
    Quaternion headRotation,
    Quaternion leftHandRotation,
    Quaternion rightHandRotation,
    out Quaternion newHeadRotation,
    out Quaternion newLeftHandRotation,
    out Quaternion newRightHandRotation);
}
