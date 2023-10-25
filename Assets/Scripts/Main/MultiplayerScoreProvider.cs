// Decompiled with JetBrains decompiler
// Type: MultiplayerScoreProvider
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

public class MultiplayerScoreProvider : MonoBehaviour
{
  [Inject]
  protected readonly IScoreSyncStateManager _scoreSyncStateManager;
  [CompilerGenerated]
  protected MultiplayerScoreProvider.RankedPlayer m_CfirstPlayer;
  protected float _sharedOffsetSyncTime;
  protected readonly List<MultiplayerScoreProvider.RankedPlayer> _rankedPlayers = new List<MultiplayerScoreProvider.RankedPlayer>();
  protected readonly Dictionary<string, MultiplayerScoreProvider.RankedPlayer> _players = new Dictionary<string, MultiplayerScoreProvider.RankedPlayer>();

  public bool scoresAvailable => this.firstPlayer != null && this.firstPlayer.score > 0;

  public MultiplayerScoreProvider.RankedPlayer firstPlayer
  {
    get => this.m_CfirstPlayer;
    private set => this.m_CfirstPlayer = value;
  }

  public event System.Action<MultiplayerScoreProvider.RankedPlayer> firstPlayerDidChangeEvent;

  public IReadOnlyList<MultiplayerScoreProvider.RankedPlayer> rankedPlayers => (IReadOnlyList<MultiplayerScoreProvider.RankedPlayer>) this._rankedPlayers;

  public virtual void Update()
  {
    for (int i = 0; i < this._scoreSyncStateManager.connectedPlayerCount; ++i)
    {
      MultiplayerSyncState<StandardScoreSyncState, StandardScoreSyncState.Score, int> syncState = this._scoreSyncStateManager.GetSyncState(i);
      MultiplayerScoreProvider.RankedPlayer rankedPlayer;
      if (!this._players.TryGetValue(syncState.player.userId, out rankedPlayer) || !rankedPlayer.isConnected || !rankedPlayer.wasActiveAtLevelStart)
      {
        if (rankedPlayer != null)
          this._rankedPlayers.Remove(rankedPlayer);
        if (syncState.player.WasActiveAtLevelStart() && !this._players.ContainsKey(syncState.player.userId))
        {
          rankedPlayer = new MultiplayerScoreProvider.RankedPlayer(syncState, this);
          this._rankedPlayers.Add(rankedPlayer);
          this._players[syncState.player.userId] = rankedPlayer;
        }
      }
    }
    this._sharedOffsetSyncTime = this._scoreSyncStateManager.syncTime;
    for (int index = 0; index < this._rankedPlayers.Count; ++index)
    {
      if (this._rankedPlayers[index].isConnected)
        this._sharedOffsetSyncTime = Mathf.Min(this._sharedOffsetSyncTime, this._rankedPlayers[index].offsetSyncTime);
    }
    if (this._rankedPlayers.Count <= 0)
      return;
    this._rankedPlayers.Sort();
    MultiplayerScoreProvider.RankedPlayer rankedPlayer1 = this._rankedPlayers.FirstOrDefault<MultiplayerScoreProvider.RankedPlayer>((Func<MultiplayerScoreProvider.RankedPlayer, bool>) (p => !p.isFailed));
    if (rankedPlayer1 == null)
    {
      this.firstPlayer = (MultiplayerScoreProvider.RankedPlayer) null;
      System.Action<MultiplayerScoreProvider.RankedPlayer> playerDidChangeEvent = this.firstPlayerDidChangeEvent;
      if (playerDidChangeEvent == null)
        return;
      playerDidChangeEvent((MultiplayerScoreProvider.RankedPlayer) null);
    }
    else
    {
      if (rankedPlayer1.score <= 0 || rankedPlayer1 == this.firstPlayer)
        return;
      this.firstPlayer = rankedPlayer1;
      System.Action<MultiplayerScoreProvider.RankedPlayer> playerDidChangeEvent = this.firstPlayerDidChangeEvent;
      if (playerDidChangeEvent == null)
        return;
      playerDidChangeEvent(rankedPlayer1);
    }
  }

  public virtual bool TryGetScore(string userId, out MultiplayerScoreProvider.RankedPlayer data) => this._players.TryGetValue(userId, out data);

  public virtual int GetPositionOfPlayer(string userId) => this._rankedPlayers.FindIndex((Predicate<MultiplayerScoreProvider.RankedPlayer>) (p => p.userId == userId)) + 1;

  public class RankedPlayer : IComparable<MultiplayerScoreProvider.RankedPlayer>
  {
    protected readonly MultiplayerSyncState<StandardScoreSyncState, StandardScoreSyncState.Score, int> _multiplayerSyncState;
    protected readonly MultiplayerScoreProvider _scoreSyncManager;

    public float offsetSyncTime => this._multiplayerSyncState.player.offsetSyncTime;

    public float lastScoreTime => this._multiplayerSyncState.GetLatestTime();

    public int score => this._multiplayerSyncState.GetState(StandardScoreSyncState.Score.ModifiedScore, this._scoreSyncManager._sharedOffsetSyncTime);

    public bool isConnected => this._multiplayerSyncState.player.isConnected;

    public bool isActiveOrFinished => this._multiplayerSyncState.player.IsActiveOrFinished();

    public bool isFailed => this._multiplayerSyncState.player.IsFailed();

    public bool wasActiveAtLevelStart => this._multiplayerSyncState.player.WasActiveAtLevelStart();

    public bool isMe => this._multiplayerSyncState.player.isMe;

    public string userId => this._multiplayerSyncState.player.userId;

    public string userName => this._multiplayerSyncState.player.userName;

    public RankedPlayer(
      MultiplayerSyncState<StandardScoreSyncState, StandardScoreSyncState.Score, int> multiplayerSyncState,
      MultiplayerScoreProvider scoreSyncManager)
    {
      this._multiplayerSyncState = multiplayerSyncState;
      this._scoreSyncManager = scoreSyncManager;
    }

    public virtual int CompareTo(MultiplayerScoreProvider.RankedPlayer other)
    {
      if (other == null || !this.isFailed && other.isFailed)
        return -1;
      return this.isFailed && !other.isFailed ? 1 : -this.score.CompareTo(other.score);
    }
  }
}
