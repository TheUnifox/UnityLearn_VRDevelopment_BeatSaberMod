using System;
using ModestTree;

namespace Zenject
{
    // Token: 0x0200014C RID: 332
    [NoReflectionBaking]
    public class InstantiateCallbackConditionCopyNonLazyBinder : ConditionCopyNonLazyBinder
    {
        // Token: 0x06000723 RID: 1827 RVA: 0x00012FA8 File Offset: 0x000111A8
        public InstantiateCallbackConditionCopyNonLazyBinder(BindInfo bindInfo) : base(bindInfo)
        {
        }

        // Token: 0x06000724 RID: 1828 RVA: 0x00012FB4 File Offset: 0x000111B4
        public ConditionCopyNonLazyBinder OnInstantiated(Action<InjectContext, object> callback)
        {
            base.BindInfo.InstantiatedCallback = callback;
            return this;
        }

        // Token: 0x06000725 RID: 1829 RVA: 0x00012FC4 File Offset: 0x000111C4
        public ConditionCopyNonLazyBinder OnInstantiated<T>(Action<InjectContext, T> callback)
        {
            base.BindInfo.InstantiatedCallback = delegate (InjectContext ctx, object obj)
            {
                ModestTree.Assert.That(obj == null || obj is T, "Invalid generic argument to OnInstantiated! {0} must be type {1}", obj.GetType(), typeof(T));
                callback(ctx, (T)((object)obj));
            };
            return this;
        }
    }
}
