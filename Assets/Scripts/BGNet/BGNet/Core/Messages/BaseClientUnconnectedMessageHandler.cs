using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using BGNet.Logging;
using LiteNetLib.Utils;
using Org.BouncyCastle.Security.Certificates;

namespace BGNet.Core.Messages
{
    // Token: 0x020000A6 RID: 166
    public abstract class BaseClientUnconnectedMessageHandler : UnconnectedMessageHandler
    {
        // Token: 0x17000101 RID: 257
        // (get) Token: 0x0600062C RID: 1580 RVA: 0x000109FF File Offset: 0x0000EBFF
        public global::DnsEndPoint endPoint
        {
            get
            {
                return this._endPoint;
            }
        }

        // Token: 0x17000102 RID: 258
        // (get) Token: 0x0600062D RID: 1581 RVA: 0x00010A08 File Offset: 0x0000EC08
        public bool isAuthenticated
        {
            get
            {
                return this._authenticationTask != null && this._authenticationTask.IsCompleted && !this._authenticationTask.IsFaulted && !this._authenticationTask.IsFaulted && base.IsConnectionStateEncrypted(this.endPoint.endPoint);
            }
        }

        // Token: 0x17000103 RID: 259
        // (get) Token: 0x0600062E RID: 1582 RVA: 0x00010A57 File Offset: 0x0000EC57
        public bool isAuthenticating
        {
            get
            {
                return this._authenticationTask != null && !this._authenticationTask.IsCompleted && !this._authenticationTask.IsFaulted && !this._authenticationTask.IsCanceled;
            }
        }

        // Token: 0x17000104 RID: 260
        // (get) Token: 0x0600062F RID: 1583 RVA: 0x00010A8B File Offset: 0x0000EC8B
        public bool hasAuthenticationFailed
        {
            get
            {
                return this._authenticationException != null;
            }
        }

        // Token: 0x06000630 RID: 1584 RVA: 0x00010A98 File Offset: 0x0000EC98
        protected BaseClientUnconnectedMessageHandler(IUnconnectedMessageSender sender, global::DnsEndPoint endPoint, ITimeProvider timeProvider, ITaskUtility taskUtility, ICertificateValidator certificateValidator, IAnalyticsManager analytics = null) : base(sender, timeProvider, taskUtility, analytics)
        {
            this.RegisterHandshakeMessageHandlers();
            this._endPoint = endPoint;
            this._certificateValidator = certificateValidator;
            base.encryptionLayer.SetUnencryptedTrafficFilter(UnconnectedMessageHandler.CreateHandshakeHeader(sender.unconnectedPacketHeader));
        }

        // Token: 0x06000631 RID: 1585 RVA: 0x00010AE7 File Offset: 0x0000ECE7
        public override void Dispose()
        {
            this._disposed = true;
            base.Dispose();
        }

        // Token: 0x06000632 RID: 1586 RVA: 0x00010AF6 File Offset: 0x0000ECF6
        protected override uint GetMessageType(IUnconnectedMessage message)
        {
            if (message is IHandshakeMessage)
            {
                return 3192347326U;
            }
            return base.GetMessageType(message);
        }

        // Token: 0x06000633 RID: 1587 RVA: 0x00010B0D File Offset: 0x0000ED0D
        protected override bool ShouldHandleMessage(IUnconnectedMessage packet, UnconnectedMessageHandler.MessageOrigin origin)
        {
            return base.ShouldHandleMessage(packet, origin) || packet is IHandshakeServerToClientMessage;
        }

