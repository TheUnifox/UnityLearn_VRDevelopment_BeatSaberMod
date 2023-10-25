using System;

namespace JetBrains.Annotations
{
    // Token: 0x0200002B RID: 43
    [AttributeUsage(AttributeTargets.All)]
    internal sealed class LocalizationRequiredAttribute : Attribute
    {
        // Token: 0x06000055 RID: 85 RVA: 0x000022C1 File Offset: 0x000004C1
        public LocalizationRequiredAttribute() : this(true)
        {
        }

        // Token: 0x06000056 RID: 86 RVA: 0x000022CA File Offset: 0x000004CA
        public LocalizationRequiredAttribute(bool required)
        {
            this.Required = required;
        }

        // Token: 0x1700000B RID: 11
        // (get) Token: 0x06000057 RID: 87 RVA: 0x000022D9 File Offset: 0x000004D9
        // (set) Token: 0x06000058 RID: 88 RVA: 0x000022E1 File Offset: 0x000004E1
        public bool Required { get; private set; }
    }
}
