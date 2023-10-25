using System;
using System.Collections.Generic;
using ModestTree;

namespace Zenject
{
    // Token: 0x0200025C RID: 604
    [NoReflectionBaking]
    public class InstanceProvider : IProvider
    {
        // Token: 0x06000DBF RID: 3519 RVA: 0x000253B0 File Offset: 0x000235B0
        public InstanceProvider(Type instanceType, object instance, DiContainer container)
        {
            this._instanceType = instanceType;
            this._instance = instance;
            this._container = container;
        }

        // Token: 0x1700011D RID: 285
        // (get) Token: 0x06000DC0 RID: 3520 RVA: 0x000253D0 File Offset: 0x000235D0
        public bool IsCached
        {
            get
            {
                return true;
            }
        }

        // Token: 0x1700011E RID: 286
        // (get) Token: 0x06000DC1 RID: 3521 RVA: 0x000253D4 File Offset: 0x000235D4
        public bool TypeVariesBasedOnMemberType
        {
            get
            {
                return false;
            }
        }

        // Token: 0x06000DC2 RID: 3522 RVA: 0x000253D8 File Offset: 0x000235D8
        public Type GetInstanceType(InjectContext context)
        {
            return this._instanceType;
        }

        // Token: 0x06000DC3 RID: 3523 RVA: 0x000253E0 File Offset: 0x000235E0
        public void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            ModestTree.Assert.That(args.Count == 0);
            ModestTree.Assert.IsNotNull(context);
            ModestTree.Assert.That(this._instanceType.DerivesFromOrEqual(context.MemberType));
            injectAction = delegate ()
            {
                this._container.LazyInject<object>(this._instance);
            };
            buffer.Add(this._instance);
        }

        // Token: 0x04000407 RID: 1031
        private readonly object _instance;

        // Token: 0x04000408 RID: 1032
        private readonly Type _instanceType;

        // Token: 0x04000409 RID: 1033
        private readonly DiContainer _container;
    }
}
