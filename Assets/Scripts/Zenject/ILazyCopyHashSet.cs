using System;

// Token: 0x02000002 RID: 2
public interface ILazyCopyHashSet<in T>
{
    // Token: 0x06000001 RID: 1
    void Add(T item);

    // Token: 0x06000002 RID: 2
    void Remove(T item);
}
