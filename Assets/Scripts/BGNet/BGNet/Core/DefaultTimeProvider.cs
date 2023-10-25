using System;

namespace BGNet.Core
{
    // Token: 0x0200009F RID: 159
    public static class DefaultTimeProvider
    {
        // Token: 0x170000FD RID: 253
        // (get) Token: 0x06000616 RID: 1558 RVA: 0x00010989 File Offset: 0x0000EB89
        public static BGNet.Core.ITimeProvider instance
        {
            get
            {
                return MonotonicTimeProvider.instance;
            }
        }
    }
}
