using System;

namespace Zenject
{
    // Token: 0x02000109 RID: 265
    [NoReflectionBaking]
    public class FactoryToChoiceIdBinder<TContract> : FactoryArgumentsToChoiceBinder<TContract>
    {
        // Token: 0x060005CD RID: 1485 RVA: 0x0000FA6C File Offset: 0x0000DC6C
        public FactoryToChoiceIdBinder(DiContainer container, BindInfo bindInfo, FactoryBindInfo factoryBindInfo) : base(container, bindInfo, factoryBindInfo)
        {
        }

        // Token: 0x060005CE RID: 1486 RVA: 0x0000FA78 File Offset: 0x0000DC78
        public FactoryArgumentsToChoiceBinder<TContract> WithId(object identifier)
        {
            base.BindInfo.Identifier = identifier;
            return this;
        }
    }

    [NoReflectionBaking]
    public class FactoryToChoiceIdBinder<TParam1, TContract> : FactoryArgumentsToChoiceBinder<TParam1, TContract>
    {
        // Token: 0x060005CF RID: 1487 RVA: 0x0000FA88 File Offset: 0x0000DC88
        public FactoryToChoiceIdBinder(DiContainer bindContainer, BindInfo bindInfo, FactoryBindInfo factoryBindInfo) : base(bindContainer, bindInfo, factoryBindInfo)
        {
        }

        // Token: 0x060005D0 RID: 1488 RVA: 0x0000FA94 File Offset: 0x0000DC94
        public FactoryArgumentsToChoiceBinder<TParam1, TContract> WithId(object identifier)
        {
            base.BindInfo.Identifier = identifier;
            return this;
        }
    }

    [NoReflectionBaking]
    public class FactoryToChoiceIdBinder<TParam1, TParam2, TContract> : FactoryArgumentsToChoiceBinder<TParam1, TParam2, TContract>
    {
        // Token: 0x060005D3 RID: 1491 RVA: 0x0000FAC0 File Offset: 0x0000DCC0
        public FactoryToChoiceIdBinder(DiContainer bindContainer, BindInfo bindInfo, FactoryBindInfo factoryBindInfo) : base(bindContainer, bindInfo, factoryBindInfo)
        {
        }

        // Token: 0x060005D4 RID: 1492 RVA: 0x0000FACC File Offset: 0x0000DCCC
        public FactoryArgumentsToChoiceBinder<TParam1, TParam2, TContract> WithId(object identifier)
        {
            base.BindInfo.Identifier = identifier;
            return this;
        }
    }

    [NoReflectionBaking]
    public class FactoryToChoiceIdBinder<TParam1, TParam2, TParam3, TContract> : FactoryArgumentsToChoiceBinder<TParam1, TParam2, TParam3, TContract>
    {
        // Token: 0x060005D5 RID: 1493 RVA: 0x0000FADC File Offset: 0x0000DCDC
        public FactoryToChoiceIdBinder(DiContainer bindContainer, BindInfo bindInfo, FactoryBindInfo factoryBindInfo) : base(bindContainer, bindInfo, factoryBindInfo)
        {
        }

        // Token: 0x060005D6 RID: 1494 RVA: 0x0000FAE8 File Offset: 0x0000DCE8
        public FactoryArgumentsToChoiceBinder<TParam1, TParam2, TParam3, TContract> WithId(object identifier)
        {
            base.BindInfo.Identifier = identifier;
            return this;
        }
    }

    [NoReflectionBaking]
    public class FactoryToChoiceIdBinder<TParam1, TParam2, TParam3, TParam4, TContract> : FactoryArgumentsToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TContract>
    {
        // Token: 0x060005D7 RID: 1495 RVA: 0x0000FAF8 File Offset: 0x0000DCF8
        public FactoryToChoiceIdBinder(DiContainer bindContainer, BindInfo bindInfo, FactoryBindInfo factoryBindInfo) : base(bindContainer, bindInfo, factoryBindInfo)
        {
        }

        // Token: 0x060005D8 RID: 1496 RVA: 0x0000FB04 File Offset: 0x0000DD04
        public FactoryArgumentsToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TContract> WithId(object identifier)
        {
            base.BindInfo.Identifier = identifier;
            return this;
        }
    }

    [NoReflectionBaking]
    public class FactoryToChoiceIdBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TContract> : FactoryArgumentsToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TContract>
    {
        // Token: 0x060005D9 RID: 1497 RVA: 0x0000FB14 File Offset: 0x0000DD14
        public FactoryToChoiceIdBinder(DiContainer bindContainer, BindInfo bindInfo, FactoryBindInfo factoryBindInfo) : base(bindContainer, bindInfo, factoryBindInfo)
        {
        }

        // Token: 0x060005DA RID: 1498 RVA: 0x0000FB20 File Offset: 0x0000DD20
        public FactoryArgumentsToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TContract> WithId(object identifier)
        {
            base.BindInfo.Identifier = identifier;
            return this;
        }
    }

    [NoReflectionBaking]
    public class FactoryToChoiceIdBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract> : FactoryArgumentsToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract>
    {
        // Token: 0x060005DB RID: 1499 RVA: 0x0000FB30 File Offset: 0x0000DD30
        public FactoryToChoiceIdBinder(DiContainer bindContainer, BindInfo bindInfo, FactoryBindInfo factoryBindInfo) : base(bindContainer, bindInfo, factoryBindInfo)
        {
        }

        // Token: 0x060005DC RID: 1500 RVA: 0x0000FB3C File Offset: 0x0000DD3C
        public FactoryArgumentsToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract> WithId(object identifier)
        {
            base.BindInfo.Identifier = identifier;
            return this;
        }
    }

    [NoReflectionBaking]
    public class FactoryToChoiceIdBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract> : FactoryArgumentsToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract>
    {
        // Token: 0x060005D1 RID: 1489 RVA: 0x0000FAA4 File Offset: 0x0000DCA4
        public FactoryToChoiceIdBinder(DiContainer bindContainer, BindInfo bindInfo, FactoryBindInfo factoryBindInfo) : base(bindContainer, bindInfo, factoryBindInfo)
        {
        }

        // Token: 0x060005D2 RID: 1490 RVA: 0x0000FAB0 File Offset: 0x0000DCB0
        public FactoryArgumentsToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract> WithId(object identifier)
        {
            base.BindInfo.Identifier = identifier;
            return this;
        }
    }
}
