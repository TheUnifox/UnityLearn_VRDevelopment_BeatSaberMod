// Decompiled with JetBrains decompiler
// Type: MultiplayerLocalActivePlayerInGameMenuController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class MultiplayerLocalActivePlayerInGameMenuController : MonoBehaviour
{
  [Inject]
  protected readonly MultiplayerLocalActivePlayerInGameMenuViewController _inGameMenuViewController;
  [Inject]
  protected readonly IMenuButtonTrigger _menuButtonTrigger;
  [Inject]
  protected readonly IVRPlatformHelper _vrPlatformHelper;
  [Inject]
  protected readonly IGamePause _gamePause;
  [Inject]
  protected readonly MultiplayerController _multiplayerController;
  [Inject]
  protected readonly SaberManager _saberManager;
  [Inject]
  protected readonly BeatmapObjectManager _beatmapObjectManager;
  protected bool _gameMenuIsShown;

  public event System.Action didGiveUpEvent;

  public event System.Action<System.Action<bool>> canShowInGameMenuEvent;

  public event System.Action requestsDisconnectEvent;

  public bool gameMenuIsShown => this._gameMenuIsShown;

  private bool canShowInGameMenu
  {
    get
    {
      bool value = true;
      System.Action<System.Action<bool>> showInGameMenuEvent = this.canShowInGameMenuEvent;
      if (showInGameMenuEvent != null)
        showInGameMenuEvent((System.Action<bool>) (newValue => value &= newValue));
      return value && !this._gameMenuIsShown;
    }
  }

  public virtual void Start()
  {
    this.HideInGameMenu();
    this._vrPlatformHelper.inputFocusWasCapturedEvent += new System.Action(this.HandleInputFocusWasCaptured);
    this._inGameMenuViewController.didPressDisconnectButtonEvent += new System.Action(this.HandleInGameMenuViewControllerDidPressDisconnectButton);
    this._inGameMenuViewController.didPressResumeButtonEvent += new System.Action(this.HandleInGameMenuViewControllerDidPressResumeButton);
    this._inGameMenuViewController.didPressGiveUpButtonEvent += new System.Action(this.HandleInGameMenuViewControllerDidPressGiveUpButton);
    this._menuButtonTrigger.menuButtonTriggeredEvent += new System.Action(this.HandleMenuButtonTriggered);
    this._multiplayerController.stateChangedEvent += new System.Action<MultiplayerController.State>(this.HandleStateChanged);
  }

  public virtual void OnDestroy()
  {
    if (this._vrPlatformHelper != null)
      this._vrPlatformHelper.inputFocusWasCapturedEvent -= new System.Action(this.HandleInputFocusWasCaptured);
    if (this._menuButtonTrigger != null)
      this._menuButtonTrigger.menuButtonTriggeredEvent -= new System.Action(this.HandleMenuButtonTriggered);
    if ((UnityEngine.Object) this._inGameMenuViewController != (UnityEngine.Object) null)
    {
      this._inGameMenuViewController.didPressDisconnectButtonEvent -= new System.Action(this.HandleInGameMenuViewControllerDidPressDisconnectButton);
      this._inGameMenuViewController.didPressResumeButtonEvent -= new System.Action(this.HandleInGameMenuViewControllerDidPressResumeButton);
      this._inGameMenuViewController.didPressGiveUpButtonEvent -= new System.Action(this.HandleInGameMenuViewControllerDidPressGiveUpButton);
    }
    if (!((UnityEngine.Object) this._multiplayerController != (UnityEngine.Object) null))
      return;
    this._multiplayerController.stateChangedEvent -= new System.Action<MultiplayerController.State>(this.HandleStateChanged);
  }

  public virtual void OnApplicationPause(bool pauseStatus)
  {
    if (!pauseStatus)
      return;
    this.ShowInGameMenu();
  }

  public virtual void ShowInGameMenu()
  {
    if (!this.canShowInGameMenu)
      return;
    this._gamePause.Pause();
    this._gameMenuIsShown = true;
    this._saberManager.disableSabers = true;
    this._inGameMenuViewController.ShowMenu();
    this._beatmapObjectManager.HideAllBeatmapObjects(true);
    this._beatmapObjectManager.spawnHidden = true;
  }

  public virtual void HideInGameMenu()
  {
    this._inGameMenuViewController.HideMenu();
    this._gameMenuIsShown = false;
    this._saberManager.disableSabers = false;
    this._gamePause.WillResume();
    this._gamePause.Resume();
    this._beatmapObjectManager.HideAllBeatmapObjects(false);
    this._beatmapObjectManager.spawnHidden = false;
  }

  public virtual void HandleMenuButtonTriggered() => this.ShowInGameMenu();

  public virtual void HandleInputFocusWasCaptured() => this.ShowInGameMenu();

  public virtual void HandleInGameMenuViewControllerDidPressResumeButton() => this.HideInGameMenu();

  public virtual void HandleInGameMenuViewControllerDidPressGiveUpButton()
  {
    this._inGameMenuViewController.HideMenu();
    System.Action didGiveUpEvent = this.didGiveUpEvent;
    if (didGiveUpEvent == null)
      return;
    didGiveUpEvent();
  }

  public virtual void HandleInGameMenuViewControllerDidPressDisconnectButton()
  {
    System.Action requestsDisconnectEvent = this.requestsDisconnectEvent;
    if (requestsDisconnectEvent == null)
      return;
    requestsDisconnectEvent();
  }

  public virtual void HandleStateChanged(MultiplayerController.State state)
  {
    if (state != MultiplayerController.State.Outro && state != MultiplayerController.State.Finished)
      return;
    this._vrPlatformHelper.inputFocusWasCapturedEvent -= new System.Action(this.HandleInputFocusWasCaptured);
    this._menuButtonTrigger.menuButtonTriggeredEvent -= new System.Action(this.HandleMenuButtonTriggered);
    this.HideInGameMenu();
  }
}
