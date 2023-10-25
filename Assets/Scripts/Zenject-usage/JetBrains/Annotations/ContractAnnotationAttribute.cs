using System;

namespace JetBrains.Annotations
{
    // Token: 0x0200002A RID: 42
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    internal sealed class ContractAnnotationAttribute : Attribute
    {
        // Token: 0x0600004F RID: 79 RVA: 0x0000227F File Offset: 0x0000047F
        public ContractAnnotationAttribute([NotNull] string contract) : this(contract, false)
        {
        }

        // Token: 0x06000050 RID: 80 RVA: 0x00002289 File Offset: 0x00000489
        public ContractAnnotationAttribute([NotNull] string contract, bool forceFullStates)
        {
            this.Contract = contract;
            this.ForceFullStates = forceFullStates;
        }

        // Token: 0x17000009 RID: 9
        // (get) Token: 0x06000051 RID: 81 RVA: 0x0000229F File Offset: 0x0000049F
        // (set) Token: 0x06000052 RID: 82 RVA: 0x000022A7 File Offset: 0x000004A7
        [NotNull]
        public string Contract { get; private set; }

        // Token: 0x1700000A RID: 10
        // (get) Token: 0x06000053 RID: 83 RVA: 0x000022B0 File Offset: 0x000004B0
        // (set) Token: 0x06000054 RID: 84 RVA: 0x000022B8 File Offset: 0x000004B8
        public bool ForceFullStates { get; private set; }
    }
}