        // Token: 0x06000634 RID: 1588 RVA: 0x00010B24 File Offset: 0x0000ED24
        private void RegisterHandshakeMessageHandlers()
        {
            NetworkPacketSerializer<HandshakeMessageType, UnconnectedMessageHandler.MessageOrigin> networkPacketSerializer = new NetworkPacketSerializer<HandshakeMessageType, UnconnectedMessageHandler.MessageOrigin>();
            networkPacketSerializer.RegisterCallback<ClientHelloRequest>(HandshakeMessageType.ClientHelloRequest, new Action<ClientHelloRequest, UnconnectedMessageHandler.MessageOrigin>(base.DefaultRequestHandler<ClientHelloRequest>), base.ObtainVersioned<ClientHelloRequest>(new Func<ClientHelloRequest>(ClientHelloRequest.pool.Obtain)));
            networkPacketSerializer.RegisterCallback<HelloVerifyRequest>(HandshakeMessageType.HelloVerifyRequest, new Action<HelloVerifyRequest, UnconnectedMessageHandler.MessageOrigin>(this.HandleHelloVerifyRequest), base.ObtainVersioned<HelloVerifyRequest>(new Func<HelloVerifyRequest>(HelloVerifyRequest.pool.Obtain)));
            networkPacketSerializer.RegisterCallback<ClientHelloWithCookieRequest>(HandshakeMessageType.ClientHelloWithCookieRequest, new Action<ClientHelloWithCookieRequest, UnconnectedMessageHandler.MessageOrigin>(base.DefaultRequestHandler<ClientHelloWithCookieRequest>), base.ObtainVersioned<ClientHelloWithCookieRequest>(new Func<ClientHelloWithCookieRequest>(ClientHelloWithCookieRequest.pool.Obtain)));
            networkPacketSerializer.RegisterCallback<ServerHelloRequest>(HandshakeMessageType.ServerHelloRequest, new Action<ServerHelloRequest, UnconnectedMessageHandler.MessageOrigin>(base.DefaultResponseHandler<ServerHelloRequest>), base.ObtainVersioned<ServerHelloRequest>(new Func<ServerHelloRequest>(ServerHelloRequest.pool.Obtain)));
            networkPacketSerializer.RegisterCallback<ServerCertificateRequest>(HandshakeMessageType.ServerCertificateRequest, new Action<ServerCertificateRequest, UnconnectedMessageHandler.MessageOrigin>(base.DefaultResponseHandler<ServerCertificateRequest>), base.ObtainVersioned<ServerCertificateRequest>(new Func<ServerCertificateRequest>(ServerCertificateRequest.pool.Obtain)));
            networkPacketSerializer.RegisterCallback<ClientKeyExchangeRequest>(HandshakeMessageType.ClientKeyExchangeRequest, new Action<ClientKeyExchangeRequest, UnconnectedMessageHandler.MessageOrigin>(base.DefaultResponseHandler<ClientKeyExchangeRequest>), base.ObtainVersioned<ClientKeyExchangeRequest>(new Func<ClientKeyExchangeRequest>(ClientKeyExchangeRequest.pool.Obtain)));
            networkPacketSerializer.RegisterCallback<ChangeCipherSpecRequest>(HandshakeMessageType.ChangeCipherSpecRequest, new Action<ChangeCipherSpecRequest, UnconnectedMessageHandler.MessageOrigin>(base.DefaultResponseHandler<ChangeCipherSpecRequest>), base.ObtainVersioned<ChangeCipherSpecRequest>(new Func<ChangeCipherSpecRequest>(ChangeCipherSpecRequest.pool.Obtain)));
            networkPacketSerializer.RegisterCallback<HandshakeMessageReceivedAcknowledge>(HandshakeMessageType.MessageReceivedAcknowledge, new Action<HandshakeMessageReceivedAcknowledge, UnconnectedMessageHandler.MessageOrigin>(base.DefaultAcknowledgeHandler<HandshakeMessageReceivedAcknowledge>), base.ObtainVersioned<HandshakeMessageReceivedAcknowledge>(new Func<HandshakeMessageReceivedAcknowledge>(HandshakeMessageReceivedAcknowledge.pool.Obtain)));
            networkPacketSerializer.RegisterCallback<HandshakeMultipartMessage>(HandshakeMessageType.MultipartMessage, new Action<HandshakeMultipartMessage, UnconnectedMessageHandler.MessageOrigin>(base.DefaultMultipartMessageHandler<HandshakeMultipartMessage>), base.ObtainVersioned<HandshakeMultipartMessage>(new Func<HandshakeMultipartMessage>(HandshakeMultipartMessage.pool.Obtain)));
            base.RegisterSerializer<HandshakeMultipartMessage, HandshakeMessageReceivedAcknowledge>(3192347326U, networkPacketSerializer, new Func<HandshakeMultipartMessage>(HandshakeMultipartMessage.pool.Obtain), new Func<HandshakeMessageReceivedAcknowledge>(HandshakeMessageReceivedAcknowledge.pool.Obtain));
        }

