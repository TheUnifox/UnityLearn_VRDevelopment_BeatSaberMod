using System;
using ModestTree;

namespace Zenject
{
    // Token: 0x02000101 RID: 257
    [NoReflectionBaking]
    public class FactoryToChoiceBinder<TContract> : FactoryFromBinder<TContract>
    {
        // Token: 0x060005B4 RID: 1460 RVA: 0x0000F5D8 File Offset: 0x0000D7D8
        public FactoryToChoiceBinder(DiContainer container, BindInfo bindInfo, FactoryBindInfo factoryBindInfo) : base(container, bindInfo, factoryBindInfo)
        {
        }

        // Token: 0x060005B5 RID: 1461 RVA: 0x0000F5E4 File Offset: 0x0000D7E4
        public FactoryFromBinder<TContract> ToSelf()
        {
            ModestTree.Assert.IsEqual(base.BindInfo.ToChoice, ToChoices.Self);
            return this;
        }

        // Token: 0x060005B6 RID: 1462 RVA: 0x0000F604 File Offset: 0x0000D804
        public FactoryFromBinderUntyped To(Type concreteType)
        {
            base.BindInfo.ToChoice = ToChoices.Concrete;
            base.BindInfo.ToTypes.Clear();
            base.BindInfo.ToTypes.Add(concreteType);
            return new FactoryFromBinderUntyped(base.BindContainer, concreteType, base.BindInfo, base.FactoryBindInfo);
        }

        // Token: 0x060005B7 RID: 1463 RVA: 0x0000F658 File Offset: 0x0000D858
        public FactoryFromBinder<TConcrete> To<TConcrete>() where TConcrete : TContract
        {
            base.BindInfo.ToChoice = ToChoices.Concrete;
            base.BindInfo.ToTypes.Clear();
            base.BindInfo.ToTypes.Add(typeof(TConcrete));
            return new FactoryFromBinder<TConcrete>(base.BindContainer, base.BindInfo, base.FactoryBindInfo);
        }
    }

    [NoReflectionBaking]
    public class FactoryToChoiceBinder<TParam1, TContract> : FactoryFromBinder<TParam1, TContract>
    {
        // Token: 0x060005B8 RID: 1464 RVA: 0x0000F6B4 File Offset: 0x0000D8B4
        public FactoryToChoiceBinder(DiContainer bindContainer, BindInfo bindInfo, FactoryBindInfo factoryBindInfo) : base(bindContainer, bindInfo, factoryBindInfo)
        {
        }

        // Token: 0x060005B9 RID: 1465 RVA: 0x0000F6C0 File Offset: 0x0000D8C0
        public FactoryFromBinder<TParam1, TContract> ToSelf()
        {
            ModestTree.Assert.IsEqual(base.BindInfo.ToChoice, ToChoices.Self);
            return this;
        }

        // Token: 0x060005BA RID: 1466 RVA: 0x0000F6E0 File Offset: 0x0000D8E0
        public FactoryFromBinder<TParam1, TConcrete> To<TConcrete>() where TConcrete : TContract
        {
            base.BindInfo.ToChoice = ToChoices.Concrete;
            base.BindInfo.ToTypes.Clear();
            base.BindInfo.ToTypes.Add(typeof(TConcrete));
            return new FactoryFromBinder<TParam1, TConcrete>(base.BindContainer, base.BindInfo, base.FactoryBindInfo);
        }
    }

    [NoReflectionBaking]
    public class FactoryToChoiceBinder<TParam1, TParam2, TContract> : FactoryFromBinder<TParam1, TParam2, TContract>
    {
        // Token: 0x060005BE RID: 1470 RVA: 0x0000F7C4 File Offset: 0x0000D9C4
        public FactoryToChoiceBinder(DiContainer bindContainer, BindInfo bindInfo, FactoryBindInfo factoryBindInfo) : base(bindContainer, bindInfo, factoryBindInfo)
        {
        }

