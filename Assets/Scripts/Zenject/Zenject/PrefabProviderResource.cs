using System;
using ModestTree;
using UnityEngine;

namespace Zenject
{
    // Token: 0x02000278 RID: 632
    [NoReflectionBaking]
    public class PrefabProviderResource : IPrefabProvider
    {
        // Token: 0x06000E3D RID: 3645 RVA: 0x0002704C File Offset: 0x0002524C
        public PrefabProviderResource(string resourcePath)
        {
            this._resourcePath = resourcePath;
        }

        // Token: 0x06000E3E RID: 3646 RVA: 0x0002705C File Offset: 0x0002525C
        public UnityEngine.Object GetPrefab()
        {
            GameObject gameObject = (GameObject)Resources.Load(this._resourcePath);
            ModestTree.Assert.That(gameObject != null, "Expected to find prefab at resource path '{0}'", this._resourcePath);
            return gameObject;
        }

        // Token: 0x04000433 RID: 1075
        private readonly string _resourcePath;
    }
}
