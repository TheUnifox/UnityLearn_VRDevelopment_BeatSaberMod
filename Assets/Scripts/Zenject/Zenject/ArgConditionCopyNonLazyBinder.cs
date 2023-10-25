using System;
using System.Collections.Generic;
using ModestTree;

namespace Zenject
{
    // Token: 0x0200004E RID: 78
    [NoReflectionBaking]
    public class ArgConditionCopyNonLazyBinder : InstantiateCallbackConditionCopyNonLazyBinder
    {
        // Token: 0x06000210 RID: 528 RVA: 0x00006E00 File Offset: 0x00005000
        public ArgConditionCopyNonLazyBinder(BindInfo bindInfo) : base(bindInfo)
        {
        }

        // Token: 0x06000211 RID: 529 RVA: 0x00006E0C File Offset: 0x0000500C
        public InstantiateCallbackConditionCopyNonLazyBinder WithArguments<T>(T param)
        {
            base.BindInfo.Arguments.Clear();
            base.BindInfo.Arguments.Add(InjectUtil.CreateTypePair<T>(param));
            return this;
        }

        // Token: 0x06000212 RID: 530 RVA: 0x00006E38 File Offset: 0x00005038
        public InstantiateCallbackConditionCopyNonLazyBinder WithArguments<TParam1, TParam2>(TParam1 param1, TParam2 param2)
        {
            base.BindInfo.Arguments.Clear();
            base.BindInfo.Arguments.Add(InjectUtil.CreateTypePair<TParam1>(param1));
            base.BindInfo.Arguments.Add(InjectUtil.CreateTypePair<TParam2>(param2));
            return this;
        }

        // Token: 0x06000213 RID: 531 RVA: 0x00006E78 File Offset: 0x00005078
        public InstantiateCallbackConditionCopyNonLazyBinder WithArguments<TParam1, TParam2, TParam3>(TParam1 param1, TParam2 param2, TParam3 param3)
        {
            base.BindInfo.Arguments.Clear();
            base.BindInfo.Arguments.Add(InjectUtil.CreateTypePair<TParam1>(param1));
            base.BindInfo.Arguments.Add(InjectUtil.CreateTypePair<TParam2>(param2));
            base.BindInfo.Arguments.Add(InjectUtil.CreateTypePair<TParam3>(param3));
            return this;
        }

        // Token: 0x06000214 RID: 532 RVA: 0x00006ED8 File Offset: 0x000050D8
        public InstantiateCallbackConditionCopyNonLazyBinder WithArguments<TParam1, TParam2, TParam3, TParam4>(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4)
        {
            base.BindInfo.Arguments.Clear();
            base.BindInfo.Arguments.Add(InjectUtil.CreateTypePair<TParam1>(param1));
            base.BindInfo.Arguments.Add(InjectUtil.CreateTypePair<TParam2>(param2));
            base.BindInfo.Arguments.Add(InjectUtil.CreateTypePair<TParam3>(param3));
            base.BindInfo.Arguments.Add(InjectUtil.CreateTypePair<TParam4>(param4));
            return this;
        }

        // Token: 0x06000215 RID: 533 RVA: 0x00006F50 File Offset: 0x00005150
        public InstantiateCallbackConditionCopyNonLazyBinder WithArguments<TParam1, TParam2, TParam3, TParam4, TParam5>(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5)
        {
            base.BindInfo.Arguments.Clear();
            base.BindInfo.Arguments.Add(InjectUtil.CreateTypePair<TParam1>(param1));
            base.BindInfo.Arguments.Add(InjectUtil.CreateTypePair<TParam2>(param2));
            base.BindInfo.Arguments.Add(InjectUtil.CreateTypePair<TParam3>(param3));
            base.BindInfo.Arguments.Add(InjectUtil.CreateTypePair<TParam4>(param4));
            base.BindInfo.Arguments.Add(InjectUtil.CreateTypePair<TParam5>(param5));
            return this;
        }

        // Token: 0x06000216 RID: 534 RVA: 0x00006FE0 File Offset: 0x000051E0
        public InstantiateCallbackConditionCopyNonLazyBinder WithArguments<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6>(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6)
        {
            base.BindInfo.Arguments.Clear();
            base.BindInfo.Arguments.Add(InjectUtil.CreateTypePair<TParam1>(param1));
            base.BindInfo.Arguments.Add(InjectUtil.CreateTypePair<TParam2>(param2));
            base.BindInfo.Arguments.Add(InjectUtil.CreateTypePair<TParam3>(param3));
            base.BindInfo.Arguments.Add(InjectUtil.CreateTypePair<TParam4>(param4));
            base.BindInfo.Arguments.Add(InjectUtil.CreateTypePair<TParam5>(param5));
            base.BindInfo.Arguments.Add(InjectUtil.CreateTypePair<TParam6>(param6));
            return this;
        }

        // Token: 0x06000217 RID: 535 RVA: 0x00007088 File Offset: 0x00005288
        public InstantiateCallbackConditionCopyNonLazyBinder WithArguments(object[] args)
        {
            base.BindInfo.Arguments.Clear();
            foreach (object obj in args)
            {
                ModestTree.Assert.IsNotNull(obj, "Cannot include null values when creating a zenject argument list because zenject has no way of deducing the type from a null value.  If you want to allow null, use the Explicit form.");
                base.BindInfo.Arguments.Add(new TypeValuePair(obj.GetType(), obj));
            }
            return this;
        }

        // Token: 0x06000218 RID: 536 RVA: 0x000070E0 File Offset: 0x000052E0
        public InstantiateCallbackConditionCopyNonLazyBinder WithArgumentsExplicit(IEnumerable<TypeValuePair> extraArgs)
        {
            base.BindInfo.Arguments.Clear();
            foreach (TypeValuePair item in extraArgs)
            {
                base.BindInfo.Arguments.Add(item);
            }
            return this;
        }
    }
}