        // Token: 0x060005BF RID: 1471 RVA: 0x0000F7D0 File Offset: 0x0000D9D0
        public FactoryFromBinder<TParam1, TParam2, TContract> ToSelf()
        {
            ModestTree.Assert.IsEqual(base.BindInfo.ToChoice, ToChoices.Self);
            return this;
        }

        // Token: 0x060005C0 RID: 1472 RVA: 0x0000F7F0 File Offset: 0x0000D9F0
        public FactoryFromBinder<TParam1, TParam2, TConcrete> To<TConcrete>() where TConcrete : TContract
        {
            base.BindInfo.ToChoice = ToChoices.Concrete;
            base.BindInfo.ToTypes.Clear();
            base.BindInfo.ToTypes.Add(typeof(TConcrete));
            return new FactoryFromBinder<TParam1, TParam2, TConcrete>(base.BindContainer, base.BindInfo, base.FactoryBindInfo);
        }
    }

    [NoReflectionBaking]
    public class FactoryToChoiceBinder<TParam1, TParam2, TParam3, TContract> : FactoryFromBinder<TParam1, TParam2, TParam3, TContract>
    {
        // Token: 0x060005C1 RID: 1473 RVA: 0x0000F84C File Offset: 0x0000DA4C
        public FactoryToChoiceBinder(DiContainer bindContainer, BindInfo bindInfo, FactoryBindInfo factoryBindInfo) : base(bindContainer, bindInfo, factoryBindInfo)
        {
        }

        // Token: 0x060005C2 RID: 1474 RVA: 0x0000F858 File Offset: 0x0000DA58
        public FactoryFromBinder<TParam1, TParam2, TParam3, TContract> ToSelf()
        {
            ModestTree.Assert.IsEqual(base.BindInfo.ToChoice, ToChoices.Self);
            return this;
        }

        // Token: 0x060005C3 RID: 1475 RVA: 0x0000F878 File Offset: 0x0000DA78
        public FactoryFromBinder<TParam1, TParam2, TParam3, TConcrete> To<TConcrete>() where TConcrete : TContract
        {
            base.BindInfo.ToChoice = ToChoices.Concrete;
            base.BindInfo.ToTypes.Clear();
            base.BindInfo.ToTypes.Add(typeof(TConcrete));
            return new FactoryFromBinder<TParam1, TParam2, TParam3, TConcrete>(base.BindContainer, base.BindInfo, base.FactoryBindInfo);
        }
    }

    [NoReflectionBaking]
    public class FactoryToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TContract> : FactoryFromBinder<TParam1, TParam2, TParam3, TParam4, TContract>
    {
        // Token: 0x060005C4 RID: 1476 RVA: 0x0000F8D4 File Offset: 0x0000DAD4
        public FactoryToChoiceBinder(DiContainer bindContainer, BindInfo bindInfo, FactoryBindInfo factoryBindInfo) : base(bindContainer, bindInfo, factoryBindInfo)
        {
        }

        // Token: 0x060005C5 RID: 1477 RVA: 0x0000F8E0 File Offset: 0x0000DAE0
        public FactoryFromBinder<TParam1, TParam2, TParam3, TParam4, TContract> ToSelf()
        {
            ModestTree.Assert.IsEqual(base.BindInfo.ToChoice, ToChoices.Self);
            return this;
        }

        // Token: 0x060005C6 RID: 1478 RVA: 0x0000F900 File Offset: 0x0000DB00
        public FactoryFromBinder<TParam1, TParam2, TParam3, TParam4, TConcrete> To<TConcrete>() where TConcrete : TContract
        {
            base.BindInfo.ToChoice = ToChoices.Concrete;
            base.BindInfo.ToTypes.Clear();
            base.BindInfo.ToTypes.Add(typeof(TConcrete));
            return new FactoryFromBinder<TParam1, TParam2, TParam3, TParam4, TConcrete>(base.BindContainer, base.BindInfo, base.FactoryBindInfo);
        }
    }

