using System;
using System.Collections.Generic;
using ModestTree;
using UnityEngine;

namespace Zenject
{
    // Token: 0x0200024D RID: 589
    [NoReflectionBaking]
    public class InstantiateOnPrefabComponentProvider : IProvider
    {
        // Token: 0x06000D84 RID: 3460 RVA: 0x00024514 File Offset: 0x00022714
        public InstantiateOnPrefabComponentProvider(Type componentType, IPrefabInstantiator prefabInstantiator)
        {
            this._prefabInstantiator = prefabInstantiator;
            this._componentType = componentType;
        }

        // Token: 0x1700010F RID: 271
        // (get) Token: 0x06000D85 RID: 3461 RVA: 0x0002452C File Offset: 0x0002272C
        public bool IsCached
        {
            get
            {
                return false;
            }
        }

        // Token: 0x17000110 RID: 272
        // (get) Token: 0x06000D86 RID: 3462 RVA: 0x00024530 File Offset: 0x00022730
        public bool TypeVariesBasedOnMemberType
        {
            get
            {
                return false;
            }
        }

        // Token: 0x06000D87 RID: 3463 RVA: 0x00024534 File Offset: 0x00022734
        public Type GetInstanceType(InjectContext context)
        {
            return this._componentType;
        }

        // Token: 0x06000D88 RID: 3464 RVA: 0x0002453C File Offset: 0x0002273C
        public void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            ModestTree.Assert.IsNotNull(context);
            Component item = this._prefabInstantiator.Instantiate(context, args, out injectAction).AddComponent(this._componentType);
            buffer.Add(item);
        }

        // Token: 0x040003FA RID: 1018
        private readonly IPrefabInstantiator _prefabInstantiator;

        // Token: 0x040003FB RID: 1019
        private readonly Type _componentType;
    }
}
