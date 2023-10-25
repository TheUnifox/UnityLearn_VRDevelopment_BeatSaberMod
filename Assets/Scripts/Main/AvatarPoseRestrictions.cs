// Decompiled with JetBrains decompiler
// Type: AvatarPoseRestrictions
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class AvatarPoseRestrictions : MonoBehaviour
{
  [SerializeField]
  protected float _maxHeadSquareDistanceFromCenter = 4f;
  [SerializeField]
  protected float _minHeadYPos = 0.75f;
  [SerializeField]
  protected float _maxHeadYPos = 2.5f;
  [FormerlySerializedAs("_minHandSquareDistanceFromHeadCenter")]
  [SerializeField]
  protected float _minHandXZSquareDistanceFromHeadCenter = 0.125f;
  [FormerlySerializedAs("_maxHandSquareDistanceFromHeadCenter")]
  [SerializeField]
  protected float _maxHandXZSquareDistanceFromHeadCenter = 1.5f;
  [SerializeField]
  protected float _minHandYDistanceFromHeadCenter = 0.01f;
  [SerializeField]
  protected float _maxHandYDistanceFromHeadCenter = 1f;
  [SerializeField]
  protected bool _forceHeadPosition;
  [SerializeField]
  protected Vector3 _centerHeadOffset = new Vector3(0.0f, 0.0f, -0.1f);
  [Inject]
  protected readonly AvatarPoseController _avatarPoseController;

  public virtual void Start() => this._avatarPoseController.latePositionsWillBeSetCallback = new AvatarPoseController.LatePositionsWillBeSetDelegate(this.HandleAvatarPoseControllerPositionsWillBeSet);

  public virtual void HandleAvatarPoseControllerPositionsWillBeSet(
    Quaternion headRotation,
    Vector3 headPosition,
    Vector3 leftHandPosition,
    Vector3 rightHandPosition,
    out Vector3 newHeadPosition,
    out Vector3 newLeftHandPosition,
    out Vector3 newRightHandPosition)
  {
    Vector3 vector3 = this._forceHeadPosition ? new Vector3(-headPosition.x, 0.0f, -headPosition.z) : Vector3.zero;
    headPosition += vector3;
    Vector2 vector2_1 = new Vector2(headPosition.x, headPosition.z);
    float sqrMagnitude = vector2_1.sqrMagnitude;
    if ((double) sqrMagnitude > (double) this._maxHeadSquareDistanceFromCenter)
    {
      Vector2 vector2_2 = vector2_1 * Mathf.Sqrt(this._maxHeadSquareDistanceFromCenter / sqrMagnitude);
      headPosition = new Vector3(vector2_2.x, headPosition.y, vector2_2.y);
    }
    float num1 = Mathf.Clamp(headPosition.y, this._minHeadYPos, this._maxHeadYPos);
    float num2 = num1 - headPosition.y;
    headPosition.y = num1;
    newHeadPosition = headPosition;
    Vector3 headCenter = headPosition + headRotation * this._centerHeadOffset;
    leftHandPosition.y += num2;
    rightHandPosition.y += num2;
    newLeftHandPosition = this.LimitHandPositionRelativeToHead(leftHandPosition + vector3, headCenter);
    newRightHandPosition = this.LimitHandPositionRelativeToHead(rightHandPosition + vector3, headCenter);
  }

  public virtual Vector3 LimitHandPositionRelativeToHead(Vector3 handPosition, Vector3 headCenter)
  {
    Vector3 vector3 = handPosition - headCenter;
    if ((double) vector3.sqrMagnitude == 0.0)
      return handPosition;
    Vector2 vector2 = new Vector2(vector3.x, vector3.z);
    float sqrMagnitude = vector2.sqrMagnitude;
    if ((double) sqrMagnitude > (double) this._maxHandXZSquareDistanceFromHeadCenter)
      vector2 *= Mathf.Sqrt(this._maxHandXZSquareDistanceFromHeadCenter / sqrMagnitude);
    else if ((double) sqrMagnitude < (double) this._minHandXZSquareDistanceFromHeadCenter)
      vector2 *= Mathf.Sqrt(this._minHandXZSquareDistanceFromHeadCenter / sqrMagnitude);
    float y = Mathf.Sign(vector3.y) * Mathf.Clamp(Mathf.Abs(vector3.y), this._minHandYDistanceFromHeadCenter, this._maxHandYDistanceFromHeadCenter);
    return headCenter + new Vector3(vector2.x, y, vector2.y);
  }
}
