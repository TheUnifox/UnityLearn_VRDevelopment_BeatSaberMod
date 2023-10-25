// Decompiled with JetBrains decompiler
// Type: SliderMovement
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class SliderMovement : MonoBehaviour
{
  [Inject]
  protected readonly IAudioTimeSource _audioTimeSyncController;
  [Inject]
  protected readonly PlayerTransforms _playerTransforms;
  protected Vector3 _headNoteJumpStartPos;
  protected Vector3 _headNoteJumpEndPos;
  protected float _headNoteTime;
  protected float _tailNoteTime;
  protected Vector3 _localPosition;
  protected Quaternion _worldRotation;
  protected Quaternion _inverseWorldRotation;
  protected float _jumpDuration;
  protected float _headNoteGravity;
  protected float _tailNoteGravity;
  protected Transform _transform;
  protected bool _movementEndReported;
  protected bool _headDidMovePastCutMarkReported;
  protected bool _tailDidMovePastCutMarkReported;
  protected float _timeSinceHeadNoteJump;

  public event System.Action movementDidFinishEvent;

  public event System.Action<float> movementDidMoveEvent;

  public event System.Action headDidMovePastCutMarkEvent;

  public event System.Action tailDidMovePastCutMarkEvent;

  public float jumpDuration => this._jumpDuration;

  public float headNoteGravity => this._headNoteGravity;

  public float tailNoteGravity => this._tailNoteGravity;

  public float timeSinceHeadNoteJump => this._timeSinceHeadNoteJump;

  public virtual void Init(
    float headNoteTime,
    float tailNoteTime,
    float worldRotation,
    Vector3 headNoteJumpStartPos,
    Vector3 headNoteJumpEndPos,
    float jumpDuration,
    float headNoteGravity,
    float tailNoteGravity)
  {
    this._movementEndReported = false;
    this._headDidMovePastCutMarkReported = false;
    this._tailDidMovePastCutMarkReported = false;
    this._worldRotation = Quaternion.Euler(0.0f, worldRotation, 0.0f);
    this._inverseWorldRotation = Quaternion.Euler(0.0f, -worldRotation, 0.0f);
    this._headNoteJumpStartPos = headNoteJumpStartPos;
    this._headNoteJumpEndPos = headNoteJumpEndPos;
    this._jumpDuration = jumpDuration;
    this._headNoteGravity = headNoteGravity;
    this._tailNoteGravity = tailNoteGravity;
    this._headNoteTime = headNoteTime;
    this._tailNoteTime = tailNoteTime;
    this._transform = this.transform;
    this._timeSinceHeadNoteJump = (float) (-(double) jumpDuration * 0.5);
  }

  public virtual void StartMovement() => this._transform.localRotation = this._worldRotation;

  public virtual void ManualUpdate()
  {
    float songTime = this._audioTimeSyncController.songTime;
    this._timeSinceHeadNoteJump = songTime - (this._headNoteTime - this._jumpDuration * 0.5f);
    double num1 = (double) songTime - ((double) this._tailNoteTime - (double) this._jumpDuration * 0.5);
    float t = this._timeSinceHeadNoteJump / this._jumpDuration;
    double jumpDuration = (double) this._jumpDuration;
    float num2 = (float) (num1 / jumpDuration);
    this._localPosition.z = this._playerTransforms.MoveTowardsHead(this._headNoteJumpStartPos.z, this._headNoteJumpEndPos.z, this._inverseWorldRotation, t);
    this._transform.localPosition = this._worldRotation * this._localPosition;
    if (!this._headDidMovePastCutMarkReported && (double) t > 0.5)
    {
      this._headDidMovePastCutMarkReported = true;
      System.Action pastCutMarkEvent = this.headDidMovePastCutMarkEvent;
      if (pastCutMarkEvent != null)
        pastCutMarkEvent();
    }
    if (!this._tailDidMovePastCutMarkReported && (double) num2 > 0.5)
    {
      this._tailDidMovePastCutMarkReported = true;
      System.Action pastCutMarkEvent = this.tailDidMovePastCutMarkEvent;
      if (pastCutMarkEvent != null)
        pastCutMarkEvent();
    }
    if (!this._movementEndReported && (double) num2 > 0.75)
    {
      this._movementEndReported = true;
      System.Action movementDidFinishEvent = this.movementDidFinishEvent;
      if (movementDidFinishEvent != null)
        movementDidFinishEvent();
    }
    System.Action<float> movementDidMoveEvent = this.movementDidMoveEvent;
    if (movementDidMoveEvent == null)
      return;
    movementDidMoveEvent(this._timeSinceHeadNoteJump);
  }
}
