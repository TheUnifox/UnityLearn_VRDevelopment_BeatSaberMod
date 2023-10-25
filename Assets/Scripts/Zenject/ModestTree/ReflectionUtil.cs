using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Assertions;

namespace ModestTree
{
    // Token: 0x02000017 RID: 23
    public static class ReflectionUtil
    {
        // Token: 0x060000A1 RID: 161 RVA: 0x000035D4 File Offset: 0x000017D4
        public static Array CreateArray(Type elementType, List<object> instances)
        {
            Array array = Array.CreateInstance(elementType, instances.Count);
            for (int i = 0; i < instances.Count; i++)
            {
                object obj = instances[i];
                if (obj != null)
                {
                    Assert.That(obj.GetType().DerivesFromOrEqual(elementType), string.Concat(new object[]
                    {
                        "Wrong type when creating array, expected something assignable from '",
                        elementType,
                        "', but found '",
                        obj.GetType(),
                        "'"
                    }));
                }
                array.SetValue(obj, i);
            }
            return array;
        }

        // Token: 0x060000A2 RID: 162 RVA: 0x00003654 File Offset: 0x00001854
        public static IList CreateGenericList(Type elementType, List<object> instances)
        {
            IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(new Type[]
            {
                elementType
            }));
            for (int i = 0; i < instances.Count; i++)
            {
                object obj = instances[i];
                if (obj != null)
                {
                    Assert.That(obj.GetType().DerivesFromOrEqual(elementType), string.Concat(new object[]
                    {
                        "Wrong type when creating generic list, expected something assignable from '",
                        elementType,
                        "', but found '",
                        obj.GetType(),
                        "'"
                    }));
                }
                list.Add(obj);
            }
            return list;
        }

        // Token: 0x060000A3 RID: 163 RVA: 0x000036EC File Offset: 0x000018EC
        public static string ToDebugString(this MethodInfo method)
        {
            return "{0}.{1}".Fmt(new object[]
            {
                method.DeclaringType.PrettyName(),
                method.Name
            });
        }

        // Token: 0x060000A4 RID: 164 RVA: 0x00003718 File Offset: 0x00001918
        public static string ToDebugString(this Action action)
        {
            return action.Method.ToDebugString();
        }

        // Token: 0x060000A5 RID: 165 RVA: 0x00003728 File Offset: 0x00001928
        public static string ToDebugString<TParam1>(this Action<TParam1> action)
        {
            return action.Method.ToDebugString();
        }

        // Token: 0x060000A6 RID: 166 RVA: 0x00003738 File Offset: 0x00001938
        public static string ToDebugString<TParam1, TParam2>(this Action<TParam1, TParam2> action)
        {
            return action.Method.ToDebugString();
        }

        // Token: 0x060000A7 RID: 167 RVA: 0x00003748 File Offset: 0x00001948
        public static string ToDebugString<TParam1, TParam2, TParam3>(this Action<TParam1, TParam2, TParam3> action)
        {
            return action.Method.ToDebugString();
        }

        // Token: 0x060000A8 RID: 168 RVA: 0x00003758 File Offset: 0x00001958
        public static string ToDebugString<TParam1, TParam2, TParam3, TParam4>(this Action<TParam1, TParam2, TParam3, TParam4> action)
        {
            return action.Method.ToDebugString();
        }

        // Token: 0x060000A9 RID: 169 RVA: 0x00003768 File Offset: 0x00001968
        public static string ToDebugString<TParam1, TParam2, TParam3, TParam4, TParam5>(this Action<TParam1, TParam2, TParam3, TParam4, TParam5> action)
        {
            return action.Method.ToDebugString();
        }

        // Token: 0x060000AA RID: 170 RVA: 0x00003778 File Offset: 0x00001978
        public static string ToDebugString<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6>(this Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6> action)
        {
            return action.Method.ToDebugString();
        }

        // Token: 0x060000AB RID: 171 RVA: 0x00003788 File Offset: 0x00001988
        public static string ToDebugString<TParam1>(this Func<TParam1> func)
        {
            return func.Method.ToDebugString();
        }

        // Token: 0x060000AC RID: 172 RVA: 0x00003798 File Offset: 0x00001998
        public static string ToDebugString<TParam1, TParam2>(this Func<TParam1, TParam2> func)
        {
            return func.Method.ToDebugString();
        }

        // Token: 0x060000AD RID: 173 RVA: 0x000037A8 File Offset: 0x000019A8
        public static string ToDebugString<TParam1, TParam2, TParam3>(this Func<TParam1, TParam2, TParam3> func)
        {
            return func.Method.ToDebugString();
        }

        // Token: 0x060000AE RID: 174 RVA: 0x000037B8 File Offset: 0x000019B8
        public static string ToDebugString<TParam1, TParam2, TParam3, TParam4>(this Func<TParam1, TParam2, TParam3, TParam4> func)
        {
            return func.Method.ToDebugString();
        }
    }
}
