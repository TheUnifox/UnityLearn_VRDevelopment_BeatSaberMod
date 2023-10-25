using System;
using System.Collections.Generic;
using ModestTree;
using UnityEngine;

namespace Zenject
{
    // Token: 0x0200024E RID: 590
    [NoReflectionBaking]
    public class EmptyGameObjectProvider : IProvider
    {
        // Token: 0x06000D89 RID: 3465 RVA: 0x00024574 File Offset: 0x00022774
        public EmptyGameObjectProvider(DiContainer container, GameObjectCreationParameters gameObjectBindInfo)
        {
            this._gameObjectBindInfo = gameObjectBindInfo;
            this._container = container;
        }

        // Token: 0x17000111 RID: 273
        // (get) Token: 0x06000D8A RID: 3466 RVA: 0x0002458C File Offset: 0x0002278C
        public bool IsCached
        {
            get
            {
                return false;
            }
        }

        // Token: 0x17000112 RID: 274
        // (get) Token: 0x06000D8B RID: 3467 RVA: 0x00024590 File Offset: 0x00022790
        public bool TypeVariesBasedOnMemberType
        {
            get
            {
                return false;
            }
        }

        // Token: 0x06000D8C RID: 3468 RVA: 0x00024594 File Offset: 0x00022794
        public Type GetInstanceType(InjectContext context)
        {
            return typeof(GameObject);
        }

        // Token: 0x06000D8D RID: 3469 RVA: 0x000245A0 File Offset: 0x000227A0
        public void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            ModestTree.Assert.IsEmpty<TypeValuePair>(args);
            injectAction = null;
            GameObject item = this._container.CreateEmptyGameObject(this._gameObjectBindInfo, context);
            buffer.Add(item);
        }

        // Token: 0x040003FC RID: 1020
        private readonly DiContainer _container;

        // Token: 0x040003FD RID: 1021
        private readonly GameObjectCreationParameters _gameObjectBindInfo;
    }
}
