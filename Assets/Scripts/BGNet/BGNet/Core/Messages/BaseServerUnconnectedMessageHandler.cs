using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using BGNet.Logging;
using LiteNetLib.Utils;

namespace BGNet.Core.Messages
{
    // Token: 0x020000AB RID: 171
    public abstract class BaseServerUnconnectedMessageHandler : UnconnectedMessageHandler
    {
        // Token: 0x06000670 RID: 1648 RVA: 0x000113EC File Offset: 0x0000F5EC
        public BaseServerUnconnectedMessageHandler(IUnconnectedMessageSender sender, ITimeProvider timeProvider, ITaskUtility taskUtility, IAsyncComputeManager asyncCompute, IAnalyticsManager analytics, ICertificateEncryptionProvider certificateEncryptionProvider, IEnumerable<X509Certificate2> certificateList) : base(sender, timeProvider, taskUtility, analytics)
        {
            this.RegisterHandshakeMessageHandlers();
            this._certificateEncryptionProvider = certificateEncryptionProvider;
            this._certificateChain = (from cert in certificateList
                                      select cert.Export(X509ContentType.Cert)).ToArray<byte[]>();
            this._hmacKey = SecureRandomProvider.GetBytes(64);
            this._serverKeys = DiffieHellmanUtility.GenerateKeys(DiffieHellmanUtility.KeyType.ElipticalCurve);
            this._lastServerKeyPairRequestTime = DateTime.UtcNow.Ticks;
            this._asyncCompute = asyncCompute;
            base.encryptionLayer.SetUnencryptedTrafficFilter(UnconnectedMessageHandler.CreateHandshakeHeader(sender.unconnectedPacketHeader));
        }

        // Token: 0x06000671 RID: 1649 RVA: 0x000114A3 File Offset: 0x0000F6A3
        public override void PollUpdate()
        {
            base.PollUpdate();
            this.RotateServerKeys();
        }

        // Token: 0x06000672 RID: 1650 RVA: 0x00010AF6 File Offset: 0x0000ECF6
        protected override uint GetMessageType(IUnconnectedMessage message)
        {
            if (message is IHandshakeMessage)
            {
                return 3192347326U;
            }
            return base.GetMessageType(message);
        }

        // Token: 0x06000673 RID: 1651 RVA: 0x000114B4 File Offset: 0x0000F6B4
        protected override bool ShouldHandleMessage(IUnconnectedMessage packet, UnconnectedMessageHandler.MessageOrigin origin)
        {
            if (base.ShouldHandleMessage(packet, origin))
            {
                return true;
            }
            ClientHelloWithCookieRequest clientHelloWithCookieRequest;
            if ((clientHelloWithCookieRequest = (packet as ClientHelloWithCookieRequest)) == null)
            {
                return packet is IHandshakeClientToServerMessage;
            }
            if (!this.GetCookie(origin.endPoint, clientHelloWithCookieRequest.random).SequenceEqual(clientHelloWithCookieRequest.cookie.data))
            {
                return false;
            }
            base.BeginSession(origin.endPoint, clientHelloWithCookieRequest.requestId);
            return true;
        }

