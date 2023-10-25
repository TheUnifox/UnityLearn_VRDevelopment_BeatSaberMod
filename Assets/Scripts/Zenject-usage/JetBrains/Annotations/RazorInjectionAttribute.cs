using System;

namespace JetBrains.Annotations
{
    // Token: 0x02000064 RID: 100
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    internal sealed class RazorInjectionAttribute : Attribute
    {
        // Token: 0x060000D4 RID: 212 RVA: 0x000027A1 File Offset: 0x000009A1
        public RazorInjectionAttribute([NotNull] string type, [NotNull] string fieldName)
        {
            this.Type = type;
            this.FieldName = fieldName;
        }

        // Token: 0x17000029 RID: 41
        // (get) Token: 0x060000D5 RID: 213 RVA: 0x000027B7 File Offset: 0x000009B7
        // (set) Token: 0x060000D6 RID: 214 RVA: 0x000027BF File Offset: 0x000009BF
        [NotNull]
        public string Type { get; private set; }

        // Token: 0x1700002A RID: 42
        // (get) Token: 0x060000D7 RID: 215 RVA: 0x000027C8 File Offset: 0x000009C8
        // (set) Token: 0x060000D8 RID: 216 RVA: 0x000027D0 File Offset: 0x000009D0
        [NotNull]
        public string FieldName { get; private set; }
    }
}
