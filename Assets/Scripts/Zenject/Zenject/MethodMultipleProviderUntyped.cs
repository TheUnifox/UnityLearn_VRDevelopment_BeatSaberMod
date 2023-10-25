using System;
using System.Collections.Generic;
using ModestTree;
using static UnityEditor.Rendering.FilterWindow;

namespace Zenject
{
    // Token: 0x0200025D RID: 605
    [NoReflectionBaking]
    public class MethodMultipleProviderUntyped : IProvider
    {
        // Token: 0x06000DC5 RID: 3525 RVA: 0x00025448 File Offset: 0x00023648
        public MethodMultipleProviderUntyped(Func<InjectContext, IEnumerable<object>> method, DiContainer container)
        {
            this._container = container;
            this._method = method;
        }

        // Token: 0x1700011F RID: 287
        // (get) Token: 0x06000DC6 RID: 3526 RVA: 0x00025460 File Offset: 0x00023660
        public bool IsCached
        {
            get
            {
                return false;
            }
        }

        // Token: 0x17000120 RID: 288
        // (get) Token: 0x06000DC7 RID: 3527 RVA: 0x00025464 File Offset: 0x00023664
        public bool TypeVariesBasedOnMemberType
        {
            get
            {
                return false;
            }
        }

        // Token: 0x06000DC8 RID: 3528 RVA: 0x00025468 File Offset: 0x00023668
        public Type GetInstanceType(InjectContext context)
        {
            return context.MemberType;
        }

        // Token: 0x06000DC9 RID: 3529 RVA: 0x00025470 File Offset: 0x00023670
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
            IEnumerable<object> enumerable = this._method(context);
            if (enumerable == null)
            {
                throw ModestTree.Assert.CreateException("Method '{0}' returned null when list was expected. Object graph:\n {1}", new object[]
                {
                    this._method.ToDebugString<InjectContext, IEnumerable<object>>(),
                    context.GetObjectGraphString()
                });
            }
            foreach (object item in enumerable)
            {
                buffer.Add(item);
            }
        }

        // Token: 0x0400040A RID: 1034
        private readonly DiContainer _container;

        // Token: 0x0400040B RID: 1035
        private readonly Func<InjectContext, IEnumerable<object>> _method;
    }
}
