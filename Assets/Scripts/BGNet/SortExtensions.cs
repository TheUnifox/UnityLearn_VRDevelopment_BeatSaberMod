using System;
using System.Collections.Generic;

// Token: 0x02000080 RID: 128
public static class SortExtensions
{
    // Token: 0x0600054E RID: 1358 RVA: 0x0000EA90 File Offset: 0x0000CC90
    public static void InsertSorted<T>(this List<T> list, T item, Func<T, int> getSortIndex)
    {
        int num = 0;
        int num2 = getSortIndex(item);
        while (num < list.Count && num2 >= getSortIndex(list[num]))
        {
            num++;
        }
        list.Insert(num, item);
    }

    // Token: 0x0600054F RID: 1359 RVA: 0x0000EAD0 File Offset: 0x0000CCD0
    public static void Sort<T>(this List<T> list, Func<T, int> getSortIndex)
    {
        list.Sort((T a, T b) => getSortIndex(a).CompareTo(getSortIndex(b)));
    }
}
