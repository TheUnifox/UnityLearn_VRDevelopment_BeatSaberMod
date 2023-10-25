// Decompiled with JetBrains decompiler
// Type: OptionsViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class OptionsViewController : ViewController
{
  [SerializeField]
  protected Button _editAvatarButton;
  [SerializeField]
  protected Button _playerOptionsButton;
  [SerializeField]
  protected Button _settingsButton;
  [Inject]
  protected readonly AppStaticSettingsSO _appStaticSettings;

  public event System.Action<OptionsViewController.OptionsButton> didFinishEvent;

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (!firstActivation)
      return;
    if (this._appStaticSettings.disableMultiplayer)
      this._editAvatarButton.gameObject.SetActive(false);
    else
      this.buttonBinder.AddBinding(this._editAvatarButton, (System.Action) (() =>
      {
        System.Action<OptionsViewController.OptionsButton> didFinishEvent = this.didFinishEvent;
        if (didFinishEvent == null)
          return;
        didFinishEvent(OptionsViewController.OptionsButton.EditAvatar);
      }));
    this.buttonBinder.AddBinding(this._playerOptionsButton, (System.Action) (() =>
    {
      System.Action<OptionsViewController.OptionsButton> didFinishEvent = this.didFinishEvent;
      if (didFinishEvent == null)
        return;
      didFinishEvent(OptionsViewController.OptionsButton.PlayerOptions);
    }));
    this.buttonBinder.AddBinding(this._settingsButton, (System.Action) (() =>
    {
      System.Action<OptionsViewController.OptionsButton> didFinishEvent = this.didFinishEvent;
      if (didFinishEvent == null)
        return;
      didFinishEvent(OptionsViewController.OptionsButton.Settings);
    }));
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__8_0()
  {
    System.Action<OptionsViewController.OptionsButton> didFinishEvent = this.didFinishEvent;
    if (didFinishEvent == null)
      return;
    didFinishEvent(OptionsViewController.OptionsButton.EditAvatar);
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__8_1()
  {
    System.Action<OptionsViewController.OptionsButton> didFinishEvent = this.didFinishEvent;
    if (didFinishEvent == null)
      return;
    didFinishEvent(OptionsViewController.OptionsButton.PlayerOptions);
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__8_2()
  {
    System.Action<OptionsViewController.OptionsButton> didFinishEvent = this.didFinishEvent;
    if (didFinishEvent == null)
      return;
    didFinishEvent(OptionsViewController.OptionsButton.Settings);
  }

  public enum OptionsButton
  {
    EditAvatar,
    PlayerOptions,
    Settings,
  }
}
