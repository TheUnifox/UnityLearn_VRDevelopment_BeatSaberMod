using System;

namespace JetBrains.Annotations
{
    // Token: 0x02000035 RID: 53
    [AttributeUsage(AttributeTargets.Method)]
    internal sealed class MustUseReturnValueAttribute : Attribute
    {
        // Token: 0x06000073 RID: 115 RVA: 0x000023F6 File Offset: 0x000005F6
        public MustUseReturnValueAttribute()
        {
        }

        // Token: 0x06000074 RID: 116 RVA: 0x000023FE File Offset: 0x000005FE
        public MustUseReturnValueAttribute([NotNull] string justification)
        {
            this.Justification = justification;
        }

        // Token: 0x17000012 RID: 18
        // (get) Token: 0x06000075 RID: 117 RVA: 0x0000240D File Offset: 0x0000060D
        // (set) Token: 0x06000076 RID: 118 RVA: 0x00002415 File Offset: 0x00000615
        [CanBeNull]
        public string Justification { get; private set; }
    }
}
