// Decompiled with JetBrains decompiler
// Type: Priority_Queue.SimplePriorityQueue`2
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Priority_Queue
{
  public class SimplePriorityQueue<TItem, TPriority> : 
    IPriorityQueue<TItem, TPriority>,
    IEnumerable<TItem>,
    IEnumerable
    where TPriority : IComparable<TPriority>
  {
    protected const int INITIAL_QUEUE_SIZE = 10;
    protected readonly GenericPriorityQueue<SimplePriorityQueue<TItem, TPriority>.SimpleNode, TPriority> _queue;
    protected readonly Dictionary<TItem, IList<SimplePriorityQueue<TItem, TPriority>.SimpleNode>> _itemToNodesCache;
    protected readonly IList<SimplePriorityQueue<TItem, TPriority>.SimpleNode> _nullNodesCache;

    public SimplePriorityQueue()
      : this((IComparer<TPriority>) Comparer<TPriority>.Default, (IEqualityComparer<TItem>) EqualityComparer<TItem>.Default)
    {
    }

    public SimplePriorityQueue(IComparer<TPriority> priorityComparer)
      : this(new Comparison<TPriority>(priorityComparer.Compare), (IEqualityComparer<TItem>) EqualityComparer<TItem>.Default)
    {
    }

    public SimplePriorityQueue(Comparison<TPriority> priorityComparer)
      : this(priorityComparer, (IEqualityComparer<TItem>) EqualityComparer<TItem>.Default)
    {
    }

    public SimplePriorityQueue(IEqualityComparer<TItem> itemEquality)
      : this((IComparer<TPriority>) Comparer<TPriority>.Default, itemEquality)
    {
    }

    public SimplePriorityQueue(
      IComparer<TPriority> priorityComparer,
      IEqualityComparer<TItem> itemEquality)
      : this(new Comparison<TPriority>(priorityComparer.Compare), itemEquality)
    {
    }

    public SimplePriorityQueue(
      Comparison<TPriority> priorityComparer,
      IEqualityComparer<TItem> itemEquality)
    {
      this._queue = new GenericPriorityQueue<SimplePriorityQueue<TItem, TPriority>.SimpleNode, TPriority>(10, priorityComparer);
      this._itemToNodesCache = new Dictionary<TItem, IList<SimplePriorityQueue<TItem, TPriority>.SimpleNode>>(itemEquality);
      this._nullNodesCache = (IList<SimplePriorityQueue<TItem, TPriority>.SimpleNode>) new List<SimplePriorityQueue<TItem, TPriority>.SimpleNode>();
    }

    public virtual SimplePriorityQueue<TItem, TPriority>.SimpleNode GetExistingNode(TItem item)
    {
      IList<SimplePriorityQueue<TItem, TPriority>.SimpleNode> simpleNodeList;
      return (object) item == null ? (this._nullNodesCache.Count <= 0 ? (SimplePriorityQueue<TItem, TPriority>.SimpleNode) null : this._nullNodesCache[0]) : (!this._itemToNodesCache.TryGetValue(item, out simpleNodeList) ? (SimplePriorityQueue<TItem, TPriority>.SimpleNode) null : simpleNodeList[0]);
    }

    public virtual void AddToNodeCache(
      SimplePriorityQueue<TItem, TPriority>.SimpleNode node)
    {
      if ((object) node.Data == null)
      {
        this._nullNodesCache.Add(node);
      }
      else
      {
        IList<SimplePriorityQueue<TItem, TPriority>.SimpleNode> simpleNodeList;
        if (!this._itemToNodesCache.TryGetValue(node.Data, out simpleNodeList))
        {
          simpleNodeList = (IList<SimplePriorityQueue<TItem, TPriority>.SimpleNode>) new List<SimplePriorityQueue<TItem, TPriority>.SimpleNode>();
          this._itemToNodesCache[node.Data] = simpleNodeList;
        }
        simpleNodeList.Add(node);
      }
    }

    public virtual void RemoveFromNodeCache(
      SimplePriorityQueue<TItem, TPriority>.SimpleNode node)
    {
      if ((object) node.Data == null)
      {
        this._nullNodesCache.Remove(node);
      }
      else
      {
        IList<SimplePriorityQueue<TItem, TPriority>.SimpleNode> simpleNodeList;
        if (!this._itemToNodesCache.TryGetValue(node.Data, out simpleNodeList))
          return;
        simpleNodeList.Remove(node);
        if (simpleNodeList.Count != 0)
          return;
        this._itemToNodesCache.Remove(node.Data);
      }
    }

    public int Count
    {
      get
      {
        lock (this._queue)
          return this._queue.Count;
      }
    }

    public TItem First
    {
      get
      {
        lock (this._queue)
        {
          if (this._queue.Count <= 0)
            throw new InvalidOperationException("Cannot call .First on an empty queue");
          return this._queue.First.Data;
        }
      }
    }

    public virtual void Clear()
    {
      lock (this._queue)
      {
        this._queue.Clear();
        this._itemToNodesCache.Clear();
        this._nullNodesCache.Clear();
      }
    }

    public virtual bool Contains(TItem item)
    {
      lock (this._queue)
        return (object) item == null ? this._nullNodesCache.Count > 0 : this._itemToNodesCache.ContainsKey(item);
    }

    public virtual TItem Dequeue()
    {
      lock (this._queue)
      {
        SimplePriorityQueue<TItem, TPriority>.SimpleNode node = this._queue.Count > 0 ? this._queue.Dequeue() : throw new InvalidOperationException("Cannot call Dequeue() on an empty queue");
        this.RemoveFromNodeCache(node);
        return node.Data;
      }
    }

    public virtual SimplePriorityQueue<TItem, TPriority>.SimpleNode EnqueueNoLockOrCache(
      TItem item,
      TPriority priority)
    {
      SimplePriorityQueue<TItem, TPriority>.SimpleNode node = new SimplePriorityQueue<TItem, TPriority>.SimpleNode(item);
      if (this._queue.Count == this._queue.MaxSize)
        this._queue.Resize(this._queue.MaxSize * 2 + 1);
      this._queue.Enqueue(node, priority);
      return node;
    }

    public virtual void Enqueue(TItem item, TPriority priority)
    {
      lock (this._queue)
      {
        IList<SimplePriorityQueue<TItem, TPriority>.SimpleNode> simpleNodeList;
        if ((object) item == null)
          simpleNodeList = this._nullNodesCache;
        else if (!this._itemToNodesCache.TryGetValue(item, out simpleNodeList))
        {
          simpleNodeList = (IList<SimplePriorityQueue<TItem, TPriority>.SimpleNode>) new List<SimplePriorityQueue<TItem, TPriority>.SimpleNode>();
          this._itemToNodesCache[item] = simpleNodeList;
        }
        SimplePriorityQueue<TItem, TPriority>.SimpleNode simpleNode = this.EnqueueNoLockOrCache(item, priority);
        simpleNodeList.Add(simpleNode);
      }
    }

    public virtual bool EnqueueWithoutDuplicates(TItem item, TPriority priority)
    {
      lock (this._queue)
      {
        IList<SimplePriorityQueue<TItem, TPriority>.SimpleNode> simpleNodeList;
        if ((object) item == null)
        {
          if (this._nullNodesCache.Count > 0)
            return false;
          simpleNodeList = this._nullNodesCache;
        }
        else
        {
          if (this._itemToNodesCache.ContainsKey(item))
            return false;
          simpleNodeList = (IList<SimplePriorityQueue<TItem, TPriority>.SimpleNode>) new List<SimplePriorityQueue<TItem, TPriority>.SimpleNode>();
          this._itemToNodesCache[item] = simpleNodeList;
        }
        SimplePriorityQueue<TItem, TPriority>.SimpleNode simpleNode = this.EnqueueNoLockOrCache(item, priority);
        simpleNodeList.Add(simpleNode);
        return true;
      }
    }

    public virtual void Remove(TItem item)
    {
      lock (this._queue)
      {
        SimplePriorityQueue<TItem, TPriority>.SimpleNode node;
        IList<SimplePriorityQueue<TItem, TPriority>.SimpleNode> nullNodesCache;
        if ((object) item == null)
        {
          node = this._nullNodesCache.Count != 0 ? this._nullNodesCache[0] : throw new InvalidOperationException("Cannot call Remove() on a node which is not enqueued: " + (object) item);
          nullNodesCache = this._nullNodesCache;
        }
        else
        {
          if (!this._itemToNodesCache.TryGetValue(item, out nullNodesCache))
            throw new InvalidOperationException("Cannot call Remove() on a node which is not enqueued: " + (object) item);
          node = nullNodesCache[0];
          if (nullNodesCache.Count == 1)
            this._itemToNodesCache.Remove(item);
        }
        this._queue.Remove(node);
        nullNodesCache.Remove(node);
      }
    }

    public virtual void UpdatePriority(TItem item, TPriority priority)
    {
      lock (this._queue)
        this._queue.UpdatePriority(this.GetExistingNode(item) ?? throw new InvalidOperationException("Cannot call UpdatePriority() on a node which is not enqueued: " + (object) item), priority);
    }

    public virtual TPriority GetPriority(TItem item)
    {
      lock (this._queue)
        return (this.GetExistingNode(item) ?? throw new InvalidOperationException("Cannot call GetPriority() on a node which is not enqueued: " + (object) item)).Priority;
    }

    public virtual bool TryFirst(out TItem first)
    {
      if (this._queue.Count > 0)
      {
        lock (this._queue)
        {
          if (this._queue.Count > 0)
          {
            first = this._queue.First.Data;
            return true;
          }
        }
      }
      first = default (TItem);
      return false;
    }

    public virtual bool TryDequeue(out TItem first)
    {
      if (this._queue.Count > 0)
      {
        lock (this._queue)
        {
          if (this._queue.Count > 0)
          {
            SimplePriorityQueue<TItem, TPriority>.SimpleNode node = this._queue.Dequeue();
            first = node.Data;
            this.RemoveFromNodeCache(node);
            return true;
          }
        }
      }
      first = default (TItem);
      return false;
    }

    public virtual bool TryRemove(TItem item)
    {
      lock (this._queue)
      {
        SimplePriorityQueue<TItem, TPriority>.SimpleNode node;
        IList<SimplePriorityQueue<TItem, TPriority>.SimpleNode> nullNodesCache;
        if ((object) item == null)
        {
          if (this._nullNodesCache.Count == 0)
            return false;
          node = this._nullNodesCache[0];
          nullNodesCache = this._nullNodesCache;
        }
        else
        {
          if (!this._itemToNodesCache.TryGetValue(item, out nullNodesCache))
            return false;
          node = nullNodesCache[0];
          if (nullNodesCache.Count == 1)
            this._itemToNodesCache.Remove(item);
        }
        this._queue.Remove(node);
        nullNodesCache.Remove(node);
        return true;
      }
    }

    public virtual bool TryUpdatePriority(TItem item, TPriority priority)
    {
      lock (this._queue)
      {
        SimplePriorityQueue<TItem, TPriority>.SimpleNode existingNode = this.GetExistingNode(item);
        if (existingNode == null)
          return false;
        this._queue.UpdatePriority(existingNode, priority);
        return true;
      }
    }

    public virtual bool TryGetPriority(TItem item, out TPriority priority)
    {
      lock (this._queue)
      {
        SimplePriorityQueue<TItem, TPriority>.SimpleNode existingNode = this.GetExistingNode(item);
        if (existingNode == null)
        {
          priority = default (TPriority);
          return false;
        }
        priority = existingNode.Priority;
        return true;
      }
    }

    public virtual IEnumerator<TItem> GetEnumerator()
    {
      List<TItem> objList = new List<TItem>();
      lock (this._queue)
      {
        foreach (SimplePriorityQueue<TItem, TPriority>.SimpleNode simpleNode in this._queue)
          objList.Add(simpleNode.Data);
      }
      return (IEnumerator<TItem>) objList.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public virtual bool IsValidQueue()
    {
      lock (this._queue)
      {
        foreach (IEnumerable<SimplePriorityQueue<TItem, TPriority>.SimpleNode> simpleNodes in this._itemToNodesCache.Values)
        {
          foreach (SimplePriorityQueue<TItem, TPriority>.SimpleNode node in simpleNodes)
          {
            if (!this._queue.Contains(node))
              return false;
          }
        }
        foreach (SimplePriorityQueue<TItem, TPriority>.SimpleNode simpleNode in this._queue)
        {
          if (this.GetExistingNode(simpleNode.Data) == null)
            return false;
        }
        return this._queue.IsValidQueue();
      }
    }

    public class SimpleNode : GenericPriorityQueueNode<TPriority>
    {
      [CompilerGenerated]
      protected TItem m_CData;

      public TItem Data
      {
        get => this.m_CData;
        private set => this.m_CData = value;
      }

      public SimpleNode(TItem data) => this.Data = data;
    }
  }
}
