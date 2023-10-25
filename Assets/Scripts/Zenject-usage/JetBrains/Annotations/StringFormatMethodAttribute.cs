using System;

namespace JetBrains.Annotations
{
    // Token: 0x02000026 RID: 38
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Delegate)]
    internal sealed class StringFormatMethodAttribute : Attribute
    {
        // Token: 0x06000044 RID: 68 RVA: 0x0000220F File Offset: 0x0000040F
        public StringFormatMethodAttribute([NotNull] string formatParameterName)
        {
            this.FormatParameterName = formatParameterName;
        }

        // Token: 0x17000006 RID: 6
        // (get) Token: 0x06000045 RID: 69 RVA: 0x0000221E File Offset: 0x0000041E
        // (set) Token: 0x06000046 RID: 70 RVA: 0x00002226 File Offset: 0x00000426
        [NotNull]
        public string FormatParameterName { get; private set; }
    }
}
