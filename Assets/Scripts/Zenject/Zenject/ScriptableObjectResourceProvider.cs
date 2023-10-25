using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;
using Zenject.Internal;
using static UnityEditor.Rendering.FilterWindow;

namespace Zenject
{
    // Token: 0x0200027E RID: 638
    [NoReflectionBaking]
    public class ScriptableObjectResourceProvider : IProvider
    {
        // Token: 0x06000E54 RID: 3668 RVA: 0x0002744C File Offset: 0x0002564C
        public ScriptableObjectResourceProvider(string resourcePath, Type resourceType, DiContainer container, IEnumerable<TypeValuePair> extraArguments, bool createNew, object concreteIdentifier, Action<InjectContext, object> instantiateCallback)
        {
            this._container = container;
            ModestTree.Assert.DerivesFromOrEqual<ScriptableObject>(resourceType);
            this._extraArguments = extraArguments.ToList<TypeValuePair>();
            this._resourceType = resourceType;
            this._resourcePath = resourcePath;
            this._createNew = createNew;
            this._concreteIdentifier = concreteIdentifier;
            this._instantiateCallback = instantiateCallback;
        }

        // Token: 0x1700014C RID: 332
        // (get) Token: 0x06000E55 RID: 3669 RVA: 0x000274A0 File Offset: 0x000256A0
        public bool IsCached
        {
            get
            {
                return false;
            }
        }

        // Token: 0x1700014D RID: 333
        // (get) Token: 0x06000E56 RID: 3670 RVA: 0x000274A4 File Offset: 0x000256A4
        public bool TypeVariesBasedOnMemberType
        {
            get
            {
                return false;
            }
        }

        // Token: 0x06000E57 RID: 3671 RVA: 0x000274A8 File Offset: 0x000256A8
        public Type GetInstanceType(InjectContext context)
        {
            return this._resourceType;
        }

        // Token: 0x06000E58 RID: 3672 RVA: 0x000274B0 File Offset: 0x000256B0
        public void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            ModestTree.Assert.IsNotNull(context);
            if (this._createNew)
            {
                UnityEngine.Object[] array = Resources.LoadAll(this._resourcePath, this._resourceType);
                for (int i = 0; i < array.Length; i++)
                {
                    buffer.Add(UnityEngine.Object.Instantiate(array[i]));
                }
            }
            else
            {
                buffer.AllocFreeAddRange(Resources.LoadAll(this._resourcePath, this._resourceType));
            }
            ModestTree.Assert.That(buffer.Count > 0, "Could not find resource at path '{0}' with type '{1}'", this._resourcePath, this._resourceType);
            injectAction = delegate ()
            {
                for (int j = 0; j < buffer.Count; j++)
                {
                    object obj = buffer[j];
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

        // Token: 0x04000448 RID: 1096
        private readonly DiContainer _container;

        // Token: 0x04000449 RID: 1097
        private readonly Type _resourceType;

        // Token: 0x0400044A RID: 1098
        private readonly string _resourcePath;

        // Token: 0x0400044B RID: 1099
        private readonly List<TypeValuePair> _extraArguments;

        // Token: 0x0400044C RID: 1100
        private readonly bool _createNew;

        // Token: 0x0400044D RID: 1101
        private readonly object _concreteIdentifier;

        // Token: 0x0400044E RID: 1102
        private readonly Action<InjectContext, object> _instantiateCallback;
    }
}
