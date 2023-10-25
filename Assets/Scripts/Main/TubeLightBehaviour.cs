// Decompiled with JetBrains decompiler
// Type: TubeLightBehaviour
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class TubeLightBehaviour : PlayableBehaviour
{
  public bool _noPredefinedStartValue;
  [DrawIf("_noPredefinedStartValue", false, DrawIfAttribute.DisablingType.DontDraw)]
  public ColorSO startColor;
  public ColorSO endColor;
  public float blend;
  protected bool _initialized;
  protected Color _originalColor;
  protected TubeBloomPrePassLight[] _tubeLights;
  protected DirectionalLight[] _directionalLights;
  protected bool started;
  protected Color _firstFrameColor;

  public override void ProcessFrame(Playable playable, FrameData info, object playerData)
  {
    if (this._noPredefinedStartValue && playable.GetTime<Playable>() <= 0.0)
      return;
    TimelineArrayReference timelineArrayReference = playerData as TimelineArrayReference;
    if (timelineArrayReference._tubeLightArray.Length == 0 && timelineArrayReference._directionalLights.Length == 0)
      return;
    TubeLightBehaviour behaviour = ((ScriptPlayable<TubeLightBehaviour>) playable).GetBehaviour();
    if (!this._initialized)
    {
      this._tubeLights = timelineArrayReference._tubeLightArray;
      this._directionalLights = timelineArrayReference._directionalLights;
      this._originalColor = this._tubeLights.Length != 0 ? this._tubeLights[0].color : this._directionalLights[0].color;
      this._initialized = true;
    }
    if (this._noPredefinedStartValue && !this.started)
    {
      this.started = true;
      this._firstFrameColor = this._tubeLights.Length != 0 ? this._tubeLights[0].color : this._directionalLights[0].color;
    }
    Color color = this._noPredefinedStartValue ? Color.Lerp(behaviour._firstFrameColor, (Color) behaviour.endColor, behaviour.blend) : Color.Lerp((Color) behaviour.startColor, (Color) behaviour.endColor, behaviour.blend);
    foreach (TubeBloomPrePassLight tubeLight in this._tubeLights)
      tubeLight.color = color;
    foreach (DirectionalLight directionalLight in this._directionalLights)
      directionalLight.color = color * color.a;
  }

  public override void OnPlayableDestroy(Playable playable)
  {
    if (!this._initialized)
      return;
    foreach (TubeBloomPrePassLight tubeLight in this._tubeLights)
      tubeLight.color = this._originalColor;
    foreach (DirectionalLight directionalLight in this._directionalLights)
      directionalLight.color = this._originalColor * this._originalColor.a;
  }

  public enum ParameterType
  {
    Values,
    References,
  }
}
