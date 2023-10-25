using System;

namespace JetBrains.Annotations
{
    // Token: 0x02000066 RID: 102
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    internal sealed class RazorPageBaseTypeAttribute : Attribute
    {
        // Token: 0x060000DC RID: 220 RVA: 0x000027F9 File Offset: 0x000009F9
        public RazorPageBaseTypeAttribute([NotNull] string baseType)
        {
            this.BaseType = baseType;
        }

        // Token: 0x060000DD RID: 221 RVA: 0x00002808 File Offset: 0x00000A08
        public RazorPageBaseTypeAttribute([NotNull] string baseType, string pageName)
        {
            this.BaseType = baseType;
            this.PageName = pageName;
        }

        // Token: 0x1700002C RID: 44
        // (get) Token: 0x060000DE RID: 222 RVA: 0x0000281E File Offset: 0x00000A1E
        // (set) Token: 0x060000DF RID: 223 RVA: 0x00002826 File Offset: 0x00000A26
        [NotNull]
        public string BaseType { get; private set; }

        // Token: 0x1700002D RID: 45
        // (get) Token: 0x060000E0 RID: 224 RVA: 0x0000282F File Offset: 0x00000A2F
        // (set) Token: 0x060000E1 RID: 225 RVA: 0x00002837 File Offset: 0x00000A37
        [CanBeNull]
        public string PageName { get; private set; }
    }
}
