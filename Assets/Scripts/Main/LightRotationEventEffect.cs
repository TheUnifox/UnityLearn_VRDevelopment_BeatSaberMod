// Decompiled with JetBrains decompiler
// Type: LightRotationEventEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class LightRotationEventEffect : MonoBehaviour
{
  [SerializeField]
  protected BasicBeatmapEventType _event;
  [SerializeField]
  protected Vector3 _rotationVector = Vector3.up;
  [SerializeField]
  protected float _rotationSpeedMultiplier = 1f;
  [Inject]
  protected readonly BeatmapCallbacksController _beatmapCallbacksController;
  [Inject]
  protected readonly IAudioTimeSource _audioTimeSource;
  protected const float kSpeedMultiplier = 20f;
  protected Transform _transform;
  protected Quaternion _startRotation;
  protected float _rotationSpeed;
  protected BeatmapDataCallbackWrapper _beatmapDataCallbackWrapper;

  public virtual void Start()
  {
    this._beatmapDataCallbackWrapper = this._beatmapCallbacksController.AddBeatmapCallback<BasicBeatmapEventData>(new BeatmapDataCallback<BasicBeatmapEventData>(this.HandleBeatmapEvent), BasicBeatmapEventData.SubtypeIdentifier(this._event));
    this._transform = this.transform;
    this._startRotation = this._transform.rotation;
    this.enabled = false;
  }

  public virtual void Update() => this._transform.Rotate(this._rotationVector, this._audioTimeSource.lastFrameDeltaSongTime * this._rotationSpeed, Space.Self);

  public virtual void OnDestroy()
  {
    if (this._beatmapCallbacksController == null)
      return;
    this._beatmapCallbacksController.RemoveBeatmapCallback(this._beatmapDataCallbackWrapper);
  }

  public virtual void HandleBeatmapEvent(BasicBeatmapEventData basicBeatmapEventData)
  {
    if (basicBeatmapEventData.value == 0)
    {
      this.enabled = false;
      this._transform.localRotation = this._startRotation;
    }
    else
    {
      if (basicBeatmapEventData.value <= 0)
        return;
      this._transform.localRotation = this._startRotation;
      this._transform.Rotate(this._rotationVector, Random.Range(0.0f, 180f), Space.Self);
      this.enabled = true;
      this._rotationSpeed = (float) ((double) basicBeatmapEventData.value * (double) this._rotationSpeedMultiplier * 20.0 * ((double) Random.value > 0.5 ? 1.0 : -1.0));
    }
  }
}
