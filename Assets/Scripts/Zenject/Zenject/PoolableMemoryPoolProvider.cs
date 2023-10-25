using System;
using System.Buffers;
using System.Collections.Generic;
using ModestTree;

namespace Zenject
{
    // Token: 0x0200026B RID: 619
    [NoReflectionBaking]
    public class PoolableMemoryPoolProvider<TContract, TMemoryPool> : PoolableMemoryPoolProviderBase<TContract>, IValidatable where TContract : IPoolable<IMemoryPool> where TMemoryPool : MemoryPool<IMemoryPool, TContract>
    {
        // Token: 0x06000E10 RID: 3600 RVA: 0x000263F4 File Offset: 0x000245F4
        public PoolableMemoryPoolProvider(DiContainer container, Guid poolId) : base(container, poolId)
        {
        }

        // Token: 0x06000E11 RID: 3601 RVA: 0x00026400 File Offset: 0x00024600
        public void Validate()
        {
            base.Container.ResolveId<TMemoryPool>(base.PoolId);
        }

        // Token: 0x06000E12 RID: 3602 RVA: 0x0002641C File Offset: 0x0002461C
        public override void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            ModestTree.Assert.That(args.IsEmpty<TypeValuePair>());
            ModestTree.Assert.IsNotNull(context);
            ModestTree.Assert.That(typeof(TContract).DerivesFromOrEqual(context.MemberType));
            injectAction = null;
            if (this._pool == null)
            {
                this._pool = base.Container.ResolveId<TMemoryPool>(base.PoolId);
            }
            buffer.Add(this._pool.Spawn(this._pool));
        }

