// Decompiled with JetBrains decompiler
// Type: ParticleSystemContinuousEventEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class ParticleSystemContinuousEventEffect : MonoBehaviour
{
  [SerializeField]
  protected BasicBeatmapEventType _beatmapEvent;
  [SerializeField]
  protected ParticleSystem[] _particleSystems;
  [Inject]
  protected readonly BeatmapCallbacksController _beatmapCallbacksController;
  protected BeatmapDataCallbackWrapper _beatmapDataCallbackWrapper;

  public virtual void Start() => this._beatmapDataCallbackWrapper = this._beatmapCallbacksController.AddBeatmapCallback<BasicBeatmapEventData>(new BeatmapDataCallback<BasicBeatmapEventData>(this.HandleBeatmapEvent), BasicBeatmapEventData.SubtypeIdentifier(this._beatmapEvent));

  public virtual void OnDestroy()
  {
    if (this._beatmapCallbacksController == null)
      return;
    this._beatmapCallbacksController.RemoveBeatmapCallback(this._beatmapDataCallbackWrapper);
  }

  public virtual void HandleBeatmapEvent(BasicBeatmapEventData basicBeatmapEventData) => this.ToggleEmitting(basicBeatmapEventData.value == 1);

  public virtual void ToggleEmitting(bool shouldPlay)
  {
    if (shouldPlay)
    {
      foreach (ParticleSystem particleSystem in this._particleSystems)
        particleSystem.Play(false);
    }
    else
    {
      foreach (ParticleSystem particleSystem in this._particleSystems)
        particleSystem.Stop(false, ParticleSystemStopBehavior.StopEmitting);
    }
  }
}
