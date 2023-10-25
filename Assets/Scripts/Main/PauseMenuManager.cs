// Decompiled with JetBrains decompiler
// Type: PauseMenuManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PauseMenuManager : MonoBehaviour
{
  [SerializeField]
  protected PauseAnimationController _pauseAnimationController;
  [SerializeField]
  [NullAllowed]
  protected LevelBar _levelBar;
  [SerializeField]
  protected Button _continueButton;
  [SerializeField]
  protected Button _restartButton;
  [SerializeField]
  protected Button _backButton;
  [SerializeField]
  protected TextMeshProUGUI _backButtonText;
  [SerializeField]
  protected Transform _pauseContainerTransform;
  [Inject]
  protected readonly PauseMenuManager.InitData _initData;
  [Inject]
  protected readonly VRControllersInputManager _vrControllersInputManager;
  [Inject]
  protected readonly EnvironmentSpawnRotation _environmentSpawnRotation;
  protected ButtonBinder _buttonBinder;
  protected float _disabledInteractionRemainingTime;
  protected const float kDisabledInteractionDuration = 0.2f;

  public event System.Action didPressContinueButtonEvent;

  public event System.Action didPressMenuButtonEvent;

  public event System.Action didPressRestartButtonEvent;

  public event System.Action didFinishResumeAnimationEvent;

  public virtual void Awake()
  {
    this._pauseAnimationController.resumeFromPauseAnimationDidFinishEvent += new System.Action(this.HandleResumeFromPauseAnimationDidFinish);
    this.enabled = false;
    this._buttonBinder = new ButtonBinder();
    this._buttonBinder.AddBinding(this._continueButton, new System.Action(this.ContinueButtonPressed));
    this._buttonBinder.AddBinding(this._restartButton, new System.Action(this.RestartButtonPressed));
    this._buttonBinder.AddBinding(this._backButton, new System.Action(this.MenuButtonPressed));
  }

  public virtual void Start()
  {
    if (this._initData.showLevelBar)
      this._levelBar.Setup(this._initData.previewBeatmapLevel, this._initData.beatmapCharacteristic, this._initData.beatmapDifficulty);
    else if ((UnityEngine.Object) this._levelBar != (UnityEngine.Object) null)
      this._levelBar.gameObject.SetActive(false);
    if ((bool) (UnityEngine.Object) this._restartButton)
      this._restartButton.gameObject.SetActive(this._initData.showRestartButton);
    this._backButtonText.text = this._initData.backButtonText;
  }

  public virtual void OnDestroy()
  {
    if ((UnityEngine.Object) this._pauseAnimationController != (UnityEngine.Object) null)
      this._pauseAnimationController.resumeFromPauseAnimationDidFinishEvent -= new System.Action(this.HandleResumeFromPauseAnimationDidFinish);
    this._buttonBinder?.ClearBindings();
  }

  public virtual void Update()
  {
    if ((double) this._disabledInteractionRemainingTime > 0.0)
    {
      this._disabledInteractionRemainingTime -= Time.deltaTime;
    }
    else
    {
      if (this._vrControllersInputManager.MenuButtonDown())
        this.ContinueButtonPressed();
      if (Input.GetKeyDown(KeyCode.R))
        this.RestartButtonPressed();
      if (Input.GetKeyDown(KeyCode.M))
        this.MenuButtonPressed();
      if (!Input.GetKeyDown(KeyCode.C))
        return;
      this.ContinueButtonPressed();
    }
  }

  public virtual void ShowMenu()
  {
    this.enabled = true;
    this._disabledInteractionRemainingTime = 0.2f;
    this._pauseAnimationController.StartEnterPauseAnimation();
    this._pauseContainerTransform.eulerAngles = new Vector3(0.0f, this._environmentSpawnRotation.targetRotation, 0.0f);
  }

  public virtual void StartResumeAnimation()
  {
    this.enabled = false;
    this._pauseAnimationController.StartResumeFromPauseAnimation();
  }

  public virtual void HandleResumeFromPauseAnimationDidFinish()
  {
    System.Action resumeAnimationEvent = this.didFinishResumeAnimationEvent;
    if (resumeAnimationEvent == null)
      return;
    resumeAnimationEvent();
  }

  public virtual void MenuButtonPressed()
  {
    this.enabled = false;
    System.Action pressMenuButtonEvent = this.didPressMenuButtonEvent;
    if (pressMenuButtonEvent == null)
      return;
    pressMenuButtonEvent();
  }

  public virtual void RestartButtonPressed()
  {
    if (!this._initData.showRestartButton)
      return;
    this.enabled = false;
    System.Action restartButtonEvent = this.didPressRestartButtonEvent;
    if (restartButtonEvent == null)
      return;
    restartButtonEvent();
  }

  public virtual void ContinueButtonPressed()
  {
    this.enabled = false;
    System.Action continueButtonEvent = this.didPressContinueButtonEvent;
    if (continueButtonEvent == null)
      return;
    continueButtonEvent();
  }

  public class InitData
  {
    public readonly string backButtonText;
    public readonly IPreviewBeatmapLevel previewBeatmapLevel;
    public readonly BeatmapDifficulty beatmapDifficulty;
    public readonly BeatmapCharacteristicSO beatmapCharacteristic;
    public readonly bool showRestartButton;
    public readonly bool showLevelBar;

    public InitData(
      string backButtonText,
      IPreviewBeatmapLevel previewBeatmapLevel,
      BeatmapDifficulty beatmapDifficulty,
      BeatmapCharacteristicSO beatmapCharacteristic,
      bool showRestartButton,
      bool showLevelBar)
    {
      this.backButtonText = backButtonText;
      this.previewBeatmapLevel = previewBeatmapLevel;
      this.beatmapDifficulty = beatmapDifficulty;
      this.beatmapCharacteristic = beatmapCharacteristic;
      this.showRestartButton = showRestartButton;
      this.showLevelBar = showLevelBar;
    }
  }
}
