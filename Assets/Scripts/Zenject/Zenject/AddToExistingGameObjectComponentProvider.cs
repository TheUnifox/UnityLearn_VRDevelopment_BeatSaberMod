using System;
using System.Collections.Generic;
using UnityEngine;

namespace Zenject
{
    // Token: 0x02000245 RID: 581
    [NoReflectionBaking]
    public class AddToExistingGameObjectComponentProvider : AddToGameObjectComponentProviderBase
    {
        // Token: 0x06000D5F RID: 3423 RVA: 0x00023F44 File Offset: 0x00022144
        public AddToExistingGameObjectComponentProvider(GameObject gameObject, DiContainer container, Type componentType, IEnumerable<TypeValuePair> extraArguments, object concreteIdentifier, Action<InjectContext, object> instantiateCallback) : base(container, componentType, extraArguments, concreteIdentifier, instantiateCallback)
        {
            this._gameObject = gameObject;
        }

        // Token: 0x17000101 RID: 257
        // (get) Token: 0x06000D60 RID: 3424 RVA: 0x00023F5C File Offset: 0x0002215C
        protected override bool ShouldToggleActive
        {
            get
            {
                return false;
            }
        }

        // Token: 0x06000D61 RID: 3425 RVA: 0x00023F60 File Offset: 0x00022160
        protected override GameObject GetGameObject(InjectContext context)
        {
            return this._gameObject;
        }

        // Token: 0x040003E3 RID: 995
        private readonly GameObject _gameObject;
    }
}
