using System;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x02000211 RID: 529
    public abstract class Installer : InstallerBase
    {
        // Token: 0x06000B75 RID: 2933 RVA: 0x0001EB58 File Offset: 0x0001CD58
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(Installer), new InjectTypeInfo.InjectConstructorInfo(null, new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    public abstract class Installer<TDerived> : InstallerBase where TDerived : Installer<TDerived>
    {
        // Token: 0x06000B76 RID: 2934 RVA: 0x0001EB9C File Offset: 0x0001CD9C
        public static void Install(DiContainer container)
        {
            container.Instantiate<TDerived>().InstallBindings();
        }

        // Token: 0x06000B78 RID: 2936 RVA: 0x0001EBB8 File Offset: 0x0001CDB8
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(Installer<TDerived>), new InjectTypeInfo.InjectConstructorInfo(null, new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    public abstract class Installer<TParam1, TDerived> : InstallerBase where TDerived : Installer<TParam1, TDerived>
    {
        // Token: 0x06000B79 RID: 2937 RVA: 0x0001EBFC File Offset: 0x0001CDFC
        public static void Install(DiContainer container, TParam1 p1)
        {
            container.InstantiateExplicit<TDerived>(InjectUtil.CreateArgListExplicit<TParam1>(p1)).InstallBindings();
        }

        // Token: 0x06000B7B RID: 2939 RVA: 0x0001EC1C File Offset: 0x0001CE1C
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(Installer<TParam1, TDerived>), new InjectTypeInfo.InjectConstructorInfo(null, new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    public abstract class Installer<TParam1, TParam2, TDerived> : InstallerBase where TDerived : Installer<TParam1, TParam2, TDerived>
    {
        // Token: 0x06000B7C RID: 2940 RVA: 0x0001EC60 File Offset: 0x0001CE60
        public static void Install(DiContainer container, TParam1 p1, TParam2 p2)
        {
            container.InstantiateExplicit<TDerived>(InjectUtil.CreateArgListExplicit<TParam1, TParam2>(p1, p2)).InstallBindings();
        }

        // Token: 0x06000B7E RID: 2942 RVA: 0x0001EC84 File Offset: 0x0001CE84
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(Installer<TParam1, TParam2, TDerived>), new InjectTypeInfo.InjectConstructorInfo(null, new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    public abstract class Installer<TParam1, TParam2, TParam3, TDerived> : InstallerBase where TDerived : Installer<TParam1, TParam2, TParam3, TDerived>
    {
        // Token: 0x06000B7F RID: 2943 RVA: 0x0001ECC8 File Offset: 0x0001CEC8
        public static void Install(DiContainer container, TParam1 p1, TParam2 p2, TParam3 p3)
        {
            container.InstantiateExplicit<TDerived>(InjectUtil.CreateArgListExplicit<TParam1, TParam2, TParam3>(p1, p2, p3)).InstallBindings();
        }

        // Token: 0x06000B81 RID: 2945 RVA: 0x0001ECEC File Offset: 0x0001CEEC
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(Installer<TParam1, TParam2, TParam3, TDerived>), new InjectTypeInfo.InjectConstructorInfo(null, new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    public abstract class Installer<TParam1, TParam2, TParam3, TParam4, TDerived> : InstallerBase where TDerived : Installer<TParam1, TParam2, TParam3, TParam4, TDerived>
    {
        // Token: 0x06000B82 RID: 2946 RVA: 0x0001ED30 File Offset: 0x0001CF30
        public static void Install(DiContainer container, TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4)
        {
            container.InstantiateExplicit<TDerived>(InjectUtil.CreateArgListExplicit<TParam1, TParam2, TParam3, TParam4>(p1, p2, p3, p4)).InstallBindings();
        }

        // Token: 0x06000B84 RID: 2948 RVA: 0x0001ED54 File Offset: 0x0001CF54
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(Installer<TParam1, TParam2, TParam3, TParam4, TDerived>), new InjectTypeInfo.InjectConstructorInfo(null, new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    public abstract class Installer<TParam1, TParam2, TParam3, TParam4, TParam5, TDerived> : InstallerBase where TDerived : Installer<TParam1, TParam2, TParam3, TParam4, TParam5, TDerived>
    {
        // Token: 0x06000B85 RID: 2949 RVA: 0x0001ED98 File Offset: 0x0001CF98
        public static void Install(DiContainer container, TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5)
        {
            container.InstantiateExplicit<TDerived>(InjectUtil.CreateArgListExplicit<TParam1, TParam2, TParam3, TParam4, TParam5>(p1, p2, p3, p4, p5)).InstallBindings();
        }

        // Token: 0x06000B87 RID: 2951 RVA: 0x0001EDC0 File Offset: 0x0001CFC0
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(Installer<TParam1, TParam2, TParam3, TParam4, TParam5, TDerived>), new InjectTypeInfo.InjectConstructorInfo(null, new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }
}
