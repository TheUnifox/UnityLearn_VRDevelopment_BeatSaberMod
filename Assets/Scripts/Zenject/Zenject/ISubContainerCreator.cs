using System;
using System.Collections.Generic;

namespace Zenject
{
    // Token: 0x02000280 RID: 640
    public interface ISubContainerCreator
    {
        // Token: 0x06000E5D RID: 3677
        DiContainer CreateSubContainer(List<TypeValuePair> args, InjectContext context);
    }
}