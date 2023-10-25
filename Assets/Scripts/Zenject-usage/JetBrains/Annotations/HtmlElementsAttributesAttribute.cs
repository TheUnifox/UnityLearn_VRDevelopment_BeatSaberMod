using System;

namespace JetBrains.Annotations
{
    // Token: 0x0200004E RID: 78
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    internal sealed class HtmlElementAttributesAttribute : Attribute
    {
        // Token: 0x060000AD RID: 173 RVA: 0x00002621 File Offset: 0x00000821
        public HtmlElementAttributesAttribute()
        {
        }

        // Token: 0x060000AE RID: 174 RVA: 0x00002629 File Offset: 0x00000829
        public HtmlElementAttributesAttribute([NotNull] string name)
        {
            this.Name = name;
        }

        // Token: 0x17000020 RID: 32
        // (get) Token: 0x060000AF RID: 175 RVA: 0x00002638 File Offset: 0x00000838
        // (set) Token: 0x060000B0 RID: 176 RVA: 0x00002640 File Offset: 0x00000840
        [CanBeNull]
        public string Name { get; private set; }
    }
}
