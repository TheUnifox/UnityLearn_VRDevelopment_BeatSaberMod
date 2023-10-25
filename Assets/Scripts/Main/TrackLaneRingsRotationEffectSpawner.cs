// Decompiled with JetBrains decompiler
// Type: TrackLaneRingsRotationEffectSpawner
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class TrackLaneRingsRotationEffectSpawner : MonoBehaviour
{
  [SerializeField]
  protected TrackLaneRingsRotationEffect _trackLaneRingsRotationEffect;
  [Space]
  [SerializeField]
  protected BasicBeatmapEventType _beatmapEventType;
  [Space]
  [SerializeField]
  protected float _rotation = 90f;
  [SerializeField]
  protected float _rotationStep = 5f;
  [SerializeField]
  protected TrackLaneRingsRotationEffectSpawner.RotationStepType _rotationStepType = TrackLaneRingsRotationEffectSpawner.RotationStepType.Range;
  [SerializeField]
  protected int _rotationPropagationSpeed = 1;
  [SerializeField]
  protected float _rotationFlexySpeed = 1f;
  [Inject]
  protected readonly BeatmapCallbacksController _beatmapCallbacksController;
  protected BeatmapDataCallbackWrapper _beatmapDataCallbackWrapper;

  public virtual void Start() => this._beatmapDataCallbackWrapper = this._beatmapCallbacksController.AddBeatmapCallback<BasicBeatmapEventData>(new BeatmapDataCallback<BasicBeatmapEventData>(this.HandleBeatmapEvent), BasicBeatmapEventData.SubtypeIdentifier(this._beatmapEventType));

  public virtual void OnDestroy()
  {
    if (this._beatmapCallbacksController == null)
      return;
    this._beatmapCallbacksController.RemoveBeatmapCallback(this._beatmapDataCallbackWrapper);
  }

  public virtual void HandleBeatmapEvent(BasicBeatmapEventData basicBeatmapEventData)
  {
    float step = 0.0f;
    switch (this._rotationStepType)
    {
      case TrackLaneRingsRotationEffectSpawner.RotationStepType.Range0ToMax:
        step = Random.Range(0.0f, this._rotationStep);
        break;
      case TrackLaneRingsRotationEffectSpawner.RotationStepType.Range:
        step = Random.Range(-this._rotationStep, this._rotationStep);
        break;
      case TrackLaneRingsRotationEffectSpawner.RotationStepType.MaxOr0:
        step = (double) Random.value < 0.5 ? this._rotationStep : 0.0f;
        break;
    }
    this._trackLaneRingsRotationEffect.AddRingRotationEffect(this._trackLaneRingsRotationEffect.GetFirstRingDestinationRotationAngle() + this._rotation * ((double) Random.value < 0.5 ? 1f : -1f), step, this._rotationPropagationSpeed, this._rotationFlexySpeed);
  }

  public enum RotationStepType
  {
    Range0ToMax,
    Range,
    MaxOr0,
  }
}