        // Token: 0x06000674 RID: 1652 RVA: 0x00011520 File Offset: 0x0000F720
        private void RegisterHandshakeMessageHandlers()
        {
            NetworkPacketSerializer<HandshakeMessageType, UnconnectedMessageHandler.MessageOrigin> networkPacketSerializer = new NetworkPacketSerializer<HandshakeMessageType, UnconnectedMessageHandler.MessageOrigin>();
            networkPacketSerializer.RegisterCallback<ClientHelloRequest>(HandshakeMessageType.ClientHelloRequest, new Action<ClientHelloRequest, UnconnectedMessageHandler.MessageOrigin>(this.HandleClientHelloRequest), base.ObtainVersioned<ClientHelloRequest>(new Func<ClientHelloRequest>(ClientHelloRequest.pool.Obtain)));
            networkPacketSerializer.RegisterCallback<HelloVerifyRequest>(HandshakeMessageType.HelloVerifyRequest, new Action<HelloVerifyRequest, UnconnectedMessageHandler.MessageOrigin>(base.DefaultRequestHandler<HelloVerifyRequest>), base.ObtainVersioned<HelloVerifyRequest>(new Func<HelloVerifyRequest>(HelloVerifyRequest.pool.Obtain)));
            networkPacketSerializer.RegisterCallback<ClientHelloWithCookieRequest>(HandshakeMessageType.ClientHelloWithCookieRequest, base.CustomResponseHandler<ClientHelloWithCookieRequest>(new Action<ClientHelloWithCookieRequest, UnconnectedMessageHandler.MessageOrigin>(this.HandleClientHelloWithCookieRequest)), base.ObtainVersioned<ClientHelloWithCookieRequest>(new Func<ClientHelloWithCookieRequest>(ClientHelloWithCookieRequest.pool.Obtain)));
            networkPacketSerializer.RegisterCallback<ServerHelloRequest>(HandshakeMessageType.ServerHelloRequest, new Action<ServerHelloRequest, UnconnectedMessageHandler.MessageOrigin>(base.DefaultResponseHandler<ServerHelloRequest>), base.ObtainVersioned<ServerHelloRequest>(new Func<ServerHelloRequest>(ServerHelloRequest.pool.Obtain)));
            networkPacketSerializer.RegisterCallback<ServerCertificateRequest>(HandshakeMessageType.ServerCertificateRequest, new Action<ServerCertificateRequest, UnconnectedMessageHandler.MessageOrigin>(base.DefaultResponseHandler<ServerCertificateRequest>), base.ObtainVersioned<ServerCertificateRequest>(new Func<ServerCertificateRequest>(ServerCertificateRequest.pool.Obtain)));
            networkPacketSerializer.RegisterCallback<ClientKeyExchangeRequest>(HandshakeMessageType.ClientKeyExchangeRequest, new Action<ClientKeyExchangeRequest, UnconnectedMessageHandler.MessageOrigin>(base.DefaultResponseHandler<ClientKeyExchangeRequest>), base.ObtainVersioned<ClientKeyExchangeRequest>(new Func<ClientKeyExchangeRequest>(ClientKeyExchangeRequest.pool.Obtain)));
            networkPacketSerializer.RegisterCallback<ChangeCipherSpecRequest>(HandshakeMessageType.ChangeCipherSpecRequest, new Action<ChangeCipherSpecRequest, UnconnectedMessageHandler.MessageOrigin>(base.DefaultResponseHandler<ChangeCipherSpecRequest>), base.ObtainVersioned<ChangeCipherSpecRequest>(new Func<ChangeCipherSpecRequest>(ChangeCipherSpecRequest.pool.Obtain)));
            networkPacketSerializer.RegisterCallback<HandshakeMessageReceivedAcknowledge>(HandshakeMessageType.MessageReceivedAcknowledge, new Action<HandshakeMessageReceivedAcknowledge, UnconnectedMessageHandler.MessageOrigin>(base.DefaultAcknowledgeHandler<HandshakeMessageReceivedAcknowledge>), base.ObtainVersioned<HandshakeMessageReceivedAcknowledge>(new Func<HandshakeMessageReceivedAcknowledge>(HandshakeMessageReceivedAcknowledge.pool.Obtain)));
            networkPacketSerializer.RegisterCallback<HandshakeMultipartMessage>(HandshakeMessageType.MultipartMessage, new Action<HandshakeMultipartMessage, UnconnectedMessageHandler.MessageOrigin>(base.DefaultMultipartMessageHandler<HandshakeMultipartMessage>), base.ObtainVersioned<HandshakeMultipartMessage>(new Func<HandshakeMultipartMessage>(HandshakeMultipartMessage.pool.Obtain)));
            base.RegisterSerializer<HandshakeMultipartMessage, HandshakeMessageReceivedAcknowledge>(3192347326U, networkPacketSerializer, new Func<HandshakeMultipartMessage>(HandshakeMultipartMessage.pool.Obtain), new Func<HandshakeMessageReceivedAcknowledge>(HandshakeMessageReceivedAcknowledge.pool.Obtain));
        }

