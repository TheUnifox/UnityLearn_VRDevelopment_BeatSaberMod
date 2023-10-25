using System;

namespace Zenject
{
    // Token: 0x020001BE RID: 446
    [NoReflectionBaking]
    public class PoolExceededFixedSizeException : Exception
    {
        // Token: 0x0600092E RID: 2350 RVA: 0x00018924 File Offset: 0x00016B24
        public PoolExceededFixedSizeException(string errorMessage) : base(errorMessage)
        {
        }
    }
}
