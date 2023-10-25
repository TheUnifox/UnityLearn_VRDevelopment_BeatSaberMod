// Decompiled with JetBrains decompiler
// Type: SaberActivityCounter
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class SaberActivityCounter : MonoBehaviour
{
  [SerializeField]
  protected float _averageWindowDuration = 0.5f;
  [SerializeField]
  protected float _valuesPerSecond = 2f;
  [SerializeField]
  protected float _increaseSpeed = 100f;
  [SerializeField]
  protected float _deceraseSpeed = 20f;
  [SerializeField]
  protected float _movementSensitivityThreshold = 0.05f;
  [Inject]
  protected readonly SaberManager _saberManager;
  protected Saber _leftSaber;
  protected Saber _rightSaber;
  protected Vector3 _prevLeftSaberTipPos;
  protected Vector3 _prevRightSaberTipPos;
  protected Vector3 _prevLeftHandPos;
  protected Vector3 _prevRightHandPos;
  protected bool _hasPrevPos;
  protected float _leftSaberMovementDistance;
  protected float _rightSaberMovementDistance;
  protected float _leftHandMovementDistance;
  protected float _rightHandMovementDistance;
  protected MovementHistoryRecorder _saberMovementHistoryRecorder;
  protected MovementHistoryRecorder _handMovementHistoryRecorder;

  public event System.Action<float> totalDistanceDidChangeEvent;

  public float leftSaberMovementDistance => this._leftSaberMovementDistance;

  public float rightSaberMovementDistance => this._rightSaberMovementDistance;

  public float leftHandMovementDistance => this._leftHandMovementDistance;

  public float rightHandMovementDistance => this._rightHandMovementDistance;

  public AveragingValueRecorder saberMovementAveragingValueRecorder => this._saberMovementHistoryRecorder.averagingValueRecorer;

  public AveragingValueRecorder handMovementAveragingValueRecorder => this._handMovementHistoryRecorder.averagingValueRecorer;

  public virtual void Awake()
  {
    this._saberMovementHistoryRecorder = new MovementHistoryRecorder(this._averageWindowDuration, this._valuesPerSecond, this._increaseSpeed, this._deceraseSpeed);
    this._handMovementHistoryRecorder = new MovementHistoryRecorder(this._averageWindowDuration, this._valuesPerSecond, this._increaseSpeed, this._deceraseSpeed);
  }

  public virtual void Start()
  {
    this._leftSaber = this._saberManager.leftSaber;
    this._rightSaber = this._saberManager.rightSaber;
  }

  public virtual void Update()
  {
    if ((double) Time.timeSinceLevelLoad < 1.0)
      return;
    if (!this._hasPrevPos)
    {
      this._prevLeftSaberTipPos = this._leftSaber.saberBladeTopPos;
      this._prevRightSaberTipPos = this._rightSaber.saberBladeTopPos;
      this._prevLeftHandPos = this._leftSaber.handlePos;
      this._prevRightHandPos = this._rightSaber.handlePos;
      this._hasPrevPos = true;
    }
    float num1 = this._leftHandMovementDistance + this._rightHandMovementDistance;
    Vector3 saberBladeTopPos1 = this._leftSaber.saberBladeTopPos;
    float distance1 = Vector3.Distance(saberBladeTopPos1, this._prevLeftSaberTipPos);
    if ((double) distance1 > (double) this._movementSensitivityThreshold)
    {
      this._leftSaberMovementDistance += distance1;
      this._prevLeftSaberTipPos = saberBladeTopPos1;
      this._saberMovementHistoryRecorder.AddMovement(distance1);
    }
    Vector3 saberBladeTopPos2 = this._rightSaber.saberBladeTopPos;
    float distance2 = Vector3.Distance(saberBladeTopPos2, this._prevRightSaberTipPos);
    if ((double) distance2 > (double) this._movementSensitivityThreshold)
    {
      this._rightSaberMovementDistance += distance2;
      this._prevRightSaberTipPos = saberBladeTopPos2;
      this._saberMovementHistoryRecorder.AddMovement(distance2);
    }
    this._saberMovementHistoryRecorder.ManualUpdate(Time.deltaTime);
    Vector3 handlePos1 = this._leftSaber.handlePos;
    float distance3 = Vector3.Distance(handlePos1, this._prevLeftHandPos);
    if ((double) distance3 > (double) this._movementSensitivityThreshold)
    {
      this._leftHandMovementDistance += distance3;
      this._prevLeftHandPos = handlePos1;
      this._handMovementHistoryRecorder.AddMovement(distance3);
    }
    Vector3 handlePos2 = this._rightSaber.handlePos;
    float distance4 = Vector3.Distance(handlePos2, this._prevRightHandPos);
    if ((double) distance4 > (double) this._movementSensitivityThreshold)
    {
      this._rightHandMovementDistance += distance4;
      this._prevRightHandPos = handlePos2;
      this._handMovementHistoryRecorder.AddMovement(distance4);
    }
    this._handMovementHistoryRecorder.ManualUpdate(Time.deltaTime);
    float num2 = this._leftHandMovementDistance + this._rightHandMovementDistance;
    if ((double) num2 == (double) num1)
      return;
    System.Action<float> distanceDidChangeEvent = this.totalDistanceDidChangeEvent;
    if (distanceDidChangeEvent == null)
      return;
    distanceDidChangeEvent(num2);
  }
}
