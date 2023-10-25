using System;

namespace Zenject
{
    // Token: 0x02000007 RID: 7
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class InjectAttribute : InjectAttributeBase
    {
    }
}
