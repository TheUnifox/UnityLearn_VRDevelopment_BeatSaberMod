using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

namespace Zenject
{
    // Token: 0x02000078 RID: 120
    [NoReflectionBaking]
    public class FactoryFromBinder<TContract> : FactoryFromBinderBase
    {
        // Token: 0x06000313 RID: 787 RVA: 0x00008F00 File Offset: 0x00007100
        public FactoryFromBinder(DiContainer container, BindInfo bindInfo, FactoryBindInfo factoryBindInfo) : base(container, typeof(TContract), bindInfo, factoryBindInfo)
        {
        }

        // Token: 0x06000314 RID: 788 RVA: 0x00008F18 File Offset: 0x00007118
        public ConditionCopyNonLazyBinder FromResolveGetter<TObj>(Func<TObj, TContract> method)
        {
            return this.FromResolveGetter<TObj>(null, method);
        }

        // Token: 0x06000315 RID: 789 RVA: 0x00008F24 File Offset: 0x00007124
        public ConditionCopyNonLazyBinder FromResolveGetter<TObj>(object subIdentifier, Func<TObj, TContract> method)
        {
            return this.FromResolveGetter<TObj>(subIdentifier, method, InjectSources.Any);
        }

        // Token: 0x06000316 RID: 790 RVA: 0x00008F30 File Offset: 0x00007130
        public ConditionCopyNonLazyBinder FromResolveGetter<TObj>(object subIdentifier, Func<TObj, TContract> method, InjectSources source)
        {
            base.FactoryBindInfo.ProviderFunc = ((DiContainer container) => new GetterProvider<TObj, TContract>(subIdentifier, method, container, source, false));
            return this;
        }

        // Token: 0x06000317 RID: 791 RVA: 0x00008F70 File Offset: 0x00007170
        public ConditionCopyNonLazyBinder FromMethod(Func<DiContainer, TContract> method)
        {
            base.ProviderFunc = ((DiContainer container) => new MethodProviderWithContainer<TContract>(method));
            return this;
        }

        // Token: 0x06000318 RID: 792 RVA: 0x00008FA0 File Offset: 0x000071A0
        public ArgConditionCopyNonLazyBinder FromFactory<TSubFactory>() where TSubFactory : IFactory<TContract>
        {
            return this.FromIFactory(delegate (ConcreteBinderGeneric<IFactory<TContract>> x)
            {
                x.To<TSubFactory>().AsCached();
            });
        }

        // Token: 0x06000319 RID: 793 RVA: 0x00008FC8 File Offset: 0x000071C8
        public FactorySubContainerBinder<TContract> FromSubContainerResolve()
        {
            return this.FromSubContainerResolve(null);
        }

        // Token: 0x0600031A RID: 794 RVA: 0x00008FD4 File Offset: 0x000071D4
        public FactorySubContainerBinder<TContract> FromSubContainerResolve(object subIdentifier)
        {
            return new FactorySubContainerBinder<TContract>(base.BindContainer, base.BindInfo, base.FactoryBindInfo, subIdentifier);
        }

        // Token: 0x0600031B RID: 795 RVA: 0x00008FF0 File Offset: 0x000071F0
        public ConditionCopyNonLazyBinder FromComponentInHierarchy(bool includeInactive = true)
        {
            BindingUtil.AssertIsInterfaceOrComponent(ContractType);

            return FromMethod(_ =>
            {
                var res = BindContainer.Resolve<Context>().GetRootGameObjects()
                    .Select(x => x.GetComponentInChildren<TContract>(includeInactive))
                    .Where(x => x != null).FirstOrDefault();

                Assert.IsNotNull(res,
                    "Could not find component '{0}' through FromComponentInHierarchy factory binding", typeof(TContract));

                return res;
            });
        }
    }

    [NoReflectionBaking]
    public class FactoryFromBinder<TParam1, TContract> : FactoryFromBinderBase
    {
        // Token: 0x06000351 RID: 849 RVA: 0x00009718 File Offset: 0x00007918
        public FactoryFromBinder(DiContainer container, BindInfo bindInfo, FactoryBindInfo factoryBindInfo) : base(container, typeof(TContract), bindInfo, factoryBindInfo)
        {
        }

        // Token: 0x06000352 RID: 850 RVA: 0x00009730 File Offset: 0x00007930
        public ConditionCopyNonLazyBinder FromMethod(Func<DiContainer, TParam1, TContract> method)
        {
            base.ProviderFunc = ((DiContainer container) => new MethodProviderWithContainer<TParam1, TContract>(method));
            return this;
        }

