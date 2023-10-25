// Decompiled with JetBrains decompiler
// Type: MultiplayerLobbyController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

public class MultiplayerLobbyController : MonoBehaviour
{
  [SerializeField]
  protected float _innerCircleRadius = 1f;
  [SerializeField]
  protected float _minOuterCircleRadius = 4.4f;
  [Inject]
  protected readonly MultiplayerLobbyAvatarManager _multiplayerLobbyAvatarManager;
  [Inject]
  protected readonly MultiplayerLobbyCenterStageManager _multiplayerLobbyCenterStageManager;
  [Inject]
  protected readonly MultiplayerLobbyAvatarPlaceManager _multiplayerLobbyAvatarPlaceManager;
  [Inject]
  protected readonly MenuEnvironmentManager _menuEnvironmentManager;
  [CompilerGenerated]
  protected bool m_ClobbyActivated;

  public bool lobbyActivated
  {
    get => this.m_ClobbyActivated;
    private set => this.m_ClobbyActivated = value;
  }

  public virtual void ActivateMultiplayerLobby()
  {
    this.lobbyActivated = true;
    this.gameObject.SetActive(this.lobbyActivated);
    this._multiplayerLobbyCenterStageManager.Init(this._innerCircleRadius, this._minOuterCircleRadius);
    this._multiplayerLobbyAvatarManager.Init(this._innerCircleRadius, this._minOuterCircleRadius);
    this._multiplayerLobbyCenterStageManager.ActivateCenterStageManager();
    this._multiplayerLobbyAvatarManager.ActivateMultiplayerLobbyAvatarManager();
    this._multiplayerLobbyAvatarPlaceManager.Activate(this._innerCircleRadius, this._minOuterCircleRadius);
    this._menuEnvironmentManager.ShowEnvironmentType(MenuEnvironmentManager.MenuEnvironmentType.Lobby);
  }

  public virtual void DeactivateMultiplayerLobby()
  {
    this.lobbyActivated = false;
    this._multiplayerLobbyCenterStageManager.DeactivateCenterStageManager();
    this._multiplayerLobbyAvatarManager.DeactivateMultiplayerLobbyAvatarManager();
    this._multiplayerLobbyAvatarPlaceManager.Deactivate();
    this._menuEnvironmentManager.ShowEnvironmentType(MenuEnvironmentManager.MenuEnvironmentType.Default);
    this.gameObject.SetActive(this.lobbyActivated);
  }
}
