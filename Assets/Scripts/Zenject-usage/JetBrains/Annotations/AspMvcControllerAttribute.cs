using System;

namespace JetBrains.Annotations
{
    // Token: 0x02000042 RID: 66
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter)]
    internal sealed class AspMvcControllerAttribute : Attribute
    {
        // Token: 0x0600009E RID: 158 RVA: 0x000025A1 File Offset: 0x000007A1
        public AspMvcControllerAttribute()
        {
        }

        // Token: 0x0600009F RID: 159 RVA: 0x000025A9 File Offset: 0x000007A9
        public AspMvcControllerAttribute([NotNull] string anonymousProperty)
        {
            this.AnonymousProperty = anonymousProperty;
        }

        // Token: 0x1700001F RID: 31
        // (get) Token: 0x060000A0 RID: 160 RVA: 0x000025B8 File Offset: 0x000007B8
        // (set) Token: 0x060000A1 RID: 161 RVA: 0x000025C0 File Offset: 0x000007C0
        [CanBeNull]
        public string AnonymousProperty { get; private set; }
    }
}
