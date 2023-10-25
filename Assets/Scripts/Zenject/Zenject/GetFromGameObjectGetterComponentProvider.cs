using System;
using System.Collections.Generic;
using ModestTree;
using UnityEngine;

namespace Zenject
{
    // Token: 0x0200024B RID: 587
    [NoReflectionBaking]
    public class GetFromGameObjectGetterComponentProvider : IProvider
    {
        // Token: 0x06000D7A RID: 3450 RVA: 0x00024364 File Offset: 0x00022564
        public GetFromGameObjectGetterComponentProvider(Type componentType, Func<InjectContext, GameObject> gameObjectGetter, bool matchSingle)
        {
            this._componentType = componentType;
            this._matchSingle = matchSingle;
            this._gameObjectGetter = gameObjectGetter;
        }

        // Token: 0x1700010B RID: 267
        // (get) Token: 0x06000D7B RID: 3451 RVA: 0x00024384 File Offset: 0x00022584
        public bool IsCached
        {
            get
            {
                return false;
            }
        }

        // Token: 0x1700010C RID: 268
        // (get) Token: 0x06000D7C RID: 3452 RVA: 0x00024388 File Offset: 0x00022588
        public bool TypeVariesBasedOnMemberType
        {
            get
            {
                return false;
            }
        }

        // Token: 0x06000D7D RID: 3453 RVA: 0x0002438C File Offset: 0x0002258C
        public Type GetInstanceType(InjectContext context)
        {
            return this._componentType;
        }

        // Token: 0x06000D7E RID: 3454 RVA: 0x00024394 File Offset: 0x00022594
        public void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            ModestTree.Assert.IsNotNull(context);
            injectAction = null;
            if (context.Container.IsValidating)
            {
                buffer.Add(new ValidationMarker(this._componentType));
                return;
            }
            GameObject gameObject = this._gameObjectGetter(context);
            if (this._matchSingle)
            {
                Component component = gameObject.GetComponent(this._componentType);
                ModestTree.Assert.IsNotNull(component, "Could not find component with type '{0}' on game object '{1}'", this._componentType, gameObject.name);
                buffer.Add(component);
                return;
            }
            Component[] components = gameObject.GetComponents(this._componentType);
            ModestTree.Assert.That(components.Length >= 1, "Expected to find at least one component with type '{0}' on prefab '{1}'", this._componentType, gameObject.name);
            buffer.AllocFreeAddRange(components);
        }

        // Token: 0x040003F4 RID: 1012
        private readonly Func<InjectContext, GameObject> _gameObjectGetter;

        // Token: 0x040003F5 RID: 1013
        private readonly Type _componentType;

        // Token: 0x040003F6 RID: 1014
        private readonly bool _matchSingle;
    }
}
