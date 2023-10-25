using System;

namespace Zenject
{
    // Token: 0x0200004D RID: 77
    [NoReflectionBaking]
    public class MemoryPoolBindInfo
    {
        // Token: 0x06000207 RID: 519 RVA: 0x00006D8C File Offset: 0x00004F8C
        public MemoryPoolBindInfo()
        {
            this.ExpandMethod = PoolExpandMethods.OneAtATime;
            this.MaxSize = int.MaxValue;
            this.ShowExpandWarning = true;
        }

        // Token: 0x17000036 RID: 54
        // (get) Token: 0x06000208 RID: 520 RVA: 0x00006DB0 File Offset: 0x00004FB0
        // (set) Token: 0x06000209 RID: 521 RVA: 0x00006DB8 File Offset: 0x00004FB8
        public bool ShowExpandWarning { get; set; }

        // Token: 0x17000037 RID: 55
        // (get) Token: 0x0600020A RID: 522 RVA: 0x00006DC4 File Offset: 0x00004FC4
        // (set) Token: 0x0600020B RID: 523 RVA: 0x00006DCC File Offset: 0x00004FCC
        public PoolExpandMethods ExpandMethod { get; set; }

        // Token: 0x17000038 RID: 56
        // (get) Token: 0x0600020C RID: 524 RVA: 0x00006DD8 File Offset: 0x00004FD8
        // (set) Token: 0x0600020D RID: 525 RVA: 0x00006DE0 File Offset: 0x00004FE0
        public int InitialSize { get; set; }

        // Token: 0x17000039 RID: 57
        // (get) Token: 0x0600020E RID: 526 RVA: 0x00006DEC File Offset: 0x00004FEC
        // (set) Token: 0x0600020F RID: 527 RVA: 0x00006DF4 File Offset: 0x00004FF4
        public int MaxSize { get; set; }
    }
}
