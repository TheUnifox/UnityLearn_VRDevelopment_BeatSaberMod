using System;

namespace JetBrains.Annotations
{
    // Token: 0x02000040 RID: 64
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter)]
    internal sealed class AspMvcActionAttribute : Attribute
    {
        // Token: 0x06000096 RID: 150 RVA: 0x00002551 File Offset: 0x00000751
        public AspMvcActionAttribute()
        {
        }

        // Token: 0x06000097 RID: 151 RVA: 0x00002559 File Offset: 0x00000759
        public AspMvcActionAttribute([NotNull] string anonymousProperty)
        {
            this.AnonymousProperty = anonymousProperty;
        }

        // Token: 0x1700001D RID: 29
        // (get) Token: 0x06000098 RID: 152 RVA: 0x00002568 File Offset: 0x00000768
        // (set) Token: 0x06000099 RID: 153 RVA: 0x00002570 File Offset: 0x00000770
        [CanBeNull]
        public string AnonymousProperty { get; private set; }
    }
}
