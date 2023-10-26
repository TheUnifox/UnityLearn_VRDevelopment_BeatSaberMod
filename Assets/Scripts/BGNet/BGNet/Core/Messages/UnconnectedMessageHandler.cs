using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using BGNet.Logging;
using LiteNetLib.Utils;
using Org.BouncyCastle.Asn1.Crmf;

namespace BGNet.Core.Messages
{
    // Token: 0x020000C2 RID: 194
    public abstract class UnconnectedMessageHandler : IUnconnectedMessageReceiver, IPollable, IDisposable
    {
        // Token: 0x17000127 RID: 295
        // (get) Token: 0x060006BC RID: 1724 RVA: 0x00012050 File Offset: 0x00010250
        protected PacketEncryptionLayer encryptionLayer
        {
            get
            {
                return this._sender.encryptionLayer;
            }
        }

        // Token: 0x17000128 RID: 296
        // (get) Token: 0x060006BD RID: 1725 RVA: 0x0001205D File Offset: 0x0001025D
        protected ITimeProvider timeProvider
        {
            get
            {
                return this._timeProvider;
            }
        }

        // Token: 0x060006BE RID: 1726 RVA: 0x00012068 File Offset: 0x00010268
        protected UnconnectedMessageHandler(IUnconnectedMessageSender sender, ITimeProvider timeProvider, ITaskUtility taskUtility, IAnalyticsManager analytics = null)
        {
            this._sender = sender;
            this._timeProvider = timeProvider;
            this._taskUtility = taskUtility;
            this._connectionStates = new ExpiringDictionary<IPEndPoint, UnconnectedMessageHandler.ConnectionState>(timeProvider, 300000L);
            this._sentRequestWaiters = new ExpiringDictionary<UnconnectedMessageHandler.RequestWaiterId, UnconnectedMessageHandler.SentRequestWaiter>(timeProvider, 5000L);
            this._requestResponseWaiters = new ExpiringDictionary<UnconnectedMessageHandler.RequestWaiterId, UnconnectedMessageHandler.RequestResponseWaiter>(timeProvider, 15000L);
            this._multipartMessageBuffer = new ExpiringDictionary<UnconnectedMessageHandler.RequestWaiterId, UnconnectedMessageHandler.MultipartMessageWaiter>(timeProvider, 15000L);
            this.analytics = analytics;
            this._sender.RegisterReceiver(this);
        }

        // Token: 0x060006BF RID: 1727 RVA: 0x00012139 File Offset: 0x00010339
        protected void RegisterSerializer<TMultipartMessage, TAcknowledgeMessage>(uint messageType, INetworkPacketSerializer<UnconnectedMessageHandler.MessageOrigin> serializer, Func<TMultipartMessage> obtainMultipartMessage, Func<TAcknowledgeMessage> obtainAcknowledgeMessage) where TMultipartMessage : BaseMultipartMessage where TAcknowledgeMessage : BaseAcknowledgeMessage
        {
            this._serializers.Add(messageType, serializer);
            this._multipartMessageRegistry.Add(messageType, new Func<BaseMultipartMessage>(obtainMultipartMessage.Invoke));
            this._acknowledgeMessageRegistery.Add(messageType, new Func<BaseAcknowledgeMessage>(obtainAcknowledgeMessage.Invoke));
        }

        // Token: 0x060006C0 RID: 1728 RVA: 0x00012179 File Offset: 0x00010379
        protected virtual bool ShouldHandleMessage(IUnconnectedMessage packet, UnconnectedMessageHandler.MessageOrigin origin)
        {
            return packet is IUnconnectedMultipartMessage;
        }

        // Token: 0x060006C1 RID: 1729 RVA: 0x00012186 File Offset: 0x00010386
        protected virtual uint GetMessageType(IUnconnectedMessage message)
        {
            throw new Exception(string.Format("Cannot write message of unknown type: {0}", message));
        }

        // Token: 0x060006C2 RID: 1730 RVA: 0x00012198 File Offset: 0x00010398
        private BaseMultipartMessage GetMultipartMessage(IUnconnectedMessage message)
        {
            return this._multipartMessageRegistry[this.GetMessageType(message)]();
        }

        // Token: 0x060006C3 RID: 1731 RVA: 0x000121B1 File Offset: 0x000103B1
        private BaseAcknowledgeMessage GetAcknowledgeMessage(IUnconnectedMessage message)
        {
            return this._acknowledgeMessageRegistery[this.GetMessageType(message)]();
        }

        // Token: 0x060006C4 RID: 1732 RVA: 0x000121CA File Offset: 0x000103CA
        protected Func<UnconnectedMessageHandler.MessageOrigin, T> ObtainVersioned<T>(Func<uint, T> obtain)
        {
            return (UnconnectedMessageHandler.MessageOrigin origin) => obtain(origin.protocolVersion);
        }

        // Token: 0x060006C5 RID: 1733 RVA: 0x000121E3 File Offset: 0x000103E3
        protected Func<T> ObtainVersioned<T>(Func<T> obtain)
        {
            return obtain;
        }

        // Token: 0x060006C6 RID: 1734 RVA: 0x000121E6 File Offset: 0x000103E6
        protected void DefaultAcknowledgeHandler<T>(T packet, UnconnectedMessageHandler.MessageOrigin origin) where T : IUnconnectedMessage, IUnconnectedAcknowledgeMessage
        {
            IAnalyticsManager analyticsManager = this.analytics;
            if (analyticsManager != null)
            {
                analyticsManager.ReceivedUnreliableResponseEvent(packet);
            }
            this.CompleteSend(packet, origin.endPoint);
            packet.Release();
        }

        // Token: 0x060006C7 RID: 1735 RVA: 0x0001221E File Offset: 0x0001041E
        protected void DefaultRequestHandler<T>(T packet, UnconnectedMessageHandler.MessageOrigin origin) where T : IUnconnectedMessage
        {
            packet.Release();
        }

        // Token: 0x060006C8 RID: 1736 RVA: 0x0001222D File Offset: 0x0001042D
        protected void DefaultResponseHandler<T>(T packet, UnconnectedMessageHandler.MessageOrigin origin) where T : IUnconnectedReliableResponse
        {
            if (this.IsUnhandledMessage(packet, origin))
            {
                packet.Release();
            }
        }

        // Token: 0x060006C9 RID: 1737 RVA: 0x0001224C File Offset: 0x0001044C
        protected void DefaultMultipartMessageHandler<T>(T packet, UnconnectedMessageHandler.MessageOrigin origin) where T : IUnconnectedMultipartMessage
        {
            if (!this.IsUnhandledMessage(packet, origin))
            {
                return;
            }
            UnconnectedMessageHandler.RequestWaiterId key = new UnconnectedMessageHandler.RequestWaiterId(origin.endPoint, packet.multipartMessageId);
            UnconnectedMessageHandler.MultipartMessageWaiter multipartMessageWaiter = this._multipartMessageBuffer[key];
            if (multipartMessageWaiter == null)
            {
                multipartMessageWaiter = new UnconnectedMessageHandler.MultipartMessageWaiter(this._bufferPool);
                this._multipartMessageBuffer.Add(key, multipartMessageWaiter);
            }
            multipartMessageWaiter.Append(packet);
            packet.Release();
            if (multipartMessageWaiter.isWaiting)
            {
                return;
            }
            this._multipartMessageBuffer.Remove(key);
            this._multipartReader.SetSource(multipartMessageWaiter.data, 0, multipartMessageWaiter.length);
            this.ReceiveUnconnectedMessage(origin.endPoint, this._multipartReader);
            multipartMessageWaiter.Dispose();
        }

        // Token: 0x060006CA RID: 1738 RVA: 0x0001230C File Offset: 0x0001050C
        protected Action<T, UnconnectedMessageHandler.MessageOrigin> CustomResponseHandler<T>(Action<T, UnconnectedMessageHandler.MessageOrigin> customHandler) where T : IUnconnectedReliableRequest
        {
            return delegate (T packet, UnconnectedMessageHandler.MessageOrigin origin)
            {
                if (this.IsUnhandledMessage(packet, origin))
                {
                    customHandler(packet, origin);
                }
            };
        }

        // Token: 0x060006CB RID: 1739 RVA: 0x0001232C File Offset: 0x0001052C
        protected Action<T, UnconnectedMessageHandler.MessageOrigin> CustomUnreliableResponseHandler<T>(Action<T, UnconnectedMessageHandler.MessageOrigin> customHandler) where T : IUnconnectedUnreliableMessage
        {
            return delegate (T packet, UnconnectedMessageHandler.MessageOrigin origin)
            {
                IAnalyticsManager analyticsManager = this.analytics;
                if (analyticsManager != null)
                {
                    analyticsManager.ReceivedUnreliableMessageEvent(packet);
                }
                customHandler(packet, origin);
            };
        }

        // Token: 0x060006CC RID: 1740 RVA: 0x0001234C File Offset: 0x0001054C
        private bool IsUnhandledMessage(IUnconnectedReliableRequest packet, UnconnectedMessageHandler.MessageOrigin origin)
        {
            bool flag = this.ShouldHandleMessage(packet, origin);
            UnconnectedMessageHandler.ConnectionState connectionState;
            flag &= this._connectionStates.TryGetValue(origin.endPoint, out connectionState);
            this.SendUnreliableResponse(origin.protocolVersion, origin.endPoint, packet.requestId, this.GetAcknowledgeMessage(packet).Init(flag));
            if (!flag)
            {
                packet.Release();
                return false;
            }
            if (!connectionState.CanAcceptRequest(packet.requestId))
            {
                packet.Release();
                return false;
            }
            IUnconnectedReliableResponse unconnectedReliableResponse;
            if ((unconnectedReliableResponse = (packet as IUnconnectedReliableResponse)) != null)
            {
                IAnalyticsManager analyticsManager = this.analytics;
                if (analyticsManager != null)
                {
                    analyticsManager.ReceivedReliableResponseEvent(unconnectedReliableResponse);
                }
                return !this.CompleteRequest(unconnectedReliableResponse, origin.endPoint);
            }
            IAnalyticsManager analyticsManager2 = this.analytics;
            if (analyticsManager2 != null)
            {
                analyticsManager2.ReceivedReliableRequestEvent(packet);
            }
            return true;
        }

        // Token: 0x060006CD RID: 1741 RVA: 0x00012400 File Offset: 0x00010600
        private void CompleteSend(IUnconnectedResponse packet, IPEndPoint remoteEndPoint)
        {
            UnconnectedMessageHandler.SentRequestWaiter sentRequestWaiter = this._sentRequestWaiters[new UnconnectedMessageHandler.RequestWaiterId(remoteEndPoint, packet.responseId)];
            IUnconnectedAcknowledgeMessage unconnectedAcknowledgeMessage;
            if ((unconnectedAcknowledgeMessage = (packet as IUnconnectedAcknowledgeMessage)) != null)
            {
                if (sentRequestWaiter != null)
                {
                    sentRequestWaiter.Complete(unconnectedAcknowledgeMessage.messageHandled);
                    return;
                }
            }
            else if (sentRequestWaiter != null)
            {
                sentRequestWaiter.Complete(true);
            }
        }

        // Token: 0x060006CE RID: 1742 RVA: 0x0001244C File Offset: 0x0001064C
        protected bool CompleteRequest(IUnconnectedReliableResponse packet, IPEndPoint remoteEndPoint)
        {
            UnconnectedMessageHandler.RequestResponseWaiter requestResponseWaiter = this._requestResponseWaiters[new UnconnectedMessageHandler.RequestWaiterId(remoteEndPoint, packet.responseId)];
            if (requestResponseWaiter != null)
            {
                this.CompleteSend(packet, remoteEndPoint);
                requestResponseWaiter.Complete(packet);
                return true;
            }
            return false;
        }

        // Token: 0x060006CF RID: 1743 RVA: 0x00012488 File Offset: 0x00010688
        protected async void GetAndSendResponse<TRequest, TResponse>(TRequest request, UnconnectedMessageHandler.MessageOrigin origin, Func<TRequest, UnconnectedMessageHandler.MessageOrigin, Task<TResponse>> tryGetResponse, Func<TResponse> getFailureResponse) where TRequest : IUnconnectedReliableRequest where TResponse : IUnconnectedReliableResponse
        {
            try
            {
                await this.GetAndSendResponseAsync<TRequest, TResponse>(request, origin, tryGetResponse, getFailureResponse);
            }
            catch (Exception exception)
            {
                IAnalyticsManager analyticsManager = this.analytics;
                if (analyticsManager != null)
                {
                    analyticsManager.IncrementCounter("ReceivedMessageException", 1L, AnalyticsMetricUnit.Count);
                }
                this.ReceivedMessageException(origin.endPoint, exception);
            }
        }

