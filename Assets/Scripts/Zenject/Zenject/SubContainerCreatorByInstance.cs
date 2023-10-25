using System;
using System.Collections.Generic;
using ModestTree;

namespace Zenject
{
    // Token: 0x02000283 RID: 643
    [NoReflectionBaking]
    public class SubContainerCreatorByInstance : ISubContainerCreator
    {
        // Token: 0x06000E68 RID: 3688 RVA: 0x00027790 File Offset: 0x00025990
        public SubContainerCreatorByInstance(DiContainer subcontainer)
        {
            this._subcontainer = subcontainer;
        }

        // Token: 0x06000E69 RID: 3689 RVA: 0x000277A0 File Offset: 0x000259A0
        public DiContainer CreateSubContainer(List<TypeValuePair> args, InjectContext context)
        {
            ModestTree.Assert.That(args.IsEmpty<TypeValuePair>());
            return this._subcontainer;
        }

        // Token: 0x0400045A RID: 1114
        private readonly DiContainer _subcontainer;
    }
}
