using System;
using System.Collections.Generic;
using ModestTree;
using UnityEngine;
using static UnityEditor.Rendering.FilterWindow;

namespace Zenject
{
    // Token: 0x0200027B RID: 635
    [NoReflectionBaking]
    public class ResourceProvider : IProvider
    {
        // Token: 0x06000E46 RID: 3654 RVA: 0x00027184 File Offset: 0x00025384
        public ResourceProvider(string resourcePath, Type resourceType, bool matchSingle)
        {
            this._resourceType = resourceType;
            this._resourcePath = resourcePath;
            this._matchSingle = matchSingle;
        }

        // Token: 0x17000148 RID: 328
        // (get) Token: 0x06000E47 RID: 3655 RVA: 0x000271A4 File Offset: 0x000253A4
        public bool IsCached
        {
            get
            {
                return false;
            }
        }

        // Token: 0x17000149 RID: 329
        // (get) Token: 0x06000E48 RID: 3656 RVA: 0x000271A8 File Offset: 0x000253A8
        public bool TypeVariesBasedOnMemberType
        {
            get
            {
                return false;
            }
        }

        // Token: 0x06000E49 RID: 3657 RVA: 0x000271AC File Offset: 0x000253AC
        public Type GetInstanceType(InjectContext context)
        {
            return this._resourceType;
        }

        // Token: 0x06000E4A RID: 3658 RVA: 0x000271B4 File Offset: 0x000253B4
        public void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            ModestTree.Assert.IsEmpty<TypeValuePair>(args);
            ModestTree.Assert.IsNotNull(context);
            if (this._matchSingle)
            {
                UnityEngine.Object @object = Resources.Load(this._resourcePath, this._resourceType);
                ModestTree.Assert.That(@object != null, "Could not find resource at path '{0}' with type '{1}'", this._resourcePath, this._resourceType);
                injectAction = null;
                buffer.Add(@object);
                return;
            }
            UnityEngine.Object[] array = Resources.LoadAll(this._resourcePath, this._resourceType);
            ModestTree.Assert.That(array.Length != 0, "Could not find resource at path '{0}' with type '{1}'", this._resourcePath, this._resourceType);
            injectAction = null;
            buffer.AllocFreeAddRange(array);
        }

        // Token: 0x0400043A RID: 1082
        private readonly Type _resourceType;

        // Token: 0x0400043B RID: 1083
        private readonly string _resourcePath;

        // Token: 0x0400043C RID: 1084
        private readonly bool _matchSingle;
    }
}
