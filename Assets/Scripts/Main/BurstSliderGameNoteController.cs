// Decompiled with JetBrains decompiler
// Type: BurstSliderGameNoteController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class BurstSliderGameNoteController : 
  NoteController,
  ICubeNoteControllerInitializable<BurstSliderGameNoteController>,
  INoteVisualModifierTypeProvider,
  INoteMovementProvider,
  IGameNoteMirrorable,
  INoteMirrorable
{
  [SerializeField]
  protected BoxCuttableBySaber[] _bigCuttableBySaberList;
  [SerializeField]
  protected BoxCuttableBySaber[] _smallCuttableBySaberList;
  [SerializeField]
  protected GameObject _wrapperGO;
  [Inject]
  protected readonly AudioTimeSyncController _audioTimeSyncController;
  protected NoteVisualModifierType _noteVisualModifierType;
  protected NoteData.GameplayType _gameplayType;
  protected BoxCuttableBySaber _bigCuttableBySaber;
  protected Vector3 _originalColliderSize;
  protected Vector3 _originalColliderCenter;

  public event System.Action<BurstSliderGameNoteController> cubeNoteControllerDidInitEvent;

  public NoteMovement noteMovement => this._noteMovement;

  public NoteVisualModifierType noteVisualModifierType => this._noteVisualModifierType;

  public NoteData.GameplayType gameplayType => this._gameplayType;

  public virtual void Init(
    NoteData noteData,
    float worldRotation,
    Vector3 moveStartPos,
    Vector3 moveEndPos,
    Vector3 jumpEndPos,
    float moveDuration,
    float jumpDuration,
    float jumpGravity,
    NoteVisualModifierType noteVisualModifierType,
    float uniformScale)
  {
    this._noteVisualModifierType = noteVisualModifierType;
    this._gameplayType = noteData.gameplayType;
    Vector3 vector3 = (2f - uniformScale) * Vector3.one;
    foreach (BoxCuttableBySaber bigCuttableBySaber in this._bigCuttableBySaberList)
    {
      bigCuttableBySaber.transform.localScale = vector3;
      bigCuttableBySaber.canBeCut = false;
    }
    if ((UnityEngine.Object) this._bigCuttableBySaber != (UnityEngine.Object) null)
    {
      float z = (moveEndPos.z - jumpEndPos.z) / jumpDuration * noteData.timeToPrevColorNote;
      this._bigCuttableBySaber.colliderSize = this._originalColliderSize + new Vector3(0.0f, 0.0f, z);
      this._bigCuttableBySaber.colliderCenter = this._originalColliderCenter - new Vector3(0.0f, 0.0f, z * 0.5f);
    }
    this.Init(noteData, worldRotation, moveStartPos, moveEndPos, jumpEndPos, moveDuration, jumpDuration, jumpGravity, noteData.cutDirection.RotationAngle() + noteData.cutDirectionAngleOffset, uniformScale, false, false);
    System.Action<BurstSliderGameNoteController> controllerDidInitEvent = this.cubeNoteControllerDidInitEvent;
    if (controllerDidInitEvent == null)
      return;
    controllerDidInitEvent(this);
  }

  protected override void Awake()
  {
    base.Awake();
    if (this._bigCuttableBySaberList.Length == 1)
    {
      this._bigCuttableBySaber = this._bigCuttableBySaberList[0];
      this._originalColliderCenter = this._bigCuttableBySaber.colliderCenter;
      this._originalColliderSize = this._bigCuttableBySaber.colliderSize;
    }
    foreach (CuttableBySaber bigCuttableBySaber in this._bigCuttableBySaberList)
      bigCuttableBySaber.wasCutBySaberEvent += new CuttableBySaber.WasCutBySaberDelegate(this.HandleBigWasCutBySaber);
    foreach (CuttableBySaber smallCuttableBySaber in this._smallCuttableBySaberList)
      smallCuttableBySaber.wasCutBySaberEvent += new CuttableBySaber.WasCutBySaberDelegate(this.HandleSmallWasCutBySaber);
  }

  protected override void OnDestroy()
  {
    base.OnDestroy();
    if (this._bigCuttableBySaberList != null)
    {
      foreach (BoxCuttableBySaber bigCuttableBySaber in this._bigCuttableBySaberList)
      {
        if ((UnityEngine.Object) bigCuttableBySaber != (UnityEngine.Object) null)
          bigCuttableBySaber.wasCutBySaberEvent -= new CuttableBySaber.WasCutBySaberDelegate(this.HandleBigWasCutBySaber);
      }
    }
    if (this._smallCuttableBySaberList == null)
      return;
    foreach (BoxCuttableBySaber smallCuttableBySaber in this._smallCuttableBySaberList)
    {
      if ((UnityEngine.Object) smallCuttableBySaber != (UnityEngine.Object) null)
        smallCuttableBySaber.wasCutBySaberEvent -= new CuttableBySaber.WasCutBySaberDelegate(this.HandleSmallWasCutBySaber);
    }
  }

  protected override void NoteDidPassMissedMarker()
  {
    foreach (CuttableBySaber bigCuttableBySaber in this._bigCuttableBySaberList)
      bigCuttableBySaber.canBeCut = false;
    foreach (CuttableBySaber smallCuttableBySaber in this._smallCuttableBySaberList)
      smallCuttableBySaber.canBeCut = false;
    this.SendNoteWasMissedEvent();
  }

  protected override void NoteDidStartDissolving()
  {
    foreach (CuttableBySaber bigCuttableBySaber in this._bigCuttableBySaberList)
      bigCuttableBySaber.canBeCut = false;
    foreach (CuttableBySaber smallCuttableBySaber in this._smallCuttableBySaberList)
      smallCuttableBySaber.canBeCut = false;
  }

  public virtual void HandleBigWasCutBySaber(
    Saber saber,
    Vector3 cutPoint,
    Quaternion orientation,
    Vector3 cutDirVec)
  {
    this.HandleCut(saber, cutPoint, orientation, cutDirVec, false);
  }

  public virtual void HandleSmallWasCutBySaber(
    Saber saber,
    Vector3 cutPoint,
    Quaternion orientation,
    Vector3 cutDirVec)
  {
    this.HandleCut(saber, cutPoint, orientation, cutDirVec, true);
  }

  public virtual void HandleCut(
    Saber saber,
    Vector3 cutPoint,
    Quaternion orientation,
    Vector3 cutDirVec,
    bool allowBadCut)
  {
    float num1 = this.noteData.time - this._audioTimeSyncController.songTime;
    bool directionOK;
    bool speedOK;
    bool saberTypeOK;
    float cutDirDeviation1;
    float cutDirAngle;
    NoteBasicCutInfoHelper.GetBasicCutInfo(this._noteTransform, this.noteData.colorType, this.noteData.cutDirection, saber.saberType, saber.bladeSpeed, cutDirVec, 360f, out directionOK, out speedOK, out saberTypeOK, out cutDirDeviation1, out cutDirAngle);
    if ((!directionOK || !speedOK || !saberTypeOK) && !allowBadCut)
      return;
    Vector3 inNormal = orientation * Vector3.up;
    Plane plane = new Plane(inNormal, cutPoint);
    float num2 = Mathf.Abs(plane.GetDistanceToPoint(this._noteTransform.position));
    NoteData noteData = this.noteData;
    int num3 = speedOK ? 1 : 0;
    int num4 = directionOK ? 1 : 0;
    int num5 = saberTypeOK ? 1 : 0;
    double bladeSpeed = (double) saber.bladeSpeed;
    Vector3 saberDir = cutDirVec;
    int saberType = (int) saber.saberType;
    double timeDeviation = (double) num1;
    double cutDirDeviation2 = (double) cutDirDeviation1;
    Vector3 cutPoint1 = plane.ClosestPointOnPlane(this.transform.position);
    Vector3 cutNormal = inNormal;
    double cutDistanceToCenter = (double) num2;
    double cutAngle = (double) cutDirAngle;
    Quaternion worldRotation = this.worldRotation;
    Quaternion inverseWorldRotation = this.inverseWorldRotation;
    Vector3 position = this.noteTransform.position;
    Quaternion rotation = this.noteTransform.rotation;
    Vector3 notePosition = position;
    SaberMovementData movementData = saber.movementData;
    NoteCutInfo noteCutInfo = new NoteCutInfo(noteData, num3 != 0, num4 != 0, num5 != 0, false, (float) bladeSpeed, saberDir, (SaberType) saberType, (float) timeDeviation, (float) cutDirDeviation2, cutPoint1, cutNormal, (float) cutDistanceToCenter, (float) cutAngle, worldRotation, inverseWorldRotation, rotation, notePosition, (ISaberMovementData) movementData);
    foreach (CuttableBySaber bigCuttableBySaber in this._bigCuttableBySaberList)
      bigCuttableBySaber.canBeCut = false;
    foreach (CuttableBySaber smallCuttableBySaber in this._smallCuttableBySaberList)
      smallCuttableBySaber.canBeCut = false;
    this.SendNoteWasCutEvent(in noteCutInfo);
  }

  protected override void NoteDidStartJump()
  {
    foreach (CuttableBySaber bigCuttableBySaber in this._bigCuttableBySaberList)
      bigCuttableBySaber.canBeCut = true;
    foreach (CuttableBySaber smallCuttableBySaber in this._smallCuttableBySaberList)
      smallCuttableBySaber.canBeCut = true;
  }

  protected override void HiddenStateDidChange(bool hide) => this._wrapperGO.SetActive(!hide);

  public override void Pause(bool pause) => this.enabled = !pause;

  public class Pool : MonoMemoryPool<BurstSliderGameNoteController>
  {
  }
}
