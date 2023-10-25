using System;
using System.Collections.Generic;
using ModestTree;

namespace Zenject
{
    // Token: 0x020002B5 RID: 693
    [NoReflectionBaking]
    public class SubContainerCreatorCached : ISubContainerCreator
    {
        // Token: 0x06000EF2 RID: 3826 RVA: 0x00029EA4 File Offset: 0x000280A4
        public SubContainerCreatorCached(ISubContainerCreator subCreator)
        {
            this._subCreator = subCreator;
        }

        // Token: 0x06000EF3 RID: 3827 RVA: 0x00029EB4 File Offset: 0x000280B4
        public DiContainer CreateSubContainer(List<TypeValuePair> args, InjectContext context)
        {
            ModestTree.Assert.IsEmpty<TypeValuePair>(args);
            if (this._subContainer == null)
            {
                ModestTree.Assert.That(!this._isLookingUp, "Found unresolvable circular dependency when looking up sub container!  Object graph:\n {0}", context.GetObjectGraphString());
                this._isLookingUp = true;
                this._subContainer = this._subCreator.CreateSubContainer(new List<TypeValuePair>(), context);
                this._isLookingUp = false;
                ModestTree.Assert.IsNotNull(this._subContainer);
            }
            return this._subContainer;
        }

        // Token: 0x040004A5 RID: 1189
        private readonly ISubContainerCreator _subCreator;

        // Token: 0x040004A6 RID: 1190
        private bool _isLookingUp;

        // Token: 0x040004A7 RID: 1191
        private DiContainer _subContainer;
    }
}