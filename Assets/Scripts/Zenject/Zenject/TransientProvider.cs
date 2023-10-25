using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using Zenject.Internal;
using static UnityEditor.Rendering.FilterWindow;

namespace Zenject
{
    // Token: 0x020002B9 RID: 697
    [NoReflectionBaking]
    public class TransientProvider : IProvider
    {
        // Token: 0x06000F00 RID: 3840 RVA: 0x0002A100 File Offset: 0x00028300
        public TransientProvider(Type concreteType, DiContainer container, IEnumerable<TypeValuePair> extraArguments, string bindingContext, object concreteIdentifier, Action<InjectContext, object> instantiateCallback)
        {
            ModestTree.Assert.That(!concreteType.IsAbstract(), "Expected non-abstract type for given binding but instead found type '{0}'{1}", concreteType, (bindingContext == null) ? "" : " when binding '{0}'".Fmt(new object[]
            {
                bindingContext
            }));
            this._container = container;
            this._concreteType = concreteType;
            this._extraArguments = extraArguments.ToList<TypeValuePair>();
            this._concreteIdentifier = concreteIdentifier;
            this._instantiateCallback = instantiateCallback;
        }

        // Token: 0x17000155 RID: 341
        // (get) Token: 0x06000F01 RID: 3841 RVA: 0x0002A174 File Offset: 0x00028374
        public bool IsCached
        {
            get
            {
                return false;
            }
        }

        // Token: 0x17000156 RID: 342
        // (get) Token: 0x06000F02 RID: 3842 RVA: 0x0002A178 File Offset: 0x00028378
        public bool TypeVariesBasedOnMemberType
        {
            get
            {
                return this._concreteType.IsOpenGenericType();
            }
        }

        // Token: 0x06000F03 RID: 3843 RVA: 0x0002A188 File Offset: 0x00028388
        public Type GetInstanceType(InjectContext context)
        {
            if (!this._concreteType.DerivesFromOrEqual(context.MemberType))
            {
                return null;
            }
            return this.GetTypeToCreate(context.MemberType);
        }

        // Token: 0x06000F04 RID: 3844 RVA: 0x0002A1AC File Offset: 0x000283AC
        public void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            ModestTree.Assert.IsNotNull(context);
            Type instanceType = this.GetTypeToCreate(context.MemberType);
            List<TypeValuePair> extraArgs = Zenject.Internal.ZenPools.SpawnList<TypeValuePair>();
            extraArgs.AllocFreeAddRange(this._extraArguments);
            extraArgs.AllocFreeAddRange(args);
            object instance = this._container.InstantiateExplicit(instanceType, false, extraArgs, context, this._concreteIdentifier);
            injectAction = delegate ()
            {
                this._container.InjectExplicit(instance, instanceType, extraArgs, context, this._concreteIdentifier);
                ModestTree.Assert.That(extraArgs.Count == 0);
                Zenject.Internal.ZenPools.DespawnList<TypeValuePair>(extraArgs);
                if (this._instantiateCallback != null)
                {
                    this._instantiateCallback(context, instance);
                }
            };
            buffer.Add(instance);
        }

        // Token: 0x06000F05 RID: 3845 RVA: 0x0002A25C File Offset: 0x0002845C
        private Type GetTypeToCreate(Type contractType)
        {
            return ProviderUtil.GetTypeToInstantiate(contractType, this._concreteType);
        }

        // Token: 0x040004AD RID: 1197
        private readonly DiContainer _container;

        // Token: 0x040004AE RID: 1198
        private readonly Type _concreteType;

        // Token: 0x040004AF RID: 1199
        private readonly List<TypeValuePair> _extraArguments;

        // Token: 0x040004B0 RID: 1200
        private readonly object _concreteIdentifier;

        // Token: 0x040004B1 RID: 1201
        private readonly Action<InjectContext, object> _instantiateCallback;
    }
}
