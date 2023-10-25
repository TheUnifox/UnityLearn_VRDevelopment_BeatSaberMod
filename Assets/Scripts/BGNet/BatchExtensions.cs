using System;
using System.Collections.Generic;

// Token: 0x02000007 RID: 7
public static class BatchExtensions
{
    // Token: 0x06000021 RID: 33 RVA: 0x000024BF File Offset: 0x000006BF
    public static IEnumerable<List<T>> Batch<T>(this IEnumerable<T> enumerable, int batchSize)
    {
        List<T> list = null;
        foreach (T item in enumerable)
        {
            if (list == null)
            {
                list = new List<T>(batchSize);
            }
            list.Add(item);
            if (list.Count >= batchSize)
            {
                yield return list;
                list = null;
            }
        }
        if (list != null)
        {
            yield return list;
        }
        yield break;
    }
}
