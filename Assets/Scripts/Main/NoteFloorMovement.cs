// Decompiled with JetBrains decompiler
// Type: NoteFloorMovement
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class NoteFloorMovement : MonoBehaviour
{
  [SerializeField]
  protected Transform _rotatedObject;
  [Inject]
  protected readonly PlayerTransforms _playerTransforms;
  [Inject]
  protected readonly IAudioTimeSource _audioTimeSyncController;
  protected Vector3 _startPos;
  protected Vector3 _endPos;
  protected float _moveDuration;
  protected float _startTime;
  protected Quaternion _worldRotation;
  protected Quaternion _inverseWorldRotation;
  protected Vector3 _localPosition;

  public event System.Action floorMovementDidFinishEvent;

  public float distanceToPlayer => Mathf.Abs(this._localPosition.z - (this._inverseWorldRotation * this._playerTransforms.headPseudoLocalPos).z);

  public Vector3 startPos => this._startPos;

  public Vector3 endPos => this._endPos;

  public float startTime => this._startTime;

  public float moveDuration => this._moveDuration;

  public Quaternion worldRotation => this._worldRotation;

  public Quaternion inverseWorldRotation => this._inverseWorldRotation;

  public Vector3 localPosition => this._localPosition;

  public virtual void Init(
    float worldRotation,
    Vector3 startPos,
    Vector3 endPos,
    float moveDuration,
    float startTime)
  {
    this._worldRotation = Quaternion.Euler(0.0f, worldRotation, 0.0f);
    this._inverseWorldRotation = Quaternion.Euler(0.0f, -worldRotation, 0.0f);
    this._startPos = startPos;
    this._endPos = endPos;
    this._moveDuration = moveDuration;
    this._startTime = startTime;
  }

  public virtual Vector3 SetToStart()
  {
    this._localPosition = this._startPos;
    Vector3 start = this._worldRotation * this._localPosition;
    Transform transform = this.transform;
    transform.localPosition = start;
    transform.localRotation = this._worldRotation;
    this._rotatedObject.localRotation = Quaternion.identity;
    return start;
  }

  public virtual Vector3 ManualUpdate()
  {
    float num = this._audioTimeSyncController.songTime - this._startTime;
    this._localPosition = Vector3.Lerp(this._startPos, this._endPos, num / this._moveDuration);
    Vector3 vector3 = this._worldRotation * this._localPosition;
    this.transform.localPosition = vector3;
    if ((double) num >= (double) this._moveDuration)
    {
      System.Action movementDidFinishEvent = this.floorMovementDidFinishEvent;
      if (movementDidFinishEvent != null)
        movementDidFinishEvent();
    }
    return vector3;
  }
}
