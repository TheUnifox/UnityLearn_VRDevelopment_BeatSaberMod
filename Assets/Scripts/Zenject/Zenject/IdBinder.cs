using System;

namespace Zenject
{
    // Token: 0x02000149 RID: 329
    [NoReflectionBaking]
    public class IdBinder
    {
        // Token: 0x0600071B RID: 1819 RVA: 0x00012F38 File Offset: 0x00011138
        public IdBinder(BindInfo bindInfo)
        {
            this._bindInfo = bindInfo;
        }

        // Token: 0x0600071C RID: 1820 RVA: 0x00012F48 File Offset: 0x00011148
        public void WithId(object identifier)
        {
            this._bindInfo.Identifier = identifier;
        }

        // Token: 0x0400025E RID: 606
        private BindInfo _bindInfo;
    }
}
