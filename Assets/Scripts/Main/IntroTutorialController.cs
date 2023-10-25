// Decompiled with JetBrains decompiler
// Type: IntroTutorialController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections;
using UnityEngine;
using Zenject;

public class IntroTutorialController : MonoBehaviour
{
  [SerializeField]
  protected IntroTutorialRing _redRing;
  [SerializeField]
  protected IntroTutorialRing _blueRing;
  [SerializeField]
  protected GameObject _redRingWrapper;
  [SerializeField]
  protected GameObject _blueRingWrapper;
  [SerializeField]
  protected CanvasGroup _textCanvasGroup;
  [SerializeField]
  protected ParticleSystem _shockWavePS;
  [Space]
  [SerializeField]
  protected BloomFogEnvironmentParams _finishForParams;
  [Inject]
  protected readonly BloomFogParamsAnimator _bloomFogAnimator;
  [Inject]
  protected readonly IGamePause _gamePause;
  protected bool _showingFinishAnimation;
  protected bool _redRingWrapperActive;
  protected bool _blueRingWrapperActive;

  public event System.Action introTutorialDidFinishEvent;

  public virtual void Start()
  {
    this._gamePause.didPauseEvent += new System.Action(this.HandleGameDidPause);
    this._gamePause.didResumeEvent += new System.Action(this.HandlegameDidResume);
  }

  public virtual void OnDestroy() => this.CleanUp();

  public virtual void Update()
  {
    if (!this._redRing.enabled || !this._redRing.fullyActivated || !this._blueRing.enabled || !this._blueRing.fullyActivated)
      return;
    this.ShowFinishAnimation();
  }

  public virtual void CleanUp()
  {
    if (this._gamePause == null)
      return;
    this._gamePause.didPauseEvent -= new System.Action(this.HandleGameDidPause);
    this._gamePause.didResumeEvent -= new System.Action(this.HandlegameDidResume);
  }

  public virtual void HandleGameDidPause()
  {
    this._redRingWrapperActive = this._redRingWrapper.activeSelf;
    this._blueRingWrapperActive = this._redRingWrapper.activeSelf;
    this._redRingWrapper.SetActive(false);
    this._blueRingWrapper.SetActive(false);
  }

  public virtual void HandlegameDidResume()
  {
    this._redRingWrapper.SetActive(this._redRingWrapperActive);
    this._blueRingWrapper.SetActive(this._blueRingWrapperActive);
  }

  public virtual void ShowFinishAnimation()
  {
    if (this._showingFinishAnimation)
      return;
    this._showingFinishAnimation = true;
    this._redRing.enabled = false;
    this._blueRing.enabled = false;
    this._shockWavePS.Emit(1);
    this.StartCoroutine(this.ShowFinishAnimationCoroutine());
  }

  public virtual IEnumerator ShowFinishAnimationCoroutine()
  {
    float elapsedTime = 0.0f;
    float duration = 0.2f;
    this._bloomFogAnimator.AnimateBloomFogParamsChange(this._finishForParams, duration);
    while ((double) elapsedTime < (double) duration)
    {
      this.SetFinishAnimationParams(elapsedTime / duration);
      elapsedTime += Time.deltaTime;
      yield return (object) null;
    }
    this.SetFinishAnimationParams(1f);
    this._redRingWrapper.SetActive(false);
    this._blueRingWrapper.SetActive(false);
    this._textCanvasGroup.gameObject.SetActive(false);
    System.Action tutorialDidFinishEvent = this.introTutorialDidFinishEvent;
    if (tutorialDidFinishEvent != null)
      tutorialDidFinishEvent();
    this.CleanUp();
  }

  public virtual void SetFinishAnimationParams(float progress)
  {
    this._textCanvasGroup.alpha = 1f - progress;
    this._redRing.alpha = 1f - progress;
    this._blueRing.alpha = 1f - progress;
  }
}
