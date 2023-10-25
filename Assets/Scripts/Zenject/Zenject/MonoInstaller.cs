using System;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x02000219 RID: 537
    public class MonoInstaller : MonoInstallerBase
    {
        // Token: 0x06000B8F RID: 2959 RVA: 0x0001EEB8 File Offset: 0x0001D0B8
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(MonoInstaller), new InjectTypeInfo.InjectConstructorInfo(null, new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    public class MonoInstaller<TDerived> : MonoInstaller where TDerived : MonoInstaller<TDerived>
    {
        // Token: 0x06000B90 RID: 2960 RVA: 0x0001EEFC File Offset: 0x0001D0FC
        public static TDerived InstallFromResource(DiContainer container)
        {
            return MonoInstaller<TDerived>.InstallFromResource(MonoInstallerUtil.GetDefaultResourcePath<TDerived>(), container);
        }

        // Token: 0x06000B91 RID: 2961 RVA: 0x0001EF0C File Offset: 0x0001D10C
        public static TDerived InstallFromResource(string resourcePath, DiContainer container)
        {
            return MonoInstaller<TDerived>.InstallFromResource(resourcePath, container, new object[0]);
        }

        // Token: 0x06000B92 RID: 2962 RVA: 0x0001EF1C File Offset: 0x0001D11C
        public static TDerived InstallFromResource(DiContainer container, object[] extraArgs)
        {
            return MonoInstaller<TDerived>.InstallFromResource(MonoInstallerUtil.GetDefaultResourcePath<TDerived>(), container, extraArgs);
        }

        // Token: 0x06000B93 RID: 2963 RVA: 0x0001EF2C File Offset: 0x0001D12C
        public static TDerived InstallFromResource(string resourcePath, DiContainer container, object[] extraArgs)
        {
            TDerived tderived = MonoInstallerUtil.CreateInstaller<TDerived>(resourcePath, container);
            container.Inject(tderived, extraArgs);
            tderived.InstallBindings();
            return tderived;
        }

        // Token: 0x06000B95 RID: 2965 RVA: 0x0001EF64 File Offset: 0x0001D164
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(MonoInstaller<TDerived>), new InjectTypeInfo.InjectConstructorInfo(null, new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    public class MonoInstaller<TParam1, TDerived> : MonoInstallerBase where TDerived : MonoInstaller<TParam1, TDerived>
    {
        // Token: 0x06000B96 RID: 2966 RVA: 0x0001EFA8 File Offset: 0x0001D1A8
        public static TDerived InstallFromResource(DiContainer container, TParam1 p1)
        {
            return MonoInstaller<TParam1, TDerived>.InstallFromResource(MonoInstallerUtil.GetDefaultResourcePath<TDerived>(), container, p1);
        }

        // Token: 0x06000B97 RID: 2967 RVA: 0x0001EFB8 File Offset: 0x0001D1B8
        public static TDerived InstallFromResource(string resourcePath, DiContainer container, TParam1 p1)
        {
            TDerived tderived = MonoInstallerUtil.CreateInstaller<TDerived>(resourcePath, container);
            container.InjectExplicit(tderived, InjectUtil.CreateArgListExplicit<TParam1>(p1));
            tderived.InstallBindings();
            return tderived;
        }

        // Token: 0x06000B99 RID: 2969 RVA: 0x0001EFF4 File Offset: 0x0001D1F4
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(MonoInstaller<TParam1, TDerived>), new InjectTypeInfo.InjectConstructorInfo(null, new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    public class MonoInstaller<TParam1, TParam2, TDerived> : MonoInstallerBase where TDerived : MonoInstaller<TParam1, TParam2, TDerived>
    {
        // Token: 0x06000B9A RID: 2970 RVA: 0x0001F038 File Offset: 0x0001D238
        public static TDerived InstallFromResource(DiContainer container, TParam1 p1, TParam2 p2)
        {
            return MonoInstaller<TParam1, TParam2, TDerived>.InstallFromResource(MonoInstallerUtil.GetDefaultResourcePath<TDerived>(), container, p1, p2);
        }

        // Token: 0x06000B9B RID: 2971 RVA: 0x0001F048 File Offset: 0x0001D248
        public static TDerived InstallFromResource(string resourcePath, DiContainer container, TParam1 p1, TParam2 p2)
        {
            TDerived tderived = MonoInstallerUtil.CreateInstaller<TDerived>(resourcePath, container);
            container.InjectExplicit(tderived, InjectUtil.CreateArgListExplicit<TParam1, TParam2>(p1, p2));
            tderived.InstallBindings();
            return tderived;
        }

        // Token: 0x06000B9D RID: 2973 RVA: 0x0001F084 File Offset: 0x0001D284
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(MonoInstaller<TParam1, TParam2, TDerived>), new InjectTypeInfo.InjectConstructorInfo(null, new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    public class MonoInstaller<TParam1, TParam2, TParam3, TDerived> : MonoInstallerBase where TDerived : MonoInstaller<TParam1, TParam2, TParam3, TDerived>
    {
        // Token: 0x06000B9E RID: 2974 RVA: 0x0001F0C8 File Offset: 0x0001D2C8
        public static TDerived InstallFromResource(DiContainer container, TParam1 p1, TParam2 p2, TParam3 p3)
        {
            return MonoInstaller<TParam1, TParam2, TParam3, TDerived>.InstallFromResource(MonoInstallerUtil.GetDefaultResourcePath<TDerived>(), container, p1, p2, p3);
        }

        // Token: 0x06000B9F RID: 2975 RVA: 0x0001F0D8 File Offset: 0x0001D2D8
        public static TDerived InstallFromResource(string resourcePath, DiContainer container, TParam1 p1, TParam2 p2, TParam3 p3)
        {
            TDerived tderived = MonoInstallerUtil.CreateInstaller<TDerived>(resourcePath, container);
            container.InjectExplicit(tderived, InjectUtil.CreateArgListExplicit<TParam1, TParam2, TParam3>(p1, p2, p3));
            tderived.InstallBindings();
            return tderived;
        }

        // Token: 0x06000BA1 RID: 2977 RVA: 0x0001F118 File Offset: 0x0001D318
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(MonoInstaller<TParam1, TParam2, TParam3, TDerived>), new InjectTypeInfo.InjectConstructorInfo(null, new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    public class MonoInstaller<TParam1, TParam2, TParam3, TParam4, TDerived> : MonoInstallerBase where TDerived : MonoInstaller<TParam1, TParam2, TParam3, TParam4, TDerived>
    {
        // Token: 0x06000BA2 RID: 2978 RVA: 0x0001F15C File Offset: 0x0001D35C
        public static TDerived InstallFromResource(DiContainer container, TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4)
        {
            return MonoInstaller<TParam1, TParam2, TParam3, TParam4, TDerived>.InstallFromResource(MonoInstallerUtil.GetDefaultResourcePath<TDerived>(), container, p1, p2, p3, p4);
        }

        // Token: 0x06000BA3 RID: 2979 RVA: 0x0001F170 File Offset: 0x0001D370
        public static TDerived InstallFromResource(string resourcePath, DiContainer container, TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4)
        {
            TDerived tderived = MonoInstallerUtil.CreateInstaller<TDerived>(resourcePath, container);
            container.InjectExplicit(tderived, InjectUtil.CreateArgListExplicit<TParam1, TParam2, TParam3, TParam4>(p1, p2, p3, p4));
            tderived.InstallBindings();
            return tderived;
        }

        // Token: 0x06000BA5 RID: 2981 RVA: 0x0001F1B0 File Offset: 0x0001D3B0
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(MonoInstaller<TParam1, TParam2, TParam3, TParam4, TDerived>), new InjectTypeInfo.InjectConstructorInfo(null, new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    public class MonoInstaller<TParam1, TParam2, TParam3, TParam4, TParam5, TDerived> : MonoInstallerBase where TDerived : MonoInstaller<TParam1, TParam2, TParam3, TParam4, TParam5, TDerived>
    {
        // Token: 0x06000BA6 RID: 2982 RVA: 0x0001F1F4 File Offset: 0x0001D3F4
        public static TDerived InstallFromResource(DiContainer container, TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5)
        {
            return MonoInstaller<TParam1, TParam2, TParam3, TParam4, TParam5, TDerived>.InstallFromResource(MonoInstallerUtil.GetDefaultResourcePath<TDerived>(), container, p1, p2, p3, p4, p5);
        }

        // Token: 0x06000BA7 RID: 2983 RVA: 0x0001F208 File Offset: 0x0001D408
        public static TDerived InstallFromResource(string resourcePath, DiContainer container, TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5)
        {
            TDerived tderived = MonoInstallerUtil.CreateInstaller<TDerived>(resourcePath, container);
            container.InjectExplicit(tderived, InjectUtil.CreateArgListExplicit<TParam1, TParam2, TParam3, TParam4, TParam5>(p1, p2, p3, p4, p5));
            tderived.InstallBindings();
            return tderived;
        }

        // Token: 0x06000BA9 RID: 2985 RVA: 0x0001F24C File Offset: 0x0001D44C
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(MonoInstaller<TParam1, TParam2, TParam3, TParam4, TParam5, TDerived>), new InjectTypeInfo.InjectConstructorInfo(null, new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }
}
