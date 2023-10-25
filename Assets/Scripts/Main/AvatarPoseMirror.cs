// Decompiled with JetBrains decompiler
// Type: AvatarPoseMirror
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class AvatarPoseMirror : MonoBehaviour
{
  [Inject]
  protected readonly AvatarPoseController _avatarPoseController;

  public virtual void Start()
  {
    this._avatarPoseController.earlyPositionsWillBeSetCallback = new AvatarPoseController.PositionsWillBeSetDelegate(AvatarPoseMirror.HandleAvatarPoseControllerPositionsWillBeSet);
    this._avatarPoseController.earlyRotationsWillBeSetCallback = new AvatarPoseController.RotationsWillBeSetDelegate(AvatarPoseMirror.HandleAvatarPoseControllerRotationsWillBeSet);
  }

  private static void HandleAvatarPoseControllerPositionsWillBeSet(
    Vector3 headPosition,
    Vector3 leftHandPosition,
    Vector3 rightHandPosition,
    out Vector3 newHeadPosition,
    out Vector3 newLeftHandPosition,
    out Vector3 newRightHandPosition)
  {
    newHeadPosition = AvatarPoseMirror.MirrorPosition(headPosition);
    newLeftHandPosition = AvatarPoseMirror.MirrorPosition(leftHandPosition);
    newRightHandPosition = AvatarPoseMirror.MirrorPosition(rightHandPosition);
  }

  private static void HandleAvatarPoseControllerRotationsWillBeSet(
    Quaternion headRotation,
    Quaternion leftHandRotation,
    Quaternion rightHandRotation,
    out Quaternion newHeadRotation,
    out Quaternion newLeftHandRotation,
    out Quaternion newRightHandRotation)
  {
    newHeadRotation = AvatarPoseMirror.MirrorRotation(headRotation);
    newLeftHandRotation = AvatarPoseMirror.MirrorRotation(leftHandRotation);
    newRightHandRotation = AvatarPoseMirror.MirrorRotation(rightHandRotation);
  }

  private static Quaternion MirrorRotation(Quaternion rotation) => new Quaternion(rotation.x, -rotation.y, -rotation.z, rotation.w);

  private static Vector3 MirrorPosition(Vector3 position) => new Vector3(-position.x, position.y, position.z);
}
