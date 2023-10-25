// Decompiled with JetBrains decompiler
// Type: HelpFlowCoordinator
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using System.Collections.Generic;
using Zenject;

public class HelpFlowCoordinator : FlowCoordinator
{
  [Inject]
  protected readonly HelpMenuViewController _helpMenuViewController;
  [Inject]
  protected readonly HelpNavigationController _helpNavigationController;
  [Inject]
  protected readonly HowToPlayViewController _howToPlayViewController;
  [Inject]
  protected readonly HealthWarningDisplayViewController _healthWarningDisplayViewController;
  [Inject]
  protected readonly PrivacyPolicyDisplayViewController _privacyPolicyDisplayViewController;
  [Inject]
  protected readonly EulaDisplayViewController _eulaDisplayViewController;
  [Inject]
  protected readonly PlayerStatisticsViewController _playerStatisticsViewController;
  [Inject]
  protected readonly PlayerDataModel _playerDataModel;
  [Inject]
  protected readonly MenuTransitionsHelper _menuTransitionsHelper;
  [LocalizationKey]
  protected const string kHowToPlayMenu = "LABEL_HOW_TO_PLAY";
  [LocalizationKey]
  protected const string kPrivacyPolicyMenu = "PRIVACY_POLICY_MENU";
  [LocalizationKey]
  protected const string kEulaMenu = "EULA_MENU";
  [LocalizationKey]
  protected const string kHealthWarningMenu = "HEALTH_AND_SAFETY_MENU";
  protected List<(ViewController viewController, string localizationString)> _viewControllers;

  public event System.Action<HelpFlowCoordinator> didFinishEvent;

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    this.SetTitle(Localization.Get("HELP_TITLE"));
    this.showBackButton = true;
    this._howToPlayViewController.didFinishEvent += new System.Action<HowToPlayViewController.HowToPlayOptions>(this.HandleHowToPlayViewControllerDidFinish);
    if (!addedToHierarchy)
      return;
    this._viewControllers = new List<(ViewController, string)>()
    {
      ((ViewController) this._howToPlayViewController, "LABEL_HOW_TO_PLAY"),
      ((ViewController) this._privacyPolicyDisplayViewController, "PRIVACY_POLICY_MENU"),
      ((ViewController) this._eulaDisplayViewController, "EULA_MENU"),
      ((ViewController) this._healthWarningDisplayViewController, "HEALTH_AND_SAFETY_MENU")
    };
    this._helpMenuViewController.Init(this._viewControllers);
    this._helpMenuViewController.didSelectHelpSubMenuEvent += new System.Action<int>(this.HandleDidSelectHelpSubMenu);
    this.SetViewControllersToNavigationController((NavigationController) this._helpNavigationController, (ViewController) this._helpMenuViewController, (ViewController) this._howToPlayViewController);
    this.ProvideInitialViewControllers((ViewController) this._helpNavigationController, rightScreenViewController: (ViewController) this._playerStatisticsViewController);
  }

  protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
  {
    this._howToPlayViewController.didFinishEvent -= new System.Action<HowToPlayViewController.HowToPlayOptions>(this.HandleHowToPlayViewControllerDidFinish);
    if (!removedFromHierarchy)
      return;
    this._helpMenuViewController.didSelectHelpSubMenuEvent -= new System.Action<int>(this.HandleDidSelectHelpSubMenu);
  }

  public virtual void HandleDidSelectHelpSubMenu(int idx) => this.ReplaceViewController(this._viewControllers[idx].viewController);

  public virtual void ReplaceViewController(ViewController viewController)
  {
    bool immediately = false;
    if (this._helpNavigationController.viewControllers.Count > 1)
    {
      immediately = true;
      this.PopViewControllerFromNavigationController((NavigationController) this._helpNavigationController, immediately: true);
    }
    this.SetLeftScreenViewController((ViewController) null, ViewController.AnimationType.None);
    if ((UnityEngine.Object) viewController == (UnityEngine.Object) this._howToPlayViewController)
      this.SetRightScreenViewController((ViewController) this._playerStatisticsViewController, ViewController.AnimationType.None);
    else
      this.SetRightScreenViewController((ViewController) null, ViewController.AnimationType.None);
    this.PushViewControllerToNavigationController((NavigationController) this._helpNavigationController, viewController, immediately: immediately);
  }

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

  protected override void BackButtonWasPressed(ViewController topViewController)
  {
    System.Action<HelpFlowCoordinator> didFinishEvent = this.didFinishEvent;
    if (didFinishEvent == null)
      return;
    didFinishEvent(this);
  }
}
