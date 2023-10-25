// Decompiled with JetBrains decompiler
// Type: LightPairRotationEventEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class LightPairRotationEventEffect : MonoBehaviour
{
  [SerializeField]
  protected BasicBeatmapEventType _eventL;
  [SerializeField]
  protected BasicBeatmapEventType _eventR;
  [SerializeField]
  protected BasicBeatmapEventType _switchOverrideRandomValuesEvent = BasicBeatmapEventType.VoidEvent;
  [SerializeField]
  protected Vector3 _rotationVector = Vector3.up;
  [Space]
  [SerializeField]
  protected bool _overrideRandomValues;
  [SerializeField]
  protected bool _useZPositionForAngleOffset;
  [SerializeField]
  protected float _zPositionAngleOffsetScale = 1f;
  [SerializeField]
  protected float _startRotation;
  [Space]
  [SerializeField]
  protected Transform _transformL;
  [SerializeField]
  protected Transform _transformR;
  [Inject]
  protected readonly BeatmapCallbacksController _beatmapCallbacksController;
  [Inject]
  protected readonly IAudioTimeSource _audioTimeSource;
  protected const float kSpeedMultiplier = 20f;
  protected LightPairRotationEventEffect.RotationData _rotationDataL;
  protected LightPairRotationEventEffect.RotationData _rotationDataR;
  protected int _randomGenerationFrameNum = -1;
  protected float _randomStartRotation;
  protected float _randomDirection;
  protected BeatmapDataCallbackWrapper _beatmapDataCallbackWrapper;

  public virtual void Start()
  {
    this._beatmapDataCallbackWrapper = this._beatmapCallbacksController.AddBeatmapCallback<BasicBeatmapEventData>(new BeatmapDataCallback<BasicBeatmapEventData>(this.HandleBeatmapEvent), BasicBeatmapEventData.SubtypeIdentifier(this._eventL), BasicBeatmapEventData.SubtypeIdentifier(this._eventR), BasicBeatmapEventData.SubtypeIdentifier(this._switchOverrideRandomValuesEvent));
    this._rotationDataL = new LightPairRotationEventEffect.RotationData()
    {
      enabled = false,
      rotationSpeed = 0.0f,
      startRotation = this._transformL.rotation,
      startRotationAngle = this._startRotation,
      transform = this._transformL
    };
    this._rotationDataR = new LightPairRotationEventEffect.RotationData()
    {
      enabled = false,
      rotationSpeed = 0.0f,
      startRotation = this._transformR.rotation,
      startRotationAngle = -this._startRotation,
      transform = this._transformR
    };
    this._rotationDataL.transform.localRotation = this._rotationDataL.startRotation * Quaternion.Euler(this._rotationVector * this._rotationDataL.startRotationAngle);
    this._rotationDataR.transform.localRotation = this._rotationDataR.startRotation * Quaternion.Euler(this._rotationVector * this._rotationDataR.startRotationAngle);
    this.enabled = false;
  }

  public virtual void Update()
  {
    float frameDeltaSongTime = this._audioTimeSource.lastFrameDeltaSongTime;
    if (this._rotationDataL.enabled)
    {
      this._rotationDataL.rotationAngle += frameDeltaSongTime * this._rotationDataL.rotationSpeed;
      this._rotationDataL.transform.localRotation = this._rotationDataL.startRotation * Quaternion.Euler(this._rotationVector * this._rotationDataL.rotationAngle);
    }
    if (!this._rotationDataR.enabled)
      return;
    this._rotationDataR.rotationAngle += frameDeltaSongTime * this._rotationDataR.rotationSpeed;
    this._rotationDataR.transform.localRotation = this._rotationDataR.startRotation * Quaternion.Euler(this._rotationVector * this._rotationDataR.rotationAngle);
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
      this._overrideRandomValues = basicBeatmapEventData.sameTypeIndex % 2 == 1;
    int frameCount = Time.frameCount;
    if (this._randomGenerationFrameNum != frameCount)
    {
      this._randomGenerationFrameNum = frameCount;
      if (this._overrideRandomValues)
      {
        this._randomDirection = 1f;
        this._randomStartRotation = (float) (frameCount % 360);
        if (this._useZPositionForAngleOffset)
          this._randomStartRotation += this.transform.position.z * this._zPositionAngleOffsetScale;
      }
      else
      {
        this._randomDirection = (double) Random.value > 0.5 ? 1f : -1f;
        this._randomStartRotation = Random.Range(0.0f, 360f);
      }
    }
    if (basicBeatmapEventData.basicBeatmapEventType == this._eventL)
      this.UpdateRotationData(basicBeatmapEventData.value, this._rotationDataL, this._randomStartRotation, this._randomDirection);
    else if (basicBeatmapEventData.basicBeatmapEventType == this._eventR)
      this.UpdateRotationData(basicBeatmapEventData.value, this._rotationDataR, -this._randomStartRotation, -this._randomDirection);
    else if (basicBeatmapEventData.basicBeatmapEventType == this._switchOverrideRandomValuesEvent)
    {
      this._rotationDataL.rotationAngle = this._randomStartRotation + this._rotationDataL.startRotationAngle;
      this._rotationDataL.rotationSpeed = Mathf.Abs(this._rotationDataL.rotationSpeed);
      this._rotationDataL.transform.localRotation = this._rotationDataL.startRotation * Quaternion.Euler(this._rotationVector * this._rotationDataL.rotationAngle);
      this._rotationDataR.rotationAngle = -this._randomStartRotation + this._rotationDataR.startRotationAngle;
      this._rotationDataR.rotationSpeed = -Mathf.Abs(this._rotationDataR.rotationSpeed);
      this._rotationDataR.transform.localRotation = this._rotationDataR.startRotation * Quaternion.Euler(this._rotationVector * this._rotationDataR.rotationAngle);
    }
    this.enabled = this._rotationDataL.enabled || this._rotationDataR.enabled;
  }

  public virtual void UpdateRotationData(
    int beatmapEventDataValue,
    LightPairRotationEventEffect.RotationData rotationData,
    float startRotationOffset,
    float direction)
  {
    if (beatmapEventDataValue == 0)
    {
      rotationData.enabled = false;
      rotationData.transform.localRotation = rotationData.startRotation * Quaternion.Euler(this._rotationVector * rotationData.startRotationAngle);
    }
    else
    {
      if (beatmapEventDataValue <= 0)
        return;
      rotationData.enabled = true;
      rotationData.rotationAngle = startRotationOffset + rotationData.startRotationAngle;
      rotationData.transform.localRotation = rotationData.startRotation * Quaternion.Euler(this._rotationVector * rotationData.rotationAngle);
      rotationData.rotationSpeed = (float) beatmapEventDataValue * 20f * direction;
    }
  }

  public class RotationData
  {
    public bool enabled;
    public float rotationSpeed;
    public Quaternion startRotation;
    public Transform transform;
    public float startRotationAngle;
    public float rotationAngle;
  }
}
