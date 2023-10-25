// Decompiled with JetBrains decompiler
// Type: TubeLightIntensityBehaviour
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class TubeLightIntensityBehaviour : PlayableBehaviour
{
  [Header("Start")]
  public bool _noPredefinedStartValue;
  [DrawIf("_noPredefinedStartValue", false, DrawIfAttribute.DisablingType.DontDraw)]
  public float _startLightIntensity;
  [DrawIf("_noPredefinedStartValue", false, DrawIfAttribute.DisablingType.DontDraw)]
  public float _startLaserIntensity;
  [Header("End")]
  public float _endLightIntensity;
  public float _endLaserIntensity;
  public bool _disableWhenFinished;
  public float _blend;
  protected bool _initialized;
  protected float _originalLightIntensity;
  protected float _originalLaserIntensity;
  protected TubeBloomPrePassLight[] _tubeLights;
  protected DirectionalLight[] _directionalLights;
  protected bool _started;
  protected bool _finished;
  protected float _firstFrameLightIntensity;
  protected float _firstFrameLaserIntensity;

  public override void ProcessFrame(Playable playable, FrameData info, object playerData)
  {
    if (this._noPredefinedStartValue && playable.GetTime<Playable>() <= 0.0)
      return;
    TimelineArrayReference timelineArrayReference = playerData as TimelineArrayReference;
    if (timelineArrayReference._tubeLightArray.Length == 0 && timelineArrayReference._directionalLights.Length == 0)
      return;
    TubeLightIntensityBehaviour behaviour = ((ScriptPlayable<TubeLightIntensityBehaviour>) playable).GetBehaviour();
    if (!this._initialized)
    {
      this._tubeLights = timelineArrayReference._tubeLightArray;
      this._directionalLights = timelineArrayReference._directionalLights;
      if (this._directionalLights.Length != 0)
        this._originalLightIntensity = this._directionalLights[0].intensity;
      if (this._tubeLights.Length != 0)
        this._originalLaserIntensity = this._tubeLights[0].color.a;
      this._initialized = true;
    }
    if (this._noPredefinedStartValue && !this._started)
    {
      this._started = true;
      if (this._directionalLights.Length != 0)
        this._firstFrameLightIntensity = this._directionalLights[0].intensity;
      if (this._tubeLights.Length != 0)
        this._firstFrameLaserIntensity = this._tubeLights[0].color.a;
    }
    if (this._disableWhenFinished)
    {
      if ((double) behaviour._blend >= 1.0)
      {
        this._finished = true;
        this.EnableObjects(false);
        return;
      }
      if (this._finished)
      {
        this._finished = false;
        this.EnableObjects(true);
      }
    }
    float num = Mathf.Lerp(this._noPredefinedStartValue ? this._firstFrameLightIntensity : behaviour._startLightIntensity, behaviour._endLightIntensity, behaviour._blend);
    float alpha = Mathf.Lerp(this._noPredefinedStartValue ? this._firstFrameLaserIntensity : behaviour._startLaserIntensity, behaviour._endLaserIntensity, behaviour._blend);
    foreach (TubeBloomPrePassLight tubeLight in this._tubeLights)
      tubeLight.color = tubeLight.color.ColorWithAlpha(alpha);
    foreach (DirectionalLight directionalLight in this._directionalLights)
      directionalLight.intensity = num;
  }

  public virtual void EnableObjects(bool on)
  {
    foreach (Component tubeLight in this._tubeLights)
      tubeLight.gameObject.SetActive(on);
    foreach (Component directionalLight in this._directionalLights)
      directionalLight.gameObject.SetActive(on);
  }

  public override void OnPlayableDestroy(Playable playable)
  {
    if (!this._initialized)
      return;
    foreach (TubeBloomPrePassLight tubeLight in this._tubeLights)
      tubeLight.color = tubeLight.color.ColorWithAlpha(this._originalLaserIntensity);
    foreach (DirectionalLight directionalLight in this._directionalLights)
      directionalLight.intensity = this._originalLightIntensity;
    if (!this._disableWhenFinished)
      return;
    this.EnableObjects(true);
  }
}
