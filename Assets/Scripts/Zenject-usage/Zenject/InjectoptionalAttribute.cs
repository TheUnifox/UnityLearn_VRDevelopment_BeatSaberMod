using System;

namespace Zenject
{
    // Token: 0x02000009 RID: 9
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class InjectOptionalAttribute : InjectAttributeBase
    {
        // Token: 0x0600000E RID: 14 RVA: 0x000020DF File Offset: 0x000002DF
        public InjectOptionalAttribute()
        {
            base.Optional = true;
        }
    }
}
