using System;
using System.Collections.Generic;
using System.Reflection;

// Token: 0x02000004 RID: 4
public static class HashSetExtensions
{
    // Token: 0x06000008 RID: 8 RVA: 0x0000212C File Offset: 0x0000032C
    public static void SetCapacity<T>(this HashSet<T> hs, int capacity)
    {
        global::HashSetExtensions.HashSetDelegateHolder<T>.InitializeMethod.Invoke(hs, new object[]
        {
            capacity
        });
    }

    // Token: 0x06000009 RID: 9 RVA: 0x0000214C File Offset: 0x0000034C
    public static HashSet<T> GetHashSet<T>(int capacity)
    {
        HashSet<T> hashSet = new HashSet<T>();
        hashSet.SetCapacity(capacity);
        return hashSet;
    }

    // Token: 0x02000005 RID: 5
    private static class HashSetDelegateHolder<T>
    {
        // Token: 0x17000002 RID: 2
        // (get) Token: 0x0600000A RID: 10 RVA: 0x0000215C File Offset: 0x0000035C
        public static MethodInfo InitializeMethod { get; } = typeof(HashSet<T>).GetMethod("Initialize", BindingFlags.Instance | BindingFlags.NonPublic);

        // Token: 0x04000004 RID: 4
        private const BindingFlags Flags = BindingFlags.Instance | BindingFlags.NonPublic;
    }
}
