using System;
using System.Collections.Generic;
using ModestTree;

namespace Zenject
{
    // Token: 0x02000284 RID: 644
    [NoReflectionBaking]
    public class SubContainerCreatorByInstanceGetter : ISubContainerCreator
    {
        // Token: 0x06000E6A RID: 3690 RVA: 0x000277B4 File Offset: 0x000259B4
        public SubContainerCreatorByInstanceGetter(Func<InjectContext, DiContainer> subcontainerGetter)
        {
            this._subcontainerGetter = subcontainerGetter;
        }

        // Token: 0x06000E6B RID: 3691 RVA: 0x000277C4 File Offset: 0x000259C4
        public DiContainer CreateSubContainer(List<TypeValuePair> args, InjectContext context)
        {
            ModestTree.Assert.That(args.IsEmpty<TypeValuePair>());
            return this._subcontainerGetter(context);
        }

        // Token: 0x0400045B RID: 1115
        private readonly Func<InjectContext, DiContainer> _subcontainerGetter;
    }
}