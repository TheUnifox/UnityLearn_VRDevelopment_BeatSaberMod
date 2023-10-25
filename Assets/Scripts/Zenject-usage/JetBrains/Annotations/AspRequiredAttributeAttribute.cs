using System;

namespace JetBrains.Annotations
{
    // Token: 0x02000061 RID: 97
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    internal sealed class AspRequiredAttributeAttribute : Attribute
    {
        // Token: 0x060000CB RID: 203 RVA: 0x00002741 File Offset: 0x00000941
        public AspRequiredAttributeAttribute([NotNull] string attribute)
        {
            this.Attribute = attribute;
        }

        // Token: 0x17000026 RID: 38
        // (get) Token: 0x060000CC RID: 204 RVA: 0x00002750 File Offset: 0x00000950
        // (set) Token: 0x060000CD RID: 205 RVA: 0x00002758 File Offset: 0x00000958
        [NotNull]
        public string Attribute { get; private set; }
    }
}
