using System;

namespace Zenject
{
    // Token: 0x020002FD RID: 765
    [NoReflectionBaking]
    public class ValidationMarker
    {
        // Token: 0x06001082 RID: 4226 RVA: 0x0002E704 File Offset: 0x0002C904
        public ValidationMarker(Type markedType, bool instantiateFailed)
        {
            this.MarkedType = markedType;
            this.InstantiateFailed = instantiateFailed;
        }

        // Token: 0x06001083 RID: 4227 RVA: 0x0002E71C File Offset: 0x0002C91C
        public ValidationMarker(Type markedType) : this(markedType, false)
        {
        }

        // Token: 0x17000160 RID: 352
        // (get) Token: 0x06001084 RID: 4228 RVA: 0x0002E728 File Offset: 0x0002C928
        // (set) Token: 0x06001085 RID: 4229 RVA: 0x0002E730 File Offset: 0x0002C930
        public bool InstantiateFailed { get; private set; }

        // Token: 0x17000161 RID: 353
        // (get) Token: 0x06001086 RID: 4230 RVA: 0x0002E73C File Offset: 0x0002C93C
        // (set) Token: 0x06001087 RID: 4231 RVA: 0x0002E744 File Offset: 0x0002C944
        public Type MarkedType { get; private set; }
    }
}
