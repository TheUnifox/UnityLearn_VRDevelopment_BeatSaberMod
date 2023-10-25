// Decompiled with JetBrains decompiler
// Type: LobbyPlayerPermissionsModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;
using Zenject;

public class LobbyPlayerPermissionsModel
{
  [Inject]
  protected readonly IMenuRpcManager _menuRpcManager;
  [Inject]
  protected readonly IMultiplayerSessionManager _multiplayerSessionManager;
  [CompilerGenerated]
  protected bool m_CisPartyOwner;
  [CompilerGenerated]
  protected bool m_ChasRecommendBeatmapPermission;
  [CompilerGenerated]
  protected bool m_ChasRecommendModifiersPermission;
  [CompilerGenerated]
  protected bool m_ChasKickVotePermission;
  [CompilerGenerated]
  protected bool m_ChasInvitePermission;

  public bool isPartyOwner
  {
    get => this.m_CisPartyOwner;
    private set => this.m_CisPartyOwner = value;
  }

  public bool hasRecommendBeatmapPermission
  {
    get => this.m_ChasRecommendBeatmapPermission;
    private set => this.m_ChasRecommendBeatmapPermission = value;
  }

  public bool hasRecommendModifiersPermission
  {
    get => this.m_ChasRecommendModifiersPermission;
    private set => this.m_ChasRecommendModifiersPermission = value;
  }

  public bool hasKickVotePermission
  {
    get => this.m_ChasKickVotePermission;
    private set => this.m_ChasKickVotePermission = value;
  }

  public bool hasInvitePermission
  {
    get => this.m_ChasInvitePermission;
    private set => this.m_ChasInvitePermission = value;
  }

  public event System.Action permissionsChangedEvent;

  public virtual void Activate() => this._menuRpcManager.setPlayersPermissionConfigurationEvent += new System.Action<string, PlayersLobbyPermissionConfigurationNetSerializable>(this.HandleMenuRpcManagerSetPlayersPermissionConfiguration);

  public virtual void Deactivate() => this._menuRpcManager.setPlayersPermissionConfigurationEvent -= new System.Action<string, PlayersLobbyPermissionConfigurationNetSerializable>(this.HandleMenuRpcManagerSetPlayersPermissionConfiguration);

  public virtual void SetPlayerPermissions(
    bool isPartyOwner,
    bool hasRecommendBeatmapPermission,
    bool hasRecommendModifiersPermission,
    bool hasKickVotePermission,
    bool hasInvitePermission)
  {
    this.isPartyOwner = isPartyOwner;
    this.hasRecommendBeatmapPermission = hasRecommendBeatmapPermission;
    this.hasRecommendModifiersPermission = hasRecommendModifiersPermission;
    this.hasKickVotePermission = hasKickVotePermission;
    this.hasInvitePermission = hasInvitePermission;
    System.Action permissionsChangedEvent = this.permissionsChangedEvent;
    if (permissionsChangedEvent == null)
      return;
    permissionsChangedEvent();
  }

  public virtual void HandleMenuRpcManagerSetPlayersPermissionConfiguration(
    string userId,
    PlayersLobbyPermissionConfigurationNetSerializable playersLobbyPermissionConfiguration)
  {
    foreach (PlayerLobbyPermissionConfigurationNetSerializable configurationNetSerializable in playersLobbyPermissionConfiguration.playersPermission)
    {
      if (configurationNetSerializable.userId == this._multiplayerSessionManager.localPlayer.userId)
        this.SetPlayerPermissions(configurationNetSerializable.isServerOwner, configurationNetSerializable.hasRecommendBeatmapsPermission, configurationNetSerializable.hasRecommendGameplayModifiersPermission, configurationNetSerializable.hasKickVotePermission, configurationNetSerializable.hasInvitePermission);
    }
  }
}
