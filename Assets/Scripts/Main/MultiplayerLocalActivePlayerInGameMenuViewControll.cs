// Decompiled with JetBrains decompiler
// Type: MultiplayerLocalActivePlayerInGameMenuViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MultiplayerLocalActivePlayerInGameMenuViewController : MonoBehaviour
{
  [SerializeField]
  protected Button _disconnectButton;
  [SerializeField]
  protected LocalizedTextMeshProUGUI _disconnectButtonLocalizedText;
  [SerializeField]
  protected Button _giveUpButton;
  [SerializeField]
  protected Button _resumeButton;
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
  protected GameObject _menuWrapperGameObject;
  [SerializeField]
  protected GameObject _menuControllersGameObject;
  [Inject]
  protected readonly IConnectedPlayer _connectedPlayer;
  [Inject]
  protected readonly LocalPlayerInGameMenuInitData _localPlayerInGameMenuInitData;
  [Inject]
  protected readonly VRControllersInputManager _vrControllersInputManager;
  [Inject]
  protected readonly MultiplayerLocalPlayerDisconnectHelper _disconnectHelper;
  protected ButtonBinder _buttonBinder;
  protected float _disabledInteractionRemainingTime;
  protected const float kDisabledInteractionDuration = 0.2f;

  public event System.Action didPressDisconnectButtonEvent;

  public event System.Action didPressGiveUpButtonEvent;

  public event System.Action didPressResumeButtonEvent;

  public virtual void Awake()
  {
    this._buttonBinder = new ButtonBinder();
    this._buttonBinder.AddBinding(this._resumeButton, new System.Action(this.ResumeButtonPressed));
    this._buttonBinder.AddBinding(this._giveUpButton, new System.Action(this.GiveUpButtonPressed));
    this._buttonBinder.AddBinding(this._disconnectButton, new System.Action(this.DisconnectButtonPressed));
    this._disconnectPromptView.didViewFinishEvent += new System.Action<bool>(this.HandleDisconnectPromptViewDidViewFinish);
  }

  public virtual void Start()
  {
    this._disconnectButtonLocalizedText.Key = this._disconnectHelper.ResolveDisconnectButtonString();
    this._levelBar.Setup(this._localPlayerInGameMenuInitData.previewBeatmapLevel, this._localPlayerInGameMenuInitData.beatmapCharacteristic, this._localPlayerInGameMenuInitData.beatmapDifficulty);
  }

  public virtual void OnDestroy()
  {
    this._buttonBinder?.ClearBindings();
    this._disconnectPromptView.didViewFinishEvent -= new System.Action<bool>(this.HandleDisconnectPromptViewDidViewFinish);
  }

  public virtual void Update()
  {
    if ((double) this._disabledInteractionRemainingTime > 0.0)
    {
      this._disabledInteractionRemainingTime -= Time.deltaTime;
    }
    else
    {
      if (!this._vrControllersInputManager.MenuButtonDown())
        return;
      this.ResumeButtonPressed();
    }
  }

  public virtual void ShowMenu()
  {
    this.enabled = true;
    this._disabledInteractionRemainingTime = 0.2f;
    this._menuWrapperGameObject.SetActive(true);
    this._menuControllersGameObject.SetActive(true);
  }

  public virtual void HideMenu()
  {
    this.enabled = false;
    this._menuWrapperGameObject.SetActive(false);
    this._menuControllersGameObject.SetActive(false);
  }

  public virtual void DisconnectButtonPressed()
  {
    this._mainBar.SetActive(false);
    this._disconnectPromptView.Show();
  }

  public virtual void HandleDisconnectPromptViewDidViewFinish(bool disconnect)
  {
    if (disconnect)
    {
      this.enabled = false;
      System.Action disconnectButtonEvent = this.didPressDisconnectButtonEvent;
      if (disconnectButtonEvent == null)
        return;
      disconnectButtonEvent();
    }
    else
      this._disconnectPromptView.Hide((System.Action) (() => this._mainBar.SetActive(true)));
  }

  public virtual void GiveUpButtonPressed()
  {
    if (!this._connectedPlayer.IsActive())
      return;
    this.enabled = false;
    System.Action giveUpButtonEvent = this.didPressGiveUpButtonEvent;
    if (giveUpButtonEvent == null)
      return;
    giveUpButtonEvent();
  }

  public virtual void ResumeButtonPressed()
  {
    this.enabled = false;
    System.Action resumeButtonEvent = this.didPressResumeButtonEvent;
    if (resumeButtonEvent == null)
      return;
    resumeButtonEvent();
  }

  [CompilerGenerated]
  public virtual void m_CHandleDisconnectPromptViewDidViewFinishm_Eb__32_0() => this._mainBar.SetActive(true);
}
