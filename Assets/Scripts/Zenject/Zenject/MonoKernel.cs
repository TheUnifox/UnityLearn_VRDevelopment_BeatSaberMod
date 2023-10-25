using System;
using ModestTree;
using UnityEngine;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x020002D2 RID: 722
    public abstract class MonoKernel : MonoBehaviour
    {
        // Token: 0x17000157 RID: 343
        // (get) Token: 0x06000F73 RID: 3955 RVA: 0x0002B9F4 File Offset: 0x00029BF4
        protected bool IsDestroyed
        {
            get
            {
                return this._isDestroyed;
            }
        }

        // Token: 0x06000F74 RID: 3956 RVA: 0x0002B9FC File Offset: 0x00029BFC
        public virtual void Start()
        {
            this.Initialize();
        }

        // Token: 0x06000F75 RID: 3957 RVA: 0x0002BA04 File Offset: 0x00029C04
        public void Initialize()
        {
            if (!this._hasInitialized)
            {
                this._hasInitialized = true;
                this._initializableManager.Initialize();
            }
        }

        // Token: 0x06000F76 RID: 3958 RVA: 0x0002BA20 File Offset: 0x00029C20
        public virtual void Update()
        {
            if (this._tickableManager != null)
            {
                this._tickableManager.Update();
            }
        }

        // Token: 0x06000F77 RID: 3959 RVA: 0x0002BA38 File Offset: 0x00029C38
        public virtual void FixedUpdate()
        {
            if (this._tickableManager != null)
            {
                this._tickableManager.FixedUpdate();
            }
        }

        // Token: 0x06000F78 RID: 3960 RVA: 0x0002BA50 File Offset: 0x00029C50
        public virtual void LateUpdate()
        {
            if (this._tickableManager != null)
            {
                this._tickableManager.LateUpdate();
            }
        }

        // Token: 0x06000F79 RID: 3961 RVA: 0x0002BA68 File Offset: 0x00029C68
        public virtual void OnDestroy()
        {
            if (this._disposablesManager != null)
            {
                ModestTree.Assert.That(!this._isDestroyed);
                this._isDestroyed = true;
                this._disposablesManager.Dispose();
                this._disposablesManager.LateDispose();
            }
        }

        // Token: 0x06000F7B RID: 3963 RVA: 0x0002BAA8 File Offset: 0x00029CA8
        private static void __zenFieldSetter0(object P_0, object P_1)
        {
            ((MonoKernel)P_0)._tickableManager = (TickableManager)P_1;
        }

        // Token: 0x06000F7C RID: 3964 RVA: 0x0002BAC8 File Offset: 0x00029CC8
        private static void __zenFieldSetter1(object P_0, object P_1)
        {
            ((MonoKernel)P_0)._initializableManager = (InitializableManager)P_1;
        }

        // Token: 0x06000F7D RID: 3965 RVA: 0x0002BAE8 File Offset: 0x00029CE8
        private static void __zenFieldSetter2(object P_0, object P_1)
        {
            ((MonoKernel)P_0)._disposablesManager = (DisposableManager)P_1;
        }

        // Token: 0x06000F7E RID: 3966 RVA: 0x0002BB08 File Offset: 0x00029D08
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(MonoKernel), new InjectTypeInfo.InjectConstructorInfo(null, new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[]
            {
                new InjectTypeInfo.InjectMemberInfo(new ZenMemberSetterMethod(MonoKernel.__zenFieldSetter0), new InjectableInfo(false, null, "_tickableManager", typeof(TickableManager), null, InjectSources.Local)),
                new InjectTypeInfo.InjectMemberInfo(new ZenMemberSetterMethod(MonoKernel.__zenFieldSetter1), new InjectableInfo(false, null, "_initializableManager", typeof(InitializableManager), null, InjectSources.Local)),
                new InjectTypeInfo.InjectMemberInfo(new ZenMemberSetterMethod(MonoKernel.__zenFieldSetter2), new InjectableInfo(false, null, "_disposablesManager", typeof(DisposableManager), null, InjectSources.Local))
            });
        }

        // Token: 0x040004DD RID: 1245
        [InjectLocal]
        private TickableManager _tickableManager;

        // Token: 0x040004DE RID: 1246
        [InjectLocal]
        private InitializableManager _initializableManager;

        // Token: 0x040004DF RID: 1247
        [InjectLocal]
        private DisposableManager _disposablesManager;

        // Token: 0x040004E0 RID: 1248
        private bool _hasInitialized;

        // Token: 0x040004E1 RID: 1249
        private bool _isDestroyed;
    }
}
