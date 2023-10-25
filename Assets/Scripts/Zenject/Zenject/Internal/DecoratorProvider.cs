using System;
using System.Collections.Generic;
using ModestTree;
using static UnityEditor.Rendering.FilterWindow;

namespace Zenject.Internal
{
    // Token: 0x02000301 RID: 769
    [NoReflectionBaking]
    public class DecoratorProvider<TContract> : IDecoratorProvider
    {
        // Token: 0x0600108F RID: 4239 RVA: 0x0002E828 File Offset: 0x0002CA28
        public DecoratorProvider(DiContainer container)
        {
            this._container = container;
        }

        // Token: 0x06001090 RID: 4240 RVA: 0x0002E850 File Offset: 0x0002CA50
        public void AddFactoryId(Guid factoryBindId)
        {
            this._factoryBindIds.Add(factoryBindId);
        }

        // Token: 0x06001091 RID: 4241 RVA: 0x0002E860 File Offset: 0x0002CA60
        private void LazyInitializeDecoratorFactories()
        {
            if (this._decoratorFactories == null)
            {
                this._decoratorFactories = new List<IFactory<TContract, TContract>>();
                for (int i = 0; i < this._factoryBindIds.Count; i++)
                {
                    Guid guid = this._factoryBindIds[i];
                    IFactory<TContract, TContract> item = this._container.ResolveId<IFactory<TContract, TContract>>(guid);
                    this._decoratorFactories.Add(item);
                }
            }
        }

        // Token: 0x06001092 RID: 4242 RVA: 0x0002E8C4 File Offset: 0x0002CAC4
        public void GetAllInstances(IProvider provider, InjectContext context, List<object> buffer)
        {
            if (provider.IsCached)
            {
                List<object> list;
                if (!this._cachedInstances.TryGetValue(provider, out list))
                {
                    list = new List<object>();
                    this.WrapProviderInstances(provider, context, list);
                    this._cachedInstances.Add(provider, list);
                }
                buffer.AllocFreeAddRange(list);
                return;
            }
            this.WrapProviderInstances(provider, context, buffer);
        }

        // Token: 0x06001093 RID: 4243 RVA: 0x0002E918 File Offset: 0x0002CB18
        private void WrapProviderInstances(IProvider provider, InjectContext context, List<object> buffer)
        {
            this.LazyInitializeDecoratorFactories();
            provider.GetAllInstances(context, buffer);
            for (int i = 0; i < buffer.Count; i++)
            {
                buffer[i] = this.DecorateInstance(buffer[i], context);
            }
        }

        // Token: 0x06001094 RID: 4244 RVA: 0x0002E95C File Offset: 0x0002CB5C
        private object DecorateInstance(object instance, InjectContext context)
        {
            for (int i = 0; i < this._decoratorFactories.Count; i++)
            {
                instance = this._decoratorFactories[i].Create(context.Container.IsValidating ? default(TContract) : ((TContract)((object)instance)));
            }
            return instance;
        }

        // Token: 0x0400053D RID: 1341
        private readonly Dictionary<IProvider, List<object>> _cachedInstances = new Dictionary<IProvider, List<object>>();

        // Token: 0x0400053E RID: 1342
        private readonly DiContainer _container;

        // Token: 0x0400053F RID: 1343
        private readonly List<Guid> _factoryBindIds = new List<Guid>();

        // Token: 0x04000540 RID: 1344
        private List<IFactory<TContract, TContract>> _decoratorFactories;
    }
}
