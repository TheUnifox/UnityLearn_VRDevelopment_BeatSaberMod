// Decompiled with JetBrains decompiler
// Type: PauseController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class PauseController : MonoBehaviour
{
  [Inject]
  protected readonly PauseMenuManager _pauseMenuManager;
  [Inject]
  protected readonly IGamePause _gamePause;
  [Inject]
  protected readonly IMenuButtonTrigger _menuButtonTrigger;
  [Inject]
  protected readonly BeatmapObjectManager _beatmapObjectManager;
  [Inject]
  protected readonly ILevelRestartController _levelRestartController;
  [Inject]
  protected readonly IReturnToMenuController _returnToMenuController;
  [Inject]
  protected readonly IVRPlatformHelper _vrPlatformHelper;
  [Inject]
  protected readonly ILevelStartController _levelStartController;
  [Inject]
  protected readonly PauseController.InitData _initData;
  protected bool _wantsToPause;
  protected bool _paused;

  public event System.Action didPauseEvent;

  public event System.Action didResumeEvent;

  public event System.Action<System.Action<bool>> canPauseEvent;

  public event System.Action didReturnToMenuEvent;

  public bool wantsToPause => this._wantsToPause;

  private bool canPause
  {
    get
    {
      bool value = true;
      System.Action<System.Action<bool>> canPauseEvent = this.canPauseEvent;
      if (canPauseEvent != null)
        canPauseEvent((System.Action<bool>) (newValue => value &= newValue));
      return value && !this._paused;
    }
  }

  public virtual void Start()
  {
    this._vrPlatformHelper.inputFocusWasCapturedEvent += new System.Action(this.HandleInputFocusWasCaptured);
    this._vrPlatformHelper.hmdUnmountedEvent += new System.Action(this.HandleHMDUnmounted);
    this._pauseMenuManager.didFinishResumeAnimationEvent += new System.Action(this.HandlePauseMenuManagerDidFinishResumeAnimation);
    this._pauseMenuManager.didPressContinueButtonEvent += new System.Action(this.HandlePauseMenuManagerDidPressContinueButton);
    this._pauseMenuManager.didPressRestartButtonEvent += new System.Action(this.HandlePauseMenuManagerDidPressRestartButton);
    this._pauseMenuManager.didPressMenuButtonEvent += new System.Action(this.HandlePauseMenuManagerDidPressMenuButton);
    this._menuButtonTrigger.menuButtonTriggeredEvent += new System.Action(this.HandleMenuButtonTriggered);
    this._levelStartController.levelDidStartEvent += new System.Action(this.HandleLevelDidStart);
    this._levelStartController.levelWillStartIntroEvent += new System.Action(this.HandleLevelWillStartIntro);
    this._wantsToPause = this.GetDefaultPausedState();
    if (!this._initData.startPaused)
      return;
    this._wantsToPause = true;
  }

  public virtual void OnDestroy()
  {
    if (this._vrPlatformHelper != null)
    {
      this._vrPlatformHelper.inputFocusWasCapturedEvent -= new System.Action(this.HandleInputFocusWasCaptured);
      this._vrPlatformHelper.hmdUnmountedEvent -= new System.Action(this.HandleHMDUnmounted);
    }
    if (this._menuButtonTrigger != null)
      this._menuButtonTrigger.menuButtonTriggeredEvent -= new System.Action(this.HandleMenuButtonTriggered);
    if ((UnityEngine.Object) this._pauseMenuManager != (UnityEngine.Object) null)
    {
      this._pauseMenuManager.didFinishResumeAnimationEvent -= new System.Action(this.HandlePauseMenuManagerDidFinishResumeAnimation);
      this._pauseMenuManager.didPressContinueButtonEvent -= new System.Action(this.HandlePauseMenuManagerDidPressContinueButton);
      this._pauseMenuManager.didPressRestartButtonEvent -= new System.Action(this.HandlePauseMenuManagerDidPressRestartButton);
      this._pauseMenuManager.didPressMenuButtonEvent -= new System.Action(this.HandlePauseMenuManagerDidPressMenuButton);
    }
    if (this._levelStartController == null)
      return;
    this._levelStartController.levelDidStartEvent -= new System.Action(this.HandleLevelDidStart);
    this._levelStartController.levelWillStartIntroEvent -= new System.Action(this.HandleLevelWillStartIntro);
  }

  public virtual void OnApplicationPause(bool pauseStatus)
  {
    if (!pauseStatus)
      return;
    this.Pause();
  }

  public virtual void Pause()
  {
    if (!this.canPause)
      return;
    this._paused = true;
    this._gamePause.Pause();
    this._pauseMenuManager.ShowMenu();
    this._beatmapObjectManager.HideAllBeatmapObjects(true);
    this._beatmapObjectManager.PauseAllBeatmapObjects(true);
    System.Action didPauseEvent = this.didPauseEvent;
    if (didPauseEvent == null)
      return;
    didPauseEvent();
  }

  public virtual void PauseGameOnStartupIfItShouldBePaused()
  {
    this._wantsToPause = this.GetDefaultPausedState();
    if (this._initData.startPaused)
      this._wantsToPause = true;
    if (!this.wantsToPause)
      return;
    this.Pause();
  }

  public virtual void HandleLevelDidStart() => this.PauseGameOnStartupIfItShouldBePaused();

  public virtual void HandleLevelWillStartIntro() => this.PauseGameOnStartupIfItShouldBePaused();

  public virtual void HandleMenuButtonTriggered() => this.Pause();

  public virtual void HandleInputFocusWasCaptured() => this.Pause();

  public virtual void HandleHMDUnmounted() => this.Pause();

  public virtual void HandlePauseMenuManagerDidFinishResumeAnimation()
  {
    this._paused = false;
    this._wantsToPause = this.GetDefaultPausedState();
    this._gamePause.Resume();
    System.Action didResumeEvent = this.didResumeEvent;
    if (didResumeEvent != null)
      didResumeEvent();
    if (!this.wantsToPause)
      return;
    this.Pause();
  }

  public virtual void HandlePauseMenuManagerDidPressContinueButton()
  {
    this._beatmapObjectManager.HideAllBeatmapObjects(false);
    this._beatmapObjectManager.PauseAllBeatmapObjects(false);
    this._gamePause.WillResume();
    this._pauseMenuManager.StartResumeAnimation();
  }

  public virtual void HandlePauseMenuManagerDidPressRestartButton() => this._levelRestartController.RestartLevel();

  public virtual void HandlePauseMenuManagerDidPressMenuButton()
  {
    System.Action returnToMenuEvent = this.didReturnToMenuEvent;
    if (returnToMenuEvent != null)
      returnToMenuEvent();
    this._returnToMenuController.ReturnToMenu();
  }

  public virtual bool GetDefaultPausedState() => !this._vrPlatformHelper.hasInputFocus;

  public class InitData
  {
    public readonly bool startPaused;

    public InitData(bool startPaused) => this.startPaused = startPaused;
  }
}
