// Decompiled with JetBrains decompiler
// Type: SliderIntensityEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

public class SliderIntensityEffect : MonoBehaviour
{
  [SerializeField]
  protected float _longSliderHeadIntensity = 0.8f;
  [SerializeField]
  protected float _shortSliderHeadIntensity = 1.2f;
  [SerializeField]
  protected float _tailIntensity = 1.5f;
  [Space]
  [SerializeField]
  protected float _fadeOutDuration = 0.1f;
  [SerializeField]
  protected float _stayOffDuration = 1f;
  [Space]
  [SerializeField]
  protected float _flashBoost = 2f;
  [SerializeField]
  protected float _flashInDuration = 0.3f;
  [SerializeField]
  protected float _flashOutDuration = 0.7f;
  [Inject]
  protected readonly AudioTimeSyncController _audioTimeSyncController;
  [Inject]
  protected readonly SliderIntensityEffect.InitData _initData;
  protected float _coreIntensity;
  protected float _effectIntensity;
  protected float _halfJumpDuration;
  protected float _sliderDuration;
  protected float headIntensity;
  protected SliderIntensityEffect.IntensityCalculationDelegate _intensityCalculationDelegate;
  protected SliderIntensityEffect.FadeElement[] _dipEffectFadeElements;
  protected SliderIntensityEffect.FadeElement[] _flashEffectFadeElements;
  protected SliderIntensityEffect.FadeElement[] _fadeInEffectFadeElements;

  public event System.Action fadeInDidStartEvent;

  public float intensity => this._coreIntensity * this._effectIntensity * (float) this._initData.hapticFeedback;

  public float colorIntensity => this._coreIntensity * this._effectIntensity * this._initData.sliderColorIntensity;

  public virtual void Awake()
  {
    this._dipEffectFadeElements = new SliderIntensityEffect.FadeElement[3]
    {
      new SliderIntensityEffect.FadeElement(EaseType.OutQuad, 1f, 0.0f),
      new SliderIntensityEffect.FadeElement(EaseType.Linear, 0.0f, 0.0f),
      new SliderIntensityEffect.FadeElement(EaseType.Linear, 0.0f, 1f, (System.Action) (() =>
      {
        System.Action fadeInDidStartEvent = this.fadeInDidStartEvent;
        if (fadeInDidStartEvent == null)
          return;
        fadeInDidStartEvent();
      }))
    };
    this._flashEffectFadeElements = new SliderIntensityEffect.FadeElement[2]
    {
      new SliderIntensityEffect.FadeElement(EaseType.OutQuad, 1f, this._flashBoost),
      new SliderIntensityEffect.FadeElement(EaseType.OutQuart, this._flashBoost, 1f)
    };
    this._fadeInEffectFadeElements = new SliderIntensityEffect.FadeElement[1]
    {
      new SliderIntensityEffect.FadeElement(EaseType.Linear, 0.0f, 1f, (System.Action) (() =>
      {
        System.Action fadeInDidStartEvent = this.fadeInDidStartEvent;
        if (fadeInDidStartEvent == null)
          return;
        fadeInDidStartEvent();
      }))
    };
  }

  public virtual void Init(float sliderDuration, float halfJumpDuration, bool startVisible)
  {
    this._halfJumpDuration = halfJumpDuration;
    this._sliderDuration = sliderDuration;
    this.headIntensity = (double) sliderDuration < (double) halfJumpDuration ? this._shortSliderHeadIntensity : this._longSliderHeadIntensity;
    this._coreIntensity = this.headIntensity;
    this._effectIntensity = startVisible ? 1f : 0.0f;
    float num = Mathf.Max(sliderDuration - this._fadeOutDuration - this._stayOffDuration, 0.1f);
    this._dipEffectFadeElements[0].duration = this._fadeOutDuration;
    this._dipEffectFadeElements[1].duration = this._stayOffDuration;
    this._dipEffectFadeElements[2].duration = num;
    this._flashEffectFadeElements[0].duration = this._flashInDuration;
    this._flashEffectFadeElements[1].duration = this._flashOutDuration;
    this._fadeInEffectFadeElements[0].duration = num;
  }

