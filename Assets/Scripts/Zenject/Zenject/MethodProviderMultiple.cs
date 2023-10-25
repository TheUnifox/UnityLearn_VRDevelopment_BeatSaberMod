using System;
using System.Collections.Generic;
using ModestTree;

namespace Zenject
{
    // Token: 0x0200025F RID: 607
    [NoReflectionBaking]
    public class MethodProviderMultiple<TReturn> : IProvider
    {
        // Token: 0x06000DCF RID: 3535 RVA: 0x000255DC File Offset: 0x000237DC
        public MethodProviderMultiple(Func<InjectContext, IEnumerable<TReturn>> method, DiContainer container)
        {
            this._container = container;
            this._method = method;
        }

        // Token: 0x17000123 RID: 291
        // (get) Token: 0x06000DD0 RID: 3536 RVA: 0x000255F4 File Offset: 0x000237F4
        public bool IsCached
        {
            get
            {
                return false;
            }
        }

        // Token: 0x17000124 RID: 292
        // (get) Token: 0x06000DD1 RID: 3537 RVA: 0x000255F8 File Offset: 0x000237F8
        public bool TypeVariesBasedOnMemberType
        {
            get
            {
                return false;
            }
        }

        // Token: 0x06000DD2 RID: 3538 RVA: 0x000255FC File Offset: 0x000237FC
        public Type GetInstanceType(InjectContext context)
        {
            return typeof(TReturn);
        }

        // Token: 0x06000DD3 RID: 3539 RVA: 0x00025608 File Offset: 0x00023808
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
            IEnumerable<TReturn> enumerable = this._method(context);
            if (enumerable == null)
            {
                throw ModestTree.Assert.CreateException("Method '{0}' returned null when list was expected. Object graph:\n {1}", new object[]
                {
                    this._method.ToDebugString<InjectContext, IEnumerable<TReturn>>(),
                    context.GetObjectGraphString()
                });
            }
            foreach (TReturn treturn in enumerable)
            {
                buffer.Add(treturn);
            }
        }

        // Token: 0x0400040E RID: 1038
        private readonly DiContainer _container;

        // Token: 0x0400040F RID: 1039
        private readonly Func<InjectContext, IEnumerable<TReturn>> _method;
    }
}
