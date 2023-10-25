using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x02000273 RID: 627
    [NoReflectionBaking]
    public class PrefabInstantiator : IPrefabInstantiator
    {
        // Token: 0x06000E2A RID: 3626 RVA: 0x00026C58 File Offset: 0x00024E58
        public PrefabInstantiator(DiContainer container, GameObjectCreationParameters gameObjectBindInfo, Type argumentTarget, IEnumerable<Type> instantiateCallbackTypes, IEnumerable<TypeValuePair> extraArguments, IPrefabProvider prefabProvider, Action<InjectContext, object> instantiateCallback)
        {
            this._prefabProvider = prefabProvider;
            this._extraArguments = extraArguments.ToList<TypeValuePair>();
            this._container = container;
            this._gameObjectBindInfo = gameObjectBindInfo;
            this._argumentTarget = argumentTarget;
            this._instantiateCallbackTypes = instantiateCallbackTypes.ToList<Type>();
            this._instantiateCallback = instantiateCallback;
        }

        // Token: 0x17000140 RID: 320
        // (get) Token: 0x06000E2B RID: 3627 RVA: 0x00026CAC File Offset: 0x00024EAC
        public GameObjectCreationParameters GameObjectCreationParameters
        {
            get
            {
                return this._gameObjectBindInfo;
            }
        }

        // Token: 0x17000141 RID: 321
        // (get) Token: 0x06000E2C RID: 3628 RVA: 0x00026CB4 File Offset: 0x00024EB4
        public Type ArgumentTarget
        {
            get
            {
                return this._argumentTarget;
            }
        }

        // Token: 0x17000142 RID: 322
        // (get) Token: 0x06000E2D RID: 3629 RVA: 0x00026CBC File Offset: 0x00024EBC
        public List<TypeValuePair> ExtraArguments
        {
            get
            {
                return this._extraArguments;
            }
        }

        // Token: 0x06000E2E RID: 3630 RVA: 0x00026CC4 File Offset: 0x00024EC4
        public UnityEngine.Object GetPrefab()
        {
            return this._prefabProvider.GetPrefab();
        }

        // Token: 0x06000E2F RID: 3631 RVA: 0x00026CD4 File Offset: 0x00024ED4
        public GameObject Instantiate(InjectContext context, List<TypeValuePair> args, out Action injectAction)
        {
            ModestTree.Assert.That(this._argumentTarget == null || this._argumentTarget.DerivesFromOrEqual(context.MemberType));
            bool shouldMakeActive;
            GameObject gameObject = this._container.CreateAndParentPrefab(this.GetPrefab(), this._gameObjectBindInfo, context, out shouldMakeActive);
            ModestTree.Assert.IsNotNull(gameObject);
            injectAction = delegate ()
            {
                List<TypeValuePair> list = Zenject.Internal.ZenPools.SpawnList<TypeValuePair>();
                list.AllocFreeAddRange(this._extraArguments);
                list.AllocFreeAddRange(args);
                if (this._argumentTarget == null)
                {
                    ModestTree.Assert.That(list.IsEmpty<TypeValuePair>(), "Unexpected arguments provided to prefab instantiator.  Arguments are not allowed if binding multiple components in the same binding");
                }
                if (this._argumentTarget == null || list.IsEmpty<TypeValuePair>())
                {
                    this._container.InjectGameObject(gameObject);
                }
                else
                {
                    this._container.InjectGameObjectForComponentExplicit(gameObject, this._argumentTarget, list, context, null);
                    ModestTree.Assert.That(list.Count == 0);
                }
                Zenject.Internal.ZenPools.DespawnList<TypeValuePair>(list);
                if (shouldMakeActive && !this._container.IsValidating)
                {
                    gameObject.SetActive(true);
                }
                if (this._instantiateCallback != null)
                {
                    HashSet<object> hashSet = Zenject.Internal.ZenPools.SpawnHashSet<object>();
                    foreach (Type type in this._instantiateCallbackTypes)
                    {
                        Component componentInChildren = gameObject.GetComponentInChildren(type);
                        if (componentInChildren != null)
                        {
                            hashSet.Add(componentInChildren);
                        }
                    }
                    foreach (object arg in hashSet)
                    {
                        this._instantiateCallback(context, arg);
                    }
                    Zenject.Internal.ZenPools.DespawnHashSet<object>(hashSet);
                }
            };
            return gameObject;
        }

        // Token: 0x04000424 RID: 1060
        private readonly IPrefabProvider _prefabProvider;

        // Token: 0x04000425 RID: 1061
        private readonly DiContainer _container;

        // Token: 0x04000426 RID: 1062
        private readonly List<TypeValuePair> _extraArguments;

        // Token: 0x04000427 RID: 1063
        private readonly GameObjectCreationParameters _gameObjectBindInfo;

        // Token: 0x04000428 RID: 1064
        private readonly Type _argumentTarget;

        // Token: 0x04000429 RID: 1065
        private readonly List<Type> _instantiateCallbackTypes;

        // Token: 0x0400042A RID: 1066
        private readonly Action<InjectContext, object> _instantiateCallback;
    }
}
