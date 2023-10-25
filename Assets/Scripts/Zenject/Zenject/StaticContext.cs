using System;

namespace Zenject
{
    // Token: 0x0200020F RID: 527
    public static class StaticContext
    {
        // Token: 0x06000B6F RID: 2927 RVA: 0x0001EB24 File Offset: 0x0001CD24
        public static void Clear()
        {
            StaticContext._container = null;
        }

        // Token: 0x170000CB RID: 203
        // (get) Token: 0x06000B70 RID: 2928 RVA: 0x0001EB2C File Offset: 0x0001CD2C
        public static bool HasContainer
        {
            get
            {
                return StaticContext._container != null;
            }
        }

        // Token: 0x170000CC RID: 204
        // (get) Token: 0x06000B71 RID: 2929 RVA: 0x0001EB38 File Offset: 0x0001CD38
        public static DiContainer Container
        {
            get
            {
                if (StaticContext._container == null)
                {
                    StaticContext._container = new DiContainer();
                }
                return StaticContext._container;
            }
        }

        // Token: 0x04000378 RID: 888
        private static DiContainer _container;
    }
}
