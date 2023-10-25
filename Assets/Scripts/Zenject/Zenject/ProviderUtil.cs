using System;
using ModestTree;

namespace Zenject
{
    // Token: 0x02000279 RID: 633
    public static class ProviderUtil
    {
        // Token: 0x06000E3F RID: 3647 RVA: 0x00027088 File Offset: 0x00025288
        public static Type GetTypeToInstantiate(Type contractType, Type concreteType)
        {
            if (concreteType.IsOpenGenericType())
            {
                return concreteType.MakeGenericType(contractType.GetGenericArguments());
            }
            ModestTree.Assert.DerivesFromOrEqual(concreteType, contractType);
            return concreteType;
        }
    }
}
