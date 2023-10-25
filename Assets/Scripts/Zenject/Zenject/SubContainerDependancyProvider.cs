using System;
using System.Collections.Generic;
using ModestTree;
using static UnityEditor.Rendering.FilterWindow;

namespace Zenject
{
    // Token: 0x020002B8 RID: 696
    [NoReflectionBaking]
    public class SubContainerDependencyProvider : IProvider
    {
        // Token: 0x06000EFA RID: 3834 RVA: 0x0002A054 File Offset: 0x00028254
        public SubContainerDependencyProvider(Type dependencyType, object identifier, ISubContainerCreator subContainerCreator, bool resolveAll)
        {
            this._subContainerCreator = subContainerCreator;
            this._dependencyType = dependencyType;
            this._identifier = identifier;
            this._resolveAll = resolveAll;
        }

        // Token: 0x17000153 RID: 339
        // (get) Token: 0x06000EFB RID: 3835 RVA: 0x0002A07C File Offset: 0x0002827C
        public bool IsCached
        {
            get
            {
                return false;
            }
        }

        // Token: 0x17000154 RID: 340
        // (get) Token: 0x06000EFC RID: 3836 RVA: 0x0002A080 File Offset: 0x00028280
        public bool TypeVariesBasedOnMemberType
        {
            get
            {
                return false;
            }
        }

        // Token: 0x06000EFD RID: 3837 RVA: 0x0002A084 File Offset: 0x00028284
        public Type GetInstanceType(InjectContext context)
        {
            return this._dependencyType;
        }

        // Token: 0x06000EFE RID: 3838 RVA: 0x0002A08C File Offset: 0x0002828C
        private InjectContext CreateSubContext(InjectContext parent, DiContainer subContainer)
        {
            InjectContext injectContext = parent.CreateSubContext(this._dependencyType, this._identifier);
            injectContext.Container = subContainer;
            injectContext.SourceType = InjectSources.Local;
            return injectContext;
        }

        // Token: 0x06000EFF RID: 3839 RVA: 0x0002A0B0 File Offset: 0x000282B0
        public void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            ModestTree.Assert.IsNotNull(context);
            DiContainer diContainer = this._subContainerCreator.CreateSubContainer(args, context);
            InjectContext context2 = this.CreateSubContext(context, diContainer);
            injectAction = null;
            if (this._resolveAll)
            {
                diContainer.ResolveAll(context2, buffer);
                return;
            }
            buffer.Add(diContainer.Resolve(context2));
        }

        // Token: 0x040004A9 RID: 1193
        private readonly ISubContainerCreator _subContainerCreator;

        // Token: 0x040004AA RID: 1194
        private readonly Type _dependencyType;

        // Token: 0x040004AB RID: 1195
        private readonly object _identifier;

        // Token: 0x040004AC RID: 1196
        private readonly bool _resolveAll;
    }
}