        // Token: 0x06000635 RID: 1589 RVA: 0x00010CE0 File Offset: 0x0000EEE0
        private void HandleHelloVerifyRequest(HelloVerifyRequest packet, UnconnectedMessageHandler.MessageOrigin origin)
        {
            IAnalyticsManager analytics = this.analytics;
            if (analytics != null)
            {
                analytics.ReceivedUnreliableResponseEvent(packet);
            }
            if (!base.CompleteRequest(packet, origin.endPoint))
            {
                packet.Release();
            }
        }

        // Token: 0x06000636 RID: 1590 RVA: 0x00010D0C File Offset: 0x0000EF0C
        protected async void SendOrderedAuthenticatedRequest(string queue, IUnconnectedReliableRequest message, CancellationToken cancellationToken = default(CancellationToken))
        {
            Task completedTask;
            if (!this._orderedRequests.TryGetValue(queue, out completedTask))
            {
                completedTask = Task.CompletedTask;
            }
            Task task = this.SendOrderedAuthenticatedRequestAsync(completedTask, message, cancellationToken);
            this._orderedRequests[queue] = task;
            await task;
            Task task2;
            if (this._orderedRequests.TryGetValue(queue, out task2) && task2 == task)
            {
                this._orderedRequests.Remove(queue);
            }
        }

        // Token: 0x06000637 RID: 1591 RVA: 0x00010D60 File Offset: 0x0000EF60
        private async Task SendOrderedAuthenticatedRequestAsync(Task previousTask, IUnconnectedReliableRequest message, CancellationToken cancellationToken = default(CancellationToken))
        {
            await previousTask;
            try
            {
                await this.SendAuthenticatedRequestAsync(message, cancellationToken);
            }
            catch (TimeoutException)
            {
            }
            catch (TaskCanceledException)
            {
            }
            catch (AuthenticationException)
            {
            }
            catch (SocketException)
            {
            }
            catch (Exception exception)
            {
                BGNet.Logging.Debug.LogException(exception, "Exception sending message");
            }
        }

        // Token: 0x06000638 RID: 1592 RVA: 0x00010DC0 File Offset: 0x0000EFC0
        protected async Task<T> SendAuthenticatedRequestAsync<T>(IUnconnectedReliableRequest message, CancellationToken cancellationToken = default(CancellationToken)) where T : IUnconnectedReliableResponse
        {
            await this.AuthenticateWithServerAsync();
            return await base.SendReliableRequestAndAwaitResponseAsync<T>(8U, this.endPoint.endPoint, message, new Func<uint, IPEndPoint, IUnconnectedReliableRequest, CancellationToken, Task<T>>(this.OnSendFailedAwaitResponse<T>), cancellationToken);
        }

        // Token: 0x06000639 RID: 1593 RVA: 0x00010E18 File Offset: 0x0000F018
        protected async Task SendAuthenticatedRequestAsync(IUnconnectedReliableRequest message, CancellationToken cancellationToken = default(CancellationToken))
        {
            await this.AuthenticateWithServerAsync();
            await base.SendReliableRequestAsync(8U, this.endPoint.endPoint, message, new Func<uint, IPEndPoint, IUnconnectedReliableRequest, CancellationToken, Task>(this.OnSendFailed), cancellationToken);
        }

