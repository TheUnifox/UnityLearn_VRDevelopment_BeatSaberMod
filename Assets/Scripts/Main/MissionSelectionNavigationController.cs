// Decompiled with JetBrains decompiler
// Type: MissionSelectionNavigationController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Zenject;

public class MissionSelectionNavigationController : NavigationController
{
  [Inject]
  protected MissionSelectionMapViewController _missionSelectionMapViewController;
  [Inject]
  protected MissionLevelDetailViewController _missionLevelDetailViewController;

  public event System.Action<MissionSelectionNavigationController> didPressPlayButtonEvent;

  public MissionNode selectedMissionNode => this._missionLevelDetailViewController.missionNode;

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (!addedToHierarchy)
      return;
    this.SetChildViewControllers((ViewController) this._missionSelectionMapViewController);
    this._missionSelectionMapViewController.didSelectMissionLevelEvent += new System.Action<MissionSelectionMapViewController, MissionNode>(this.HandleMissionSelectionMapViewControllerDidSelectMissionLevel);
    this._missionLevelDetailViewController.didPressPlayButtonEvent += new System.Action<MissionLevelDetailViewController>(this.HandleMissionLevelDetailViewControllerDidPressPlayButton);
  }

  protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
  {
    if (!removedFromHierarchy)
      return;
    this._missionSelectionMapViewController.didSelectMissionLevelEvent -= new System.Action<MissionSelectionMapViewController, MissionNode>(this.HandleMissionSelectionMapViewControllerDidSelectMissionLevel);
    this._missionLevelDetailViewController.didPressPlayButtonEvent -= new System.Action<MissionLevelDetailViewController>(this.HandleMissionLevelDetailViewControllerDidPressPlayButton);
  }

  public virtual void HandleMissionSelectionMapViewControllerDidSelectMissionLevel(
    MissionSelectionMapViewController viewController,
    MissionNode _missionNode)
  {
    this._missionLevelDetailViewController.Setup(_missionNode);
    if (this._missionLevelDetailViewController.isInViewControllerHierarchy)
      return;
    this.PushViewController((ViewController) this._missionLevelDetailViewController, (System.Action) null);
  }

  public virtual void HandleMissionLevelDetailViewControllerDidPressPlayButton(
    MissionLevelDetailViewController viewController)
  {
    System.Action<MissionSelectionNavigationController> pressPlayButtonEvent = this.didPressPlayButtonEvent;
    if (pressPlayButtonEvent == null)
      return;
    pressPlayButtonEvent(this);
  }

  public virtual void PresentMissionClearedIfNeeded(System.Action<bool> finishedCallback)
  {
    if (!this._missionSelectionMapViewController.animatedUpdateIsRequired)
    {
      System.Action<bool> action = finishedCallback;
      if (action == null)
        return;
      action(false);
    }
    else
      this.PopViewController((System.Action) (() =>
      {
        this._missionSelectionMapViewController.DeselectSelectedNode();
        this._missionSelectionMapViewController.ShowMissionClearedAnimation((System.Action) (() =>
        {
          System.Action<bool> action = finishedCallback;
          if (action == null)
            return;
          action(true);
        }));
      }), false);
  }
}
