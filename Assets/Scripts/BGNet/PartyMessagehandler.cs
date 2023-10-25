using System;
using LiteNetLib.Utils;

// Token: 0x0200006B RID: 107
public class PartyMessageHandler : IDisposable
{
    // Token: 0x060004B9 RID: 1209 RVA: 0x0000CA24 File Offset: 0x0000AC24
    public PartyMessageHandler(ConnectedPlayerManager connectedPlayerManager)
    {
        this._connectedPlayerManager = connectedPlayerManager;
        this._serializer.RegisterCallback<PartyMessageHandler.ConnectToMasterServerMessage>(PartyMessageHandler.MessageType.ConnectToMasterServer, new Action<PartyMessageHandler.ConnectToMasterServerMessage>(this.HandleConnectToMasterServer), new Func<PartyMessageHandler.ConnectToMasterServerMessage>(PartyMessageHandler.ConnectToMasterServerMessage.pool.Obtain));
        this._connectedPlayerManager.RegisterSerializer(ConnectedPlayerManager.MessageType.Party, this._serializer);
    }

    // Token: 0x060004BA RID: 1210 RVA: 0x0000CA84 File Offset: 0x0000AC84
    public void Dispose()
    {
        this._connectedPlayerManager.UnregisterSerializer(ConnectedPlayerManager.MessageType.Party, this._serializer);
    }

    // Token: 0x140000AA RID: 170
    // (add) Token: 0x060004BB RID: 1211 RVA: 0x0000CA98 File Offset: 0x0000AC98
    // (remove) Token: 0x060004BC RID: 1212 RVA: 0x0000CAD0 File Offset: 0x0000ACD0
    public event PartyMessageHandler.ConnectToMasterServerDelegate connectToMasterServerEvent;

    // Token: 0x060004BD RID: 1213 RVA: 0x0000CB05 File Offset: 0x0000AD05
    public void ConnectToMasterServer(string secret)
    {
        this._connectedPlayerManager.Send<PartyMessageHandler.ConnectToMasterServerMessage>(PartyMessageHandler.ConnectToMasterServerMessage.pool.Obtain().Init(secret));
    }

    // Token: 0x060004BE RID: 1214 RVA: 0x0000CB22 File Offset: 0x0000AD22
    private void HandleConnectToMasterServer(PartyMessageHandler.ConnectToMasterServerMessage packet)
    {
        PartyMessageHandler.ConnectToMasterServerDelegate connectToMasterServerDelegate = this.connectToMasterServerEvent;
        if (connectToMasterServerDelegate != null)
        {
            connectToMasterServerDelegate(packet.secret);
        }
        packet.Release();
    }

    // Token: 0x040001CD RID: 461
    private readonly NetworkPacketSerializer<PartyMessageHandler.MessageType, IConnectedPlayer> _serializer = new NetworkPacketSerializer<PartyMessageHandler.MessageType, IConnectedPlayer>();

    // Token: 0x040001CE RID: 462
    private readonly ConnectedPlayerManager _connectedPlayerManager;

    // Token: 0x02000142 RID: 322
    private enum MessageType
    {
        // Token: 0x04000431 RID: 1073
        ConnectToMasterServer
    }

    // Token: 0x02000143 RID: 323
    // (Invoke) Token: 0x06000834 RID: 2100
    public delegate void ServerStatusUpdatedDelegate(BeatmapLevelSelectionMask selectionMask, GameplayServerConfiguration configuration);

    // Token: 0x02000144 RID: 324
    // (Invoke) Token: 0x06000838 RID: 2104
    public delegate void ConnectToMasterServerDelegate(string secret);

    // Token: 0x02000145 RID: 325
    private class ConnectToMasterServerMessage : INetSerializable, IPoolablePacket
    {
        // Token: 0x17000162 RID: 354
        // (get) Token: 0x0600083B RID: 2107 RVA: 0x000155B6 File Offset: 0x000137B6
        public static PacketPool<PartyMessageHandler.ConnectToMasterServerMessage> pool
        {
            get
            {
                return ThreadStaticPacketPool<PartyMessageHandler.ConnectToMasterServerMessage>.pool;
            }
        }

        // Token: 0x0600083C RID: 2108 RVA: 0x000155BD File Offset: 0x000137BD
        public PartyMessageHandler.ConnectToMasterServerMessage Init(string secret)
        {
            this.secret = secret;
            return this;
        }

        // Token: 0x0600083D RID: 2109 RVA: 0x000155C7 File Offset: 0x000137C7
        public void Serialize(NetDataWriter writer)
        {
            writer.Put(this.secret);
        }

        // Token: 0x0600083E RID: 2110 RVA: 0x000155D5 File Offset: 0x000137D5
        public void Deserialize(NetDataReader reader)
        {
            this.secret = reader.GetString();
        }

        // Token: 0x0600083F RID: 2111 RVA: 0x000155E3 File Offset: 0x000137E3
        public void Release()
        {
            PartyMessageHandler.ConnectToMasterServerMessage.pool.Release(this);
        }

        // Token: 0x04000432 RID: 1074
        public string secret;
    }
}
