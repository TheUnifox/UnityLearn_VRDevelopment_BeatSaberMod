using System;

namespace JetBrains.Annotations
{
    // Token: 0x0200003B RID: 59
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    internal sealed class AspMvcAreaPartialViewLocationFormatAttribute : Attribute
    {
        // Token: 0x06000087 RID: 135 RVA: 0x000024B1 File Offset: 0x000006B1
        public AspMvcAreaPartialViewLocationFormatAttribute([NotNull] string format)
        {
            this.Format = format;
        }

        // Token: 0x17000018 RID: 24
        // (get) Token: 0x06000088 RID: 136 RVA: 0x000024C0 File Offset: 0x000006C0
        // (set) Token: 0x06000089 RID: 137 RVA: 0x000024C8 File Offset: 0x000006C8
        [NotNull]
        public string Format { get; private set; }
    }
}
