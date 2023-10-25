using System;

namespace Zenject
{
    // Token: 0x02000160 RID: 352
    public interface IBindingFinalizer
    {
        // Token: 0x17000052 RID: 82
        // (get) Token: 0x0600079A RID: 1946
        BindingInheritanceMethods BindingInheritanceMethod { get; }

        // Token: 0x0600079B RID: 1947
        void FinalizeBinding(DiContainer container);
    }
}