        // Token: 0x0600063A RID: 1594 RVA: 0x00010E70 File Offset: 0x0000F070
        private async Task OnSendFailed(uint protocolVersion, IPEndPoint remoteEndPoint, IUnconnectedReliableRequest message, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                await this.ReauthenticateWithServer();
            }
            catch (Exception)
            {
                message.Release();
                throw;
            }
            await base.SendReliableRequestAsync(protocolVersion, remoteEndPoint, message, null, cancellationToken);
        }

        // Token: 0x0600063B RID: 1595 RVA: 0x00010ED8 File Offset: 0x0000F0D8
        private async Task<T> OnSendFailedAwaitResponse<T>(uint protocolVersion, IPEndPoint remoteEndPoint, IUnconnectedReliableRequest message, CancellationToken cancellationToken = default(CancellationToken)) where T : IUnconnectedReliableResponse
        {
            try
            {
                await this.ReauthenticateWithServer();
            }
            catch (Exception)
            {
                message.Release();
                throw;
            }
            return await base.SendReliableRequestAndAwaitResponseAsync<T>(protocolVersion, remoteEndPoint, message, null, cancellationToken);
        }

        // Token: 0x0600063C RID: 1596 RVA: 0x00010F40 File Offset: 0x0000F140
        private async Task ReauthenticateWithServer()
        {
            if (this._disposed)
            {
                throw new TaskCanceledException("MessageHandler Disposed");
            }
            this.UnauthenticateWithServer();
            await this.AuthenticateWithServerAsync();
        }

        // Token: 0x0600063D RID: 1597 RVA: 0x00010F85 File Offset: 0x0000F185
        protected void UnauthenticateWithServer()
        {
            this._authenticationTask = null;
            if (this._endPoint.endPoint != null)
            {
                base.encryptionLayer.RemoveEncryptedEndpoint(this._endPoint.endPoint, null);
            }
        }

        // Token: 0x0600063E RID: 1598 RVA: 0x00010FB4 File Offset: 0x0000F1B4
        protected Task AuthenticateWithServerAsync()
        {
            if (this._authenticationException != null)
            {
                return Task.FromException(this._authenticationException);
            }
            if (this._authenticationTask == null || this._authenticationTask.IsCanceled || this._authenticationTask.IsFaulted)
            {
                this._authenticationTask = this.AuthenticateWithServerAsyncInternal();
            }
            return this._authenticationTask;
        }

        // Token: 0x0600063F RID: 1599 RVA: 0x0001100C File Offset: 0x0000F20C
        protected async void AuthenticateWithServer()
        {
            if (!this.isAuthenticated && !this.isAuthenticating)
            {
                try
                {
                    await this.AuthenticateWithServerAsync();
                }
                catch (Exception)
                {
                }
            }
        }

        // Token: 0x06000640 RID: 1600 RVA: 0x00011048 File Offset: 0x0000F248
        private async Task AuthenticateWithServerAsyncInternalVerbose()
        {
            try
            {
                BGNet.Logging.Debug.Log(string.Format("[{0}] Authenticating with Server {1}", base.GetType().Name, this._endPoint));
                await this.AuthenticateWithServerAsyncInternal();
                BGNet.Logging.Debug.Log(string.Format("[{0}] Successfully Authenticated with Server {1}", base.GetType().Name, this._endPoint));
            }
            catch (Exception exception)
            {
                BGNet.Logging.Debug.LogException(exception, string.Format("[{0}] Exception Authenticating with Server {1}", base.GetType().Name, this._endPoint));
                throw;
            }
        }

