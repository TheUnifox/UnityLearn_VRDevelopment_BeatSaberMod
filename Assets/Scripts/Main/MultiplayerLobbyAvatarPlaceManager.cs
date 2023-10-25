// Decompiled with JetBrains decompiler
// Type: MultiplayerLobbyAvatarPlaceManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MultiplayerLobbyAvatarPlaceManager : MonoBehaviour
{
  [Inject]
  protected readonly ILobbyStateDataModel _lobbyStateDataModel;
  [Inject]
  protected readonly MultiplayerLobbyAvatarPlace.Pool _avatarPlacesPool;
  protected readonly List<MultiplayerLobbyAvatarPlace> _allPlaces = new List<MultiplayerLobbyAvatarPlace>();
  protected float _innerCircleRadius;
  protected float _minOuterCircleRadius;

  public virtual void Activate(float innerCircleRadius, float minOuterCircleRadius)
  {
    this._innerCircleRadius = innerCircleRadius;
    this._minOuterCircleRadius = minOuterCircleRadius;
    this.SpawnAllPlaces();
  }

  public virtual void Deactivate() => this.DespawnAllPlaces();

  public virtual void OnDestroy() => this.DespawnAllPlaces();

  public virtual void SpawnAllPlaces()
  {
    this.DespawnAllPlaces();
    float withEvenAdjustment = MultiplayerPlayerPlacement.GetAngleBetweenPlayersWithEvenAdjustment(this._lobbyStateDataModel.configuration.maxPlayerCount, MultiplayerPlayerLayout.Circle);
    float outerCircleRadius = Mathf.Max(MultiplayerPlayerPlacement.GetOuterCircleRadius(withEvenAdjustment, this._innerCircleRadius), this._minOuterCircleRadius);
    int sortIndex = this._lobbyStateDataModel.localPlayer.sortIndex;
    for (int playerIndex = 0; playerIndex < this._lobbyStateDataModel.configuration.maxPlayerCount; ++playerIndex)
    {
      if (this._lobbyStateDataModel.localPlayer.sortIndex != playerIndex)
      {
        float positionAngleForPlayer = MultiplayerPlayerPlacement.GetOuterCirclePositionAngleForPlayer(playerIndex, sortIndex, withEvenAdjustment);
        Vector3 playerWorldPosition = MultiplayerPlayerPlacement.GetPlayerWorldPosition(outerCircleRadius, positionAngleForPlayer, MultiplayerPlayerLayout.Circle);
        MultiplayerLobbyAvatarPlace lobbyAvatarPlace = this._avatarPlacesPool.Spawn();
        lobbyAvatarPlace.SetPositionAndRotation(playerWorldPosition, Quaternion.Euler(0.0f, positionAngleForPlayer, 0.0f));
        this._allPlaces.Add(lobbyAvatarPlace);
      }
    }
  }

  public virtual void DespawnAllPlaces()
  {
    for (int index = this._allPlaces.Count - 1; index >= 0; --index)
    {
      if ((Object) this._allPlaces[index] != (Object) null)
        this._avatarPlacesPool.Despawn(this._allPlaces[index]);
    }
    this._allPlaces.Clear();
  }
}
