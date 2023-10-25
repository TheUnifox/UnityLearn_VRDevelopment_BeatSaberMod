using System;
using System.Collections.Generic;
using ModestTree;
using static UnityEditor.Rendering.FilterWindow;

namespace Zenject
{
    // Token: 0x0200025E RID: 606
    [NoReflectionBaking]
    public class MethodProvider<TReturn> : IProvider
    {
        // Token: 0x06000DCA RID: 3530 RVA: 0x00025530 File Offset: 0x00023730
        public MethodProvider(Func<InjectContext, TReturn> method, DiContainer container)
        {
            this._container = container;
            this._method = method;
        }

        // Token: 0x17000121 RID: 289
        // (get) Token: 0x06000DCB RID: 3531 RVA: 0x00025548 File Offset: 0x00023748
        public bool IsCached
        {
            get
            {
                return false;
            }
        }

        // Token: 0x17000122 RID: 290
        // (get) Token: 0x06000DCC RID: 3532 RVA: 0x0002554C File Offset: 0x0002374C
        public bool TypeVariesBasedOnMemberType
        {
            get
            {
                return false;
            }
        }

        // Token: 0x06000DCD RID: 3533 RVA: 0x00025550 File Offset: 0x00023750
        public Type GetInstanceType(InjectContext context)
        {
            return typeof(TReturn);
        }

        // Token: 0x06000DCE RID: 3534 RVA: 0x0002555C File Offset: 0x0002375C
        public void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            ModestTree.Assert.IsEmpty<TypeValuePair>(args);
            ModestTree.Assert.IsNotNull(context);
            ModestTree.Assert.That(typeof(TReturn).DerivesFromOrEqual(context.MemberType));
            injectAction = null;
            if (this._container.IsValidating && !TypeAnalyzer.ShouldAllowDuringValidation(context.MemberType))
            {
                buffer.Add(new ValidationMarker(typeof(TReturn)));
                return;
            }
            buffer.Add(this._method(context));
        }

        // Token: 0x0400040C RID: 1036
        private readonly DiContainer _container;

        // Token: 0x0400040D RID: 1037
        private readonly Func<InjectContext, TReturn> _method;
    }
}
