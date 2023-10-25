// Decompiled with JetBrains decompiler
// Type: MockPlayerGamePoseGeneratorMirror
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;

public class MockPlayerGamePoseGeneratorMirror : MockPlayerGamePoseGenerator
{
  protected readonly NodePoseSyncStateManager _nodePoseSyncStateManager;
  protected IConnectedPlayer _mirroredPlayer;
  protected System.Action _onSongFinished;

  public MockPlayerGamePoseGeneratorMirror(
    IMultiplayerSessionManager multiplayerSessionManager,
    IGameplayRpcManager gameplayRpcManager,
    bool leftHanded,
    NodePoseSyncStateManager nodePoseSyncStateManager)
    : base(multiplayerSessionManager, gameplayRpcManager, leftHanded)
  {
    this._nodePoseSyncStateManager = nodePoseSyncStateManager;
    gameplayRpcManager.noteWasSpawnedEvent += new Action<string, float, float, NoteSpawnInfoNetSerializable>(this.HandleNoteWasSpawned);
    gameplayRpcManager.obstacleWasSpawnedEvent += new Action<string, float, float, ObstacleSpawnInfoNetSerializable>(this.HandleObstacleWasSpawned);
    gameplayRpcManager.sliderWasSpawnedEvent += new Action<string, float, float, SliderSpawnInfoNetSerializable>(this.HandleSliderWasSpawned);
    gameplayRpcManager.noteWasCutEvent += new Action<string, float, float, NoteCutInfoNetSerializable>(this.HandleNoteWasCut);
    gameplayRpcManager.noteWasMissedEvent += new Action<string, float, float, NoteMissInfoNetSerializable>(this.HandleNoteWasMissed);
    gameplayRpcManager.levelFinishedEvent += new System.Action<string, MultiplayerLevelCompletionResults>(this.HandleLevelFinished);
    multiplayerSessionManager.RegisterCallback<StandardScoreSyncStateNetSerializable>(MultiplayerSessionManager.MessageType.ScoreSyncState, new System.Action<StandardScoreSyncStateNetSerializable, IConnectedPlayer>(this.HandleScoreSyncStateUpdate), new Func<StandardScoreSyncStateNetSerializable>(StandardScoreSyncStateNetSerializable.pool.Obtain));
  }

  public override void Dispose()
  {
    base.Dispose();
    if (this.gameplayRpcManager != null)
    {
      this.gameplayRpcManager.noteWasSpawnedEvent -= new Action<string, float, float, NoteSpawnInfoNetSerializable>(this.HandleNoteWasSpawned);
      this.gameplayRpcManager.obstacleWasSpawnedEvent -= new Action<string, float, float, ObstacleSpawnInfoNetSerializable>(this.HandleObstacleWasSpawned);
      this.gameplayRpcManager.sliderWasSpawnedEvent -= new Action<string, float, float, SliderSpawnInfoNetSerializable>(this.HandleSliderWasSpawned);
      this.gameplayRpcManager.noteWasCutEvent -= new Action<string, float, float, NoteCutInfoNetSerializable>(this.HandleNoteWasCut);
      this.gameplayRpcManager.noteWasMissedEvent -= new Action<string, float, float, NoteMissInfoNetSerializable>(this.HandleNoteWasMissed);
    }
    this.multiplayerSessionManager?.UnregisterCallback<StandardScoreSyncStateNetSerializable>(MultiplayerSessionManager.MessageType.ScoreSyncState);
  }

  public override void Init(
    float introStartTime,
    MockBeatmapData beatmapData,
    GameplayModifiers gameplayModifiers,
    System.Action onSongFinished)
  {
    this._onSongFinished = onSongFinished;
  }

  public override void Tick()
  {
    LocalMultiplayerSyncState<NodePoseSyncState, NodePoseSyncState.NodePose, PoseSerializable> localState = this._nodePoseSyncStateManager.localState;
    if (localState == null)
      return;
    this.mockNodePoseSyncStateSender.SendPose(localState.GetState(NodePoseSyncState.NodePose.Head), localState.GetState(NodePoseSyncState.NodePose.LeftController), localState.GetState(NodePoseSyncState.NodePose.RightController));
  }

