using System;
using System.Collections.Generic;
using ModestTree;
using UnityEngine;

namespace Zenject
{
    // Token: 0x02000275 RID: 629
    [NoReflectionBaking]
    public class PrefabInstantiatorCached : IPrefabInstantiator
    {
        // Token: 0x06000E34 RID: 3636 RVA: 0x00026F9C File Offset: 0x0002519C
        public PrefabInstantiatorCached(IPrefabInstantiator subInstantiator)
        {
            this._subInstantiator = subInstantiator;
        }

        // Token: 0x17000143 RID: 323
        // (get) Token: 0x06000E35 RID: 3637 RVA: 0x00026FAC File Offset: 0x000251AC
        public List<TypeValuePair> ExtraArguments
        {
            get
            {
                return this._subInstantiator.ExtraArguments;
            }
        }

        // Token: 0x17000144 RID: 324
        // (get) Token: 0x06000E36 RID: 3638 RVA: 0x00026FBC File Offset: 0x000251BC
        public Type ArgumentTarget
        {
            get
            {
                return this._subInstantiator.ArgumentTarget;
            }
        }

        // Token: 0x17000145 RID: 325
        // (get) Token: 0x06000E37 RID: 3639 RVA: 0x00026FCC File Offset: 0x000251CC
        public GameObjectCreationParameters GameObjectCreationParameters
        {
            get
            {
                return this._subInstantiator.GameObjectCreationParameters;
            }
        }

        // Token: 0x06000E38 RID: 3640 RVA: 0x00026FDC File Offset: 0x000251DC
        public UnityEngine.Object GetPrefab()
        {
            return this._subInstantiator.GetPrefab();
        }

        // Token: 0x06000E39 RID: 3641 RVA: 0x00026FEC File Offset: 0x000251EC
        public GameObject Instantiate(InjectContext context, List<TypeValuePair> args, out Action injectAction)
        {
            ModestTree.Assert.IsEmpty<TypeValuePair>(args);
            if (this._gameObject != null)
            {
                injectAction = null;
                return this._gameObject;
            }
            this._gameObject = this._subInstantiator.Instantiate(context, new List<TypeValuePair>(), out injectAction);
            return this._gameObject;
        }

        // Token: 0x04000430 RID: 1072
        private readonly IPrefabInstantiator _subInstantiator;

        // Token: 0x04000431 RID: 1073
        private GameObject _gameObject;
    }
}
