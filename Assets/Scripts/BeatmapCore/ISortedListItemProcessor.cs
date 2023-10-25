using System;
using System.Collections.Generic;

// Token: 0x0200003B RID: 59
public interface ISortedListItemProcessor<T>
{
    // Token: 0x0600012A RID: 298
    void ProcessInsertedData(LinkedListNode<T> insertedNode);

    // Token: 0x0600012B RID: 299
    void ProcessBeforeDeleteData(LinkedListNode<T> nodeToDelete);
}
