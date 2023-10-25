using System;
using System.Collections.Generic;

// Token: 0x0200003D RID: 61
public class SortedList<TBase> : SortedList<TBase, TBase> where TBase : class, IComparable<TBase>
{
    // Token: 0x06000130 RID: 304 RVA: 0x00004C70 File Offset: 0x00002E70
    public SortedList() : base(null)
    {
    }

    // Token: 0x06000131 RID: 305 RVA: 0x00004C79 File Offset: 0x00002E79
    public SortedList(ISortedListItemProcessor<TBase> sortedListDataProcessor) : base(sortedListDataProcessor)
    {
    }
}

// Token: 0x0200003E RID: 62
public class SortedList<T, TBase> : ISortedList<TBase> where TBase : class, IComparable<TBase>
{
    // Token: 0x17000042 RID: 66
    // (get) Token: 0x06000132 RID: 306 RVA: 0x00004C82 File Offset: 0x00002E82
    public LinkedList<TBase> items
    {
        get
        {
            return this._items;
        }
    }

    // Token: 0x06000133 RID: 307 RVA: 0x00004C8A File Offset: 0x00002E8A
    public SortedList(ISortedListItemProcessor<TBase> sortedListDataProcessor)
    {
        this._sortedListDataProcessor = sortedListDataProcessor;
    }

    // Token: 0x06000134 RID: 308 RVA: 0x00004CA4 File Offset: 0x00002EA4
    public virtual LinkedListNode<TBase> Insert(TBase newItem)
    {
        LinkedListNode<TBase> linkedListNode = this.InsertInternal(newItem);
        ISortedListItemProcessor < TBase > sortedListDataProcessor = this._sortedListDataProcessor;
        if (sortedListDataProcessor != null)
        {
            sortedListDataProcessor.ProcessInsertedData(linkedListNode);
        }
        return linkedListNode;
    }

    // Token: 0x06000135 RID: 309 RVA: 0x00004CCC File Offset: 0x00002ECC
    public virtual void Remove(LinkedListNode<TBase> node)
    {
        if (node.Previous != null)
        {
            this._lastUsedNode = node.Previous;
        }
        else if (node.Next != null)
        {
            this._lastUsedNode = node.Next;
        }
        else
        {
            this._lastUsedNode = null;
        }
        ISortedListItemProcessor < TBase > sortedListDataProcessor = this._sortedListDataProcessor;
        if (sortedListDataProcessor != null)
        {
            sortedListDataProcessor.ProcessBeforeDeleteData(node);
        }
        this._items.Remove(node);
    }

    // Token: 0x06000136 RID: 310 RVA: 0x00004D2A File Offset: 0x00002F2A
    public virtual void TouchLastUsedNode(LinkedListNode<TBase> node)
    {
        this._lastUsedNode = node;
    }

    // Token: 0x06000137 RID: 311 RVA: 0x00004D34 File Offset: 0x00002F34
    public virtual LinkedListNode<TBase> InsertInternal(TBase newItem)
    {
        LinkedListNode<TBase> linkedListNode = this._lastUsedNode;
        if (linkedListNode == null)
        {
            this._lastUsedNode = this._items.AddLast(newItem);
            return this._lastUsedNode;
        }
        if (newItem.CompareTo(linkedListNode.Value) >= 0)
        {
            while (linkedListNode != null)
            {
                if (newItem.CompareTo(linkedListNode.Value) == -1)
                {
                    return this._lastUsedNode = this._items.AddBefore(linkedListNode, newItem);
                }
                linkedListNode = linkedListNode.Next;
            }
            return this._lastUsedNode = this._items.AddLast(newItem);
        }
        while (linkedListNode != null)
        {
            if (newItem.CompareTo(linkedListNode.Value) == 1)
            {
                return this._lastUsedNode = this._items.AddAfter(linkedListNode, newItem);
            }
            linkedListNode = linkedListNode.Previous;
        }
        return this._lastUsedNode = this._items.AddFirst(newItem);
    }

    // Token: 0x04000101 RID: 257
    protected readonly LinkedList<TBase> _items = new LinkedList<TBase>();

    // Token: 0x04000102 RID: 258
    protected readonly ISortedListItemProcessor<TBase> _sortedListDataProcessor;

    // Token: 0x04000103 RID: 259
    protected LinkedListNode<TBase> _lastUsedNode;
}
