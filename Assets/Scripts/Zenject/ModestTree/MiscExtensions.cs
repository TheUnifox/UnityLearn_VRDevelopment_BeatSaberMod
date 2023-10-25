using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

namespace ModestTree
{
    // Token: 0x0200000E RID: 14
    public static class MiscExtensions
    {
        // Token: 0x0600005B RID: 91 RVA: 0x00002B00 File Offset: 0x00000D00
        public static string Fmt(this string s, params object[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                object obj = args[i];
                if (obj == null)
                {
                    args[i] = "NULL";
                }
                else if (obj is Type)
                {
                    args[i] = ((Type)obj).PrettyName();
                }
            }
            return string.Format(s, args);
        }

        // Token: 0x0600005C RID: 92 RVA: 0x00002B4C File Offset: 0x00000D4C
        public static int IndexOf<T>(this IList<T> list, T item)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (object.Equals(list[i], item))
                {
                    return i;
                }
            }
            return -1;
        }

        // Token: 0x0600005D RID: 93 RVA: 0x00002B88 File Offset: 0x00000D88
        public static string Join(this IEnumerable<string> values, string separator)
        {
            return string.Join(separator, values.ToArray<string>());
        }

        // Token: 0x0600005E RID: 94 RVA: 0x00002B98 File Offset: 0x00000D98
        public static void AllocFreeAddRange<T>(this IList<T> list, IList<T> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                list.Add(items[i]);
            }
        }

        // Token: 0x0600005F RID: 95 RVA: 0x00002BC4 File Offset: 0x00000DC4
        public static void RemoveWithConfirm<T>(this IList<T> list, T item)
        {
            Assert.That(list.Remove(item));
        }

        // Token: 0x06000060 RID: 96 RVA: 0x00002BD4 File Offset: 0x00000DD4
        public static void RemoveWithConfirm<T>(this LinkedList<T> list, T item)
        {
            Assert.That(list.Remove(item));
        }

        // Token: 0x06000061 RID: 97 RVA: 0x00002BE4 File Offset: 0x00000DE4
        public static void RemoveWithConfirm<TKey, TVal>(this IDictionary<TKey, TVal> dictionary, TKey key)
        {
            Assert.That(dictionary.Remove(key));
        }

        // Token: 0x06000062 RID: 98 RVA: 0x00002BF4 File Offset: 0x00000DF4
        public static void RemoveWithConfirm<T>(this HashSet<T> set, T item)
        {
            Assert.That(set.Remove(item));
        }

        // Token: 0x06000063 RID: 99 RVA: 0x00002C04 File Offset: 0x00000E04
        public static TVal GetValueAndRemove<TKey, TVal>(this IDictionary<TKey, TVal> dictionary, TKey key)
        {
            TVal result = dictionary[key];
            dictionary.RemoveWithConfirm(key);
            return result;
        }
    }
}
