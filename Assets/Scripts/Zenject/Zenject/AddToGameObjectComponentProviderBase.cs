using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x02000247 RID: 583
    [NoReflectionBaking]
    public abstract class AddToGameObjectComponentProviderBase : IProvider
    {
        // Token: 0x06000D65 RID: 3429 RVA: 0x00023FA0 File Offset: 0x000221A0
        public AddToGameObjectComponentProviderBase(DiContainer container, Type componentType, IEnumerable<TypeValuePair> extraArguments, object concreteIdentifier, Action<InjectContext, object> instantiateCallback)
        {
            ModestTree.Assert.That(componentType.DerivesFrom<Component>());
            this._extraArguments = extraArguments.ToList<TypeValuePair>();
            this._componentType = componentType;
            this._container = container;
            this._concreteIdentifier = concreteIdentifier;
            this._instantiateCallback = instantiateCallback;
        }

        // Token: 0x17000103 RID: 259
        // (get) Token: 0x06000D66 RID: 3430 RVA: 0x00023FE0 File Offset: 0x000221E0
        public bool IsCached
        {
            get
            {
                return false;
            }
        }

        // Token: 0x17000104 RID: 260
        // (get) Token: 0x06000D67 RID: 3431 RVA: 0x00023FE4 File Offset: 0x000221E4
        public bool TypeVariesBasedOnMemberType
        {
            get
            {
                return false;
            }
        }

        // Token: 0x17000105 RID: 261
        // (get) Token: 0x06000D68 RID: 3432 RVA: 0x00023FE8 File Offset: 0x000221E8
        protected DiContainer Container
        {
            get
            {
                return this._container;
            }
        }

        // Token: 0x17000106 RID: 262
        // (get) Token: 0x06000D69 RID: 3433 RVA: 0x00023FF0 File Offset: 0x000221F0
        protected Type ComponentType
        {
            get
            {
                return this._componentType;
            }
        }

        // Token: 0x17000107 RID: 263
        // (get) Token: 0x06000D6A RID: 3434
        protected abstract bool ShouldToggleActive { get; }

        // Token: 0x06000D6B RID: 3435 RVA: 0x00023FF8 File Offset: 0x000221F8
        public Type GetInstanceType(InjectContext context)
        {
            return this._componentType;
        }

        // Token: 0x06000D6C RID: 3436 RVA: 0x00024000 File Offset: 0x00022200
        public void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            ModestTree.Assert.IsNotNull(context);
            GameObject gameObj = this.GetGameObject(context);
            bool wasActive = gameObj.activeSelf;
            if (wasActive && this.ShouldToggleActive)
            {
                gameObj.SetActive(false);
            }
            object instance;
            if (!this._container.IsValidating || TypeAnalyzer.ShouldAllowDuringValidation(this._componentType))
            {
                if (this._componentType == typeof(Transform))
                {
                    instance = gameObj.transform;
                }
                else
                {
                    instance = gameObj.AddComponent(this._componentType);
                }
                ModestTree.Assert.IsNotNull(instance);
            }
            else
            {
                instance = new ValidationMarker(this._componentType);
            }
            injectAction = delegate ()
            {
                try
                {
                    List<TypeValuePair> list = Zenject.Internal.ZenPools.SpawnList<TypeValuePair>();
                    list.AllocFreeAddRange(this._extraArguments);
                    list.AllocFreeAddRange(args);
                    this._container.InjectExplicit(instance, this._componentType, list, context, this._concreteIdentifier);
                    ModestTree.Assert.That(list.Count == 0);
                    Zenject.Internal.ZenPools.DespawnList<TypeValuePair>(list);
                    if (this._instantiateCallback != null)
                    {
                        this._instantiateCallback(context, instance);
                    }
                }
                finally
                {
                    if (wasActive && this.ShouldToggleActive)
                    {
                        gameObj.SetActive(true);
                    }
                }
            };
            buffer.Add(instance);
        }

        // Token: 0x06000D6D RID: 3437
        protected abstract GameObject GetGameObject(InjectContext context);

        // Token: 0x040003E5 RID: 997
        private readonly Type _componentType;

        // Token: 0x040003E6 RID: 998
        private readonly DiContainer _container;

        // Token: 0x040003E7 RID: 999
        private readonly List<TypeValuePair> _extraArguments;

        // Token: 0x040003E8 RID: 1000
        private readonly object _concreteIdentifier;

        // Token: 0x040003E9 RID: 1001
        private readonly Action<InjectContext, object> _instantiateCallback;
    }
}
