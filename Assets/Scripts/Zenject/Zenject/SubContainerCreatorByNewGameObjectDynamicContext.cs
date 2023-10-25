using System;
using UnityEngine;

namespace Zenject
{
    // Token: 0x0200028E RID: 654
    [NoReflectionBaking]
    public abstract class SubContainerCreatorByNewGameObjectDynamicContext : SubContainerCreatorDynamicContext
    {
        // Token: 0x06000E7F RID: 3711 RVA: 0x00027F60 File Offset: 0x00026160
        public SubContainerCreatorByNewGameObjectDynamicContext(DiContainer container, GameObjectCreationParameters gameObjectBindInfo) : base(container)
        {
            this._gameObjectBindInfo = gameObjectBindInfo;
        }

        // Token: 0x06000E80 RID: 3712 RVA: 0x00027F70 File Offset: 0x00026170
        protected override GameObject CreateGameObject(out bool shouldMakeActive)
        {
            shouldMakeActive = true;
            GameObject gameObject = base.Container.CreateEmptyGameObject(this._gameObjectBindInfo, null);
            gameObject.SetActive(false);
            return gameObject;
        }

        // Token: 0x04000466 RID: 1126
        private readonly GameObjectCreationParameters _gameObjectBindInfo;
    }
}