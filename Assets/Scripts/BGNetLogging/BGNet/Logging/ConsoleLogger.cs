using System;
using System.Diagnostics;

namespace BGNet.Logging
{
    // Token: 0x02000002 RID: 2
    public class ConsoleLogger : Debug.ILogger
    {
        // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        public void LogInfo(string message)
        {
            Console.WriteLine(string.Format("[Info][{0}] ", DateTime.Now) + message);
        }

        // Token: 0x06000002 RID: 2 RVA: 0x00002074 File Offset: 0x00000274
        public void LogError(string message)
        {
            Console.Error.WriteLine(string.Concat(new object[]
            {
                string.Format("[Error][{0}] ", DateTime.Now),
                message,
                "\n",
                new StackTrace(1)
            }));
        }

        // Token: 0x06000003 RID: 3 RVA: 0x000020C2 File Offset: 0x000002C2
        public void LogException(Exception exception, string message)
        {
            Console.Error.WriteLine(string.Format("[Error][{0}] {1}\n{2}", DateTime.Now, message, exception));
        }

        // Token: 0x06000004 RID: 4 RVA: 0x000020E4 File Offset: 0x000002E4
        public void LogWarning(string message)
        {
            Console.WriteLine(string.Format("[Warning][{0}] ", DateTime.Now) + message);
        }
    }
}
