// Decompiled with JetBrains decompiler
// Type: SettingsFlowCoordinator
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using UnityEngine;
using Zenject;

public class SettingsFlowCoordinator : FlowCoordinator
{
  [SerializeField]
  protected MainSettingsModelSO _mainSettingsModel;
  [Inject]
  protected MainSettingsMenuViewController _mainSettingsMenuViewController;
  [Inject]
  protected SettingsNavigationController _settingsNavigationController;
  [DoesNotRequireDomainReloadInit]
  protected static int _selectedSettingsSubMenuInfoIdx;

  public event System.Action<SettingsFlowCoordinator, SettingsFlowCoordinator.FinishAction> didFinishEvent;

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    this.SetTitle(Localization.Get("TITLE_SETTINGS"));
    if (!addedToHierarchy)
      return;
    this._settingsNavigationController.didFinishEvent += new System.Action<SettingsNavigationController.FinishAction>(this.HandleSettingsNavigationControllerDidFinish);
    this._mainSettingsMenuViewController.didSelectSettingsSubMenuEvent += new System.Action<SettingsSubMenuInfo, int>(this.HandleDidSelectSettingsSubMenu);
    this._mainSettingsMenuViewController.Init(SettingsFlowCoordinator._selectedSettingsSubMenuInfoIdx);
    if (this._mainSettingsMenuViewController.numberOfSubMenus == 1)
      this.SetViewControllersToNavigationController((NavigationController) this._settingsNavigationController, this._mainSettingsMenuViewController.selectedSubMenuInfo.viewController);
    else
      this.SetViewControllersToNavigationController((NavigationController) this._settingsNavigationController, (ViewController) this._mainSettingsMenuViewController, this._mainSettingsMenuViewController.selectedSubMenuInfo.viewController);
    this.ProvideInitialViewControllers((ViewController) this._settingsNavigationController);
  }

  protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
  {
    if (!removedFromHierarchy)
      return;
    this._settingsNavigationController.didFinishEvent -= new System.Action<SettingsNavigationController.FinishAction>(this.HandleSettingsNavigationControllerDidFinish);
    this._mainSettingsMenuViewController.didSelectSettingsSubMenuEvent -= new System.Action<SettingsSubMenuInfo, int>(this.HandleDidSelectSettingsSubMenu);
  }

  public virtual void ShowSecretViewController(ViewController viewController) => this.ReplaceViewController(viewController);

  public virtual void HandleDidSelectSettingsSubMenu(
    SettingsSubMenuInfo settingsSubMenuInfo,
    int idx)
  {
    SettingsFlowCoordinator._selectedSettingsSubMenuInfoIdx = idx;
    this.ReplaceViewController(settingsSubMenuInfo.viewController);
  }

  public virtual void ReplaceViewController(ViewController viewController)
  {
    bool immediately = false;
    if (this._settingsNavigationController.viewControllers.Count > 1)
    {
      immediately = true;
      this.PopViewControllerFromNavigationController((NavigationController) this._settingsNavigationController, immediately: true);
    }
    this.PushViewControllerToNavigationController((NavigationController) this._settingsNavigationController, viewController, immediately: immediately);
  }

  public virtual void HandleSettingsNavigationControllerDidFinish(
    SettingsNavigationController.FinishAction finishAction)
  {
    switch (finishAction)
    {
      case SettingsNavigationController.FinishAction.Ok:
        this.ApplySettings();
        System.Action<SettingsFlowCoordinator, SettingsFlowCoordinator.FinishAction> didFinishEvent1 = this.didFinishEvent;
        if (didFinishEvent1 == null)
          break;
        didFinishEvent1(this, SettingsFlowCoordinator.FinishAction.Ok);
        break;
      case SettingsNavigationController.FinishAction.Cancel:
        this.CancelSettings();
        System.Action<SettingsFlowCoordinator, SettingsFlowCoordinator.FinishAction> didFinishEvent2 = this.didFinishEvent;
        if (didFinishEvent2 == null)
          break;
        didFinishEvent2(this, SettingsFlowCoordinator.FinishAction.Cancel);
        break;
      case SettingsNavigationController.FinishAction.Apply:
        this.ApplySettings();
        System.Action<SettingsFlowCoordinator, SettingsFlowCoordinator.FinishAction> didFinishEvent3 = this.didFinishEvent;
        if (didFinishEvent3 == null)
          break;
        didFinishEvent3(this, SettingsFlowCoordinator.FinishAction.Apply);
        break;
    }
  }

  public virtual void ApplySettings()
  {
    this._mainSettingsModel.Save();
    this._mainSettingsModel.Load(true);
  }

  public virtual void CancelSettings() => this._mainSettingsModel.Load(true);

  public enum FinishAction
  {
    Cancel,
    Ok,
    Apply,
  }
}
