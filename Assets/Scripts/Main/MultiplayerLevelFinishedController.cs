// Decompiled with JetBrains decompiler
// Type: MultiplayerLevelFinishedController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MultiplayerLevelFinishedController : MonoBehaviour
{
  [Inject]
  protected readonly IMultiplayerLevelEndActionsPublisher _levelEndActionsPublisher;
  [Inject]
  protected readonly IGameplayRpcManager _rpcManager;
  [Inject]
  protected readonly IMultiplayerSessionManager _multiplayerSessionManager;
  protected readonly Dictionary<string, MultiplayerLevelCompletionResults> _otherPlayersCompletionResults = new Dictionary<string, MultiplayerLevelCompletionResults>();
  protected MultiplayerLevelCompletionResults _localPlayerResults;
  protected bool _gameFinishReported;
  protected float _sceneLoadTime;
  protected const float kMinSceneDuration = 2f;

  public event System.Action<MultiplayerLevelCompletionResults, Dictionary<string, MultiplayerLevelCompletionResults>> allResultsCollectedEvent;

  public bool gameResultsReady => this._gameFinishReported;

  public Dictionary<string, MultiplayerLevelCompletionResults> otherPlayersCompletionResults => this._otherPlayersCompletionResults;

  public MultiplayerLevelCompletionResults localPlayerResults => this._localPlayerResults;

  public virtual void Start()
  {
    this._sceneLoadTime = Time.time;
    this._levelEndActionsPublisher.playerDidFinishEvent += new System.Action<MultiplayerLevelCompletionResults>(this.HandlePlayerDidFinish);
    this._levelEndActionsPublisher.playerNetworkDidFailedEvent += new System.Action<MultiplayerLevelCompletionResults>(this.HandlePlayerNetworkDidFailed);
    this._rpcManager.levelFinishedEvent += new System.Action<string, MultiplayerLevelCompletionResults>(this.HandleRpcLevelFinished);
  }

  public virtual void OnDestroy()
  {
    if (this._levelEndActionsPublisher != null)
    {
      this._levelEndActionsPublisher.playerDidFinishEvent -= new System.Action<MultiplayerLevelCompletionResults>(this.HandlePlayerDidFinish);
      this._levelEndActionsPublisher.playerNetworkDidFailedEvent -= new System.Action<MultiplayerLevelCompletionResults>(this.HandlePlayerNetworkDidFailed);
    }
    if (this._rpcManager == null)
      return;
    this._rpcManager.levelFinishedEvent -= new System.Action<string, MultiplayerLevelCompletionResults>(this.HandleRpcLevelFinished);
  }

  public virtual IEnumerator StartLevelFinished(
    MultiplayerLevelCompletionResults localPlayerResults)
  {
    bool flag1 = localPlayerResults == null;
    if (localPlayerResults == null)
      localPlayerResults = new MultiplayerLevelCompletionResults(MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndState.NotStarted, MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndReason.WasInactive, (LevelCompletionResults) null);
    this._localPlayerResults = localPlayerResults;
    this._rpcManager.LevelFinished(localPlayerResults);
    float waitStartTime = Time.timeSinceLevelLoad;
    bool isPlayingAlone = this._multiplayerSessionManager.connectedPlayerCount == 0;
    float resultsTimeoutLength = !isPlayingAlone ? (flag1 || localPlayerResults.playerLevelEndReason != MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndReason.Cleared ? float.MaxValue : 10f) : 0.0f;
    while ((double) Time.timeSinceLevelLoad - (double) waitStartTime < (double) resultsTimeoutLength)
    {
      bool flag2 = true;
      for (int index = 0; index < this._multiplayerSessionManager.connectedPlayerCount; ++index)
      {
        IConnectedPlayer connectedPlayer = this._multiplayerSessionManager.GetConnectedPlayer(index);
        if (connectedPlayer.IsActiveOrFinished() && !this._otherPlayersCompletionResults.ContainsKey(connectedPlayer.userId))
        {
          flag2 = false;
          break;
        }
      }
      if (!flag2)
        yield return (object) new WaitForSeconds(0.5f);
      else
        break;
    }
    float seconds = (float) (2.0 - ((double) Time.time - (double) this._sceneLoadTime));
    if ((double) seconds > 0.0 && !isPlayingAlone)
      yield return (object) new WaitForSeconds(seconds);
    this._gameFinishReported = true;
    System.Action<MultiplayerLevelCompletionResults, Dictionary<string, MultiplayerLevelCompletionResults>> resultsCollectedEvent = this.allResultsCollectedEvent;
    if (resultsCollectedEvent != null)
      resultsCollectedEvent(localPlayerResults, this._otherPlayersCompletionResults);
  }

  public virtual void HandlePlayerDidFinish(
    MultiplayerLevelCompletionResults levelCompletionResults)
  {
    this.StartCoroutine(this.StartLevelFinished(levelCompletionResults));
  }

  public virtual void HandlePlayerNetworkDidFailed(
    MultiplayerLevelCompletionResults levelCompletionResults)
  {
    if (this._gameFinishReported)
      return;
    this._gameFinishReported = true;
    this._localPlayerResults = levelCompletionResults;
    System.Action<MultiplayerLevelCompletionResults, Dictionary<string, MultiplayerLevelCompletionResults>> resultsCollectedEvent = this.allResultsCollectedEvent;
    if (resultsCollectedEvent == null)
      return;
    resultsCollectedEvent(levelCompletionResults, this._otherPlayersCompletionResults);
  }

  public virtual void HandleRpcLevelFinished(
    string userId,
    MultiplayerLevelCompletionResults results)
  {
    this._otherPlayersCompletionResults[userId] = results;
  }
}
