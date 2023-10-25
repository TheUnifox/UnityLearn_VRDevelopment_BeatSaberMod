// Decompiled with JetBrains decompiler
// Type: ILobbyGameStateController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Threading;
using System.Threading.Tasks;

public interface ILobbyGameStateController
{
  event System.Action<ILevelGameplaySetupData> selectedLevelGameplaySetupDataChangedEvent;

  event System.Action<ILevelGameplaySetupData> gameStartedEvent;

  event System.Action gameStartCancelledEvent;

  event System.Action countdownStartedEvent;

  event System.Action countdownCancelledEvent;

  event System.Action songStillDownloadingEvent;

  event System.Action startTimeChangedEvent;

  event System.Action<MultiplayerLevelScenesTransitionSetupDataSO, MultiplayerResultsData> levelFinishedEvent;

  event System.Action<DisconnectedReason> levelDidGetDisconnectedEvent;

  event System.Action lobbyDisconnectedEvent;

  event System.Action beforeSceneSwitchCallbackEvent;

  event System.Action<MultiplayerLobbyState> lobbyStateChangedEvent;

  event System.Action<CannotStartGameReason> startButtonEnabledEvent;

  event System.Action<PlayersMissingEntitlementsNetSerializable> playerMissingEntitlementsChangedEvent;

  MultiplayerLobbyState state { get; set; }

  CannotStartGameReason cannotStartGameReason { get; }

  ILevelGameplaySetupData selectedLevelGameplaySetupData { get; }

  bool countdownStarted { get; }

  float countdownEndTime { get; }

  bool levelStartInitiated { get; }

  float startTime { get; }

  float predictedCountdownEndTime { get; }

  bool isDisconnected { get; }

  DisconnectedReason disconnectedReason { get; }

  void Activate();

  void Deactivate();

  void StartListeningToGameStart();

  void GetCurrentLevelIfGameStarted();

  void ClearDisconnectedState();

  Task GetGameStateAndConfigurationAsync(CancellationToken cancellationToken);

  void PredictCountdownEndTime();

  bool IsCloseToStartGame();
}
