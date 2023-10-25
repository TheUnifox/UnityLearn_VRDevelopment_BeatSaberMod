using System;
using BGNet.Logging;
using LiteNetLib;

// Token: 0x02000061 RID: 97
public class NetLogger : INetLogger
{
    // Token: 0x0600046B RID: 1131 RVA: 0x0000B474 File Offset: 0x00009674
    public void WriteNet(NetLogLevel level, string str, params object[] args)
    {
        switch (level)
        {
            case NetLogLevel.Warning:
                Debug.LogWarning("[NetLogger] " + string.Format(str, args));
                return;
            case NetLogLevel.Error:
                Debug.LogError("[NetLogger] " + string.Format(str, args));
                break;
            case NetLogLevel.Trace:
                break;
            case NetLogLevel.Info:
                Debug.Log("[NetLogger] " + string.Format(str, args));
                return;
            default:
                return;
        }
    }
}
