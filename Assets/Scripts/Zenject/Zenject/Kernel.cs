using System;
using System.Diagnostics;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x020002D1 RID: 721
    [DebuggerStepThrough]
    public class Kernel : IInitializable, IDisposable, ITickable, ILateTickable, IFixedTickable, ILateDisposable
    {
        // Token: 0x06000F67 RID: 3943 RVA: 0x0002B828 File Offset: 0x00029A28
        public virtual void Initialize()
        {
            this._initializableManager.Initialize();
        }

        // Token: 0x06000F68 RID: 3944 RVA: 0x0002B838 File Offset: 0x00029A38
        public virtual void Dispose()
        {
            this._disposablesManager.Dispose();
        }

        // Token: 0x06000F69 RID: 3945 RVA: 0x0002B848 File Offset: 0x00029A48
        public virtual void LateDispose()
        {
            this._disposablesManager.LateDispose();
        }

        // Token: 0x06000F6A RID: 3946 RVA: 0x0002B858 File Offset: 0x00029A58
        public virtual void Tick()
        {
            this._tickableManager.Update();
        }

        // Token: 0x06000F6B RID: 3947 RVA: 0x0002B868 File Offset: 0x00029A68
        public virtual void LateTick()
        {
            this._tickableManager.LateUpdate();
        }

        // Token: 0x06000F6C RID: 3948 RVA: 0x0002B878 File Offset: 0x00029A78
        public virtual void FixedTick()
        {
            this._tickableManager.FixedUpdate();
        }

        // Token: 0x06000F6E RID: 3950 RVA: 0x0002B890 File Offset: 0x00029A90
        private static object __zenCreate(object[] P_0)
        {
            return new Kernel();
        }

        // Token: 0x06000F6F RID: 3951 RVA: 0x0002B8A8 File Offset: 0x00029AA8
        private static void __zenFieldSetter0(object P_0, object P_1)
        {
            ((Kernel)P_0)._tickableManager = (TickableManager)P_1;
        }

        // Token: 0x06000F70 RID: 3952 RVA: 0x0002B8C8 File Offset: 0x00029AC8
        private static void __zenFieldSetter1(object P_0, object P_1)
        {
            ((Kernel)P_0)._initializableManager = (InitializableManager)P_1;
        }

        // Token: 0x06000F71 RID: 3953 RVA: 0x0002B8E8 File Offset: 0x00029AE8
        private static void __zenFieldSetter2(object P_0, object P_1)
        {
            ((Kernel)P_0)._disposablesManager = (DisposableManager)P_1;
        }

        // Token: 0x06000F72 RID: 3954 RVA: 0x0002B908 File Offset: 0x00029B08
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(Kernel), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(Kernel.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[]
            {
                new InjectTypeInfo.InjectMemberInfo(new ZenMemberSetterMethod(Kernel.__zenFieldSetter0), new InjectableInfo(false, null, "_tickableManager", typeof(TickableManager), null, InjectSources.Local)),
                new InjectTypeInfo.InjectMemberInfo(new ZenMemberSetterMethod(Kernel.__zenFieldSetter1), new InjectableInfo(false, null, "_initializableManager", typeof(InitializableManager), null, InjectSources.Local)),
                new InjectTypeInfo.InjectMemberInfo(new ZenMemberSetterMethod(Kernel.__zenFieldSetter2), new InjectableInfo(false, null, "_disposablesManager", typeof(DisposableManager), null, InjectSources.Local))
            });
        }

        // Token: 0x040004DA RID: 1242
        [InjectLocal]
        private TickableManager _tickableManager;

        // Token: 0x040004DB RID: 1243
        [InjectLocal]
        private InitializableManager _initializableManager;

        // Token: 0x040004DC RID: 1244
        [InjectLocal]
        private DisposableManager _disposablesManager;
    }
}