        // Token: 0x06000353 RID: 851 RVA: 0x00009760 File Offset: 0x00007960
        public ConditionCopyNonLazyBinder FromFactory<TSubFactory>() where TSubFactory : IFactory<TParam1, TContract>
        {
            return this.FromIFactory(delegate (ConcreteBinderGeneric<IFactory<TParam1, TContract>> x)
            {
                x.To<TSubFactory>().AsCached();
            });
        }

        // Token: 0x06000354 RID: 852 RVA: 0x00009788 File Offset: 0x00007988
        public FactorySubContainerBinder<TParam1, TContract> FromSubContainerResolve()
        {
            return this.FromSubContainerResolve(null);
        }

        // Token: 0x06000355 RID: 853 RVA: 0x00009794 File Offset: 0x00007994
        public FactorySubContainerBinder<TParam1, TContract> FromSubContainerResolve(object subIdentifier)
        {
            return new FactorySubContainerBinder<TParam1, TContract>(base.BindContainer, base.BindInfo, base.FactoryBindInfo, subIdentifier);
        }
    }

    [NoReflectionBaking]
    public class FactoryFromBinder<TParam1, TParam2, TContract> : FactoryFromBinderBase
    {
        // Token: 0x06000390 RID: 912 RVA: 0x00009EE0 File Offset: 0x000080E0
        public FactoryFromBinder(DiContainer container, BindInfo bindInfo, FactoryBindInfo factoryBindInfo) : base(container, typeof(TContract), bindInfo, factoryBindInfo)
        {
        }

        // Token: 0x06000391 RID: 913 RVA: 0x00009EF8 File Offset: 0x000080F8
        public ConditionCopyNonLazyBinder FromMethod(Func<DiContainer, TParam1, TParam2, TContract> method)
        {
            base.ProviderFunc = ((DiContainer container) => new MethodProviderWithContainer<TParam1, TParam2, TContract>(method));
            return this;
        }

        // Token: 0x06000392 RID: 914 RVA: 0x00009F28 File Offset: 0x00008128
        public ConditionCopyNonLazyBinder FromFactory<TSubFactory>() where TSubFactory : IFactory<TParam1, TParam2, TContract>
        {
            return this.FromIFactory(delegate (ConcreteBinderGeneric<IFactory<TParam1, TParam2, TContract>> x)
            {
                x.To<TSubFactory>().AsCached();
            });
        }

        // Token: 0x06000393 RID: 915 RVA: 0x00009F50 File Offset: 0x00008150
        public FactorySubContainerBinder<TParam1, TParam2, TContract> FromSubContainerResolve()
        {
            return this.FromSubContainerResolve(null);
        }

        // Token: 0x06000394 RID: 916 RVA: 0x00009F5C File Offset: 0x0000815C
        public FactorySubContainerBinder<TParam1, TParam2, TContract> FromSubContainerResolve(object subIdentifier)
        {
            return new FactorySubContainerBinder<TParam1, TParam2, TContract>(base.BindContainer, base.BindInfo, base.FactoryBindInfo, subIdentifier);
        }
    }

    [NoReflectionBaking]
    public class FactoryFromBinder<TParam1, TParam2, TParam3, TContract> : FactoryFromBinderBase
    {
        // Token: 0x060003BC RID: 956 RVA: 0x0000A440 File Offset: 0x00008640
        public FactoryFromBinder(DiContainer container, BindInfo bindInfo, FactoryBindInfo factoryBindInfo) : base(container, typeof(TContract), bindInfo, factoryBindInfo)
        {
        }

        // Token: 0x060003BD RID: 957 RVA: 0x0000A458 File Offset: 0x00008658
        public ConditionCopyNonLazyBinder FromMethod(Func<DiContainer, TParam1, TParam2, TParam3, TContract> method)
        {
            base.ProviderFunc = ((DiContainer container) => new MethodProviderWithContainer<TParam1, TParam2, TParam3, TContract>(method));
            return this;
        }

        // Token: 0x060003BE RID: 958 RVA: 0x0000A488 File Offset: 0x00008688
        public ConditionCopyNonLazyBinder FromFactory<TSubFactory>() where TSubFactory : IFactory<TParam1, TParam2, TParam3, TContract>
        {
            return this.FromIFactory(delegate (ConcreteBinderGeneric<IFactory<TParam1, TParam2, TParam3, TContract>> x)
            {
                x.To<TSubFactory>().AsCached();
            });
        }

