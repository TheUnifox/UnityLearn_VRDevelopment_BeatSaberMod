// Decompiled with JetBrains decompiler
// Type: GameLiftNetworkPlayerModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using BGNet.Core.GameLift;
using System.Collections.Generic;
using Zenject;

public class GameLiftNetworkPlayerModel : NetworkPlayerModel<GameLiftConnectionManager>
{
  [Inject]
  protected readonly IGameLiftPlayerSessionProvider _gameLiftPlayerSessionProvider;
  protected readonly GameLiftConnectionManager.ConnectToServerParams _cachedConnectToServerParams = new GameLiftConnectionManager.ConnectToServerParams();
  protected readonly GameLiftConnectionManager.StartClientParams _cachedStartClientParams = new GameLiftConnectionManager.StartClientParams();

  public override string secret => this.connectionManager?.secret;

  public override string code => this.connectionManager?.code;

  public override string partyOwnerId => (string) null;

  public override GameplayServerConfiguration configuration
  {
    get
    {
      GameLiftConnectionManager connectionManager = this.connectionManager;
      return connectionManager == null ? base.configuration : connectionManager.configuration;
    }
  }

  public override BeatmapLevelSelectionMask selectionMask
  {
    get
    {
      GameLiftConnectionManager connectionManager = this.connectionManager;
      return connectionManager == null ? base.selectionMask : connectionManager.selectionMask;
    }
  }

  protected override void Update()
  {
    base.Update();
    this._gameLiftPlayerSessionProvider.PollUpdate();
  }

  protected override void RefreshPublicServers(
    BeatmapLevelSelectionMask localSelectionMask,
    GameplayServerConfiguration localConfiguration,
    System.Action<IReadOnlyList<PublicServerInfo>> onSuccess,
    System.Action<ConnectionFailedReason> onFailure)
  {
    this.connectionManager?.GetPublicServers(onSuccess, onFailure, localSelectionMask, localConfiguration);
  }

  protected override IConnectionInitParams<GameLiftConnectionManager> GetConnectToServerParams(
    BeatmapLevelSelectionMask selectionMask,
    GameplayServerConfiguration configuration,
    string secret = null,
    string code = null)
  {
    this._cachedConnectToServerParams.gameLiftPlayerSessionProvider = this._gameLiftPlayerSessionProvider;
    this._cachedConnectToServerParams.authenticationTokenProviderTask = this.authenticationTokenProviderTask;
    this._cachedConnectToServerParams.selectionMask = selectionMask;
    this._cachedConnectToServerParams.configuration = configuration;
    this._cachedConnectToServerParams.secret = secret;
    this._cachedConnectToServerParams.code = code;
    return (IConnectionInitParams<GameLiftConnectionManager>) this._cachedConnectToServerParams;
  }

  protected override IConnectionInitParams<GameLiftConnectionManager> GetStartClientParams(
    BeatmapLevelSelectionMask selectionMask,
    GameplayServerConfiguration configuration)
  {
    this._cachedStartClientParams.gameLiftPlayerSessionProvider = this._gameLiftPlayerSessionProvider;
    this._cachedStartClientParams.authenticationTokenProviderTask = this.authenticationTokenProviderTask;
    this._cachedStartClientParams.selectionMask = selectionMask;
    this._cachedStartClientParams.configuration = configuration;
    return (IConnectionInitParams<GameLiftConnectionManager>) this._cachedStartClientParams;
  }
}
