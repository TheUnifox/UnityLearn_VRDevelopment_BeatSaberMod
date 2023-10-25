using System;

namespace JetBrains.Annotations
{
    // Token: 0x0200004F RID: 79
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    internal sealed class HtmlAttributeValueAttribute : Attribute
    {
        // Token: 0x060000B1 RID: 177 RVA: 0x00002649 File Offset: 0x00000849
        public HtmlAttributeValueAttribute([NotNull] string name)
        {
            this.Name = name;
        }

        // Token: 0x17000021 RID: 33
        // (get) Token: 0x060000B2 RID: 178 RVA: 0x00002658 File Offset: 0x00000858
        // (set) Token: 0x060000B3 RID: 179 RVA: 0x00002660 File Offset: 0x00000860
        [NotNull]
        public string Name { get; private set; }
    }
}
