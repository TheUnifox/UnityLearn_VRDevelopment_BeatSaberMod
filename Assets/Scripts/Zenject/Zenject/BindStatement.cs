using System;
using System.Collections.Generic;
using ModestTree;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x02000049 RID: 73
    [NoReflectionBaking]
    public class BindStatement : IDisposable
    {
        // Token: 0x060001E3 RID: 483 RVA: 0x00006A7C File Offset: 0x00004C7C
        public BindStatement()
        {
            this._disposables = new List<IDisposable>();
            this.Reset();
        }

        // Token: 0x1700002B RID: 43
        // (get) Token: 0x060001E4 RID: 484 RVA: 0x00006A98 File Offset: 0x00004C98
        public BindingInheritanceMethods BindingInheritanceMethod
        {
            get
            {
                this.AssertHasFinalizer();
                return this._bindingFinalizer.BindingInheritanceMethod;
            }
        }

        // Token: 0x1700002C RID: 44
        // (get) Token: 0x060001E5 RID: 485 RVA: 0x00006AAC File Offset: 0x00004CAC
        public bool HasFinalizer
        {
            get
            {
                return this._bindingFinalizer != null;
            }
        }

        // Token: 0x060001E6 RID: 486 RVA: 0x00006AB8 File Offset: 0x00004CB8
        public void SetFinalizer(IBindingFinalizer bindingFinalizer)
        {
            this._bindingFinalizer = bindingFinalizer;
        }

        // Token: 0x060001E7 RID: 487 RVA: 0x00006AC4 File Offset: 0x00004CC4
        private void AssertHasFinalizer()
        {
            if (this._bindingFinalizer == null)
            {
                throw ModestTree.Assert.CreateException("Unfinished binding!  Some required information was left unspecified.");
            }
        }

        // Token: 0x060001E8 RID: 488 RVA: 0x00006ADC File Offset: 0x00004CDC
        public void AddDisposable(IDisposable disposable)
        {
            this._disposables.Add(disposable);
        }

        // Token: 0x060001E9 RID: 489 RVA: 0x00006AEC File Offset: 0x00004CEC
        public BindInfo SpawnBindInfo()
        {
            BindInfo bindInfo = Zenject.Internal.ZenPools.SpawnBindInfo();
            this.AddDisposable(bindInfo);
            return bindInfo;
        }

        // Token: 0x060001EA RID: 490 RVA: 0x00006B08 File Offset: 0x00004D08
        public void FinalizeBinding(DiContainer container)
        {
            this.AssertHasFinalizer();
            this._bindingFinalizer.FinalizeBinding(container);
        }

        // Token: 0x060001EB RID: 491 RVA: 0x00006B1C File Offset: 0x00004D1C
        public void Reset()
        {
            this._bindingFinalizer = null;
            for (int i = 0; i < this._disposables.Count; i++)
            {
                this._disposables[i].Dispose();
            }
            this._disposables.Clear();
        }

        // Token: 0x060001EC RID: 492 RVA: 0x00006B64 File Offset: 0x00004D64
        public void Dispose()
        {
            Zenject.Internal.ZenPools.DespawnStatement(this);
        }

        // Token: 0x040000B8 RID: 184
        private readonly List<IDisposable> _disposables;

        // Token: 0x040000B9 RID: 185
        private IBindingFinalizer _bindingFinalizer;
    }
}
