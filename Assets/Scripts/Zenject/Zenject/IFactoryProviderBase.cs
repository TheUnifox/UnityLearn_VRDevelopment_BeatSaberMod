using System;
using System.Collections.Generic;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x02000251 RID: 593
    public abstract class IFactoryProviderBase<TContract> : IProvider
    {
        // Token: 0x06000D99 RID: 3481 RVA: 0x000247A8 File Offset: 0x000229A8
        public IFactoryProviderBase(DiContainer container, Guid factoryId)
        {
            this.Container = container;
            this.FactoryId = factoryId;
        }

        // Token: 0x17000117 RID: 279
        // (get) Token: 0x06000D9A RID: 3482 RVA: 0x000247C0 File Offset: 0x000229C0
        public bool IsCached
        {
            get
            {
                return false;
            }
        }

        // Token: 0x17000118 RID: 280
        // (get) Token: 0x06000D9B RID: 3483 RVA: 0x000247C4 File Offset: 0x000229C4
        // (set) Token: 0x06000D9C RID: 3484 RVA: 0x000247CC File Offset: 0x000229CC
        protected Guid FactoryId { get; private set; }

        // Token: 0x17000119 RID: 281
        // (get) Token: 0x06000D9D RID: 3485 RVA: 0x000247D8 File Offset: 0x000229D8
        // (set) Token: 0x06000D9E RID: 3486 RVA: 0x000247E0 File Offset: 0x000229E0
        protected DiContainer Container { get; private set; }

        // Token: 0x1700011A RID: 282
        // (get) Token: 0x06000D9F RID: 3487 RVA: 0x000247EC File Offset: 0x000229EC
        public bool TypeVariesBasedOnMemberType
        {
            get
            {
                return false;
            }
        }

        // Token: 0x06000DA0 RID: 3488 RVA: 0x000247F0 File Offset: 0x000229F0
        public Type GetInstanceType(InjectContext context)
        {
            return typeof(TContract);
        }

        // Token: 0x06000DA1 RID: 3489
        public abstract void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer);

        // Token: 0x06000DA2 RID: 3490 RVA: 0x000247FC File Offset: 0x000229FC
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(IFactoryProviderBase<TContract>), new InjectTypeInfo.InjectConstructorInfo(null, new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }
}