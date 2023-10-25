// Decompiled with JetBrains decompiler
// Type: MovementBeatmapEventEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;
using Zenject;

public class MovementBeatmapEventEffect : MonoBehaviour
{
  [SerializeField]
  protected BasicBeatmapEventType _beatmapEventType;
  [Space]
  [SerializeField]
  protected float _transitionSpeed;
  [SerializeField]
  protected MovementBeatmapEventEffect.MovementData[] _movementData;
  [Space]
  [SerializeField]
  protected Transform[] _transforms;
  [Inject]
  protected readonly BeatmapCallbacksController _beatmapCallbacksController;
  protected int _currentMovementDataIdx;
  protected Vector3 _currentPositionOffset;
  protected Vector3 _prevPositionOffset;
  protected Vector3[] _startLocalPositions;
  protected BeatmapDataCallbackWrapper _beatmapDataCallbackWrapper;

  public virtual void Start()
  {
    this._beatmapDataCallbackWrapper = this._beatmapCallbacksController.AddBeatmapCallback<BasicBeatmapEventData>(new BeatmapDataCallback<BasicBeatmapEventData>(this.HandleBeatmapEvent), BasicBeatmapEventData.SubtypeIdentifier(this._beatmapEventType));
    this._currentPositionOffset = this._movementData[0].localPositionOffset;
    this._prevPositionOffset = this._currentPositionOffset;
    this._startLocalPositions = new Vector3[this._transforms.Length];
    for (int index = 0; index < this._transforms.Length; ++index)
      this._startLocalPositions[index] = this._transforms[index].localPosition;
    this.SetLocalPositionOffsetsForAllObjects(this._currentPositionOffset);
  }

  public virtual void OnDestroy()
  {
    if (this._beatmapCallbacksController == null)
      return;
    this._beatmapCallbacksController.RemoveBeatmapCallback(this._beatmapDataCallbackWrapper);
  }

  public virtual void FixedUpdate()
  {
    this._prevPositionOffset = this._currentPositionOffset;
    this._currentPositionOffset = Vector3.LerpUnclamped(this._currentPositionOffset, this._movementData[this._currentMovementDataIdx].localPositionOffset, Time.fixedDeltaTime * this._transitionSpeed);
    if ((double) (this._currentPositionOffset - this._movementData[this._currentMovementDataIdx].localPositionOffset).sqrMagnitude >= 0.0099999997764825821)
      return;
    this.enabled = false;
  }

  public virtual void LateUpdate() => this.SetLocalPositionOffsetsForAllObjects(Vector3.LerpUnclamped(this._prevPositionOffset, this._currentPositionOffset, TimeHelper.interpolationFactor));

  public virtual void HandleBeatmapEvent(BasicBeatmapEventData basicBeatmapEventData)
  {
    this._currentMovementDataIdx = basicBeatmapEventData.sameTypeIndex % this._movementData.Length;
    this.enabled = true;
  }

  public virtual void SetLocalPositionOffsetsForAllObjects(Vector3 localPositionOffset)
  {
    for (int index = 0; index < this._transforms.Length; ++index)
      this._transforms[index].localPosition = this._startLocalPositions[index] + localPositionOffset;
  }

  [Serializable]
  public class MovementData
  {
    [SerializeField]
    protected Vector3 _localPositionOffset;

    public Vector3 localPositionOffset => this._localPositionOffset;
  }
}
