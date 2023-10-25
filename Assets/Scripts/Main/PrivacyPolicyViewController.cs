// Decompiled with JetBrains decompiler
// Type: PrivacyPolicyViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PrivacyPolicyViewController : ViewController
{
  [Header("Buttons")]
  [SerializeField]
  protected Button _okButton;
  [SerializeField]
  protected Button _iAcceptButton;
  [Header("Texts")]
  [SerializeField]
  protected TextPageScrollView _textPageScrollView;
  [SerializeField]
  protected LocalizedTextAsset _privacyPolicyLocalizedTextAsset;
  [Header("Update Notice")]
  [SerializeField]
  [LocalizationKey]
  protected string _defaultPrivacyPolicyHeaderLocalizationKey;
  [SerializeField]
  [LocalizationKey]
  protected string _updateNoticeLocalizationKey;
  protected bool _showUpdate;
  protected bool _showIAcceptPrompt;

  public event System.Action<PrivacyPolicyViewController.ButtonType> didFinishEvent;

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (!firstActivation)
      return;
    this._okButton.onClick.AddListener((UnityAction) (() =>
    {
      System.Action<PrivacyPolicyViewController.ButtonType> didFinishEvent = this.didFinishEvent;
      if (didFinishEvent == null)
        return;
      didFinishEvent(PrivacyPolicyViewController.ButtonType.Ok);
    }));
    this._iAcceptButton.onClick.AddListener((UnityAction) (() =>
    {
      System.Action<PrivacyPolicyViewController.ButtonType> didFinishEvent = this.didFinishEvent;
      if (didFinishEvent == null)
        return;
      didFinishEvent(PrivacyPolicyViewController.ButtonType.Ok);
    }));
    this._okButton.gameObject.SetActive(!this._showIAcceptPrompt);
    this._iAcceptButton.gameObject.SetActive(this._showIAcceptPrompt);
    this._textPageScrollView.SetText((this._showUpdate ? Localization.Get(this._updateNoticeLocalizationKey) + "\n\n" : Localization.Get(this._defaultPrivacyPolicyHeaderLocalizationKey) + "\n\n") + this._privacyPolicyLocalizedTextAsset.localizedText);
  }

  public virtual void Init(bool showUpdate, bool showIAcceptPrompt)
  {
    this._showUpdate = showUpdate;
    this._showIAcceptPrompt = showIAcceptPrompt;
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__12_0()
  {
    System.Action<PrivacyPolicyViewController.ButtonType> didFinishEvent = this.didFinishEvent;
    if (didFinishEvent == null)
      return;
    didFinishEvent(PrivacyPolicyViewController.ButtonType.Ok);
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__12_1()
  {
    System.Action<PrivacyPolicyViewController.ButtonType> didFinishEvent = this.didFinishEvent;
    if (didFinishEvent == null)
      return;
    didFinishEvent(PrivacyPolicyViewController.ButtonType.Ok);
  }

  public enum ButtonType
  {
    Ok,
  }
}
