using System;
using System.Diagnostics;
using UnityEngine;

namespace ModestTree
{
    // Token: 0x0200000D RID: 13
    public static class Log
    {
        // Token: 0x06000054 RID: 84 RVA: 0x00002AA4 File Offset: 0x00000CA4
        [Conditional("UNITY_EDITOR")]
        public static void Debug(string message, params object[] args)
        {
        }

        // Token: 0x06000055 RID: 85 RVA: 0x00002AA8 File Offset: 0x00000CA8
        public static void Info(string message, params object[] args)
        {
            UnityEngine.Debug.Log(message.Fmt(args));
        }

        // Token: 0x06000056 RID: 86 RVA: 0x00002AB8 File Offset: 0x00000CB8
        public static void Warn(string message, params object[] args)
        {
            UnityEngine.Debug.LogWarning(message.Fmt(args));
        }

        // Token: 0x06000057 RID: 87 RVA: 0x00002AC8 File Offset: 0x00000CC8
        public static void Trace(string message, params object[] args)
        {
            UnityEngine.Debug.Log(message.Fmt(args));
        }

        // Token: 0x06000058 RID: 88 RVA: 0x00002AD8 File Offset: 0x00000CD8
        public static void ErrorException(Exception e)
        {
            UnityEngine.Debug.LogException(e);
        }

        // Token: 0x06000059 RID: 89 RVA: 0x00002AE0 File Offset: 0x00000CE0
        public static void ErrorException(string message, Exception e)
        {
            UnityEngine.Debug.LogError(message);
            UnityEngine.Debug.LogException(e);
        }

        // Token: 0x0600005A RID: 90 RVA: 0x00002AF0 File Offset: 0x00000CF0
        public static void Error(string message, params object[] args)
        {
            UnityEngine.Debug.LogError(message.Fmt(args));
        }
    }
}