        // Token: 0x060006D0 RID: 1744 RVA: 0x000124E4 File Offset: 0x000106E4
        protected async Task GetAndSendResponseAsync<TRequest, TResponse>(TRequest request, UnconnectedMessageHandler.MessageOrigin origin, Func<TRequest, UnconnectedMessageHandler.MessageOrigin, Task<TResponse>> tryGetResponse, Func<TResponse> getFailureResponse) where TRequest : IUnconnectedReliableRequest where TResponse : IUnconnectedReliableResponse
        {
            TResponse response = default(TResponse);
            try
            {
                TResponse tresponse = await tryGetResponse(request, origin);
                response = tresponse;
                if (response == null)
                {
                    response = getFailureResponse();
                }
            }
            catch (Exception)
            {
                response = getFailureResponse();
                throw;
            }
            finally
            {
                this.SendReliableResponse(origin.protocolVersion, origin.endPoint, request, response, default(CancellationToken));
            }
        }

        // Token: 0x060006D1 RID: 1745 RVA: 0x0001254C File Offset: 0x0001074C
        protected async void GetAndSendUnreilableResponse<TRequest, TResponse>(TRequest request, UnconnectedMessageHandler.MessageOrigin origin, Func<TRequest, UnconnectedMessageHandler.MessageOrigin, Task<TResponse>> tryGetResponse, Func<TResponse> getFailureResponse) where TRequest : IUnconnectedUnreliableMessage where TResponse : IUnconnectedUnreliableMessage
        {
            TResponse response = default(TResponse);
            try
            {
                TResponse tresponse = await tryGetResponse(request, origin);
                response = tresponse;
                if (response == null)
                {
                    response = getFailureResponse();
                }
            }
            catch (Exception)
            {
                response = getFailureResponse();
            }
            finally
            {
                request.Release();
                this.SendUnreliableMessage(origin.protocolVersion, origin.endPoint, response);
            }
        }

        // Token: 0x060006D2 RID: 1746 RVA: 0x000125A6 File Offset: 0x000107A6
        protected void SendUnreliableMessage(uint protocolVersion, IPEndPoint remoteEndPoint, IUnconnectedUnreliableMessage message)
        {
            IAnalyticsManager analyticsManager = this.analytics;
            if (analyticsManager != null)
            {
                analyticsManager.SentUnreliableMessageEvent(message);
            }
            this.SendMessage(protocolVersion, remoteEndPoint, message);
        }

        // Token: 0x060006D3 RID: 1747 RVA: 0x000125C4 File Offset: 0x000107C4
        protected void SendUnreliableResponse(uint protocolVersion, IPEndPoint remoteEndPoint, IUnconnectedReliableRequest request, IUnconnectedResponse response)
        {
            uint requestId = request.requestId;
            request.Release();
            this.SendUnreliableResponse(protocolVersion, remoteEndPoint, requestId, response);
        }

        // Token: 0x060006D4 RID: 1748 RVA: 0x000125E9 File Offset: 0x000107E9
        protected void SendUnreliableResponse(uint protocolVersion, IPEndPoint remoteEndPoint, uint responseId, IUnconnectedResponse response)
        {
            IAnalyticsManager analyticsManager = this.analytics;
            if (analyticsManager != null)
            {
                analyticsManager.SentUnreliableResponseEvent(response);
            }
            this.SendMessage(protocolVersion, remoteEndPoint, response.WithResponseId(responseId));
        }

        // Token: 0x060006D5 RID: 1749 RVA: 0x0001260E File Offset: 0x0001080E
        protected void SendReliableRequest(uint protocolVersion, IPEndPoint remoteEndPoint, IUnconnectedReliableRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            IAnalyticsManager analyticsManager = this.analytics;
            if (analyticsManager != null)
            {
                analyticsManager.SentReliableRequestEvent(request);
            }
            this.SendMessageWithRetry(protocolVersion, remoteEndPoint, request.WithRequestId(this.GetNextRequestId(remoteEndPoint)), cancellationToken);
        }

        // Token: 0x060006D6 RID: 1750 RVA: 0x00012639 File Offset: 0x00010839
        protected Task SendReliableRequestAsync(uint protocolVersion, IPEndPoint remoteEndPoint, IUnconnectedReliableRequest request, Func<uint, IPEndPoint, IUnconnectedReliableRequest, CancellationToken, Task> onSendFailed = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            IAnalyticsManager analyticsManager = this.analytics;
            if (analyticsManager != null)
            {
                analyticsManager.SentReliableRequestEvent(request);
            }
            return this.SendMessageWithRetryAsync(protocolVersion, remoteEndPoint, request.WithRequestId(this.GetNextRequestId(remoteEndPoint)), onSendFailed, cancellationToken);
        }

        // Token: 0x060006D7 RID: 1751 RVA: 0x00012668 File Offset: 0x00010868
        protected void SendReliableResponse(uint protocolVersion, IPEndPoint remoteEndPoint, IUnconnectedReliableRequest request, IUnconnectedReliableResponse response, CancellationToken cancellationToken = default(CancellationToken))
        {
            uint requestId = request.requestId;
            request.Release();
            this.SendReliableResponse(protocolVersion, remoteEndPoint, requestId, response, cancellationToken);
        }

        // Token: 0x060006D8 RID: 1752 RVA: 0x0001268F File Offset: 0x0001088F
        protected void SendReliableResponse(uint protocolVersion, IPEndPoint remoteEndPoint, uint responseId, IUnconnectedReliableResponse response, CancellationToken cancellationToken = default(CancellationToken))
        {
            IAnalyticsManager analyticsManager = this.analytics;
            if (analyticsManager != null)
            {
                analyticsManager.SentReliableResponseEvent(response);
            }
            this.SendMessageWithRetry(protocolVersion, remoteEndPoint, response.WithRequestAndResponseId(this.GetNextRequestId(remoteEndPoint), responseId), cancellationToken);
        }

        // Token: 0x060006D9 RID: 1753 RVA: 0x000126C0 File Offset: 0x000108C0
        protected Task SendReliableResponseAsync(uint protocolVersion, IPEndPoint remoteEndPoint, IUnconnectedReliableRequest request, IUnconnectedReliableResponse response, CancellationToken cancellationToken = default(CancellationToken))
        {
            uint requestId = request.requestId;
            request.Release();
            IAnalyticsManager analyticsManager = this.analytics;
            if (analyticsManager != null)
            {
                analyticsManager.SentReliableResponseEvent(response);
            }
            return this.SendMessageWithRetryAsync(protocolVersion, remoteEndPoint, response.WithRequestAndResponseId(this.GetNextRequestId(remoteEndPoint), requestId), null, cancellationToken);
        }

        // Token: 0x060006DA RID: 1754 RVA: 0x00012707 File Offset: 0x00010907
        protected Task<T> SendReliableRequestAndAwaitResponseAsync<T>(uint protocolVersion, IPEndPoint remoteEndPoint, IUnconnectedReliableRequest request, Func<uint, IPEndPoint, IUnconnectedReliableRequest, CancellationToken, Task<T>> onSendFailedAwaitResponse = null, CancellationToken cancellationToken = default(CancellationToken)) where T : IUnconnectedReliableResponse
        {
            IAnalyticsManager analyticsManager = this.analytics;
            if (analyticsManager != null)
            {
                analyticsManager.SentReliableRequestEvent(request);
            }
            return this.SendMessageWithRetryAwaitResponseAsync<T>(protocolVersion, remoteEndPoint, request.WithRequestId(this.GetNextRequestId(remoteEndPoint)), onSendFailedAwaitResponse, cancellationToken);
        }

        // Token: 0x060006DB RID: 1755 RVA: 0x00012734 File Offset: 0x00010934
        protected Task<T> SendReliableResponseAndAwaitResponseAsync<T>(uint protocolVersion, IPEndPoint remoteEndPoint, IUnconnectedReliableRequest request, IUnconnectedReliableResponse response, CancellationToken cancellationToken = default(CancellationToken)) where T : IUnconnectedReliableResponse
        {
            uint requestId = request.requestId;
            request.Release();
            return this.SendReliableResponseAndAwaitResponseAsync<T>(protocolVersion, remoteEndPoint, requestId, response, cancellationToken);
        }

        // Token: 0x060006DC RID: 1756 RVA: 0x0001275B File Offset: 0x0001095B
        protected Task<T> SendReliableResponseAndAwaitResponseAsync<T>(uint protocolVersion, IPEndPoint remoteEndPoint, uint responseId, IUnconnectedReliableResponse response, CancellationToken cancellationToken = default(CancellationToken)) where T : IUnconnectedReliableResponse
        {
            IAnalyticsManager analyticsManager = this.analytics;
            if (analyticsManager != null)
            {
                analyticsManager.SentReliableResponseEvent(response);
            }
            return this.SendMessageWithRetryAwaitResponseAsync<T>(protocolVersion, remoteEndPoint, response.WithRequestAndResponseId(this.GetNextRequestId(remoteEndPoint), responseId), null, cancellationToken);
        }

        // Token: 0x060006DD RID: 1757 RVA: 0x0001278A File Offset: 0x0001098A
        private void SendMessage(uint protocolVersion, IPEndPoint remoteEndPoint, IUnconnectedMessage message)
        {
            this._connectionStates.ResetExpiration(remoteEndPoint);
            this._sender.SendUnconnectedMessage(remoteEndPoint, this.Write(protocolVersion, message));
            message.Release();
        }

        // Token: 0x060006DE RID: 1758 RVA: 0x000127B4 File Offset: 0x000109B4
        private async void SendMessageWithRetry(uint protocolVersion, IPEndPoint remoteEndPoint, IUnconnectedReliableRequest message, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                await this.SendMessageWithRetryAsync(protocolVersion, remoteEndPoint, message, null, cancellationToken);
            }
            catch (TimeoutException)
            {
            }
            catch (TaskCanceledException)
            {
            }
            catch (Exception exception)
            {
                BGNet.Logging.Debug.LogException(exception, "Exception thrown sending message");
            }
        }

        // Token: 0x060006DF RID: 1759 RVA: 0x00012810 File Offset: 0x00010A10
        private Task SendMessageWithRetryAsync(uint protocolVersion, IPEndPoint remoteEndPoint, IUnconnectedReliableRequest message, Func<uint, IPEndPoint, IUnconnectedReliableRequest, CancellationToken, Task> onSendFailed = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            NetDataWriter netDataWriter = this.Write(protocolVersion, message);
            if (netDataWriter.Length > 412)
            {
                return this.SendMultipartMessageWithRetryAsync(protocolVersion, remoteEndPoint, message, netDataWriter, onSendFailed, cancellationToken);
            }
            return this.SendMessageWithRetryAsyncInternal(protocolVersion, remoteEndPoint, message, onSendFailed, cancellationToken);
        }

        // Token: 0x060006E0 RID: 1760 RVA: 0x00012850 File Offset: 0x00010A50
        private async Task SendMultipartMessageWithRetryAsync(uint protocolVersion, IPEndPoint remoteEndPoint, IUnconnectedReliableRequest message, NetDataWriter data, Func<uint, IPEndPoint, IUnconnectedReliableRequest, CancellationToken, Task> onSendFailed, CancellationToken cancellationToken)
        {
            List<IUnconnectedReliableRequest> list = new List<IUnconnectedReliableRequest>();
            uint nextRequestId = this.GetNextRequestId(remoteEndPoint);
            for (int i = 0; i < data.Length; i += 384)
            {
                BaseMultipartMessage multipartMessage = this.GetMultipartMessage(message);
                list.Add(multipartMessage.Init(nextRequestId, data.Data, i, Math.Min(384, data.Length - i), data.Length));
            }
            bool shouldReleaseMessage = true;
            try
            {
                int num = 0;
                Exception obj = null;
                try
                {
                    await Task.WhenAll(from mm in list
                                       select this.SendMessageWithRetryAsyncInternal(protocolVersion, remoteEndPoint, mm.WithRequestId(this.GetNextRequestId(remoteEndPoint)), null, cancellationToken));
                }
                catch (TimeoutException obj1)
                {
                    num = 1;
                    obj = obj1;
                }
                if (num == 1)
                {
                    if (onSendFailed != null)
                    {
                        shouldReleaseMessage = false;
                        await onSendFailed(protocolVersion, remoteEndPoint, message, cancellationToken);
                    }
                    else
                    {
                        Exception ex = obj;
                        if (ex == null)
                        {
                            throw obj;
                        }
                        ExceptionDispatchInfo.Capture(ex).Throw();
                    }
                }
                obj = null;
            }
            finally
            {
                if (shouldReleaseMessage)
                {
                    message.Release();
                }
            }
        }

