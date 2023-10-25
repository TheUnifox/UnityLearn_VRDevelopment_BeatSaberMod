using System;
using System.Collections.Generic;

namespace Zenject
{
    // Token: 0x02000285 RID: 645
    [NoReflectionBaking]
    public abstract class SubContainerCreatorByMethodBase : ISubContainerCreator
    {
        // Token: 0x06000E6C RID: 3692 RVA: 0x000277E0 File Offset: 0x000259E0
        public SubContainerCreatorByMethodBase(DiContainer container, SubContainerCreatorBindInfo containerBindInfo)
        {
            this._container = container;
            this._containerBindInfo = containerBindInfo;
        }

        // Token: 0x06000E6D RID: 3693
        public abstract DiContainer CreateSubContainer(List<TypeValuePair> args, InjectContext context);

        // Token: 0x06000E6E RID: 3694 RVA: 0x000277F8 File Offset: 0x000259F8
        protected DiContainer CreateEmptySubContainer()
        {
            DiContainer diContainer = this._container.CreateSubContainer();
            SubContainerCreatorUtil.ApplyBindSettings(this._containerBindInfo, diContainer);
            return diContainer;
        }

        // Token: 0x0400045C RID: 1116
        private readonly DiContainer _container;

        // Token: 0x0400045D RID: 1117
        private readonly SubContainerCreatorBindInfo _containerBindInfo;
    }
}