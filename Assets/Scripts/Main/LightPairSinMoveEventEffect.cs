// Decompiled with JetBrains decompiler
// Type: LightPairSinMoveEventEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class LightPairSinMoveEventEffect : MonoBehaviour
{
  [SerializeField]
  protected BasicBeatmapEventType _eventL;
  [SerializeField]
  protected BasicBeatmapEventType _eventR;
  [SerializeField]
  protected BasicBeatmapEventType _switchOverrideRandomValuesEvent = BasicBeatmapEventType.VoidEvent;
  [Space]
  [SerializeField]
  protected bool _overrideRandomValues;
  [SerializeField]
  [Tooltip("Range 0 - 1")]
  protected float _startValueOffset;
  [SerializeField]
  protected Vector3 _startPositionOffset = new Vector3(0.0f, 0.0f, 0.0f);
  [SerializeField]
  protected Vector3 _endPositionOffset = new Vector3(0.0f, 2f, 0.0f);
  [Space]
  [SerializeField]
  protected Transform _transformL;
  [SerializeField]
  protected Transform _transformR;
  [Inject]
  protected readonly BeatmapCallbacksController _beatmapCallbacksController;
  [Inject]
  protected readonly IAudioTimeSource _audioTimeSource;
  protected const float kSpeedMultiplier = 1f;
  protected LightPairSinMoveEventEffect.MovementData _movementDataL;
  protected LightPairSinMoveEventEffect.MovementData _movementDataR;
  protected int _randomGenerationFrameNum = -1;
  protected float _randomStartOffset;
  protected BeatmapDataCallbackWrapper _beatmapDataCallbackWrapper;

  public virtual void Start()
  {
    this._beatmapDataCallbackWrapper = this._beatmapCallbacksController.AddBeatmapCallback<BasicBeatmapEventData>(new BeatmapDataCallback<BasicBeatmapEventData>(this.HandleBeatmapEvent), BasicBeatmapEventData.SubtypeIdentifier(this._eventL), BasicBeatmapEventData.SubtypeIdentifier(this._eventR), BasicBeatmapEventData.SubtypeIdentifier(this._switchOverrideRandomValuesEvent));
    this._movementDataL = new LightPairSinMoveEventEffect.MovementData()
    {
      enabled = false,
      speed = 0.0f,
      startPosition = this._transformL.localPosition,
      startMovementValue = this._startValueOffset,
      transform = this._transformL,
      side = 1f
    };
    this._movementDataR = new LightPairSinMoveEventEffect.MovementData()
    {
      enabled = false,
      speed = 0.0f,
      startPosition = this._transformR.localPosition,
      startMovementValue = this._startValueOffset,
      transform = this._transformR,
      side = -1f
    };
    Vector3 vector3_1 = Vector3.LerpUnclamped(this._startPositionOffset, this._endPositionOffset, (float) ((double) Mathf.Sin(this._movementDataL.startMovementValue) * 0.5 + 0.5));
    vector3_1.x *= this._movementDataL.side;
    this._movementDataL.transform.localPosition = this._movementDataL.startPosition + vector3_1;
    Vector3 vector3_2 = Vector3.LerpUnclamped(this._startPositionOffset, this._endPositionOffset, (float) ((double) Mathf.Sin(this._movementDataR.startMovementValue) * 0.5 + 0.5));
    vector3_2.x *= this._movementDataR.side;
    this._movementDataR.transform.localPosition = this._movementDataR.startPosition + vector3_2;
    this.enabled = false;
  }

  public virtual void Update()
  {
    float frameDeltaSongTime = this._audioTimeSource.lastFrameDeltaSongTime;
    if (this._movementDataL.enabled)
    {
      this._movementDataL.movementValue += frameDeltaSongTime * this._movementDataL.speed;
      Vector3 vector3 = Vector3.LerpUnclamped(this._startPositionOffset, this._endPositionOffset, (float) ((double) Mathf.Sin(this._movementDataL.movementValue) * 0.5 + 0.5));
      vector3.x *= this._movementDataL.side;
      this._movementDataL.transform.localPosition = this._movementDataL.startPosition + vector3;
    }
    if (!this._movementDataR.enabled)
      return;
    this._movementDataR.movementValue += frameDeltaSongTime * this._movementDataR.speed;
    Vector3 vector3_1 = Vector3.LerpUnclamped(this._startPositionOffset, this._endPositionOffset, (float) ((double) Mathf.Sin(this._movementDataR.movementValue) * 0.5 + 0.5));
    vector3_1.x *= this._movementDataR.side;
    this._movementDataR.transform.localPosition = this._movementDataR.startPosition + vector3_1;
  }

  public virtual void OnDestroy()
  {
    if (this._beatmapCallbacksController == null)
      return;
    this._beatmapCallbacksController.RemoveBeatmapCallback(this._beatmapDataCallbackWrapper);
  }

  public virtual void HandleBeatmapEvent(BasicBeatmapEventData basicBeatmapEventData)
  {
    if (basicBeatmapEventData.basicBeatmapEventType == this._switchOverrideRandomValuesEvent)
    {
      this._overrideRandomValues = basicBeatmapEventData.sameTypeIndex % 2 == 1;
      this._randomGenerationFrameNum = -1;
    }
    int frameCount = Time.frameCount;
    if (this._randomGenerationFrameNum != frameCount)
    {
      this._randomGenerationFrameNum = frameCount;
      this._randomStartOffset = this._overrideRandomValues ? 0.0f : Random.Range(0.0f, 6.28318548f);
    }
    if (basicBeatmapEventData.basicBeatmapEventType == this._eventL)
      this.UpdateMovementData(basicBeatmapEventData.value, this._movementDataL, this._randomStartOffset);
    else if (basicBeatmapEventData.basicBeatmapEventType == this._eventR)
      this.UpdateMovementData(basicBeatmapEventData.value, this._movementDataR, -this._randomStartOffset);
    else if (basicBeatmapEventData.basicBeatmapEventType == this._switchOverrideRandomValuesEvent)
    {
      this._movementDataL.movementValue = this._randomStartOffset + this._movementDataL.startMovementValue;
      this._movementDataL.speed = Mathf.Abs(this._movementDataL.speed);
      this._movementDataR.movementValue = this._randomStartOffset + this._movementDataR.startMovementValue;
      this._movementDataR.speed = Mathf.Abs(this._movementDataL.speed);
    }
    this.enabled = this._movementDataL.enabled || this._movementDataR.enabled;
  }

  public virtual void UpdateMovementData(
    int beatmapEventDataValue,
    LightPairSinMoveEventEffect.MovementData movementData,
    float movementValueOffset)
  {
    if (beatmapEventDataValue == 0)
    {
      movementData.enabled = false;
      Vector3 vector3 = Vector3.LerpUnclamped(this._startPositionOffset, this._endPositionOffset, (float) ((double) Mathf.Sin(movementData.startMovementValue) * 0.5 + 0.5));
      vector3.x *= movementData.side;
      movementData.transform.localPosition = movementData.startPosition + vector3;
    }
    else
    {
      if (beatmapEventDataValue <= 0)
        return;
      movementData.enabled = true;
      movementData.movementValue = movementValueOffset + movementData.startMovementValue;
      Vector3 vector3 = Vector3.LerpUnclamped(this._startPositionOffset, this._endPositionOffset, (float) ((double) Mathf.Sin(movementData.movementValue) * 0.5 + 0.5));
      vector3.x *= movementData.side;
      movementData.transform.localPosition = movementData.startPosition + vector3;
      movementData.speed = (float) beatmapEventDataValue * 1f;
    }
  }

  public class MovementData
  {
    public bool enabled;
    public float speed;
    public Vector3 startPosition;
    public Transform transform;
    public float startMovementValue;
    public float movementValue;
    public float side;
  }
}
