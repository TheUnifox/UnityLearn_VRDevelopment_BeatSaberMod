// Decompiled with JetBrains decompiler
// Type: LobbyPlayersDataModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Zenject;

public class LobbyPlayersDataModel : 
  ILobbyPlayersDataModel,
  IReadOnlyDictionary<string, ILobbyPlayerData>,
  IEnumerable<KeyValuePair<string, ILobbyPlayerData>>,
  IEnumerable,
  IReadOnlyCollection<KeyValuePair<string, ILobbyPlayerData>>,
  IDisposable
{
  [Inject]
  protected readonly IMenuRpcManager _menuRpcManager;
  [Inject]
  protected readonly IMultiplayerSessionManager _multiplayerSessionManager;
  [Inject]
  protected readonly BeatmapLevelsModel _beatmapLevelsModel;
  [Inject]
  protected readonly BeatmapCharacteristicCollectionSO _beatmapCharacteristicCollection;
  [Inject]
  protected readonly AdditionalContentModel _additionalContentModel;
  [Inject]
  protected readonly LobbyPlayerPermissionsModel _lobbyPlayerPermissionsModel;
  protected readonly LobbyPlayerData _emptyLobbyPlayerData = new LobbyPlayerData();
  protected readonly Dictionary<string, LobbyPlayerData> _playersData = new Dictionary<string, LobbyPlayerData>();
  protected readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
  [CompilerGenerated]
  protected string m_CpartyOwnerId = string.Empty;

  public string localUserId => this._multiplayerSessionManager.localPlayer?.userId ?? string.Empty;

  public string partyOwnerId
  {
    get => this.m_CpartyOwnerId;
    private set => this.m_CpartyOwnerId = value;
  }

  public event System.Action<string> didChangeEvent;

  public ILobbyPlayerData this[string userId]
  {
    get
    {
      LobbyPlayerData lobbyPlayerData;
      return !this._playersData.TryGetValue(userId, out lobbyPlayerData) ? (ILobbyPlayerData) this._emptyLobbyPlayerData : (ILobbyPlayerData) lobbyPlayerData;
    }
  }

  public virtual LobbyPlayerData GetOrCreateLobbyPlayerDataModel(
    string userId,
    out bool alreadyExists)
  {
    LobbyPlayerData lobbyPlayerDataModel;
    alreadyExists = this._playersData.TryGetValue(userId, out lobbyPlayerDataModel);
    if (!alreadyExists)
      this._playersData[userId] = lobbyPlayerDataModel = new LobbyPlayerData();
    return lobbyPlayerDataModel;
  }

  public virtual void SetPlayerBeatmapLevel(string userId, PreviewDifficultyBeatmap beatmapLevel)
  {
    bool alreadyExists;
    LobbyPlayerData lobbyPlayerDataModel = this.GetOrCreateLobbyPlayerDataModel(userId, out alreadyExists);
    if (alreadyExists && lobbyPlayerDataModel.beatmapLevel == beatmapLevel)
      return;
    lobbyPlayerDataModel.SetBeatmapLevel(beatmapLevel);
    this.NotifyModelChange(userId);
  }

  public virtual void SetPlayerGameplayModifiers(string userId, GameplayModifiers modifiers)
  {
    bool alreadyExists;
    LobbyPlayerData lobbyPlayerDataModel = this.GetOrCreateLobbyPlayerDataModel(userId, out alreadyExists);
    if (alreadyExists && lobbyPlayerDataModel.gameplayModifiers == modifiers)
      return;
    lobbyPlayerDataModel.SetGameplayModifiers(modifiers);
    this.NotifyModelChange(userId);
  }

  public virtual void SetPlayerIsActive(string userId, bool isActive, bool notifyChange = true)
  {
    bool alreadyExists;
    LobbyPlayerData lobbyPlayerDataModel = this.GetOrCreateLobbyPlayerDataModel(userId, out alreadyExists);
    if (alreadyExists && lobbyPlayerDataModel.isActive == isActive)
      return;
    lobbyPlayerDataModel.isActive = isActive;
    if (!notifyChange)
      return;
    this.NotifyModelChange(userId);
  }

  public virtual void SetPlayerIsReady(string userId, bool isReady, bool notifyChange = true)
  {
    bool alreadyExists;
    LobbyPlayerData lobbyPlayerDataModel = this.GetOrCreateLobbyPlayerDataModel(userId, out alreadyExists);
    if (alreadyExists && lobbyPlayerDataModel.isReady == isReady)
      return;
    lobbyPlayerDataModel.isReady = isReady;
    if (!notifyChange)
      return;
    this.NotifyModelChange(userId);
  }

  public virtual void SetPlayerIsInLobby(string userId, bool isInLobby, bool notifyChange = true)
  {
    bool alreadyExists;
    LobbyPlayerData lobbyPlayerDataModel = this.GetOrCreateLobbyPlayerDataModel(userId, out alreadyExists);
    if (alreadyExists && lobbyPlayerDataModel.isInLobby == isInLobby)
      return;
    lobbyPlayerDataModel.isInLobby = isInLobby;
    if (!notifyChange)
      return;
    this.NotifyModelChange(userId);
  }

  public virtual void SetPlayerIsPartyOwner(string userId, bool isPartyOwner, bool notifyChange = true)
  {
    bool alreadyExists;
    LobbyPlayerData lobbyPlayerDataModel = this.GetOrCreateLobbyPlayerDataModel(userId, out alreadyExists);
    if (alreadyExists && lobbyPlayerDataModel.isPartyOwner == isPartyOwner)
      return;
    this.partyOwnerId = userId;
    lobbyPlayerDataModel.isPartyOwner = isPartyOwner;
    if (!notifyChange)
      return;
    this.NotifyModelChange(userId);
  }

  public virtual void SetLocalPlayerBeatmapLevel(PreviewDifficultyBeatmap beatmapLevel)
  {
    if (beatmapLevel != (PreviewDifficultyBeatmap) null)
      this._menuRpcManager.RecommendBeatmap(beatmapLevel.ToIdentifier());
    else
      this._menuRpcManager.ClearRecommendedBeatmap();
    this.SetPlayerBeatmapLevel(this.localUserId, beatmapLevel);
  }

  public virtual void ClearLocalPlayerBeatmapLevel()
  {
    this._menuRpcManager.ClearRecommendedBeatmap();
    this.SetPlayerBeatmapLevel(this.localUserId, (PreviewDifficultyBeatmap) null);
  }

  public virtual void SetLocalPlayerGameplayModifiers(GameplayModifiers modifiers)
  {
    if (modifiers != null)
      this._menuRpcManager.RecommendGameplayModifiers(modifiers);
    else
      this._menuRpcManager.ClearRecommendedGameplayModifiers();
    this.SetPlayerGameplayModifiers(this.localUserId, modifiers);
  }

  public virtual void ClearLocalPlayerGameplayModifiers()
  {
    this._menuRpcManager.ClearRecommendedGameplayModifiers();
    this.SetPlayerGameplayModifiers(this.localUserId, (GameplayModifiers) null);
  }

  public virtual void SetLocalPlayerIsActive(bool isActive) => this.SetLocalPlayerIsActive(isActive, true);

  public virtual void SetLocalPlayerIsActive(bool isActive, bool notifyChange)
  {
    this._multiplayerSessionManager.SetLocalPlayerState("wants_to_play_next_level", isActive);
    this.SetPlayerIsActive(this.localUserId, isActive, notifyChange);
  }

  public virtual void SetLocalPlayerIsReady(bool isReady) => this.SetLocalPlayerIsReady(isReady, true);

  public virtual void SetLocalPlayerIsReady(bool isReady, bool notifyChange)
  {
    this._menuRpcManager.SetIsReady(isReady);
    this.SetPlayerIsReady(this.localUserId, isReady, notifyChange);
  }

  public virtual void SetLocalPlayerIsInLobby(bool isInLobby) => this.SetLocalPlayerIsInLobby(isInLobby, true);

  public virtual void SetLocalPlayerIsInLobby(bool isInLobby, bool notifyChange)
  {
    this._menuRpcManager.SetIsInLobby(isInLobby);
    this.SetPlayerIsInLobby(this.localUserId, isInLobby, notifyChange);
  }

  public virtual void RequestKickPlayer(string userId)
  {
    if (!this._lobbyPlayerPermissionsModel.hasKickVotePermission)
      return;
    this._menuRpcManager.RequestKickPlayer(userId);
  }

  public virtual void ClearData()
  {
    this.SetLocalPlayerIsActive(true, false);
    this.SetLocalPlayerIsReady(false, false);
    this.SetLocalPlayerIsInLobby(false, false);
    this._playersData.Clear();
  }

  public virtual void ClearRecommendations()
  {
    foreach (KeyValuePair<string, LobbyPlayerData> keyValuePair in this._playersData)
      keyValuePair.Value.ClearGameplaySetupData();
  }

  public virtual void Activate()
  {
    this._menuRpcManager.getRecommendedBeatmapEvent += new System.Action<string>(this.HandleMenuRpcManagerGetRecommendedBeatmap);
    this._menuRpcManager.recommendBeatmapEvent += new System.Action<string, BeatmapIdentifierNetSerializable>(this.HandleMenuRpcManagerRecommendBeatmap);
    this._menuRpcManager.clearRecommendedBeatmapEvent += new System.Action<string>(this.HandleMenuRpcManagerClearBeatmap);
    this._menuRpcManager.getRecommendedGameplayModifiersEvent += new System.Action<string>(this.HandleMenuRpcManagerGetRecommendedGameplayModifiers);
    this._menuRpcManager.recommendGameplayModifiersEvent += new System.Action<string, GameplayModifiers>(this.HandleMenuRpcManagerRecommendGameplayModifiers);
    this._menuRpcManager.clearRecommendedGameplayModifiersEvent += new System.Action<string>(this.HandleMenuRpcManagerClearRecommendedGameplayModifiers);
    this._menuRpcManager.getIsReadyEvent += new System.Action<string>(this.HandleMenuRpcManagerGetIsReady);
    this._menuRpcManager.setIsReadyEvent += new System.Action<string, bool>(this.HandleMenuRpcManagerSetIsReady);
    this._menuRpcManager.getIsInLobbyEvent += new System.Action<string>(this.HandleMenuRpcManagerGetIsInLobby);
    this._menuRpcManager.setIsInLobbyEvent += new System.Action<string, bool>(this.HandleMenuRpcManagerSetIsInLobby);
    this._menuRpcManager.getOwnedSongPacksEvent += new System.Action<string>(this.HandleMenuRpcManagerGetOwnedSongPacks);
    this._menuRpcManager.setPlayersPermissionConfigurationEvent += new System.Action<string, PlayersLobbyPermissionConfigurationNetSerializable>(this.HandleMenuRpcManagerSetPlayersPermissionConfiguration);
    this._multiplayerSessionManager.playerStateChangedEvent += new System.Action<IConnectedPlayer>(this.HandleMultiplayerSessionManagerPlayerStateChanged);
    this._multiplayerSessionManager.playerConnectedEvent += new System.Action<IConnectedPlayer>(this.HandleMultiplayerSessionManagerPlayerConnected);
    this._multiplayerSessionManager.playerDisconnectedEvent += new System.Action<IConnectedPlayer>(this.HandleMultiplayerSessionManagerPlayerDisconnected);
    this._menuRpcManager.GetIsReady();
    this._menuRpcManager.GetIsInLobby();
    this._menuRpcManager.GetRecommendedBeatmap();
    this._menuRpcManager.GetRecommendedGameplayModifiers();
    this._menuRpcManager.GetPlayersPermissionConfiguration();
    this.SetOwnedSongPacks();
    this.SetLocalPlayerIsReady(false, false);
    this.SetLocalPlayerIsInLobby(true, false);
    this.SetPlayerIsActive(this.localUserId, this._multiplayerSessionManager.localPlayer.WantsToPlayNextLevel(), false);
    this.NotifyModelChange(this.localUserId);
    foreach (IConnectedPlayer connectedPlayer in (IEnumerable<IConnectedPlayer>) this._multiplayerSessionManager.connectedPlayers)
      this.SetPlayerIsActive(connectedPlayer.userId, connectedPlayer.WantsToPlayNextLevel());
  }

  public virtual void Deactivate()
  {
    this.ClearData();
    this._menuRpcManager.getRecommendedBeatmapEvent -= new System.Action<string>(this.HandleMenuRpcManagerGetRecommendedBeatmap);
    this._menuRpcManager.recommendBeatmapEvent -= new System.Action<string, BeatmapIdentifierNetSerializable>(this.HandleMenuRpcManagerRecommendBeatmap);
    this._menuRpcManager.clearRecommendedBeatmapEvent -= new System.Action<string>(this.HandleMenuRpcManagerClearBeatmap);
    this._menuRpcManager.getRecommendedGameplayModifiersEvent -= new System.Action<string>(this.HandleMenuRpcManagerGetRecommendedGameplayModifiers);
    this._menuRpcManager.recommendGameplayModifiersEvent -= new System.Action<string, GameplayModifiers>(this.HandleMenuRpcManagerRecommendGameplayModifiers);
    this._menuRpcManager.clearRecommendedGameplayModifiersEvent -= new System.Action<string>(this.HandleMenuRpcManagerClearRecommendedGameplayModifiers);
    this._menuRpcManager.getIsReadyEvent -= new System.Action<string>(this.HandleMenuRpcManagerGetIsReady);
    this._menuRpcManager.setIsReadyEvent -= new System.Action<string, bool>(this.HandleMenuRpcManagerSetIsReady);
    this._menuRpcManager.getIsInLobbyEvent -= new System.Action<string>(this.HandleMenuRpcManagerGetIsInLobby);
    this._menuRpcManager.setIsInLobbyEvent -= new System.Action<string, bool>(this.HandleMenuRpcManagerSetIsInLobby);
    this._menuRpcManager.getOwnedSongPacksEvent -= new System.Action<string>(this.HandleMenuRpcManagerGetOwnedSongPacks);
    this._menuRpcManager.setPlayersPermissionConfigurationEvent -= new System.Action<string, PlayersLobbyPermissionConfigurationNetSerializable>(this.HandleMenuRpcManagerSetPlayersPermissionConfiguration);
    this._multiplayerSessionManager.playerStateChangedEvent -= new System.Action<IConnectedPlayer>(this.HandleMultiplayerSessionManagerPlayerStateChanged);
    this._multiplayerSessionManager.playerConnectedEvent -= new System.Action<IConnectedPlayer>(this.HandleMultiplayerSessionManagerPlayerConnected);
    this._multiplayerSessionManager.playerDisconnectedEvent -= new System.Action<IConnectedPlayer>(this.HandleMultiplayerSessionManagerPlayerDisconnected);
    this._cancellationTokenSource.Cancel();
  }

  public virtual void Dispose() => this.Deactivate();

  public virtual async void SetOwnedSongPacks()
  {
    SongPackMask songPackMask = new SongPackMask(((IEnumerable<IBeatmapLevelPack>) this._beatmapLevelsModel.ostAndExtrasPackCollection.beatmapLevelPacks).Select<IBeatmapLevelPack, string>((Func<IBeatmapLevelPack, string>) (pack => pack.packID)));
    IBeatmapLevelPack[] beatmapLevelPackArray = this._beatmapLevelsModel.dlcBeatmapLevelPackCollection.beatmapLevelPacks;
    for (int index = 0; index < beatmapLevelPackArray.Length; ++index)
    {
      IBeatmapLevelPack dlcSongPack = beatmapLevelPackArray[index];
      try
      {
        if (await this._additionalContentModel.GetPackEntitlementStatusAsync(dlcSongPack.packName, this._cancellationTokenSource.Token) == AdditionalContentModel.EntitlementStatus.Owned)
          songPackMask |= new SongPackMask(dlcSongPack.packID);
      }
      catch (TaskCanceledException)
      {
        return;
      }
      dlcSongPack = (IBeatmapLevelPack) null;
    }
    beatmapLevelPackArray = (IBeatmapLevelPack[]) null;
    this._menuRpcManager.SetOwnedSongPacks(songPackMask);
  }

  public virtual void HandleMenuRpcManagerGetRecommendedBeatmap(string userId)
  {
    BeatmapIdentifierNetSerializable identifier = this[this.localUserId].beatmapLevel.ToIdentifier();
    if (identifier != null)
      this._menuRpcManager.RecommendBeatmap(identifier);
    else
      this._menuRpcManager.ClearRecommendedBeatmap();
  }

  public virtual void HandleMenuRpcManagerGetRecommendedGameplayModifiers(string userId)
  {
    GameplayModifiers gameplayModifiers = this[this.localUserId].gameplayModifiers;
    if (gameplayModifiers != null)
      this._menuRpcManager.RecommendGameplayModifiers(gameplayModifiers);
    else
      this._menuRpcManager.ClearRecommendedGameplayModifiers();
  }

  public virtual void HandleMenuRpcManagerGetIsInLobby(string userId) => this._menuRpcManager.SetIsInLobby(this[this.localUserId].isInLobby);

  public virtual void HandleMenuRpcManagerGetIsReady(string userId) => this._menuRpcManager.SetIsReady(this[this.localUserId].isReady);

  public virtual void HandleMenuRpcManagerRecommendBeatmap(
    string userId,
    BeatmapIdentifierNetSerializable beatmapId)
  {
    this.SetPlayerBeatmapLevel(userId, beatmapId.ToPreviewDifficultyBeatmap(this._beatmapLevelsModel, this._beatmapCharacteristicCollection));
  }

  public virtual void HandleMenuRpcManagerRecommendGameplayModifiers(
    string userId,
    GameplayModifiers gameplayModifiers)
  {
    this.SetPlayerGameplayModifiers(userId, gameplayModifiers);
  }

  public virtual void HandleMenuRpcManagerClearRecommendedGameplayModifiers(string userId) => this.SetPlayerGameplayModifiers(userId, (GameplayModifiers) null);

  public virtual void HandleMenuRpcManagerClearBeatmap(string userId) => this.SetPlayerBeatmapLevel(userId, (PreviewDifficultyBeatmap) null);

  public virtual void HandleMenuRpcManagerSetIsReady(string userId, bool isReady) => this.SetPlayerIsReady(userId, isReady);

  public virtual void HandleMenuRpcManagerSetIsInLobby(string userId, bool isInLobby) => this.SetPlayerIsInLobby(userId, isInLobby);

  public virtual void HandleMultiplayerSessionManagerPlayerStateChanged(
    IConnectedPlayer connectedPlayer)
  {
    this.SetPlayerIsActive(connectedPlayer.userId, connectedPlayer.WantsToPlayNextLevel());
  }

  public virtual void HandleMultiplayerSessionManagerPlayerConnected(
    IConnectedPlayer connectedPlayer)
  {
    this.SetPlayerIsReady(connectedPlayer.userId, false, false);
    this.SetPlayerIsInLobby(connectedPlayer.userId, true, false);
    this.SetPlayerIsActive(connectedPlayer.userId, connectedPlayer.WantsToPlayNextLevel(), false);
    this.NotifyModelChange(connectedPlayer.userId);
  }

  public virtual void HandleMultiplayerSessionManagerPlayerDisconnected(
    IConnectedPlayer connectedPlayer)
  {
    this._playersData.Remove(connectedPlayer.userId);
    this.NotifyModelChange(connectedPlayer.userId);
  }

  public virtual void NotifyModelChange(string userId)
  {
    System.Action<string> didChangeEvent = this.didChangeEvent;
    if (didChangeEvent == null)
      return;
    didChangeEvent(userId);
  }

  public virtual void HandleMenuRpcManagerGetOwnedSongPacks(string userId) => this.SetOwnedSongPacks();

  public virtual void HandleMenuRpcManagerSetPlayersPermissionConfiguration(
    string userId,
    PlayersLobbyPermissionConfigurationNetSerializable playersLobbyPermissionConfiguration)
  {
    foreach (PlayerLobbyPermissionConfigurationNetSerializable configurationNetSerializable in playersLobbyPermissionConfiguration.playersPermission)
      this.SetPlayerIsPartyOwner(configurationNetSerializable.userId, configurationNetSerializable.isServerOwner);
  }

  public virtual IEnumerator<KeyValuePair<string, ILobbyPlayerData>> GetEnumerator()
  {
    foreach (KeyValuePair<string, LobbyPlayerData> keyValuePair in this._playersData)
      yield return new KeyValuePair<string, ILobbyPlayerData>(keyValuePair.Key, (ILobbyPlayerData) keyValuePair.Value);
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

  public int Count => this._playersData.Count;

  public virtual bool ContainsKey(string key) => this._playersData.ContainsKey(key);

  public virtual bool TryGetValue(string key, out ILobbyPlayerData value)
  {
    LobbyPlayerData lobbyPlayerData;
    int num = this._playersData.TryGetValue(key, out lobbyPlayerData) ? 1 : 0;
    value = (ILobbyPlayerData) lobbyPlayerData;
    return num != 0;
  }

  public IEnumerable<string> Keys => (IEnumerable<string>) this._playersData.Keys;

  public IEnumerable<ILobbyPlayerData> Values => (IEnumerable<ILobbyPlayerData>) this._playersData.Values;
}
