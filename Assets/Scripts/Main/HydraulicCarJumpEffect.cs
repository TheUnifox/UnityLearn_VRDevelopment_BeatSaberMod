// Decompiled with JetBrains decompiler
// Type: HydraulicCarJumpEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class HydraulicCarJumpEffect : MonoBehaviour
{
  [SerializeField]
  protected BasicBeatmapEventType _event;
  [SerializeField]
  protected int[] _eventValues;
  [Space]
  [SerializeField]
  protected Vector3 _impulse;
  [SerializeField]
  protected float _randomness = 0.1f;
  [SerializeField]
  protected Vector3 _position;
  [SerializeField]
  protected float _minDelayBetweenEvents = 0.5f;
  [Space]
  [SerializeField]
  protected Rigidbody _rigidbody;
  [Inject]
  protected readonly BeatmapCallbacksController _beatmapCallbacksController;
  protected float _lastEventTime;
  protected HashSet<int> _eventValuesHashSet;
  protected BeatmapDataCallbackWrapper _beatmapDataCallbackWrapper;

  public virtual void Start()
  {
    this._eventValuesHashSet = new HashSet<int>((IEnumerable<int>) this._eventValues);
    this._beatmapDataCallbackWrapper = this._beatmapCallbacksController.AddBeatmapCallback<BasicBeatmapEventData>(new BeatmapDataCallback<BasicBeatmapEventData>(this.HandleBeatmapEvent), BasicBeatmapEventData.SubtypeIdentifier(this._event));
  }

  public virtual void OnDestroy()
  {
    if (this._beatmapCallbacksController == null)
      return;
    this._beatmapCallbacksController.RemoveBeatmapCallback(this._beatmapDataCallbackWrapper);
  }

  public virtual void HandleBeatmapEvent(BasicBeatmapEventData basicBeatmapEventData)
  {
    if (!this._eventValuesHashSet.Contains(basicBeatmapEventData.value))
      return;
    float timeSinceLevelLoad = Time.timeSinceLevelLoad;
    if ((double) timeSinceLevelLoad - (double) this._lastEventTime < (double) this._minDelayBetweenEvents)
      return;
    this._lastEventTime = timeSinceLevelLoad;
    this._rigidbody.AddForceAtPosition(this._impulse * (1f + Random.Range((float) (-(double) this._randomness * 0.5), this._randomness * 0.5f)), this.transform.TransformPoint(this._position));
  }
}