        // Token: 0x06000675 RID: 1653 RVA: 0x000116E4 File Offset: 0x0000F8E4
        protected void HandleClientHelloRequest(ClientHelloRequest packet, UnconnectedMessageHandler.MessageOrigin origin)
        {
            if (!base.IsValidSessionStartRequestId(origin.endPoint, packet.requestId))
            {
                packet.Release();
                return;
            }
            byte[] cookie = this.GetCookie(origin.endPoint, packet.random);
            base.SendUnreliableResponse(origin.protocolVersion, origin.endPoint, packet, HelloVerifyRequest.pool.Obtain().Init(cookie));
        }

        // Token: 0x06000676 RID: 1654 RVA: 0x00011742 File Offset: 0x0000F942
        protected void HandleClientHelloWithCookieRequest(ClientHelloWithCookieRequest packet, UnconnectedMessageHandler.MessageOrigin origin)
        {
            this.StartServerAuthenticationFlow(origin.protocolVersion, origin.endPoint, packet.requestId, packet.certificateResponseId, packet.random);
            packet.Release();
        }

        // Token: 0x06000677 RID: 1655 RVA: 0x00011774 File Offset: 0x0000F974
        private async void StartServerAuthenticationFlow(uint protocolVersion, IPEndPoint endPoint, uint requestId, uint certificateResponseId, byte[] clientRandom)
        {
            try
            {
                await this.StartServerAuthenticationFlowAsync(protocolVersion, endPoint, requestId, certificateResponseId, clientRandom);
            }
            catch (TimeoutException)
            {
            }
            catch (TaskCanceledException)
            {
            }
            catch (Exception exception)
            {
                BGNet.Logging.Debug.LogException(exception, "Exception thrown handing server authentication");
            }
        }

        // Token: 0x06000678 RID: 1656
        protected abstract Task VerifyAuthenticationRequest(uint protocolVersion, IPEndPoint endPoint, IUnconnectedAuthenticateRequest authenticateRequest);

        // Token: 0x06000679 RID: 1657 RVA: 0x000117D8 File Offset: 0x0000F9D8
        private async Task StartServerAuthenticationFlowAsync(uint protocolVersion, IPEndPoint endPoint, uint requestId, uint certificateResponseId, byte[] clientRandom)
        {
            byte[] serverRandom = SecureRandomProvider.GetBytes(32);
            IDiffieHellmanKeyPair serverKeys = this._serverKeys;
            Task<byte[]> signatureAsync = this.GetSignatureAsync(clientRandom, serverRandom, serverKeys.publicKey);
            base.SendReliableResponse(protocolVersion, endPoint, certificateResponseId, ServerCertificateRequest.pool.Obtain().Init(this._certificateChain), default(CancellationToken));
            byte[] signature = await signatureAsync;
            ClientKeyExchangeRequest clientKeyExchangeRequest = await base.SendReliableResponseAndAwaitResponseAsync<ClientKeyExchangeRequest>(protocolVersion, endPoint, requestId, ServerHelloRequest.pool.Obtain().Init(serverRandom, serverKeys.publicKey, signature), default(CancellationToken));
            uint responseId = clientKeyExchangeRequest.requestId;
            byte[] clientKey = clientKeyExchangeRequest.clientPublicKey;
            clientKeyExchangeRequest.Release();
            byte[] preMasterSecret = await this.GetPreMasterSecretAsync(serverKeys, clientKey);
            EncryptionUtility.IEncryptionState encryptionState = await base.encryptionLayer.AddEncryptedEndpointAsync(endPoint, preMasterSecret, serverRandom, clientRandom, false);
            base.GetConnectionState(endPoint).SetEncryptionState(encryptionState);
            try
            {
                await this.VerifyAuthenticationRequest(protocolVersion, endPoint, await base.SendReliableResponseAndAwaitResponseAsync<IUnconnectedAuthenticateRequest>(protocolVersion, endPoint, responseId, ChangeCipherSpecRequest.pool.Obtain(), default(CancellationToken)));
            }
            catch (Exception arg)
            {
                BGNet.Logging.Debug.LogWarning(string.Format("Authentication Failure: {0}", arg));
                base.encryptionLayer.RemoveEncryptedEndpoint(endPoint, encryptionState);
            }
        }

