using System;

namespace JetBrains.Annotations
{
    // Token: 0x02000065 RID: 101
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    internal sealed class RazorDirectiveAttribute : Attribute
    {
        // Token: 0x060000D9 RID: 217 RVA: 0x000027D9 File Offset: 0x000009D9
        public RazorDirectiveAttribute([NotNull] string directive)
        {
            this.Directive = directive;
        }

        // Token: 0x1700002B RID: 43
        // (get) Token: 0x060000DA RID: 218 RVA: 0x000027E8 File Offset: 0x000009E8
        // (set) Token: 0x060000DB RID: 219 RVA: 0x000027F0 File Offset: 0x000009F0
        [NotNull]
        public string Directive { get; private set; }
    }
}
