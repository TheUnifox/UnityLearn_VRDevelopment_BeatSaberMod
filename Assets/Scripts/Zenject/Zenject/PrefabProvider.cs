using System;
using ModestTree;
using UnityEngine;

namespace Zenject
{
    // Token: 0x02000277 RID: 631
    [NoReflectionBaking]
    public class PrefabProvider : IPrefabProvider
    {
        // Token: 0x06000E3B RID: 3643 RVA: 0x0002702C File Offset: 0x0002522C
        public PrefabProvider(UnityEngine.Object prefab)
        {
            ModestTree.Assert.IsNotNull(prefab);
            this._prefab = prefab;
        }

        // Token: 0x06000E3C RID: 3644 RVA: 0x00027044 File Offset: 0x00025244
        public UnityEngine.Object GetPrefab()
        {
            return this._prefab;
        }

        // Token: 0x04000432 RID: 1074
        private readonly UnityEngine.Object _prefab;
    }
}
