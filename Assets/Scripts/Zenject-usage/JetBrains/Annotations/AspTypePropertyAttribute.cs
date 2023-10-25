using System;

namespace JetBrains.Annotations
{
    // Token: 0x02000062 RID: 98
    [AttributeUsage(AttributeTargets.Property)]
    internal sealed class AspTypePropertyAttribute : Attribute
    {
        // Token: 0x17000027 RID: 39
        // (get) Token: 0x060000CE RID: 206 RVA: 0x00002761 File Offset: 0x00000961
        // (set) Token: 0x060000CF RID: 207 RVA: 0x00002769 File Offset: 0x00000969
        public bool CreateConstructorReferences { get; private set; }

        // Token: 0x060000D0 RID: 208 RVA: 0x00002772 File Offset: 0x00000972
        public AspTypePropertyAttribute(bool createConstructorReferences)
        {
            this.CreateConstructorReferences = createConstructorReferences;
        }
    }
}
