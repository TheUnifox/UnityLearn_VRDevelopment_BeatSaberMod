// Decompiled with JetBrains decompiler
// Type: MultiplayerLocalInactivePlayerInGameMenuViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using System.Runtime.CompilerServices;
using Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MultiplayerLocalInactivePlayerInGameMenuViewController : MonoBehaviour
{
  [SerializeField]
  protected Button _disconnectButton;
  [SerializeField]
  protected LocalizedTextMeshProUGUI _disconnectButtonLocalizedText;
  [SerializeField]
  protected Toggle _detailsToggle;
  [SerializeField]
  protected CanvasGroup _globalCanvasGroup;
  [Space]
  [SerializeField]
  protected GameObject _mainBar;
  [SerializeField]
  protected DisconnectPromptView _disconnectPromptView;
  [Space]
  [SerializeField]
  protected LevelBar _levelBar;
  [Space]
  [SerializeField]
  protected GameObject _dontOwnSongGameObject;
  [Space]
  [SerializeField]
  protected GameObject _detailsGameObject;
  [Inject]
  protected readonly LocalPlayerInGameMenuInitData _localPlayerInGameMenuInitData;
  [Inject]
  protected readonly MultiplayerLocalPlayerDisconnectHelper _disconnectHelper;
  [Inject]
  protected readonly MultiplayerController _multiplayerController;
  [Inject]
  protected readonly TimeTweeningManager _tweeningManager;
  protected readonly ButtonBinder _buttonBinder = new ButtonBinder();
  protected readonly ToggleBinder _toggleBinder = new ToggleBinder();
  protected Tween _fadeOutTween;

  public virtual void OnEnable()
  {
    this._buttonBinder.AddBinding(this._disconnectButton, new System.Action(this.DisconnectButtonPressed));
    this._toggleBinder.AddBinding(this._detailsToggle, new System.Action<bool>(this.DetailsToggleValueChanged));
    this._disconnectPromptView.didViewFinishEvent += new System.Action<bool>(this.HandleDisconnectPromptViewDidViewFinish);
  }

  public virtual void OnDisable()
  {
    this._buttonBinder.ClearBindings();
    this._toggleBinder.ClearBindings();
    this._disconnectPromptView.didViewFinishEvent -= new System.Action<bool>(this.HandleDisconnectPromptViewDidViewFinish);
  }

  public virtual void Start()
  {
    if (this._multiplayerController.state == MultiplayerController.State.Outro || this._multiplayerController.state == MultiplayerController.State.Finished)
    {
      this.gameObject.SetActive(false);
    }
    else
    {
      this._disconnectButtonLocalizedText.Key = this._disconnectHelper.ResolveDisconnectButtonString();
      this._levelBar.Setup(this._localPlayerInGameMenuInitData.previewBeatmapLevel, this._localPlayerInGameMenuInitData.beatmapCharacteristic, this._localPlayerInGameMenuInitData.beatmapDifficulty);
      this._dontOwnSongGameObject.SetActive(!this._localPlayerInGameMenuInitData.hasSong);
      this._multiplayerController.stateChangedEvent += new System.Action<MultiplayerController.State>(this.HandleStateChanged);
    }
  }

  public virtual void OnDestroy()
  {
    if ((UnityEngine.Object) this._multiplayerController != (UnityEngine.Object) null)
      this._multiplayerController.stateChangedEvent -= new System.Action<MultiplayerController.State>(this.HandleStateChanged);
    if ((UnityEngine.Object) this._tweeningManager != (UnityEngine.Object) null)
      this._tweeningManager.KillAllTweens((object) this);
    this._buttonBinder?.ClearBindings();
  }

  public virtual void DisconnectButtonPressed()
  {
    this._mainBar.SetActive(false);
    this._disconnectPromptView.Show();
  }

  public virtual void DetailsToggleValueChanged(bool isOn) => this._detailsGameObject.SetActive(isOn);

  public virtual void HandleDisconnectPromptViewDidViewFinish(bool disconnect)
  {
    if (disconnect)
    {
      this.enabled = false;
      this._disconnectHelper.Disconnect(MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndState.NotStarted, (LevelCompletionResults) null);
    }
    else
      this._disconnectPromptView.Hide((System.Action) (() => this._mainBar.SetActive(true)));
  }

  public virtual void HandleStateChanged(MultiplayerController.State state)
  {
    if (state != MultiplayerController.State.Outro && state != MultiplayerController.State.Finished)
      return;
    this._multiplayerController.stateChangedEvent -= new System.Action<MultiplayerController.State>(this.HandleStateChanged);
    if (this._fadeOutTween == null)
    {
      this._globalCanvasGroup.interactable = false;
      this._fadeOutTween = (Tween) new FloatTween(1f, 0.0f, (System.Action<float>) (val => this._globalCanvasGroup.alpha = val), 1f, EaseType.InSine);
      this._fadeOutTween.onCompleted = (System.Action) (() => this.gameObject.SetActive(false));
    }
    this._tweeningManager.RestartTween(this._fadeOutTween, (object) this);
  }

  [CompilerGenerated]
  public virtual void m_CHandleDisconnectPromptViewDidViewFinishm_Eb__22_0() => this._mainBar.SetActive(true);

  [CompilerGenerated]
  public virtual void m_CHandleStateChangedm_Eb__23_0(float val) => this._globalCanvasGroup.alpha = val;

  [CompilerGenerated]
  public virtual void m_CHandleStateChangedm_Eb__23_1() => this.gameObject.SetActive(false);
}
