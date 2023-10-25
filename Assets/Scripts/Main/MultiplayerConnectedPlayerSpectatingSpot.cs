// Decompiled with JetBrains decompiler
// Type: MultiplayerConnectedPlayerSpectatingSpot
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

public class MultiplayerConnectedPlayerSpectatingSpot : MonoBehaviour, IMultiplayerSpectatingSpot
{
  [Inject]
  protected readonly IConnectedPlayer _connectedPlayer;
  [Inject]
  protected readonly IMultiplayerSessionManager _multiplayerSessionManager;
  [Inject]
  protected readonly MultiplayerSpectatingSpotManager _spectatingSpotManager;
  [CompilerGenerated]
  protected IMultiplayerObservable m_Cobservable;
  protected bool _playerFailed;

  public event System.Action<IMultiplayerSpectatingSpot> hasBeenRemovedEvent;

  public event System.Action<bool> isObservedChangedEvent;

  public IMultiplayerObservable observable
  {
    get => this.m_Cobservable;
    private set => this.m_Cobservable = value;
  }

  public string spotName => this._connectedPlayer.userName;

  public bool isMain => false;

  public virtual void Start()
  {
    this.observable = (IMultiplayerObservable) new MultiplayerConnectedPlayerObservable(this._connectedPlayer);
    this._playerFailed = this._connectedPlayer.IsFailed();
    if (!this._playerFailed)
      this._spectatingSpotManager.RegisterSpectatingSpot((IMultiplayerSpectatingSpot) this);
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

  public virtual void SetIsObserved(bool isObserved)
  {
    System.Action<bool> observedChangedEvent = this.isObservedChangedEvent;
    if (observedChangedEvent == null)
      return;
    observedChangedEvent(isObserved);
  }

  public virtual void ReloadBasedOnPlayerCurrentState(IConnectedPlayer connectedPlayer)
  {
    if (connectedPlayer.userId != this._connectedPlayer.userId || !connectedPlayer.IsFailed() || this._playerFailed)
      return;
    this._playerFailed = true;
    System.Action<IMultiplayerSpectatingSpot> beenRemovedEvent = this.hasBeenRemovedEvent;
    if (beenRemovedEvent == null)
      return;
    beenRemovedEvent((IMultiplayerSpectatingSpot) this);
  }

  public virtual void HandlePlayerStateChanged(IConnectedPlayer connectedPlayer) => this.ReloadBasedOnPlayerCurrentState(connectedPlayer);

  public virtual void HandlePlayerDisconnected(IConnectedPlayer connectedPlayer) => this.ReloadBasedOnPlayerCurrentState(connectedPlayer);

}