  public virtual void ManualUpdate(float timeSinceHeadNoteJump) => this._coreIntensity = Mathf.Lerp(this.headIntensity, this._tailIntensity, (timeSinceHeadNoteJump - this._halfJumpDuration) / this._sliderDuration);

  public virtual IEnumerator ProcessEffectCoroutine(
    IEnumerable<SliderIntensityEffect.FadeElement> fadeElements)
  {
    foreach (SliderIntensityEffect.FadeElement fadeElement in fadeElements)
    {
      System.Action startCallback = fadeElement.startCallback;
      if (startCallback != null)
        startCallback();
      float startTime = this._audioTimeSyncController.songTime;
      float num;
      while ((double) (num = this._audioTimeSyncController.songTime - startTime) < (double) fadeElement.duration)
      {
        float t = Interpolation.Interpolate(num / fadeElement.duration, fadeElement.easeType);
        this._effectIntensity = Mathf.LerpUnclamped(fadeElement.startIntensity, fadeElement.endIntensity, t);
        yield return (object) null;
      }
    }
  }

  public virtual void StartIntensityDipEffect()
  {
    this.StopAllCoroutines();
    this.StartCoroutine(this.ProcessEffectCoroutine((IEnumerable<SliderIntensityEffect.FadeElement>) this._dipEffectFadeElements));
  }

  public virtual void StartIntensityFadeInEffect()
  {
    this.StopAllCoroutines();
    this.StartCoroutine(this.ProcessEffectCoroutine((IEnumerable<SliderIntensityEffect.FadeElement>) this._fadeInEffectFadeElements));
  }

  public virtual void StartFlashEffect()
  {
    this.StopAllCoroutines();
    this.StartCoroutine(this.ProcessEffectCoroutine((IEnumerable<SliderIntensityEffect.FadeElement>) this._flashEffectFadeElements));
  }

  [CompilerGenerated]
  public virtual void m_CAwakem_Eb__29_0()
  {
    System.Action fadeInDidStartEvent = this.fadeInDidStartEvent;
    if (fadeInDidStartEvent == null)
      return;
    fadeInDidStartEvent();
  }

  [CompilerGenerated]
  public virtual void m_CAwakem_Eb__29_1()
  {
    System.Action fadeInDidStartEvent = this.fadeInDidStartEvent;
    if (fadeInDidStartEvent == null)
      return;
    fadeInDidStartEvent();
  }

  public class InitData
  {
    public readonly float sliderColorIntensity;
    public readonly int hapticFeedback = 1;

    public InitData(ArcVisibilityType arcVisibilityType, bool hapticFeedback)
    {
      if (!hapticFeedback)
        this.hapticFeedback = 0;
      switch (arcVisibilityType)
      {
        case ArcVisibilityType.None:
          this.sliderColorIntensity = 0.0f;
          break;
        case ArcVisibilityType.Low:
          this.sliderColorIntensity = 0.5f;
          break;
        case ArcVisibilityType.Standard:
          this.sliderColorIntensity = 1f;
          break;
        case ArcVisibilityType.High:
          this.sliderColorIntensity = 1.5f;
          break;
      }
    }
  }

  public class FadeElement
  {
    public float duration;
    public readonly float startIntensity;
    public readonly float endIntensity;
    public readonly EaseType easeType;
    public readonly System.Action startCallback;

    public FadeElement(
      EaseType easeType,
      float startIntensity,
      float endIntensity,
      System.Action startCallback = null)
    {
      this.easeType = easeType;
      this.startIntensity = startIntensity;
      this.endIntensity = endIntensity;
      this.startCallback = startCallback;
    }
  }

  public delegate float IntensityCalculationDelegate(
    float timeSinceLastSection,
    float timeSinceHeadNoteJump);
}
