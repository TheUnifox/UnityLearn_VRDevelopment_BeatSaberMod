using System;
using System.Collections.Generic;

namespace Zenject
{
    // Token: 0x0200006E RID: 110
    [NoReflectionBaking]
    public class CopyNonLazyBinder : NonLazyBinder
    {
        // Token: 0x060002C0 RID: 704 RVA: 0x000087C4 File Offset: 0x000069C4
        public CopyNonLazyBinder(BindInfo bindInfo) : base(bindInfo)
        {
        }

        // Token: 0x060002C1 RID: 705 RVA: 0x000087D0 File Offset: 0x000069D0
        internal void AddSecondaryCopyBindInfo(BindInfo bindInfo)
        {
            if (this._secondaryBindInfos == null)
            {
                this._secondaryBindInfos = new List<BindInfo>();
            }
            this._secondaryBindInfos.Add(bindInfo);
        }

        // Token: 0x060002C2 RID: 706 RVA: 0x000087F4 File Offset: 0x000069F4
        public NonLazyBinder CopyIntoAllSubContainers()
        {
            this.SetInheritanceMethod(BindingInheritanceMethods.CopyIntoAll);
            return this;
        }

        // Token: 0x060002C3 RID: 707 RVA: 0x00008800 File Offset: 0x00006A00
        public NonLazyBinder CopyIntoDirectSubContainers()
        {
            this.SetInheritanceMethod(BindingInheritanceMethods.CopyDirectOnly);
            return this;
        }

        // Token: 0x060002C4 RID: 708 RVA: 0x0000880C File Offset: 0x00006A0C
        public NonLazyBinder MoveIntoAllSubContainers()
        {
            this.SetInheritanceMethod(BindingInheritanceMethods.MoveIntoAll);
            return this;
        }

        // Token: 0x060002C5 RID: 709 RVA: 0x00008818 File Offset: 0x00006A18
        public NonLazyBinder MoveIntoDirectSubContainers()
        {
            this.SetInheritanceMethod(BindingInheritanceMethods.MoveDirectOnly);
            return this;
        }

        // Token: 0x060002C6 RID: 710 RVA: 0x00008824 File Offset: 0x00006A24
        private void SetInheritanceMethod(BindingInheritanceMethods method)
        {
            base.BindInfo.BindingInheritanceMethod = method;
            if (this._secondaryBindInfos != null)
            {
                foreach (BindInfo bindInfo in this._secondaryBindInfos)
                {
                    bindInfo.BindingInheritanceMethod = method;
                }
            }
        }

        // Token: 0x040000ED RID: 237
        private List<BindInfo> _secondaryBindInfos;
    }
}
