using System;

namespace JetBrains.Annotations
{
    // Token: 0x02000027 RID: 39
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true)]
    internal sealed class ValueProviderAttribute : Attribute
    {
        // Token: 0x06000047 RID: 71 RVA: 0x0000222F File Offset: 0x0000042F
        public ValueProviderAttribute([NotNull] string name)
        {
            this.Name = name;
        }

        // Token: 0x17000007 RID: 7
        // (get) Token: 0x06000048 RID: 72 RVA: 0x0000223E File Offset: 0x0000043E
        // (set) Token: 0x06000049 RID: 73 RVA: 0x00002246 File Offset: 0x00000446
        [NotNull]
        public string Name { get; private set; }
    }
}