        // Token: 0x060006E1 RID: 1761 RVA: 0x000128C8 File Offset: 0x00010AC8
        private async Task SendMessageWithRetryAsyncInternal(uint protocolVersion, IPEndPoint remoteEndPoint, IUnconnectedReliableRequest message, Func<uint, IPEndPoint, IUnconnectedReliableRequest, CancellationToken, Task> onSendFailed = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            UnconnectedMessageHandler.SentRequestWaiter sentRequest = new UnconnectedMessageHandler.SentRequestWaiter(this._disposedTokenSource.Token, cancellationToken);
            UnconnectedMessageHandler.RequestWaiterId waiterId = new UnconnectedMessageHandler.RequestWaiterId(remoteEndPoint, message.requestId);
            this._sentRequestWaiters.Add(waiterId, sentRequest);
            bool shouldReleaseMessage = true;
            try
            {
                int num = 0;
                Exception obj = null;
                try
                {
                    int i = 0;
                    while (i <= 5 && sentRequest.isWaiting)
                    {
                        this._sender.SendUnconnectedMessage(remoteEndPoint, this.Write(protocolVersion, message));
                        await Task.WhenAny(new Task[]
                        {
                            sentRequest.task,
                            this.WaitForRetry(i, cancellationToken)
                        });
                        i++;
                    }
                    if (sentRequest.isWaiting)
                    {
                        this._sender.SendUnconnectedMessage(remoteEndPoint, this.Write(protocolVersion, message));
                        await sentRequest.task;
                    }
                }
                catch (TaskCanceledException)
                {
                    sentRequest.Cancel();
                    throw;
                }
                catch (TimeoutException obj1)
                {
                    num = 1;
                    obj = obj1;
                }
                if (num == 1)
                {
                    if (onSendFailed != null)
                    {
                        shouldReleaseMessage = false;
                        await onSendFailed(protocolVersion, remoteEndPoint, message, cancellationToken);
                    }
                    else
                    {
                        Exception ex = obj;
                        if (ex == null)
                        {
                            throw obj;
                        }
                        ExceptionDispatchInfo.Capture(ex).Throw();
                    }
                }
                obj = null;
            }
            finally
            {
                this._sentRequestWaiters.Remove(waiterId);
                if (shouldReleaseMessage)
                {
                    message.Release();
                }
            }
        }

        // Token: 0x060006E2 RID: 1762 RVA: 0x00012938 File Offset: 0x00010B38
        private async Task<T> SendMessageWithRetryAwaitResponseAsync<T>(uint protocolVersion, IPEndPoint remoteEndPoint, IUnconnectedReliableRequest message, Func<uint, IPEndPoint, IUnconnectedReliableRequest, CancellationToken, Task<T>> onSendFailedAwaitResponse = null, CancellationToken cancellationToken = default(CancellationToken)) where T : IUnconnectedMessage
        {
            UnconnectedMessageHandler.RequestResponseWaiter request = new UnconnectedMessageHandler.RequestResponseWaiter(this._disposedTokenSource.Token, cancellationToken);
            UnconnectedMessageHandler.RequestWaiterId waiterId = new UnconnectedMessageHandler.RequestWaiterId(remoteEndPoint, message.requestId);
            this._requestResponseWaiters.Add(waiterId, request);
            try
            {
                await this.SendMessageWithRetryAsync(protocolVersion, remoteEndPoint, message, this.WrapOnSendFailedAwaitResponse<T>(request, onSendFailedAwaitResponse), cancellationToken);
            }
            catch (TaskCanceledException)
            {
                request.Cancel();
            }
            catch (Exception ex)
            {
                request.Fail(ex);
            }
            IUnconnectedMessage unconnectedMessage = await request.task;
            this._requestResponseWaiters.Remove(waiterId);
            IUnconnectedMessage unconnectedMessage2 = unconnectedMessage;
            if (unconnectedMessage2 is T)
            {
                return (T)((object)unconnectedMessage2);
            }
            if (unconnectedMessage != null)
            {
                unconnectedMessage.Release();
            }
            throw new Exception("Received Unexpected response");
        }

        // Token: 0x060006E3 RID: 1763 RVA: 0x000129A8 File Offset: 0x00010BA8
        private Func<uint, IPEndPoint, IUnconnectedReliableRequest, CancellationToken, Task> WrapOnSendFailedAwaitResponse<T>(UnconnectedMessageHandler.RequestResponseWaiter waiter, Func<uint, IPEndPoint, IUnconnectedReliableRequest, CancellationToken, Task<T>> onSendFailed) where T : IUnconnectedMessage
        {
            UnconnectedMessageHandler.c__DisplayClass77_0<T> CS_8__locals1 = new UnconnectedMessageHandler.c__DisplayClass77_0<T>();
            CS_8__locals1.waiter = waiter;
            CS_8__locals1.onSendFailed = onSendFailed;
            if (CS_8__locals1.onSendFailed == null)
			{
                return null;
            }
            return (uint protocolVersion, IPEndPoint remoteEndPoint, IUnconnectedReliableRequest message, CancellationToken cancellationToken) =>
            {
                UnconnectedMessageHandler.c__DisplayClass77_0<T>.WrapOnSendFailedAwaitResponse_b__0_d WrapOnSendFailedAwaitResponse_b__0_d = new();

                WrapOnSendFailedAwaitResponse_b__0_d.m_this = CS_8__locals1;

                WrapOnSendFailedAwaitResponse_b__0_d.protocolVersion = protocolVersion;

                WrapOnSendFailedAwaitResponse_b__0_d.remoteEndPoint = remoteEndPoint;

                WrapOnSendFailedAwaitResponse_b__0_d.message = message;

                WrapOnSendFailedAwaitResponse_b__0_d.cancellationToken = cancellationToken;

                WrapOnSendFailedAwaitResponse_b__0_d.t__builder = AsyncTaskMethodBuilder.Create();

                WrapOnSendFailedAwaitResponse_b__0_d.m_state = -1;
                AsyncTaskMethodBuilder t__builder = WrapOnSendFailedAwaitResponse_b__0_d.t__builder;

                t__builder.Start<UnconnectedMessageHandler.c__DisplayClass77_0<T>.WrapOnSendFailedAwaitResponse_b__0_d>(ref WrapOnSendFailedAwaitResponse_b__0_d);
                return WrapOnSendFailedAwaitResponse_b__0_d.t__builder.Task;
            };
        }

        // Token: 0x060006E4 RID: 1764 RVA: 0x000129E0 File Offset: 0x00010BE0
        protected async Task<T> AwaitResponseAsync<T>(uint protocolVersion, IPEndPoint remoteEndPoint, uint requestId, CancellationToken cancellationToken = default(CancellationToken)) where T : IUnconnectedReliableResponse
        {
            UnconnectedMessageHandler.RequestResponseWaiter requestResponseWaiter = new UnconnectedMessageHandler.RequestResponseWaiter(this._disposedTokenSource.Token, cancellationToken);
            UnconnectedMessageHandler.RequestWaiterId waiterId = new UnconnectedMessageHandler.RequestWaiterId(remoteEndPoint, requestId);
            this._requestResponseWaiters.Add(waiterId, requestResponseWaiter);
            IUnconnectedMessage unconnectedMessage = await requestResponseWaiter.task;
            this._requestResponseWaiters.Remove(waiterId);
            IUnconnectedMessage unconnectedMessage2 = unconnectedMessage;
            if (unconnectedMessage2 is T)
            {
                return (T)((object)unconnectedMessage2);
            }
            if (unconnectedMessage != null)
            {
                unconnectedMessage.Release();
            }
            throw new Exception("Received Unexpected response");
        }

        // Token: 0x060006E5 RID: 1765 RVA: 0x00012A40 File Offset: 0x00010C40
        private Task WaitForRetry(int retryAttempt, CancellationToken cancellationToken)
        {
            int num;
            switch (retryAttempt)
            {
                case 0:
                    num = 200;
                    break;
                case 1:
                    num = 300;
                    break;
                case 2:
                    num = 450;
                    break;
                case 3:
                    num = 600;
                    break;
                case 4:
                    num = 1000;
                    break;
                default:
                    return Task.CompletedTask;
            }
            return this._taskUtility.Delay(TimeSpan.FromMilliseconds((double)num), cancellationToken);
        }

        // Token: 0x060006E6 RID: 1766 RVA: 0x00012AAC File Offset: 0x00010CAC
        private NetDataWriter Write(uint protocolVersion, IUnconnectedMessage message)
        {
            this._dataWriter.Reset();
            uint messageType = this.GetMessageType(message);
            INetworkPacketSerializer<UnconnectedMessageHandler.MessageOrigin> serializer = this.GetSerializer(protocolVersion, messageType, true);
            this._dataWriter.Put(messageType);
            this._dataWriter.PutVarUInt(protocolVersion);
            serializer.SerializePacket(this._dataWriter, message);
            return this._dataWriter;
        }

        // Token: 0x060006E7 RID: 1767 RVA: 0x00012AFF File Offset: 0x00010CFF
        public virtual void PollUpdate()
        {
            this._requestResponseWaiters.PollUpdate();
            this._sentRequestWaiters.PollUpdate();
            this._connectionStates.PollUpdate();
            this.encryptionLayer.PollUpdate();
        }

        // Token: 0x060006E8 RID: 1768 RVA: 0x00012B30 File Offset: 0x00010D30
        public void ReceiveUnconnectedMessage(IPEndPoint remoteEndPoint, NetDataReader reader)
        {
            if (!this.ShouldHandleMessageFromEndPoint(remoteEndPoint))
            {
                return;
            }
            uint messageType;
            uint protocolVersion;
            if (!reader.TryGetUInt(out messageType) || !reader.TryGetVarUInt(out protocolVersion))
            {
                return;
            }
            try
            {
                INetworkPacketSerializer<UnconnectedMessageHandler.MessageOrigin> serializer = this.GetSerializer(protocolVersion, messageType, false);
                if (serializer != null)
                {
                    UnconnectedMessageHandler.MessageOrigin data = new UnconnectedMessageHandler.MessageOrigin(remoteEndPoint, protocolVersion);
                    this._connectionStates.ResetExpiration(remoteEndPoint);
                    serializer.ProcessAllPackets(reader, data);
                }
            }
            catch (Exception exception)
            {
                this.ReceivedMessageException(remoteEndPoint, exception);
            }
        }

        // Token: 0x060006E9 RID: 1769 RVA: 0x00004774 File Offset: 0x00002974
        protected virtual bool ShouldHandleMessageFromEndPoint(IPEndPoint endPoint)
        {
            return true;
        }

        // Token: 0x060006EA RID: 1770 RVA: 0x00012BA8 File Offset: 0x00010DA8
        protected virtual void ReceivedMessageException(IPEndPoint endPoint, Exception exception)
        {
            BGNet.Logging.Debug.LogException(exception, null);
        }

        // Token: 0x060006EB RID: 1771 RVA: 0x00012BB1 File Offset: 0x00010DB1
        protected void BeginSession(IPEndPoint endPoint)
        {
            this.GetConnectionState(endPoint).BeginSession();
        }

        // Token: 0x060006EC RID: 1772 RVA: 0x00012BBF File Offset: 0x00010DBF
        protected void BeginSession(IPEndPoint endPoint, uint requestId)
        {
            this.GetConnectionState(endPoint).BeginSession(requestId);
        }

        // Token: 0x060006ED RID: 1773 RVA: 0x00012BCE File Offset: 0x00010DCE
        protected uint GetNextRequestId(IPEndPoint endPoint)
        {
            return this.GetConnectionState(endPoint).GetNextRequestId();
        }

        // Token: 0x060006EE RID: 1774 RVA: 0x00012BDC File Offset: 0x00010DDC
        protected UnconnectedMessageHandler.ConnectionState GetConnectionState(IPEndPoint endPoint)
        {
            UnconnectedMessageHandler.ConnectionState connectionState;
            if (this._connectionStates.TryGetValue(endPoint, out connectionState))
            {
                return connectionState;
            }
            connectionState = new UnconnectedMessageHandler.ConnectionState();
            this._connectionStates.Add(endPoint, connectionState);
            return connectionState;
        }

        // Token: 0x060006EF RID: 1775 RVA: 0x00012C10 File Offset: 0x00010E10
        protected bool IsValidSessionStartRequestId(IPEndPoint endPoint, uint requestId)
        {
            UnconnectedMessageHandler.ConnectionState connectionState;
            return !this._connectionStates.TryGetValue(endPoint, out connectionState) || connectionState.IsValidSessionStartRequestId(requestId);
        }

        // Token: 0x060006F0 RID: 1776 RVA: 0x00012C38 File Offset: 0x00010E38
        protected bool IsConnectionStateEncrypted(IPEndPoint endPoint)
        {
            UnconnectedMessageHandler.ConnectionState connectionState;
            return endPoint != null && this._connectionStates.TryGetValue(endPoint, out connectionState) && connectionState.isEncrypted;
        }

        // Token: 0x060006F1 RID: 1777 RVA: 0x00012C60 File Offset: 0x00010E60
        private INetworkPacketSerializer<UnconnectedMessageHandler.MessageOrigin> GetSerializer(uint protocolVersion, uint messageType, bool throwOnInvalidVersion = true)
        {
            if (protocolVersion - 6U <= 2U)
            {
                INetworkPacketSerializer<UnconnectedMessageHandler.MessageOrigin> result;
                if (!this._serializers.TryGetValue(messageType, out result))
                {
                    throw new Exception(string.Format("Unknown Message Type {0}", messageType));
                }
                return result;
            }
            else
            {
                if (throwOnInvalidVersion)
                {
                    throw new Exception(string.Format("Unknown Protocol Version {0}", protocolVersion));
                }
                return null;
            }
        }

