using System;

namespace Zenject
{
    // Token: 0x0200006C RID: 108
    [NoReflectionBaking]
    public class ConventionSelectTypesBinder
    {
        // Token: 0x060002B1 RID: 689 RVA: 0x00008614 File Offset: 0x00006814
        public ConventionSelectTypesBinder(ConventionBindInfo bindInfo)
        {
            this._bindInfo = bindInfo;
        }

        // Token: 0x060002B2 RID: 690 RVA: 0x00008624 File Offset: 0x00006824
        private ConventionFilterTypesBinder CreateNextBinder()
        {
            return new ConventionFilterTypesBinder(this._bindInfo);
        }

        // Token: 0x060002B3 RID: 691 RVA: 0x00008634 File Offset: 0x00006834
        public ConventionFilterTypesBinder AllTypes()
        {
            return this.CreateNextBinder();
        }

        // Token: 0x060002B4 RID: 692 RVA: 0x0000863C File Offset: 0x0000683C
        public ConventionFilterTypesBinder AllClasses()
        {
            this._bindInfo.AddTypeFilter((Type t) => t.IsClass);
            return this.CreateNextBinder();
        }

        // Token: 0x060002B5 RID: 693 RVA: 0x00008670 File Offset: 0x00006870
        public ConventionFilterTypesBinder AllNonAbstractClasses()
        {
            this._bindInfo.AddTypeFilter((Type t) => t.IsClass && !t.IsAbstract);
            return this.CreateNextBinder();
        }

        // Token: 0x060002B6 RID: 694 RVA: 0x000086A4 File Offset: 0x000068A4
        public ConventionFilterTypesBinder AllAbstractClasses()
        {
            this._bindInfo.AddTypeFilter((Type t) => t.IsClass && t.IsAbstract);
            return this.CreateNextBinder();
        }

        // Token: 0x060002B7 RID: 695 RVA: 0x000086D8 File Offset: 0x000068D8
        public ConventionFilterTypesBinder AllInterfaces()
        {
            this._bindInfo.AddTypeFilter((Type t) => t.IsInterface);
            return this.CreateNextBinder();
        }

        // Token: 0x040000E7 RID: 231
        private readonly ConventionBindInfo _bindInfo;
    }
}
