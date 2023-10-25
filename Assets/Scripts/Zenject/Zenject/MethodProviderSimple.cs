using System;
using System.Collections.Generic;
using ModestTree;
using static UnityEditor.Rendering.FilterWindow;

namespace Zenject
{
    // Token: 0x02000260 RID: 608
    [NoReflectionBaking]
    public class MethodProviderSimple<TReturn> : IProvider
    {
        // Token: 0x06000DD4 RID: 3540 RVA: 0x000256E8 File Offset: 0x000238E8
        public MethodProviderSimple(Func<TReturn> method)
        {
            this._method = method;
        }

        // Token: 0x17000125 RID: 293
        // (get) Token: 0x06000DD5 RID: 3541 RVA: 0x000256F8 File Offset: 0x000238F8
        public bool IsCached
        {
            get
            {
                return false;
            }
        }

        // Token: 0x17000126 RID: 294
        // (get) Token: 0x06000DD6 RID: 3542 RVA: 0x000256FC File Offset: 0x000238FC
        public bool TypeVariesBasedOnMemberType
        {
            get
            {
                return false;
            }
        }

        // Token: 0x06000DD7 RID: 3543 RVA: 0x00025700 File Offset: 0x00023900
        public Type GetInstanceType(InjectContext context)
        {
            return typeof(TReturn);
        }

        // Token: 0x06000DD8 RID: 3544 RVA: 0x0002570C File Offset: 0x0002390C
        public void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            ModestTree.Assert.IsEmpty<TypeValuePair>(args);
            ModestTree.Assert.IsNotNull(context);
            ModestTree.Assert.That(typeof(TReturn).DerivesFromOrEqual(context.MemberType));
            injectAction = null;
            buffer.Add(this._method());
        }

        // Token: 0x04000410 RID: 1040
        private readonly Func<TReturn> _method;
    }
}