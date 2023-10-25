using System;
using System.Diagnostics;

namespace Zenject
{
    // Token: 0x02000201 RID: 513
    [DebuggerStepThrough]
    public struct TypeValuePair
    {
        // Token: 0x06000ACD RID: 2765 RVA: 0x0001C6DC File Offset: 0x0001A8DC
        public TypeValuePair(Type type, object value)
        {
            this.Type = type;
            this.Value = value;
        }

        // Token: 0x04000330 RID: 816
        public Type Type;

        // Token: 0x04000331 RID: 817
        public object Value;
    }
}
