// Decompiled with JetBrains decompiler
// Type: HealthWarningFlowCoordinator
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class HealthWarningFlowCoordinator : FlowCoordinator
{
  [SerializeField]
  protected MainSettingsModelSO _mainSettingsModel;
  [Space]
  [SerializeField]
  protected AppInitScenesTransitionSetupDataContainerSO _appInitScenesTransitionSetupDataContainer;
  [Space]
  [SerializeField]
  protected SelectLanguageViewController _selectLanguageViewController;
  [SerializeField]
  protected SelectRegionViewController _selectRegionViewController;
  [SerializeField]
  protected EulaViewController _eulaViewController;
  [SerializeField]
  protected PrivacyPolicyViewController _privacyPolicyViewController;
  [SerializeField]
  protected HealthWarningViewController _healthWarningViewController;
  [Inject]
  protected readonly PlayerDataModel _playerDataModel;
  [Inject]
  protected readonly DlcPromoPanelModel _dlcPromoPanelModel;
  [Inject]
  protected readonly FadeInOutController _fadeInOut;
  [Inject]
  protected readonly GameScenesManager _gameScenesManager;
  [Inject]
  protected readonly HealthWarningFlowCoordinator.InitData _initData;
  protected Dictionary<ViewController, string> _viewControllerTitles;
  protected SelectRegionViewController.Region _selectedRegion;

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    this.showBackButton = false;
    if (firstActivation)
      this._viewControllerTitles = new Dictionary<ViewController, string>()
      {
        {
          (ViewController) this._selectLanguageViewController,
          Localization.Get("SELECT_LANGUAGE")
        },
        {
          (ViewController) this._selectRegionViewController,
          Localization.Get("SELECT_REGION")
        },
        {
          (ViewController) this._eulaViewController,
          Localization.Get("EULA_TITLE")
        },
        {
          (ViewController) this._privacyPolicyViewController,
          Localization.Get("PRIVACY_POLICY_TITLE")
        },
        {
          (ViewController) this._healthWarningViewController,
          ""
        }
      };
    if (!addedToHierarchy)
      return;
    this._selectLanguageViewController.didChangeLanguageEvent += new System.Action(this.HandleSelectLanguageViewControllerDidChangeLanguage);
    this._selectLanguageViewController.didPressContinueButtonEvent += new System.Action(this.HandleSelectLanguageViewControllerDidPressContinueButton);
    this._selectRegionViewController.didPressContinueButtonEvent += new System.Action<SelectRegionViewController.Region>(this.HandleSelectRegionViewControllerDidPressContinueButton);
    this._eulaViewController.didFinishEvent += new System.Action<EulaViewController.ButtonType>(this.HandleEulaViewControllerDidFinish);
    this._privacyPolicyViewController.didFinishEvent += new System.Action<PrivacyPolicyViewController.ButtonType>(this.HandlePrivacyPolicyViewControllerDidFinish);
    this._healthWarningViewController.didFinishEvent += new System.Action(this.HandleHealthWarningViewControllerDidFinish);
    ViewController viewController = this.ResolveMainViewController();
    this.SetTitle(this._viewControllerTitles[viewController]);
    this.ProvideInitialViewControllers(viewController);
  }

  protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
  {
    if (!removedFromHierarchy)
      return;
    this._selectLanguageViewController.didChangeLanguageEvent -= new System.Action(this.HandleSelectLanguageViewControllerDidChangeLanguage);
    this._selectLanguageViewController.didPressContinueButtonEvent -= new System.Action(this.HandleSelectLanguageViewControllerDidPressContinueButton);
    this._selectRegionViewController.didPressContinueButtonEvent -= new System.Action<SelectRegionViewController.Region>(this.HandleSelectRegionViewControllerDidPressContinueButton);
    this._eulaViewController.didFinishEvent -= new System.Action<EulaViewController.ButtonType>(this.HandleEulaViewControllerDidFinish);
    this._privacyPolicyViewController.didFinishEvent -= new System.Action<PrivacyPolicyViewController.ButtonType>(this.HandlePrivacyPolicyViewControllerDidFinish);
    this._healthWarningViewController.didFinishEvent -= new System.Action(this.HandleHealthWarningViewControllerDidFinish);
  }

  protected override void TopViewControllerWillChange(
    ViewController oldViewController,
    ViewController newViewController,
    ViewController.AnimationType animationType)
  {
    string str;
    this._viewControllerTitles.TryGetValue(newViewController, out str);
    this.SetTitle(str, animationType);
  }

  public virtual void Update()
  {
    if (!Input.GetKeyDown(KeyCode.Return))
      return;
    this.HandleHealthWarningViewControllerDidFinish();
  }

  public virtual void HandleSelectLanguageViewControllerDidChangeLanguage()
  {
    this._mainSettingsModel.Save();
    this._mainSettingsModel.Load(true);
    this._appInitScenesTransitionSetupDataContainer.appInitScenesTransitionSetupData.InitAsAppStart();
    this._gameScenesManager.ClearAndOpenScenes((ScenesTransitionSetupDataSO) this._appInitScenesTransitionSetupDataContainer.appInitScenesTransitionSetupData, 0.35f);
  }

  public virtual void HandleSelectLanguageViewControllerDidPressContinueButton()
  {
    this._playerDataModel.playerData.MarkLanguageAsSelected();
    this.ReplaceTopViewController(this.ResolveMainViewController());
  }

  public virtual void HandleSelectRegionViewControllerDidPressContinueButton(
    SelectRegionViewController.Region region)
  {
    this._selectedRegion = region;
    this.ReplaceTopViewController(this.ResolvePlayerAgreementsViewController());
  }

  public virtual void HandleEulaViewControllerDidFinish(EulaViewController.ButtonType buttonType)
  {
    switch (buttonType)
    {
      case EulaViewController.ButtonType.Agree:
        this._playerDataModel.playerData.playerAgreements.AgreeToEula();
        break;
      case EulaViewController.ButtonType.DoNotAgree:
        this._fadeInOut.FadeOutInstant();
        Application.Quit();
        return;
    }
    this.ReplaceTopViewController(this.ResolvePlayerAgreementsViewController());
  }

  public virtual void HandlePrivacyPolicyViewControllerDidFinish(
    PrivacyPolicyViewController.ButtonType buttonType)
  {
    if (buttonType == PrivacyPolicyViewController.ButtonType.Ok)
      this._playerDataModel.playerData.playerAgreements.AgreeToPrivacyPolicy();
    this.ReplaceTopViewController(this.ResolvePlayerAgreementsViewController());
  }

  public virtual void HandleHealthWarningViewControllerDidFinish()
  {
    if (!this._playerDataModel.playerData.playerAgreements.AgreedToHealthAndSafety())
      this._playerDataModel.playerData.playerAgreements.AgreeToHealthAndSafety();
    if (!this._playerDataModel.playerData.DidSelectRegion())
      this._playerDataModel.playerData.MarkRegionAsSelected();
    this.GoToNextScene();
  }

  public virtual ViewController ResolveMainViewController()
  {
    if (!this._playerDataModel.playerData.didSelectLanguage)
      return (ViewController) this._selectLanguageViewController;
    return !this._playerDataModel.playerData.DidSelectRegion() ? (ViewController) this._selectRegionViewController : this.ResolvePlayerAgreementsViewController();
  }

  public virtual ViewController ResolvePlayerAgreementsViewController()
  {
    bool eula = this._playerDataModel.playerData.playerAgreements.AgreedToEula();
    if (this._selectedRegion != SelectRegionViewController.Region.SouthKorea & this._playerDataModel.playerData.playerAgreements.AgreedToPreviousPrivacyPolicy())
      this._playerDataModel.playerData.playerAgreements.AgreeToPrivacyPolicy();
    bool privacyPolicy = this._playerDataModel.playerData.playerAgreements.AgreedToPrivacyPolicy();
    if (eula & privacyPolicy)
    {
      this._healthWarningViewController.Init(this._playerDataModel.playerData.playerAgreements.AgreedToHealthAndSafety());
      return (ViewController) this._healthWarningViewController;
    }
    if (eula)
    {
      this._privacyPolicyViewController.Init(this._playerDataModel.playerData.playerAgreements.AgreedToAnyPreviousPrivacyPolicy() && this._selectedRegion != SelectRegionViewController.Region.SouthKorea, this._selectedRegion == SelectRegionViewController.Region.SouthKorea);
      return (ViewController) this._privacyPolicyViewController;
    }
    bool anyPreviousEula = this._playerDataModel.playerData.playerAgreements.AgreedToAnyPreviousEula();
    this._eulaViewController.Init(anyPreviousEula, anyPreviousEula);
    return (ViewController) this._eulaViewController;
  }

  public virtual void GoToNextScene()
  {
    this._dlcPromoPanelModel.InitAfterPlatformWasInitialized(false);
    this._gameScenesManager.ReplaceScenes(this._initData.nextScenesTransitionSetupData, minDuration: 0.7f);
  }

  public class InitData
  {
    public readonly ScenesTransitionSetupDataSO nextScenesTransitionSetupData;

    public InitData(
      ScenesTransitionSetupDataSO nextScenesTransitionSetupData)
    {
      this.nextScenesTransitionSetupData = nextScenesTransitionSetupData;
    }
  }
}
