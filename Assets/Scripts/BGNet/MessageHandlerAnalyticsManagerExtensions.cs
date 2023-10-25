using System;
using System.Collections.Generic;
using BGNet.Core;
using BGNet.Core.Messages;

// Token: 0x0200005A RID: 90
public static class MessageHandlerAnalyticsManagerExtensions
{
    // Token: 0x06000402 RID: 1026 RVA: 0x00009C20 File Offset: 0x00007E20
    private static string GetMessageName(IUnconnectedMessage message)
    {
        Type type = message.GetType();
        string name;
        if (!MessageHandlerAnalyticsManagerExtensions._typeNameLookup.TryGetValue(type, out name))
        {
            name = type.Name;
            if (name.Length == 2)
            {
                name = type.BaseType.Name;
            }
            MessageHandlerAnalyticsManagerExtensions._typeNameLookup[type] = name;
        }
        return name;
    }

    // Token: 0x06000403 RID: 1027 RVA: 0x00009C6C File Offset: 0x00007E6C
    private static string GetResponseCodeName(IUnconnectedResponse response)
    {
        Dictionary<byte, string> dictionary;
        if (!MessageHandlerAnalyticsManagerExtensions._responseCodeLookup.TryGetValue(response.GetType(), out dictionary))
        {
            dictionary = (MessageHandlerAnalyticsManagerExtensions._responseCodeLookup[response.GetType()] = new Dictionary<byte, string>());
        }
        string resultCodeString;
        if (!dictionary.TryGetValue(response.resultCode, out resultCodeString))
        {
            resultCodeString = response.resultCodeString;
            dictionary[response.resultCode] = resultCodeString;
        }
        return resultCodeString;
    }

    // Token: 0x06000404 RID: 1028 RVA: 0x00009CCC File Offset: 0x00007ECC
    public static void ReceivedReliableRequestEvent(this IAnalyticsManager analyticsManager, IUnconnectedReliableRequest request)
    {
        if (analyticsManager != null)
        {
            analyticsManager.IncrementCounter(new MetricIdentifier("ReceivedMessages", new ValueTuple<string, string>("DeliveryType", "Reliable"), new ValueTuple<string, string>("MessageCategory", "Request"), new ValueTuple<string, string>("MessageType", MessageHandlerAnalyticsManagerExtensions.GetMessageName(request)), default(ValueTuple<string, string>)), 1L, AnalyticsMetricUnit.Count);
        }
    }

    // Token: 0x06000405 RID: 1029 RVA: 0x00009D28 File Offset: 0x00007F28
    public static void SentReliableRequestEvent(this IAnalyticsManager analyticsManager, IUnconnectedReliableRequest request)
    {
        if (analyticsManager != null)
        {
            analyticsManager.IncrementCounter(new MetricIdentifier("SentMessages", new ValueTuple<string, string>("DeliveryType", "Reliable"), new ValueTuple<string, string>("MessageCategory", "Request"), new ValueTuple<string, string>("MessageType", MessageHandlerAnalyticsManagerExtensions.GetMessageName(request)), default(ValueTuple<string, string>)), 1L, AnalyticsMetricUnit.Count);
        }
    }

    // Token: 0x06000406 RID: 1030 RVA: 0x00009D84 File Offset: 0x00007F84
    public static void ReceivedReliableResponseEvent(this IAnalyticsManager analyticsManager, IUnconnectedReliableResponse response)
    {
        if (analyticsManager != null)
        {
            analyticsManager.IncrementCounter(new MetricIdentifier("ReceivedMessages", new ValueTuple<string, string>("DeliveryType", "Reliable"), new ValueTuple<string, string>("MessageCategory", "Response"), new ValueTuple<string, string>("MessageType", MessageHandlerAnalyticsManagerExtensions.GetMessageName(response)), new ValueTuple<string, string>("Result", MessageHandlerAnalyticsManagerExtensions.GetResponseCodeName(response))), 1L, AnalyticsMetricUnit.Count);
        }
    }

    // Token: 0x06000407 RID: 1031 RVA: 0x00009DE8 File Offset: 0x00007FE8
    public static void SentReliableResponseEvent(this IAnalyticsManager analyticsManager, IUnconnectedReliableResponse response)
    {
        if (analyticsManager != null)
        {
            analyticsManager.IncrementCounter(new MetricIdentifier("SentMessages", new ValueTuple<string, string>("DeliveryType", "Reliable"), new ValueTuple<string, string>("MessageCategory", "Response"), new ValueTuple<string, string>("MessageType", MessageHandlerAnalyticsManagerExtensions.GetMessageName(response)), new ValueTuple<string, string>("Result", MessageHandlerAnalyticsManagerExtensions.GetResponseCodeName(response))), 1L, AnalyticsMetricUnit.Count);
        }
    }