        // Token: 0x06000641 RID: 1601 RVA: 0x00011090 File Offset: 0x0000F290
        private async Task AuthenticateWithServerAsyncInternal()
        {
            BaseClientUnconnectedMessageHandler unconnectedMessageHandler = this;
            IPEndPoint endPoint = await unconnectedMessageHandler._endPoint.GetEndPointAsync(unconnectedMessageHandler._taskUtility);
            IUnconnectedAuthenticateRequest authenticationRequest = await unconnectedMessageHandler.GetAuthenticationRequest();
            byte[] clientRandom = SecureRandomProvider.GetBytes(32);
            Task<IDiffieHellmanKeyPair> generateKeysTask = DiffieHellmanUtility.GenerateKeysAsync(unconnectedMessageHandler._taskUtility);
            unconnectedMessageHandler.BeginSession(endPoint);
            HelloVerifyRequest helloVerifyRequest = await unconnectedMessageHandler.SendReliableRequestAndAwaitResponseAsync<HelloVerifyRequest>(8U, endPoint, (IUnconnectedReliableRequest)ClientHelloRequest.pool.Obtain().Init(clientRandom), (Func<uint, IPEndPoint, IUnconnectedReliableRequest, CancellationToken, Task<HelloVerifyRequest>>)((protocolVersion, ep, request, ct) =>
            {
                this.BeginSession(ep);
                return this.SendReliableRequestAndAwaitResponseAsync<HelloVerifyRequest>(protocolVersion, ep, request, cancellationToken: ct);
            }));
            byte[] cookie = (byte[])helloVerifyRequest.cookie;
            helloVerifyRequest.Release();
            uint nextRequestId = unconnectedMessageHandler.GetNextRequestId(endPoint);
            Task<ServerCertificateRequest> serverCertificateTask = unconnectedMessageHandler.AwaitResponseAsync<ServerCertificateRequest>(8U, endPoint, nextRequestId);
            ServerHelloRequest serverHelloRequest = await unconnectedMessageHandler.SendReliableRequestAndAwaitResponseAsync<ServerHelloRequest>(8U, endPoint, (IUnconnectedReliableRequest)ClientHelloWithCookieRequest.pool.Obtain().Init(nextRequestId, clientRandom, cookie));
            byte[] serverRandom = (byte[])serverHelloRequest.random;
            byte[] signature = (byte[])serverHelloRequest.signature;
            byte[] serverPublicKey = (byte[])serverHelloRequest.publicKey;
            uint responseId = serverHelloRequest.requestId;
            serverHelloRequest.Release();
            Task<byte[]> getPreMasterSecretTask = unconnectedMessageHandler._taskUtility.ContinueWith<IDiffieHellmanKeyPair, byte[]>(generateKeysTask, (Func<Task<IDiffieHellmanKeyPair>, Task<byte[]>>)(async result => await result.Result.GetPreMasterSecretAsync(this._taskUtility, serverPublicKey)));
            ServerCertificateRequest certificateRequest = await serverCertificateTask;
            byte[][] array = certificateRequest.certificateList.ToArray<byte[]>();
            certificateRequest.Release();
            Task<bool> verifySignatureTask = unconnectedMessageHandler.VerifySignature(clientRandom, serverRandom, serverPublicKey, signature, array);
            IDiffieHellmanKeyPair clientKeys = await generateKeysTask;
            if (!await verifySignatureTask)
                throw new CertificateException("Signature sent by server doesn't match!");
            ChangeCipherSpecRequest cipherSpecRequest = await unconnectedMessageHandler.SendReliableResponseAndAwaitResponseAsync<ChangeCipherSpecRequest>(8U, endPoint, responseId, (IUnconnectedReliableResponse)ClientKeyExchangeRequest.pool.Obtain().Init(clientKeys.publicKey));
            responseId = cipherSpecRequest.requestId;
            cipherSpecRequest.Release();
            byte[] preMasterSecret = await getPreMasterSecretTask;
            EncryptionUtility.IEncryptionState encryptionState = await unconnectedMessageHandler.encryptionLayer.AddEncryptedEndpointAsync(endPoint, preMasterSecret, serverRandom, clientRandom, true);
            unconnectedMessageHandler.GetConnectionState(endPoint).SetEncryptionState(encryptionState);
            try
            {
                if (!(await unconnectedMessageHandler.SendReliableResponseAndAwaitResponseAsync<IUnconnectedAuthenticateResponse>(8U, endPoint, responseId, (IUnconnectedReliableResponse)authenticationRequest)).isAuthenticated)
                {
                    unconnectedMessageHandler._authenticationException = new AuthenticationException("Failed to Authenticate with Server");
                    unconnectedMessageHandler.encryptionLayer.RemoveEncryptedEndpoint(endPoint);
                    throw unconnectedMessageHandler._authenticationException;
                }
            }
            catch (Exception)
            {
                unconnectedMessageHandler.encryptionLayer.RemoveEncryptedEndpoint(endPoint, encryptionState);
                throw;
            }
        }

