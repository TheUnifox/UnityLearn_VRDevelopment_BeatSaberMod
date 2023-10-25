using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x02000243 RID: 579
    [NoReflectionBaking]
    public class AddToCurrentGameObjectComponentProvider : IProvider
    {
        // Token: 0x06000D54 RID: 3412 RVA: 0x00023CD0 File Offset: 0x00021ED0
        public AddToCurrentGameObjectComponentProvider(DiContainer container, Type componentType, IEnumerable<TypeValuePair> extraArguments, object concreteIdentifier, Action<InjectContext, object> instantiateCallback)
        {
            ModestTree.Assert.That(componentType.DerivesFrom<Component>());
            this._extraArguments = extraArguments.ToList<TypeValuePair>();
            this._componentType = componentType;
            this._container = container;
            this._concreteIdentifier = concreteIdentifier;
            this._instantiateCallback = instantiateCallback;
        }

        // Token: 0x170000FD RID: 253
        // (get) Token: 0x06000D55 RID: 3413 RVA: 0x00023D10 File Offset: 0x00021F10
        public bool IsCached
        {
            get
            {
                return false;
            }
        }

        // Token: 0x170000FE RID: 254
        // (get) Token: 0x06000D56 RID: 3414 RVA: 0x00023D14 File Offset: 0x00021F14
        public bool TypeVariesBasedOnMemberType
        {
            get
            {
                return false;
            }
        }

        // Token: 0x170000FF RID: 255
        // (get) Token: 0x06000D57 RID: 3415 RVA: 0x00023D18 File Offset: 0x00021F18
        protected DiContainer Container
        {
            get
            {
                return this._container;
            }
        }

        // Token: 0x17000100 RID: 256
        // (get) Token: 0x06000D58 RID: 3416 RVA: 0x00023D20 File Offset: 0x00021F20
        protected Type ComponentType
        {
            get
            {
                return this._componentType;
            }
        }

        // Token: 0x06000D59 RID: 3417 RVA: 0x00023D28 File Offset: 0x00021F28
        public Type GetInstanceType(InjectContext context)
        {
            return this._componentType;
        }

        // Token: 0x06000D5A RID: 3418 RVA: 0x00023D30 File Offset: 0x00021F30
        public void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            ModestTree.Assert.IsNotNull(context);
            ModestTree.Assert.That(context.ObjectType.DerivesFrom<Component>(), "Object '{0}' can only be injected into MonoBehaviour's since it was bound with 'FromNewComponentSibling'. Attempted to inject into non-MonoBehaviour '{1}'", context.MemberType, context.ObjectType);
            object instance;
            if (!this._container.IsValidating || TypeAnalyzer.ShouldAllowDuringValidation(this._componentType))
            {
                GameObject gameObject = ((Component)context.ObjectInstance).gameObject;
                Component component = gameObject.GetComponent(this._componentType);
                instance = component;
                if (component != null)
                {
                    injectAction = null;
                    buffer.Add(instance);
                    return;
                }
                instance = gameObject.AddComponent(this._componentType);
            }
            else
            {
                instance = new ValidationMarker(this._componentType);
            }
            injectAction = delegate ()
            {
                List<TypeValuePair> list = Zenject.Internal.ZenPools.SpawnList<TypeValuePair>();
                list.AllocFreeAddRange(this._extraArguments);
                list.AllocFreeAddRange(args);
                this._container.InjectExplicit(instance, this._componentType, list, context, this._concreteIdentifier);
                ModestTree.Assert.That(list.IsEmpty<TypeValuePair>());
                Zenject.Internal.ZenPools.DespawnList<TypeValuePair>(list);
                if (this._instantiateCallback != null)
                {
                    this._instantiateCallback(context, instance);
                }
            };
            buffer.Add(instance);
        }

        // Token: 0x040003DA RID: 986
        private readonly Type _componentType;

        // Token: 0x040003DB RID: 987
        private readonly DiContainer _container;

        // Token: 0x040003DC RID: 988
        private readonly List<TypeValuePair> _extraArguments;

        // Token: 0x040003DD RID: 989
        private readonly object _concreteIdentifier;

        // Token: 0x040003DE RID: 990
        private readonly Action<InjectContext, object> _instantiateCallback;
    }
}
