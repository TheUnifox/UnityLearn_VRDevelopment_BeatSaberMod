using System;

namespace JetBrains.Annotations
{
    // Token: 0x0200003C RID: 60
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    internal sealed class AspMvcAreaViewLocationFormatAttribute : Attribute
    {
        // Token: 0x0600008A RID: 138 RVA: 0x000024D1 File Offset: 0x000006D1
        public AspMvcAreaViewLocationFormatAttribute([NotNull] string format)
        {
            this.Format = format;
        }

        // Token: 0x17000019 RID: 25
        // (get) Token: 0x0600008B RID: 139 RVA: 0x000024E0 File Offset: 0x000006E0
        // (set) Token: 0x0600008C RID: 140 RVA: 0x000024E8 File Offset: 0x000006E8
        [NotNull]
        public string Format { get; private set; }
    }
}
