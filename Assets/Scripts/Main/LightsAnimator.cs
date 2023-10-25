// Decompiled with JetBrains decompiler
// Type: LightsAnimator
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections;
using Tweening;
using UnityEngine;
using Zenject;

public class LightsAnimator : MonoBehaviour
{
  [SerializeField]
  protected TubeBloomPrePassLight[] _lights;
  [SerializeField]
  protected DirectionalLight[] _directionalLights;
  [Inject]
  protected readonly TimeTweeningManager _tweeningManager;
  protected ColorTween _tween;

  public virtual void Awake() => this._tween = new ColorTween(Color.white, Color.white, new System.Action<Color>(this.SetLightsColor), 0.0f, EaseType.Linear);

  public virtual void OnDestroy()
  {
    if (!((UnityEngine.Object) this._tweeningManager != (UnityEngine.Object) null))
      return;
    this._tweeningManager.KillAllTweens((object) this);
  }

  public virtual void AnimateToColor(Color color, float duration, EaseType easeType)
  {
    if ((this._lights == null || this._lights.Length == 0) && (this._directionalLights == null || this._directionalLights.Length == 0))
      return;
    if ((double) duration < 9.9999997473787516E-05)
    {
      this.SetColor(color);
    }
    else
    {
      TubeBloomPrePassLight[] lights = this._lights;
      this._tween.fromValue = (lights != null ? (lights.Length != 0 ? 1 : 0) : 0) != 0 ? this._lights[0].color : this._directionalLights[0].color;
      this._tween.toValue = color;
      this._tween.duration = duration;
      this._tween.easeType = easeType;
      this._tweeningManager.RestartTween((Tween) this._tween, (object) this);
    }
  }

  public virtual void SetColor(Color color)
  {
    this._tween.Kill();
    this.SetLightsColor(color);
  }

  public virtual void SetLightsColor(Color color)
  {
    foreach (TubeBloomPrePassLight light in this._lights)
      light.color = color;
    foreach (DirectionalLight directionalLight in this._directionalLights)
      directionalLight.color = color * color.a;
  }

  public virtual void SetLightsWidth(float width)
  {
    foreach (TubeBloomPrePassLight light in this._lights)
      light.width = width;
  }

  public virtual void DisableDirectionalLights(float delay)
  {
    if (this._directionalLights == null || this._directionalLights.Length == 0)
      return;
    this.StartCoroutine(this.DisableDirectionalLightsCoroutine(delay));
  }

  public virtual IEnumerator DisableDirectionalLightsCoroutine(float delay)
  {
    if ((double) delay > 0.0)
      yield return (object) new WaitForSeconds(delay);
    foreach (Component directionalLight in this._directionalLights)
      directionalLight.gameObject.SetActive(false);
  }
}
