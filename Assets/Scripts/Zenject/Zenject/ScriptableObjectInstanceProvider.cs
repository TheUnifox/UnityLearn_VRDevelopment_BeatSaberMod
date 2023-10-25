using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;
using Zenject.Internal;
using static UnityEditor.Rendering.FilterWindow;

namespace Zenject
{
    // Token: 0x0200027C RID: 636
    [NoReflectionBaking]
    public class ScriptableObjectInstanceProvider : IProvider
    {
        // Token: 0x06000E4B RID: 3659 RVA: 0x00027248 File Offset: 0x00025448
        public ScriptableObjectInstanceProvider(UnityEngine.Object resource, Type resourceType, DiContainer container, IEnumerable<TypeValuePair> extraArguments, bool createNew, object concreteIdentifier, Action<InjectContext, object> instantiateCallback)
        {
            this._container = container;
            ModestTree.Assert.DerivesFromOrEqual<ScriptableObject>(resourceType);
            this._resource = resource;
            this._extraArguments = extraArguments.ToList<TypeValuePair>();
            this._resourceType = resourceType;
            this._createNew = createNew;
            this._concreteIdentifier = concreteIdentifier;
            this._instantiateCallback = instantiateCallback;
        }

        // Token: 0x1700014A RID: 330
        // (get) Token: 0x06000E4C RID: 3660 RVA: 0x0002729C File Offset: 0x0002549C
        public bool IsCached
        {
            get
            {
                return false;
            }
        }

        // Token: 0x1700014B RID: 331
        // (get) Token: 0x06000E4D RID: 3661 RVA: 0x000272A0 File Offset: 0x000254A0
        public bool TypeVariesBasedOnMemberType
        {
            get
            {
                return false;
            }
        }

        // Token: 0x06000E4E RID: 3662 RVA: 0x000272A4 File Offset: 0x000254A4
        public Type GetInstanceType(InjectContext context)
        {
            return this._resourceType;
        }

        // Token: 0x06000E4F RID: 3663 RVA: 0x000272AC File Offset: 0x000254AC
        public void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            ModestTree.Assert.IsNotNull(context);
            if (this._createNew)
            {
                buffer.Add(UnityEngine.Object.Instantiate(this._resource));
            }
            else
            {
                buffer.Add(this._resource);
            }
            injectAction = delegate ()
            {
                for (int i = 0; i < buffer.Count; i++)
                {
                    object obj = buffer[i];
                    List<TypeValuePair> list = Zenject.Internal.ZenPools.SpawnList<TypeValuePair>();
                    list.AllocFreeAddRange(this._extraArguments);
                    list.AllocFreeAddRange(args);
                    this._container.InjectExplicit(obj, this._resourceType, list, context, this._concreteIdentifier);
                    Zenject.Internal.ZenPools.DespawnList<TypeValuePair>(list);
                    if (this._instantiateCallback != null)
                    {
                        this._instantiateCallback(context, obj);
                    }
                }
            };
        }

        // Token: 0x0400043D RID: 1085
        private readonly DiContainer _container;

        // Token: 0x0400043E RID: 1086
        private readonly Type _resourceType;

        // Token: 0x0400043F RID: 1087
        private readonly List<TypeValuePair> _extraArguments;

        // Token: 0x04000440 RID: 1088
        private readonly bool _createNew;

        // Token: 0x04000441 RID: 1089
        private readonly object _concreteIdentifier;

        // Token: 0x04000442 RID: 1090
        private readonly Action<InjectContext, object> _instantiateCallback;

        // Token: 0x04000443 RID: 1091
        private readonly UnityEngine.Object _resource;
    }
}
