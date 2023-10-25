// Decompiled with JetBrains decompiler
// Type: LocalNetworkPlayersViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LocalNetworkPlayersViewController : NetworkPlayersViewController
{
  [SerializeField]
  protected Toggle _enableNetworkingToggle;
  [SerializeField]
  protected Toggle _enableOpenPartyToggle;
  [Inject]
  protected readonly LocalNetworkPlayerModel _localNetworkPlayerModel;
  [Inject]
  protected readonly INetworkConfig _networkConfig;
  protected ToggleBinder _toggleBinder;
  protected bool _enableBroadcasting;
  protected bool _allowAnyoneToJoin;

  public override string myPartyTitle => "MY PARTY";

  public override string otherPlayersTitle => "NEARBY PLAYERS";

  public override INetworkPlayerModel networkPlayerModel => (INetworkPlayerModel) this._localNetworkPlayerModel;

  protected override void NetworkPlayersViewControllerDidActivate(
    bool firstActivation,
    bool addedToHierarchy)
  {
    if (firstActivation)
    {
      this._toggleBinder = new ToggleBinder();
      this._toggleBinder.AddBinding(this._enableNetworkingToggle, new System.Action<bool>(this.HandleNetworkingToggleChanged));
      this._toggleBinder.AddBinding(this._enableOpenPartyToggle, new System.Action<bool>(this.HandleOpenPartyToggleChanged));
    }
    if (!addedToHierarchy)
      return;
    this.RefreshParty();
  }

  protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
  {
    if (!removedFromHierarchy)
      return;
    this.RefreshParty(true);
  }

  protected override void OnDestroy()
  {
    this._toggleBinder?.ClearBindings();
    base.OnDestroy();
  }

  public virtual void HandleNetworkingToggleChanged(bool enabled)
  {
    this._enableBroadcasting = enabled;
    this.RefreshParty();
  }

  public virtual void HandleOpenPartyToggleChanged(bool openParty)
  {
    this._allowAnyoneToJoin = openParty;
    this.RefreshParty();
  }

  public virtual void RefreshParty(bool overrideHide = false)
  {
    LocalNetworkPlayerModel networkPlayerModel = this._localNetworkPlayerModel;
    LocalNetworkPlayerModel.CreatePartyConfig createConfig = new LocalNetworkPlayerModel.CreatePartyConfig();
    createConfig.selectionMask = new BeatmapLevelSelectionMask(BeatmapDifficultyMask.All, GameplayModifierMask.All, SongPackMask.all);
    createConfig.configuration = new GameplayServerConfiguration(this._networkConfig.maxPartySize, this._allowAnyoneToJoin ? DiscoveryPolicy.Public : DiscoveryPolicy.Hidden, this._allowAnyoneToJoin ? InvitePolicy.AnyoneCanInvite : InvitePolicy.OnlyConnectionOwnerCanInvite, GameplayServerMode.Countdown, SongSelectionMode.OwnerPicks, GameplayServerControlSettings.All);
    networkPlayerModel.CreatePartyConnection<LocalNetworkPlayerModel>((INetworkPlayerModelPartyConfig<LocalNetworkPlayerModel>) createConfig);
  }
}
