using System;
using System.Collections.Generic;
using ModestTree;

namespace Zenject
{
    // Token: 0x02000252 RID: 594
    [NoReflectionBaking]
    public class IFactoryProvider<TContract> : IFactoryProviderBase<TContract>
    {
        // Token: 0x06000DA3 RID: 3491 RVA: 0x00024840 File Offset: 0x00022A40
        public IFactoryProvider(DiContainer container, Guid factoryId) : base(container, factoryId)
        {
        }

        // Token: 0x06000DA4 RID: 3492 RVA: 0x0002484C File Offset: 0x00022A4C
        public override void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            ModestTree.Assert.That(args.IsEmpty<TypeValuePair>());
            ModestTree.Assert.IsNotNull(context);
            ModestTree.Assert.That(typeof(TContract).DerivesFromOrEqual(context.MemberType));
            object obj = base.Container.ResolveId(typeof(IFactory<TContract>), base.FactoryId);
            injectAction = null;
            if (base.Container.IsValidating)
            {
                buffer.Add(new ValidationMarker(typeof(TContract)));
                return;
            }
            buffer.Add(((IFactory<TContract>)obj).Create());
        }
    }

    [NoReflectionBaking]
    public class IFactoryProvider<TParam1, TContract> : IFactoryProviderBase<TContract>
    {
        // Token: 0x06000DA5 RID: 3493 RVA: 0x000248E4 File Offset: 0x00022AE4
        public IFactoryProvider(DiContainer container, Guid factoryId) : base(container, factoryId)
        {
        }

        // Token: 0x06000DA6 RID: 3494 RVA: 0x000248F0 File Offset: 0x00022AF0
        public override void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            ModestTree.Assert.IsEqual(args.Count, 1);
            ModestTree.Assert.IsNotNull(context);
            ModestTree.Assert.That(typeof(TContract).DerivesFromOrEqual(context.MemberType));
            ModestTree.Assert.That(args[0].Type.DerivesFromOrEqual<TParam1>());
            object obj = base.Container.ResolveId(typeof(IFactory<TParam1, TContract>), base.FactoryId);
            injectAction = null;
            if (base.Container.IsValidating)
            {
                buffer.Add(new ValidationMarker(typeof(TContract)));
                return;
            }
            buffer.Add(((IFactory<TParam1, TContract>)obj).Create((TParam1)((object)args[0].Value)));
        }
    }

    [NoReflectionBaking]
    public class IFactoryProvider<TParam1, TParam2, TContract> : IFactoryProviderBase<TContract>
    {
        // Token: 0x06000DA7 RID: 3495 RVA: 0x000249BC File Offset: 0x00022BBC
        public IFactoryProvider(DiContainer container, Guid factoryId) : base(container, factoryId)
        {
        }

        // Token: 0x06000DA8 RID: 3496 RVA: 0x000249C8 File Offset: 0x00022BC8
        public override void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            ModestTree.Assert.IsEqual(args.Count, 2);
            ModestTree.Assert.IsNotNull(context);
            ModestTree.Assert.That(typeof(TContract).DerivesFromOrEqual(context.MemberType));
            ModestTree.Assert.That(args[0].Type.DerivesFromOrEqual<TParam1>());
            ModestTree.Assert.That(args[1].Type.DerivesFromOrEqual<TParam2>());
            object obj = base.Container.ResolveId(typeof(IFactory<TParam1, TParam2, TContract>), base.FactoryId);
            injectAction = null;
            if (base.Container.IsValidating)
            {
                buffer.Add(new ValidationMarker(typeof(TContract)));
                return;
            }
            buffer.Add(((IFactory<TParam1, TParam2, TContract>)obj).Create((TParam1)((object)args[0].Value), (TParam2)((object)args[1].Value)));
        }
    }

    [NoReflectionBaking]
    public class IFactoryProvider<TParam1, TParam2, TParam3, TContract> : IFactoryProviderBase<TContract>
    {
        // Token: 0x06000DA9 RID: 3497 RVA: 0x00024AB8 File Offset: 0x00022CB8
        public IFactoryProvider(DiContainer container, Guid factoryId) : base(container, factoryId)
        {
        }

        // Token: 0x06000DAA RID: 3498 RVA: 0x00024AC4 File Offset: 0x00022CC4
        public override void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            ModestTree.Assert.IsEqual(args.Count, 3);
            ModestTree.Assert.IsNotNull(context);
            ModestTree.Assert.That(typeof(TContract).DerivesFromOrEqual(context.MemberType));
            ModestTree.Assert.That(args[0].Type.DerivesFromOrEqual<TParam1>());
            ModestTree.Assert.That(args[1].Type.DerivesFromOrEqual<TParam2>());
            ModestTree.Assert.That(args[2].Type.DerivesFromOrEqual<TParam3>());
            object obj = base.Container.ResolveId(typeof(IFactory<TParam1, TParam2, TParam3, TContract>), base.FactoryId);
            injectAction = null;
            if (base.Container.IsValidating)
            {
                buffer.Add(new ValidationMarker(typeof(TContract)));
                return;
            }
            buffer.Add(((IFactory<TParam1, TParam2, TParam3, TContract>)obj).Create((TParam1)((object)args[0].Value), (TParam2)((object)args[1].Value), (TParam3)((object)args[2].Value)));
        }
    }

    [NoReflectionBaking]
    public class IFactoryProvider<TParam1, TParam2, TParam3, TParam4, TContract> : IFactoryProviderBase<TContract>
    {
        // Token: 0x06000DAB RID: 3499 RVA: 0x00024BDC File Offset: 0x00022DDC
        public IFactoryProvider(DiContainer container, Guid factoryId) : base(container, factoryId)
        {
        }

        // Token: 0x06000DAC RID: 3500 RVA: 0x00024BE8 File Offset: 0x00022DE8
        public override void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            ModestTree.Assert.IsEqual(args.Count, 4);
            ModestTree.Assert.IsNotNull(context);
            ModestTree.Assert.That(typeof(TContract).DerivesFromOrEqual(context.MemberType));
            ModestTree.Assert.That(args[0].Type.DerivesFromOrEqual<TParam1>());
            ModestTree.Assert.That(args[1].Type.DerivesFromOrEqual<TParam2>());
            ModestTree.Assert.That(args[2].Type.DerivesFromOrEqual<TParam3>());
            ModestTree.Assert.That(args[3].Type.DerivesFromOrEqual<TParam4>());
            object obj = base.Container.ResolveId(typeof(IFactory<TParam1, TParam2, TParam3, TParam4, TContract>), base.FactoryId);
            injectAction = null;
            if (base.Container.IsValidating)
            {
                buffer.Add(new ValidationMarker(typeof(TContract)));
                return;
            }
            buffer.Add(((IFactory<TParam1, TParam2, TParam3, TParam4, TContract>)obj).Create((TParam1)((object)args[0].Value), (TParam2)((object)args[1].Value), (TParam3)((object)args[2].Value), (TParam4)((object)args[3].Value)));
        }
    }

    [NoReflectionBaking]
    public class IFactoryProvider<TParam1, TParam2, TParam3, TParam4, TParam5, TContract> : IFactoryProviderBase<TContract>
    {
        // Token: 0x06000DAD RID: 3501 RVA: 0x00024D28 File Offset: 0x00022F28
        public IFactoryProvider(DiContainer container, Guid factoryId) : base(container, factoryId)
        {
        }

        // Token: 0x06000DAE RID: 3502 RVA: 0x00024D34 File Offset: 0x00022F34
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
            object obj = base.Container.ResolveId(typeof(IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TContract>), base.FactoryId);
            injectAction = null;
            if (base.Container.IsValidating)
            {
                buffer.Add(new ValidationMarker(typeof(TContract)));
                return;
            }
            buffer.Add(((IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TContract>)obj).Create((TParam1)((object)args[0].Value), (TParam2)((object)args[1].Value), (TParam3)((object)args[2].Value), (TParam4)((object)args[3].Value), (TParam5)((object)args[4].Value)));
        }
    }

    [NoReflectionBaking]
    public class IFactoryProvider<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract> : IFactoryProviderBase<TContract>
    {
        // Token: 0x06000DAF RID: 3503 RVA: 0x00024E9C File Offset: 0x0002309C
        public IFactoryProvider(DiContainer container, Guid factoryId) : base(container, factoryId)
        {
        }

        // Token: 0x06000DB0 RID: 3504 RVA: 0x00024EA8 File Offset: 0x000230A8
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
            object obj = base.Container.ResolveId(typeof(IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract>), base.FactoryId);
            injectAction = null;
            if (base.Container.IsValidating)
            {
                buffer.Add(new ValidationMarker(typeof(TContract)));
                return;
            }
            buffer.Add(((IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract>)obj).Create((TParam1)((object)args[0].Value), (TParam2)((object)args[1].Value), (TParam3)((object)args[2].Value), (TParam4)((object)args[3].Value), (TParam5)((object)args[4].Value), (TParam6)((object)args[5].Value)));
        }
    }

    [NoReflectionBaking]
    public class IFactoryProvider<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract> : IFactoryProviderBase<TContract>
    {
        // Token: 0x06000DB1 RID: 3505 RVA: 0x00025034 File Offset: 0x00023234
        public IFactoryProvider(DiContainer container, Guid factoryId) : base(container, factoryId)
        {
        }

        // Token: 0x06000DB2 RID: 3506 RVA: 0x00025040 File Offset: 0x00023240
        public override void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            ModestTree.Assert.IsEqual(args.Count, 10);
            ModestTree.Assert.IsNotNull(context);
            ModestTree.Assert.That(typeof(TContract).DerivesFromOrEqual(context.MemberType));
            ModestTree.Assert.That(args[0].Type.DerivesFromOrEqual<TParam1>());
            ModestTree.Assert.That(args[1].Type.DerivesFromOrEqual<TParam2>());
            ModestTree.Assert.That(args[2].Type.DerivesFromOrEqual<TParam3>());
            ModestTree.Assert.That(args[3].Type.DerivesFromOrEqual<TParam4>());
            ModestTree.Assert.That(args[4].Type.DerivesFromOrEqual<TParam5>());
            ModestTree.Assert.That(args[5].Type.DerivesFromOrEqual<TParam6>());
            ModestTree.Assert.That(args[6].Type.DerivesFromOrEqual<TParam7>());
            ModestTree.Assert.That(args[7].Type.DerivesFromOrEqual<TParam8>());
            ModestTree.Assert.That(args[8].Type.DerivesFromOrEqual<TParam9>());
            ModestTree.Assert.That(args[9].Type.DerivesFromOrEqual<TParam10>());
            object obj = base.Container.ResolveId(typeof(IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract>), base.FactoryId);
            injectAction = null;
            if (base.Container.IsValidating)
            {
                buffer.Add(new ValidationMarker(typeof(TContract)));
                return;
            }
            buffer.Add(((IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract>)obj).Create((TParam1)((object)args[0].Value), (TParam2)((object)args[1].Value), (TParam3)((object)args[2].Value), (TParam4)((object)args[3].Value), (TParam5)((object)args[4].Value), (TParam6)((object)args[5].Value), (TParam7)((object)args[6].Value), (TParam8)((object)args[7].Value), (TParam9)((object)args[8].Value), (TParam10)((object)args[9].Value)));
        }
    }
}
