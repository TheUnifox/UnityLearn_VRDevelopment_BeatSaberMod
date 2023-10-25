using System;

namespace JetBrains.Annotations
{
    // Token: 0x0200003E RID: 62
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    internal sealed class AspMvcPartialViewLocationFormatAttribute : Attribute
    {
        // Token: 0x06000090 RID: 144 RVA: 0x00002511 File Offset: 0x00000711
        public AspMvcPartialViewLocationFormatAttribute([NotNull] string format)
        {
            this.Format = format;
        }

        // Token: 0x1700001B RID: 27
        // (get) Token: 0x06000091 RID: 145 RVA: 0x00002520 File Offset: 0x00000720
        // (set) Token: 0x06000092 RID: 146 RVA: 0x00002528 File Offset: 0x00000728
        [NotNull]
        public string Format { get; private set; }
    }
}