// Decompiled with JetBrains decompiler
// Type: NoteController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections;
using UnityEngine;

public abstract class NoteController : NoteControllerBase, INoteMirrorable, IBeatmapObjectController
{
  [SerializeField]
  protected NoteMovement _noteMovement;
  [SerializeField]
  protected Transform _noteTransform;
  private readonly LazyCopyHashSet<INoteControllerDidInitEvent> _didInitEvent = new LazyCopyHashSet<INoteControllerDidInitEvent>();
  private readonly LazyCopyHashSet<INoteControllerNoteDidStartJumpEvent> _noteDidStartJumpEvent = new LazyCopyHashSet<INoteControllerNoteDidStartJumpEvent>();
  private readonly LazyCopyHashSet<INoteControllerNoteDidFinishJumpEvent> _noteDidFinishJumpEvent = new LazyCopyHashSet<INoteControllerNoteDidFinishJumpEvent>();
  private readonly LazyCopyHashSet<INoteControllerNoteDidPassJumpThreeQuartersEvent> _noteDidPassJumpThreeQuartersEvent = new LazyCopyHashSet<INoteControllerNoteDidPassJumpThreeQuartersEvent>();
  private readonly LazyCopyHashSet<INoteControllerNoteWasCutEvent> _noteWasCutEvent = new LazyCopyHashSet<INoteControllerNoteWasCutEvent>();
  private readonly LazyCopyHashSet<INoteControllerNoteWasMissedEvent> _noteWasMissedEvent = new LazyCopyHashSet<INoteControllerNoteWasMissedEvent>();
  private readonly LazyCopyHashSet<INoteControllerNoteDidStartDissolvingEvent> _noteDidStartDissolvingEvent = new LazyCopyHashSet<INoteControllerNoteDidStartDissolvingEvent>();
  private readonly LazyCopyHashSet<INoteControllerNoteDidDissolveEvent> _noteDidDissolveEvent = new LazyCopyHashSet<INoteControllerNoteDidDissolveEvent>();
  private NoteData _noteData;
  private bool _dissolving;
  private float _uniformScale;

  public override ILazyCopyHashSet<INoteControllerDidInitEvent> didInitEvent => (ILazyCopyHashSet<INoteControllerDidInitEvent>) this._didInitEvent;

  public ILazyCopyHashSet<INoteControllerNoteDidStartJumpEvent> noteDidStartJumpEvent => (ILazyCopyHashSet<INoteControllerNoteDidStartJumpEvent>) this._noteDidStartJumpEvent;

  public ILazyCopyHashSet<INoteControllerNoteDidFinishJumpEvent> noteDidFinishJumpEvent => (ILazyCopyHashSet<INoteControllerNoteDidFinishJumpEvent>) this._noteDidFinishJumpEvent;

  public override ILazyCopyHashSet<INoteControllerNoteDidPassJumpThreeQuartersEvent> noteDidPassJumpThreeQuartersEvent => (ILazyCopyHashSet<INoteControllerNoteDidPassJumpThreeQuartersEvent>) this._noteDidPassJumpThreeQuartersEvent;

  public ILazyCopyHashSet<INoteControllerNoteWasCutEvent> noteWasCutEvent => (ILazyCopyHashSet<INoteControllerNoteWasCutEvent>) this._noteWasCutEvent;

  public ILazyCopyHashSet<INoteControllerNoteWasMissedEvent> noteWasMissedEvent => (ILazyCopyHashSet<INoteControllerNoteWasMissedEvent>) this._noteWasMissedEvent;

  public override ILazyCopyHashSet<INoteControllerNoteDidStartDissolvingEvent> noteDidStartDissolvingEvent => (ILazyCopyHashSet<INoteControllerNoteDidStartDissolvingEvent>) this._noteDidStartDissolvingEvent;

  public ILazyCopyHashSet<INoteControllerNoteDidDissolveEvent> noteDidDissolveEvent => (ILazyCopyHashSet<INoteControllerNoteDidDissolveEvent>) this._noteDidDissolveEvent;

  public Transform noteTransform => this._noteTransform;

  public Quaternion worldRotation => this._noteMovement.worldRotation;

  public Quaternion inverseWorldRotation => this._noteMovement.inverseWorldRotation;

  public float moveStartTime => this._noteMovement.moveStartTime;

  public float moveDuration => this._noteMovement.moveDuration;

  public float jumpDuration => this._noteMovement.jumpDuration;

  public Vector3 jumpMoveVec => this._noteMovement.jumpMoveVec;

  public Vector3 beatPos => this._noteMovement.beatPos;

  public Vector3 jumpStartPos => this._noteMovement.moveEndPos;

  public override NoteData noteData => this._noteData;

  public Vector3 moveVec => this.worldRotation * this.jumpMoveVec;

  public float uniformScale => this._uniformScale;

  public bool hidden { get; private set; }

  protected virtual void Awake()
  {
    this._noteMovement.noteDidFinishJumpEvent += new System.Action(this.HandleNoteDidFinishJump);
    this._noteMovement.noteDidStartJumpEvent += new System.Action(this.HandleNoteDidStartJump);
    this._noteMovement.noteDidPassJumpThreeQuartersEvent += new System.Action<NoteMovement>(this.HandleNoteDidPassJumpThreeQuarters);
    this._noteMovement.noteDidPassMissedMarkerEvent += new System.Action(this.HandleNoteDidPassMissedMarkerEvent);
  }

