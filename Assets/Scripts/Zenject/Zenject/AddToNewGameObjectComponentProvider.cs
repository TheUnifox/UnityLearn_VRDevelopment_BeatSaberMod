using System;
using System.Collections.Generic;
using UnityEngine;

namespace Zenject
{
    // Token: 0x02000249 RID: 585
    [NoReflectionBaking]
    public class AddToNewGameObjectComponentProvider : AddToGameObjectComponentProviderBase
    {
        // Token: 0x06000D72 RID: 3442 RVA: 0x0002424C File Offset: 0x0002244C
        public AddToNewGameObjectComponentProvider(DiContainer container, Type componentType, IEnumerable<TypeValuePair> extraArguments, GameObjectCreationParameters gameObjectBindInfo, object concreteIdentifier, Action<InjectContext, object> instantiateCallback) : base(container, componentType, extraArguments, concreteIdentifier, instantiateCallback)
        {
            this._gameObjectBindInfo = gameObjectBindInfo;
        }

        // Token: 0x17000108 RID: 264
        // (get) Token: 0x06000D73 RID: 3443 RVA: 0x00024264 File Offset: 0x00022464
        protected override bool ShouldToggleActive
        {
            get
            {
                return true;
            }
        }

        // Token: 0x06000D74 RID: 3444 RVA: 0x00024268 File Offset: 0x00022468
        protected override GameObject GetGameObject(InjectContext context)
        {
            if (this._gameObjectBindInfo.Name == null)
            {
                this._gameObjectBindInfo.Name = base.ComponentType.Name;
            }
            return base.Container.CreateEmptyGameObject(this._gameObjectBindInfo, context);
        }

        // Token: 0x040003F0 RID: 1008
        private readonly GameObjectCreationParameters _gameObjectBindInfo;
    }
}
