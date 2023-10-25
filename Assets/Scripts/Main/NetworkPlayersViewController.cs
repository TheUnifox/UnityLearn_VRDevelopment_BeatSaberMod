// Decompiled with JetBrains decompiler
// Type: NetworkPlayersViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using UnityEngine;

public abstract class NetworkPlayersViewController : ViewController
{
  [SerializeField]
  private NetworkPlayersTableView _networkPlayersTableView;
  private bool _refreshIsNeeded;

  public abstract string myPartyTitle { get; }

  public abstract string otherPlayersTitle { get; }

  public abstract INetworkPlayerModel networkPlayerModel { get; }

  public event System.Action<INetworkPlayer> onJoinRequestEvent;

  public event System.Action<INetworkPlayer> onInviteRequestEvent;

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (addedToHierarchy)
    {
      this.networkPlayerModel.joinRequestedEvent += new System.Action<INetworkPlayer>(this.HandleJoinRequest);
      this.networkPlayerModel.inviteRequestedEvent += new System.Action<INetworkPlayer>(this.HandleInviteRequest);
      this.networkPlayerModel.partyChangedEvent += new System.Action<INetworkPlayerModel>(this.HandlePartyChanged);
    }
    this.NetworkPlayersViewControllerDidActivate(firstActivation, addedToHierarchy);
    if (!(this._refreshIsNeeded | addedToHierarchy))
      return;
    this.Refresh();
  }

  protected virtual void NetworkPlayersViewControllerDidActivate(
    bool firstActivation,
    bool addedToHierarchy)
  {
  }

  protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
  {
    if (removedFromHierarchy)
    {
      this.networkPlayerModel.joinRequestedEvent -= new System.Action<INetworkPlayer>(this.HandleJoinRequest);
      this.networkPlayerModel.inviteRequestedEvent -= new System.Action<INetworkPlayer>(this.HandleInviteRequest);
      this.networkPlayerModel.partyChangedEvent -= new System.Action<INetworkPlayerModel>(this.HandlePartyChanged);
    }
    this.NetworkPlayersViewControllerDidDeactivate(removedFromHierarchy);
  }

  protected virtual void NetworkPlayersViewControllerDidDeactivate(bool removedFromHierarchy)
  {
  }

  protected override void OnDestroy() => base.OnDestroy();

  private void HandlePartyChanged(INetworkPlayerModel playerModel)
  {
    if (this.isActivated)
      this.Refresh();
    else
      this._refreshIsNeeded = true;
  }

  private void HandleJoinRequest(INetworkPlayer player)
  {
    System.Action<INetworkPlayer> joinRequestEvent = this.onJoinRequestEvent;
    if (joinRequestEvent == null)
      return;
    joinRequestEvent(player);
  }

  private void HandleInviteRequest(INetworkPlayer player)
  {
    System.Action<INetworkPlayer> inviteRequestEvent = this.onInviteRequestEvent;
    if (inviteRequestEvent == null)
      return;
    inviteRequestEvent(player);
  }

  private void Refresh()
  {
    this._refreshIsNeeded = false;
    this._networkPlayersTableView.SetParties(this.networkPlayerModel.partyPlayers, this.networkPlayerModel.otherPlayers, this.myPartyTitle, this.otherPlayersTitle);
  }
}
