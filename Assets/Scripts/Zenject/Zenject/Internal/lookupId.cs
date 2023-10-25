using System;
using ModestTree;
using static UnityEditor.Rendering.FilterWindow;

namespace Zenject.Internal
{
    // Token: 0x020002FE RID: 766
    [NoReflectionBaking]
    public class LookupId
    {
        // Token: 0x06001088 RID: 4232 RVA: 0x0002E750 File Offset: 0x0002C950
        public LookupId()
        {
        }

        // Token: 0x06001089 RID: 4233 RVA: 0x0002E758 File Offset: 0x0002C958
        public LookupId(IProvider provider, BindingId bindingId)
        {
            ModestTree.Assert.IsNotNull(provider);
            ModestTree.Assert.IsNotNull(bindingId);
            this.Provider = provider;
            this.BindingId = bindingId;
        }

        // Token: 0x0600108A RID: 4234 RVA: 0x0002E780 File Offset: 0x0002C980
        public override int GetHashCode()
        {
            return (17 * 23 + this.Provider.GetHashCode()) * 23 + this.BindingId.GetHashCode();
        }

        // Token: 0x04000539 RID: 1337
        public IProvider Provider;

        // Token: 0x0400053A RID: 1338
        public BindingId BindingId;
    }
}