        // Token: 0x060006F2 RID: 1778 RVA: 0x00012CB8 File Offset: 0x00010EB8
        public virtual void Dispose()
        {
            this._multipartMessageBuffer.Dispose();
            this._sentRequestWaiters.Dispose();
            this._requestResponseWaiters.Dispose();
            this._connectionStates.Dispose();
            this._disposedTokenSource.Cancel();
            this._sender.UnregisterReceiver(this);
        }

        // Token: 0x060006F3 RID: 1779 RVA: 0x00012D08 File Offset: 0x00010F08
        protected static byte[] CreateHandshakeHeader(byte[] packetHeader)
        {
            byte[] array = new byte[packetHeader.Length + 4];
            for (int i = 0; i < packetHeader.Length; i++)
            {
                array[i] = packetHeader[i];
            }
            FastBitConverter.GetBytes(array, packetHeader.Length, 3192347326U);
            return array;
        }

        // Token: 0x040002A4 RID: 676
        protected const uint kProtocolVersion = 8U;

        // Token: 0x040002A5 RID: 677
        public const int kMinSignatureLength = 128;

        // Token: 0x040002A6 RID: 678
        public const int kMaxSignatureLength = 512;

        // Token: 0x040002A7 RID: 679
        private const int kMaxPacketSize = 412;

        // Token: 0x040002A8 RID: 680
        private const int kRetryCount = 5;

        // Token: 0x040002A9 RID: 681
        private const int kRetryDelay0Ms = 200;

        // Token: 0x040002AA RID: 682
        private const int kRetryDelay1Ms = 300;

        // Token: 0x040002AB RID: 683
        private const int kRetryDelay2Ms = 450;

        // Token: 0x040002AC RID: 684
        private const int kRetryDelay3Ms = 600;

        // Token: 0x040002AD RID: 685
        private const int kRetryDelay4Ms = 1000;

        // Token: 0x040002AE RID: 686
        private const long kConnectionStateTimeoutMs = 300000L;

        // Token: 0x040002AF RID: 687
        private const long kSentRequestTimeoutMs = 5000L;

        // Token: 0x040002B0 RID: 688
        private const long kSentRequestWithResponseTimeoutMs = 15000L;

        // Token: 0x040002B1 RID: 689
        private const long kMultipartMessageTimeoutMs = 15000L;

        // Token: 0x040002B2 RID: 690
        private readonly Dictionary<uint, INetworkPacketSerializer<UnconnectedMessageHandler.MessageOrigin>> _serializers = new Dictionary<uint, INetworkPacketSerializer<UnconnectedMessageHandler.MessageOrigin>>();

        // Token: 0x040002B3 RID: 691
        private readonly Dictionary<uint, Func<BaseMultipartMessage>> _multipartMessageRegistry = new Dictionary<uint, Func<BaseMultipartMessage>>();

        // Token: 0x040002B4 RID: 692
        private readonly Dictionary<uint, Func<BaseAcknowledgeMessage>> _acknowledgeMessageRegistery = new Dictionary<uint, Func<BaseAcknowledgeMessage>>();

        // Token: 0x040002B5 RID: 693
        private readonly NetDataWriter _dataWriter = new NetDataWriter();

        // Token: 0x040002B6 RID: 694
        private readonly NetDataReader _multipartReader = new NetDataReader();

        // Token: 0x040002B7 RID: 695
        private readonly IUnconnectedMessageSender _sender;

        // Token: 0x040002B8 RID: 696
        protected readonly IAnalyticsManager analytics;

        // Token: 0x040002B9 RID: 697
        private readonly ITimeProvider _timeProvider;

        // Token: 0x040002BA RID: 698
        protected readonly ITaskUtility _taskUtility;

        // Token: 0x040002BB RID: 699
        private readonly ExpiringDictionary<UnconnectedMessageHandler.RequestWaiterId, UnconnectedMessageHandler.SentRequestWaiter> _sentRequestWaiters;

        // Token: 0x040002BC RID: 700
        private readonly ExpiringDictionary<UnconnectedMessageHandler.RequestWaiterId, UnconnectedMessageHandler.RequestResponseWaiter> _requestResponseWaiters;

        // Token: 0x040002BD RID: 701
        private readonly ExpiringDictionary<IPEndPoint, UnconnectedMessageHandler.ConnectionState> _connectionStates;

        // Token: 0x040002BE RID: 702
        private readonly ExpiringDictionary<UnconnectedMessageHandler.RequestWaiterId, UnconnectedMessageHandler.MultipartMessageWaiter> _multipartMessageBuffer;

        // Token: 0x040002BF RID: 703
        private readonly SmallBufferPool _bufferPool = new SmallBufferPool();

        // Token: 0x040002C0 RID: 704
        private readonly CancellationTokenSource _disposedTokenSource = new CancellationTokenSource();

        // Token: 0x0200017E RID: 382
        protected class ConnectionState : IDisposable
        {
            // Token: 0x1700016D RID: 365
            // (get) Token: 0x060008D8 RID: 2264 RVA: 0x00018B23 File Offset: 0x00016D23
            public bool isEncrypted
            {
                get
                {
                    return this._encryptionState != null && this._encryptionState.isValid;
                }
            }

            // Token: 0x060008D9 RID: 2265 RVA: 0x00018B3A File Offset: 0x00016D3A
            public uint GetNextRequestId()
            {
                this._currentRequestId = (this._currentRequestId + 1U) % 16777216U;
                return this._currentRequestId | this._currentEpoch;
            }

            // Token: 0x060008DA RID: 2266 RVA: 0x00018B60 File Offset: 0x00016D60
            public void BeginSession()
            {
                uint num;
                do
                {
                    num = (uint)((uint)SecureRandomProvider.GetByte() << 24);
                }
                while (num == this._currentEpoch);
                this.SetEpoch(num);
            }

            // Token: 0x060008DB RID: 2267 RVA: 0x00018B88 File Offset: 0x00016D88
            public bool IsValidSessionStartRequestId(uint requestId)
            {
                if ((requestId & 4278190080U) != this._currentEpoch)
                {
                    return true;
                }
                if (this._receivedRequestCount == 0)
                {
                    return true;
                }
                int num = (int)((requestId + 16777216U - this._lastReceivedRequestId) % 16777216U);
                return num > 0 && num < 64;
            }

            // Token: 0x060008DC RID: 2268 RVA: 0x00018BD0 File Offset: 0x00016DD0
            public void BeginSession(uint requestId)
            {
                uint num = requestId & 4278190080U;
                if (this._currentEpoch == num)
                {
                    return;
                }
                this.SetEpoch(num);
            }

            // Token: 0x060008DD RID: 2269 RVA: 0x00018BF6 File Offset: 0x00016DF6
            private void SetEpoch(uint epoch)
            {
                this._currentEpoch = epoch;
                this._currentRequestId = 0U;
                this._lastReceivedRequestIndex = 0;
                this._receivedRequestCount = 0;
                Array.Clear(this._receivedRequest, 0, 64);
            }

            // Token: 0x060008DE RID: 2270 RVA: 0x00018C24 File Offset: 0x00016E24
            public bool CanAcceptRequest(uint requestId)
            {
                uint num = requestId & 4278190080U;
                if (this._currentEpoch != num)
                {
                    return false;
                }
                requestId &= 16777215U;
                int num2 = (int)((requestId + 16777216U - this._lastReceivedRequestId) % 16777216U);
                if (this._receivedRequestCount == 0)
                {
                    num2 = 1;
                }
                if (num2 > 0 && num2 < 64)
                {
                    for (int i = 0; i < num2; i++)
                    {
                        this._lastReceivedRequestIndex = (this._lastReceivedRequestIndex + 1) % 64;
                        this._receivedRequest[this._lastReceivedRequestIndex] = (i == num2 - 1);
                    }
                    this._lastReceivedRequestId = requestId;
                    this._receivedRequestCount++;
                    return true;
                }
                if ((long)num2 < 16777152L)
                {
                    return false;
                }
                int num3 = (int)((long)(this._lastReceivedRequestIndex + 64 + num2) - 16777216L) % 64;
                if (this._receivedRequest[num3])
                {
                    return false;
                }
                this._receivedRequest[num3] = true;
                return true;
            }

            // Token: 0x060008DF RID: 2271 RVA: 0x00018CF6 File Offset: 0x00016EF6
            public void SetEncryptionState(EncryptionUtility.IEncryptionState encryptionState)
            {
                this._encryptionState = encryptionState;
            }

            // Token: 0x060008E0 RID: 2272 RVA: 0x00018CFF File Offset: 0x00016EFF
            public void SetIdentity(uint protocolVersion, string userId, string userName = null)
            {
                this._protocolVersion = protocolVersion;
                this._userId = userId;
                this._userName = userName;
                this._hasIdentity = true;
            }

            // Token: 0x060008E1 RID: 2273 RVA: 0x00018D1D File Offset: 0x00016F1D
            public bool VerifyIdentity(uint protocolVersion, string userId, string userName = null)
            {
                return this._hasIdentity && this._protocolVersion == protocolVersion && this._userId == userId && this._userName == userName && this.isEncrypted;
            }

            // Token: 0x060008E2 RID: 2274 RVA: 0x00018D54 File Offset: 0x00016F54
            public void Dispose()
            {
                this._hasIdentity = false;
                this._userId = null;
                this._userName = null;
                this._encryptionState = null;
            }

            // Token: 0x060008E3 RID: 2275 RVA: 0x00018D72 File Offset: 0x00016F72
            [Conditional("DEBUG_REQUEST_ID")]
            private static void LogD(string message)
            {
                BGNet.Logging.Debug.Log("[ConnectionState] " + message);
            }

            // Token: 0x060008E4 RID: 2276 RVA: 0x00018D84 File Offset: 0x00016F84
            public ConnectionState()
            {
            }

            // Token: 0x0400052B RID: 1323
            private const int kEpochBitOffset = 24;

            // Token: 0x0400052C RID: 1324
            private const uint kRequestIdRange = 16777216U;

            // Token: 0x0400052D RID: 1325
            private const uint kRangeMask = 16777215U;

            // Token: 0x0400052E RID: 1326
            private const uint kEpochMask = 4278190080U;

            // Token: 0x0400052F RID: 1327
            private string _userId;

            // Token: 0x04000530 RID: 1328
            private string _userName;

            // Token: 0x04000531 RID: 1329
            private uint _protocolVersion;

            // Token: 0x04000532 RID: 1330
            private bool _hasIdentity;

            // Token: 0x04000533 RID: 1331
            private EncryptionUtility.IEncryptionState _encryptionState;

            // Token: 0x04000534 RID: 1332
            private const int kRequestBufferLength = 64;

            // Token: 0x04000535 RID: 1333
            private int _lastReceivedRequestIndex;

            // Token: 0x04000536 RID: 1334
            private uint _lastReceivedRequestId;

            // Token: 0x04000537 RID: 1335
            private int _receivedRequestCount;

            // Token: 0x04000538 RID: 1336
            private readonly bool[] _receivedRequest = new bool[64];

            // Token: 0x04000539 RID: 1337
            private uint _currentRequestId;

            // Token: 0x0400053A RID: 1338
            private uint _currentEpoch;
        }

        // Token: 0x0200017F RID: 383
        private struct RequestWaiterId : IEquatable<UnconnectedMessageHandler.RequestWaiterId>
        {
            // Token: 0x060008E5 RID: 2277 RVA: 0x00018D99 File Offset: 0x00016F99
            public RequestWaiterId(IPEndPoint endPoint, uint requestId)
            {
                this.endPoint = endPoint;
                this.requestId = requestId;
            }

            // Token: 0x060008E6 RID: 2278 RVA: 0x00018DA9 File Offset: 0x00016FA9
            public bool Equals(UnconnectedMessageHandler.RequestWaiterId other)
            {
                return object.Equals(this.endPoint, other.endPoint) && this.requestId == other.requestId;
            }

            // Token: 0x060008E7 RID: 2279 RVA: 0x00018DD0 File Offset: 0x00016FD0
            public override bool Equals(object other)
            {
                if (other is UnconnectedMessageHandler.RequestWaiterId)
                {
                    UnconnectedMessageHandler.RequestWaiterId other2 = (UnconnectedMessageHandler.RequestWaiterId)other;
                    return this.Equals(other2);
                }
                return false;
            }

            // Token: 0x060008E8 RID: 2280 RVA: 0x00018DF7 File Offset: 0x00016FF7
            public override int GetHashCode()
            {
                return ((this.endPoint == null) ? 0 : this.endPoint.GetHashCode()) ^ (int)this.requestId;
            }

