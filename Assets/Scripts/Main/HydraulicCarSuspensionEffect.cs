// Decompiled with JetBrains decompiler
// Type: HydraulicCarSuspensionEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class HydraulicCarSuspensionEffect : MonoBehaviour
{
  [SerializeField]
  protected BasicBeatmapEventType _contractEvent;
  [SerializeField]
  protected int[] _contractEventValues;
  [Space]
  [SerializeField]
  protected BasicBeatmapEventType _expandEvent;
  [SerializeField]
  protected int[] _expandEventValues;
  [Space]
  [SerializeField]
  protected SpringJoint _springJoint;
  [SerializeField]
  protected float _contractDistance = 0.3f;
  [SerializeField]
  protected float _expandDistance = 0.4f;
  [Space]
  [SerializeField]
  protected Rigidbody _rigidbody;
  [Inject]
  protected readonly BeatmapCallbacksController _beatmapCallbacksController;
  protected HashSet<int> _contractEventValuesHashSet;
  protected HashSet<int> _expandEventValuesHashSet;
  protected BeatmapDataCallbackWrapper _contractBeatmapDataCallbackWrapper;
  protected BeatmapDataCallbackWrapper _expandBeatmapDataCallbackWrapper;

  public virtual void Start()
  {
    this._contractEventValuesHashSet = new HashSet<int>((IEnumerable<int>) this._contractEventValues);
    this._expandEventValuesHashSet = new HashSet<int>((IEnumerable<int>) this._expandEventValues);
    this._contractBeatmapDataCallbackWrapper = this._beatmapCallbacksController.AddBeatmapCallback<BasicBeatmapEventData>(new BeatmapDataCallback<BasicBeatmapEventData>(this.HandleContractBeatmapEvent), BasicBeatmapEventData.SubtypeIdentifier(this._contractEvent));
    this._expandBeatmapDataCallbackWrapper = this._beatmapCallbacksController.AddBeatmapCallback<BasicBeatmapEventData>(new BeatmapDataCallback<BasicBeatmapEventData>(this.HandleExpandBeatmapEvent), BasicBeatmapEventData.SubtypeIdentifier(this._expandEvent));
  }

  public virtual void OnDestroy()
  {
    if (this._beatmapCallbacksController == null)
      return;
    this._beatmapCallbacksController.RemoveBeatmapCallback(this._contractBeatmapDataCallbackWrapper);
    this._beatmapCallbacksController.RemoveBeatmapCallback(this._expandBeatmapDataCallbackWrapper);
  }

  public virtual void HandleContractBeatmapEvent(BasicBeatmapEventData basicBeatmapEventData)
  {
    if (!this._contractEventValuesHashSet.Contains(basicBeatmapEventData.value))
      return;
    this._springJoint.minDistance = this._contractDistance;
    this._springJoint.maxDistance = this._contractDistance;
    this._rigidbody.WakeUp();
  }

  public virtual void HandleExpandBeatmapEvent(BasicBeatmapEventData basicBeatmapEventData)
  {
    if (!this._expandEventValuesHashSet.Contains(basicBeatmapEventData.value))
      return;
    this._springJoint.minDistance = this._expandDistance;
    this._springJoint.maxDistance = this._expandDistance;
    this._rigidbody.WakeUp();
  }
}
