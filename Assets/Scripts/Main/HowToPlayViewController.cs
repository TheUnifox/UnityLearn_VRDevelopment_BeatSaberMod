// Decompiled with JetBrains decompiler
// Type: HowToPlayViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayViewController : ViewController
{
  [SerializeField]
  protected Button _tutorialButton;
  [SerializeField]
  protected Button _creditsButton;

  public event System.Action<HowToPlayViewController.HowToPlayOptions> didFinishEvent;

  public virtual void Setup(bool showTutorialButton) => this._tutorialButton.gameObject.SetActive(showTutorialButton);

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (!firstActivation)
      return;
    this.buttonBinder.AddBinding(this._tutorialButton, (System.Action) (() =>
    {
      System.Action<HowToPlayViewController.HowToPlayOptions> didFinishEvent = this.didFinishEvent;
      if (didFinishEvent == null)
        return;
      didFinishEvent(HowToPlayViewController.HowToPlayOptions.HowToPlay);
    }));
    this.buttonBinder.AddBinding(this._creditsButton, (System.Action) (() =>
    {
      System.Action<HowToPlayViewController.HowToPlayOptions> didFinishEvent = this.didFinishEvent;
      if (didFinishEvent == null)
        return;
      didFinishEvent(HowToPlayViewController.HowToPlayOptions.Credits);
    }));
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__7_0()
  {
    System.Action<HowToPlayViewController.HowToPlayOptions> didFinishEvent = this.didFinishEvent;
    if (didFinishEvent == null)
      return;
    didFinishEvent(HowToPlayViewController.HowToPlayOptions.HowToPlay);
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__7_1()
  {
    System.Action<HowToPlayViewController.HowToPlayOptions> didFinishEvent = this.didFinishEvent;
    if (didFinishEvent == null)
      return;
    didFinishEvent(HowToPlayViewController.HowToPlayOptions.Credits);
  }

  public enum HowToPlayOptions
  {
    HowToPlay,
    Credits,
  }
}
