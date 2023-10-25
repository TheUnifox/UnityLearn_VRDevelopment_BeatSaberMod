// Decompiled with JetBrains decompiler
// Type: OculusNetworkPlayerModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Oculus.Platform;
using Oculus.Platform.Models;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class OculusNetworkPlayerModel : PlatformNetworkPlayerModel, INetworkPlayerModel
{
  protected const string kConnectionTypeKey = "connection";
  protected const string kConnectionTypeOculus = "oculus";
  protected const string kConnectionTypeLiteNetLib = "litenetlib";
  protected const string kCurrentPartySizeKey = "currentPartySize";
  protected const string kDifficultiesKey = "difficulties";
  protected const string kModifiersKey = "modifiers";
  protected const string kSongPacksKey = "songPacks";
  protected const string kMaxPlayerCountKey = "maxPlayerCount";
  protected const string kDiscoveryPolicyKey = "discoveryPolicy";
  protected const string kInvitePolicyKey = "invitePolicy";
  protected const string kGameplayServerModeKey = "gameplayServerMode";
  protected const string kSongSelectionModeKey = "songSelectionMode";
  protected const string kGameplayServerControlSettingsKey = "gameplayServerControlSettings";
  [Inject]
  protected readonly IPlatformUserModel _platformUserModel;
  protected const float kAutoRefreshRate = 30f;
  protected float _lastRefreshTime = -30f;
  protected bool _isRefreshing;
  protected OculusNetworkPlayerModel.OculusNetworkPlayer _localPlayer;
  protected RoomJoinPolicy _joinPolicy = RoomJoinPolicy.InvitedUsers;
  protected bool _partyEnabled;
  protected bool _waitingOnRoomCreate;
  protected readonly Dictionary<string, string> _roomDataStore = new Dictionary<string, string>();
  protected readonly List<OculusNetworkPlayerModel.OculusNetworkPlayer> _partyPlayers = new List<OculusNetworkPlayerModel.OculusNetworkPlayer>();
  protected readonly List<OculusNetworkPlayerModel.OculusNetworkPlayer> _otherPlayers = new List<OculusNetworkPlayerModel.OculusNetworkPlayer>();

  public INetworkPlayer localPlayer => (INetworkPlayer) this._localPlayer;

  public override bool localPlayerIsPartyOwner => this._localPlayer.isPartyOwner;

  public override int currentPartySize => this._partyPlayers.Count;

  public override event System.Action<int> partySizeChangedEvent;

  public override event System.Action<INetworkPlayerModel> partyChangedEvent;

  public override event System.Action<INetworkPlayer> inviteRequestedEvent;

  protected OculusConnectionManager oculusConnectionManager => this.connectedPlayerManager?.GetConnectionManager<OculusConnectionManager>();

  protected override async void Start()
  {
    OculusNetworkPlayerModel playerModel = this;
    UserInfo userInfo = await playerModel._platformUserModel.GetUserInfo();
    if (userInfo == null || (UnityEngine.Object) playerModel == (UnityEngine.Object) null)
      return;
    playerModel._localPlayer = new OculusNetworkPlayerModel.OculusNetworkPlayer(playerModel, ulong.Parse(userInfo.platformUserId), userInfo.userName, true);
    playerModel._partyPlayers.Add(playerModel._localPlayer);
    Rooms.SetUpdateNotificationCallback(new Message<Room>.Callback(playerModel.HandleRoomUpdate));
    Rooms.SetRoomInviteAcceptedNotificationCallback(new Message<string>.Callback(playerModel.HandleRoomInviteAccepted));
    Rooms.SetRoomInviteReceivedNotificationCallback(new Message<RoomInviteNotification>.Callback(playerModel.HandleRoomInviteReceived));
    // ISSUE: explicit non-virtual call
    __nonvirtual (playerModel.Refresh());
  }

  protected override void Update()
  {
    base.Update();
    if ((double) this._lastRefreshTime >= (double) Time.realtimeSinceStartup - 30.0 || !this.discoveryEnabled)
      return;
    this.Refresh();
  }

  protected override IEnumerable<INetworkPlayer> GetPartyPlayers() => (IEnumerable<INetworkPlayer>) this._partyPlayers;

  protected override IEnumerable<INetworkPlayer> GetOtherPlayers() => (IEnumerable<INetworkPlayer>) this._otherPlayers;

  public virtual OculusNetworkPlayerModel.OculusNetworkPlayer GetPlayer(ulong id)
  {
    for (int index = 0; index < this._partyPlayers.Count; ++index)
    {
      if ((long) this._partyPlayers[index].id == (long) id)
        return this._partyPlayers[index];
    }
    for (int index = 0; index < this._otherPlayers.Count; ++index)
    {
      if ((long) this._otherPlayers[index].id == (long) id)
        return this._otherPlayers[index];
    }
    return (OculusNetworkPlayerModel.OculusNetworkPlayer) null;
  }

  public virtual OculusNetworkPlayerModel.OculusNetworkPlayer GetPlayer(string userId)
  {
    for (int index = 0; index < this._partyPlayers.Count; ++index)
    {
      if (this._partyPlayers[index].userId == userId)
        return this._partyPlayers[index];
    }
    for (int index = 0; index < this._otherPlayers.Count; ++index)
    {
      if (this._otherPlayers[index].userId == userId)
        return this._otherPlayers[index];
    }
    return (OculusNetworkPlayerModel.OculusNetworkPlayer) null;
  }

  public virtual bool TryCreateRoom()
  {
    if (this._localPlayer == null)
      return false;
    if (this._localPlayer.room != null || this._waitingOnRoomCreate)
      return true;
    if (!this.isConnectedOrConnecting)
    {
      if (!this.CreateConnectedPlayerManager<OculusConnectionManager>((IConnectionInitParams<OculusConnectionManager>) new OculusConnectionManager.StartServerParams()
      {
        oculusNetworkPlayerModel = this
      }))
        return false;
    }
    this._waitingOnRoomCreate = true;
    RoomOptions roomOptions = new RoomOptions();
    foreach (KeyValuePair<string, string> keyValuePair in this._roomDataStore)
      roomOptions.SetDataStore(keyValuePair.Key, keyValuePair.Value);
    Rooms.CreateAndJoinPrivate2(this._joinPolicy, (uint) this.configuration.maxPlayerCount, roomOptions).OnComplete((Message<Room>.Callback) (result =>
    {
      this._waitingOnRoomCreate = false;
      if (result.IsError)
        return;
      this.HandleRoomUpdate(result);
    }));
    return true;
  }

  public virtual string GetUserName(ulong id) => this.GetPlayer(id)?.userName;

  public virtual Task<string> GetUserNameAsync(ulong id)
  {
    TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
    Users.Get(id).OnComplete((Message<User>.Callback) (result =>
    {
      if (result.IsError)
      {
        Debug.LogError((object) ("Could not find user with id " + (object) id));
        tcs.SetResult((string) null);
      }
      else
        tcs.SetResult(result.Data.OculusID);
    }));
    return tcs.Task;
  }

  public virtual async void HandleRoomInviteReceived(Message<RoomInviteNotification> message)
  {
    OculusNetworkPlayerModel playerModel = this;
    // ISSUE: explicit non-virtual call
    OculusNetworkPlayerModel.OculusNetworkPlayer player = __nonvirtual (playerModel.GetPlayer(message.Data.SenderID));
    if (player == null)
    {
      // ISSUE: explicit non-virtual call
      string userNameAsync = await __nonvirtual (playerModel.GetUserNameAsync(message.Data.SenderID));
      if (userNameAsync == null)
        return;
      player = new OculusNetworkPlayerModel.OculusNetworkPlayer(playerModel, message.Data.SenderID, userNameAsync);
    }
    Rooms.Get(message.Data.RoomID).OnComplete((Message<Room>.Callback) (result =>
    {
      if (result.IsError)
        return;
      player.room = result.Data;
      System.Action<INetworkPlayer> inviteRequestedEvent = this.inviteRequestedEvent;
      if (inviteRequestedEvent == null)
        return;
      inviteRequestedEvent((INetworkPlayer) player);
    }));
  }

  public virtual void HandleRoomInviteAccepted(Message<string> message)
  {
  }

  public virtual void HandleRoomUpdate(Message<Room> message)
  {
    if (message.IsError)
      return;
    Room room = message.Data;
    if (room != null && room.Joinability != RoomJoinability.AreIn)
      room = (Room) null;
    this._localPlayer.room = room;
    if (room == null)
    {
      if (this._partyEnabled)
        this.TryCreateRoom();
      this.HandlePlayersChanged();
    }
    else
    {
      if (room.UsersOptional != null)
      {
        for (int index = this._partyPlayers.Count - 1; index >= 0; --index)
        {
          if (!this._partyPlayers[index].isMe)
          {
            this._partyPlayers[index].room = (Room) null;
            this._otherPlayers.Add(this._partyPlayers[index]);
            this._partyPlayers.RemoveAt(index);
          }
        }
        foreach (User user in (DeserializableList<User>) room.UsersOptional)
        {
          if ((long) user.ID != (long) this._localPlayer.id)
          {
            OculusNetworkPlayerModel.OculusNetworkPlayer oculusNetworkPlayer = this.GetPlayer(user.ID) ?? new OculusNetworkPlayerModel.OculusNetworkPlayer(this, user.ID, user.OculusID);
            this._otherPlayers.Remove(oculusNetworkPlayer);
            this._partyPlayers.Add(oculusNetworkPlayer);
            oculusNetworkPlayer.isWaitingOnInvite = false;
            oculusNetworkPlayer.room = room;
          }
        }
      }
      if (!this.localPlayerIsPartyOwner && !this.isClient)
        this.TryConnectToServer();
      this.HandlePlayersChanged();
    }
  }

  public virtual void HandlePlayersChanged()
  {
    System.Action<int> sizeChangedEvent = this.partySizeChangedEvent;
    if (sizeChangedEvent != null)
      sizeChangedEvent(this.currentPartySize);
    System.Action<INetworkPlayerModel> partyChangedEvent = this.partyChangedEvent;
    if (partyChangedEvent == null)
      return;
    partyChangedEvent((INetworkPlayerModel) this);
  }

  public virtual void Refresh()
  {
    if (this._isRefreshing)
      return;
    this._lastRefreshTime = Time.realtimeSinceStartup;
    this._isRefreshing = true;
    for (int index = 0; index < this._otherPlayers.Count; ++index)
      this._otherPlayers[index].removed = true;
    Message<UserAndRoomList>.Callback onGetFriends = (Message<UserAndRoomList>.Callback) null;
    Message<UserList>.Callback onGetInvitable = (Message<UserList>.Callback) null;
    onGetFriends = (Message<UserAndRoomList>.Callback) (result =>
    {
      if (result.IsError)
      {
        this._isRefreshing = false;
      }
      else
      {
        foreach (UserAndRoom userAndRoom in (DeserializableList<UserAndRoom>) result.Data)
        {
          if (userAndRoom.User.PresenceStatus == UserPresenceStatus.Online)
          {
            OculusNetworkPlayerModel.OculusNetworkPlayer oculusNetworkPlayer = this.GetPlayer(userAndRoom.User.ID);
            if (oculusNetworkPlayer == null)
            {
              oculusNetworkPlayer = new OculusNetworkPlayerModel.OculusNetworkPlayer(this, userAndRoom.User.ID, userAndRoom.User.OculusID);
              this._otherPlayers.Add(oculusNetworkPlayer);
            }
            oculusNetworkPlayer.room = userAndRoom.RoomOptional;
            oculusNetworkPlayer.removed = false;
            if (oculusNetworkPlayer.SameRoomAs(this._localPlayer))
            {
              if (this._otherPlayers.Remove(oculusNetworkPlayer))
                this._partyPlayers.Add(oculusNetworkPlayer);
            }
            else if (this._partyPlayers.Remove(oculusNetworkPlayer))
              this._otherPlayers.Add(oculusNetworkPlayer);
          }
        }
        if (result.Data.HasNextPage)
          Users.GetNextUserAndRoomListPage(result.Data).OnComplete(onGetFriends);
        else
          Rooms.GetInvitableUsers2().OnComplete(onGetInvitable);
      }
    });
    onGetInvitable = (Message<UserList>.Callback) (result =>
    {
      if (result.IsError)
      {
        this._isRefreshing = false;
      }
      else
      {
        foreach (User user in (DeserializableList<User>) result.Data)
        {
          if (user.PresenceStatus == UserPresenceStatus.Online)
          {
            OculusNetworkPlayerModel.OculusNetworkPlayer oculusNetworkPlayer = this.GetPlayer(user.ID);
            if (oculusNetworkPlayer == null)
            {
              oculusNetworkPlayer = new OculusNetworkPlayerModel.OculusNetworkPlayer(this, user.ID, user.OculusID);
              this._otherPlayers.Add(oculusNetworkPlayer);
            }
            oculusNetworkPlayer.removed = false;
            oculusNetworkPlayer.inviteToken = user.InviteToken;
            if (oculusNetworkPlayer.inviteToken != null && oculusNetworkPlayer.isWaitingOnInvite)
              oculusNetworkPlayer.isWaitingOnInvite = false;
          }
        }
        if (result.Data.HasNextPage)
        {
          Users.GetNextUserListPage(result.Data).OnComplete(onGetInvitable);
        }
        else
        {
          this._isRefreshing = false;
          for (int index = this._otherPlayers.Count - 1; index >= 0; --index)
          {
            if (this._otherPlayers[index].removed)
              this._otherPlayers.RemoveAt(index);
          }
          this.HandlePlayersChanged();
        }
      }
    });
    Users.GetLoggedInUserFriendsAndRooms().OnComplete(onGetFriends);
  }

  public override bool CreatePartyConnection<T>(INetworkPlayerModelPartyConfig<T> createConfig)
  {
    if (!base.CreatePartyConnection<T>(createConfig) || !(createConfig is PlatformNetworkPlayerModel.CreatePartyConfig createPartyConfig))
      return false;
    this._partyEnabled = true;
    this._joinPolicy = createPartyConfig.configuration.discoveryPolicy == DiscoveryPolicy.Public ? RoomJoinPolicy.Everyone : RoomJoinPolicy.InvitedUsers;
    if (this.localPlayerIsPartyOwner && this._localPlayer.room != null)
      Rooms.UpdatePrivateRoomJoinPolicy(this._localPlayer.room.ID, this._joinPolicy).OnComplete(new Message<Room>.Callback(this.HandleRoomUpdate));
    this._roomDataStore["currentPartySize"] = string.Concat((object) this.partyManager.currentPartySize);
    this._roomDataStore["difficulties"] = string.Concat((object) (int) this.partyManager.selectionMask.difficulties);
    this._roomDataStore["modifiers"] = string.Concat((object) (int) this.partyManager.selectionMask.modifiers);
    // ISSUE: explicit non-virtual call
    this._roomDataStore["songPacks"] = __nonvirtual (this.partyManager.selectionMask.songPacks.ToShortString());
    this._roomDataStore["maxPlayerCount"] = string.Concat((object) this.partyManager.configuration.maxPlayerCount);
    this._roomDataStore["discoveryPolicy"] = string.Concat((object) (int) this.partyManager.configuration.discoveryPolicy);
    this._roomDataStore["invitePolicy"] = string.Concat((object) (int) this.partyManager.configuration.invitePolicy);
    this._roomDataStore["gameplayServerMode"] = string.Concat((object) (int) this.partyManager.configuration.gameplayServerMode);
    this._roomDataStore["songSelectionMode"] = string.Concat((object) (int) this.partyManager.configuration.songSelectionMode);
    this._roomDataStore["gameplayServerControlSettings"] = string.Concat((object) (int) this.partyManager.configuration.gameplayServerControlSettings);
    this.UpdateRoomDataStore();
    return this.TryCreateRoom();
  }

  public override void DestroyPartyConnection()
  {
    base.DestroyPartyConnection();
    this._partyEnabled = false;
    this.TryLeaveRoom();
  }

  protected override void PlayerConnected(IConnectedPlayer connectedPlayer)
  {
    OculusNetworkPlayerModel.OculusNetworkPlayer player = this.GetPlayer(connectedPlayer.userId);
    if (player == null)
      return;
    player.connectedPlayer = connectedPlayer;
  }

  protected override void PlayerDisconnected(IConnectedPlayer connectedPlayer)
  {
    OculusNetworkPlayerModel.OculusNetworkPlayer player = this.GetPlayer(connectedPlayer.userId);
    if (player != null)
    {
      player.connectedPlayer = (IConnectedPlayer) null;
      player.room = (Room) null;
    }
    this.HandlePlayersChanged();
  }

  protected override void PartySizeChanged(int currentPartySize)
  {
    this._roomDataStore[nameof (currentPartySize)] = currentPartySize.ToString();
    this.UpdateRoomDataStore();
  }

  public virtual void UpdateRoomDataStore()
  {
    if (this._localPlayer?.room == null)
      return;
    Rooms.UpdateDataStore(this._localPlayer.room.ID, this._roomDataStore);
  }

  public virtual void TryLeaveRoom()
  {
    if (this._localPlayer?.room == null)
      return;
    Rooms.Leave(this._localPlayer.room.ID).OnComplete(new Message<Room>.Callback(this.HandleRoomUpdate));
    this._localPlayer.room = (Room) null;
    for (int index = this._partyPlayers.Count - 1; index >= 0; --index)
    {
      if (!this._partyPlayers[index].isMe)
      {
        this._otherPlayers.Add(this._partyPlayers[index]);
        this._partyPlayers.RemoveAt(index);
      }
    }
    this.HandlePlayersChanged();
  }

  public virtual void TryConnectToServer()
  {
    if (this._localPlayer?.room?.OwnerOptional == null)
      return;
    this.CreateConnectedPlayerManager<OculusConnectionManager>((IConnectionInitParams<OculusConnectionManager>) new OculusConnectionManager.ConnectToServerParams()
    {
      serverId = this._localPlayer.room.OwnerOptional.ID,
      oculusNetworkPlayerModel = this
    });
  }

  public virtual Task<bool> ShouldAcceptConnectionFromPlayer(ulong userId)
  {
    OculusNetworkPlayerModel.OculusNetworkPlayer player1 = this.GetPlayer(userId);
    // ISSUE: explicit non-virtual call
    if ((player1 != null ? (__nonvirtual (player1.SameRoomAs(this._localPlayer)) ? 1 : 0) : 0) != 0)
      return Task.FromResult<bool>(true);
    TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
    Rooms.GetCurrent().OnComplete((Message<Room>.Callback) (result =>
    {
      this.HandleRoomUpdate(result);
      TaskCompletionSource<bool> completionSource = tcs;
      OculusNetworkPlayerModel.OculusNetworkPlayer player2 = this.GetPlayer(userId);
      // ISSUE: explicit non-virtual call
      int num = player2 != null ? (__nonvirtual (player2.SameRoomAs(this._localPlayer)) ? 1 : 0) : 0;
      completionSource.SetResult(num != 0);
    }));
    return tcs.Task;
  }

  [CompilerGenerated]
  public virtual void m_CTryCreateRoomm_Eb__48_0(Message<Room> result)
  {
    this._waitingOnRoomCreate = false;
    if (result.IsError)
      return;
    this.HandleRoomUpdate(result);
  }

  public class OculusNetworkPlayer : INetworkPlayer
  {
    protected readonly OculusNetworkPlayerModel _playerModel;
    protected readonly ulong _id;
    protected readonly string _userId;
    protected readonly string _userName;
    protected readonly bool _isMe;
    protected Room _room;
    protected BeatmapLevelSelectionMask? _cachedSelectionMask;
    protected GameplayServerConfiguration? _cachedConfiguration;
    public bool removed;
    public string inviteToken;
    [CompilerGenerated]
    protected IConnectedPlayer m_CconnectedPlayer;
    [CompilerGenerated]
    protected bool m_CisWaitingOnJoin;
    [CompilerGenerated]
    protected bool m_CisWaitingOnInvite;

    public ulong id => this._id;

    public string userId => this._userId;

    public string userName => this._userName;

    public bool isMe => this._isMe;

    public bool isPartyOwner => this._room == null || this.isRoomOwner;

    public bool isRoomOwner
    {
      get
      {
        ulong? id1 = this._room?.OwnerOptional?.ID;
        ulong id2 = this._id;
        return (long) id1.GetValueOrDefault() == (long) id2 & id1.HasValue;
      }
    }

    public int currentPartySize
    {
      get
      {
        if (this._room == null)
          return 1;
        string s;
        int result;
        if (this._room.DataStore != null && this._room.DataStore.TryGetValue(nameof (currentPartySize), out s) && int.TryParse(s, out result))
          return result;
        return this._room.UsersOptional == null ? 1 : this._room.UsersOptional.Count;
      }
    }

    public BeatmapLevelSelectionMask selectionMask
    {
      get
      {
        BeatmapDifficultyMask difficulties = BeatmapDifficultyMask.All;
        GameplayModifierMask modifiers = GameplayModifierMask.All;
        SongPackMask songPacks = SongPackMask.all;
        if (this._room?.DataStore != null)
        {
          string s1;
          int result1;
          if (this._room.DataStore.TryGetValue("difficulties", out s1) && int.TryParse(s1, out result1))
            difficulties = (BeatmapDifficultyMask) result1;
          string s2;
          int result2;
          if (this._room.DataStore.TryGetValue("modifiers", out s2) && int.TryParse(s2, out result2))
            modifiers = (GameplayModifierMask) result2;
          string stringSerializedMask;
          SongPackMask songPackMask;
          if (this._room.DataStore.TryGetValue("songPacks", out stringSerializedMask) && SongPackMask.TryParse(stringSerializedMask, out songPackMask))
            songPacks = songPackMask;
        }
        this._cachedSelectionMask = new BeatmapLevelSelectionMask?(new BeatmapLevelSelectionMask(difficulties, modifiers, songPacks));
        return this._cachedSelectionMask.GetValueOrDefault();
      }
    }

    public GameplayServerConfiguration configuration
    {
      get
      {
        if (this._cachedConfiguration.HasValue)
          return this._cachedConfiguration.GetValueOrDefault();
        int maxPlayerCount = this._room != null ? (int) this._room.MaxUsers : 1;
        DiscoveryPolicy discoveryPolicy = DiscoveryPolicy.Public;
        InvitePolicy invitePolicy = InvitePolicy.AnyoneCanInvite;
        GameplayServerMode gameplayServerMode = GameplayServerMode.Managed;
        SongSelectionMode songSelectionMode = SongSelectionMode.OwnerPicks;
        GameplayServerControlSettings gameplayServerControlSettings = GameplayServerControlSettings.All;
        if (this._room?.DataStore != null)
        {
          string s1;
          int result1;
          if (this._room.DataStore.TryGetValue("maxPlayerCount", out s1) && int.TryParse(s1, out result1))
            maxPlayerCount = result1;
          string s2;
          int result2;
          if (this._room.DataStore.TryGetValue("discoveryPolicy", out s2) && int.TryParse(s2, out result2))
            discoveryPolicy = (DiscoveryPolicy) result2;
          string s3;
          int result3;
          if (this._room.DataStore.TryGetValue("invitePolicy", out s3) && int.TryParse(s3, out result3))
            invitePolicy = (InvitePolicy) result3;
          string s4;
          int result4;
          if (this._room.DataStore.TryGetValue("gameplayServerMode", out s4) && int.TryParse(s4, out result4))
            gameplayServerMode = (GameplayServerMode) result4;
          string s5;
          int result5;
          if (this._room.DataStore.TryGetValue("songSelectionMode", out s5) && int.TryParse(s5, out result5))
            songSelectionMode = (SongSelectionMode) result5;
          string s6;
          int result6;
          if (this._room.DataStore.TryGetValue("gameplayServerControlSettings", out s6) && int.TryParse(s6, out result6))
            gameplayServerControlSettings = (GameplayServerControlSettings) result6;
        }
        this._cachedConfiguration = new GameplayServerConfiguration?(new GameplayServerConfiguration(maxPlayerCount, discoveryPolicy, invitePolicy, gameplayServerMode, songSelectionMode, gameplayServerControlSettings));
        return this._cachedConfiguration.GetValueOrDefault();
      }
    }

    public Room room
    {
      get => this._room;
      set
      {
        this._cachedConfiguration = new GameplayServerConfiguration?();
        this._cachedSelectionMask = new BeatmapLevelSelectionMask?();
        this._room = value;
      }
    }

    public IConnectedPlayer connectedPlayer
    {
      get => this.m_CconnectedPlayer;
      set => this.m_CconnectedPlayer = value;
    }

    public OculusNetworkPlayer(
      OculusNetworkPlayerModel playerModel,
      ulong id,
      string userName,
      bool isMe = false)
    {
      this._playerModel = playerModel;
      this._id = id;
      this._userId = NetworkUtility.GetHashedUserId(id.ToString(), AuthenticationToken.Platform.OculusRift);
      this._userName = userName;
      this._isMe = isMe;
    }

    public bool isMyPartyOwner
    {
      get
      {
        if (!this.isPartyOwner)
          return false;
        return this.isMe || this.SameRoomAs(this._playerModel._localPlayer);
      }
    }

    public bool canJoin => this._room != null && this._room.Joinability == RoomJoinability.CanJoin;

    public virtual void Join()
    {
      if (!this.canJoin)
      {
        this._playerModel.HandlePlayersChanged();
      }
      else
      {
        this.isWaitingOnJoin = true;
        Rooms.Join2(this._room.ID, (RoomOptions) null).OnComplete((Message<Room>.Callback) (result =>
        {
          this.isWaitingOnJoin = false;
          this._playerModel.HandleRoomUpdate(result);
        }));
      }
    }

    public bool requiresPassword => false;

    public virtual void Join(string password)
    {
    }

    public bool isWaitingOnJoin
    {
      get => this.m_CisWaitingOnJoin;
      private set => this.m_CisWaitingOnJoin = value;
    }

    public bool canInvite => this.inviteToken != null && this._playerModel._localPlayer.isRoomOwner && !this.SameRoomAs(this._playerModel._localPlayer);

    public virtual void Invite()
    {
      if (!this.canInvite)
      {
        this._playerModel.HandlePlayersChanged();
      }
      else
      {
        string inviteToken = this.inviteToken;
        this.inviteToken = (string) null;
        this.isWaitingOnInvite = true;
        Rooms.InviteUser(this._playerModel._localPlayer._room.ID, inviteToken);
      }
    }

    public bool isWaitingOnInvite
    {
      get => this.m_CisWaitingOnInvite;
      set => this.m_CisWaitingOnInvite = value;
    }

    public bool canKick => this._playerModel.localPlayerIsPartyOwner && !this.isMe && this.SameRoomAs(this._playerModel._localPlayer);

    public virtual void Kick()
    {
      if (!this.canKick)
      {
        this._playerModel.HandlePlayersChanged();
      }
      else
      {
        if (this._playerModel.isServer)
          this._playerModel.connectedPlayerManager?.KickPlayer(this.userId);
        long id1 = (long) this._room.ID;
        this._room = (Room) null;
        long id2 = (long) this._id;
        Rooms.KickUser((ulong) id1, (ulong) id2, 30).OnComplete(new Message<Room>.Callback(this._playerModel.HandleRoomUpdate));
      }
    }

    public bool canLeave => this.SameRoomAs(this._playerModel._localPlayer) && this._room.UsersOptional != null && this._room.UsersOptional.Count > 1;

    public virtual void Leave() => this._playerModel.DestroyPartyConnection();

    public bool canBlock => false;

    public virtual void Block()
    {
    }

    public bool canUnblock => false;

    public virtual void Unblock()
    {
    }

    public virtual void SendJoinResponse(bool accept) => throw new NotImplementedException();

    public virtual void SendInviteResponse(bool accept)
    {
      if (!accept)
        return;
      this.Join();
    }

    public virtual bool SameRoomAs(OculusNetworkPlayerModel.OculusNetworkPlayer other) => this._room != null && other._room != null && (long) this._room.ID == (long) other._room.ID;

    [CompilerGenerated]
    public virtual void m_CJoinm_Eb__40_0(Message<Room> result)
    {
      this.isWaitingOnJoin = false;
      this._playerModel.HandleRoomUpdate(result);
    }
  }
}
