using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Rendering.FilterWindow;

namespace Zenject
{
    // Token: 0x0200024F RID: 591
    [NoReflectionBaking]
    public class PrefabGameObjectProvider : IProvider
    {
        // Token: 0x06000D8E RID: 3470 RVA: 0x000245D4 File Offset: 0x000227D4
        public PrefabGameObjectProvider(IPrefabInstantiator prefabCreator)
        {
            this._prefabCreator = prefabCreator;
        }

        // Token: 0x17000113 RID: 275
        // (get) Token: 0x06000D8F RID: 3471 RVA: 0x000245E4 File Offset: 0x000227E4
        public bool IsCached
        {
            get
            {
                return false;
            }
        }

        // Token: 0x17000114 RID: 276
        // (get) Token: 0x06000D90 RID: 3472 RVA: 0x000245E8 File Offset: 0x000227E8
        public bool TypeVariesBasedOnMemberType
        {
            get
            {
                return false;
            }
        }

        // Token: 0x06000D91 RID: 3473 RVA: 0x000245EC File Offset: 0x000227EC
        public Type GetInstanceType(InjectContext context)
        {
            return typeof(GameObject);
        }

        // Token: 0x06000D92 RID: 3474 RVA: 0x000245F8 File Offset: 0x000227F8
        public void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            GameObject item = this._prefabCreator.Instantiate(context, args, out injectAction);
            buffer.Add(item);
        }

        // Token: 0x040003FE RID: 1022
        private readonly IPrefabInstantiator _prefabCreator;
    }
}
