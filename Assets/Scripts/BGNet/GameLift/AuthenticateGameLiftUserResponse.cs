using System;
using BGNet.Core.Messages;
using LiteNetLib.Utils;

namespace GameLift
{
    // Token: 0x0200009B RID: 155
    public class AuthenticateGameLiftUserResponse : BaseReliableResponse, IGameLiftServerToClientMessage, IGameLiftMessage, IUnconnectedMessage, INetSerializable, IPoolablePacket, IUnconnectedAuthenticateResponse, IUnconnectedReliableResponse, IUnconnectedReliableRequest, IUnconnectedResponse
    {
        // Token: 0x170000F7 RID: 247
        // (get) Token: 0x060005FD RID: 1533 RVA: 0x00010886 File Offset: 0x0000EA86
        public static PacketPool<AuthenticateGameLiftUserResponse> pool
        {
            get
            {
                return ThreadStaticPacketPool<AuthenticateGameLiftUserResponse>.pool;
            }
        }

        // Token: 0x170000F8 RID: 248
        // (get) Token: 0x060005FE RID: 1534 RVA: 0x0001088D File Offset: 0x0000EA8D
        public override byte resultCode
        {
            get
            {
                return (byte)this.result;
            }
        }

        // Token: 0x170000F9 RID: 249
        // (get) Token: 0x060005FF RID: 1535 RVA: 0x00010896 File Offset: 0x0000EA96
        public override string resultCodeString
        {
            get
            {
                return this.result.ToString();
            }
        }

        // Token: 0x170000FA RID: 250
        // (get) Token: 0x06000600 RID: 1536 RVA: 0x000108A9 File Offset: 0x0000EAA9
        public bool isAuthenticated
        {
            get
            {
                return this.result == AuthenticateGameLiftUserResponse.Result.Success;
            }
        }

        // Token: 0x06000601 RID: 1537 RVA: 0x000108B4 File Offset: 0x0000EAB4
        public AuthenticateGameLiftUserResponse Init(AuthenticateGameLiftUserResponse.Result result)
        {
            this.result = result;
            return this;
        }

        // Token: 0x06000602 RID: 1538 RVA: 0x000108BE File Offset: 0x0000EABE
        public override void Serialize(NetDataWriter writer)
        {
            base.Serialize(writer);
            writer.Put((byte)this.result);
        }

        // Token: 0x06000603 RID: 1539 RVA: 0x000108D4 File Offset: 0x0000EAD4
        public override void Deserialize(NetDataReader reader)
        {
            base.Deserialize(reader);
            this.result = (AuthenticateGameLiftUserResponse.Result)reader.GetByte();
        }

        // Token: 0x06000604 RID: 1540 RVA: 0x000108E9 File Offset: 0x0000EAE9
        public override void Release()
        {
            AuthenticateGameLiftUserResponse.pool.Release(this);
        }

        // Token: 0x04000265 RID: 613
        public AuthenticateGameLiftUserResponse.Result result;

        // Token: 0x02000168 RID: 360
        public enum Result
        {
            // Token: 0x04000491 RID: 1169
            Success,
            // Token: 0x04000492 RID: 1170
            Failed,
            // Token: 0x04000493 RID: 1171
            UnknownError
        }
    }
}
