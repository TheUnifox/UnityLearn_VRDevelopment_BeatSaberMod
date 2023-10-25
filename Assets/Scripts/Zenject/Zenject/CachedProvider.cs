using System;
using System.Collections.Generic;
using ModestTree;

namespace Zenject
{
    // Token: 0x02000242 RID: 578
    [NoReflectionBaking]
    public class CachedProvider : IProvider
    {
        // Token: 0x06000D4D RID: 3405 RVA: 0x00023BE4 File Offset: 0x00021DE4
        public CachedProvider(IProvider creator)
        {
            this._creator = creator;
        }

        // Token: 0x170000FA RID: 250
        // (get) Token: 0x06000D4E RID: 3406 RVA: 0x00023BF4 File Offset: 0x00021DF4
        public bool IsCached
        {
            get
            {
                return true;
            }
        }

        // Token: 0x170000FB RID: 251
        // (get) Token: 0x06000D4F RID: 3407 RVA: 0x00023BF8 File Offset: 0x00021DF8
        public bool TypeVariesBasedOnMemberType
        {
            get
            {
                throw ModestTree.Assert.CreateException();
            }
        }

        // Token: 0x170000FC RID: 252
        // (get) Token: 0x06000D50 RID: 3408 RVA: 0x00023C00 File Offset: 0x00021E00
        public int NumInstances
        {
            get
            {
                if (this._instances != null)
                {
                    return this._instances.Count;
                }
                return 0;
            }
        }

        // Token: 0x06000D51 RID: 3409 RVA: 0x00023C18 File Offset: 0x00021E18
        public void ClearCache()
        {
            this._instances = null;
        }

        // Token: 0x06000D52 RID: 3410 RVA: 0x00023C24 File Offset: 0x00021E24
        public Type GetInstanceType(InjectContext context)
        {
            return this._creator.GetInstanceType(context);
        }

        // Token: 0x06000D53 RID: 3411 RVA: 0x00023C34 File Offset: 0x00021E34
        public void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            ModestTree.Assert.IsNotNull(context);
            if (this._instances != null)
            {
                injectAction = null;
                buffer.AllocFreeAddRange(this._instances);
                return;
            }
            if (this._isCreatingInstance)
            {
                Type instanceType = this._creator.GetInstanceType(context);
                throw ModestTree.Assert.CreateException("Found circular dependency when creating type '{0}'. Object graph:\n {1}{2}\n", new object[]
                {
                    instanceType,
                    context.GetObjectGraphString(),
                    instanceType
                });
            }
            this._isCreatingInstance = true;
            List<object> list = new List<object>();
            this._creator.GetAllInstancesWithInjectSplit(context, args, out injectAction, list);
            ModestTree.Assert.IsNotNull(list);
            this._instances = list;
            this._isCreatingInstance = false;
            buffer.AllocFreeAddRange(list);
        }

        // Token: 0x040003D7 RID: 983
        private readonly IProvider _creator;

        // Token: 0x040003D8 RID: 984
        private List<object> _instances;

        // Token: 0x040003D9 RID: 985
        private bool _isCreatingInstance;
    }
}
