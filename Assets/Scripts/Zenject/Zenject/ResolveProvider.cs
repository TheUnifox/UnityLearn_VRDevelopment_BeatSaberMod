using System;
using System.Collections.Generic;
using ModestTree;
using static UnityEditor.Rendering.FilterWindow;

namespace Zenject
{
    // Token: 0x0200027A RID: 634
    [NoReflectionBaking]
    public class ResolveProvider : IProvider
    {
        // Token: 0x06000E40 RID: 3648 RVA: 0x000270A8 File Offset: 0x000252A8
        public ResolveProvider(Type contractType, DiContainer container, object identifier, bool isOptional, InjectSources source, bool matchAll)
        {
            this._contractType = contractType;
            this._identifier = identifier;
            this._container = container;
            this._isOptional = isOptional;
            this._source = source;
            this._matchAll = matchAll;
        }

        // Token: 0x17000146 RID: 326
        // (get) Token: 0x06000E41 RID: 3649 RVA: 0x000270E0 File Offset: 0x000252E0
        public bool IsCached
        {
            get
            {
                return false;
            }
        }

        // Token: 0x17000147 RID: 327
        // (get) Token: 0x06000E42 RID: 3650 RVA: 0x000270E4 File Offset: 0x000252E4
        public bool TypeVariesBasedOnMemberType
        {
            get
            {
                return false;
            }
        }

        // Token: 0x06000E43 RID: 3651 RVA: 0x000270E8 File Offset: 0x000252E8
        public Type GetInstanceType(InjectContext context)
        {
            return this._contractType;
        }

        // Token: 0x06000E44 RID: 3652 RVA: 0x000270F0 File Offset: 0x000252F0
        public void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            ModestTree.Assert.IsEmpty<TypeValuePair>(args);
            ModestTree.Assert.IsNotNull(context);
            ModestTree.Assert.That(this._contractType.DerivesFromOrEqual(context.MemberType));
            injectAction = null;
            if (this._matchAll)
            {
                this._container.ResolveAll(this.GetSubContext(context), buffer);
                return;
            }
            buffer.Add(this._container.Resolve(this.GetSubContext(context)));
        }

        // Token: 0x06000E45 RID: 3653 RVA: 0x00027158 File Offset: 0x00025358
        private InjectContext GetSubContext(InjectContext parent)
        {
            InjectContext injectContext = parent.CreateSubContext(this._contractType, this._identifier);
            injectContext.SourceType = this._source;
            injectContext.Optional = this._isOptional;
            return injectContext;
        }

        // Token: 0x04000434 RID: 1076
        private readonly object _identifier;

        // Token: 0x04000435 RID: 1077
        private readonly DiContainer _container;

        // Token: 0x04000436 RID: 1078
        private readonly Type _contractType;

        // Token: 0x04000437 RID: 1079
        private readonly bool _isOptional;

        // Token: 0x04000438 RID: 1080
        private readonly InjectSources _source;

        // Token: 0x04000439 RID: 1081
        private readonly bool _matchAll;
    }
}
