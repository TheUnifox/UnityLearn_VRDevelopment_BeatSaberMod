using System;

namespace JetBrains.Annotations
{
    // Token: 0x0200003F RID: 63
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    internal sealed class AspMvcViewLocationFormatAttribute : Attribute
    {
        // Token: 0x06000093 RID: 147 RVA: 0x00002531 File Offset: 0x00000731
        public AspMvcViewLocationFormatAttribute([NotNull] string format)
        {
            this.Format = format;
        }

        // Token: 0x1700001C RID: 28
        // (get) Token: 0x06000094 RID: 148 RVA: 0x00002540 File Offset: 0x00000740
        // (set) Token: 0x06000095 RID: 149 RVA: 0x00002548 File Offset: 0x00000748
        [NotNull]
        public string Format { get; private set; }
    }
}
