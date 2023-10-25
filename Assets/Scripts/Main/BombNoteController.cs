// Decompiled with JetBrains decompiler
// Type: BombNoteController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class BombNoteController : NoteController
{
  [SerializeField]
  protected CuttableBySaber _cuttableBySaber;
  [SerializeField]
  protected GameObject _wrapperGO;

  public virtual void Init(
    NoteData noteData,
    float worldRotation,
    Vector3 moveStartPos,
    Vector3 moveEndPos,
    Vector3 jumpEndPos,
    float moveDuration,
    float jumpDuration,
    float jumpGravity)
  {
    this._cuttableBySaber.canBeCut = true;
    this.Init(noteData, worldRotation, moveStartPos, moveEndPos, jumpEndPos, moveDuration, jumpDuration, jumpGravity, 0.0f, 1f, true, true);
  }

  protected override void Awake()
  {
    base.Awake();
    this._cuttableBySaber.wasCutBySaberEvent += new CuttableBySaber.WasCutBySaberDelegate(this.HandleWasCutBySaber);
    this._noteMovement.noteDidPassHalfJumpEvent += new System.Action(this.HandleDidPassHalfJump);
  }

  protected override void OnDestroy()
  {
    base.OnDestroy();
    if (!((UnityEngine.Object) this._cuttableBySaber != (UnityEngine.Object) null))
      return;
    this._cuttableBySaber.wasCutBySaberEvent -= new CuttableBySaber.WasCutBySaberDelegate(this.HandleWasCutBySaber);
  }

  protected override void NoteDidPassMissedMarker()
  {
    this._cuttableBySaber.canBeCut = false;
    this.SendNoteWasMissedEvent();
  }

  public virtual void HandleDidPassHalfJump() => this._cuttableBySaber.canBeCut = false;

  public virtual void HandleWasCutBySaber(
    Saber saber,
    Vector3 cutPoint,
    Quaternion orientation,
    Vector3 cutDirVec)
  {
    Vector3 vector3 = orientation * Vector3.up;
    NoteData noteData = this.noteData;
    double bladeSpeed = (double) saber.bladeSpeed;
    Vector3 saberDir = cutDirVec;
    int saberType = (int) saber.saberType;
    Vector3 cutPoint1 = cutPoint;
    Vector3 cutNormal = vector3;
    Quaternion worldRotation = this.worldRotation;
    Quaternion inverseWorldRotation = this.inverseWorldRotation;
    Vector3 position = this.noteTransform.position;
    Quaternion rotation = this.noteTransform.rotation;
    Vector3 notePosition = position;
    SaberMovementData movementData = saber.movementData;
    this.SendNoteWasCutEvent(new NoteCutInfo(noteData, true, true, false, false, (float) bladeSpeed, saberDir, (SaberType) saberType, 0.0f, 0.0f, cutPoint1, cutNormal, 0.0f, 0.0f, worldRotation, inverseWorldRotation, rotation, notePosition, (ISaberMovementData) movementData));
  }

  protected override void NoteDidStartDissolving() => this._cuttableBySaber.canBeCut = false;

  protected override void HiddenStateDidChange(bool hide) => this._wrapperGO.SetActive(!hide);

  public override void Pause(bool pause) => this.enabled = !pause;

  public class Pool : MonoMemoryPool<BombNoteController>
  {
  }
}
