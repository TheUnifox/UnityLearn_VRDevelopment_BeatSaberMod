// Decompiled with JetBrains decompiler
// Type: ScaleAnimator
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;
using Tweening;
using UnityEngine;
using Zenject;

public class ScaleAnimator : MonoBehaviour
{
  [SerializeField]
  protected float _displayedScale = 1f;
  [SerializeField]
  protected Transform _targetTransform;
  [Inject]
  protected readonly TimeTweeningManager _tweeningManager;
  protected Tween<float> _scaleUpTween;
  protected Tween<float> _scaleDownTween;
  protected bool _initialized;

  public virtual void OnDestroy()
  {
    if (!((UnityEngine.Object) this._tweeningManager != (UnityEngine.Object) null))
      return;
    this._tweeningManager.KillAllTweens((object) this);
  }

  public virtual void InitIfNeeded()
  {
    if (this._initialized)
      return;
    this._initialized = true;
    this._scaleUpTween = (Tween<float>) new FloatTween(0.0f, this._displayedScale, (System.Action<float>) (val => this._targetTransform.localScale = new Vector3(val, val, val)), 0.0f, EaseType.Linear);
    Tween<float> scaleUpTween = this._scaleUpTween;
    scaleUpTween.onStart = scaleUpTween.onStart + (System.Action) (() => this._targetTransform.gameObject.SetActive(true));
    this._scaleDownTween = (Tween<float>) new FloatTween(this._displayedScale, 0.0f, (System.Action<float>) (val => this._targetTransform.localScale = new Vector3(val, val, val)), 0.0f, EaseType.Linear);
    Tween<float> scaleDownTween = this._scaleDownTween;
    scaleDownTween.onCompleted = scaleDownTween.onCompleted + (System.Action) (() => this._targetTransform.gameObject.SetActive(false));
  }

  public virtual void SetPositionAndRotation(Vector3 position, Quaternion rotation) => this.transform.SetPositionAndRotation(position, rotation);

  public virtual void HideInstant()
  {
    this.InitIfNeeded();
    this._scaleUpTween.Kill();
    this._scaleDownTween.Kill();
    this._targetTransform.gameObject.SetActive(false);
    this._targetTransform.localScale = Vector3.zero;
  }

  public virtual void ShowInstant()
  {
    this.InitIfNeeded();
    this._scaleUpTween.Kill();
    this._scaleDownTween.Kill();
    this._targetTransform.gameObject.SetActive(true);
    this._targetTransform.localScale = new Vector3(this._displayedScale, this._displayedScale, this._displayedScale);
  }

  public virtual void Animate(bool show, float duration, EaseType easeType, float delay = 0.0f)
  {
    this.InitIfNeeded();
    this._scaleUpTween.Kill();
    this._scaleDownTween.Kill();
    if (show)
    {
      this._scaleUpTween.duration = duration;
      this._scaleUpTween.easeType = easeType;
      this._scaleUpTween.fromValue = this._targetTransform.localScale.x;
      this._scaleUpTween.delay = delay;
      this._targetTransform.gameObject.SetActive(true);
      this._tweeningManager.RestartTween((Tween) this._scaleUpTween, (object) this);
    }
    else
    {
      this._scaleDownTween.duration = duration;
      this._scaleDownTween.easeType = easeType;
      this._scaleDownTween.fromValue = this._targetTransform.localScale.x;
      this._scaleDownTween.delay = delay;
      this._tweeningManager.RestartTween((Tween) this._scaleDownTween, (object) this);
    }
  }

  [CompilerGenerated]
  public virtual void m_CInitIfNeededm_Eb__7_0(float val) => this._targetTransform.localScale = new Vector3(val, val, val);

  [CompilerGenerated]
  public virtual void m_CInitIfNeededm_Eb__7_2() => this._targetTransform.gameObject.SetActive(true);

  [CompilerGenerated]
  public virtual void m_CInitIfNeededm_Eb__7_1(float val) => this._targetTransform.localScale = new Vector3(val, val, val);

  [CompilerGenerated]
  public virtual void m_CInitIfNeededm_Eb__7_3() => this._targetTransform.gameObject.SetActive(false);
}