            // Token: 0x0400053B RID: 1339
            public readonly IPEndPoint endPoint;

            // Token: 0x0400053C RID: 1340
            public readonly uint requestId;
        }

        // Token: 0x02000180 RID: 384
        private abstract class RequestWaiter : IDisposable
        {
            // Token: 0x060008E9 RID: 2281
            public abstract void Dispose();

            // Token: 0x060008EA RID: 2282 RVA: 0x000024B7 File Offset: 0x000006B7
            protected RequestWaiter()
            {
            }
        }

        // Token: 0x02000181 RID: 385
        private sealed class SentRequestWaiter : UnconnectedMessageHandler.RequestWaiter
        {
            // Token: 0x060008EB RID: 2283 RVA: 0x00018E18 File Offset: 0x00017018
            public SentRequestWaiter(CancellationToken disposedCancellationToken, CancellationToken requestCancellationToken)
            {
                this._disposedCancellationTokenRegistration = disposedCancellationToken.Register(new Action(this.Cancel));
                this._requestCancellationTokenRegistration = requestCancellationToken.Register(new Action(this.Cancel));
            }

            // Token: 0x060008EC RID: 2284 RVA: 0x00018E68 File Offset: 0x00017068
            public override void Dispose()
            {
                if (this.isWaiting)
                {
                    this._taskCompletionSource.TrySetException(new TimeoutException());
                }
                this._disposedCancellationTokenRegistration.Dispose();
                this._requestCancellationTokenRegistration.Dispose();
            }

            // Token: 0x060008ED RID: 2285 RVA: 0x00018EAA File Offset: 0x000170AA
            public void Complete(bool handled = true)
            {
                if (handled)
                {
                    this._taskCompletionSource.TrySetResult(true);
                    return;
                }
                this._taskCompletionSource.TrySetException(new Exception("Request unhandled by the remote endpoint"));
            }

            // Token: 0x060008EE RID: 2286 RVA: 0x00018ED3 File Offset: 0x000170D3
            public void Cancel()
            {
                this._taskCompletionSource.TrySetCanceled();
            }

            // Token: 0x1700016E RID: 366
            // (get) Token: 0x060008EF RID: 2287 RVA: 0x00018EE1 File Offset: 0x000170E1
            public Task task
            {
                get
                {
                    return this._taskCompletionSource.Task;
                }
            }

            // Token: 0x1700016F RID: 367
            // (get) Token: 0x060008F0 RID: 2288 RVA: 0x00018EEE File Offset: 0x000170EE
            public bool isWaiting
            {
                get
                {
                    return !this.task.IsCompleted && !this.task.IsCanceled && !this.task.IsFaulted;
                }
            }

            // Token: 0x0400053D RID: 1341
            private readonly TaskCompletionSource<bool> _taskCompletionSource = new TaskCompletionSource<bool>();

            // Token: 0x0400053E RID: 1342
            private readonly CancellationTokenRegistration _disposedCancellationTokenRegistration;

            // Token: 0x0400053F RID: 1343
            private readonly CancellationTokenRegistration _requestCancellationTokenRegistration;
        }

        // Token: 0x02000182 RID: 386
        private sealed class RequestResponseWaiter : UnconnectedMessageHandler.RequestWaiter
        {
            // Token: 0x060008F1 RID: 2289 RVA: 0x00018F1C File Offset: 0x0001711C
            public RequestResponseWaiter(CancellationToken disposedCancellationToken, CancellationToken requestCancellationToken)
            {
                this._disposedCancellationTokenRegistration = disposedCancellationToken.Register(new Action(this.Cancel));
                this._requestCancellationTokenRegistration = requestCancellationToken.Register(new Action(this.Cancel));
            }

            // Token: 0x060008F2 RID: 2290 RVA: 0x00018F6C File Offset: 0x0001716C
            public override void Dispose()
            {
                if (this.isWaiting)
                {
                    this._taskCompletionSource.TrySetException(new TimeoutException());
                }
                this._disposedCancellationTokenRegistration.Dispose();
                this._requestCancellationTokenRegistration.Dispose();
            }

            // Token: 0x060008F3 RID: 2291 RVA: 0x00018FAE File Offset: 0x000171AE
            public void Complete(IUnconnectedMessage response)
            {
                if (!this._taskCompletionSource.TrySetResult(response))
                {
                    response.Release();
                }
            }

            // Token: 0x060008F4 RID: 2292 RVA: 0x00018FC4 File Offset: 0x000171C4
            public void Fail(Exception ex)
            {
                this._taskCompletionSource.TrySetException(ex);
            }

            // Token: 0x060008F5 RID: 2293 RVA: 0x00018FD3 File Offset: 0x000171D3
            public void Cancel()
            {
                this._taskCompletionSource.TrySetCanceled();
            }

            // Token: 0x17000170 RID: 368
            // (get) Token: 0x060008F6 RID: 2294 RVA: 0x00018FE1 File Offset: 0x000171E1
            public Task<IUnconnectedMessage> task
            {
                get
                {
                    return this._taskCompletionSource.Task;
                }
            }

            // Token: 0x17000171 RID: 369
            // (get) Token: 0x060008F7 RID: 2295 RVA: 0x00018FEE File Offset: 0x000171EE
            public bool isWaiting
            {
                get
                {
                    return !this.task.IsCompleted && !this.task.IsCanceled && !this.task.IsFaulted;
                }
            }

            // Token: 0x04000540 RID: 1344
            private readonly TaskCompletionSource<IUnconnectedMessage> _taskCompletionSource = new TaskCompletionSource<IUnconnectedMessage>();

            // Token: 0x04000541 RID: 1345
            private readonly CancellationTokenRegistration _disposedCancellationTokenRegistration;

            // Token: 0x04000542 RID: 1346
            private readonly CancellationTokenRegistration _requestCancellationTokenRegistration;
        }

        // Token: 0x02000183 RID: 387
        private sealed class MultipartMessageWaiter : UnconnectedMessageHandler.RequestWaiter
        {
            // Token: 0x060008F8 RID: 2296 RVA: 0x0001901A File Offset: 0x0001721A
            public MultipartMessageWaiter(SmallBufferPool bufferPool)
            {
                this._bufferPool = bufferPool;
            }

            // Token: 0x060008F9 RID: 2297 RVA: 0x00019034 File Offset: 0x00017234
            public override void Dispose()
            {
                this._isDisposed = true;
                if (this._buffer != null)
                {
                    this._bufferPool.ReleaseBuffer(this._buffer);
                    this._buffer = null;
                }
            }

            // Token: 0x060008FA RID: 2298 RVA: 0x00019060 File Offset: 0x00017260
            public void Append(IUnconnectedMultipartMessage packet)
            {
                if (this._isComplete || this._isDisposed)
                {
                    return;
                }
                if (this._buffer == null)
                {
                    this._length = packet.totalLength;
                    this._buffer = this._bufferPool.GetBuffer(this._length);
                }
                Buffer.BlockCopy(packet.data, 0, this._buffer, packet.offset, packet.length);
                bool flag = false;
                for (int i = 0; i < this._ranges.Count; i++)
                {
                    if (packet.offset < this._ranges[i].Item1)
                    {
                        this._ranges.Insert(i, new ValueTuple<int, int>(packet.offset, packet.length));
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    this._ranges.Add(new ValueTuple<int, int>(packet.offset, packet.length));
                }
                for (int j = this._ranges.Count - 1; j >= 1; j--)
                {
                    ValueTuple<int, int> valueTuple = this._ranges[j];
                    ValueTuple<int, int> valueTuple2 = this._ranges[j - 1];
                    if (valueTuple2.Item1 + valueTuple2.Item2 >= valueTuple.Item1)
                    {
                        int item = valueTuple2.Item1;
                        int num = Math.Max(valueTuple2.Item1 + valueTuple2.Item2, valueTuple.Item1 + valueTuple.Item2);
                        this._ranges[j - 1] = new ValueTuple<int, int>(item, num - item);
                        this._ranges.RemoveAt(j);
                    }
                }
                if (this._ranges.Count == 1)
                {
                    ValueTuple<int, int> valueTuple3 = this._ranges[0];
                    int length = this._length;
                    if (valueTuple3.Item1 == 0 && valueTuple3.Item2 == length)
                    {
                        this._isComplete = true;
                    }
                }
            }

            // Token: 0x17000172 RID: 370
            // (get) Token: 0x060008FB RID: 2299 RVA: 0x0001921B File Offset: 0x0001741B
            public bool isWaiting
            {
                get
                {
                    return !this._isComplete && !this._isDisposed;
                }
            }

            // Token: 0x17000173 RID: 371
            // (get) Token: 0x060008FC RID: 2300 RVA: 0x00019230 File Offset: 0x00017430
            public byte[] data
            {
                get
                {
                    return this._buffer;
                }
            }

            // Token: 0x17000174 RID: 372
            // (get) Token: 0x060008FD RID: 2301 RVA: 0x00019238 File Offset: 0x00017438
            public int length
            {
                get
                {
                    return this._length;
                }
            }

            // Token: 0x04000543 RID: 1347
            private readonly SmallBufferPool _bufferPool;

            // Token: 0x04000544 RID: 1348
            private byte[] _buffer;

            // Token: 0x04000545 RID: 1349
            private int _length;

            // Token: 0x04000546 RID: 1350
            private readonly List<(int offset, int range)> _ranges = new List<(int, int)>();

            // Token: 0x04000547 RID: 1351
            private bool _isComplete;

            // Token: 0x04000548 RID: 1352
            private bool _isDisposed;
        }

        // Token: 0x02000184 RID: 388
        protected readonly struct MessageOrigin
        {
            // Token: 0x060008FE RID: 2302 RVA: 0x00019240 File Offset: 0x00017440
            public MessageOrigin(IPEndPoint endPoint, uint protocolVersion)
            {
                this.endPoint = endPoint;
                this.protocolVersion = protocolVersion;
            }

            // Token: 0x04000549 RID: 1353
            public readonly IPEndPoint endPoint;

            // Token: 0x0400054A RID: 1354
            public readonly uint protocolVersion;
        }

        // Token: 0x02000185 RID: 389
        [CompilerGenerated]
        private sealed class c__DisplayClass46_0<T>
		{
			// Token: 0x060008FF RID: 2303 RVA: 0x000024B7 File Offset: 0x000006B7
			public c__DisplayClass46_0() { }

            // Token: 0x06000900 RID: 2304 RVA: 0x00019250 File Offset: 0x00017450
            internal T ObtainVersioned_b__0(UnconnectedMessageHandler.MessageOrigin origin)
            {
                return this.obtain(origin.protocolVersion);
            }

            // Token: 0x0400054B RID: 1355
            public Func<uint, T> obtain;
        }

        // Token: 0x02000186 RID: 390
        [CompilerGenerated]
        private sealed class c__DisplayClass52_0<T> where T : IUnconnectedReliableRequest
		{
			// Token: 0x06000901 RID: 2305 RVA: 0x000024B7 File Offset: 0x000006B7
			public c__DisplayClass52_0() { }

            // Token: 0x06000902 RID: 2306 RVA: 0x00019263 File Offset: 0x00017463
            internal void CustomResponseHandler_b__0(T packet, UnconnectedMessageHandler.MessageOrigin origin)
            {
                if (this.m_this.IsUnhandledMessage(packet, origin))
				{
                    this.customHandler(packet, origin);
                }
            }

            // Token: 0x0400054C RID: 1356
            public UnconnectedMessageHandler m_this;

            // Token: 0x0400054D RID: 1357
            public Action<T, UnconnectedMessageHandler.MessageOrigin> customHandler;
        }

        // Token: 0x02000187 RID: 391
        [CompilerGenerated]
        private sealed class c__DisplayClass53_0<T> where T : IUnconnectedUnreliableMessage
		{
            // Token: 0x06000903 RID: 2307 RVA: 0x000024B7 File Offset: 0x000006B7
            public c__DisplayClass53_0() { }

            // Token: 0x06000904 RID: 2308 RVA: 0x00019286 File Offset: 0x00017486
            internal void CustomUnreliableResponseHandler_b__0(T packet, UnconnectedMessageHandler.MessageOrigin origin)

            {
                IAnalyticsManager analytics = this.m_this.analytics;
                if (analytics != null)
                {
                    analytics.ReceivedUnreliableMessageEvent(packet);
                }
                this.customHandler(packet, origin);
            }

            // Token: 0x0400054E RID: 1358
            public UnconnectedMessageHandler m_this;

            // Token: 0x0400054F RID: 1359
            public Action<T, UnconnectedMessageHandler.MessageOrigin> customHandler;
		}

