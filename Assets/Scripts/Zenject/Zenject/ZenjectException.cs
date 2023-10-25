using System;
using System.Diagnostics;

namespace Zenject
{
    // Token: 0x020002F8 RID: 760
    [DebuggerStepThrough]
    [NoReflectionBaking]
    public class ZenjectException : Exception
    {
        // Token: 0x06001062 RID: 4194 RVA: 0x0002E2BC File Offset: 0x0002C4BC
        public ZenjectException(string message) : base(message)
        {
        }

        // Token: 0x06001063 RID: 4195 RVA: 0x0002E2C8 File Offset: 0x0002C4C8
        public ZenjectException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
