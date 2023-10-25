using System;

namespace Zenject
{
    // Token: 0x02000008 RID: 8
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class InjectLocalAttribute : InjectAttributeBase
    {
        // Token: 0x0600000D RID: 13 RVA: 0x000020D0 File Offset: 0x000002D0
        public InjectLocalAttribute()
        {
            base.Source = InjectSources.Local;
        }
    }
}