		// Token: 0x02000188 RID: 392
		[CompilerGenerated]
        [StructLayout(LayoutKind.Auto)]
        private struct GetAndSendResponse_d__57<TRequest, TResponse> : IAsyncStateMachine where TRequest : IUnconnectedReliableRequest where TResponse : IUnconnectedReliableResponse
		{
			// Token: 0x06000905 RID: 2309 RVA: 0x000192B4 File Offset: 0x000174B4
			void IAsyncStateMachine.MoveNext()
			{
                int num = this.m_state;
                UnconnectedMessageHandler unconnectedMessageHandler = this.m_this;
                try
                {
                    try
                    {
                        TaskAwaiter awaiter;
                        if (num != 0)
                        {
                            awaiter = unconnectedMessageHandler.GetAndSendResponseAsync<TRequest, TResponse>(this.request, this.origin, this.tryGetResponse, this.getFailureResponse).GetAwaiter();
                            if (!awaiter.IsCompleted)
                            {
                                this.m_state = 0;
                                this.u__1 = awaiter;
                                this.t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, UnconnectedMessageHandler.GetAndSendResponse_d__57<TRequest, TResponse>>(ref awaiter, ref this);
                                return;
                            }
                        }
                        else
                        {
                            awaiter = this.u__1;
                            this.u__1 = default(TaskAwaiter);
                            this.m_state = -1;
                        }
                        awaiter.GetResult();
                    }
                    catch (Exception exception)
                    {
                        IAnalyticsManager analytics = unconnectedMessageHandler.analytics;
                        if (analytics != null)
                        {
                            analytics.IncrementCounter("ReceivedMessageException", 1L, AnalyticsMetricUnit.Count);
                        }
                        unconnectedMessageHandler.ReceivedMessageException(this.origin.endPoint, exception);
                    }
                }
                catch (Exception exception2)
                {
                    this.m_state = -2;
                    this.t__builder.SetException(exception2);
                    return;
                }
                this.m_state = -2;
                this.t__builder.SetResult();
            }

            // Token: 0x06000906 RID: 2310 RVA: 0x000193C8 File Offset: 0x000175C8
            [DebuggerHidden]
            void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)

            {
                this.t__builder.SetStateMachine(stateMachine);
            }

            // Token: 0x04000550 RID: 1360
            public int m_state;
            
            // Token: 0x04000551 RID: 1361
            public AsyncVoidMethodBuilder t__builder;
            
            // Token: 0x04000552 RID: 1362
            public UnconnectedMessageHandler m_this;
            
            // Token: 0x04000553 RID: 1363
            public TRequest request;
            
            // Token: 0x04000554 RID: 1364
            public UnconnectedMessageHandler.MessageOrigin origin;
            
            // Token: 0x04000555 RID: 1365
            public Func<TRequest, UnconnectedMessageHandler.MessageOrigin, Task<TResponse>> tryGetResponse;
            
            // Token: 0x04000556 RID: 1366
            public Func<TResponse> getFailureResponse;
            
            // Token: 0x04000557 RID: 1367
            private TaskAwaiter u__1;
		}

		// Token: 0x02000189 RID: 393
		[CompilerGenerated]
        [StructLayout(LayoutKind.Auto)]
        private struct GetAndSendResponseAsync_d__58<TRequest, TResponse> : IAsyncStateMachine where TRequest : IUnconnectedReliableRequest where TResponse : IUnconnectedReliableResponse
		{
			// Token: 0x06000907 RID: 2311 RVA: 0x000193D8 File Offset: 0x000175D8
			void IAsyncStateMachine.MoveNext()
			{
                int num = this.m_state;
                UnconnectedMessageHandler unconnectedMessageHandler = this.m_this;
                try
                {
                    if (num != 0)
                    {
                        this.response_5__2 = default(TResponse);
                    }
                    try
                    {
                        TaskAwaiter<TResponse> awaiter;
                        if (num != 0)
                        {
                            awaiter = this.tryGetResponse(this.request, this.origin).GetAwaiter();
                            if (!awaiter.IsCompleted)
                            {
                                num = (this.m_state = 0);
                                this.u__1 = awaiter;
                                this.t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<TResponse>, UnconnectedMessageHandler.GetAndSendResponseAsync_d__58<TRequest, TResponse>> (ref awaiter, ref this);
                                return;
                            }
                        }
                        else
                        {
                            awaiter = this.u__1;
                            this.u__1 = default(TaskAwaiter<TResponse>);
                            num = (this.m_state = -1);
                        }
                        TResponse result = awaiter.GetResult();
                        this.response_5__2 = result;
                        if (this.response_5__2 == null)
                        {
                            this.response_5__2 = this.getFailureResponse();
                        }
                    }
                    catch (Exception)
                    {
                        this.response_5__2 = this.getFailureResponse();
                        throw;
                    }
                    finally
                    {
                        if (num < 0)
                        {
                            unconnectedMessageHandler.SendReliableResponse(this.origin.protocolVersion, this.origin.endPoint, this.request, this.response_5__2, default(CancellationToken));
                        }
                    }
                }
                catch (Exception exception)
                {
                    this.m_state = -2;
                    this.t__builder.SetException(exception);
                    return;
                }
                this.m_state = -2;
                this.t__builder.SetResult();
            }
            
            // Token: 0x06000908 RID: 2312 RVA: 0x0001956C File Offset: 0x0001776C
            [DebuggerHidden]
            void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
            
                        {
                this.t__builder.SetStateMachine(stateMachine);
            }
            
            // Token: 0x04000558 RID: 1368
            public int m_state;
            
            // Token: 0x04000559 RID: 1369
            public AsyncTaskMethodBuilder t__builder;
            
            // Token: 0x0400055A RID: 1370
            public Func<TRequest, UnconnectedMessageHandler.MessageOrigin, Task<TResponse>> tryGetResponse;
            
            // Token: 0x0400055B RID: 1371
            public TRequest request;
            
            // Token: 0x0400055C RID: 1372
            public UnconnectedMessageHandler.MessageOrigin origin;
            
            // Token: 0x0400055D RID: 1373
            public Func<TResponse> getFailureResponse;
            
            // Token: 0x0400055E RID: 1374
            public UnconnectedMessageHandler m_this;
            
            // Token: 0x0400055F RID: 1375
            private TResponse response_5__2;
            
            // Token: 0x04000560 RID: 1376
            private TaskAwaiter<TResponse> u__1;
		}

		// Token: 0x0200018A RID: 394
		[CompilerGenerated]
        [StructLayout(LayoutKind.Auto)]
        private struct GetAndSendUnreilableResponse_d__59<TRequest, TResponse> : IAsyncStateMachine where TRequest : IUnconnectedUnreliableMessage where TResponse : IUnconnectedUnreliableMessage
		{
			// Token: 0x06000909 RID: 2313 RVA: 0x0001957C File Offset: 0x0001777C
			void IAsyncStateMachine.MoveNext()
			{
                int num = this.m_state;
                UnconnectedMessageHandler unconnectedMessageHandler = this.m_this;
                try
                {
                    if (num != 0)
                    {
                        this.response_5__2 = default(TResponse);
                    }
                    try
                    {
                        TaskAwaiter<TResponse> awaiter;
                        if (num != 0)
                        {
                            awaiter = this.tryGetResponse(this.request, this.origin).GetAwaiter();
                            if (!awaiter.IsCompleted)
                            {
                                num = (this.m_state = 0);
                                this.u__1 = awaiter;
                                this.t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<TResponse>, UnconnectedMessageHandler.GetAndSendUnreilableResponse_d__59<TRequest, TResponse>>(ref awaiter, ref this);
                                return;
                            }
                        }
                        else
                        {
                            awaiter = this.u__1;
                            this.u__1 = default(TaskAwaiter<TResponse>);
                            num = (this.m_state = -1);
                        }
                        TResponse result = awaiter.GetResult();
                        this.response_5__2 = result;
                        if (this.response_5__2 == null)
                        {
                            this.response_5__2 = this.getFailureResponse();
                        }
                    }
                    catch (Exception)
                    {
                        this.response_5__2 = this.getFailureResponse();
                    }
                    finally
                    {
                        if (num < 0)
                        {
                            this.request.Release();
                            unconnectedMessageHandler.SendUnreliableMessage(this.origin.protocolVersion, this.origin.endPoint, this.response_5__2);
                        }
                    }
                }
                catch (Exception exception)
                {
                    this.m_state = -2;
                    this.t__builder.SetException(exception);
                    return;
                }
                this.m_state = -2;
                this.t__builder.SetResult();
            }
            
            // Token: 0x0600090A RID: 2314 RVA: 0x000196E8 File Offset: 0x000178E8
            [DebuggerHidden]
            void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
            
                        {
                this.t__builder.SetStateMachine(stateMachine);
            }
            
            // Token: 0x04000561 RID: 1377
            public int m_state;
            
            // Token: 0x04000562 RID: 1378
            public AsyncVoidMethodBuilder t__builder;
            
            // Token: 0x04000563 RID: 1379
            public Func<TRequest, UnconnectedMessageHandler.MessageOrigin, Task<TResponse>> tryGetResponse;
            
            // Token: 0x04000564 RID: 1380
            public TRequest request;
            
            // Token: 0x04000565 RID: 1381
            public UnconnectedMessageHandler.MessageOrigin origin;
            
            // Token: 0x04000566 RID: 1382
            public Func<TResponse> getFailureResponse;
            
            // Token: 0x04000567 RID: 1383
            public UnconnectedMessageHandler m_this;
            
            // Token: 0x04000568 RID: 1384
            private TResponse response_5__2;
            
            // Token: 0x04000569 RID: 1385
            private TaskAwaiter<TResponse>  u__1;
		}

		// Token: 0x0200018B RID: 395
		[CompilerGenerated]
        [StructLayout(LayoutKind.Auto)]
        private struct SendMessageWithRetry_d__72: IAsyncStateMachine
        {
            // Token: 0x0600090B RID: 2315 RVA: 0x000196F8 File Offset: 0x000178F8
            void IAsyncStateMachine.MoveNext()
            {
                int num = this.m_state;
                UnconnectedMessageHandler unconnectedMessageHandler = this.m_this;
                try
                {
                    try
                    {
                        TaskAwaiter awaiter;
                        if (num != 0)
                        {
                            awaiter = unconnectedMessageHandler.SendMessageWithRetryAsync(this.protocolVersion, this.remoteEndPoint, this.message, null, this.cancellationToken).GetAwaiter();
                            if (!awaiter.IsCompleted)
                            {
                                this.m_state = 0;
                                this.u__1 = awaiter;
                                this.t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, UnconnectedMessageHandler.SendMessageWithRetry_d__72>(ref awaiter, ref this);
                                return;
                            }
                        }
                        else
                        {
                            awaiter = this.u__1;
                            this.u__1 = default(TaskAwaiter);
                            this.m_state = -1;
                        }
                        awaiter.GetResult();
                    }
                    catch (TimeoutException)
                    {
                    }
                    catch (TaskCanceledException)
                    {
                    }
                    catch (Exception exception)
                    {
                        BGNet.Logging.Debug.LogException(exception, "Exception thrown sending message");
                    }
                }
                catch (Exception exception2)
                {
                    this.m_state = -2;
                    this.t__builder.SetException(exception2);
                    return;
                }
                this.m_state = -2;
                this.t__builder.SetResult();
            }
        
            // Token: 0x0600090C RID: 2316 RVA: 0x00019800 File Offset: 0x00017A00
            [DebuggerHidden]
            void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.t__builder.SetStateMachine(stateMachine);
            }
        
            // Token: 0x0400056A RID: 1386
            public int m_state;
        
            // Token: 0x0400056B RID: 1387
            public AsyncVoidMethodBuilder t__builder;
        
            // Token: 0x0400056C RID: 1388
            public UnconnectedMessageHandler m_this;
            
            // Token: 0x0400056D RID: 1389
            public uint protocolVersion;
            
            // Token: 0x0400056E RID: 1390
            public IPEndPoint remoteEndPoint;
            
            // Token: 0x0400056F RID: 1391
            public IUnconnectedReliableRequest message;
            
            // Token: 0x04000570 RID: 1392
            public CancellationToken cancellationToken;
            
