// Decompiled with JetBrains decompiler
// Type: MultiplayerSettingsPanelController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerSettingsPanelController : MonoBehaviour, IRefreshable
{
  [SerializeField]
  protected ServerCodeView _serverCodeView;
  [SerializeField]
  protected Toggle _spectateToggle;
  [SerializeField]
  protected GameObject _connectionSettingsWrapper;
  [SerializeField]
  protected GameObject _spectateSettingsWrapper;
  protected ToggleBinder _toggleBinder;
  protected ILobbyPlayerData _lobbyPlayerData;
  protected bool _refreshed;

  public event System.Action<bool> playerActiveStateChangedEvent;

  public virtual void SetLobbyPlayerDataModel(ILobbyPlayerData lobbyPlayerData)
  {
    this._refreshed = false;
    this._lobbyPlayerData = lobbyPlayerData;
    this.Refresh();
  }

  public virtual void HideConnectionSettings(bool hide) => this._connectionSettingsWrapper.SetActive(!hide);

  public virtual void HideSpectateSettings(bool hide) => this._spectateSettingsWrapper.SetActive(!hide);

  public virtual void SetLobbyCode(string code) => this._serverCodeView.SetCode(code);

  public virtual void Awake()
  {
    this._toggleBinder = new ToggleBinder();
    this._toggleBinder.AddBinding(this._spectateToggle, (System.Action<bool>) (on => this.UpdateLocalPlayerIsActiveState(on)));
  }

  public virtual void OnDestroy() => this._toggleBinder?.ClearBindings();

  public virtual void UpdateLocalPlayerIsActiveState(bool isActive)
  {
    System.Action<bool> stateChangedEvent = this.playerActiveStateChangedEvent;
    if (stateChangedEvent == null)
      return;
    stateChangedEvent(!isActive);
  }

  public virtual void Refresh()
  {
    if (this._refreshed)
      return;
    this._refreshed = true;
    if (this._lobbyPlayerData != null)
    {
      this._spectateToggle.isOn = !this._lobbyPlayerData.isActive;
    }
    else
    {
      this._spectateToggle.isOn = false;
      this._spectateToggle.interactable = false;
    }
  }

  [CompilerGenerated]
  public virtual void m_CAwakem_Eb__14_0(bool on) => this.UpdateLocalPlayerIsActiveState(on);
}
