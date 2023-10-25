using System;

namespace Zenject
{
    // Token: 0x020000F0 RID: 240
    [NoReflectionBaking]
    public class FactoryFromBinderUntyped : FactoryFromBinderBase
    {
        // Token: 0x06000555 RID: 1365 RVA: 0x0000E4A0 File Offset: 0x0000C6A0
        public FactoryFromBinderUntyped(DiContainer bindContainer, Type contractType, BindInfo bindInfo, FactoryBindInfo factoryBindInfo) : base(bindContainer, contractType, bindInfo, factoryBindInfo)
        {
        }
    }
}
