using System;
using System.Collections.Generic;
using static UnityEditor.Rendering.FilterWindow;

namespace Zenject.Internal
{
    // Token: 0x02000300 RID: 768
    public interface IDecoratorProvider
    {
        // Token: 0x0600108E RID: 4238
        void GetAllInstances(IProvider provider, InjectContext context, List<object> buffer);
    }
}
