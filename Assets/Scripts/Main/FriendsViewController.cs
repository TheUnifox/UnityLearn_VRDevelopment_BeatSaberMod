// Decompiled with JetBrains decompiler
// Type: FriendsViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class FriendsViewController : NetworkPlayersViewController
{
  [SerializeField]
  protected Toggle _enableOpenPartyToggle;
  [Inject]
  protected readonly PlatformNetworkPlayerModel _networkPlayerModel;
  [Inject]
  protected readonly INetworkConfig _networkConfig;
  protected ToggleBinder _toggleBinder;
  protected bool _allowAnyoneToJoin;

  public override string myPartyTitle => "MY PARTY";

  public override string otherPlayersTitle => "FRIENDS";

  public override INetworkPlayerModel networkPlayerModel => (INetworkPlayerModel) this._networkPlayerModel;

  protected override void NetworkPlayersViewControllerDidActivate(
    bool firstActivation,
    bool addedToHierarchy)
  {
    if (firstActivation)
    {
      this._toggleBinder = new ToggleBinder();
      this._toggleBinder.AddBinding(this._enableOpenPartyToggle, new System.Action<bool>(this.HandleOpenPartyToggleChanged));
    }
    if (!addedToHierarchy)
      return;
    this._networkPlayerModel.discoveryEnabled = true;
    this.RefreshParty();
  }

  protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
  {
    if (!removedFromHierarchy)
      return;
    this._networkPlayerModel.discoveryEnabled = false;
    this.RefreshParty(true);
  }

  protected override void OnDestroy()
  {
    this._toggleBinder?.ClearBindings();
    base.OnDestroy();
  }

  public virtual void HandleOpenPartyToggleChanged(bool openParty)
  {
    this._allowAnyoneToJoin = openParty;
    this.RefreshParty();
  }

  public virtual void RefreshParty(bool overrideHide = false)
  {
    PlatformNetworkPlayerModel networkPlayerModel = this._networkPlayerModel;
    PlatformNetworkPlayerModel.CreatePartyConfig createConfig = new PlatformNetworkPlayerModel.CreatePartyConfig();
    createConfig.configuration = new GameplayServerConfiguration(this._networkConfig.maxPartySize, !this._allowAnyoneToJoin || overrideHide ? DiscoveryPolicy.Hidden : DiscoveryPolicy.Public, InvitePolicy.OnlyConnectionOwnerCanInvite, GameplayServerMode.Countdown, SongSelectionMode.OwnerPicks, GameplayServerControlSettings.All);
    networkPlayerModel.CreatePartyConnection<PlatformNetworkPlayerModel>((INetworkPlayerModelPartyConfig<PlatformNetworkPlayerModel>) createConfig);
  }
}
