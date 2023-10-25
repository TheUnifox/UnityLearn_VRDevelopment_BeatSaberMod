using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ModestTree;

namespace Zenject
{
    // Token: 0x02000202 RID: 514
    [DebuggerStepThrough]
    public static class InjectUtil
    {
        // Token: 0x06000ACE RID: 2766 RVA: 0x0001C6EC File Offset: 0x0001A8EC
        public static List<TypeValuePair> CreateArgList(IEnumerable<object> args)
        {
            ModestTree.Assert.That(!args.ContainsItem(null), "Cannot include null values when creating a zenject argument list because zenject has no way of deducing the type from a null value.  If you want to allow null, use the Explicit form.");
            return (from x in args
                    select new TypeValuePair(x.GetType(), x)).ToList<TypeValuePair>();
        }

        // Token: 0x06000ACF RID: 2767 RVA: 0x0001C72C File Offset: 0x0001A92C
        public static TypeValuePair CreateTypePair<T>(T param)
        {
            return new TypeValuePair((param == null) ? typeof(T) : param.GetType(), param);
        }

        // Token: 0x06000AD0 RID: 2768 RVA: 0x0001C75C File Offset: 0x0001A95C
        public static List<TypeValuePair> CreateArgListExplicit<T>(T param)
        {
            return new List<TypeValuePair>
            {
                InjectUtil.CreateTypePair<T>(param)
            };
        }

        // Token: 0x06000AD1 RID: 2769 RVA: 0x0001C770 File Offset: 0x0001A970
        public static List<TypeValuePair> CreateArgListExplicit<TParam1, TParam2>(TParam1 param1, TParam2 param2)
        {
            return new List<TypeValuePair>
            {
                InjectUtil.CreateTypePair<TParam1>(param1),
                InjectUtil.CreateTypePair<TParam2>(param2)
            };
        }

        // Token: 0x06000AD2 RID: 2770 RVA: 0x0001C790 File Offset: 0x0001A990
        public static List<TypeValuePair> CreateArgListExplicit<TParam1, TParam2, TParam3>(TParam1 param1, TParam2 param2, TParam3 param3)
        {
            return new List<TypeValuePair>
            {
                InjectUtil.CreateTypePair<TParam1>(param1),
                InjectUtil.CreateTypePair<TParam2>(param2),
                InjectUtil.CreateTypePair<TParam3>(param3)
            };
        }

        // Token: 0x06000AD3 RID: 2771 RVA: 0x0001C7BC File Offset: 0x0001A9BC
        public static List<TypeValuePair> CreateArgListExplicit<TParam1, TParam2, TParam3, TParam4>(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4)
        {
            return new List<TypeValuePair>
            {
                InjectUtil.CreateTypePair<TParam1>(param1),
                InjectUtil.CreateTypePair<TParam2>(param2),
                InjectUtil.CreateTypePair<TParam3>(param3),
                InjectUtil.CreateTypePair<TParam4>(param4)
            };
        }

        // Token: 0x06000AD4 RID: 2772 RVA: 0x0001C7F4 File Offset: 0x0001A9F4
        public static List<TypeValuePair> CreateArgListExplicit<TParam1, TParam2, TParam3, TParam4, TParam5>(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5)
        {
            return new List<TypeValuePair>
            {
                InjectUtil.CreateTypePair<TParam1>(param1),
                InjectUtil.CreateTypePair<TParam2>(param2),
                InjectUtil.CreateTypePair<TParam3>(param3),
                InjectUtil.CreateTypePair<TParam4>(param4),
                InjectUtil.CreateTypePair<TParam5>(param5)
            };
        }

        // Token: 0x06000AD5 RID: 2773 RVA: 0x0001C844 File Offset: 0x0001AA44
        public static List<TypeValuePair> CreateArgListExplicit<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6>(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6)
        {
            return new List<TypeValuePair>
            {
                InjectUtil.CreateTypePair<TParam1>(param1),
                InjectUtil.CreateTypePair<TParam2>(param2),
                InjectUtil.CreateTypePair<TParam3>(param3),
                InjectUtil.CreateTypePair<TParam4>(param4),
                InjectUtil.CreateTypePair<TParam5>(param5),
                InjectUtil.CreateTypePair<TParam6>(param6)
            };
        }

        // Token: 0x06000AD6 RID: 2774 RVA: 0x0001C8A0 File Offset: 0x0001AAA0
        public static bool PopValueWithType(List<TypeValuePair> extraArgMap, Type injectedFieldType, out object value)
        {
            for (int i = 0; i < extraArgMap.Count; i++)
            {
                TypeValuePair typeValuePair = extraArgMap[i];
                if (typeValuePair.Type.DerivesFromOrEqual(injectedFieldType))
                {
                    value = typeValuePair.Value;
                    extraArgMap.RemoveAt(i);
                    return true;
                }
            }
            value = injectedFieldType.GetDefaultValue();
            return false;
        }
    }
}