        // Token: 0x06000642 RID: 1602
        protected abstract Task<IUnconnectedAuthenticateRequest> GetAuthenticationRequest();

        // Token: 0x06000643 RID: 1603 RVA: 0x000110D5 File Offset: 0x0000F2D5
        protected override bool ShouldHandleMessageFromEndPoint(IPEndPoint endPoint)
        {
            return endPoint.Equals(this._endPoint.endPoint);
        }

        // Token: 0x06000644 RID: 1604 RVA: 0x000110E8 File Offset: 0x0000F2E8
        private Task<bool> VerifySignature(byte[] clientRandom, byte[] serverRandom, byte[] serverKey, byte[] signature, byte[][] certData)
        {
            return this._taskUtility.Run<bool>(delegate ()
            {
                if (BaseClientUnconnectedMessageHandler._authenticationDataWriter == null)
                {
                    BaseClientUnconnectedMessageHandler._authenticationDataWriter = new NetDataWriter();
                }
                bool result;
                try
                {
                    BaseClientUnconnectedMessageHandler._authenticationDataWriter.Put(clientRandom);
                    BaseClientUnconnectedMessageHandler._authenticationDataWriter.Put(serverRandom);
                    BaseClientUnconnectedMessageHandler._authenticationDataWriter.Put(serverKey);
                    using (X509Certificate2 x509Certificate = new X509Certificate2(certData[0]))
                    {
                        this._certificateValidator.ValidateCertificateChain(this.endPoint, x509Certificate, certData);
                        using (RSA rsapublicKey = x509Certificate.GetRSAPublicKey())
                        {
                            result = rsapublicKey.VerifyData(BaseClientUnconnectedMessageHandler._authenticationDataWriter.Data, 0, BaseClientUnconnectedMessageHandler._authenticationDataWriter.Length, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                        }
                    }
                }
                finally
                {
                    BaseClientUnconnectedMessageHandler._authenticationDataWriter.Reset();
                }
                return result;
            }, default(CancellationToken));
        }

        // Token: 0x06000645 RID: 1605 RVA: 0x00011147 File Offset: 0x0000F347
        [Conditional("DEBUG_HANDSHAKE_VERBOSE")]
        private void HandshakeLog(string message)
        {
            BGNet.Logging.Debug.Log("[Handshake] " + message);
        }

        // Token: 0x04000273 RID: 627
        private readonly global::DnsEndPoint _endPoint;

        // Token: 0x04000274 RID: 628
        private readonly ICertificateValidator _certificateValidator;

        // Token: 0x04000275 RID: 629
        [ThreadStatic]
        private static NetDataWriter _authenticationDataWriter;

        // Token: 0x04000276 RID: 630
        private Task _authenticationTask;

        // Token: 0x04000277 RID: 631
        private AuthenticationException _authenticationException;

        // Token: 0x04000278 RID: 632
        private bool _disposed;

        // Token: 0x04000279 RID: 633
        private readonly Dictionary<string, Task> _orderedRequests = new Dictionary<string, Task>();
    }
}
