// Decompiled with JetBrains decompiler
// Type: MainMenuViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainMenuViewController : ViewController
{
  [SerializeField]
  protected Button _soloButton;
  [SerializeField]
  protected Button _partyButton;
  [SerializeField]
  protected Button _campaignButton;
  [SerializeField]
  protected Button _quitButton;
  [SerializeField]
  protected Button _howToPlayButton;
  [SerializeField]
  protected Button _beatmapEditorButton;
  [SerializeField]
  protected Button _multiplayerButton;
  [SerializeField]
  protected Button _optionsButton;
  [SerializeField]
  protected Button _musicPackPromoButton;
  [SerializeField]
  protected MusicPackPromoBanner _musicPackPromoBanner;
  [Inject]
  protected readonly DlcPromoPanelModel _dlcPromoPanelModel;
  [Inject]
  protected readonly AppStaticSettingsSO _appStaticSettings;
  [Inject]
  protected readonly IAnalyticsModel _analyticsModel;

  public event System.Action<MainMenuViewController, MainMenuViewController.MenuButton> didFinishEvent;

  public event System.Action<IBeatmapLevelPack, IPreviewBeatmapLevel> musicPackPromoButtonWasPressedEvent;

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (firstActivation)
    {
      this._analyticsModel.LogImpression("Main Menu", new Dictionary<string, string>()
      {
        ["language"] = Localization.Instance.SelectedLanguage.ToSerializedName()
      });
      if (this._appStaticSettings.disableMultiplayer)
        this._multiplayerButton.gameObject.SetActive(false);
      this.buttonBinder.AddBinding(this._soloButton, (System.Action) (() => this.HandleMenuButton(MainMenuViewController.MenuButton.SoloFreePlay)));
      this.buttonBinder.AddBinding(this._partyButton, (System.Action) (() => this.HandleMenuButton(MainMenuViewController.MenuButton.Party)));
      this.buttonBinder.AddBinding(this._campaignButton, (System.Action) (() => this.HandleMenuButton(MainMenuViewController.MenuButton.SoloCampaign)));
      this.buttonBinder.AddBinding(this._beatmapEditorButton, (System.Action) (() => this.HandleMenuButton(MainMenuViewController.MenuButton.BeatmapEditor)));
      this.buttonBinder.AddBinding(this._quitButton, (System.Action) (() => this.HandleMenuButton(MainMenuViewController.MenuButton.Quit)));
      this.buttonBinder.AddBinding(this._multiplayerButton, (System.Action) (() => this.HandleMenuButton(MainMenuViewController.MenuButton.Multiplayer)));
      this.buttonBinder.AddBinding(this._optionsButton, (System.Action) (() => this.HandleMenuButton(MainMenuViewController.MenuButton.Options)));
      this.buttonBinder.AddBinding(this._howToPlayButton, (System.Action) (() => this.HandleMenuButton(MainMenuViewController.MenuButton.HowToPlay)));
      this.buttonBinder.AddBinding(this._musicPackPromoButton, new System.Action(this.PackPromoButtonWasPressed));
    }
    bool owned;
    DlcPromoPanelDataSO.MusicPackPromoInfo mainMenuPromoBanner = this._dlcPromoPanelModel.GetPackDataForMainMenuPromoBanner(out owned);
    this._musicPackPromoBanner.Setup(mainMenuPromoBanner, owned);
    this._dlcPromoPanelModel.MainMenuDlcPromoBannerWasShown(mainMenuPromoBanner.previewBeatmapLevelPack, this._musicPackPromoBanner.promoButtonText);
  }

  public virtual void PackPromoButtonWasPressed()
  {
    this._dlcPromoPanelModel.MainMenuDlcPromoBannerWasPressed(this._musicPackPromoBanner.currentPromoMusicPack, this._musicPackPromoBanner.promoButtonText);
    System.Action<IBeatmapLevelPack, IPreviewBeatmapLevel> buttonWasPressedEvent = this.musicPackPromoButtonWasPressedEvent;
    if (buttonWasPressedEvent == null)
      return;
    buttonWasPressedEvent(this._musicPackPromoBanner.currentPromoMusicPack, this._musicPackPromoBanner.currentPromoBeatmapLevel);
  }

  public virtual void HandleMenuButton(MainMenuViewController.MenuButton menuButton)
  {
    this._analyticsModel.LogClick("Main Menu Button", new Dictionary<string, string>()
    {
      ["menu_button"] = menuButton.ToString()
    });
    System.Action<MainMenuViewController, MainMenuViewController.MenuButton> didFinishEvent = this.didFinishEvent;
    if (didFinishEvent == null)
      return;
    didFinishEvent(this, menuButton);
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__20_0() => this.HandleMenuButton(MainMenuViewController.MenuButton.SoloFreePlay);

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__20_1() => this.HandleMenuButton(MainMenuViewController.MenuButton.Party);

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__20_2() => this.HandleMenuButton(MainMenuViewController.MenuButton.SoloCampaign);

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__20_3() => this.HandleMenuButton(MainMenuViewController.MenuButton.BeatmapEditor);

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__20_4() => this.HandleMenuButton(MainMenuViewController.MenuButton.Quit);

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__20_5() => this.HandleMenuButton(MainMenuViewController.MenuButton.Multiplayer);

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__20_6() => this.HandleMenuButton(MainMenuViewController.MenuButton.Options);

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__20_7() => this.HandleMenuButton(MainMenuViewController.MenuButton.HowToPlay);

  public enum MenuButton
  {
    SoloFreePlay,
    Party,
    BeatmapEditor,
    SoloCampaign,
    FloorAdjust,
    Quit,
    Multiplayer,
    Options,
    HowToPlay,
  }
}
