// Decompiled with JetBrains decompiler
// Type: TutorialNoteController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class TutorialNoteController : NoteController, IGameNoteMirrorable, INoteMirrorable
{
  [SerializeField]
  protected BoxCuttableBySaber _cuttableBySaberCore;
  [SerializeField]
  protected BoxCuttableBySaber _cuttableBySaberBeforeNote;
  [SerializeField]
  protected GameObject _wrapperGO;
  protected bool _beforeNoteCutWasOk;
  protected float _cutAngleTolerance;

  public NoteMovement noteMovement => this._noteMovement;

  public NoteVisualModifierType noteVisualModifierType => NoteVisualModifierType.Normal;

  public NoteData.GameplayType gameplayType => NoteData.GameplayType.Normal;

  public virtual void Init(
    NoteData noteData,
    float worldRotation,
    Vector3 moveStartPos,
    Vector3 moveEndPos,
    Vector3 jumpEndPos,
    float moveDuration,
    float jumpDuration,
    float jumpGravity,
    float cutAngleTolerance,
    float uniformScale)
  {
    this._beforeNoteCutWasOk = false;
    this._cutAngleTolerance = cutAngleTolerance;
    this._cuttableBySaberCore.canBeCut = true;
    this._cuttableBySaberBeforeNote.canBeCut = true;
    this.Init(noteData, worldRotation, moveStartPos, moveEndPos, jumpEndPos, moveDuration, jumpDuration, jumpGravity, noteData.cutDirection.RotationAngle() + noteData.cutDirectionAngleOffset, uniformScale, true, true);
  }

  protected override void Awake()
  {
    base.Awake();
    this._cuttableBySaberCore.wasCutBySaberEvent += new CuttableBySaber.WasCutBySaberDelegate(this.HandleCoreWasCutBySaber);
    this._cuttableBySaberBeforeNote.wasCutBySaberEvent += new CuttableBySaber.WasCutBySaberDelegate(this.HandleBeforeNoteWasCutBySaber);
  }

  protected override void OnDestroy()
  {
    base.OnDestroy();
    if ((Object) this._cuttableBySaberCore != (Object) null)
      this._cuttableBySaberCore.wasCutBySaberEvent -= new CuttableBySaber.WasCutBySaberDelegate(this.HandleCoreWasCutBySaber);
    if (!((Object) this._cuttableBySaberBeforeNote != (Object) null))
      return;
    this._cuttableBySaberBeforeNote.wasCutBySaberEvent -= new CuttableBySaber.WasCutBySaberDelegate(this.HandleBeforeNoteWasCutBySaber);
  }

  protected override void NoteDidPassMissedMarker()
  {
    this._cuttableBySaberCore.canBeCut = false;
    this._cuttableBySaberBeforeNote.canBeCut = false;
    this.SendNoteWasMissedEvent();
  }

  public virtual void HandleBeforeNoteWasCutBySaber(
    Saber saber,
    Vector3 cutPoint,
    Quaternion orientation,
    Vector3 cutDirVec)
  {
    if (this._beforeNoteCutWasOk)
      return;
    bool directionOK;
    bool speedOK;
    bool saberTypeOK;
    NoteBasicCutInfoHelper.GetBasicCutInfo(this._noteTransform, this.noteData.colorType, this.noteData.cutDirection, saber.saberType, saber.bladeSpeed, cutDirVec, this._cutAngleTolerance, out directionOK, out speedOK, out saberTypeOK, out float _, out float _);
    this._beforeNoteCutWasOk = directionOK & speedOK & saberTypeOK;
  }

  public virtual void HandleCoreWasCutBySaber(
    Saber saber,
    Vector3 cutPoint,
    Quaternion orientation,
    Vector3 cutDirVec)
  {
    float num1 = 0.0f;
    bool directionOK;
    bool speedOK;
    bool saberTypeOK;
    float cutDirDeviation1;
    float cutDirAngle;
    NoteBasicCutInfoHelper.GetBasicCutInfo(this._noteTransform, this.noteData.colorType, this.noteData.cutDirection, saber.saberType, saber.bladeSpeed, cutDirVec, this._cutAngleTolerance, out directionOK, out speedOK, out saberTypeOK, out cutDirDeviation1, out cutDirAngle);
    bool flag = this._beforeNoteCutWasOk & saberTypeOK && (!directionOK || !speedOK);
    Vector3 inNormal = orientation * Vector3.up;
    Plane plane = new Plane(inNormal, cutPoint);
    NoteData noteData = this.noteData;
    int num2 = speedOK ? 1 : 0;
    int num3 = directionOK ? 1 : 0;
    int num4 = saberTypeOK ? 1 : 0;
    int num5 = flag ? 1 : 0;
    double bladeSpeed = (double) saber.bladeSpeed;
    Vector3 saberDir = cutDirVec;
    int saberType = (int) saber.saberType;
    double timeDeviation = (double) num1;
    double cutDirDeviation2 = (double) cutDirDeviation1;
    Vector3 cutPoint1 = plane.ClosestPointOnPlane(this.transform.position);
    Vector3 cutNormal = inNormal;
    double cutAngle = (double) cutDirAngle;
    Quaternion worldRotation = this.worldRotation;
    Quaternion inverseWorldRotation = this.inverseWorldRotation;
    Vector3 position = this.noteTransform.position;
    Quaternion rotation = this.noteTransform.rotation;
    Vector3 notePosition = position;
    SaberMovementData movementData = saber.movementData;
    this.SendNoteWasCutEvent(new NoteCutInfo(noteData, num2 != 0, num3 != 0, num4 != 0, num5 != 0, (float) bladeSpeed, saberDir, (SaberType) saberType, (float) timeDeviation, (float) cutDirDeviation2, cutPoint1, cutNormal, 0.0f, (float) cutAngle, worldRotation, inverseWorldRotation, rotation, notePosition, (ISaberMovementData) movementData));
  }

  protected override void HiddenStateDidChange(bool hide) => this._wrapperGO.SetActive(!hide);

  public override void Pause(bool pause) => this.enabled = !pause;

  public class Pool : MonoMemoryPool<TutorialNoteController>
  {
  }
}