        // Token: 0x060003BF RID: 959 RVA: 0x0000A4B0 File Offset: 0x000086B0
        public FactorySubContainerBinder<TParam1, TParam2, TParam3, TContract> FromSubContainerResolve()
        {
            return this.FromSubContainerResolve(null);
        }

        // Token: 0x060003C0 RID: 960 RVA: 0x0000A4BC File Offset: 0x000086BC
        public FactorySubContainerBinder<TParam1, TParam2, TParam3, TContract> FromSubContainerResolve(object subIdentifier)
        {
            return new FactorySubContainerBinder<TParam1, TParam2, TParam3, TContract>(base.BindContainer, base.BindInfo, base.FactoryBindInfo, subIdentifier);
        }
    }

    [NoReflectionBaking]
    public class FactoryFromBinder<TParam1, TParam2, TParam3, TParam4, TContract> : FactoryFromBinderBase
    {
        // Token: 0x060003E8 RID: 1000 RVA: 0x0000A9A0 File Offset: 0x00008BA0
        public FactoryFromBinder(DiContainer container, BindInfo bindInfo, FactoryBindInfo factoryBindInfo) : base(container, typeof(TContract), bindInfo, factoryBindInfo)
        {
        }

        // Token: 0x060003E9 RID: 1001 RVA: 0x0000A9B8 File Offset: 0x00008BB8
        public ConditionCopyNonLazyBinder FromMethod(Func<DiContainer, TParam1, TParam2, TParam3, TParam4, TContract> method)
        {
            base.ProviderFunc = ((DiContainer container) => new MethodProviderWithContainer<TParam1, TParam2, TParam3, TParam4, TContract>(method));
            return this;
        }

        // Token: 0x060003EA RID: 1002 RVA: 0x0000A9E8 File Offset: 0x00008BE8
        public ConditionCopyNonLazyBinder FromFactory<TSubFactory>() where TSubFactory : IFactory<TParam1, TParam2, TParam3, TParam4, TContract>
        {
            return this.FromIFactory(delegate (ConcreteBinderGeneric<IFactory<TParam1, TParam2, TParam3, TParam4, TContract>> x)
            {
                x.To<TSubFactory>().AsCached();
            });
        }

        // Token: 0x060003EB RID: 1003 RVA: 0x0000AA10 File Offset: 0x00008C10
        public FactorySubContainerBinder<TParam1, TParam2, TParam3, TParam4, TContract> FromSubContainerResolve()
        {
            return this.FromSubContainerResolve(null);
        }

        // Token: 0x060003EC RID: 1004 RVA: 0x0000AA1C File Offset: 0x00008C1C
        public FactorySubContainerBinder<TParam1, TParam2, TParam3, TParam4, TContract> FromSubContainerResolve(object subIdentifier)
        {
            return new FactorySubContainerBinder<TParam1, TParam2, TParam3, TParam4, TContract>(base.BindContainer, base.BindInfo, base.FactoryBindInfo, subIdentifier);
        }
    }

    [NoReflectionBaking]
    public class FactoryFromBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TContract> : FactoryFromBinderBase
    {
        // Token: 0x06000414 RID: 1044 RVA: 0x0000AF00 File Offset: 0x00009100
        public FactoryFromBinder(DiContainer container, BindInfo bindInfo, FactoryBindInfo factoryBindInfo) : base(container, typeof(TContract), bindInfo, factoryBindInfo)
        {
        }

        // Token: 0x06000415 RID: 1045 RVA: 0x0000AF18 File Offset: 0x00009118
        public ConditionCopyNonLazyBinder FromMethod(Func<DiContainer, TParam1, TParam2, TParam3, TParam4, TParam5, TContract> method)
        {
            base.ProviderFunc = ((DiContainer container) => new MethodProviderWithContainer<TParam1, TParam2, TParam3, TParam4, TParam5, TContract>(method));
            return this;
        }

        // Token: 0x06000416 RID: 1046 RVA: 0x0000AF48 File Offset: 0x00009148
        public ConditionCopyNonLazyBinder FromFactory<TSubFactory>() where TSubFactory : IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TContract>
        {
            return this.FromIFactory(delegate (ConcreteBinderGeneric<IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TContract>> x)
            {
                x.To<TSubFactory>().AsCached();
            });
        }

        // Token: 0x06000417 RID: 1047 RVA: 0x0000AF70 File Offset: 0x00009170
        public FactorySubContainerBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TContract> FromSubContainerResolve()
        {
            return this.FromSubContainerResolve(null);
        }

