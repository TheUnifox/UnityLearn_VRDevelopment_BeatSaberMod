using System;
using System.Collections.Generic;
using ModestTree;
using UnityEngine;

namespace Zenject
{
    // Token: 0x02000246 RID: 582
    [NoReflectionBaking]
    public class AddToExistingGameObjectComponentProviderGetter : AddToGameObjectComponentProviderBase
    {
        // Token: 0x06000D62 RID: 3426 RVA: 0x00023F68 File Offset: 0x00022168
        public AddToExistingGameObjectComponentProviderGetter(Func<InjectContext, GameObject> gameObjectGetter, DiContainer container, Type componentType, List<TypeValuePair> extraArguments, object concreteIdentifier, Action<InjectContext, object> instantiateCallback) : base(container, componentType, extraArguments, concreteIdentifier, instantiateCallback)
        {
            this._gameObjectGetter = gameObjectGetter;
        }

        // Token: 0x17000102 RID: 258
        // (get) Token: 0x06000D63 RID: 3427 RVA: 0x00023F80 File Offset: 0x00022180
        protected override bool ShouldToggleActive
        {
            get
            {
                return false;
            }
        }

        // Token: 0x06000D64 RID: 3428 RVA: 0x00023F84 File Offset: 0x00022184
        protected override GameObject GetGameObject(InjectContext context)
        {
            GameObject gameObject = this._gameObjectGetter(context);
            ModestTree.Assert.IsNotNull(gameObject, "Provided Func<InjectContext, GameObject> returned null value for game object when using FromComponentOn");
            return gameObject;
        }

        // Token: 0x040003E4 RID: 996
        private readonly Func<InjectContext, GameObject> _gameObjectGetter;
    }
}
