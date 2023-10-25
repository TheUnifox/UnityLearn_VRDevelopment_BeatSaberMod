// Decompiled with JetBrains decompiler
// Type: TrackLaneRingsPositionStepEffectSpawner
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class TrackLaneRingsPositionStepEffectSpawner : MonoBehaviour
{
  [SerializeField]
  protected TrackLaneRingsManager _trackLaneRingsManager;
  [Space]
  [SerializeField]
  protected BasicBeatmapEventType _beatmapEventType;
  [Space]
  [SerializeField]
  protected float _minPositionStep = 1f;
  [SerializeField]
  protected float _maxPositionStep = 2f;
  [SerializeField]
  protected float _moveSpeed = 1f;
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
    float num = basicBeatmapEventData.sameTypeIndex % 2 == 0 ? this._maxPositionStep : this._minPositionStep;
    TrackLaneRing[] rings = this._trackLaneRingsManager.Rings;
    for (int index = 0; index < rings.Length; ++index)
    {
      float destPosZ = (float) index * num;
      rings[index].SetPosition(destPosZ, this._moveSpeed);
    }
  }
}
