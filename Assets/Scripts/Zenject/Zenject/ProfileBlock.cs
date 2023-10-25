using System;
using System.Text.RegularExpressions;

namespace Zenject
{
    // Token: 0x020002EF RID: 751
    [NoReflectionBaking]
    public class ProfileBlock : IDisposable
    {
        // Token: 0x06001031 RID: 4145 RVA: 0x0002DD04 File Offset: 0x0002BF04
        private ProfileBlock(string sampleName, bool rootBlock)
        {
        }

        // Token: 0x06001032 RID: 4146 RVA: 0x0002DD0C File Offset: 0x0002BF0C
        private ProfileBlock(string sampleName) : this(sampleName, false)
        {
        }

        // Token: 0x1700015D RID: 349
        // (get) Token: 0x06001033 RID: 4147 RVA: 0x0002DD18 File Offset: 0x0002BF18
        // (set) Token: 0x06001034 RID: 4148 RVA: 0x0002DD20 File Offset: 0x0002BF20
        public static Regex ProfilePattern { get; set; }

        // Token: 0x06001035 RID: 4149 RVA: 0x0002DD28 File Offset: 0x0002BF28
        public static ProfileBlock Start()
        {
            return null;
        }

        // Token: 0x06001036 RID: 4150 RVA: 0x0002DD2C File Offset: 0x0002BF2C
        public static ProfileBlock Start(string sampleNameFormat, object obj1, object obj2)
        {
            return null;
        }

        // Token: 0x06001037 RID: 4151 RVA: 0x0002DD30 File Offset: 0x0002BF30
        public static ProfileBlock Start(string sampleNameFormat, object obj)
        {
            return null;
        }

        // Token: 0x06001038 RID: 4152 RVA: 0x0002DD34 File Offset: 0x0002BF34
        public static ProfileBlock Start(string sampleName)
        {
            return null;
        }

        // Token: 0x06001039 RID: 4153 RVA: 0x0002DD38 File Offset: 0x0002BF38
        public void Dispose()
        {
        }
    }
}