// Decompiled with JetBrains decompiler
// Type: MainFlowCoordinator
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

public class MainFlowCoordinator : FlowCoordinator
{
  [SerializeField]
  protected MenuLightsPresetSO _defaultLightsPreset;
  [Inject]
  protected readonly SoloFreePlayFlowCoordinator _soloFreePlayFlowCoordinator;
  [Inject]
  protected readonly PartyFreePlayFlowCoordinator _partyFreePlayFlowCoordinator;
  [Inject]
  protected readonly CampaignFlowCoordinator _campaignFlowCoordinator;
  [Inject]
  protected readonly SettingsFlowCoordinator _settingsFlowCoordinator;
  [Inject]
  protected readonly MultiplayerModeSelectionFlowCoordinator _multiplayerModeSelectionFlowCoordinator;
  [Inject]
  protected readonly EditAvatarFlowCoordinator _editAvatarFlowCoordinator;
  [Inject]
  protected readonly HelpFlowCoordinator _helpFlowCoordinator;
  [Inject]
  protected readonly SimpleDialogPromptViewController _simpleDialogPromptViewController;
  [Inject]
  protected readonly MainMenuViewController _mainMenuViewController;
  [Inject]
  protected readonly PlayerOptionsViewController _playerOptionsViewController;
  [Inject]
  protected readonly OptionsViewController _optionsViewController;
  [Inject]
  protected readonly PlayerDataModel _playerDataModel;
  [Inject]
  protected readonly MenuLightsManager _menuLightsManager;
  [Inject]
  protected readonly FadeInOutController _fadeInOut;
  [Inject]
  protected readonly BeatmapLevelsModel _beatmapLevelsModel;
  [Inject]
  protected readonly MenuTransitionsHelper _menuTransitionsHelper;
  [Inject]
  protected readonly AppStaticSettingsSO _appStaticSettings;
  [Inject]
  protected readonly IAnalyticsModel _analyticsModel;
  [InjectOptional]
  protected MenuDestination _menuDestinationRequest;
  protected FlowCoordinator _afterDialogPromptFlowCoordinator;
  protected static bool _startWithSettings;

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    this._mainMenuViewController.didFinishEvent += new System.Action<MainMenuViewController, MainMenuViewController.MenuButton>(this.HandleMainMenuViewControllerDidFinish);
    this._mainMenuViewController.musicPackPromoButtonWasPressedEvent += new System.Action<IBeatmapLevelPack, IPreviewBeatmapLevel>(this.HandleMainMenuViewControllerMusicPackPromoButtonWasPressed);
    this._playerOptionsViewController.didFinishEvent += new System.Action<ViewController>(this.HandlePlayerOptionsViewControllerDidFinish);
    this._optionsViewController.didFinishEvent += new System.Action<OptionsViewController.OptionsButton>(this.HandleOptionsViewControllerDidFinish);
    if (addedToHierarchy)
    {
      this._soloFreePlayFlowCoordinator.didFinishEvent += new System.Action<SinglePlayerLevelSelectionFlowCoordinator>(this.HandleSoloFreePlayFlowCoordinatorDidFinish);
      this._partyFreePlayFlowCoordinator.didFinishEvent += new System.Action<SinglePlayerLevelSelectionFlowCoordinator>(this.HandlePartyFreePlayFlowCoordinatorDidFinish);
      this._settingsFlowCoordinator.didFinishEvent += new System.Action<SettingsFlowCoordinator, SettingsFlowCoordinator.FinishAction>(this.HandleSettingsFlowCoordinatorDidFinish);
      this._campaignFlowCoordinator.didFinishEvent += new System.Action<CampaignFlowCoordinator>(this.HandleCampaignFlowCoordinatorDidFinish);
      this._editAvatarFlowCoordinator.didFinishEvent += new System.Action<EditAvatarFlowCoordinator, EditAvatarFlowCoordinator.EditAvatarType>(this.HandleEditAvatarFlowCoordinatorDidFinish);
      this._multiplayerModeSelectionFlowCoordinator.didFinishEvent += new System.Action<MultiplayerModeSelectionFlowCoordinator>(this.HandleMultiplayerModeSelectionFlowCoordinatorDidFinish);
      this._helpFlowCoordinator.didFinishEvent += new System.Action<HelpFlowCoordinator>(this.HandleHelpFlowCoordinatorDidFinish);
    }
    if (addedToHierarchy)
      this.ProvideInitialViewControllers((ViewController) this._mainMenuViewController);
    this._menuLightsManager.SetColorPreset(this._defaultLightsPreset, true);
    if (!firstActivation || this._menuDestinationRequest == null)
      return;
    this.StartCoroutine(this.ProcessMenuDestinationRequestAfterFrameCoroutine(this._menuDestinationRequest));
    this._menuDestinationRequest = (MenuDestination) null;
  }

  protected override void TopViewControllerWillChange(
    ViewController oldViewController,
    ViewController newViewController,
    ViewController.AnimationType animationType)
  {
    if ((UnityEngine.Object) newViewController == (UnityEngine.Object) this._mainMenuViewController)
    {
      this.SetLeftScreenViewController((ViewController) null, animationType);
      this.SetRightScreenViewController((ViewController) null, animationType);
      this.SetBottomScreenViewController((ViewController) null, animationType);
    }
    else
    {
      this.SetLeftScreenViewController((ViewController) null, animationType);
      this.SetRightScreenViewController((ViewController) null, animationType);
      this.SetBottomScreenViewController((ViewController) null, animationType);
    }
    if ((UnityEngine.Object) newViewController == (UnityEngine.Object) this._playerOptionsViewController)
    {
      this.SetTitle(Localization.Get("BUTTON_PLAYER_OPTIONS"), animationType);
      this.showBackButton = false;
    }
    else if ((UnityEngine.Object) newViewController == (UnityEngine.Object) this._optionsViewController)
    {
      this.SetTitle(Localization.Get("LABEL_OPTIONS"), animationType);
      this.showBackButton = true;
    }
    else
    {
      this.SetTitle("", animationType);
      this.showBackButton = false;
    }
  }

  protected override void InitialViewControllerWasPresented()
  {
    if (!MainFlowCoordinator._startWithSettings)
      return;
    this.PresentFlowCoordinator((FlowCoordinator) this._settingsFlowCoordinator, immediately: true);
  }

  protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
  {
    this._mainMenuViewController.didFinishEvent -= new System.Action<MainMenuViewController, MainMenuViewController.MenuButton>(this.HandleMainMenuViewControllerDidFinish);
    this._mainMenuViewController.musicPackPromoButtonWasPressedEvent -= new System.Action<IBeatmapLevelPack, IPreviewBeatmapLevel>(this.HandleMainMenuViewControllerMusicPackPromoButtonWasPressed);
    this._playerOptionsViewController.didFinishEvent -= new System.Action<ViewController>(this.HandlePlayerOptionsViewControllerDidFinish);
    this._optionsViewController.didFinishEvent -= new System.Action<OptionsViewController.OptionsButton>(this.HandleOptionsViewControllerDidFinish);
    if (!removedFromHierarchy)
      return;
    this._settingsFlowCoordinator.didFinishEvent -= new System.Action<SettingsFlowCoordinator, SettingsFlowCoordinator.FinishAction>(this.HandleSettingsFlowCoordinatorDidFinish);
    this._soloFreePlayFlowCoordinator.didFinishEvent -= new System.Action<SinglePlayerLevelSelectionFlowCoordinator>(this.HandleSoloFreePlayFlowCoordinatorDidFinish);
    this._partyFreePlayFlowCoordinator.didFinishEvent -= new System.Action<SinglePlayerLevelSelectionFlowCoordinator>(this.HandlePartyFreePlayFlowCoordinatorDidFinish);
    this._campaignFlowCoordinator.didFinishEvent -= new System.Action<CampaignFlowCoordinator>(this.HandleCampaignFlowCoordinatorDidFinish);
    this._editAvatarFlowCoordinator.didFinishEvent -= new System.Action<EditAvatarFlowCoordinator, EditAvatarFlowCoordinator.EditAvatarType>(this.HandleEditAvatarFlowCoordinatorDidFinish);
    this._multiplayerModeSelectionFlowCoordinator.didFinishEvent -= new System.Action<MultiplayerModeSelectionFlowCoordinator>(this.HandleMultiplayerModeSelectionFlowCoordinatorDidFinish);
    this._helpFlowCoordinator.didFinishEvent -= new System.Action<HelpFlowCoordinator>(this.HandleHelpFlowCoordinatorDidFinish);
  }

  public virtual void PresentFlowCoordinatorOrAskForTutorial(FlowCoordinator flowCoordinator)
  {
    this._afterDialogPromptFlowCoordinator = flowCoordinator;
    if (this._playerDataModel.playerData.shouldShowTutorialPrompt)
    {
      this._playerDataModel.playerData.MarkTutorialAsShown();
      this._playerDataModel.Save();
      this._simpleDialogPromptViewController.Init(Localization.Get("PROMPT_INFORMATION"), Localization.Get("PROMPT_HAVENT_PLAYED_YET"), Localization.Get("PROMPT_YES"), Localization.Get("PROMPT_NO"), (System.Action<int>) (buttonNumber =>
      {
        if (buttonNumber == 0)
          this._menuTransitionsHelper.StartTutorial(this._playerDataModel.playerData.playerSpecificSettings, (System.Action) (() => this.DismissViewController((ViewController) this._simpleDialogPromptViewController, immediately: true)));
        else
          this.PresentFlowCoordinator(this._afterDialogPromptFlowCoordinator, replaceTopViewController: true);
      }));
      this.PresentViewController((ViewController) this._simpleDialogPromptViewController);
    }
    else
      this.PresentFlowCoordinator(flowCoordinator);
  }

  public virtual void HandleMainMenuViewControllerDidFinish(
    MainMenuViewController viewController,
    MainMenuViewController.MenuButton subMenuType)
  {
    switch (subMenuType)
    {
      case MainMenuViewController.MenuButton.SoloFreePlay:
        this.PresentFlowCoordinatorOrAskForTutorial((FlowCoordinator) this._soloFreePlayFlowCoordinator);
        break;
      case MainMenuViewController.MenuButton.Party:
        this.PresentFlowCoordinatorOrAskForTutorial((FlowCoordinator) this._partyFreePlayFlowCoordinator);
        break;
      case MainMenuViewController.MenuButton.BeatmapEditor:
        this._menuTransitionsHelper.StartBeatmapEditor((System.Action) (() => this._beatmapLevelsModel.ClearLoadedBeatmapLevelsCaches()));
        break;
      case MainMenuViewController.MenuButton.SoloCampaign:
        this.PresentFlowCoordinatorOrAskForTutorial((FlowCoordinator) this._campaignFlowCoordinator);
        break;
      case MainMenuViewController.MenuButton.Quit:
        this._fadeInOut.FadeOutInstant();
        Application.Quit();
        break;
      case MainMenuViewController.MenuButton.Multiplayer:
        if (!this._playerDataModel.playerData.agreedToMultiplayerDisclaimer)
        {
          this._simpleDialogPromptViewController.Init(Localization.Get("LABEL_MULTIPLAYER_DISCLAIMER"), Localization.Get("MULTIPLAYER_DISCLAIMER"), Localization.Get("BUTTON_AGREE"), Localization.Get("BUTTON_DO_NOT_AGREE_AND_QUIT"), (System.Action<int>) (buttonNumber =>
          {
            bool flag = buttonNumber == 0;
            this._analyticsModel.LogClick("Multiplayer Disclaimer", new Dictionary<string, string>()
            {
              ["page"] = "Multiplayer",
              ["custom_text"] = flag ? "BUTTON_AGREE" : "BUTTON_DO_NOT_AGREE_AND_QUIT"
            });
            if (!flag)
            {
              this.DismissViewController((ViewController) this._simpleDialogPromptViewController, ViewController.AnimationDirection.Vertical);
            }
            else
            {
              this._playerDataModel.playerData.MarkMultiplayerDisclaimerAsAgreed();
              if (!this._playerDataModel.playerData.avatarCreated)
              {
                this._editAvatarFlowCoordinator.Setup(EditAvatarFlowCoordinator.EditAvatarType.Create);
                this.PresentFlowCoordinator((FlowCoordinator) this._editAvatarFlowCoordinator, animationDirection: ViewController.AnimationDirection.Vertical, replaceTopViewController: true);
                this._analyticsModel.LogImpression("Create Avatar", new Dictionary<string, string>()
                {
                  ["page"] = "Avatar"
                });
              }
              else
                this.PresentFlowCoordinator((FlowCoordinator) this._multiplayerModeSelectionFlowCoordinator, replaceTopViewController: true);
            }
          }));
          this.PresentViewController((ViewController) this._simpleDialogPromptViewController);
          this._analyticsModel.LogImpression("Multiplayer Disclaimer", new Dictionary<string, string>()
          {
            ["page"] = "Multiplayer"
          });
          break;
        }
        if (!this._playerDataModel.playerData.avatarCreated)
        {
          this._editAvatarFlowCoordinator.Setup(EditAvatarFlowCoordinator.EditAvatarType.Create);
          this.PresentFlowCoordinator((FlowCoordinator) this._editAvatarFlowCoordinator, animationDirection: ViewController.AnimationDirection.Vertical);
          this._analyticsModel.LogImpression("Create Avatar", new Dictionary<string, string>()
          {
            ["page"] = "Avatar"
          });
          break;
        }
        this.PresentFlowCoordinator((FlowCoordinator) this._multiplayerModeSelectionFlowCoordinator, animationDirection: ViewController.AnimationDirection.Vertical);
        break;
      case MainMenuViewController.MenuButton.Options:
        this.PresentViewController((ViewController) this._optionsViewController);
        break;
      case MainMenuViewController.MenuButton.HowToPlay:
        this.PresentFlowCoordinator((FlowCoordinator) this._helpFlowCoordinator);
        break;
    }
  }

  public virtual void HandleOptionsViewControllerDidFinish(
    OptionsViewController.OptionsButton optionsType)
  {
    switch (optionsType)
    {
      case OptionsViewController.OptionsButton.EditAvatar:
        this._editAvatarFlowCoordinator.Setup(EditAvatarFlowCoordinator.EditAvatarType.Edit);
        this.PresentFlowCoordinator((FlowCoordinator) this._editAvatarFlowCoordinator, animationDirection: ViewController.AnimationDirection.Vertical);
        break;
      case OptionsViewController.OptionsButton.PlayerOptions:
        this.PresentViewController((ViewController) this._playerOptionsViewController, animationDirection: ViewController.AnimationDirection.Vertical);
        break;
      case OptionsViewController.OptionsButton.Settings:
        MainFlowCoordinator._startWithSettings = true;
        this.PresentFlowCoordinator((FlowCoordinator) this._settingsFlowCoordinator, animationDirection: ViewController.AnimationDirection.Vertical);
        break;
    }
  }

  public virtual void HandlePlayerOptionsViewControllerDidFinish(ViewController viewController) => this.DismissViewController(viewController, ViewController.AnimationDirection.Vertical);

  public virtual void HandleHowToPlayViewControllerDidFinish(
    HowToPlayViewController.HowToPlayOptions howToPlayOptions)
  {
    if (howToPlayOptions != HowToPlayViewController.HowToPlayOptions.HowToPlay)
    {
      if (howToPlayOptions != HowToPlayViewController.HowToPlayOptions.Credits)
        return;
      this._menuTransitionsHelper.ShowCredits();
    }
    else
    {
      this._playerDataModel.playerData.MarkTutorialAsShown();
      this._menuTransitionsHelper.StartTutorial(this._playerDataModel.playerData.playerSpecificSettings);
    }
  }

  public virtual void HandleCampaignFlowCoordinatorDidFinish(CampaignFlowCoordinator flowCoordinator) => this.DismissFlowCoordinator((FlowCoordinator) flowCoordinator);

  public virtual void HandleEditAvatarFlowCoordinatorDidFinish(
    EditAvatarFlowCoordinator flowCoordinator,
    EditAvatarFlowCoordinator.EditAvatarType editAvatarType)
  {
    if (editAvatarType == EditAvatarFlowCoordinator.EditAvatarType.Create && this._playerDataModel.playerData.avatarCreated)
    {
      this.ReplaceChildFlowCoordinator((FlowCoordinator) this._multiplayerModeSelectionFlowCoordinator, animationDirection: ViewController.AnimationDirection.Vertical);
      this._analyticsModel.LogClick("Create Avatar", new Dictionary<string, string>()
      {
        ["page"] = "Avatar",
        ["custom_text"] = this._playerDataModel.playerData.avatarCreated ? "Create Avatar" : "Cancel"
      });
    }
    else
      this.DismissFlowCoordinator((FlowCoordinator) flowCoordinator, ViewController.AnimationDirection.Vertical);
  }

  public virtual void HandleSoloFreePlayFlowCoordinatorDidFinish(
    LevelSelectionFlowCoordinator flowCoordinator)
  {
    this.DismissFlowCoordinator((FlowCoordinator) flowCoordinator);
  }

  public virtual void HandlePartyFreePlayFlowCoordinatorDidFinish(
    LevelSelectionFlowCoordinator flowCoordinator)
  {
    this.DismissFlowCoordinator((FlowCoordinator) flowCoordinator);
  }

  public virtual void HandleSettingsFlowCoordinatorDidFinish(
    SettingsFlowCoordinator settingsFlowCoordinator,
    SettingsFlowCoordinator.FinishAction finishAction)
  {
    if (finishAction != SettingsFlowCoordinator.FinishAction.Apply)
      MainFlowCoordinator._startWithSettings = false;
    if (finishAction == SettingsFlowCoordinator.FinishAction.Cancel)
      this.DismissFlowCoordinator((FlowCoordinator) this._settingsFlowCoordinator, ViewController.AnimationDirection.Vertical);
    else
      this._menuTransitionsHelper.RestartGame();
  }

  public virtual void HandleHelpFlowCoordinatorDidFinish(HelpFlowCoordinator helpFlowCoordinator) => this.DismissFlowCoordinator((FlowCoordinator) helpFlowCoordinator);

  public virtual void HandleMultiplayerModeSelectionFlowCoordinatorDidFinish(
    MultiplayerModeSelectionFlowCoordinator multiplayerModeSelectionFlowCoordinator)
  {
    this.DismissFlowCoordinator((FlowCoordinator) multiplayerModeSelectionFlowCoordinator);
  }

  public virtual void HandleMainMenuViewControllerMusicPackPromoButtonWasPressed(
    IBeatmapLevelPack beatmapLevelPack,
    IPreviewBeatmapLevel previewBeatmapLevel)
  {
    this._playerDataModel.playerData.overrideEnvironmentSettings.overrideEnvironments = false;
    this._playerDataModel.playerData.colorSchemesSettings.overrideDefaultColors = false;
    this._soloFreePlayFlowCoordinator.Setup(new LevelSelectionFlowCoordinator.State(beatmapLevelPack, previewBeatmapLevel));
    this.PresentFlowCoordinatorOrAskForTutorial((FlowCoordinator) this._soloFreePlayFlowCoordinator);
  }

  public virtual void ProcessMenuDestinationRequest(MenuDestination destination)
  {
    switch (destination)
    {
      case SelectLevelPackDestination levelPackDestination:
        this._soloFreePlayFlowCoordinator.Setup(new LevelSelectionFlowCoordinator.State(levelPackDestination.beatmapLevelPack));
        this.PresentFlowCoordinator((FlowCoordinator) this._soloFreePlayFlowCoordinator, immediately: true);
        break;
      case SelectLevelDestination levelDestination:
        this._playerDataModel.playerData.SetLastSelectedBeatmapDifficulty(levelDestination.beatmapDifficulty);
        if ((UnityEngine.Object) levelDestination.beatmapCharacteristic != (UnityEngine.Object) null)
          this._playerDataModel.playerData.SetLastSelectedBeatmapCharacteristic(levelDestination.beatmapCharacteristic);
        this._soloFreePlayFlowCoordinator.Setup(new LevelSelectionFlowCoordinator.State(levelDestination.beatmapLevelPack, levelDestination.previewBeatmapLevel));
        this.PresentFlowCoordinator((FlowCoordinator) this._soloFreePlayFlowCoordinator, immediately: true);
        break;
      case RunLevelMenuDestination runLevelMenuDestination:
        this._playerDataModel.playerData.SetLastSelectedBeatmapDifficulty(runLevelMenuDestination.beatmapDifficulty);
        if ((UnityEngine.Object) runLevelMenuDestination.beatmapCharacteristic != (UnityEngine.Object) null)
          this._playerDataModel.playerData.SetLastSelectedBeatmapCharacteristic(runLevelMenuDestination.beatmapCharacteristic);
        this._soloFreePlayFlowCoordinator.Setup(new LevelSelectionFlowCoordinator.State(runLevelMenuDestination.beatmapLevelPack, runLevelMenuDestination.previewBeatmapLevel));
        this.PresentFlowCoordinator((FlowCoordinator) this._soloFreePlayFlowCoordinator, immediately: true);
        this._soloFreePlayFlowCoordinator.__SetupFromDestination(runLevelMenuDestination);
        this._soloFreePlayFlowCoordinator.StartLevel((System.Action) null, runLevelMenuDestination.practice);
        break;
      case SelectMultiplayerLobbyDestination lobbyDestination when !this._appStaticSettings.disableMultiplayer:
        this._multiplayerModeSelectionFlowCoordinator.Setup(lobbyDestination);
        this.PresentMultiplayerModeSelectionFlowCoordinatorWithDisclaimerAndAvatarCreator();
        break;
      case SelectSubMenuDestination subMenuDestination:
        switch (subMenuDestination.menuDestination)
        {
          case SelectSubMenuDestination.Destination.Campaign:
            this.PresentFlowCoordinator((FlowCoordinator) this._campaignFlowCoordinator, immediately: true);
            return;
          case SelectSubMenuDestination.Destination.SoloFreePlay:
            this.PresentFlowCoordinator((FlowCoordinator) this._soloFreePlayFlowCoordinator, immediately: true);
            return;
          case SelectSubMenuDestination.Destination.PartyFreePlay:
            this.PresentFlowCoordinator((FlowCoordinator) this._partyFreePlayFlowCoordinator, immediately: true);
            return;
          case SelectSubMenuDestination.Destination.Settings:
            this.PresentFlowCoordinator((FlowCoordinator) this._settingsFlowCoordinator, immediately: true);
            return;
          case SelectSubMenuDestination.Destination.Tutorial:
            this.PresentFlowCoordinator((FlowCoordinator) this._helpFlowCoordinator, immediately: true);
            return;
          case SelectSubMenuDestination.Destination.Multiplayer:
            if (this._appStaticSettings.disableMultiplayer)
              return;
            this.PresentMultiplayerModeSelectionFlowCoordinatorWithDisclaimerAndAvatarCreator();
            return;
          default:
            return;
        }
    }
  }

  public virtual void PresentMultiplayerModeSelectionFlowCoordinatorWithDisclaimerAndAvatarCreator()
  {
    if (!this._playerDataModel.playerData.agreedToMultiplayerDisclaimer)
    {
      this._simpleDialogPromptViewController.Init(Localization.Get("LABEL_MULTIPLAYER_DISCLAIMER"), Localization.Get("MULTIPLAYER_DISCLAIMER"), Localization.Get("BUTTON_AGREE"), Localization.Get("BUTTON_DO_NOT_AGREE_AND_QUIT"), (System.Action<int>) (buttonNumber =>
      {
        if (buttonNumber != 0)
        {
          this.DismissViewController((ViewController) this._simpleDialogPromptViewController, ViewController.AnimationDirection.Vertical);
        }
        else
        {
          this._playerDataModel.playerData.MarkMultiplayerDisclaimerAsAgreed();
          if (!this._playerDataModel.playerData.avatarCreated)
          {
            this._editAvatarFlowCoordinator.Setup(EditAvatarFlowCoordinator.EditAvatarType.Create);
            this.PresentFlowCoordinator((FlowCoordinator) this._editAvatarFlowCoordinator, animationDirection: ViewController.AnimationDirection.Vertical, replaceTopViewController: true);
          }
          else
            this.PresentFlowCoordinator((FlowCoordinator) this._multiplayerModeSelectionFlowCoordinator, replaceTopViewController: true);
        }
      }));
      this.PresentViewController((ViewController) this._simpleDialogPromptViewController);
    }
    else if (!this._playerDataModel.playerData.avatarCreated)
    {
      this._editAvatarFlowCoordinator.Setup(EditAvatarFlowCoordinator.EditAvatarType.Create);
      this.PresentFlowCoordinator((FlowCoordinator) this._editAvatarFlowCoordinator, immediately: true);
    }
    else
      this.PresentFlowCoordinator((FlowCoordinator) this._multiplayerModeSelectionFlowCoordinator, immediately: true);
  }

  public virtual IEnumerator ProcessMenuDestinationRequestAfterFrameCoroutine(
    MenuDestination destination)
  {
    yield return (object) null;
    this.ProcessMenuDestinationRequest(destination);
  }

  protected override void BackButtonWasPressed(ViewController topViewController)
  {
    if (!((UnityEngine.Object) topViewController == (UnityEngine.Object) this._optionsViewController))
      return;
    this.DismissViewController(topViewController);
  }

  [CompilerGenerated]
  public virtual void m_CPresentFlowCoordinatorOrAskForTutorialm_Eb__26_0(int buttonNumber)
  {
    if (buttonNumber == 0)
      this._menuTransitionsHelper.StartTutorial(this._playerDataModel.playerData.playerSpecificSettings, (System.Action) (() => this.DismissViewController((ViewController) this._simpleDialogPromptViewController, immediately: true)));
    else
      this.PresentFlowCoordinator(this._afterDialogPromptFlowCoordinator, replaceTopViewController: true);
  }

  [CompilerGenerated]
  public virtual void m_CPresentFlowCoordinatorOrAskForTutorialm_Eb__26_1() => this.DismissViewController((ViewController) this._simpleDialogPromptViewController, immediately: true);

  [CompilerGenerated]
  public virtual void m_CHandleMainMenuViewControllerDidFinishm_Eb__27_0() => this._beatmapLevelsModel.ClearLoadedBeatmapLevelsCaches();

  [CompilerGenerated]
  public virtual void m_CHandleMainMenuViewControllerDidFinishm_Eb__27_1(int buttonNumber)
  {
    bool flag = buttonNumber == 0;
    this._analyticsModel.LogClick("Multiplayer Disclaimer", new Dictionary<string, string>()
    {
      ["page"] = "Multiplayer",
      ["custom_text"] = flag ? "BUTTON_AGREE" : "BUTTON_DO_NOT_AGREE_AND_QUIT"
    });
    if (!flag)
    {
      this.DismissViewController((ViewController) this._simpleDialogPromptViewController, ViewController.AnimationDirection.Vertical);
    }
    else
    {
      this._playerDataModel.playerData.MarkMultiplayerDisclaimerAsAgreed();
      if (!this._playerDataModel.playerData.avatarCreated)
      {
        this._editAvatarFlowCoordinator.Setup(EditAvatarFlowCoordinator.EditAvatarType.Create);
        this.PresentFlowCoordinator((FlowCoordinator) this._editAvatarFlowCoordinator, animationDirection: ViewController.AnimationDirection.Vertical, replaceTopViewController: true);
        this._analyticsModel.LogImpression("Create Avatar", new Dictionary<string, string>()
        {
          ["page"] = "Avatar"
        });
      }
      else
        this.PresentFlowCoordinator((FlowCoordinator) this._multiplayerModeSelectionFlowCoordinator, replaceTopViewController: true);
    }
  }

  [CompilerGenerated]
  public virtual void m_CPresentMultiplayerModeSelectionFlowCoordinatorWithDisclaimerAndAvatarCreatorm_Eb__40_0(
    int buttonNumber)
  {
    if (buttonNumber != 0)
    {
      this.DismissViewController((ViewController) this._simpleDialogPromptViewController, ViewController.AnimationDirection.Vertical);
    }
    else
    {
      this._playerDataModel.playerData.MarkMultiplayerDisclaimerAsAgreed();
      if (!this._playerDataModel.playerData.avatarCreated)
      {
        this._editAvatarFlowCoordinator.Setup(EditAvatarFlowCoordinator.EditAvatarType.Create);
        this.PresentFlowCoordinator((FlowCoordinator) this._editAvatarFlowCoordinator, animationDirection: ViewController.AnimationDirection.Vertical, replaceTopViewController: true);
      }
      else
        this.PresentFlowCoordinator((FlowCoordinator) this._multiplayerModeSelectionFlowCoordinator, replaceTopViewController: true);
    }
  }
}
