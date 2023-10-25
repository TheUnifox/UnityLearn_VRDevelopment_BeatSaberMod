using System;

namespace JetBrains.Annotations
{
    // Token: 0x0200003A RID: 58
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    internal sealed class AspMvcAreaMasterLocationFormatAttribute : Attribute
    {
        // Token: 0x06000084 RID: 132 RVA: 0x00002491 File Offset: 0x00000691
        public AspMvcAreaMasterLocationFormatAttribute([NotNull] string format)
        {
            this.Format = format;
        }

        // Token: 0x17000017 RID: 23
        // (get) Token: 0x06000085 RID: 133 RVA: 0x000024A0 File Offset: 0x000006A0
        // (set) Token: 0x06000086 RID: 134 RVA: 0x000024A8 File Offset: 0x000006A8
        [NotNull]
        public string Format { get; private set; }
    }
}
