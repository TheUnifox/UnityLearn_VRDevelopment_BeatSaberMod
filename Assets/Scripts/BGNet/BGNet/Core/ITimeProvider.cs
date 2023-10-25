using System;

namespace BGNet.Core
{
    // Token: 0x020000A4 RID: 164
    public interface ITimeProvider
    {
        // Token: 0x06000622 RID: 1570
        long GetTimeMs();

        // Token: 0x06000623 RID: 1571
        long GetTicks();
    }
}
