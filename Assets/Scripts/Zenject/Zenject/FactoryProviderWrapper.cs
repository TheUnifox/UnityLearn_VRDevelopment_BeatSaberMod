using System;
using ModestTree;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x0200017C RID: 380
    public class FactoryProviderWrapper<TContract> : IFactory<TContract>, IFactory
    {
        // Token: 0x06000811 RID: 2065 RVA: 0x00016064 File Offset: 0x00014264
        public FactoryProviderWrapper(IProvider provider, InjectContext injectContext)
        {
            ModestTree.Assert.That(injectContext.MemberType.DerivesFromOrEqual<TContract>());
            this._provider = provider;
            this._injectContext = injectContext;
        }

        // Token: 0x06000812 RID: 2066 RVA: 0x0001608C File Offset: 0x0001428C
        public TContract Create()
        {
            object instance = this._provider.GetInstance(this._injectContext);
            if (this._injectContext.Container.IsValidating)
            {
                return default(TContract);
            }
            ModestTree.Assert.That(instance == null || instance.GetType().DerivesFromOrEqual(this._injectContext.MemberType));
            return (TContract)((object)instance);
        }

        // Token: 0x06000813 RID: 2067 RVA: 0x000160F0 File Offset: 0x000142F0
        private static object __zenCreate(object[] P_0)
        {
            return new FactoryProviderWrapper<TContract>((IProvider)P_0[0], (InjectContext)P_0[1]);
        }

        // Token: 0x06000814 RID: 2068 RVA: 0x00016120 File Offset: 0x00014320
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(FactoryProviderWrapper<TContract>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(FactoryProviderWrapper<TContract>.__zenCreate), new InjectableInfo[]
            {
                new InjectableInfo(false, null, "provider", typeof(IProvider), null, InjectSources.Any),
                new InjectableInfo(false, null, "injectContext", typeof(InjectContext), null, InjectSources.Any)
            }), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x040002BB RID: 699
        private readonly IProvider _provider;

        // Token: 0x040002BC RID: 700
        private readonly InjectContext _injectContext;
    }
}
