// Decompiled with JetBrains decompiler
// Type: MultiplayerLobbyAvatarManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MultiplayerLobbyAvatarManager : MonoBehaviour
{
  [Inject]
  protected readonly ILobbyStateDataModel _lobbyStateDataModel;
  [Inject]
  protected readonly MultiplayerLobbyAvatarController.Factory _avatarControllerFactory;
  protected float _innerCircleRadius = 1f;
  protected float _minOuterCircleRadius = 4.4f;
  protected readonly Dictionary<string, MultiplayerLobbyAvatarController> _playerIdToAvatarMap = new Dictionary<string, MultiplayerLobbyAvatarController>();
  protected readonly HashSet<MultiplayerLobbyAvatarController> _inProgressDespawnAnimations = new HashSet<MultiplayerLobbyAvatarController>();

  public virtual void Init(float innerCircleRadius, float minOuterCircleRadius)
  {
    this._minOuterCircleRadius = minOuterCircleRadius;
    this._innerCircleRadius = innerCircleRadius;
  }

  public virtual void ActivateMultiplayerLobbyAvatarManager()
  {
    this._lobbyStateDataModel.playerConnectedEvent += new System.Action<IConnectedPlayer>(this.HandleLobbyStateDataModelPlayerConnected);
    this._lobbyStateDataModel.playerDisconnectedEvent += new System.Action<IConnectedPlayer>(this.HandleLobbyStateDataModelPlayerDisconnected);
    foreach (IConnectedPlayer connectedPlayer in this._lobbyStateDataModel.connectedPlayers)
      this.AddPlayer(connectedPlayer);
  }

  public virtual void DeactivateMultiplayerLobbyAvatarManager()
  {
    this._lobbyStateDataModel.playerConnectedEvent -= new System.Action<IConnectedPlayer>(this.HandleLobbyStateDataModelPlayerConnected);
    this._lobbyStateDataModel.playerDisconnectedEvent -= new System.Action<IConnectedPlayer>(this.HandleLobbyStateDataModelPlayerDisconnected);
    this.StopAllCoroutines();
    foreach (KeyValuePair<string, MultiplayerLobbyAvatarController> playerIdToAvatar in this._playerIdToAvatarMap)
      playerIdToAvatar.Value.DestroySelf();
    foreach (MultiplayerLobbyAvatarController despawnAnimation in this._inProgressDespawnAnimations)
      despawnAnimation.DestroySelf();
    this._inProgressDespawnAnimations.Clear();
    this._playerIdToAvatarMap.Clear();
  }

  public virtual void HandleLobbyStateDataModelPlayerConnected(IConnectedPlayer connectedPlayer) => this.AddPlayer(connectedPlayer);

  public virtual void HandleLobbyStateDataModelPlayerDisconnected(IConnectedPlayer connectedPlayer) => this.RemovePlayer(connectedPlayer);

  public virtual void AddPlayer(IConnectedPlayer connectedPlayer)
  {
    if (connectedPlayer.isMe)
      return;
    string userId = connectedPlayer.userId;
    MultiplayerLobbyAvatarController avatarController1;
    if (this._playerIdToAvatarMap.TryGetValue(userId, out avatarController1))
    {
      avatarController1.DestroySelf();
      this._playerIdToAvatarMap.Remove(userId);
    }
    MultiplayerLobbyAvatarController avatarController2 = this._avatarControllerFactory.Create(connectedPlayer);
    this._playerIdToAvatarMap[userId] = avatarController2;
    float withEvenAdjustment = MultiplayerPlayerPlacement.GetAngleBetweenPlayersWithEvenAdjustment(this._lobbyStateDataModel.configuration.maxPlayerCount, MultiplayerPlayerLayout.Circle);
    double outerCircleRadius = (double) Mathf.Max(MultiplayerPlayerPlacement.GetOuterCircleRadius(withEvenAdjustment, this._innerCircleRadius), this._minOuterCircleRadius);
    float positionAngleForPlayer = MultiplayerPlayerPlacement.GetOuterCirclePositionAngleForPlayer(connectedPlayer.sortIndex, this._lobbyStateDataModel.localPlayer.sortIndex, withEvenAdjustment);
    double outerCirclePositionAngle = (double) positionAngleForPlayer;
    Vector3 playerWorldPosition = MultiplayerPlayerPlacement.GetPlayerWorldPosition((float) outerCircleRadius, (float) outerCirclePositionAngle, MultiplayerPlayerLayout.Circle);
    avatarController2.ShowSpawnAnimation(playerWorldPosition, Quaternion.Euler(0.0f, positionAngleForPlayer, 0.0f));
  }

  public virtual void RemovePlayer(IConnectedPlayer connectedPlayer)
  {
    MultiplayerLobbyAvatarController multiplayerAvatar;
    if (!this._playerIdToAvatarMap.TryGetValue(connectedPlayer.userId, out multiplayerAvatar))
      return;
    this.StartCoroutine(this.RemovePlayerAndDestroy(connectedPlayer.userId, multiplayerAvatar));
  }

  public virtual IEnumerator RemovePlayerAndDestroy(
    string userId,
    MultiplayerLobbyAvatarController multiplayerAvatar)
  {
    MultiplayerLobbyAvatarManager lobbyAvatarManager = this;
    lobbyAvatarManager._playerIdToAvatarMap.Remove(userId);
    if (lobbyAvatarManager.gameObject.activeInHierarchy)
    {
      lobbyAvatarManager._inProgressDespawnAnimations.Add(multiplayerAvatar);
      yield return (object) multiplayerAvatar.ShowDespawnAnimationAndDestroy();
      lobbyAvatarManager._inProgressDespawnAnimations.Remove(multiplayerAvatar);
    }
    else
      multiplayerAvatar.DestroySelf();
  }
}
