using System;
using System.Collections.Generic;
using ModestTree;
using UnityEngine;

namespace Zenject
{
    // Token: 0x020002A0 RID: 672
    [NoReflectionBaking]
    public class SubContainerCreatorByNewPrefab : ISubContainerCreator
    {
        // Token: 0x06000EB3 RID: 3763 RVA: 0x00028D90 File Offset: 0x00026F90
        public SubContainerCreatorByNewPrefab(DiContainer container, IPrefabProvider prefabProvider, GameObjectCreationParameters gameObjectBindInfo)
        {
            this._gameObjectBindInfo = gameObjectBindInfo;
            this._prefabProvider = prefabProvider;
            this._container = container;
        }

        // Token: 0x06000EB4 RID: 3764 RVA: 0x00028DB0 File Offset: 0x00026FB0
        public DiContainer CreateSubContainer(List<TypeValuePair> args, InjectContext parentContext)
        {
            ModestTree.Assert.That(args.IsEmpty<TypeValuePair>());
            UnityEngine.Object prefab = this._prefabProvider.GetPrefab();
            GameObjectContext component = this._container.InstantiatePrefab(prefab, this._gameObjectBindInfo).GetComponent<GameObjectContext>();
            ModestTree.Assert.That(component != null, "Expected prefab with name '{0}' to container a component of type 'GameObjectContext'", prefab.name);
            return component.Container;
        }

        // Token: 0x04000481 RID: 1153
        private readonly GameObjectCreationParameters _gameObjectBindInfo;

        // Token: 0x04000482 RID: 1154
        private readonly IPrefabProvider _prefabProvider;

        // Token: 0x04000483 RID: 1155
        private readonly DiContainer _container;
    }
}