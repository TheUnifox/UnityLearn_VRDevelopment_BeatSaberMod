// Decompiled with JetBrains decompiler
// Type: NoteMovement
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;
using UnityEngine;

public class NoteMovement : MonoBehaviour
{
  [SerializeField]
  protected NoteFloorMovement _floorMovement;
  [SerializeField]
  protected NoteJump _jump;
  [Space]
  [SerializeField]
  protected float _zOffset;
  [CompilerGenerated]
  protected NoteMovement.MovementPhase m_CmovementPhase;
  protected Vector3 _position;
  protected Vector3 _prevPosition;
  protected Vector3 _localPosition;
  protected Vector3 _prevLocalPosition;

  public event System.Action didInitEvent;

  public event System.Action noteDidStartJumpEvent;

  public event System.Action noteDidFinishJumpEvent;

  public event System.Action noteDidPassMissedMarkerEvent;

  public event System.Action noteDidPassHalfJumpEvent;

  public event System.Action<NoteMovement> noteDidPassJumpThreeQuartersEvent;

  public event System.Action noteDidMoveInJumpPhaseEvent;

  public NoteMovement.MovementPhase movementPhase
  {
    get => this.m_CmovementPhase;
    private set => this.m_CmovementPhase = value;
  }

  public Vector3 position => this._position;

  public Vector3 prevPosition => this._prevPosition;

  public Vector3 localPosition => this._localPosition;

  public Vector3 prevLocalPosition => this._prevLocalPosition;

  public Quaternion worldRotation => this._floorMovement.worldRotation;

  public Quaternion inverseWorldRotation => this._floorMovement.inverseWorldRotation;

  public Vector3 moveEndPos => this._floorMovement.endPos;

  public float moveStartTime => this._floorMovement.startTime;

  public float moveDuration => this._floorMovement.moveDuration;

  public Vector3 beatPos => this._jump.beatPos;

  public float jumpDuration => this._jump.jumpDuration;

  public Vector3 jumpMoveVec => this._jump.moveVec;

  public float distanceToPlayer => this.movementPhase != NoteMovement.MovementPhase.Jumping ? this._floorMovement.distanceToPlayer : this._jump.distanceToPlayer;

  public virtual void Init(
    float beatTime,
    float worldRotation,
    Vector3 moveStartPos,
    Vector3 moveEndPos,
    Vector3 jumpEndPos,
    float moveDuration,
    float jumpDuration,
    float jumpGravity,
    float flipYSide,
    float endRotation,
    bool rotateTowardsPlayer,
    bool useRandomRotation)
  {
    moveStartPos.z += this._zOffset;
    moveEndPos.z += this._zOffset;
    jumpEndPos.z += this._zOffset;
    this._floorMovement.Init(worldRotation, moveStartPos, moveEndPos, moveDuration, (float) ((double) beatTime - (double) moveDuration - (double) jumpDuration * 0.5));
    this._position = this._floorMovement.SetToStart();
    this._prevPosition = this._position;
    this._localPosition = this._prevLocalPosition = this._floorMovement.localPosition;
    this._jump.Init(beatTime, worldRotation, moveEndPos, jumpEndPos, jumpDuration, jumpGravity, flipYSide, endRotation, rotateTowardsPlayer, useRandomRotation);
    this.movementPhase = NoteMovement.MovementPhase.MovingOnTheFloor;
    System.Action didInitEvent = this.didInitEvent;
    if (didInitEvent == null)
      return;
    didInitEvent();
  }

  public virtual void Awake()
  {
    this.movementPhase = NoteMovement.MovementPhase.None;
    this._floorMovement.floorMovementDidFinishEvent += new System.Action(this.HandleFloorMovementDidFinish);
    this._jump.noteJumpDidFinishEvent += new System.Action(this.HandleNoteJumpDidFinish);
    this._jump.noteJumpDidPassMissedMarkerEvent += new System.Action(this.HandleNoteJumpDidPassMissedMark);
    this._jump.noteJumpDidPassThreeQuartersEvent += new System.Action<NoteJump>(this.HandleNoteJumpDidPassThreeQuarters);
    this._jump.noteJumpDidPassHalfEvent += new System.Action(this.HandleNoteJumpNoteJumpDidPassHalf);
  }

  public virtual void OnDestroy()
  {
    if ((bool) (UnityEngine.Object) this._floorMovement)
      this._floorMovement.floorMovementDidFinishEvent -= new System.Action(this.HandleFloorMovementDidFinish);
    if (!(bool) (UnityEngine.Object) this._jump)
      return;
    this._jump.noteJumpDidFinishEvent -= new System.Action(this.HandleNoteJumpDidFinish);
    this._jump.noteJumpDidPassMissedMarkerEvent -= new System.Action(this.HandleNoteJumpDidPassMissedMark);
    this._jump.noteJumpDidPassThreeQuartersEvent -= new System.Action<NoteJump>(this.HandleNoteJumpDidPassThreeQuarters);
    this._jump.noteJumpDidPassHalfEvent -= new System.Action(this.HandleNoteJumpNoteJumpDidPassHalf);
  }

  public virtual void HandleFloorMovementDidFinish()
  {
    this.movementPhase = NoteMovement.MovementPhase.Jumping;
    this._position = this._jump.ManualUpdate();
    this._localPosition = this._jump.localPosition;
    System.Action didStartJumpEvent = this.noteDidStartJumpEvent;
    if (didStartJumpEvent == null)
      return;
    didStartJumpEvent();
  }

  public virtual void HandleNoteJumpDidFinish()
  {
    this.movementPhase = NoteMovement.MovementPhase.None;
    System.Action didFinishJumpEvent = this.noteDidFinishJumpEvent;
    if (didFinishJumpEvent == null)
      return;
    didFinishJumpEvent();
  }

  public virtual void HandleNoteJumpDidPassMissedMark()
  {
    System.Action missedMarkerEvent = this.noteDidPassMissedMarkerEvent;
    if (missedMarkerEvent == null)
      return;
    missedMarkerEvent();
  }

  public virtual void HandleNoteJumpDidPassThreeQuarters(NoteJump noteJump)
  {
    System.Action<NoteMovement> threeQuartersEvent = this.noteDidPassJumpThreeQuartersEvent;
    if (threeQuartersEvent == null)
      return;
    threeQuartersEvent(this);
  }

  public virtual void HandleNoteJumpNoteJumpDidPassHalf()
  {
    System.Action passHalfJumpEvent = this.noteDidPassHalfJumpEvent;
    if (passHalfJumpEvent == null)
      return;
    passHalfJumpEvent();
  }

  public virtual void ManualUpdate()
  {
    this._prevPosition = this._position;
    this._prevLocalPosition = this._localPosition;
    if (this.movementPhase == NoteMovement.MovementPhase.MovingOnTheFloor)
    {
      this._position = this._floorMovement.ManualUpdate();
      this._localPosition = this._floorMovement.localPosition;
    }
    else
    {
      this._position = this._jump.ManualUpdate();
      this._localPosition = this._jump.localPosition;
      System.Action inJumpPhaseEvent = this.noteDidMoveInJumpPhaseEvent;
      if (inJumpPhaseEvent == null)
        return;
      inJumpPhaseEvent();
    }
  }

  public enum MovementPhase
  {
    None,
    MovingOnTheFloor,
    Jumping,
  }
}
