using System;

namespace Zenject
{
    // Token: 0x02000161 RID: 353
    [NoReflectionBaking]
    public class NullBindingFinalizer : IBindingFinalizer
    {
        // Token: 0x17000053 RID: 83
        // (get) Token: 0x0600079C RID: 1948 RVA: 0x00014390 File Offset: 0x00012590
        public BindingInheritanceMethods BindingInheritanceMethod
        {
            get
            {
                return BindingInheritanceMethods.None;
            }
        }

        // Token: 0x0600079D RID: 1949 RVA: 0x00014394 File Offset: 0x00012594
        public void FinalizeBinding(DiContainer container)
        {
        }
    }
}
