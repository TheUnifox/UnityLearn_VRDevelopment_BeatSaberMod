using System;

namespace JetBrains.Annotations
{
    // Token: 0x0200003D RID: 61
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    internal sealed class AspMvcMasterLocationFormatAttribute : Attribute
    {
        // Token: 0x0600008D RID: 141 RVA: 0x000024F1 File Offset: 0x000006F1
        public AspMvcMasterLocationFormatAttribute([NotNull] string format)
        {
            this.Format = format;
        }

        // Token: 0x1700001A RID: 26
        // (get) Token: 0x0600008E RID: 142 RVA: 0x00002500 File Offset: 0x00000700
        // (set) Token: 0x0600008F RID: 143 RVA: 0x00002508 File Offset: 0x00000708
        [NotNull]
        public string Format { get; private set; }
    }
}
