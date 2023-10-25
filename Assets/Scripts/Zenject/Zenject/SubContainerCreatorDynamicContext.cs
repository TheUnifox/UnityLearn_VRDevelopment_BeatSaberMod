using System;
using System.Collections.Generic;
using UnityEngine;

namespace Zenject
{
    // Token: 0x020002B6 RID: 694
    [NoReflectionBaking]
    public abstract class SubContainerCreatorDynamicContext : ISubContainerCreator
    {
        // Token: 0x06000EF4 RID: 3828 RVA: 0x00029F20 File Offset: 0x00028120
        public SubContainerCreatorDynamicContext(DiContainer container)
        {
            this._container = container;
        }

        // Token: 0x17000152 RID: 338
        // (get) Token: 0x06000EF5 RID: 3829 RVA: 0x00029F30 File Offset: 0x00028130
        protected DiContainer Container
        {
            get
            {
                return this._container;
            }
        }

        // Token: 0x06000EF6 RID: 3830 RVA: 0x00029F38 File Offset: 0x00028138
        public DiContainer CreateSubContainer(List<TypeValuePair> args, InjectContext parentContext)
        {
            bool flag;
            GameObject gameObject = this.CreateGameObject(out flag);
            GameObjectContext gameObjectContext = gameObject.AddComponent<GameObjectContext>();
            this.AddInstallers(args, gameObjectContext);
            this._container.Inject(gameObjectContext);
            if (flag && !this._container.IsValidating)
            {
                gameObject.SetActive(true);
            }
            return gameObjectContext.Container;
        }

        // Token: 0x06000EF7 RID: 3831
        protected abstract void AddInstallers(List<TypeValuePair> args, GameObjectContext context);

        // Token: 0x06000EF8 RID: 3832
        protected abstract GameObject CreateGameObject(out bool shouldMakeActive);

        // Token: 0x040004A8 RID: 1192
        private readonly DiContainer _container;
    }
}
