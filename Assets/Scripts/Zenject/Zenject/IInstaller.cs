using System;

namespace Zenject
{
    // Token: 0x02000210 RID: 528
    public interface IInstaller
    {
        // Token: 0x06000B72 RID: 2930
        void InstallBindings();

        // Token: 0x170000CD RID: 205
        // (get) Token: 0x06000B73 RID: 2931
        bool IsEnabled { get; }
    }
}