  public virtual void FindPlayerToMirror()
  {
    if (this._mirroredPlayer != null)
      return;
    foreach (IConnectedPlayer connectedPlayer in (IEnumerable<IConnectedPlayer>) this.multiplayerSessionManager.connectedPlayers)
    {
      if (connectedPlayer is MockPlayer mockPlayer && mockPlayer.isMe)
      {
        this._mirroredPlayer = (IConnectedPlayer) mockPlayer;
        break;
      }
    }
  }

  public virtual void HandleNoteWasSpawned(
    string userId,
    float syncTime,
    float songTime,
    NoteSpawnInfoNetSerializable noteSpawnInfoNetSerializable)
  {
    this.FindPlayerToMirror();
    if (this._mirroredPlayer == null || !(userId == this._mirroredPlayer.userId))
      return;
    noteSpawnInfoNetSerializable.Retain();
    this.gameplayRpcManager.NoteSpawned(songTime, noteSpawnInfoNetSerializable);
  }

  public virtual void HandleObstacleWasSpawned(
    string userId,
    float syncTime,
    float songTime,
    ObstacleSpawnInfoNetSerializable obstacleSpawnInfoNetSerializable)
  {
    this.FindPlayerToMirror();
    if (this._mirroredPlayer == null || !(userId == this._mirroredPlayer.userId))
      return;
    obstacleSpawnInfoNetSerializable.Retain();
    this.gameplayRpcManager.ObstacleSpawned(songTime, obstacleSpawnInfoNetSerializable);
  }

  public virtual void HandleSliderWasSpawned(
    string userId,
    float syncTime,
    float songTime,
    SliderSpawnInfoNetSerializable sliderSpawnInfoNetSerializable)
  {
    this.FindPlayerToMirror();
    if (this._mirroredPlayer == null || !(userId == this._mirroredPlayer.userId))
      return;
    sliderSpawnInfoNetSerializable.Retain();
    this.gameplayRpcManager.SliderSpawned(songTime, sliderSpawnInfoNetSerializable);
  }

  public virtual void HandleNoteWasMissed(
    string userId,
    float syncTime,
    float songTime,
    NoteMissInfoNetSerializable noteMissInfo)
  {
    this.FindPlayerToMirror();
    if (this._mirroredPlayer == null || !(userId == this._mirroredPlayer.userId))
      return;
    noteMissInfo.Retain();
    this.gameplayRpcManager.NoteMissed(songTime, noteMissInfo);
  }

  public virtual void HandleNoteWasCut(
    string userId,
    float syncTime,
    float songTime,
    NoteCutInfoNetSerializable noteCutInfo)
  {
    this.FindPlayerToMirror();
    if (this._mirroredPlayer == null || !(userId == this._mirroredPlayer.userId))
      return;
    noteCutInfo.Retain();
    this.gameplayRpcManager.NoteCut(songTime, noteCutInfo);
  }

  public virtual void HandleScoreSyncStateUpdate(
    StandardScoreSyncStateNetSerializable nodePose,
    IConnectedPlayer player)
  {
    this.FindPlayerToMirror();
    if (this._mirroredPlayer == null || !(player.userId == this._mirroredPlayer.userId))
      return;
    this.multiplayerSessionManager.SendUnreliable<StandardScoreSyncStateNetSerializable>(nodePose);
  }

  public virtual void HandleLevelFinished(string userId, MultiplayerLevelCompletionResults results)
  {
    this.FindPlayerToMirror();
    if (this._mirroredPlayer == null || !(userId == this._mirroredPlayer.userId))
      return;
    this.gameplayRpcManager.LevelFinished(results);
    System.Action onSongFinished = this._onSongFinished;
    if (onSongFinished == null)
      return;
    onSongFinished();
  }
}