    // Token: 0x06000408 RID: 1032 RVA: 0x00009E4C File Offset: 0x0000804C
    public static void ReceivedUnreliableMessageEvent(this IAnalyticsManager analyticsManager, IUnconnectedUnreliableMessage message)
    {
        if (analyticsManager != null)
        {
            analyticsManager.IncrementCounter(new MetricIdentifier("ReceivedMessages", new ValueTuple<string, string>("DeliveryType", "Unreliable"), new ValueTuple<string, string>("MessageCategory", "Message"), new ValueTuple<string, string>("MessageType", MessageHandlerAnalyticsManagerExtensions.GetMessageName(message)), default(ValueTuple<string, string>)), 1L, AnalyticsMetricUnit.Count);
        }
    }

    // Token: 0x06000409 RID: 1033 RVA: 0x00009EA8 File Offset: 0x000080A8
    public static void SentUnreliableMessageEvent(this IAnalyticsManager analyticsManager, IUnconnectedUnreliableMessage message)
    {
        if (analyticsManager != null)
        {
            analyticsManager.IncrementCounter(new MetricIdentifier("SentMessages", new ValueTuple<string, string>("DeliveryType", "Unreliable"), new ValueTuple<string, string>("MessageCategory", "Message"), new ValueTuple<string, string>("MessageType", MessageHandlerAnalyticsManagerExtensions.GetMessageName(message)), default(ValueTuple<string, string>)), 1L, AnalyticsMetricUnit.Count);
        }
    }

    // Token: 0x0600040A RID: 1034 RVA: 0x00009F04 File Offset: 0x00008104
    public static void ReceivedUnreliableResponseEvent(this IAnalyticsManager analyticsManager, IUnconnectedResponse response)
    {
        if (analyticsManager != null)
        {
            analyticsManager.IncrementCounter(new MetricIdentifier("ReceivedMessages", new ValueTuple<string, string>("DeliveryType", "Unreliable"), new ValueTuple<string, string>("MessageCategory", "Response"), new ValueTuple<string, string>("MessageType", MessageHandlerAnalyticsManagerExtensions.GetMessageName(response)), new ValueTuple<string, string>("Result", MessageHandlerAnalyticsManagerExtensions.GetResponseCodeName(response))), 1L, AnalyticsMetricUnit.Count);
        }
    }

    // Token: 0x0600040B RID: 1035 RVA: 0x00009F68 File Offset: 0x00008168
    public static void SentUnreliableResponseEvent(this IAnalyticsManager analyticsManager, IUnconnectedResponse response)
    {
        if (analyticsManager != null)
        {
            analyticsManager.IncrementCounter(new MetricIdentifier("SentMessages", new ValueTuple<string, string>("DeliveryType", "Unreliable"), new ValueTuple<string, string>("MessageCategory", "Response"), new ValueTuple<string, string>("MessageType", MessageHandlerAnalyticsManagerExtensions.GetMessageName(response)), new ValueTuple<string, string>("Result", MessageHandlerAnalyticsManagerExtensions.GetResponseCodeName(response))), 1L, AnalyticsMetricUnit.Count);
        }
    }

    // Token: 0x04000157 RID: 343
    private const string kReceivedMessagesMetricName = "ReceivedMessages";

    // Token: 0x04000158 RID: 344
    private const string kSentMessagesMetricName = "SentMessages";

    // Token: 0x04000159 RID: 345
    private const string kDeliveryTypeKey = "DeliveryType";

    // Token: 0x0400015A RID: 346
    private const string kMessageCategoryKey = "MessageCategory";

    // Token: 0x0400015B RID: 347
    private const string kMessageTypeKey = "MessageType";

    // Token: 0x0400015C RID: 348
    private const string kResultKey = "Result";

    // Token: 0x0400015D RID: 349
    private const string kDeliveryTypeReliable = "Reliable";

    // Token: 0x0400015E RID: 350
    private const string kDeliveryTypeUnreliable = "Unreliable";

    // Token: 0x0400015F RID: 351
    private const string kMessageCategoryRequest = "Request";

    // Token: 0x04000160 RID: 352
    private const string kMessageCategoryResponse = "Response";

    // Token: 0x04000161 RID: 353
    private const string kMessageCategoryMessage = "Message";

    // Token: 0x04000162 RID: 354
    private static readonly Dictionary<Type, string> _typeNameLookup = new Dictionary<Type, string>();

    // Token: 0x04000163 RID: 355
    private static readonly Dictionary<Type, Dictionary<byte, string>> _responseCodeLookup = new Dictionary<Type, Dictionary<byte, string>>();
}
