// Decompiled with JetBrains decompiler
// Type: EulaViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class EulaViewController : ViewController
{
  [Header("Buttons")]
  [SerializeField]
  protected Button _agreeButton;
  [SerializeField]
  protected Button _doNotAgreeButton;
  [SerializeField]
  protected Button _continueButton;
  [SerializeField]
  protected Image _disableButtonsProgress;
  [SerializeField]
  protected float _disabledButtonDelay = 5f;
  [Header("Texts")]
  [SerializeField]
  protected TextPageScrollView _textPageScrollView;
  [SerializeField]
  protected LocalizedTextAsset _eulaLocalizedTextAsset;
  [Header("Update Notice")]
  [SerializeField]
  [LocalizationKey]
  protected string _defaultEulaHeaderLocalizationKey;
  [SerializeField]
  [LocalizationKey]
  protected string _updateNoticeLocalizationKey;
  [Inject]
  protected readonly EulaViewController.InitData _initData;
  protected bool _showUpdate;
  protected bool _showOnlyContinueButton;
  protected Coroutine _buttonsCoroutine;

  public event System.Action<EulaViewController.ButtonType> didFinishEvent;

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (firstActivation)
    {
      this.buttonBinder.AddBinding(this._agreeButton, (System.Action) (() =>
      {
        System.Action<EulaViewController.ButtonType> didFinishEvent = this.didFinishEvent;
        if (didFinishEvent == null)
          return;
        didFinishEvent(EulaViewController.ButtonType.Agree);
      }));
      this.buttonBinder.AddBinding(this._doNotAgreeButton, (System.Action) (() =>
      {
        System.Action<EulaViewController.ButtonType> didFinishEvent = this.didFinishEvent;
        if (didFinishEvent == null)
          return;
        didFinishEvent(EulaViewController.ButtonType.DoNotAgree);
      }));
      this.buttonBinder.AddBinding(this._continueButton, (System.Action) (() =>
      {
        System.Action<EulaViewController.ButtonType> didFinishEvent = this.didFinishEvent;
        if (didFinishEvent == null)
          return;
        didFinishEvent(EulaViewController.ButtonType.Agree);
      }));
      this._agreeButton.gameObject.SetActive(!this._showOnlyContinueButton);
      this._doNotAgreeButton.gameObject.SetActive(!this._showOnlyContinueButton && this._initData.showDoNotAgreeButton);
      this._continueButton.gameObject.SetActive(this._showOnlyContinueButton);
      this._textPageScrollView.SetText((this._showUpdate ? Localization.Get(this._updateNoticeLocalizationKey) + "\n\n" : Localization.Get(this._defaultEulaHeaderLocalizationKey) + "\n\n") + this._eulaLocalizedTextAsset.localizedText);
    }
    if (!addedToHierarchy)
      return;
    this._agreeButton.interactable = false;
    this._doNotAgreeButton.interactable = false;
    if (this._showOnlyContinueButton)
      return;
    this._buttonsCoroutine = PersistentSingleton<SharedCoroutineStarter>.instance.StartCoroutine(this.EnableButtonsCoroutine(this._disabledButtonDelay));
  }

  protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
  {
    if (!removedFromHierarchy || this._buttonsCoroutine == null || !PersistentSingleton<SharedCoroutineStarter>.IsSingletonAvailable)
      return;
    PersistentSingleton<SharedCoroutineStarter>.instance.StopCoroutine(this._buttonsCoroutine);
    this._buttonsCoroutine = (Coroutine) null;
  }

  public virtual void Init(bool showUpdate, bool showOnlyContinueButton)
  {
    this._showUpdate = showUpdate;
    this._showOnlyContinueButton = showOnlyContinueButton;
  }

  public virtual IEnumerator EnableButtonsCoroutine(float delay)
  {
    float startProgressBarWidth = this._disableButtonsProgress.rectTransform.rect.width;
    float elapsedTime = 0.0f;
    while ((double) elapsedTime < (double) delay)
    {
      float num = elapsedTime / delay;
      this._disableButtonsProgress.color = this._disableButtonsProgress.color.ColorWithAlpha(1f - num);
      this._disableButtonsProgress.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float) ((double) startProgressBarWidth * (1.0 - (double) num) * 0.5));
      elapsedTime += Time.deltaTime;
      yield return (object) null;
    }
    this._agreeButton.interactable = true;
    this._doNotAgreeButton.interactable = true;
    this._buttonsCoroutine = (Coroutine) null;
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__18_0()
  {
    System.Action<EulaViewController.ButtonType> didFinishEvent = this.didFinishEvent;
    if (didFinishEvent == null)
      return;
    didFinishEvent(EulaViewController.ButtonType.Agree);
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__18_1()
  {
    System.Action<EulaViewController.ButtonType> didFinishEvent = this.didFinishEvent;
    if (didFinishEvent == null)
      return;
    didFinishEvent(EulaViewController.ButtonType.DoNotAgree);
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__18_2()
  {
    System.Action<EulaViewController.ButtonType> didFinishEvent = this.didFinishEvent;
    if (didFinishEvent == null)
      return;
    didFinishEvent(EulaViewController.ButtonType.Agree);
  }

  public enum ButtonType
  {
    Agree,
    DoNotAgree,
  }

  public class InitData
  {
    public readonly bool showDoNotAgreeButton;

    public InitData(bool showDoNotAgreeButton) => this.showDoNotAgreeButton = showDoNotAgreeButton;
  }
}
