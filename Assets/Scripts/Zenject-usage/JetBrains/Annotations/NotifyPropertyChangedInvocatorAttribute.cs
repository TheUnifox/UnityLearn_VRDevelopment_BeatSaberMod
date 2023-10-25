using System;

namespace JetBrains.Annotations
{
    // Token: 0x02000029 RID: 41
    [AttributeUsage(AttributeTargets.Method)]
    internal sealed class NotifyPropertyChangedInvocatorAttribute : Attribute
    {
        // Token: 0x0600004B RID: 75 RVA: 0x00002257 File Offset: 0x00000457
        public NotifyPropertyChangedInvocatorAttribute()
        {
        }

        // Token: 0x0600004C RID: 76 RVA: 0x0000225F File Offset: 0x0000045F
        public NotifyPropertyChangedInvocatorAttribute([NotNull] string parameterName)
        {
            this.ParameterName = parameterName;
        }

        // Token: 0x17000008 RID: 8
        // (get) Token: 0x0600004D RID: 77 RVA: 0x0000226E File Offset: 0x0000046E
        // (set) Token: 0x0600004E RID: 78 RVA: 0x00002276 File Offset: 0x00000476
        [CanBeNull]
        public string ParameterName { get; private set; }
    }
}
