using System;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x02000222 RID: 546
    public class ScriptableObjectInstaller : ScriptableObjectInstallerBase
    {
        // Token: 0x06000BB5 RID: 2997 RVA: 0x0001F3D8 File Offset: 0x0001D5D8
        private static object __zenCreate(object[] P_0)
        {
            return new ScriptableObjectInstaller();
        }

        // Token: 0x06000BB6 RID: 2998 RVA: 0x0001F3F0 File Offset: 0x0001D5F0
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(ScriptableObjectInstaller), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(ScriptableObjectInstaller.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    public class ScriptableObjectInstaller<TDerived> : ScriptableObjectInstaller where TDerived : ScriptableObjectInstaller<TDerived>
    {
        // Token: 0x06000BB7 RID: 2999 RVA: 0x0001F440 File Offset: 0x0001D640
        public static TDerived InstallFromResource(DiContainer container)
        {
            return ScriptableObjectInstaller<TDerived>.InstallFromResource(ScriptableObjectInstallerUtil.GetDefaultResourcePath<TDerived>(), container);
        }

        // Token: 0x06000BB8 RID: 3000 RVA: 0x0001F450 File Offset: 0x0001D650
        public static TDerived InstallFromResource(string resourcePath, DiContainer container)
        {
            TDerived tderived = ScriptableObjectInstallerUtil.CreateInstaller<TDerived>(resourcePath, container);
            container.Inject(tderived);
            tderived.InstallBindings();
            return tderived;
        }

        // Token: 0x06000BBA RID: 3002 RVA: 0x0001F488 File Offset: 0x0001D688
        private static object __zenCreate(object[] P_0)
        {
            return new ScriptableObjectInstaller<TDerived>();
        }

        // Token: 0x06000BBB RID: 3003 RVA: 0x0001F4A0 File Offset: 0x0001D6A0
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(ScriptableObjectInstaller<TDerived>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(ScriptableObjectInstaller<TDerived>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    public class ScriptableObjectInstaller<TParam1, TDerived> : ScriptableObjectInstallerBase where TDerived : ScriptableObjectInstaller<TParam1, TDerived>
    {
        // Token: 0x06000BBC RID: 3004 RVA: 0x0001F4F0 File Offset: 0x0001D6F0
        public static TDerived InstallFromResource(DiContainer container, TParam1 p1)
        {
            return ScriptableObjectInstaller<TParam1, TDerived>.InstallFromResource(ScriptableObjectInstallerUtil.GetDefaultResourcePath<TDerived>(), container, p1);
        }

        // Token: 0x06000BBD RID: 3005 RVA: 0x0001F500 File Offset: 0x0001D700
        public static TDerived InstallFromResource(string resourcePath, DiContainer container, TParam1 p1)
        {
            TDerived tderived = ScriptableObjectInstallerUtil.CreateInstaller<TDerived>(resourcePath, container);
            container.InjectExplicit(tderived, InjectUtil.CreateArgListExplicit<TParam1>(p1));
            tderived.InstallBindings();
            return tderived;
        }

        // Token: 0x06000BBF RID: 3007 RVA: 0x0001F53C File Offset: 0x0001D73C
        private static object __zenCreate(object[] P_0)
        {
            return new ScriptableObjectInstaller<TParam1, TDerived>();
        }

        // Token: 0x06000BC0 RID: 3008 RVA: 0x0001F554 File Offset: 0x0001D754
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(ScriptableObjectInstaller<TParam1, TDerived>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(ScriptableObjectInstaller<TParam1, TDerived>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    public class ScriptableObjectInstaller<TParam1, TParam2, TDerived> : ScriptableObjectInstallerBase where TDerived : ScriptableObjectInstaller<TParam1, TParam2, TDerived>
    {
        // Token: 0x06000BC1 RID: 3009 RVA: 0x0001F5A4 File Offset: 0x0001D7A4
        public static TDerived InstallFromResource(DiContainer container, TParam1 p1, TParam2 p2)
        {
            return ScriptableObjectInstaller<TParam1, TParam2, TDerived>.InstallFromResource(ScriptableObjectInstallerUtil.GetDefaultResourcePath<TDerived>(), container, p1, p2);
        }

        // Token: 0x06000BC2 RID: 3010 RVA: 0x0001F5B4 File Offset: 0x0001D7B4
        public static TDerived InstallFromResource(string resourcePath, DiContainer container, TParam1 p1, TParam2 p2)
        {
            TDerived tderived = ScriptableObjectInstallerUtil.CreateInstaller<TDerived>(resourcePath, container);
            container.InjectExplicit(tderived, InjectUtil.CreateArgListExplicit<TParam1, TParam2>(p1, p2));
            tderived.InstallBindings();
            return tderived;
        }

        // Token: 0x06000BC4 RID: 3012 RVA: 0x0001F5F0 File Offset: 0x0001D7F0
        private static object __zenCreate(object[] P_0)
        {
            return new ScriptableObjectInstaller<TParam1, TParam2, TDerived>();
        }

        // Token: 0x06000BC5 RID: 3013 RVA: 0x0001F608 File Offset: 0x0001D808
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(ScriptableObjectInstaller<TParam1, TParam2, TDerived>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(ScriptableObjectInstaller<TParam1, TParam2, TDerived>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    public class ScriptableObjectInstaller<TParam1, TParam2, TParam3, TDerived> : ScriptableObjectInstallerBase where TDerived : ScriptableObjectInstaller<TParam1, TParam2, TParam3, TDerived>
    {
        // Token: 0x06000BC6 RID: 3014 RVA: 0x0001F658 File Offset: 0x0001D858
        public static TDerived InstallFromResource(DiContainer container, TParam1 p1, TParam2 p2, TParam3 p3)
        {
            return ScriptableObjectInstaller<TParam1, TParam2, TParam3, TDerived>.InstallFromResource(ScriptableObjectInstallerUtil.GetDefaultResourcePath<TDerived>(), container, p1, p2, p3);
        }

        // Token: 0x06000BC7 RID: 3015 RVA: 0x0001F668 File Offset: 0x0001D868
        public static TDerived InstallFromResource(string resourcePath, DiContainer container, TParam1 p1, TParam2 p2, TParam3 p3)
        {
            TDerived tderived = ScriptableObjectInstallerUtil.CreateInstaller<TDerived>(resourcePath, container);
            container.InjectExplicit(tderived, InjectUtil.CreateArgListExplicit<TParam1, TParam2, TParam3>(p1, p2, p3));
            tderived.InstallBindings();
            return tderived;
        }

        // Token: 0x06000BC9 RID: 3017 RVA: 0x0001F6A8 File Offset: 0x0001D8A8
        private static object __zenCreate(object[] P_0)
        {
            return new ScriptableObjectInstaller<TParam1, TParam2, TParam3, TDerived>();
        }

        // Token: 0x06000BCA RID: 3018 RVA: 0x0001F6C0 File Offset: 0x0001D8C0
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(ScriptableObjectInstaller<TParam1, TParam2, TParam3, TDerived>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(ScriptableObjectInstaller<TParam1, TParam2, TParam3, TDerived>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    public class ScriptableObjectInstaller<TParam1, TParam2, TParam3, TParam4, TDerived> : ScriptableObjectInstallerBase where TDerived : ScriptableObjectInstaller<TParam1, TParam2, TParam3, TParam4, TDerived>
    {
        // Token: 0x06000BCB RID: 3019 RVA: 0x0001F710 File Offset: 0x0001D910
        public static TDerived InstallFromResource(DiContainer container, TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4)
        {
            return ScriptableObjectInstaller<TParam1, TParam2, TParam3, TParam4, TDerived>.InstallFromResource(ScriptableObjectInstallerUtil.GetDefaultResourcePath<TDerived>(), container, p1, p2, p3, p4);
        }

        // Token: 0x06000BCC RID: 3020 RVA: 0x0001F724 File Offset: 0x0001D924
        public static TDerived InstallFromResource(string resourcePath, DiContainer container, TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4)
        {
            TDerived tderived = ScriptableObjectInstallerUtil.CreateInstaller<TDerived>(resourcePath, container);
            container.InjectExplicit(tderived, InjectUtil.CreateArgListExplicit<TParam1, TParam2, TParam3, TParam4>(p1, p2, p3, p4));
            tderived.InstallBindings();
            return tderived;
        }

        // Token: 0x06000BCE RID: 3022 RVA: 0x0001F764 File Offset: 0x0001D964
        private static object __zenCreate(object[] P_0)
        {
            return new ScriptableObjectInstaller<TParam1, TParam2, TParam3, TParam4, TDerived>();
        }

        // Token: 0x06000BCF RID: 3023 RVA: 0x0001F77C File Offset: 0x0001D97C
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(ScriptableObjectInstaller<TParam1, TParam2, TParam3, TParam4, TDerived>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(ScriptableObjectInstaller<TParam1, TParam2, TParam3, TParam4, TDerived>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }
}
