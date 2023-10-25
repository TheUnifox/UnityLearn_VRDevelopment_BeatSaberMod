using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using ModestTree.Util;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x020002C0 RID: 704
    public class DisposableManager : IDisposable
    {
        // Token: 0x06000F1A RID: 3866 RVA: 0x0002A668 File Offset: 0x00028868
        [Inject]
        public DisposableManager([Inject(Optional = true, Source = InjectSources.Local)] List<IDisposable> disposables, [Inject(Optional = true, Source = InjectSources.Local)] List<ModestTree.Util.ValuePair<Type, int>> priorities, [Inject(Optional = true, Source = InjectSources.Local)] List<ILateDisposable> lateDisposables, [Inject(Id = "Late", Optional = true, Source = InjectSources.Local)] List<ModestTree.Util.ValuePair<Type, int>> latePriorities)
        {
            using (List<IDisposable>.Enumerator enumerator = disposables.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    IDisposable disposable = enumerator.Current;
                    int? num = (from x in priorities
                                where disposable.GetType().DerivesFromOrEqual(x.First)
                                select new int?(x.Second)).SingleOrDefault<int?>();
                    int priority = (num != null) ? num.Value : 0;
                    this._disposables.Add(new DisposableManager.DisposableInfo(disposable, priority));
                }
            }
            using (List<ILateDisposable>.Enumerator enumerator2 = lateDisposables.GetEnumerator())
            {
                while (enumerator2.MoveNext())
                {
                    ILateDisposable lateDisposable = enumerator2.Current;
                    int? num2 = (from x in latePriorities
                                 where lateDisposable.GetType().DerivesFromOrEqual(x.First)
                                 select new int?(x.Second)).SingleOrDefault<int?>();
                    int priority2 = (num2 != null) ? num2.Value : 0;
                    this._lateDisposables.Add(new DisposableManager.LateDisposableInfo(lateDisposable, priority2));
                }
            }
        }

        // Token: 0x06000F1B RID: 3867 RVA: 0x0002A7F8 File Offset: 0x000289F8
        public void Add(IDisposable disposable)
        {
            this.Add(disposable, 0);
        }

        // Token: 0x06000F1C RID: 3868 RVA: 0x0002A804 File Offset: 0x00028A04
        public void Add(IDisposable disposable, int priority)
        {
            this._disposables.Add(new DisposableManager.DisposableInfo(disposable, priority));
        }

        // Token: 0x06000F1D RID: 3869 RVA: 0x0002A818 File Offset: 0x00028A18
        public void AddLate(ILateDisposable disposable)
        {
            this.AddLate(disposable, 0);
        }

        // Token: 0x06000F1E RID: 3870 RVA: 0x0002A824 File Offset: 0x00028A24
        public void AddLate(ILateDisposable disposable, int priority)
        {
            this._lateDisposables.Add(new DisposableManager.LateDisposableInfo(disposable, priority));
        }

        // Token: 0x06000F1F RID: 3871 RVA: 0x0002A838 File Offset: 0x00028A38
        public void Remove(IDisposable disposable)
        {
            this._disposables.RemoveWithConfirm((from x in this._disposables
                                                 where x.Disposable == disposable
                                                 select x).Single<DisposableManager.DisposableInfo>());
        }

        // Token: 0x06000F20 RID: 3872 RVA: 0x0002A87C File Offset: 0x00028A7C
        public void LateDispose()
        {
            ModestTree.Assert.That(!this._lateDisposed, "Tried to late dispose DisposableManager twice!");
            this._lateDisposed = true;
            foreach (DisposableManager.LateDisposableInfo lateDisposableInfo in (from x in this._lateDisposables
                                                                                 orderby x.Priority
                                                                                 select x).Reverse<DisposableManager.LateDisposableInfo>().ToList<DisposableManager.LateDisposableInfo>())
            {
                try
                {
                    lateDisposableInfo.LateDisposable.LateDispose();
                }
                catch (Exception innerException)
                {
                    throw ModestTree.Assert.CreateException(innerException, "Error occurred while late disposing ILateDisposable with type '{0}'", new object[]
                    {
                        lateDisposableInfo.LateDisposable.GetType()
                    });
                }
            }
        }

        // Token: 0x06000F21 RID: 3873 RVA: 0x0002A948 File Offset: 0x00028B48
        public void Dispose()
        {
            ModestTree.Assert.That(!this._disposed, "Tried to dispose DisposableManager twice!");
            this._disposed = true;
            foreach (DisposableManager.DisposableInfo disposableInfo in (from x in this._disposables
                                                                         orderby x.Priority
                                                                         select x).Reverse<DisposableManager.DisposableInfo>().ToList<DisposableManager.DisposableInfo>())
            {
                try
                {
                    disposableInfo.Disposable.Dispose();
                }
                catch (Exception innerException)
                {
                    throw ModestTree.Assert.CreateException(innerException, "Error occurred while disposing IDisposable with type '{0}'", new object[]
                    {
                        disposableInfo.Disposable.GetType()
                    });
                }
            }
        }

        // Token: 0x06000F22 RID: 3874 RVA: 0x0002AA14 File Offset: 0x00028C14
        private static object __zenCreate(object[] P_0)
        {
            return new DisposableManager((List<IDisposable>)P_0[0], (List<ModestTree.Util.ValuePair<Type, int>>)P_0[1], (List<ILateDisposable>)P_0[2], (List<ModestTree.Util.ValuePair<Type, int>>)P_0[3]);
        }

        // Token: 0x06000F23 RID: 3875 RVA: 0x0002AA5C File Offset: 0x00028C5C
        [Zenject.Internal.Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(DisposableManager), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(DisposableManager.__zenCreate), new InjectableInfo[]
            {
                new InjectableInfo(true, null, "disposables", typeof(List<IDisposable>), null, InjectSources.Local),
                new InjectableInfo(true, null, "priorities", typeof(List<ModestTree.Util.ValuePair<Type, int>>), null, InjectSources.Local),
                new InjectableInfo(true, null, "lateDisposables", typeof(List<ILateDisposable>), null, InjectSources.Local),
                new InjectableInfo(true, "Late", "latePriorities", typeof(List<ModestTree.Util.ValuePair<Type, int>>), null, InjectSources.Local)
            }), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x040004BA RID: 1210
        private readonly List<DisposableManager.DisposableInfo> _disposables = new List<DisposableManager.DisposableInfo>();

        // Token: 0x040004BB RID: 1211
        private readonly List<DisposableManager.LateDisposableInfo> _lateDisposables = new List<DisposableManager.LateDisposableInfo>();

        // Token: 0x040004BC RID: 1212
        private bool _disposed;

        // Token: 0x040004BD RID: 1213
        private bool _lateDisposed;

        // Token: 0x020002C1 RID: 705
        private struct DisposableInfo
        {
            // Token: 0x06000F24 RID: 3876 RVA: 0x0002AB3C File Offset: 0x00028D3C
            public DisposableInfo(IDisposable disposable, int priority)
            {
                this.Disposable = disposable;
                this.Priority = priority;
            }

            // Token: 0x040004BE RID: 1214
            public IDisposable Disposable;

            // Token: 0x040004BF RID: 1215
            public int Priority;
        }

        // Token: 0x020002C2 RID: 706
        private class LateDisposableInfo
        {
            // Token: 0x06000F25 RID: 3877 RVA: 0x0002AB4C File Offset: 0x00028D4C
            public LateDisposableInfo(ILateDisposable lateDisposable, int priority)
            {
                this.LateDisposable = lateDisposable;
                this.Priority = priority;
            }

            // Token: 0x06000F26 RID: 3878 RVA: 0x0002AB64 File Offset: 0x00028D64
            private static object __zenCreate(object[] P_0)
            {
                return new DisposableManager.LateDisposableInfo((ILateDisposable)P_0[0], (int)P_0[1]);
            }

            // Token: 0x06000F27 RID: 3879 RVA: 0x0002AB94 File Offset: 0x00028D94
            [Zenject.Internal.Preserve]
            private static InjectTypeInfo __zenCreateInjectTypeInfo()
            {
                return new InjectTypeInfo(typeof(DisposableManager.LateDisposableInfo), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(DisposableManager.LateDisposableInfo.__zenCreate), new InjectableInfo[]
                {
                    new InjectableInfo(false, null, "lateDisposable", typeof(ILateDisposable), null, InjectSources.Any),
                    new InjectableInfo(false, null, "priority", typeof(int), null, InjectSources.Any)
                }), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
            }

            // Token: 0x040004C0 RID: 1216
            public ILateDisposable LateDisposable;

            // Token: 0x040004C1 RID: 1217
            public int Priority;
        }
    }
}
