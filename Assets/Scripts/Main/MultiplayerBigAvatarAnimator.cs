// Decompiled with JetBrains decompiler
// Type: MultiplayerBigAvatarAnimator
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;
using Tweening;
using UnityEngine;
using Zenject;

public class MultiplayerBigAvatarAnimator : MonoBehaviour
{
  [SerializeField]
  protected float _displayedScale = 7f;
  [SerializeField]
  protected HologramRays _hologramRays;
  [SerializeField]
  protected Transform _avatarTransform;
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
    this._scaleUpTween = (Tween<float>) new FloatTween(0.0f, this._displayedScale, (System.Action<float>) (val => this._avatarTransform.localScale = Vector3.one * val), 0.0f, EaseType.Linear);
    this._scaleDownTween = (Tween<float>) new FloatTween(this._displayedScale, 0.0f, (System.Action<float>) (val => this._avatarTransform.localScale = Vector3.one * val), 0.0f, EaseType.Linear);
    Tween<float> scaleDownTween = this._scaleDownTween;
    scaleDownTween.onCompleted = scaleDownTween.onCompleted + (System.Action) (() => this._avatarTransform.gameObject.SetActive(false));
  }

  public virtual void SetPositionAndRotation(Vector3 position, Quaternion rotation) => this.transform.SetPositionAndRotation(position, rotation);

  public virtual void HideInstant()
  {
    this.InitIfNeeded();
    this._scaleUpTween.Kill();
    this._scaleDownTween.Kill();
    this._avatarTransform.gameObject.SetActive(false);
    this._avatarTransform.localScale = Vector3.zero;
  }

  public virtual void Animate(bool show, float duration, EaseType easeType)
  {
    this.InitIfNeeded();
    this._scaleUpTween.Kill();
    this._scaleDownTween.Kill();
    if ((double) duration > 0.0)
    {
      if (show)
      {
        this._scaleUpTween.duration = duration;
        this._scaleUpTween.easeType = easeType;
        this._scaleUpTween.fromValue = this._avatarTransform.localScale.x;
        this._avatarTransform.gameObject.SetActive(true);
        this._tweeningManager.RestartTween((Tween) this._scaleUpTween, (object) this);
      }
      else
      {
        this._scaleDownTween.duration = duration;
        this._scaleDownTween.easeType = easeType;
        this._scaleDownTween.fromValue = this._avatarTransform.localScale.x;
        this._tweeningManager.RestartTween((Tween) this._scaleDownTween, (object) this);
      }
    }
    else
    {
      this._avatarTransform.gameObject.SetActive(show);
      this._avatarTransform.localScale = show ? Vector3.one * this._displayedScale : Vector3.zero;
    }
    this._hologramRays.Animate(show, duration, easeType);
  }

  [CompilerGenerated]
  public virtual void m_CInitIfNeededm_Eb__8_0(float val) => this._avatarTransform.localScale = Vector3.one * val;

  [CompilerGenerated]
  public virtual void m_CInitIfNeededm_Eb__8_1(float val) => this._avatarTransform.localScale = Vector3.one * val;

  [CompilerGenerated]
  public virtual void m_CInitIfNeededm_Eb__8_2() => this._avatarTransform.gameObject.SetActive(false);
}
