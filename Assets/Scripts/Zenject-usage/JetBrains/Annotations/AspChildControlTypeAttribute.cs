using System;

namespace JetBrains.Annotations
{
    // Token: 0x0200005D RID: 93
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    internal sealed class AspChildControlTypeAttribute : Attribute
    {
        // Token: 0x060000C3 RID: 195 RVA: 0x000026F1 File Offset: 0x000008F1
        public AspChildControlTypeAttribute([NotNull] string tagName, [NotNull] Type controlType)
        {
            this.TagName = tagName;
            this.ControlType = controlType;
        }

        // Token: 0x17000024 RID: 36
        // (get) Token: 0x060000C4 RID: 196 RVA: 0x00002707 File Offset: 0x00000907
        // (set) Token: 0x060000C5 RID: 197 RVA: 0x0000270F File Offset: 0x0000090F
        [NotNull]
        public string TagName { get; private set; }

        // Token: 0x17000025 RID: 37
        // (get) Token: 0x060000C6 RID: 198 RVA: 0x00002718 File Offset: 0x00000918
        // (set) Token: 0x060000C7 RID: 199 RVA: 0x00002720 File Offset: 0x00000920
        [NotNull]
        public Type ControlType { get; private set; }
    }
}
