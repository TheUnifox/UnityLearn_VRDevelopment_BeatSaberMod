using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using BGNet.Logging;
using LiteNetLib.Utils;

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
                catch (TaskCanceledException obj1)
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
            if (onSendFailed == null)
			{
                return null;
            }
            return (uint protocolVersion, IPEndPoint remoteEndPoint, IUnconnectedReliableRequest message, CancellationToken cancellationToken) =>
            {
                AsyncTaskMethodBuilder builder = AsyncTaskMethodBuilder.Create();
                //state = -1;

                builder.Start<onSendFailed>(ref onSendFailed);
                return builder.Task;
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
                    (int, int) valueTuple = this._ranges[j];
                    (int, int) valueTuple2 = this._ranges[j - 1];
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

            private readonly List<(int offset, int length)> _ranges = new List<(int, int)>();

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
    }
}
