using System;

namespace JetBrains.Annotations
{
    // Token: 0x02000063 RID: 99
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    internal sealed class RazorImportNamespaceAttribute : Attribute
    {
        // Token: 0x060000D1 RID: 209 RVA: 0x00002781 File Offset: 0x00000981
        public RazorImportNamespaceAttribute([NotNull] string name)
        {
            this.Name = name;
        }

        // Token: 0x17000028 RID: 40
        // (get) Token: 0x060000D2 RID: 210 RVA: 0x00002790 File Offset: 0x00000990
        // (set) Token: 0x060000D3 RID: 211 RVA: 0x00002798 File Offset: 0x00000998
        [NotNull]
        public string Name { get; private set; }
    }
}
