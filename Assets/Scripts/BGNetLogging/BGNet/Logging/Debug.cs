using System;
using UnityEngine;

namespace BGNet.Logging
{
    // Token: 0x02000003 RID: 3
    public static class Debug
    {
        // Token: 0x06000006 RID: 6 RVA: 0x0000210D File Offset: 0x0000030D
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void NoDomainReloadInit()
        {
            Debug._loggers = null;
            Debug.AddLogger(new UnityLogger());
        }

        // Token: 0x06000007 RID: 7 RVA: 0x00002121 File Offset: 0x00000321
        static Debug()
        {
            Debug.NoDomainReloadInit();
        }

        // Token: 0x06000008 RID: 8 RVA: 0x00002134 File Offset: 0x00000334
        public static void AddLogger(Debug.ILogger logger)
        {
            if (logger == null)
            {
                return;
            }
            object loggersMutex = Debug._loggersMutex;
            lock (loggersMutex)
            {
                Debug._loggers = new Debug.LoggerLinkedList(logger)
                {
                    next = Debug._loggers
                };
            }
        }

        // Token: 0x06000009 RID: 9 RVA: 0x00002190 File Offset: 0x00000390
        public static void RemoveLogger(Debug.ILogger logger)
        {
            if (logger == null)
            {
                return;
            }
            object loggersMutex = Debug._loggersMutex;
            lock (loggersMutex)
            {
                if (Debug._loggers != null)
                {
                    if (Debug._loggers.logger == logger)
                    {
                        Debug._loggers = Debug._loggers.next;
                    }
                    else
                    {
                        Debug.LoggerLinkedList loggerLinkedList = Debug._loggers;
                        while (loggerLinkedList.next != null)
                        {
                            if (loggerLinkedList.next.logger == logger)
                            {
                                loggerLinkedList.next = loggerLinkedList.next.next;
                                break;
                            }
                            loggerLinkedList = loggerLinkedList.next;
                        }
                    }
                }
            }
        }

        // Token: 0x0600000A RID: 10 RVA: 0x00002244 File Offset: 0x00000444
        public static void Log(string message)
        {
            for (Debug.LoggerLinkedList loggerLinkedList = Debug._loggers; loggerLinkedList != null; loggerLinkedList = loggerLinkedList.next)
            {
                loggerLinkedList.logger.LogInfo(message);
            }
        }

        // Token: 0x0600000B RID: 11 RVA: 0x00002274 File Offset: 0x00000474
        public static void LogError(string message)
        {
            for (Debug.LoggerLinkedList loggerLinkedList = Debug._loggers; loggerLinkedList != null; loggerLinkedList = loggerLinkedList.next)
            {
                loggerLinkedList.logger.LogError(message);
            }
        }

        // Token: 0x0600000C RID: 12 RVA: 0x000022A4 File Offset: 0x000004A4
        public static void LogException(Exception exception, string message = null)
        {
            for (Debug.LoggerLinkedList loggerLinkedList = Debug._loggers; loggerLinkedList != null; loggerLinkedList = loggerLinkedList.next)
            {
                loggerLinkedList.logger.LogException(exception, message);
            }
        }

        // Token: 0x0600000D RID: 13 RVA: 0x000022D4 File Offset: 0x000004D4
        public static void LogWarning(string message)
        {
            for (Debug.LoggerLinkedList loggerLinkedList = Debug._loggers; loggerLinkedList != null; loggerLinkedList = loggerLinkedList.next)
            {
                loggerLinkedList.logger.LogWarning(message);
            }
        }

        // Token: 0x04000001 RID: 1
        [DoesNotRequireDomainReloadInit]
        private static readonly object _loggersMutex = new object();

        // Token: 0x04000002 RID: 2
        private static volatile Debug.LoggerLinkedList _loggers;

        // Token: 0x02000005 RID: 5
        private class LoggerLinkedList
        {
            // Token: 0x06000013 RID: 19 RVA: 0x0000232B File Offset: 0x0000052B
            public LoggerLinkedList(Debug.ILogger logger)
            {
                this.logger = logger;
            }

            // Token: 0x04000003 RID: 3
            public readonly Debug.ILogger logger;

            // Token: 0x04000004 RID: 4
            public volatile Debug.LoggerLinkedList next;
        }

        // Token: 0x02000006 RID: 6
        public interface ILogger
        {
            // Token: 0x06000014 RID: 20
            void LogInfo(string message);

            // Token: 0x06000015 RID: 21
            void LogError(string message);

            // Token: 0x06000016 RID: 22
            void LogException(Exception exception, string message);

            // Token: 0x06000017 RID: 23
            void LogWarning(string message);
        }
    }
}
