using System;
using UnityEngine;

namespace BGNet.Logging
{
    // Token: 0x02000004 RID: 4
    public class UnityLogger : Debug.ILogger
    {
        // Token: 0x0600000E RID: 14 RVA: 0x00002303 File Offset: 0x00000503
        public void LogInfo(string message)
        {
        }

        // Token: 0x0600000F RID: 15 RVA: 0x00002305 File Offset: 0x00000505
        public void LogError(string message)
        {
            Debug.LogError(message);
        }

        // Token: 0x06000010 RID: 16 RVA: 0x0000230D File Offset: 0x0000050D
        public void LogException(Exception exception, string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                Debug.LogError(message);
            }
            Debug.LogException(exception);
        }

        // Token: 0x06000011 RID: 17 RVA: 0x00002323 File Offset: 0x00000523
        public void LogWarning(string message)
        {
            Debug.LogWarning(message);
        }
    }
}
