using System;
using System.Collections.Generic;
using ModestTree;

namespace Zenject
{
    // Token: 0x02000250 RID: 592
    [NoReflectionBaking]
    public class GetterProvider<TObj, TResult> : IProvider
    {
        // Token: 0x06000D93 RID: 3475 RVA: 0x0002461C File Offset: 0x0002281C
        public GetterProvider(object identifier, Func<TObj, TResult> method, DiContainer container, InjectSources sourceType, bool matchAll)
        {
            this._container = container;
            this._identifier = identifier;
            this._method = method;
            this._matchAll = matchAll;
            this._sourceType = sourceType;
        }

        // Token: 0x17000115 RID: 277
        // (get) Token: 0x06000D94 RID: 3476 RVA: 0x0002464C File Offset: 0x0002284C
        public bool IsCached
        {
            get
            {
                return false;
            }
        }

        // Token: 0x17000116 RID: 278
        // (get) Token: 0x06000D95 RID: 3477 RVA: 0x00024650 File Offset: 0x00022850
        public bool TypeVariesBasedOnMemberType
        {
            get
            {
                return false;
            }
        }

        // Token: 0x06000D96 RID: 3478 RVA: 0x00024654 File Offset: 0x00022854
        public Type GetInstanceType(InjectContext context)
        {
            return typeof(TResult);
        }

        // Token: 0x06000D97 RID: 3479 RVA: 0x00024660 File Offset: 0x00022860
        private InjectContext GetSubContext(InjectContext parent)
        {
            InjectContext injectContext = parent.CreateSubContext(typeof(TObj), this._identifier);
            injectContext.Optional = false;
            injectContext.SourceType = this._sourceType;
            return injectContext;
        }

        // Token: 0x06000D98 RID: 3480 RVA: 0x0002468C File Offset: 0x0002288C
        public void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            ModestTree.Assert.IsEmpty<TypeValuePair>(args);
            ModestTree.Assert.IsNotNull(context);
            ModestTree.Assert.That(typeof(TResult).DerivesFromOrEqual(context.MemberType));
            injectAction = null;
            if (this._container.IsValidating)
            {
                if (this._matchAll)
                {
                    this._container.ResolveAll(this.GetSubContext(context));
                }
                else
                {
                    this._container.Resolve(this.GetSubContext(context));
                }
                buffer.Add(new ValidationMarker(typeof(TResult)));
                return;
            }
            if (this._matchAll)
            {
                ModestTree.Assert.That(buffer.Count == 0);
                this._container.ResolveAll(this.GetSubContext(context), buffer);
                for (int i = 0; i < buffer.Count; i++)
                {
                    buffer[i] = this._method((TObj)((object)buffer[i]));
                }
                return;
            }
            buffer.Add(this._method((TObj)((object)this._container.Resolve(this.GetSubContext(context)))));
        }

        // Token: 0x040003FF RID: 1023
        private readonly DiContainer _container;

        // Token: 0x04000400 RID: 1024
        private readonly object _identifier;

        // Token: 0x04000401 RID: 1025
        private readonly Func<TObj, TResult> _method;

        // Token: 0x04000402 RID: 1026
        private readonly bool _matchAll;

        // Token: 0x04000403 RID: 1027
        private readonly InjectSources _sourceType;
    }
}