  protected virtual void OnDestroy()
  {
    if (!((UnityEngine.Object) this._noteMovement != (UnityEngine.Object) null))
      return;
    this._noteMovement.noteDidFinishJumpEvent -= new System.Action(this.HandleNoteDidFinishJump);
    this._noteMovement.noteDidStartJumpEvent -= new System.Action(this.HandleNoteDidStartJump);
    this._noteMovement.noteDidPassJumpThreeQuartersEvent -= new System.Action<NoteMovement>(this.HandleNoteDidPassJumpThreeQuarters);
    this._noteMovement.noteDidPassMissedMarkerEvent -= new System.Action(this.HandleNoteDidPassMissedMarkerEvent);
  }

  protected void Update() => this.ManualUpdate();

  public virtual void ManualUpdate() => this._noteMovement.ManualUpdate();

  private void HandleNoteDidStartJump()
  {
    this.NoteDidStartJump();
    foreach (INoteControllerNoteDidStartJumpEvent didStartJumpEvent in this._noteDidStartJumpEvent.items)
      didStartJumpEvent.HandleNoteControllerNoteDidStartJump(this);
  }

  private void HandleNoteDidFinishJump()
  {
    if (this._dissolving)
      return;
    this.NoteDidFinishJump();
    foreach (INoteControllerNoteDidFinishJumpEvent didFinishJumpEvent in this._noteDidFinishJumpEvent.items)
      didFinishJumpEvent.HandleNoteControllerNoteDidFinishJump(this);
  }

  private void HandleNoteDidPassJumpThreeQuarters(NoteMovement noteMovement)
  {
    if (this._dissolving)
      return;
    this.NoteDidPassJumpThreeQuarters(noteMovement);
    foreach (INoteControllerNoteDidPassJumpThreeQuartersEvent threeQuartersEvent in this._noteDidPassJumpThreeQuartersEvent.items)
      threeQuartersEvent.HandleNoteControllerNoteDidPassJumpThreeQuarters((NoteControllerBase) this);
  }

  private void HandleNoteDidPassMissedMarkerEvent()
  {
    if (this._dissolving)
      return;
    this.NoteDidPassMissedMarker();
  }

  protected virtual void NoteDidStartJump()
  {
  }

  protected virtual void NoteDidFinishJump()
  {
  }

  protected virtual void NoteDidPassJumpThreeQuarters(NoteMovement noteMovement)
  {
  }

  protected virtual void NoteDidPassMissedMarker()
  {
  }

  protected virtual void NoteDidStartDissolving()
  {
  }

  protected void SendNoteWasMissedEvent()
  {
    foreach (INoteControllerNoteWasMissedEvent noteWasMissedEvent in this._noteWasMissedEvent.items)
      noteWasMissedEvent.HandleNoteControllerNoteWasMissed(this);
  }

  protected void SendNoteWasCutEvent(in NoteCutInfo noteCutInfo)
  {
    foreach (INoteControllerNoteWasCutEvent controllerNoteWasCutEvent in this._noteWasCutEvent.items)
      controllerNoteWasCutEvent.HandleNoteControllerNoteWasCut(this, in noteCutInfo);
  }

  protected void Init(
    NoteData noteData,
    float worldRotation,
    Vector3 moveStartPos,
    Vector3 moveEndPos,
    Vector3 jumpEndPos,
    float moveDuration,
    float jumpDuration,
    float jumpGravity,
    float endRotation,
    float uniformScale,
    bool rotateTowardsPlayer,
    bool useRandomRotation)
  {
    this._noteData = noteData;
    this._uniformScale = uniformScale;
    this.transform.SetPositionAndRotation(moveStartPos, Quaternion.identity);
    this._noteTransform.localScale = Vector3.one * uniformScale;
    this._noteMovement.Init(noteData.time, worldRotation, moveStartPos, moveEndPos, jumpEndPos, moveDuration, jumpDuration, jumpGravity, noteData.flipYSide, endRotation, rotateTowardsPlayer, useRandomRotation);
    foreach (INoteControllerDidInitEvent controllerDidInitEvent in this._didInitEvent.items)
      controllerDidInitEvent.HandleNoteControllerDidInit((NoteControllerBase) this);
  }

  private IEnumerator DissolveCoroutine(float duration)
  {
    NoteController noteController = this;
    foreach (INoteControllerNoteDidStartDissolvingEvent startDissolvingEvent in noteController._noteDidStartDissolvingEvent.items)
      startDissolvingEvent.HandleNoteControllerNoteDidStartDissolving((NoteControllerBase) noteController, duration);
    yield return (object) new WaitForSeconds(duration);
    noteController._dissolving = false;
    foreach (INoteControllerNoteDidDissolveEvent didDissolveEvent in noteController._noteDidDissolveEvent.items)
      didDissolveEvent.HandleNoteControllerNoteDidDissolve(noteController);
  }

  public void Dissolve(float duration)
  {
    if (this._dissolving)
      return;
    this._dissolving = true;
    this.NoteDidStartDissolving();
    this.StartCoroutine(this.DissolveCoroutine(duration));
  }

  protected abstract void HiddenStateDidChange(bool hidden);

  public void Hide(bool hide)
  {
    this.hidden = hide;
    this.HiddenStateDidChange(hide);
  }

  public abstract void Pause(bool pause);
}
