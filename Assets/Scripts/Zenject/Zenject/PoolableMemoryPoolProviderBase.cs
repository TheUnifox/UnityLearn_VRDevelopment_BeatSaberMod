using System;
using System.Collections.Generic;
using Zenject.Internal;
using static UnityEditor.Rendering.FilterWindow;

namespace Zenject
{
    // Token: 0x0200026A RID: 618
    public abstract class PoolableMemoryPoolProviderBase<TContract> : IProvider
    {
        // Token: 0x06000E06 RID: 3590 RVA: 0x0002635C File Offset: 0x0002455C
        public PoolableMemoryPoolProviderBase(DiContainer container, Guid poolId)
        {
            this.Container = container;
            this.PoolId = poolId;
        }

        // Token: 0x17000139 RID: 313
        // (get) Token: 0x06000E07 RID: 3591 RVA: 0x00026374 File Offset: 0x00024574
        public bool IsCached
        {
            get
            {
                return false;
            }
        }

        // Token: 0x1700013A RID: 314
        // (get) Token: 0x06000E08 RID: 3592 RVA: 0x00026378 File Offset: 0x00024578
        // (set) Token: 0x06000E09 RID: 3593 RVA: 0x00026380 File Offset: 0x00024580
        protected Guid PoolId { get; private set; }

        // Token: 0x1700013B RID: 315
        // (get) Token: 0x06000E0A RID: 3594 RVA: 0x0002638C File Offset: 0x0002458C
        // (set) Token: 0x06000E0B RID: 3595 RVA: 0x00026394 File Offset: 0x00024594
        protected DiContainer Container { get; private set; }

        // Token: 0x1700013C RID: 316
        // (get) Token: 0x06000E0C RID: 3596 RVA: 0x000263A0 File Offset: 0x000245A0
        public bool TypeVariesBasedOnMemberType
        {
            get
            {
                return false;
            }
        }

        // Token: 0x06000E0D RID: 3597 RVA: 0x000263A4 File Offset: 0x000245A4
        public Type GetInstanceType(InjectContext context)
        {
            return typeof(TContract);
        }

        // Token: 0x06000E0E RID: 3598
        public abstract void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer);

        // Token: 0x06000E0F RID: 3599 RVA: 0x000263B0 File Offset: 0x000245B0
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(PoolableMemoryPoolProviderBase<TContract>), new InjectTypeInfo.InjectConstructorInfo(null, new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }
}