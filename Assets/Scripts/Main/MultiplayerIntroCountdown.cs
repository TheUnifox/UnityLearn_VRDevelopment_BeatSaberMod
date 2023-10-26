// Decompiled with JetBrains decompiler
// Type: MultiplayerIntroCountdown
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Polyglot;
using System.Collections;
using Tweening;
using UnityEngine;
using Zenject;

public class MultiplayerIntroCountdown : MonoBehaviour
{
  [SerializeField]
  protected float _textAppearDuration = 0.5f;
  [SerializeField]
  protected float _textDisappearDuration = 1.35f;
  [SerializeField]
  protected float _goDisappearDuration = 2f;
  [SerializeField]
  protected float _partsDistance = 10f;
  [SerializeField]
  protected Vector3 _startLocalPosition = Vector3.zero;
  [SerializeField]
  protected Vector3 _targetLocalPosition = Vector3.zero;
  [Space]
  [SerializeField]
  protected AudioClip _readyClip;
  [SerializeField]
  protected AudioClip _setClip;
  [SerializeField]
  protected AudioClip _goClip;
  [SerializeField]
  protected AudioClip _buildUpClip;
  [Space]
  [SerializeField]
  protected MultiplayerIntroCountdownTextController _textController0;
  [SerializeField]
  protected MultiplayerIntroCountdownTextController _textController1;
  [SerializeField]
  protected AudioSource _audioSource;
  [Space]
  [SerializeField]
  protected MultiplayerOffsetPositionByLocalPlayerPosition _multiplayerOffsetByLocalPlayerPosition;
  [Inject]
  protected readonly TimeTweeningManager _tweeningManager;
  protected MultiplayerIntroCountdownTextController _currentTextController;
  protected float _fontSize;
  protected float _alpha;

  public float textAppearDuration => this._textAppearDuration;

  public virtual void Awake()
  {
    this._textController0.hide = true;
    this._textController1.hide = true;
    this._fontSize = this._textController0.fontSize;
    this._alpha = this._textController0.alpha;
  }

  protected virtual void OnDestroy()
  {
    if (!((UnityEngine.Object) this._tweeningManager != (UnityEngine.Object) null))
      return;
    this._tweeningManager.KillAllTweens((object) this);
  }

  public virtual void StartCountdown(float seconds, float delay, float durationMultiplier)
  {
    this._tweeningManager.KillAllTweens((object) this);
    this.StopAllCoroutines();
    this.StartCoroutine(this.CountdownRoutine(seconds, delay, durationMultiplier));
  }

  public virtual IEnumerator CountdownRoutine(float seconds, float delay, float durationMultiplier)
  {
    MultiplayerIntroCountdown multiplayerIntroCountdown = this;
    multiplayerIntroCountdown._multiplayerOffsetByLocalPlayerPosition.SetEnabled(true);
    if ((double) delay > 0.0)
      yield return (object) new WaitForSeconds(delay);
    float soundDelayAfterText = Mathf.Max(multiplayerIntroCountdown._textAppearDuration - 0.1f, 0.0f);
    // ISSUE: explicit non-virtual call
    multiplayerIntroCountdown.StartCoroutine(multiplayerIntroCountdown.PlayDelayed(multiplayerIntroCountdown._buildUpClip, (float)((double)seconds * (double)durationMultiplier - 2.0) + soundDelayAfterText));
    // ISSUE: explicit non-virtual call
    multiplayerIntroCountdown.StartCoroutine(multiplayerIntroCountdown.PlayDelayed(multiplayerIntroCountdown._readyClip, soundDelayAfterText * durationMultiplier));
    // ISSUE: explicit non-virtual call
    multiplayerIntroCountdown.StartCoroutine(multiplayerIntroCountdown.PhaseRoutine(Localization.Get("COUNTDOWN_READY"), multiplayerIntroCountdown._textAppearDuration * durationMultiplier, multiplayerIntroCountdown._textDisappearDuration * durationMultiplier));
    yield return (object) new WaitForSeconds(seconds * 0.5f * durationMultiplier);
    // ISSUE: explicit non-virtual call
    multiplayerIntroCountdown.StartCoroutine(multiplayerIntroCountdown.PlayDelayed(multiplayerIntroCountdown._setClip, soundDelayAfterText * durationMultiplier));
    // ISSUE: explicit non-virtual call
    multiplayerIntroCountdown.StartCoroutine(multiplayerIntroCountdown.PhaseRoutine(Localization.Get("COUNTDOWN_SET"), multiplayerIntroCountdown._textAppearDuration * durationMultiplier, multiplayerIntroCountdown._textDisappearDuration * durationMultiplier));
    yield return (object) new WaitForSeconds(seconds * 0.5f * durationMultiplier);
    // ISSUE: explicit non-virtual call
    multiplayerIntroCountdown.StartCoroutine(multiplayerIntroCountdown.PlayDelayed(multiplayerIntroCountdown._goClip, soundDelayAfterText * durationMultiplier));
    // ISSUE: explicit non-virtual call
    multiplayerIntroCountdown.StartCoroutine(multiplayerIntroCountdown.PhaseRoutine(Localization.Get("COUNTDOWN_GO"), multiplayerIntroCountdown._textAppearDuration * durationMultiplier, multiplayerIntroCountdown._goDisappearDuration * durationMultiplier));
    multiplayerIntroCountdown._multiplayerOffsetByLocalPlayerPosition.SetEnabled(false);
  }

  public virtual IEnumerator PlayDelayed(AudioClip audioClip, float delay)
  {
    if ((double) delay > 0.0)
      yield return (object) new WaitForSeconds(delay);
    this._audioSource.PlayOneShot(audioClip);
  }

  public virtual IEnumerator PhaseRoutine(
    string text,
    float appearDuration,
    float disappearDuration)
  {
    MultiplayerIntroCountdown owner = this;
    owner._currentTextController = (UnityEngine.Object) owner._currentTextController == (UnityEngine.Object) null || (UnityEngine.Object) owner._currentTextController == (UnityEngine.Object) owner._textController0 ? owner._textController1 : owner._textController0;
    MultiplayerIntroCountdownTextController textController = owner._currentTextController;
    textController.SetText(text);
    owner._tweeningManager.AddTween((Tween) new FloatTween(0.0f, owner._fontSize, (System.Action<float>) (val => textController.fontSize = val), appearDuration, EaseType.InExpo), (object) owner);
    owner._tweeningManager.AddTween((Tween) new FloatTween(0.0f, owner._alpha, (System.Action<float>) (val => textController.alpha = val), appearDuration, EaseType.InQuart), (object) owner);
    owner._tweeningManager.AddTween((Tween) new Vector3Tween(owner._startLocalPosition, owner._targetLocalPosition, (System.Action<Vector3>) (pos => textController.transform.localPosition = pos), appearDuration + disappearDuration, EaseType.InQuint), (object) owner);
    owner._tweeningManager.AddTween((Tween) new FloatTween(0.0f, 1f, (System.Action<float>) (f => textController.SetDistances(this._partsDistance * f)), appearDuration + disappearDuration, EaseType.InQuint), (object) owner);
    yield return (object) null;
    textController.gameObject.SetActive(true);
    yield return (object) new WaitForSeconds(appearDuration);
    owner._tweeningManager.AddTween((Tween) new FloatTween(owner._alpha, 0.0f, (System.Action<float>) (val => textController.alpha = val), disappearDuration, EaseType.InExpo), (object) owner);
    yield return (object) new WaitForSeconds(disappearDuration);
    textController.gameObject.SetActive(false);
  }
}
