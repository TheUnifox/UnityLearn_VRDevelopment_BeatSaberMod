using System;
using ModestTree;
using UnityEngine;

namespace Zenject
{
    // Token: 0x020002A1 RID: 673
    [NoReflectionBaking]
    public abstract class SubContainerCreatorByNewPrefabDynamicContext : SubContainerCreatorDynamicContext
    {
        // Token: 0x06000EB5 RID: 3765 RVA: 0x00028E08 File Offset: 0x00027008
        public SubContainerCreatorByNewPrefabDynamicContext(DiContainer container, IPrefabProvider prefabProvider, GameObjectCreationParameters gameObjectBindInfo) : base(container)
        {
            this._prefabProvider = prefabProvider;
            this._gameObjectBindInfo = gameObjectBindInfo;
        }

        // Token: 0x06000EB6 RID: 3766 RVA: 0x00028E20 File Offset: 0x00027020
        protected override GameObject CreateGameObject(out bool shouldMakeActive)
        {
            UnityEngine.Object prefab = this._prefabProvider.GetPrefab();
            GameObject gameObject = base.Container.CreateAndParentPrefab(prefab, this._gameObjectBindInfo, null, out shouldMakeActive);
            if (gameObject.GetComponent<GameObjectContext>() != null)
            {
                throw ModestTree.Assert.CreateException("Found GameObjectContext already attached to prefab with name '{0}'!  When using ByNewPrefabMethod or ByNewPrefabInstaller, the GameObjectContext is added to the prefab dynamically", new object[]
                {
                    prefab.name
                });
            }
            return gameObject;
        }

        // Token: 0x04000484 RID: 1156
        private readonly IPrefabProvider _prefabProvider;

        // Token: 0x04000485 RID: 1157
        private readonly GameObjectCreationParameters _gameObjectBindInfo;
    }
}
