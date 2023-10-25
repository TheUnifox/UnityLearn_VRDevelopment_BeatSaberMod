using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;

namespace Zenject
{
    // Token: 0x02000240 RID: 576
    [NoReflectionBaking]
    public class CachedOpenTypeProvider : IProvider
    {
        // Token: 0x06000D41 RID: 3393 RVA: 0x00023A80 File Offset: 0x00021C80
        public CachedOpenTypeProvider(IProvider creator)
        {
            ModestTree.Assert.That(creator.TypeVariesBasedOnMemberType);
            this._creator = creator;
        }

        // Token: 0x170000F7 RID: 247
        // (get) Token: 0x06000D42 RID: 3394 RVA: 0x00023AA8 File Offset: 0x00021CA8
        public bool IsCached
        {
            get
            {
                return true;
            }
        }

        // Token: 0x170000F8 RID: 248
        // (get) Token: 0x06000D43 RID: 3395 RVA: 0x00023AAC File Offset: 0x00021CAC
        public bool TypeVariesBasedOnMemberType
        {
            get
            {
                throw ModestTree.Assert.CreateException();
            }
        }

        // Token: 0x170000F9 RID: 249
        // (get) Token: 0x06000D44 RID: 3396 RVA: 0x00023AB4 File Offset: 0x00021CB4
        public int NumInstances
        {
            get
            {
                return (from x in this._providerMap.Values
                        select x.NumInstances).Sum();
            }
        }

        // Token: 0x06000D45 RID: 3397 RVA: 0x00023AEC File Offset: 0x00021CEC
        public void ClearCache()
        {
            this._providerMap.Clear();
        }

        // Token: 0x06000D46 RID: 3398 RVA: 0x00023AFC File Offset: 0x00021CFC
        public Type GetInstanceType(InjectContext context)
        {
            return this._creator.GetInstanceType(context);
        }

        // Token: 0x06000D47 RID: 3399 RVA: 0x00023B0C File Offset: 0x00021D0C
        public void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            ModestTree.Assert.IsNotNull(context);
            CachedProvider cachedProvider;
            if (!this._providerMap.TryGetValue(context.MemberType, out cachedProvider))
            {
                cachedProvider = new CachedProvider(this._creator);
                this._providerMap.Add(context.MemberType, cachedProvider);
            }
            cachedProvider.GetAllInstancesWithInjectSplit(context, args, out injectAction, buffer);
        }

        // Token: 0x040003D3 RID: 979
        private readonly IProvider _creator;

        // Token: 0x040003D4 RID: 980
        private readonly Dictionary<Type, CachedProvider> _providerMap = new Dictionary<Type, CachedProvider>();
    }
}
