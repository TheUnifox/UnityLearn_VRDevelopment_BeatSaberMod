// Decompiled with JetBrains decompiler
// Type: SelectLanguageViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class SelectLanguageViewController : ViewController
{
  [SerializeField]
  protected Button _continueButton;
  [SerializeField]
  protected LanguageSettingsController _languageSettingsController;

  public event System.Action didChangeLanguageEvent;

  public event System.Action didPressContinueButtonEvent;

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (!firstActivation)
      return;
    this._languageSettingsController.dropDownValueDidChangeEvent += new System.Action(this.HandleLanguageSettingsControllerOndropDownValueDidChange);
    this.buttonBinder.AddBinding(this._continueButton, (System.Action) (() =>
    {
      System.Action continueButtonEvent = this.didPressContinueButtonEvent;
      if (continueButtonEvent == null)
        return;
      continueButtonEvent();
    }));
  }

  protected override void OnDestroy()
  {
    base.OnDestroy();
    if (!((UnityEngine.Object) this._languageSettingsController != (UnityEngine.Object) null))
      return;
    this._languageSettingsController.dropDownValueDidChangeEvent -= new System.Action(this.HandleLanguageSettingsControllerOndropDownValueDidChange);
  }

  public virtual void HandleLanguageSettingsControllerOndropDownValueDidChange()
  {
    System.Action changeLanguageEvent = this.didChangeLanguageEvent;
    if (changeLanguageEvent == null)
      return;
    changeLanguageEvent();
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__8_0()
  {
    System.Action continueButtonEvent = this.didPressContinueButtonEvent;
    if (continueButtonEvent == null)
      return;
    continueButtonEvent();
  }
}
