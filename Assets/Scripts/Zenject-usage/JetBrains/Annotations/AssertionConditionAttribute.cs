using System;

namespace JetBrains.Annotations
{
    // Token: 0x02000054 RID: 84
    [AttributeUsage(AttributeTargets.Parameter)]
    internal sealed class AssertionConditionAttribute : Attribute
    {
        // Token: 0x060000B9 RID: 185 RVA: 0x00002699 File Offset: 0x00000899
        public AssertionConditionAttribute(AssertionConditionType conditionType)
        {
            this.ConditionType = conditionType;
        }

        // Token: 0x17000023 RID: 35
        // (get) Token: 0x060000BA RID: 186 RVA: 0x000026A8 File Offset: 0x000008A8
        // (set) Token: 0x060000BB RID: 187 RVA: 0x000026B0 File Offset: 0x000008B0
        public AssertionConditionType ConditionType { get; private set; }
    }
}
