using System;

namespace JetBrains.Annotations
{
    // Token: 0x02000051 RID: 81
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property)]
    internal sealed class CollectionAccessAttribute : Attribute
    {
        // Token: 0x060000B5 RID: 181 RVA: 0x00002671 File Offset: 0x00000871
        public CollectionAccessAttribute(CollectionAccessType collectionAccessType)
        {
            this.CollectionAccessType = collectionAccessType;
        }

        // Token: 0x17000022 RID: 34
        // (get) Token: 0x060000B6 RID: 182 RVA: 0x00002680 File Offset: 0x00000880
        // (set) Token: 0x060000B7 RID: 183 RVA: 0x00002688 File Offset: 0x00000888
        public CollectionAccessType CollectionAccessType { get; private set; }
    }
}
