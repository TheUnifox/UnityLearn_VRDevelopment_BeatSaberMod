using System;
using BGNet.Core.Messages;
using LiteNetLib.Utils;

namespace GameLift
{
    // Token: 0x0200009A RID: 154
    public class AuthenticateGameLiftUserRequest : BaseReliableResponse, IGameLiftClientToServerMessage, IGameLiftMessage, IUnconnectedMessage, INetSerializable, IPoolablePacket, IUnconnectedAuthenticateRequest, IUnconnectedReliableResponse, IUnconnectedReliableRequest, IUnconnectedResponse
    {
        // Token: 0x170000F4 RID: 244
        // (get) Token: 0x060005F3 RID: 1523 RVA: 0x000107D6 File Offset: 0x0000E9D6
        public static PacketPool<AuthenticateGameLiftUserRequest> pool
        {
            get
            {
                return ThreadStaticPacketPool<AuthenticateGameLiftUserRequest>.pool;
            }
        }

        // Token: 0x170000F5 RID: 245
        // (get) Token: 0x060005F4 RID: 1524 RVA: 0x000107DD File Offset: 0x0000E9DD
        // (set) Token: 0x060005F5 RID: 1525 RVA: 0x000107E5 File Offset: 0x0000E9E5
        public string userId { get; private set; }

        // Token: 0x170000F6 RID: 246
        // (get) Token: 0x060005F6 RID: 1526 RVA: 0x000107EE File Offset: 0x0000E9EE
        // (set) Token: 0x060005F7 RID: 1527 RVA: 0x000107F6 File Offset: 0x0000E9F6
        public string userName { get; private set; }

        // Token: 0x060005F8 RID: 1528 RVA: 0x000107FF File Offset: 0x0000E9FF
        public AuthenticateGameLiftUserRequest Init(string userId, string userName, string playerSessionId)
        {
            this.userId = userId;
            this.userName = userName;
            this.playerSessionId = playerSessionId;
            return this;
        }

        // Token: 0x060005F9 RID: 1529 RVA: 0x00010817 File Offset: 0x0000EA17
        public override void Serialize(NetDataWriter writer)
        {
            base.Serialize(writer);
            writer.Put(this.userId);
            writer.Put(this.userName);
            writer.Put(this.playerSessionId);
        }

        // Token: 0x060005FA RID: 1530 RVA: 0x00010844 File Offset: 0x0000EA44
        public override void Deserialize(NetDataReader reader)
        {
            base.Deserialize(reader);
            this.userId = reader.GetString();
            this.userName = reader.GetString();
            this.playerSessionId = reader.GetString();
        }

        // Token: 0x060005FB RID: 1531 RVA: 0x00010871 File Offset: 0x0000EA71
        public override void Release()
        {
            AuthenticateGameLiftUserRequest.pool.Release(this);
        }

        // Token: 0x04000264 RID: 612
        public string playerSessionId;
    }
}
