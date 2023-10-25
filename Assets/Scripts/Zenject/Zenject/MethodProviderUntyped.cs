using System;
using System.Collections.Generic;
using ModestTree;
using static UnityEditor.Rendering.FilterWindow;

namespace Zenject
{
    // Token: 0x02000261 RID: 609
    [NoReflectionBaking]
    public class MethodProviderUntyped : IProvider
    {
        // Token: 0x06000DD9 RID: 3545 RVA: 0x0002575C File Offset: 0x0002395C
        public MethodProviderUntyped(Func<InjectContext, object> method, DiContainer container)
        {
            this._container = container;
            this._method = method;
        }

        // Token: 0x17000127 RID: 295
        // (get) Token: 0x06000DDA RID: 3546 RVA: 0x00025774 File Offset: 0x00023974
        public bool IsCached
        {
            get
            {
                return false;
            }
        }

        // Token: 0x17000128 RID: 296
        // (get) Token: 0x06000DDB RID: 3547 RVA: 0x00025778 File Offset: 0x00023978
        public bool TypeVariesBasedOnMemberType
        {
            get
            {
                return false;
            }
        }

        // Token: 0x06000DDC RID: 3548 RVA: 0x0002577C File Offset: 0x0002397C
        public Type GetInstanceType(InjectContext context)
        {
            return context.MemberType;
        }

        // Token: 0x06000DDD RID: 3549 RVA: 0x00025784 File Offset: 0x00023984
        public void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            ModestTree.Assert.IsEmpty<TypeValuePair>(args);
            ModestTree.Assert.IsNotNull(context);
            injectAction = null;
            if (this._container.IsValidating && !TypeAnalyzer.ShouldAllowDuringValidation(context.MemberType))
            {
                buffer.Add(new ValidationMarker(context.MemberType));
                return;
            }
            object obj = this._method(context);
            if (obj == null)
            {
                ModestTree.Assert.That(!context.MemberType.IsPrimitive(), "Invalid value returned from FromMethod.  Expected non-null.");
            }
            else
            {
                ModestTree.Assert.That(obj.GetType().DerivesFromOrEqual(context.MemberType));
            }
            buffer.Add(obj);
        }

        // Token: 0x04000411 RID: 1041
        private readonly DiContainer _container;

        // Token: 0x04000412 RID: 1042
        private readonly Func<InjectContext, object> _method;
    }
}
