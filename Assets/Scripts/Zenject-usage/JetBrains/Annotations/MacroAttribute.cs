using System;

namespace JetBrains.Annotations
{
    // Token: 0x02000039 RID: 57
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter, AllowMultiple = true)]
    internal sealed class MacroAttribute : Attribute
    {
        // Token: 0x17000014 RID: 20
        // (get) Token: 0x0600007D RID: 125 RVA: 0x00002456 File Offset: 0x00000656
        // (set) Token: 0x0600007E RID: 126 RVA: 0x0000245E File Offset: 0x0000065E
        [CanBeNull]
        public string Expression { get; set; }

        // Token: 0x17000015 RID: 21
        // (get) Token: 0x0600007F RID: 127 RVA: 0x00002467 File Offset: 0x00000667
        // (set) Token: 0x06000080 RID: 128 RVA: 0x0000246F File Offset: 0x0000066F
        public int Editable { get; set; }

        // Token: 0x17000016 RID: 22
        // (get) Token: 0x06000081 RID: 129 RVA: 0x00002478 File Offset: 0x00000678
        // (set) Token: 0x06000082 RID: 130 RVA: 0x00002480 File Offset: 0x00000680
        [CanBeNull]
        public string Target { get; set; }
    }
}