            // Token: 0x04000571 RID: 1393
            private TaskAwaiter u__1;
		}

		// Token: 0x0200018C RID: 396
		[CompilerGenerated]
        private sealed class c__DisplayClass74_0
		{
            // Token: 0x0600090D RID: 2317 RVA: 0x000024B7 File Offset: 0x000006B7
            public c__DisplayClass74_0() { }

            // Token: 0x0600090E RID: 2318 RVA: 0x0001980E File Offset: 0x00017A0E
            internal Task SendMultipartMessageWithRetryAsync_b__0(IUnconnectedReliableRequest mm)
            {
                return this.m_this.SendMessageWithRetryAsyncInternal(this.protocolVersion, this.remoteEndPoint, mm.WithRequestId(this.m_this.GetNextRequestId(this.remoteEndPoint)), null, this.cancellationToken);
            }
            
            // Token: 0x04000572 RID: 1394
            public UnconnectedMessageHandler m_this;
            
            // Token: 0x04000573 RID: 1395
            public uint protocolVersion;
            
            // Token: 0x04000574 RID: 1396
            public IPEndPoint remoteEndPoint;
            
            // Token: 0x04000575 RID: 1397
            public CancellationToken cancellationToken;
		}

		// Token: 0x0200018D RID: 397
		[CompilerGenerated]
        [StructLayout(LayoutKind.Auto)]
        private struct SendMultipartMessageWithRetryAsync_d__74: IAsyncStateMachine
        {
            // Token: 0x0600090F RID: 2319 RVA: 0x00019848 File Offset: 0x00017A48
            void IAsyncStateMachine.MoveNext()
        
                    {
                int num = this.m_state;
                UnconnectedMessageHandler unconnectedMessageHandler = this.m_this;
                try
                {
                    List<IUnconnectedReliableRequest> list = new List<IUnconnectedReliableRequest>();
                    if (num > 1)
                    {
                        this.DC74 = new UnconnectedMessageHandler.c__DisplayClass74_0();
                        this.DC74.m_this = this.m_this;
                        this.DC74.protocolVersion = this.protocolVersion;
                        this.DC74.remoteEndPoint = this.remoteEndPoint;
                        this.DC74.cancellationToken = this.cancellationToken;
                        uint nextRequestId = unconnectedMessageHandler.GetNextRequestId(this.DC74.remoteEndPoint);
                        for (int i = 0; i < this.data.Length; i += 384)
                        {
                            BaseMultipartMessage multipartMessage = unconnectedMessageHandler.GetMultipartMessage(this.message);
                            list.Add(multipartMessage.Init(nextRequestId, this.data.Data, i, Math.Min(384, this.data.Length - i), this.data.Length));
                        }
                        this.shouldReleaseMessage_5__2 = true;
                    }
                    try
                    {
                        TaskAwaiter awaiter;
                        if (num != 0)
                        {
                            if (num == 1)
                            {
                                awaiter = this.u__1;
                                this.u__1 = default(TaskAwaiter);
                                num = (this.m_state = -1);
                                goto IL_231;
                            }
                            this.m_wrap3 = 0;
                        }
                        try
                        {
                            if (num != 0)
                            {
                                awaiter = Task.WhenAll(list.Select(new Func<IUnconnectedReliableRequest, Task>(this.DC74.SendMultipartMessageWithRetryAsync_b__0))).GetAwaiter();
                                if (!awaiter.IsCompleted)
                                {
                                    num = (this.m_state = 0);
                                    this.u__1 = awaiter;
                                    this.t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, UnconnectedMessageHandler.SendMultipartMessageWithRetryAsync_d__74>(ref awaiter, ref this);
                                    return;
                                }
                            }
                            else
                            {
                                awaiter = this.u__1;
                                this.u__1 = default(TaskAwaiter);
                                num = (this.m_state = -1);
                            }
                            awaiter.GetResult();
                        }
                        catch (TimeoutException ex)
                        {
                            this.m_wrap2 = ex;
                            this.m_wrap3 = 1;
                        }
                        int num2 = this.m_wrap3;
                        if (num2 != 1)
                        {
                            goto IL_259;
                        }
                        if (this.onSendFailed != null)
                        {
                            this.shouldReleaseMessage_5__2 = false;
                            awaiter = this.onSendFailed(this.DC74.protocolVersion, this.DC74.remoteEndPoint, this.message, this.DC74.cancellationToken).GetAwaiter();
                            if (!awaiter.IsCompleted)
                            {
                                num = (this.m_state = 1);
                                this.u__1 = awaiter;
                                this.t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, UnconnectedMessageHandler.SendMultipartMessageWithRetryAsync_d__74>(ref awaiter, ref this);
                                return;
                            }
                        }
                        else
                        {
                            Exception ex2 = this.m_wrap2;
                            if (ex2 == null)
                            {
                                throw this.m_wrap2;
                            }
                            ExceptionDispatchInfo.Capture(ex2).Throw();
                            goto IL_259;
                        }
                    IL_231:
                        awaiter.GetResult();
                    IL_259:
                        this.m_wrap2 = null;
                    }
                    finally
                    {
                        if (num < 0 && this.shouldReleaseMessage_5__2)
                        {
                            this.message.Release();
                        }
                    }
                }
                catch (Exception exception)
                {
                    this.m_state = -2;
                    this.t__builder.SetException(exception);
                    return;
                }
                this.m_state = -2;
                this.t__builder.SetResult();
            }
        
            // Token: 0x06000910 RID: 2320 RVA: 0x00019B4C File Offset: 0x00017D4C
            [DebuggerHidden]
            void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
        
                    {
                this.t__builder.SetStateMachine(stateMachine);
            }
        
            // Token: 0x04000576 RID: 1398
            public int m_state;
        
            // Token: 0x04000577 RID: 1399
            public AsyncTaskMethodBuilder t__builder;
        
            // Token: 0x04000578 RID: 1400
            public UnconnectedMessageHandler m_this;
            
            // Token: 0x04000579 RID: 1401
            public uint protocolVersion;
            
            // Token: 0x0400057A RID: 1402
            public IPEndPoint remoteEndPoint;
            
            // Token: 0x0400057B RID: 1403
            public CancellationToken cancellationToken;
            
            // Token: 0x0400057C RID: 1404
            public IUnconnectedReliableRequest message;
            
            // Token: 0x0400057D RID: 1405
            public NetDataWriter data;
            
            // Token: 0x0400057E RID: 1406
            public Func<uint, IPEndPoint, IUnconnectedReliableRequest, CancellationToken, Task> onSendFailed;
            
            // Token: 0x0400057F RID: 1407
            private UnconnectedMessageHandler.c__DisplayClass74_0 DC74;
            
            // Token: 0x04000580 RID: 1408
            private bool shouldReleaseMessage_5__2;
            
            // Token: 0x04000581 RID: 1409
            private Exception m_wrap2;
            
            // Token: 0x04000582 RID: 1410
            private int m_wrap3;
            
            // Token: 0x04000583 RID: 1411
            private TaskAwaiter u__1;
		}

		// Token: 0x0200018E RID: 398
		[CompilerGenerated]
        [StructLayout(LayoutKind.Auto)]
        private struct SendMessageWithRetryAsyncInternal_d__75: IAsyncStateMachine
        {
            // Token: 0x06000911 RID: 2321 RVA: 0x00019B5C File Offset: 0x00017D5C
            void IAsyncStateMachine.MoveNext()
        
                    {
                int num = this.m_state;
                UnconnectedMessageHandler unconnectedMessageHandler = this.m_this;
                try
                {
                    if (num > 2)
                    {
                        this.sentRequest_5__2 = new UnconnectedMessageHandler.SentRequestWaiter(unconnectedMessageHandler._disposedTokenSource.Token, this.cancellationToken);
                        this.waiterId_5__3 = new UnconnectedMessageHandler.RequestWaiterId(this.remoteEndPoint, this.message.requestId);
                        unconnectedMessageHandler._sentRequestWaiters.Add(this.waiterId_5__3, this.sentRequest_5__2);
                        this.shouldReleaseMessage_5__4 = true;
                    }
                    try
                    {
                        TaskAwaiter awaiter;
                        if (num > 1)
                        {
                            if (num == 2)
                            {
                                awaiter = this.u__2;
                                this.u__2 = default(TaskAwaiter);
                                num = (this.m_state = -1);
                                goto IL_2B3;
                            }
                            this.m_wrap5 = 0;
                        }
                        int num2;
                        try
                        {
                            TaskAwaiter<Task> awaiter2;
                            if (num != 0)
                            {
                                if (num != 1)
                                {
                                    this.i_5__7 = 0;
                                    goto IL_14D;
                                }
                                awaiter = this.u__2;
                                this.u__2 = default(TaskAwaiter);
                                num = (this.m_state = -1);
                                goto IL_1F5;
                            }
                            else
                            {
                                awaiter2 = this.u__1;
                                this.u__1 = default(TaskAwaiter<Task>);
                                num = (this.m_state = -1);
                            }
                        IL_135:
                            awaiter2.GetResult();
                            num2 = this.i_5__7;
                            this.i_5__7 = num2 + 1;
                        IL_14D:
                            if (this.i_5__7 > 5 || !this.sentRequest_5__2.isWaiting)
                            {
                                if (!this.sentRequest_5__2.isWaiting)
                                {
                                    goto IL_1FC;
                                }
                                unconnectedMessageHandler._sender.SendUnconnectedMessage(this.remoteEndPoint, unconnectedMessageHandler.Write(this.protocolVersion, this.message));
                                awaiter = this.sentRequest_5__2.task.GetAwaiter();
                                if (!awaiter.IsCompleted)
                                {
                                    num = (this.m_state = 1);
                                    this.u__2 = awaiter;
                                    this.t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, UnconnectedMessageHandler.SendMessageWithRetryAsyncInternal_d__75>(ref awaiter, ref this);
                                    return;
                                }
                            }
                            else
                            {
                                unconnectedMessageHandler._sender.SendUnconnectedMessage(this.remoteEndPoint, unconnectedMessageHandler.Write(this.protocolVersion, this.message));
                                awaiter2 = Task.WhenAny(new Task[]
                                {
                                            this.sentRequest_5__2.task,
                                            unconnectedMessageHandler.WaitForRetry(this.i_5__7, this.cancellationToken)
                                }).GetAwaiter();
                                if (!awaiter2.IsCompleted)
                                {
                                    num = (this.m_state = 0);
                                    this.u__1 = awaiter2;
                                    this.t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Task>, UnconnectedMessageHandler.SendMessageWithRetryAsyncInternal_d__75>(ref awaiter2, ref this);
                                    return;
                                }
                                goto IL_135;
                            }
                        IL_1F5:
                            awaiter.GetResult();
                        IL_1FC:;
                        }
                        catch (TaskCanceledException)
                        {
                            this.sentRequest_5__2.Cancel();
                            throw;
                        }
                        catch (TimeoutException ex)
                        {
                            this.m_wrap4 = ex;
                            this.m_wrap5 = 1;
                        }
                        num2 = this.m_wrap5;
                        if (num2 != 1)
                        {
                            goto IL_2DB;
                        }
                        if (this.onSendFailed != null)
                        {
                            this.shouldReleaseMessage_5__4 = false;
                            awaiter = this.onSendFailed(this.protocolVersion, this.remoteEndPoint, this.message, this.cancellationToken).GetAwaiter();
                            if (!awaiter.IsCompleted)
                            {
                                num = (this.m_state = 2);
                                this.u__2 = awaiter;
                                this.t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, UnconnectedMessageHandler.SendMessageWithRetryAsyncInternal_d__75 > (ref awaiter, ref this);
                                return;
                            }
                        }
                        else
                        {
                            Exception ex2 = this.m_wrap4;
                            if (ex2 == null)
                            {
                                throw this.m_wrap4;
                            }
                            ExceptionDispatchInfo.Capture(ex2).Throw();
                            goto IL_2DB;
                        }
                    IL_2B3:
                        awaiter.GetResult();
                    IL_2DB:
                        this.m_wrap4 = null;
                    }
                    finally
                    {
                        if (num < 0)
                        {
                            unconnectedMessageHandler._sentRequestWaiters.Remove(this.waiterId_5__3);
                            if (this.shouldReleaseMessage_5__4)
                            {
                                this.message.Release();
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    this.m_state = -2;
                    this.t__builder.SetException(exception);
                    return;
                }
                this.m_state = -2;
                this.t__builder.SetResult();
            }
        
            // Token: 0x06000912 RID: 2322 RVA: 0x00019F0C File Offset: 0x0001810C
            [DebuggerHidden]
            void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
        
                    {
                this.t__builder.SetStateMachine(stateMachine);
            }
        
            // Token: 0x04000584 RID: 1412
            public int m_state;
        
            // Token: 0x04000585 RID: 1413
            public AsyncTaskMethodBuilder t__builder;
        
            // Token: 0x04000586 RID: 1414
            public UnconnectedMessageHandler m_this;
            
            // Token: 0x04000587 RID: 1415
            public CancellationToken cancellationToken;
            
            // Token: 0x04000588 RID: 1416
            public IPEndPoint remoteEndPoint;
            
            // Token: 0x04000589 RID: 1417
            public IUnconnectedReliableRequest message;
            
            // Token: 0x0400058A RID: 1418
            public uint protocolVersion;
            
            // Token: 0x0400058B RID: 1419
            public Func<uint, IPEndPoint, IUnconnectedReliableRequest, CancellationToken, Task> onSendFailed;
            
            // Token: 0x0400058C RID: 1420
            private UnconnectedMessageHandler.SentRequestWaiter sentRequest_5__2;
            
            // Token: 0x0400058D RID: 1421
            private UnconnectedMessageHandler.RequestWaiterId waiterId_5__3;
            
            // Token: 0x0400058E RID: 1422
            private bool shouldReleaseMessage_5__4;
            
            // Token: 0x0400058F RID: 1423
            private Exception m_wrap4;
            
            // Token: 0x04000590 RID: 1424
            private int m_wrap5;
            
            // Token: 0x04000591 RID: 1425
            private int i_5__7;
            
            // Token: 0x04000592 RID: 1426
            private TaskAwaiter<Task> u__1;
            
            // Token: 0x04000593 RID: 1427
            private TaskAwaiter u__2;
		}

		// Token: 0x0200018F RID: 399
		[CompilerGenerated]
        [StructLayout(LayoutKind.Auto)]
        private struct SendMessageWithRetryAwaitResponseAsync_d__76<T> : IAsyncStateMachine where T : IUnconnectedMessage
		{
			// Token: 0x06000913 RID: 2323 RVA: 0x00019F1C File Offset: 0x0001811C
			void IAsyncStateMachine.MoveNext()
			{
                int num = this.m_state;
                UnconnectedMessageHandler unconnectedMessageHandler = this.m_this;
                T result2;
                try
                {
                    TaskAwaiter<IUnconnectedMessage> awaiter;
                    if (num != 0)
                    {
                        if (num == 1)
                        {
                            awaiter = this.u__2;
                            this.u__2 = default(TaskAwaiter<IUnconnectedMessage>);
                            this.m_state = -1;
                            goto IL_171;
                        }
                        this.request_5__2 = new UnconnectedMessageHandler.RequestResponseWaiter(unconnectedMessageHandler._disposedTokenSource.Token, this.cancellationToken);
                        this.waiterId_5__3 = new UnconnectedMessageHandler.RequestWaiterId(this.remoteEndPoint, this.message.requestId);
                        unconnectedMessageHandler._requestResponseWaiters.Add(this.waiterId_5__3, this.request_5__2);
                    }
                    try
                    {
                        TaskAwaiter awaiter2;
                        if (num != 0)
                        {
                            awaiter2 = unconnectedMessageHandler.SendMessageWithRetryAsync(this.protocolVersion, this.remoteEndPoint, this.message, unconnectedMessageHandler.WrapOnSendFailedAwaitResponse<T>(this.request_5__2, this.onSendFailedAwaitResponse), this.cancellationToken).GetAwaiter();
                            if (!awaiter2.IsCompleted)
                            {
                                this.m_state = 0;
                                this.u__1 = awaiter2;
                                this.t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, UnconnectedMessageHandler.SendMessageWithRetryAwaitResponseAsync_d__76<T>>(ref awaiter2, ref this);
                                return;
                            }
                        }
                        else
                        {
                            awaiter2 = this.u__1;
                            this.u__1 = default(TaskAwaiter);
                            this.m_state = -1;
                        }
                        awaiter2.GetResult();
                    }
                    catch (TaskCanceledException)
                    {
                        this.request_5__2.Cancel();
                    }
                    catch (Exception ex)
                    {
                        this.request_5__2.Fail(ex);
                    }
                    awaiter = this.request_5__2.task.GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        this.m_state = 1;
                        this.u__2 = awaiter;
                        this.t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<IUnconnectedMessage>, UnconnectedMessageHandler.SendMessageWithRetryAwaitResponseAsync_d__76<T>>(ref awaiter, ref this);
                        return;
                    }
                IL_171:
                    IUnconnectedMessage result = awaiter.GetResult();
                    unconnectedMessageHandler._requestResponseWaiters.Remove(this.waiterId_5__3);
                    IUnconnectedMessage unconnectedMessage;
                    if (!((unconnectedMessage = result) is T))
                    {
                        if (result != null)
                        {
                            result.Release();
                        }
                        throw new Exception("Received Unexpected response");
                    }
                    this.tResult_5__4 = (T)((object)unconnectedMessage);
                    result2 = this.tResult_5__4;
                }
                catch (Exception exception)
                {
                    this.m_state = -2;
                    this.t__builder.SetException(exception);
                    return;
                }
                this.m_state = -2;
                this.t__builder.SetResult(result2);
            }
            
            // Token: 0x06000914 RID: 2324 RVA: 0x0001A164 File Offset: 0x00018364
            [DebuggerHidden]
            void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
            
                        {
                this.t__builder.SetStateMachine(stateMachine);
            }
            
            // Token: 0x04000594 RID: 1428
            public int m_state;
            
            // Token: 0x04000595 RID: 1429
            public AsyncTaskMethodBuilder<T> t__builder;
            
            // Token: 0x04000596 RID: 1430
            public UnconnectedMessageHandler m_this;
            
            // Token: 0x04000597 RID: 1431
            public CancellationToken cancellationToken;
            
            // Token: 0x04000598 RID: 1432
            public IPEndPoint remoteEndPoint;
            
            // Token: 0x04000599 RID: 1433
            public IUnconnectedReliableRequest message;
            
            // Token: 0x0400059A RID: 1434
            public uint protocolVersion;
            
            // Token: 0x0400059B RID: 1435
            public Func<uint, IPEndPoint, IUnconnectedReliableRequest, CancellationToken, Task<T>> onSendFailedAwaitResponse;
            
            // Token: 0x0400059C RID: 1436
            private UnconnectedMessageHandler.RequestResponseWaiter request_5__2;
            
            // Token: 0x0400059D RID: 1437
            private UnconnectedMessageHandler.RequestWaiterId waiterId_5__3;
            
            // Token: 0x0400059E RID: 1438
            private T tResult_5__4;
            
            // Token: 0x0400059F RID: 1439
            private TaskAwaiter u__1;
            
            // Token: 0x040005A0 RID: 1440
            private TaskAwaiter<IUnconnectedMessage> u__2;
		}

        // Token: 0x02000190 RID: 400
        [CompilerGenerated]
        private sealed class c__DisplayClass77_0<T> where T : IUnconnectedMessage
        {
            // Token: 0x06000915 RID: 2325 RVA: 0x000024B7 File Offset: 0x000006B7
            public c__DisplayClass77_0() { }

            // Token: 0x06000916 RID: 2326 RVA: 0x0001A174 File Offset: 0x00018374
            internal async Task WrapOnSendFailedAwaitResponse_b__0(uint protocolVersion, IPEndPoint remoteEndPoint, IUnconnectedReliableRequest message, CancellationToken cancellationToken)
            {
                UnconnectedMessageHandler.RequestResponseWaiter requestResponseWaiter = this.waiter;
                T t = await this.onSendFailed(protocolVersion, remoteEndPoint, message, cancellationToken);
                requestResponseWaiter.Complete(t);
                requestResponseWaiter = null;
            }

            // Token: 0x040005A1 RID: 1441
            public UnconnectedMessageHandler.RequestResponseWaiter waiter;

            // Token: 0x040005A2 RID: 1442
            public Func<uint, IPEndPoint, IUnconnectedReliableRequest, CancellationToken, Task<T>> onSendFailed;


            // Token: 0x0200019E RID: 414
            [StructLayout(LayoutKind.Auto)]
            public struct WrapOnSendFailedAwaitResponse_b__0_d : IAsyncStateMachine
            {
                // Token: 0x06000935 RID: 2357 RVA: 0x0001AF58 File Offset: 0x00019158
                void IAsyncStateMachine.MoveNext()
                {
                    int num = this.m_state;
                    UnconnectedMessageHandler.c__DisplayClass77_0<T> CS_8__locals1 = this.m_this;
                    try
                    {
                        TaskAwaiter<T> awaiter;
                        if (num != 0)
                        {
                            this.m_wrap1 = CS_8__locals1.waiter;
                            awaiter = CS_8__locals1.onSendFailed(this.protocolVersion, this.remoteEndPoint, this.message, this.cancellationToken).GetAwaiter();
                            if (!awaiter.IsCompleted)
                            {
                                this.m_state = 0;
                                this.u__1 = awaiter;
                                this.t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<T>, UnconnectedMessageHandler.c__DisplayClass77_0<T>.WrapOnSendFailedAwaitResponse_b__0_d>(ref awaiter, ref this);
                                return;
                            }
                        }
                        else
                        {
                            awaiter = this.u__1;
                            this.u__1 = default(TaskAwaiter<T>);
                            this.m_state = -1;
                        }
                        T result = awaiter.GetResult();
                        this.m_wrap1.Complete(result);
                        this.m_wrap1 = null;
                    }
                    catch (Exception exception)
                    {
                        this.m_state = -2;
                        this.t__builder.SetException(exception);
                        return;
                    }
                    this.m_state = -2;
                    this.t__builder.SetResult();
                }

                // Token: 0x06000936 RID: 2358 RVA: 0x0001B050 File Offset: 0x00019250
                [DebuggerHidden]
                void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)

                {
                    this.t__builder.SetStateMachine(stateMachine);
                }

                // Token: 0x040005E5 RID: 1509
                public int m_state;

                // Token: 0x040005E6 RID: 1510
                public AsyncTaskMethodBuilder t__builder;

                // Token: 0x040005E7 RID: 1511
                public UnconnectedMessageHandler.c__DisplayClass77_0<T> m_this;

                // Token: 0x040005E8 RID: 1512
                public uint protocolVersion;

                // Token: 0x040005E9 RID: 1513
                public IPEndPoint remoteEndPoint;

                // Token: 0x040005EA RID: 1514
                public IUnconnectedReliableRequest message;

                // Token: 0x040005EB RID: 1515
                public CancellationToken cancellationToken;

                // Token: 0x040005EC RID: 1516
                private UnconnectedMessageHandler.RequestResponseWaiter m_wrap1;

                // Token: 0x040005ED RID: 1517
                private TaskAwaiter<T> u__1;
            }
        }
	

		// Token: 0x02000191 RID: 401
		[CompilerGenerated]
        [StructLayout(LayoutKind.Auto)]
        private struct AwaitResponseAsync_d__78<T> : IAsyncStateMachine where T : IUnconnectedReliableResponse
		{
			    // Token: 0x06000917 RID: 2327 RVA: 0x0001A1DC File Offset: 0x000183DC
			void IAsyncStateMachine.MoveNext()
			{
                int num = this.m_state;
                UnconnectedMessageHandler unconnectedMessageHandler = this.m_this;
                T result2;
                try
                {
                    TaskAwaiter<IUnconnectedMessage> awaiter;
                    if (num != 0)
                    {
                        UnconnectedMessageHandler.RequestResponseWaiter requestResponseWaiter = new UnconnectedMessageHandler.RequestResponseWaiter(unconnectedMessageHandler._disposedTokenSource.Token, this.cancellationToken);
                        this.waiterId_5__2 = new UnconnectedMessageHandler.RequestWaiterId(this.remoteEndPoint, this.requestId);
                        unconnectedMessageHandler._requestResponseWaiters.Add(this.waiterId_5__2, requestResponseWaiter);
                        awaiter = requestResponseWaiter.task.GetAwaiter();
                        if (!awaiter.IsCompleted)
                        {
                            this.m_state = 0;
                            this.u__1 = awaiter;
                            this.t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<IUnconnectedMessage>, UnconnectedMessageHandler.AwaitResponseAsync_d__78<T>> (ref awaiter, ref this);
                            return;
                        }
                    }
                    else
                    {
                        awaiter = this.u__1;
                        this.u__1 = default(TaskAwaiter<IUnconnectedMessage>);
                        this.m_state = -1;
                    }
                    IUnconnectedMessage result = awaiter.GetResult();
                    unconnectedMessageHandler._requestResponseWaiters.Remove(this.waiterId_5__2);
                    IUnconnectedMessage unconnectedMessage;
                    if (!((unconnectedMessage = result) is T))
                    {
                        if (result != null)
                        {
                            result.Release();
                        }
                        throw new Exception("Received Unexpected response");
                    }
                    this.tResult_5__3 = (T)((object)unconnectedMessage);
                    result2 = this.tResult_5__3;
                }
                catch (Exception exception)
                {
                    this.m_state = -2;
                    this.t__builder.SetException(exception);
                    return;
                }
                this.m_state = -2;
                this.t__builder.SetResult(result2);
            }
            
            // Token: 0x06000918 RID: 2328 RVA: 0x0001A324 File Offset: 0x00018524
            [DebuggerHidden]
            void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.t__builder.SetStateMachine(stateMachine);
            }
                
            // Token: 0x040005A3 RID: 1443
            public int m_state;
            
            // Token: 0x040005A4 RID: 1444
            public AsyncTaskMethodBuilder<T> t__builder;
            
            // Token: 0x040005A5 RID: 1445
            public UnconnectedMessageHandler m_this;
            
            // Token: 0x040005A6 RID: 1446
            public CancellationToken cancellationToken;
            
            // Token: 0x040005A7 RID: 1447
            public IPEndPoint remoteEndPoint;
            
            // Token: 0x040005A8 RID: 1448
            public uint requestId;
            
            // Token: 0x040005A9 RID: 1449
            private UnconnectedMessageHandler.RequestWaiterId waiterId_5__2;
            
            // Token: 0x040005AA RID: 1450
            private T tResult_5__3;
            
            // Token: 0x040005AB RID: 1451
            private TaskAwaiter<IUnconnectedMessage> u__1;
		}
	}
}