        // Token: 0x06000418 RID: 1048 RVA: 0x0000AF7C File Offset: 0x0000917C
        public FactorySubContainerBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TContract> FromSubContainerResolve(object subIdentifier)
        {
            return new FactorySubContainerBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TContract>(base.BindContainer, base.BindInfo, base.FactoryBindInfo, subIdentifier);
        }
    }

    [NoReflectionBaking]
    public class FactoryFromBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract> : FactoryFromBinderBase
    {
        // Token: 0x06000440 RID: 1088 RVA: 0x0000B460 File Offset: 0x00009660
        public FactoryFromBinder(DiContainer container, BindInfo bindInfo, FactoryBindInfo factoryBindInfo) : base(container, typeof(TContract), bindInfo, factoryBindInfo)
        {
        }

        // Token: 0x06000441 RID: 1089 RVA: 0x0000B478 File Offset: 0x00009678
        public ConditionCopyNonLazyBinder FromMethod(Func<DiContainer, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract> method)
        {
            base.ProviderFunc = ((DiContainer container) => new MethodProviderWithContainer<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract>(method));
            return this;
        }

        // Token: 0x06000442 RID: 1090 RVA: 0x0000B4A8 File Offset: 0x000096A8
        public ConditionCopyNonLazyBinder FromFactory<TSubFactory>() where TSubFactory : IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract>
        {
            return this.FromIFactory(delegate (ConcreteBinderGeneric<IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract>> x)
            {
                x.To<TSubFactory>().AsCached();
            });
        }

        // Token: 0x06000443 RID: 1091 RVA: 0x0000B4D0 File Offset: 0x000096D0
        public FactorySubContainerBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract> FromSubContainerResolve()
        {
            return this.FromSubContainerResolve(null);
        }

        // Token: 0x06000444 RID: 1092 RVA: 0x0000B4DC File Offset: 0x000096DC
        public FactorySubContainerBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract> FromSubContainerResolve(object subIdentifier)
        {
            return new FactorySubContainerBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract>(base.BindContainer, base.BindInfo, base.FactoryBindInfo, subIdentifier);
        }
    }

    [NoReflectionBaking]
    public class FactoryFromBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract> : FactoryFromBinderBase
    {
        // Token: 0x0600037D RID: 893 RVA: 0x00009C78 File Offset: 0x00007E78
        public FactoryFromBinder(DiContainer container, BindInfo bindInfo, FactoryBindInfo factoryBindInfo) : base(container, typeof(TContract), bindInfo, factoryBindInfo)
        {
        }

        // Token: 0x0600037E RID: 894 RVA: 0x00009C90 File Offset: 0x00007E90
        public ConditionCopyNonLazyBinder FromMethod(Func<DiContainer, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract> method)
        {
            base.ProviderFunc = ((DiContainer container) => new MethodProviderWithContainer<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract>(method));
            return this;
        }

        // Token: 0x0600037F RID: 895 RVA: 0x00009CC0 File Offset: 0x00007EC0
        public ConditionCopyNonLazyBinder FromFactory<TSubFactory>() where TSubFactory : IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract>
        {
            return this.FromIFactory(delegate (ConcreteBinderGeneric<IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract>> x)
            {
                x.To<TSubFactory>().AsCached();
            });
        }

        // Token: 0x06000380 RID: 896 RVA: 0x00009CE8 File Offset: 0x00007EE8
        public ArgConditionCopyNonLazyBinder FromIFactory(Action<ConcreteBinderGeneric<IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract>>> factoryBindGenerator)
        {
            Guid factoryId;
            factoryBindGenerator(base.CreateIFactoryBinder<IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract>>(out factoryId));
            base.ProviderFunc = ((DiContainer container) => new IFactoryProvider<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract>(container, factoryId));
            return new ArgConditionCopyNonLazyBinder(base.BindInfo);
        }

        // Token: 0x06000381 RID: 897 RVA: 0x00009D2C File Offset: 0x00007F2C
        public FactorySubContainerBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract> FromSubContainerResolve()
        {
            return this.FromSubContainerResolve(null);
        }

        // Token: 0x06000382 RID: 898 RVA: 0x00009D38 File Offset: 0x00007F38
        public FactorySubContainerBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract> FromSubContainerResolve(object subIdentifier)
        {
            return new FactorySubContainerBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract>(base.BindContainer, base.BindInfo, base.FactoryBindInfo, subIdentifier);
        }
    }
}
