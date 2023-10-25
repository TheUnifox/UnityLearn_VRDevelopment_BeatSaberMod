// Decompiled with JetBrains decompiler
// Type: MultiplayerScoreRingManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MultiplayerScoreRingManager : MonoBehaviour
{
  [SerializeField]
  protected float _delayBetweenScoreUpdates = 0.05f;
  [SerializeField]
  protected float _centerDistanceOffset = -0.5f;
  [Inject]
  protected readonly MultiplayerController _multiplayerController;
  [Inject]
  protected readonly MultiplayerPlayersManager _multiplayerPlayersManager;
  [Inject]
  protected readonly IMultiplayerSessionManager _multiplayerSessionManager;
  [Inject]
  protected readonly BeatmapObjectSpawnCenter _spawnCenter;
  [Inject]
  protected readonly MultiplayerLayoutProvider _layoutProvider;
  [Inject]
  protected readonly MultiplayerScoreProvider _scoreProvider;
  [Inject]
  protected readonly MultiplayerScoreRingItem.Pool _scoreRingItemPool;
  protected readonly Dictionary<string, MultiplayerScoreRingItem> _scoreRingItems = new Dictionary<string, MultiplayerScoreRingItem>();
  protected List<IConnectedPlayer> _allActivePlayers;
  protected int _currentlyScoreUpdateIndex;
  protected float _timeSinceLastScoreUpdate;
  protected MultiplayerScoreRingItem _firstPlayerItem;
  protected bool _spawnCenterDistanceFound;
  protected bool _playersSpawned;

  public virtual void Start()
  {
    this.enabled = false;
    if (this._spawnCenter.spawnCenterDistanceWasFound)
      this.HandleSpawnCenterDistanceWasFound(this._spawnCenter.spawnCenterDistance);
    else
      this._spawnCenter.spawnCenterDistanceWasFoundEvent += new System.Action<float>(this.HandleSpawnCenterDistanceWasFound);
    if (this._multiplayerPlayersManager.playerSpawningFinished)
      this.HandlePlayerSpawningDidFinish();
    else
      this._multiplayerPlayersManager.playerSpawningDidFinishEvent += new System.Action(this.HandlePlayerSpawningDidFinish);
    this._multiplayerController.stateChangedEvent += new System.Action<MultiplayerController.State>(this.HandleStateChanged);
    this.HandleStateChanged(this._multiplayerController.state);
  }

  public virtual void OnDestroy()
  {
    if ((UnityEngine.Object) this._spawnCenter != (UnityEngine.Object) null)
      this._spawnCenter.spawnCenterDistanceWasFoundEvent -= new System.Action<float>(this.HandleSpawnCenterDistanceWasFound);
    if ((UnityEngine.Object) this._multiplayerPlayersManager != (UnityEngine.Object) null)
      this._multiplayerPlayersManager.playerSpawningDidFinishEvent -= new System.Action(this.HandlePlayerSpawningDidFinish);
    if (this._multiplayerSessionManager != null)
    {
      this._multiplayerSessionManager.playerDisconnectedEvent -= new System.Action<IConnectedPlayer>(this.HandlePlayerDisconnected);
      this._multiplayerSessionManager.playerStateChangedEvent -= new System.Action<IConnectedPlayer>(this.HandlePlayerStateChanged);
    }
    if (!((UnityEngine.Object) this._multiplayerController != (UnityEngine.Object) null))
      return;
    this._multiplayerController.stateChangedEvent -= new System.Action<MultiplayerController.State>(this.HandleStateChanged);
  }

  public virtual void Update()
  {
    if (this._allActivePlayers == null || this._allActivePlayers.Count == 0)
      return;
    this._timeSinceLastScoreUpdate += Time.deltaTime;
    if ((double) this._timeSinceLastScoreUpdate <= (double) this._delayBetweenScoreUpdates)
      return;
    ++this._currentlyScoreUpdateIndex;
    if (this._currentlyScoreUpdateIndex >= this._allActivePlayers.Count)
      this._currentlyScoreUpdateIndex = 0;
    this.UpdateScore(this._allActivePlayers[this._currentlyScoreUpdateIndex]);
    this._timeSinceLastScoreUpdate -= this._delayBetweenScoreUpdates;
  }

  public virtual void UpdateScore(IConnectedPlayer playerToUpdate)
  {
    MultiplayerScoreProvider.RankedPlayer data;
    bool score = this._scoreProvider.TryGetScore(playerToUpdate.userId, out data);
    MultiplayerScoreRingItem multiplayerScoreRingItem;
    if (!this._scoreRingItems.TryGetValue(playerToUpdate.userId, out multiplayerScoreRingItem))
      return;
    if (!score || data.isFailed)
      multiplayerScoreRingItem.SetScore("X");
    else
      multiplayerScoreRingItem.SetScore(data.score.ToString());
  }

  public virtual void AnimateColorsForPlayer(
    string userId,
    Color nameColor,
    Color scoreColor,
    float duration,
    EaseType easeType)
  {
    if (this._layoutProvider.layout == MultiplayerPlayerLayout.Duel)
      return;
    MultiplayerScoreRingItem multiplayerScoreRingItem;
    if (!this._scoreRingItems.TryGetValue(userId, out multiplayerScoreRingItem))
      Debug.LogWarning((object) ("Unable to find text object for userId \"" + userId + "\""));
    else
      multiplayerScoreRingItem.AnimateColors(nameColor, scoreColor, duration, easeType);
  }

  public virtual MultiplayerScoreRingItem GetScoreRingItem(string userId)
  {
    if (this._layoutProvider.layout == MultiplayerPlayerLayout.Duel)
      return (MultiplayerScoreRingItem) null;
    MultiplayerScoreRingItem scoreRingItem;
    if (this._scoreRingItems.TryGetValue(userId, out scoreRingItem))
      return scoreRingItem;
    Debug.LogWarning((object) ("Unable to find text object for userId \"" + userId + "\""));
    return (MultiplayerScoreRingItem) null;
  }

  public virtual GameObject[] GetScoreRingItems()
  {
    GameObject[] scoreRingItems = new GameObject[this._scoreRingItems.Count];
    int index = 0;
    foreach (KeyValuePair<string, MultiplayerScoreRingItem> scoreRingItem in this._scoreRingItems)
    {
      scoreRingItems[index] = scoreRingItem.Value.gameObject;
      ++index;
    }
    return scoreRingItems;
  }

  public virtual void AnimateColorsForAllPlayers(
    Color nameColor,
    Color scoreColor,
    float duration,
    EaseType easeType)
  {
    foreach (string key in this._scoreRingItems.Keys)
      this.AnimateColorsForPlayer(key, nameColor, scoreColor, duration, easeType);
  }

  public virtual void SetPlayerToFailedState(IConnectedPlayer player)
  {
    MultiplayerScoreRingItem multiplayerScoreRingItem;
    if (!this._scoreRingItems.TryGetValue(player.userId, out multiplayerScoreRingItem))
      return;
    multiplayerScoreRingItem.SetScore("X");
  }

  public virtual void TrySpawnTexts()
  {
    if (!this._spawnCenterDistanceFound || !this._playersSpawned)
      return;
    this.SpawnTexts();
  }

  public virtual void SpawnTexts()
  {
    float spawnCenterDistance = this._spawnCenter.spawnCenterDistance;
    this._allActivePlayers = new List<IConnectedPlayer>((IEnumerable<IConnectedPlayer>) this._multiplayerPlayersManager.allActiveAtGameStartPlayers);
    Vector3 vector3_1 = new Vector3(0.0f, this.transform.position.y, spawnCenterDistance);
    foreach (IConnectedPlayer allActivePlayer in this._allActivePlayers)
    {
      MultiplayerScoreRingItem multiplayerScoreRingItem = this._scoreRingItemPool.Spawn();
      float centerFacingRotation;
      if (!this._multiplayerPlayersManager.TryGetConnectedCenterFacingRotation(allActivePlayer.userId, out centerFacingRotation))
      {
        Debug.LogError((object) ("Unable to find rotation for userId \"" + allActivePlayer.userId + "\""));
      }
      else
      {
        Quaternion rotation = Quaternion.Euler(0.0f, centerFacingRotation + 180f, 0.0f);
        Vector3 vector3_2 = rotation * new Vector3(0.0f, 0.0f, this._centerDistanceOffset);
        multiplayerScoreRingItem.SetPositionAndRotation(vector3_1 + vector3_2, rotation);
        multiplayerScoreRingItem.AnimateColors(Color.white.ColorWithAlpha(0.0f), Color.white.ColorWithAlpha(0.0f), 0.0f, EaseType.Linear);
        multiplayerScoreRingItem.SetName(allActivePlayer.userName);
        multiplayerScoreRingItem.SetScore("0");
        this._scoreRingItems[allActivePlayer.userId] = multiplayerScoreRingItem;
      }
    }
  }

  public virtual void HandleStateChanged(MultiplayerController.State state)
  {
    if (state == MultiplayerController.State.Gameplay)
    {
      if (this._layoutProvider.layout == MultiplayerPlayerLayout.Duel)
      {
        this.enabled = false;
      }
      else
      {
        this.enabled = true;
        this._multiplayerSessionManager.playerDisconnectedEvent += new System.Action<IConnectedPlayer>(this.HandlePlayerDisconnected);
        this._multiplayerSessionManager.playerStateChangedEvent += new System.Action<IConnectedPlayer>(this.HandlePlayerStateChanged);
        foreach (IConnectedPlayer allActivePlayer in this._allActivePlayers)
        {
          this.AnimateColorsForPlayer(allActivePlayer.userId, Color.white.ColorWithAlpha(1f), Color.white.ColorWithAlpha(0.5f), 0.0f, EaseType.InQuad);
          this.UpdateScore(allActivePlayer);
        }
      }
    }
    else
    {
      this.enabled = false;
      this._multiplayerSessionManager.playerDisconnectedEvent -= new System.Action<IConnectedPlayer>(this.HandlePlayerDisconnected);
      this._multiplayerSessionManager.playerStateChangedEvent -= new System.Action<IConnectedPlayer>(this.HandlePlayerStateChanged);
    }
  }

  public virtual void HandleSpawnCenterDistanceWasFound(float spawnCenterDistance)
  {
    this._spawnCenterDistanceFound = true;
    this.TrySpawnTexts();
  }

  public virtual void HandlePlayerSpawningDidFinish()
  {
    this._playersSpawned = true;
    this.TrySpawnTexts();
  }

  public virtual void HandlePlayerStateChanged(IConnectedPlayer player)
  {
    if (!player.IsFailed())
      return;
    this.SetPlayerToFailedState(player);
  }

  public virtual void HandlePlayerDisconnected(IConnectedPlayer player) => this.SetPlayerToFailedState(player);
}