        // Token: 0x0400041D RID: 1053
        private TMemoryPool _pool;
    }

    [NoReflectionBaking]
    public class PoolableMemoryPoolProvider<TParam1, TContract, TMemoryPool> : PoolableMemoryPoolProviderBase<TContract>, IValidatable where TContract : IPoolable<TParam1, IMemoryPool> where TMemoryPool : MemoryPool<TParam1, IMemoryPool, TContract>
    {
        // Token: 0x06000E13 RID: 3603 RVA: 0x000264A8 File Offset: 0x000246A8
        public PoolableMemoryPoolProvider(DiContainer container, Guid poolId) : base(container, poolId)
        {
        }

        // Token: 0x06000E14 RID: 3604 RVA: 0x000264B4 File Offset: 0x000246B4
        public void Validate()
        {
            base.Container.ResolveId<TMemoryPool>(base.PoolId);
        }

        // Token: 0x06000E15 RID: 3605 RVA: 0x000264D0 File Offset: 0x000246D0
        public override void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            ModestTree.Assert.IsEqual(args.Count, 1);
            ModestTree.Assert.IsNotNull(context);
            ModestTree.Assert.That(typeof(TContract).DerivesFromOrEqual(context.MemberType));
            ModestTree.Assert.That(args[0].Type.DerivesFromOrEqual<TParam1>());
            injectAction = null;
            if (this._pool == null)
            {
                this._pool = base.Container.ResolveId<TMemoryPool>(base.PoolId);
            }
            buffer.Add(this._pool.Spawn((TParam1)((object)args[0].Value), this._pool));
        }

        // Token: 0x0400041E RID: 1054
        private TMemoryPool _pool;
    }

    [NoReflectionBaking]
    public class PoolableMemoryPoolProvider<TParam1, TParam2, TContract, TMemoryPool> : PoolableMemoryPoolProviderBase<TContract>, IValidatable where TContract : IPoolable<TParam1, TParam2, IMemoryPool> where TMemoryPool : MemoryPool<TParam1, TParam2, IMemoryPool, TContract>
    {
        // Token: 0x06000E16 RID: 3606 RVA: 0x00026590 File Offset: 0x00024790
        public PoolableMemoryPoolProvider(DiContainer container, Guid poolId) : base(container, poolId)
        {
        }

        // Token: 0x06000E17 RID: 3607 RVA: 0x0002659C File Offset: 0x0002479C
        public void Validate()
        {
            base.Container.ResolveId<TMemoryPool>(base.PoolId);
        }

        // Token: 0x06000E18 RID: 3608 RVA: 0x000265B8 File Offset: 0x000247B8
        public override void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            ModestTree.Assert.IsEqual(args.Count, 2);
            ModestTree.Assert.IsNotNull(context);
            ModestTree.Assert.That(typeof(TContract).DerivesFromOrEqual(context.MemberType));
            ModestTree.Assert.That(args[0].Type.DerivesFromOrEqual<TParam1>());
            ModestTree.Assert.That(args[1].Type.DerivesFromOrEqual<TParam2>());
            injectAction = null;
            if (this._pool == null)
            {
                this._pool = base.Container.ResolveId<TMemoryPool>(base.PoolId);
            }
            buffer.Add(this._pool.Spawn((TParam1)((object)args[0].Value), (TParam2)((object)args[1].Value), this._pool));
        }

        // Token: 0x0400041F RID: 1055
        private TMemoryPool _pool;
    }

    [NoReflectionBaking]
    public class PoolableMemoryPoolProvider<TParam1, TParam2, TParam3, TContract, TMemoryPool> : PoolableMemoryPoolProviderBase<TContract>, IValidatable where TContract : IPoolable<TParam1, TParam2, TParam3, IMemoryPool> where TMemoryPool : MemoryPool<TParam1, TParam2, TParam3, IMemoryPool, TContract>
    {
        // Token: 0x06000E19 RID: 3609 RVA: 0x0002669C File Offset: 0x0002489C
        public PoolableMemoryPoolProvider(DiContainer container, Guid poolId) : base(container, poolId)
        {
        }

        // Token: 0x06000E1A RID: 3610 RVA: 0x000266A8 File Offset: 0x000248A8
        public void Validate()
        {
            base.Container.ResolveId<TMemoryPool>(base.PoolId);
        }

        // Token: 0x06000E1B RID: 3611 RVA: 0x000266C4 File Offset: 0x000248C4
        public override void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            ModestTree.Assert.IsEqual(args.Count, 3);
            ModestTree.Assert.IsNotNull(context);
            ModestTree.Assert.That(typeof(TContract).DerivesFromOrEqual(context.MemberType));
            ModestTree.Assert.That(args[0].Type.DerivesFromOrEqual<TParam1>());
            ModestTree.Assert.That(args[1].Type.DerivesFromOrEqual<TParam2>());
            ModestTree.Assert.That(args[2].Type.DerivesFromOrEqual<TParam3>());
            injectAction = null;
            if (this._pool == null)
            {
                this._pool = base.Container.ResolveId<TMemoryPool>(base.PoolId);
            }
            buffer.Add(this._pool.Spawn((TParam1)((object)args[0].Value), (TParam2)((object)args[1].Value), (TParam3)((object)args[2].Value), this._pool));
        }

        // Token: 0x04000420 RID: 1056
        private TMemoryPool _pool;
    }

    [NoReflectionBaking]
    public class PoolableMemoryPoolProvider<TParam1, TParam2, TParam3, TParam4, TContract, TMemoryPool> : PoolableMemoryPoolProviderBase<TContract>, IValidatable where TContract : IPoolable<TParam1, TParam2, TParam3, TParam4, IMemoryPool> where TMemoryPool : MemoryPool<TParam1, TParam2, TParam3, TParam4, IMemoryPool, TContract>
    {
        // Token: 0x06000E1C RID: 3612 RVA: 0x000267D0 File Offset: 0x000249D0
        public PoolableMemoryPoolProvider(DiContainer container, Guid poolId) : base(container, poolId)
        {
        }

        // Token: 0x06000E1D RID: 3613 RVA: 0x000267DC File Offset: 0x000249DC
        public void Validate()
        {
            base.Container.ResolveId<TMemoryPool>(base.PoolId);
        }

        // Token: 0x06000E1E RID: 3614 RVA: 0x000267F8 File Offset: 0x000249F8
        public override void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            ModestTree.Assert.IsEqual(args.Count, 4);
            ModestTree.Assert.IsNotNull(context);
            ModestTree.Assert.That(typeof(TContract).DerivesFromOrEqual(context.MemberType));
            ModestTree.Assert.That(args[0].Type.DerivesFromOrEqual<TParam1>());
            ModestTree.Assert.That(args[1].Type.DerivesFromOrEqual<TParam2>());
            ModestTree.Assert.That(args[2].Type.DerivesFromOrEqual<TParam3>());
            ModestTree.Assert.That(args[3].Type.DerivesFromOrEqual<TParam4>());
            injectAction = null;
            if (this._pool == null)
            {
                this._pool = base.Container.ResolveId<TMemoryPool>(base.PoolId);
            }
            buffer.Add(this._pool.Spawn((TParam1)((object)args[0].Value), (TParam2)((object)args[1].Value), (TParam3)((object)args[2].Value), (TParam4)((object)args[3].Value), this._pool));
        }

        // Token: 0x04000421 RID: 1057
        private TMemoryPool _pool;
    }

    [NoReflectionBaking]
    public class PoolableMemoryPoolProvider<TParam1, TParam2, TParam3, TParam4, TParam5, TContract, TMemoryPool> : PoolableMemoryPoolProviderBase<TContract>, IValidatable where TContract : IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, IMemoryPool> where TMemoryPool : MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, IMemoryPool, TContract>
    {
        // Token: 0x06000E1F RID: 3615 RVA: 0x0002692C File Offset: 0x00024B2C
        public PoolableMemoryPoolProvider(DiContainer container, Guid poolId) : base(container, poolId)
        {
        }

        // Token: 0x06000E20 RID: 3616 RVA: 0x00026938 File Offset: 0x00024B38
        public void Validate()
        {
            base.Container.ResolveId<TMemoryPool>(base.PoolId);
        }

        // Token: 0x06000E21 RID: 3617 RVA: 0x00026954 File Offset: 0x00024B54
        public override void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            ModestTree.Assert.IsEqual(args.Count, 5);
            ModestTree.Assert.IsNotNull(context);
            ModestTree.Assert.That(typeof(TContract).DerivesFromOrEqual(context.MemberType));
            ModestTree.Assert.That(args[0].Type.DerivesFromOrEqual<TParam1>());
            ModestTree.Assert.That(args[1].Type.DerivesFromOrEqual<TParam2>());
            ModestTree.Assert.That(args[2].Type.DerivesFromOrEqual<TParam3>());
            ModestTree.Assert.That(args[3].Type.DerivesFromOrEqual<TParam4>());
            ModestTree.Assert.That(args[4].Type.DerivesFromOrEqual<TParam5>());
            injectAction = null;
            if (this._pool == null)
            {
                this._pool = base.Container.ResolveId<TMemoryPool>(base.PoolId);
            }
            buffer.Add(this._pool.Spawn((TParam1)((object)args[0].Value), (TParam2)((object)args[1].Value), (TParam3)((object)args[2].Value), (TParam4)((object)args[3].Value), (TParam5)((object)args[4].Value), this._pool));
        }

        // Token: 0x04000422 RID: 1058
        private TMemoryPool _pool;
    }

    [NoReflectionBaking]
    public class PoolableMemoryPoolProvider<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract, TMemoryPool> : PoolableMemoryPoolProviderBase<TContract>, IValidatable where TContract : IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, IMemoryPool> where TMemoryPool : MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, IMemoryPool, TContract>
    {
        // Token: 0x06000E22 RID: 3618 RVA: 0x00026AB0 File Offset: 0x00024CB0
        public PoolableMemoryPoolProvider(DiContainer container, Guid poolId) : base(container, poolId)
        {
        }

        // Token: 0x06000E23 RID: 3619 RVA: 0x00026ABC File Offset: 0x00024CBC
        public void Validate()
        {
            base.Container.ResolveId<TMemoryPool>(base.PoolId);
        }

        // Token: 0x06000E24 RID: 3620 RVA: 0x00026AD8 File Offset: 0x00024CD8
        public override void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            ModestTree.Assert.IsEqual(args.Count, 6);
            ModestTree.Assert.IsNotNull(context);
            ModestTree.Assert.That(typeof(TContract).DerivesFromOrEqual(context.MemberType));
            ModestTree.Assert.That(args[0].Type.DerivesFromOrEqual<TParam1>());
            ModestTree.Assert.That(args[1].Type.DerivesFromOrEqual<TParam2>());
            ModestTree.Assert.That(args[2].Type.DerivesFromOrEqual<TParam3>());
            ModestTree.Assert.That(args[3].Type.DerivesFromOrEqual<TParam4>());
            ModestTree.Assert.That(args[4].Type.DerivesFromOrEqual<TParam5>());
            ModestTree.Assert.That(args[5].Type.DerivesFromOrEqual<TParam6>());
            injectAction = null;
            if (this._pool == null)
            {
                this._pool = base.Container.ResolveId<TMemoryPool>(base.PoolId);
            }
            buffer.Add(this._pool.Spawn((TParam1)((object)args[0].Value), (TParam2)((object)args[1].Value), (TParam3)((object)args[2].Value), (TParam4)((object)args[3].Value), (TParam5)((object)args[4].Value), (TParam6)((object)args[5].Value), this._pool));
        }

        // Token: 0x04000423 RID: 1059
        private TMemoryPool _pool;
    }
}