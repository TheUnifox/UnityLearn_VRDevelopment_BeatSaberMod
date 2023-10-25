using System;
using System.Collections.Generic;

namespace Zenject
{
    // Token: 0x0200004A RID: 74
    [NoReflectionBaking]
    public class FactoryBindInfo
    {
        // Token: 0x060001ED RID: 493 RVA: 0x00006B6C File Offset: 0x00004D6C
        public FactoryBindInfo(Type factoryType)
        {
            this.FactoryType = factoryType;
            this.Arguments = new List<TypeValuePair>();
        }

        // Token: 0x1700002D RID: 45
        // (get) Token: 0x060001EE RID: 494 RVA: 0x00006B88 File Offset: 0x00004D88
        // (set) Token: 0x060001EF RID: 495 RVA: 0x00006B90 File Offset: 0x00004D90
        public Type FactoryType { get; private set; }

        // Token: 0x1700002E RID: 46
        // (get) Token: 0x060001F0 RID: 496 RVA: 0x00006B9C File Offset: 0x00004D9C
        // (set) Token: 0x060001F1 RID: 497 RVA: 0x00006BA4 File Offset: 0x00004DA4
        public Func<DiContainer, IProvider> ProviderFunc { get; set; }

        // Token: 0x1700002F RID: 47
        // (get) Token: 0x060001F2 RID: 498 RVA: 0x00006BB0 File Offset: 0x00004DB0
        // (set) Token: 0x060001F3 RID: 499 RVA: 0x00006BB8 File Offset: 0x00004DB8
        public List<TypeValuePair> Arguments { get; set; }
    }
}
