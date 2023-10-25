using System;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x02000006 RID: 6
    public abstract class InjectAttributeBase : PreserveAttribute
    {
        // Token: 0x17000001 RID: 1
        // (get) Token: 0x06000005 RID: 5 RVA: 0x0000208D File Offset: 0x0000028D
        // (set) Token: 0x06000006 RID: 6 RVA: 0x00002095 File Offset: 0x00000295
        public bool Optional { get; set; }

        // Token: 0x17000002 RID: 2
        // (get) Token: 0x06000007 RID: 7 RVA: 0x0000209E File Offset: 0x0000029E
        // (set) Token: 0x06000008 RID: 8 RVA: 0x000020A6 File Offset: 0x000002A6
        public object Id { get; set; }

        // Token: 0x17000003 RID: 3
        // (get) Token: 0x06000009 RID: 9 RVA: 0x000020AF File Offset: 0x000002AF
        // (set) Token: 0x0600000A RID: 10 RVA: 0x000020B7 File Offset: 0x000002B7
        public InjectSources Source { get; set; }
    }
}
