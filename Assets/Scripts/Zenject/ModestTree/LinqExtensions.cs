using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

namespace ModestTree
{
    // Token: 0x02000009 RID: 9
    public static class LinqExtensions
    {
        // Token: 0x0600003A RID: 58 RVA: 0x0000282C File Offset: 0x00000A2C
        public static IEnumerable<T> Yield<T>(this T item)
        {
            yield return item;
            yield break;
        }

        // Token: 0x0600003B RID: 59 RVA: 0x0000283C File Offset: 0x00000A3C
        public static TSource OnlyOrDefault<TSource>(this IEnumerable<TSource> source)
        {
            Assert.IsNotNull(source);
            if (source.Count<TSource>() > 1)
            {
                return default(TSource);
            }
            return source.FirstOrDefault<TSource>();
        }

        // Token: 0x0600003C RID: 60 RVA: 0x00002868 File Offset: 0x00000A68
        public static bool HasAtLeast<T>(this IEnumerable<T> enumerable, int amount)
        {
            return enumerable.Take(amount).Count<T>() == amount;
        }

        // Token: 0x0600003D RID: 61 RVA: 0x0000287C File Offset: 0x00000A7C
        public static bool HasMoreThan<T>(this IEnumerable<T> enumerable, int amount)
        {
            return enumerable.HasAtLeast(amount + 1);
        }

        // Token: 0x0600003E RID: 62 RVA: 0x00002888 File Offset: 0x00000A88
        public static bool HasLessThan<T>(this IEnumerable<T> enumerable, int amount)
        {
            return enumerable.HasAtMost(amount - 1);
        }

        // Token: 0x0600003F RID: 63 RVA: 0x00002894 File Offset: 0x00000A94
        public static bool HasAtMost<T>(this IEnumerable<T> enumerable, int amount)
        {
            return enumerable.Take(amount + 1).Count<T>() <= amount;
        }

        // Token: 0x06000040 RID: 64 RVA: 0x000028AC File Offset: 0x00000AAC
        public static bool IsEmpty<T>(this List<T> list)
        {
            return list.Count == 0;
        }

        // Token: 0x06000041 RID: 65 RVA: 0x000028B8 File Offset: 0x00000AB8
        public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
        {
            return !enumerable.Any<T>();
        }

        // Token: 0x06000042 RID: 66 RVA: 0x000028C4 File Offset: 0x00000AC4
        public static IEnumerable<T> GetDuplicates<T>(this IEnumerable<T> list)
        {
            return from x in list
                   group x by x into x
                   where x.Skip(1).Any<T>()
                   select x.Key;
        }

        // Token: 0x06000043 RID: 67 RVA: 0x00002940 File Offset: 0x00000B40
        public static IEnumerable<T> Except<T>(this IEnumerable<T> list, T item)
        {
            return list.Except(item.Yield<T>());
        }

        // Token: 0x06000044 RID: 68 RVA: 0x00002950 File Offset: 0x00000B50
        public static bool ContainsItem<T>(this IEnumerable<T> list, T value)
        {
            return (from x in list
                    where object.Equals(x, value)
                    select x).Any<T>();
        }
    }
}
