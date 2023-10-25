using System;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x020001BF RID: 447
    [Serializable]
    public class MemoryPoolSettings
    {
        // Token: 0x0600092F RID: 2351 RVA: 0x00018930 File Offset: 0x00016B30
        public MemoryPoolSettings()
        {
            this.InitialSize = 0;
            this.MaxSize = int.MaxValue;
            this.ExpandMethod = PoolExpandMethods.OneAtATime;
            this.ShowExpandWarning = true;
        }

        // Token: 0x06000930 RID: 2352 RVA: 0x00018958 File Offset: 0x00016B58
        public MemoryPoolSettings(int initialSize, int maxSize, PoolExpandMethods expandMethod, bool showExpandWarning)
        {
            this.InitialSize = initialSize;
            this.MaxSize = maxSize;
            this.ExpandMethod = expandMethod;
            this.ShowExpandWarning = showExpandWarning;
        }

        // Token: 0x06000932 RID: 2354 RVA: 0x0001898C File Offset: 0x00016B8C
        private static object __zenCreate(object[] P_0)
        {
            return new MemoryPoolSettings();
        }

        // Token: 0x06000933 RID: 2355 RVA: 0x000189A4 File Offset: 0x00016BA4
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(MemoryPoolSettings), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(MemoryPoolSettings.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x040002DE RID: 734
        public int InitialSize;

        // Token: 0x040002DF RID: 735
        public int MaxSize;

        // Token: 0x040002E0 RID: 736
        public PoolExpandMethods ExpandMethod;

        // Token: 0x040002E1 RID: 737
        public bool ShowExpandWarning;

        // Token: 0x040002E2 RID: 738
        public static readonly MemoryPoolSettings Default = new MemoryPoolSettings();
    }
}
