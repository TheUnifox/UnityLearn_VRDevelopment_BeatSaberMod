using System;

namespace JetBrains.Annotations
{
    // Token: 0x02000032 RID: 50
    [MeansImplicitUse(ImplicitUseTargetFlags.WithMembers)]
    internal sealed class PublicAPIAttribute : Attribute
    {
        // Token: 0x0600006D RID: 109 RVA: 0x000023BE File Offset: 0x000005BE
        public PublicAPIAttribute()
        {
        }

        // Token: 0x0600006E RID: 110 RVA: 0x000023C6 File Offset: 0x000005C6
        public PublicAPIAttribute([NotNull] string comment)
        {
            this.Comment = comment;
        }

        // Token: 0x17000011 RID: 17
        // (get) Token: 0x0600006F RID: 111 RVA: 0x000023D5 File Offset: 0x000005D5
        // (set) Token: 0x06000070 RID: 112 RVA: 0x000023DD File Offset: 0x000005DD
        [CanBeNull]
        public string Comment { get; private set; }
    }
}
