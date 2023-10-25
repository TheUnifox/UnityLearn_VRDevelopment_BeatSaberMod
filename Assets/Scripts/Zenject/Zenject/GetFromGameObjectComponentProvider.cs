using System;
using System.Collections.Generic;
using ModestTree;
using UnityEngine;

namespace Zenject
{
    // Token: 0x0200024A RID: 586
    [NoReflectionBaking]
    public class GetFromGameObjectComponentProvider : IProvider
    {
        // Token: 0x06000D75 RID: 3445 RVA: 0x000242A0 File Offset: 0x000224A0
        public GetFromGameObjectComponentProvider(Type componentType, GameObject gameObject, bool matchSingle)
        {
            this._componentType = componentType;
            this._matchSingle = matchSingle;
            this._gameObject = gameObject;
        }

        // Token: 0x17000109 RID: 265
        // (get) Token: 0x06000D76 RID: 3446 RVA: 0x000242C0 File Offset: 0x000224C0
        public bool IsCached
        {
            get
            {
                return false;
            }
        }

        // Token: 0x1700010A RID: 266
        // (get) Token: 0x06000D77 RID: 3447 RVA: 0x000242C4 File Offset: 0x000224C4
        public bool TypeVariesBasedOnMemberType
        {
            get
            {
                return false;
            }
        }

        // Token: 0x06000D78 RID: 3448 RVA: 0x000242C8 File Offset: 0x000224C8
        public Type GetInstanceType(InjectContext context)
        {
            return this._componentType;
        }

        // Token: 0x06000D79 RID: 3449 RVA: 0x000242D0 File Offset: 0x000224D0
        public void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            ModestTree.Assert.IsNotNull(context);
            injectAction = null;
            if (this._matchSingle)
            {
                Component component = this._gameObject.GetComponent(this._componentType);
                ModestTree.Assert.IsNotNull(component, "Could not find component with type '{0}' on prefab '{1}'", this._componentType, this._gameObject.name);
                buffer.Add(component);
                return;
            }
            Component[] components = this._gameObject.GetComponents(this._componentType);
            ModestTree.Assert.That(components.Length >= 1, "Expected to find at least one component with type '{0}' on prefab '{1}'", this._componentType, this._gameObject.name);
            buffer.AllocFreeAddRange(components);
        }

        // Token: 0x040003F1 RID: 1009
        private readonly GameObject _gameObject;

        // Token: 0x040003F2 RID: 1010
        private readonly Type _componentType;

        // Token: 0x040003F3 RID: 1011
        private readonly bool _matchSingle;
    }
}
