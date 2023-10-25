using System;
using System.Collections.Generic;
using ModestTree;
using UnityEngine;

namespace Zenject
{
    // Token: 0x0200024C RID: 588
    [NoReflectionBaking]
    public class GetFromPrefabComponentProvider : IProvider
    {
        // Token: 0x06000D7F RID: 3455 RVA: 0x00024440 File Offset: 0x00022640
        public GetFromPrefabComponentProvider(Type componentType, IPrefabInstantiator prefabInstantiator, bool matchSingle)
        {
            this._prefabInstantiator = prefabInstantiator;
            this._componentType = componentType;
            this._matchSingle = matchSingle;
        }

        // Token: 0x1700010D RID: 269
        // (get) Token: 0x06000D80 RID: 3456 RVA: 0x00024460 File Offset: 0x00022660
        public bool IsCached
        {
            get
            {
                return false;
            }
        }

        // Token: 0x1700010E RID: 270
        // (get) Token: 0x06000D81 RID: 3457 RVA: 0x00024464 File Offset: 0x00022664
        public bool TypeVariesBasedOnMemberType
        {
            get
            {
                return false;
            }
        }

        // Token: 0x06000D82 RID: 3458 RVA: 0x00024468 File Offset: 0x00022668
        public Type GetInstanceType(InjectContext context)
        {
            return this._componentType;
        }

        // Token: 0x06000D83 RID: 3459 RVA: 0x00024470 File Offset: 0x00022670
        public void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            ModestTree.Assert.IsNotNull(context);
            GameObject gameObject = this._prefabInstantiator.Instantiate(context, args, out injectAction);
            if (this._matchSingle)
            {
                Component componentInChildren = gameObject.GetComponentInChildren(this._componentType, true);
                ModestTree.Assert.IsNotNull(componentInChildren, "Could not find component with type '{0}' on prefab '{1}'", this._componentType, this._prefabInstantiator.GetPrefab().name);
                buffer.Add(componentInChildren);
                return;
            }
            Component[] componentsInChildren = gameObject.GetComponentsInChildren(this._componentType, true);
            ModestTree.Assert.That(componentsInChildren.Length >= 1, "Expected to find at least one component with type '{0}' on prefab '{1}'", this._componentType, this._prefabInstantiator.GetPrefab().name);
            buffer.AllocFreeAddRange(componentsInChildren);
        }

        // Token: 0x040003F7 RID: 1015
        private readonly IPrefabInstantiator _prefabInstantiator;

        // Token: 0x040003F8 RID: 1016
        private readonly Type _componentType;

        // Token: 0x040003F9 RID: 1017
        private readonly bool _matchSingle;
    }
}