    [NoReflectionBaking]
    public class FactoryToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TContract> : FactoryFromBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TContract>
    {
        // Token: 0x060005C7 RID: 1479 RVA: 0x0000F95C File Offset: 0x0000DB5C
        public FactoryToChoiceBinder(DiContainer bindContainer, BindInfo bindInfo, FactoryBindInfo factoryBindInfo) : base(bindContainer, bindInfo, factoryBindInfo)
        {
        }

        // Token: 0x060005C8 RID: 1480 RVA: 0x0000F968 File Offset: 0x0000DB68
        public FactoryFromBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TContract> ToSelf()
        {
            ModestTree.Assert.IsEqual(base.BindInfo.ToChoice, ToChoices.Self);
            return this;
        }

        // Token: 0x060005C9 RID: 1481 RVA: 0x0000F988 File Offset: 0x0000DB88
        public FactoryFromBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TConcrete> To<TConcrete>() where TConcrete : TContract
        {
            base.BindInfo.ToChoice = ToChoices.Concrete;
            base.BindInfo.ToTypes.Clear();
            base.BindInfo.ToTypes.Add(typeof(TConcrete));
            return new FactoryFromBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TConcrete>(base.BindContainer, base.BindInfo, base.FactoryBindInfo);
        }
    }

    [NoReflectionBaking]
    public class FactoryToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract> : FactoryFromBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract>
    {
        // Token: 0x060005CA RID: 1482 RVA: 0x0000F9E4 File Offset: 0x0000DBE4
        public FactoryToChoiceBinder(DiContainer bindContainer, BindInfo bindInfo, FactoryBindInfo factoryBindInfo) : base(bindContainer, bindInfo, factoryBindInfo)
        {
        }

        // Token: 0x060005CB RID: 1483 RVA: 0x0000F9F0 File Offset: 0x0000DBF0
        public FactoryFromBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract> ToSelf()
        {
            ModestTree.Assert.IsEqual(base.BindInfo.ToChoice, ToChoices.Self);
            return this;
        }

        // Token: 0x060005CC RID: 1484 RVA: 0x0000FA10 File Offset: 0x0000DC10
        public FactoryFromBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TConcrete> To<TConcrete>() where TConcrete : TContract
        {
            base.BindInfo.ToChoice = ToChoices.Concrete;
            base.BindInfo.ToTypes.Clear();
            base.BindInfo.ToTypes.Add(typeof(TConcrete));
            return new FactoryFromBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TConcrete>(base.BindContainer, base.BindInfo, base.FactoryBindInfo);
        }
    }

    [NoReflectionBaking]
    public class FactoryToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract> : FactoryFromBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract>
    {
        // Token: 0x060005BB RID: 1467 RVA: 0x0000F73C File Offset: 0x0000D93C
        public FactoryToChoiceBinder(DiContainer bindContainer, BindInfo bindInfo, FactoryBindInfo factoryBindInfo) : base(bindContainer, bindInfo, factoryBindInfo)
        {
        }

        // Token: 0x060005BC RID: 1468 RVA: 0x0000F748 File Offset: 0x0000D948
        public FactoryFromBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract> ToSelf()
        {
            ModestTree.Assert.IsEqual(base.BindInfo.ToChoice, ToChoices.Self);
            return this;
        }

        // Token: 0x060005BD RID: 1469 RVA: 0x0000F768 File Offset: 0x0000D968
        public FactoryFromBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TConcrete> To<TConcrete>() where TConcrete : TContract
        {
            base.BindInfo.ToChoice = ToChoices.Concrete;
            base.BindInfo.ToTypes.Clear();
            base.BindInfo.ToTypes.Add(typeof(TConcrete));
            return new FactoryFromBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TConcrete>(base.BindContainer, base.BindInfo, base.FactoryBindInfo);
        }
    }
}
