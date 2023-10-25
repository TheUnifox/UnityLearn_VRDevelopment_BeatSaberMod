using System;

namespace Zenject
{
    // Token: 0x02000004 RID: 4
    [NoReflectionBaking]
    public class InjectableInfo
    {
        // Token: 0x06000003 RID: 3 RVA: 0x00002050 File Offset: 0x00000250
        public InjectableInfo(bool optional, object identifier, string memberName, Type memberType, object defaultValue, InjectSources sourceType)
        {
            this.Optional = optional;
            this.MemberType = memberType;
            this.MemberName = memberName;
            this.Identifier = identifier;
            this.DefaultValue = defaultValue;
            this.SourceType = sourceType;
        }

        // Token: 0x04000001 RID: 1
        public readonly bool Optional;

        // Token: 0x04000002 RID: 2
        public readonly object Identifier;

        // Token: 0x04000003 RID: 3
        public readonly InjectSources SourceType;

        // Token: 0x04000004 RID: 4
        public readonly string MemberName;

        // Token: 0x04000005 RID: 5
        public readonly Type MemberType;

        // Token: 0x04000006 RID: 6
        public readonly object DefaultValue;
    }
}
