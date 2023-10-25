using System;
using System.Net;
using System.Threading.Tasks;
using BGNet.Core;
using BGNet.Core.Messages;
using GameLift;

// Token: 0x0200001D RID: 29
public class GameLiftClientMessageHandler : BaseClientUnconnectedMessageHandler
{
    // Token: 0x060000F0 RID: 240 RVA: 0x000052A9 File Offset: 0x000034A9
    public GameLiftClientMessageHandler(IUnconnectedMessageSender sender, DnsEndPoint endPoint, BGNet.Core.ITimeProvider timeProvider, ITaskUtility taskUtility, ICertificateValidator certificateValidator, IAnalyticsManager analytics = null) : base(sender, endPoint, timeProvider, taskUtility, certificateValidator, analytics)
    {
        this.RegisterGameLiftMessages();
    }

    // Token: 0x060000F1 RID: 241 RVA: 0x000052C0 File Offset: 0x000034C0
    private void RegisterGameLiftMessages()
    {
        NetworkPacketSerializer<GameLiftMessageType, UnconnectedMessageHandler.MessageOrigin> networkPacketSerializer = new NetworkPacketSerializer<GameLiftMessageType, UnconnectedMessageHandler.MessageOrigin>();
        networkPacketSerializer.RegisterCallback<AuthenticateGameLiftUserRequest>(GameLiftMessageType.AuthenticateUserRequest, new Action<AuthenticateGameLiftUserRequest, UnconnectedMessageHandler.MessageOrigin>(base.DefaultResponseHandler<AuthenticateGameLiftUserRequest>), new Func<AuthenticateGameLiftUserRequest>(AuthenticateGameLiftUserRequest.pool.Obtain));
        networkPacketSerializer.RegisterCallback<AuthenticateGameLiftUserResponse>(GameLiftMessageType.AuthenticateUserResponse, new Action<AuthenticateGameLiftUserResponse, UnconnectedMessageHandler.MessageOrigin>(base.DefaultResponseHandler<AuthenticateGameLiftUserResponse>), new Func<AuthenticateGameLiftUserResponse>(AuthenticateGameLiftUserResponse.pool.Obtain));
        networkPacketSerializer.RegisterCallback<GameLiftMessageReceivedAcknowledge>(GameLiftMessageType.MessageReceivedAcknowledge, new Action<GameLiftMessageReceivedAcknowledge, UnconnectedMessageHandler.MessageOrigin>(base.DefaultAcknowledgeHandler<GameLiftMessageReceivedAcknowledge>), new Func<GameLiftMessageReceivedAcknowledge>(GameLiftMessageReceivedAcknowledge.pool.Obtain));
        networkPacketSerializer.RegisterCallback<GameLiftMultipartMessage>(GameLiftMessageType.MultipartMessage, new Action<GameLiftMultipartMessage, UnconnectedMessageHandler.MessageOrigin>(base.DefaultMultipartMessageHandler<GameLiftMultipartMessage>), new Func<GameLiftMultipartMessage>(GameLiftMultipartMessage.pool.Obtain));
        base.RegisterSerializer<GameLiftMultipartMessage, GameLiftMessageReceivedAcknowledge>(3U, networkPacketSerializer, new Func<GameLiftMultipartMessage>(GameLiftMultipartMessage.pool.Obtain), new Func<GameLiftMessageReceivedAcknowledge>(GameLiftMessageReceivedAcknowledge.pool.Obtain));
    }

    // Token: 0x060000F2 RID: 242 RVA: 0x0000538D File Offset: 0x0000358D
    protected override bool ShouldHandleMessage(IUnconnectedMessage packet, UnconnectedMessageHandler.MessageOrigin origin)
    {
        return base.ShouldHandleMessage(packet, origin) || packet is AuthenticateGameLiftUserResponse;
    }

    // Token: 0x060000F3 RID: 243 RVA: 0x000053A4 File Offset: 0x000035A4
    protected override uint GetMessageType(IUnconnectedMessage message)
    {
        if (message is IGameLiftMessage)
        {
            return 3U;
        }
        return base.GetMessageType(message);
    }

    // Token: 0x060000F4 RID: 244 RVA: 0x000053B7 File Offset: 0x000035B7
    protected override Task<IUnconnectedAuthenticateRequest> GetAuthenticationRequest()
    {
        return Task.FromResult<IUnconnectedAuthenticateRequest>(AuthenticateGameLiftUserRequest.pool.Obtain().Init(this._userId, this._userName, this._playerSessionId));
    }

    // Token: 0x060000F5 RID: 245 RVA: 0x000053DF File Offset: 0x000035DF
    public Task AuthenticateWithGameLiftServer(string userId, string userName, string playerSessionId)
    {
        this._userId = userId;
        this._userName = userName;
        this._playerSessionId = playerSessionId;
        return base.AuthenticateWithServerAsync();
    }

    // Token: 0x0400009F RID: 159
    private string _userId;

    // Token: 0x040000A0 RID: 160
    private string _userName;

    // Token: 0x040000A1 RID: 161
    private string _playerSessionId;
}
