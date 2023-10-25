using System;
using System.Collections.Generic;

namespace Zenject
{
    // Token: 0x02000036 RID: 54
    [NoReflectionBaking]
    public class SignalCopyBinder
    {
        // Token: 0x06000162 RID: 354 RVA: 0x0000553C File Offset: 0x0000373C
        public SignalCopyBinder()
        {
            this._bindInfos = new List<BindInfo>();
        }

        // Token: 0x06000163 RID: 355 RVA: 0x00005550 File Offset: 0x00003750
        public SignalCopyBinder(BindInfo bindInfo)
        {
            this._bindInfos = new List<BindInfo>
            {
                bindInfo
            };
        }

        // Token: 0x06000164 RID: 356 RVA: 0x0000556C File Offset: 0x0000376C
        public void AddCopyBindInfo(BindInfo bindInfo)
        {
            this._bindInfos.Add(bindInfo);
        }

        // Token: 0x06000165 RID: 357 RVA: 0x0000557C File Offset: 0x0000377C
        public void CopyIntoAllSubContainers()
        {
            this.SetInheritanceMethod(BindingInheritanceMethods.CopyIntoAll);
        }

        // Token: 0x06000166 RID: 358 RVA: 0x00005588 File Offset: 0x00003788
        public void CopyIntoDirectSubContainers()
        {
            this.SetInheritanceMethod(BindingInheritanceMethods.CopyDirectOnly);
        }

        // Token: 0x06000167 RID: 359 RVA: 0x00005594 File Offset: 0x00003794
        public void MoveIntoAllSubContainers()
        {
            this.SetInheritanceMethod(BindingInheritanceMethods.MoveIntoAll);
        }

        // Token: 0x06000168 RID: 360 RVA: 0x000055A0 File Offset: 0x000037A0
        public void MoveIntoDirectSubContainers()
        {
            this.SetInheritanceMethod(BindingInheritanceMethods.MoveDirectOnly);
        }

        // Token: 0x06000169 RID: 361 RVA: 0x000055AC File Offset: 0x000037AC
        private void SetInheritanceMethod(BindingInheritanceMethods method)
        {
            for (int i = 0; i < this._bindInfos.Count; i++)
            {
                this._bindInfos[i].BindingInheritanceMethod = method;
            }
        }

        // Token: 0x04000078 RID: 120
        private readonly List<BindInfo> _bindInfos;
    }
}