        // Token: 0x0600067A RID: 1658 RVA: 0x00011848 File Offset: 0x0000FA48
        private byte[] GetCookie(IPEndPoint endPoint, byte[] random)
        {
            byte[] result;
            try
            {
                this._cookieWriter.Put(endPoint);
                this._cookieWriter.Put(random);
                using (HMACSHA256 hmacsha = new HMACSHA256(this._hmacKey))
                {
                    result = hmacsha.ComputeHash(this._cookieWriter.Data, 0, this._cookieWriter.Length);
                }
            }
            finally
            {
                this._cookieWriter.Reset();
            }
            return result;
        }

        // Token: 0x0600067B RID: 1659 RVA: 0x000118CC File Offset: 0x0000FACC
        private async Task<byte[]> GetSignatureAsync(byte[] clientRandom, byte[] serverRandom, byte[] serverKey)
        {
            BaseServerUnconnectedMessageHandler.SigningComputeOperation signingRequest = new BaseServerUnconnectedMessageHandler.SigningComputeOperation(clientRandom, serverRandom, serverKey, this._certificateEncryptionProvider, this._signatureWriter);
            byte[] result;
            try
            {
                result = await this._asyncCompute.BeginOperation<byte[]>(signingRequest);
            }
            finally
            {
                IAnalyticsManager analytics = this.analytics;
                if (analytics != null)
                {
                    analytics.UpdateAverage("GetSignatureTime", signingRequest.elapsedTime * 1000.0, AnalyticsMetricUnit.Milliseconds, false);
                }
            }
            return result;
        }

        // Token: 0x0600067C RID: 1660 RVA: 0x0001192C File Offset: 0x0000FB2C
        private async Task<byte[]> GetPreMasterSecretAsync(IDiffieHellmanKeyPair serverKeys, byte[] clientKey)
        {
            BaseServerUnconnectedMessageHandler.GetPreMasterSecretComputeOperation preMasterSecretRequest = new BaseServerUnconnectedMessageHandler.GetPreMasterSecretComputeOperation(serverKeys, clientKey);
            byte[] result;
            try
            {
                result = await this._asyncCompute.BeginOperation<byte[]>(preMasterSecretRequest);
            }
            finally
            {
                IAnalyticsManager analytics = this.analytics;
                if (analytics != null)
                {
                    analytics.UpdateAverage("GetPreMasterSecretTime", preMasterSecretRequest.elapsedTime * 1000.0, AnalyticsMetricUnit.Milliseconds, false);
                }
            }
            return result;
        }

        // Token: 0x0600067D RID: 1661 RVA: 0x00011984 File Offset: 0x0000FB84
        private void RotateServerKeys()
        {
            if (DateTime.UtcNow.Ticks < this._lastServerKeyPairRequestTime + (long)(-1294967296))
            {
                return;
            }
            this._lastServerKeyPairRequestTime = DateTime.UtcNow.Ticks;
            if (this._serverKeyRotationTask != null && !this._serverKeyRotationTask.IsCompleted)
            {
                BGNet.Logging.Debug.LogWarning("Rotating Server Keys is taking too long, postponing next rotation!");
                return;
            }
            this._serverKeyRotationTask = this.RotateServerKeysAsync();
        }

        // Token: 0x0600067E RID: 1662 RVA: 0x000119F0 File Offset: 0x0000FBF0
        private async Task RotateServerKeysAsync()
        {
            try
            {
                IDiffieHellmanKeyPair serverKeys = await DiffieHellmanUtility.GenerateKeysAsync(this._taskUtility, default(CancellationToken), DiffieHellmanUtility.KeyType.ElipticalCurve);
                this._serverKeys = serverKeys;
            }
            catch (Exception exception)
            {
                BGNet.Logging.Debug.LogException(exception, "Failed To Rotate Server Keys");
            }
        }

