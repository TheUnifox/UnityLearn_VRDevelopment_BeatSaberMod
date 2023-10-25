// Decompiled with JetBrains decompiler
// Type: NoteJump
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class NoteJump : MonoBehaviour
{
  [SerializeField]
  protected Transform _rotatedObject;
  [Space]
  [SerializeField]
  protected float _yAvoidanceUp = 0.45f;
  [SerializeField]
  protected float _yAvoidanceDown = 0.15f;
  [Space]
  [SerializeField]
  protected float _endDistanceOffset = 500f;
  [Inject]
  protected readonly PlayerTransforms _playerTransforms;
  [Inject]
  protected readonly PlayerSpaceConvertor _playerSpaceConvertor;
  [Inject]
  protected readonly IAudioTimeSource _audioTimeSyncController;
  protected Vector3 _startPos;
  protected Vector3 _endPos;
  protected float _jumpDuration;
  protected Vector3 _moveVec;
  protected float _beatTime;
  protected float _startVerticalVelocity;
  protected Quaternion _startRotation;
  protected Quaternion _middleRotation;
  protected Quaternion _endRotation;
  protected float _gravity;
  protected float _yAvoidance;
  protected float _missedTime;
  protected bool _missedMarkReported;
  protected bool _threeQuartersMarkReported;
  protected bool _halfJumpMarkReported;
  protected Vector3 _localPosition;
  protected readonly Vector3[] _randomRotations = new Vector3[10]
  {
    new Vector3(-0.9543871f, -0.1183784f, 0.2741019f),
    new Vector3(0.7680854f, -0.08805521f, 0.6342642f),
    new Vector3(-0.6780157f, 0.306681f, -0.6680131f),
    new Vector3(0.1255014f, 0.9398643f, 0.3176546f),
    new Vector3(0.365105f, -0.3664974f, -0.8557909f),
    new Vector3(-0.8790653f, -0.06244748f, -0.4725934f),
    new Vector3(0.01886305f, -0.8065798f, 0.5908241f),
    new Vector3(-0.1455435f, 0.8901445f, 0.4318099f),
    new Vector3(0.07651193f, 0.9474725f, -0.3105508f),
    new Vector3(0.1306983f, -0.2508438f, -0.9591639f)
  };
  public const float kMissedTimeOffset = 0.15f;
  protected Quaternion _worldRotation;
  protected Quaternion _inverseWorldRotation;
  protected bool _rotateTowardsPlayer;

  public event System.Action noteJumpDidFinishEvent;

  public event System.Action noteJumpDidPassMissedMarkerEvent;

  public event System.Action<NoteJump> noteJumpDidPassThreeQuartersEvent;

  public event System.Action noteJumpDidPassHalfEvent;

  public event System.Action<float> noteJumpDidUpdateProgressEvent;

  public float distanceToPlayer => Mathf.Abs(this._localPosition.z - (this._inverseWorldRotation * this._playerTransforms.headPseudoLocalPos).z);

  public Vector3 beatPos => (this._endPos + this._startPos) * 0.5f;

  public float jumpDuration => this._jumpDuration;

  public Vector3 moveVec => this._moveVec;

  public Vector3 localPosition => this._localPosition;

  public virtual void Init(
    float beatTime,
    float worldRotation,
    Vector3 startPos,
    Vector3 endPos,
    float jumpDuration,
    float gravity,
    float flipYSide,
    float endRotation,
    bool rotateTowardsPlayer,
    bool useRandomRotation)
  {
    this._worldRotation = Quaternion.Euler(0.0f, worldRotation, 0.0f);
    this._inverseWorldRotation = Quaternion.Euler(0.0f, -worldRotation, 0.0f);
    this._rotateTowardsPlayer = rotateTowardsPlayer;
    this._startPos = startPos;
    this._endPos = endPos;
    this._jumpDuration = jumpDuration;
    this._moveVec = (this._endPos - this._startPos) / this._jumpDuration;
    this._beatTime = beatTime;
    this._gravity = gravity;
    this._yAvoidance = (double) flipYSide <= 0.0 ? flipYSide * this._yAvoidanceDown : flipYSide * this._yAvoidanceUp;
    this._missedMarkReported = false;
    this._threeQuartersMarkReported = false;
    this._startVerticalVelocity = (float) ((double) this._gravity * (double) this._jumpDuration * 0.5);
    this._endRotation = Quaternion.Euler(0.0f, 0.0f, endRotation);
    this._missedTime = beatTime + 0.15f;
    Vector3 eulerAngles = this._endRotation.eulerAngles;
    if (useRandomRotation)
    {
      int index = Mathf.Abs(Mathf.RoundToInt((float) ((double) beatTime * 10.0 + (double) endPos.x * 2.0 + (double) endPos.y * 2.0)) % this._randomRotations.Length);
      eulerAngles += this._randomRotations[index] * 20f;
    }
    this._middleRotation = new Quaternion();
    this._middleRotation.eulerAngles = eulerAngles;
    this._startRotation = Quaternion.identity;
  }

  public virtual Vector3 ManualUpdate()
  {
    double songTime = (double) this._audioTimeSyncController.songTime;
    float num1 = (float) (songTime - ((double) this._beatTime - (double) this._jumpDuration * 0.5));
    float t = num1 / this._jumpDuration;
    this._localPosition.x = (double) this._startPos.x != (double) this._endPos.x ? ((double) t >= 0.25 ? this._endPos.x : this._startPos.x + (this._endPos.x - this._startPos.x) * Easing.InOutQuad(t * 4f)) : this._startPos.x;
    this._localPosition.z = this._playerTransforms.MoveTowardsHead(this._startPos.z, this._endPos.z, this._inverseWorldRotation, t);
    this._localPosition.y = (float) ((double) this._startPos.y + (double) this._startVerticalVelocity * (double) num1 - (double) this._gravity * (double) num1 * (double) num1 * 0.5);
    if ((double) this._yAvoidance != 0.0 && (double) t < 0.25)
      this._localPosition.y += (float) (0.5 - (double) Mathf.Cos((float) ((double) t * 8.0 * 3.1415927410125732)) * 0.5) * this._yAvoidance;
    if ((double) t < 0.5)
    {
      Quaternion a = (double) t >= 0.125 ? Quaternion.Slerp(this._middleRotation, this._endRotation, Mathf.Sin((float) (((double) t - 0.125) * 3.1415927410125732 * 2.0))) : Quaternion.Slerp(this._startRotation, this._middleRotation, Mathf.Sin((float) ((double) t * 3.1415927410125732 * 4.0)));
      if (this._rotateTowardsPlayer)
      {
        Vector3 headPseudoLocalPos = this._playerTransforms.headPseudoLocalPos;
        headPseudoLocalPos.y = Mathf.Lerp(headPseudoLocalPos.y, this._localPosition.y, 0.8f);
        Vector3 normalized = (this._localPosition - this._inverseWorldRotation * headPseudoLocalPos).normalized;
        Quaternion b = new Quaternion();
        Vector3 vector3 = this._playerSpaceConvertor.worldToPlayerSpaceRotation * this._rotatedObject.up;
        b.SetLookRotation(normalized, this._inverseWorldRotation * vector3);
        this._rotatedObject.localRotation = Quaternion.Lerp(a, b, t * 2f);
      }
      else
        this._rotatedObject.localRotation = a;
    }
    if ((double) t >= 0.5 && !this._halfJumpMarkReported)
    {
      this._halfJumpMarkReported = true;
      System.Action didPassHalfEvent = this.noteJumpDidPassHalfEvent;
      if (didPassHalfEvent != null)
        didPassHalfEvent();
    }
    if ((double) t >= 0.75 && !this._threeQuartersMarkReported)
    {
      this._threeQuartersMarkReported = true;
      System.Action<NoteJump> threeQuartersEvent = this.noteJumpDidPassThreeQuartersEvent;
      if (threeQuartersEvent != null)
        threeQuartersEvent(this);
    }
    if (songTime >= (double) this._missedTime && !this._missedMarkReported)
    {
      this._missedMarkReported = true;
      System.Action missedMarkerEvent = this.noteJumpDidPassMissedMarkerEvent;
      if (missedMarkerEvent != null)
        missedMarkerEvent();
    }
    if (this._threeQuartersMarkReported)
    {
      float num2 = (float) (((double) t - 0.75) / 0.25);
      this._localPosition.z -= Mathf.LerpUnclamped(0.0f, this._endDistanceOffset, num2 * num2 * num2);
    }
    if ((double) t >= 1.0)
    {
      if (!this._missedMarkReported)
      {
        this._missedMarkReported = true;
        System.Action missedMarkerEvent = this.noteJumpDidPassMissedMarkerEvent;
        if (missedMarkerEvent != null)
          missedMarkerEvent();
      }
      System.Action jumpDidFinishEvent = this.noteJumpDidFinishEvent;
      if (jumpDidFinishEvent != null)
        jumpDidFinishEvent();
    }
    Vector3 vector3_1 = this._worldRotation * this._localPosition;
    this.transform.localPosition = vector3_1;
    System.Action<float> updateProgressEvent = this.noteJumpDidUpdateProgressEvent;
    if (updateProgressEvent != null)
      updateProgressEvent(t);
    return vector3_1;
  }
}
