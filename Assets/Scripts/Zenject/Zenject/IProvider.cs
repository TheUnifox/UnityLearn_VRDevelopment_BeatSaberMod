using System;
using System.Collections.Generic;

namespace Zenject
{
    // Token: 0x0200025A RID: 602
    public interface IProvider
    {
        // Token: 0x1700011B RID: 283
        // (get) Token: 0x06000DB3 RID: 3507
        bool TypeVariesBasedOnMemberType { get; }

        // Token: 0x1700011C RID: 284
        // (get) Token: 0x06000DB4 RID: 3508
        bool IsCached { get; }

        // Token: 0x06000DB5 RID: 3509
        Type GetInstanceType(InjectContext context);

        // Token: 0x06000DB6 RID: 3510
        void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> instances);
    }
}