        // Token: 0x0600067F RID: 1663 RVA: 0x00011A35 File Offset: 0x0000FC35
        [Conditional("DEBUG_HANDSHAKE_VERBOSE")]
        private void HandshakeLog(string message)
        {
            BGNet.Logging.Debug.Log("[Handshake] " + message);
        }

        // Token: 0x04000285 RID: 645
        private const long kServerKeyExpirationLength = 3000000000L;

        // Token: 0x04000286 RID: 646
        private readonly ICertificateEncryptionProvider _certificateEncryptionProvider;

        // Token: 0x04000287 RID: 647
        private readonly byte[][] _certificateChain;

        // Token: 0x04000288 RID: 648
        private readonly NetDataWriter _cookieWriter = new NetDataWriter();

        // Token: 0x04000289 RID: 649
        private readonly NetDataWriter _signatureWriter = new NetDataWriter();

        // Token: 0x0400028A RID: 650
        private readonly IAsyncComputeManager _asyncCompute;

        // Token: 0x0400028B RID: 651
        private long _lastServerKeyPairRequestTime;

        // Token: 0x0400028C RID: 652
        private IDiffieHellmanKeyPair _serverKeys;

        // Token: 0x0400028D RID: 653
        private Task _serverKeyRotationTask;

        // Token: 0x0400028E RID: 654
        private readonly byte[] _hmacKey;

        // Token: 0x02000175 RID: 373
        private class SigningComputeOperation : AsyncComputeOperation<byte[]>
        {
            // Token: 0x060008BE RID: 2238 RVA: 0x00018068 File Offset: 0x00016268
            public SigningComputeOperation(byte[] clientRandom, byte[] serverRandom, byte[] serverKey, ICertificateEncryptionProvider certificateEncryptionProvider, NetDataWriter writer) : base(15000)
            {
                this._clientRandom = clientRandom;
                this._serverRandom = serverRandom;
                this._serverKey = serverKey;
                this._certificateEncryptionProvider = certificateEncryptionProvider;
                this._writer = writer;
            }

            // Token: 0x060008BF RID: 2239 RVA: 0x0001809C File Offset: 0x0001629C
            protected override byte[] Compute()
            {
                this._writer.Put(this._clientRandom);
                this._writer.Put(this._serverRandom);
                this._writer.Put(this._serverKey);
                return this._certificateEncryptionProvider.SignData(this._writer.Data, 0, this._writer.Length);
            }

            // Token: 0x060008C0 RID: 2240 RVA: 0x000180FE File Offset: 0x000162FE
            protected override void Finally()
            {
                this._writer.Reset();
            }

            // Token: 0x040004F0 RID: 1264
            private readonly byte[] _clientRandom;

            // Token: 0x040004F1 RID: 1265
            private readonly byte[] _serverRandom;

            // Token: 0x040004F2 RID: 1266
            private readonly byte[] _serverKey;

            // Token: 0x040004F3 RID: 1267
            private readonly ICertificateEncryptionProvider _certificateEncryptionProvider;

            // Token: 0x040004F4 RID: 1268
            private readonly NetDataWriter _writer;
        }

        // Token: 0x02000176 RID: 374
        private class GetPreMasterSecretComputeOperation : AsyncComputeOperation<byte[]>
        {
            // Token: 0x060008C1 RID: 2241 RVA: 0x0001810B File Offset: 0x0001630B
            public GetPreMasterSecretComputeOperation(IDiffieHellmanKeyPair serverKey, byte[] clientKey) : base(15000)
            {
                this._serverKey = serverKey;
                this._clientKey = clientKey;
            }

            // Token: 0x060008C2 RID: 2242 RVA: 0x00018126 File Offset: 0x00016326
            protected override byte[] Compute()
            {
                return this._serverKey.GetPreMasterSecret(this._clientKey);
            }

            // Token: 0x040004F5 RID: 1269
            private readonly IDiffieHellmanKeyPair _serverKey;

            // Token: 0x040004F6 RID: 1270
            private readonly byte[] _clientKey;
        }
    }
}
