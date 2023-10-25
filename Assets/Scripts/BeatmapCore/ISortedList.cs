using System;
using System.Collections.Generic;

// Token: 0x0200003C RID: 60
public interface ISortedList<T> where T : IComparable<T>
{
    // Token: 0x17000041 RID: 65
    // (get) Token: 0x0600012C RID: 300
    LinkedList<T> items { get; }

    // Token: 0x0600012D RID: 301
    LinkedListNode<T> Insert(T newItem);

    // Token: 0x0600012E RID: 302
    void Remove(LinkedListNode<T> node);

    // Token: 0x0600012F RID: 303
    void TouchLastUsedNode(LinkedListNode<T> node);
}