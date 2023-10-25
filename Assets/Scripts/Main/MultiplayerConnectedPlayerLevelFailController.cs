// Decompiled with JetBrains decompiler
// Type: MultiplayerConnectedPlayerLevelFailController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class MultiplayerConnectedPlayerLevelFailController : MonoBehaviour
{
  [SerializeField]
  protected VFXController _failVFXController;
  [Inject]
  protected readonly IMultiplayerSessionManager _multiplayerSessionManager;
  [Inject]
  protected readonly BeatmapObjectManager _beatmapObjectManager;
  [Inject]
  protected readonly IConnectedPlayer _connectedPlayer;
  protected bool _wasActive;

  public event System.Action playerDidFailEvent;

  public virtual void Start()
  {
    this._wasActive = this._connectedPlayer.IsActive();
    this._multiplayerSessionManager.playerStateChangedEvent += new System.Action<IConnectedPlayer>(this.HandlePlayerStateChanged);
    this._multiplayerSessionManager.playerDisconnectedEvent += new System.Action<IConnectedPlayer>(this.HandlePlayerDisconnected);
  }

  public virtual void OnDestroy()
  {
    if (this._multiplayerSessionManager == null)
      return;
    this._multiplayerSessionManager.playerStateChangedEvent -= new System.Action<IConnectedPlayer>(this.HandlePlayerStateChanged);
    this._multiplayerSessionManager.playerDisconnectedEvent -= new System.Action<IConnectedPlayer>(this.HandlePlayerDisconnected);
  }

  public virtual void CheckIfPlayerFailed(IConnectedPlayer player)
  {
    if (player.userId != this._connectedPlayer.userId || !this._wasActive || player.IsActiveOrFinished() && player.isConnected)
      return;
    this._wasActive = false;
    this._beatmapObjectManager.DissolveAllObjects();
    System.Action playerDidFailEvent = this.playerDidFailEvent;
    if (playerDidFailEvent != null)
      playerDidFailEvent();
    this._failVFXController.Play();
  }

  public virtual void HandlePlayerDisconnected(IConnectedPlayer player) => this.CheckIfPlayerFailed(player);

  public virtual void HandlePlayerStateChanged(IConnectedPlayer player) => this.CheckIfPlayerFailed(player);
}
