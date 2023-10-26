using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using Zenject.Internal;
using static UnityEditor.Rendering.FilterWindow;

namespace Zenject
{
    // Token: 0x020001A9 RID: 425
    public abstract class PlaceholderFactoryBase<TValue> : IPlaceholderFactory, IValidatable
    {
        // Token: 0x060008DE RID: 2270 RVA: 0x00018108 File Offset: 0x00016308
        [Inject]
        private void Construct(IProvider provider, InjectContext injectContext)
        {
            ModestTree.Assert.IsNotNull(provider);
            ModestTree.Assert.IsNotNull(injectContext);
            this._provider = provider;
            this._injectContext = injectContext;
        }

        // Token: 0x060008DF RID: 2271 RVA: 0x00018124 File Offset: 0x00016324
        protected TValue CreateInternal(List<TypeValuePair> extraArgs)
        {
            TValue tvalue;
            try
            {
                object instance = this._provider.GetInstance(this._injectContext, extraArgs);
                if (this._injectContext.Container.IsValidating && instance is ValidationMarker)
                {
                    tvalue = default(TValue);
                }
                else
                {
                    ModestTree.Assert.That(instance == null || instance.GetType().DerivesFromOrEqual<TValue>());
                    tvalue = (TValue)((object)instance);
                }
            }
            catch (Exception innerException)
            {
                throw new ZenjectException("Error during construction of type '{0}' via {1}.Create method!".Fmt(new object[]
                {
                    typeof(TValue),
                    base.GetType()
                }), innerException);
            }
            return tvalue;
        }

        // Token: 0x060008E0 RID: 2272 RVA: 0x000181C8 File Offset: 0x000163C8
        public virtual void Validate()
        {
            this._provider.GetInstance(this._injectContext, ValidationUtil.CreateDefaultArgs(this.ParamTypes.ToArray<Type>()));
        }

        // Token: 0x17000077 RID: 119
        // (get) Token: 0x060008E1 RID: 2273
        protected abstract IEnumerable<Type> ParamTypes { get; }

        // Token: 0x060008E3 RID: 2275 RVA: 0x000181F4 File Offset: 0x000163F4
        private static void __zenInjectMethod0(object P_0, object[] P_1)
        {
            ((PlaceholderFactoryBase<TValue>)P_0).Construct((IProvider)P_1[0], (InjectContext)P_1[1]);
        }

        // Token: 0x060008E4 RID: 2276 RVA: 0x0001821C File Offset: 0x0001641C
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(PlaceholderFactoryBase<TValue>), new InjectTypeInfo.InjectConstructorInfo(null, new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[]
            {
                new InjectTypeInfo.InjectMethodInfo(new ZenInjectMethod(PlaceholderFactoryBase<TValue>.__zenInjectMethod0), new InjectableInfo[]
                {
                    new InjectableInfo(false, null, "provider", typeof(IProvider), null, InjectSources.Any),
                    new InjectableInfo(false, null, "injectContext", typeof(InjectContext), null, InjectSources.Any)
                }, "Construct")
            }, new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x040002DC RID: 732
        private IProvider _provider;

        // Token: 0x040002DD RID: 733
        private InjectContext _injectContext;
    }
}
