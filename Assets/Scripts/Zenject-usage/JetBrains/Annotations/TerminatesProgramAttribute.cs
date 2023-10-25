using System;

namespace JetBrains.Annotations
{
    // Token: 0x02000056 RID: 86
    [AttributeUsage(AttributeTargets.Method)]
    [Obsolete("Use [ContractAnnotation('=> halt')] instead")]
    internal sealed class TerminatesProgramAttribute : Attribute
    {
    }
}
