using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;

namespace Zenject
{
    // Token: 0x020002F4 RID: 756
    public static class ValidationUtil
    {
        // Token: 0x06001053 RID: 4179 RVA: 0x0002E0A4 File Offset: 0x0002C2A4
        public static List<TypeValuePair> CreateDefaultArgs(params Type[] argTypes)
        {
            return (from x in argTypes
                    select new TypeValuePair(x, x.GetDefaultValue())).ToList<TypeValuePair>();
        }
    }
}
