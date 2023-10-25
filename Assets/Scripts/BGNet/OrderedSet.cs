using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Token: 0x02000069 RID: 105
public class OrderedSet<T> : IEnumerable<T>, IEnumerable
{
    // Token: 0x170000C1 RID: 193
    // (get) Token: 0x06000490 RID: 1168 RVA: 0x0000BDBC File Offset: 0x00009FBC
    public int count
    {
        get
        {
            return this._sortIndices.Count;
        }
    }

    // Token: 0x06000491 RID: 1169 RVA: 0x0000BDC9 File Offset: 0x00009FC9
    public OrderedSet(Comparison<T> comparison, OrderedSet<T>.ProcessOrder processOrder = OrderedSet<T>.ProcessOrder.DontCare)
    {
        this._comparison = comparison;
        this._processOrder = processOrder;
    }

    // Token: 0x06000492 RID: 1170 RVA: 0x0000BDEC File Offset: 0x00009FEC
    public void Add(T item)
    {
        OrderedSet<T>.Node node;
        if (!this._sortIndices.TryGetValue(item, out node))
        {
            node = new OrderedSet<T>.Node(item, this._clearCount);
            this._sortIndices.Add(item, node);
            this.AppendNode(node);
        }
        this.UpdateSortedPosition(node);
    }

    // Token: 0x06000493 RID: 1171 RVA: 0x0000BE31 File Offset: 0x0000A031
    public void Clear()
    {
        this._sortIndices.Clear();
        this._head = null;
        this._tail = null;
        this._clearCount++;
    }

    // Token: 0x06000494 RID: 1172 RVA: 0x0000BE5A File Offset: 0x0000A05A
    public bool Contains(T item)
    {
        return this._sortIndices.ContainsKey(item);
    }

    // Token: 0x06000495 RID: 1173 RVA: 0x0000BE68 File Offset: 0x0000A068
    public bool Remove(T item)
    {
        OrderedSet<T>.Node node;
        if (!this._sortIndices.TryGetValue(item, out node))
        {
            return false;
        }
        this._sortIndices.Remove(item);
        this.RemoveNode(node);
        node.isRemoved = true;
        return true;
    }

    // Token: 0x06000496 RID: 1174 RVA: 0x0000BEA4 File Offset: 0x0000A0A4
    public void UpdateSortedPosition(T item)
    {
        OrderedSet<T>.Node node;
        if (!this._sortIndices.TryGetValue(item, out node))
        {
            return;
        }
        this.UpdateSortedPosition(node);
    }

    // Token: 0x06000497 RID: 1175 RVA: 0x0000BECC File Offset: 0x0000A0CC
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void AppendNode(OrderedSet<T>.Node node)
    {
        OrderedSet<T>.Node tail = this._tail;
        node.previous = tail;
        node.next = null;
        this._tail = node;
        if (tail == null)
        {
            this._head = node;
            return;
        }
        tail.next = node;
    }

    // Token: 0x06000498 RID: 1176 RVA: 0x0000BF08 File Offset: 0x0000A108
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void AppendNodeUnchecked(OrderedSet<T>.Node node)
    {
        OrderedSet<T>.Node tail = this._tail;
        node.previous = tail;
        node.next = null;
        tail.next = node;
        this._tail = node;
    }

    // Token: 0x06000499 RID: 1177 RVA: 0x0000BF38 File Offset: 0x0000A138
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void PrependNode(OrderedSet<T>.Node node)
    {
        OrderedSet<T>.Node head = this._head;
        node.next = head;
        node.previous = null;
        this._head = node;
        if (head == null)
        {
            this._tail = node;
            return;
        }
        head.previous = node;
    }

    // Token: 0x0600049A RID: 1178 RVA: 0x0000BF74 File Offset: 0x0000A174
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void PrependNodeUnchecked(OrderedSet<T>.Node node)
    {
        OrderedSet<T>.Node head = this._head;
        node.next = head;
        node.previous = null;
        head.previous = node;
        this._head = node;
    }

    // Token: 0x0600049B RID: 1179 RVA: 0x0000BFA4 File Offset: 0x0000A1A4
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void SwapNodes(OrderedSet<T>.Node previous, OrderedSet<T>.Node next)
    {
        OrderedSet<T>.Node previous2 = previous.previous;
        OrderedSet<T>.Node next2 = next.next;
        previous.previous = next;
        previous.next = next2;
        next.next = previous;
        next.previous = previous2;
        if (previous2 == null)
        {
            this._head = next;
        }
        else
        {
            previous2.next = next;
        }
        if (next2 == null)
        {
            this._tail = previous;
            return;
        }
        next2.previous = previous;
    }

    // Token: 0x0600049C RID: 1180 RVA: 0x0000C000 File Offset: 0x0000A200
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void RemoveNode(OrderedSet<T>.Node node)
    {
        OrderedSet<T>.Node previous = node.previous;
        OrderedSet<T>.Node next = node.next;
        if (next != null)
        {
            next.previous = previous;
        }
        else
        {
            this._tail = previous;
        }
        if (previous != null)
        {
            previous.next = next;
            return;
        }
        this._head = next;
    }

    // Token: 0x0600049D RID: 1181 RVA: 0x0000C040 File Offset: 0x0000A240
    private void UpdateSortedPosition(OrderedSet<T>.Node node)
    {
        if (node != this._tail && this._comparison(node.value, this._tail.value) >= 0)
        {
            this.RemoveNode(node);
            this.AppendNodeUnchecked(node);
            if (this._processOrder != OrderedSet<T>.ProcessOrder.Lifo)
            {
                return;
            }
        }
        if (node != this._head && this._comparison(node.value, this._head.value) <= 0)
        {
            this.RemoveNode(node);
            this.PrependNodeUnchecked(node);
            if (this._processOrder != OrderedSet<T>.ProcessOrder.Fifo)
            {
                return;
            }
        }
        while (node.previous != null)
        {
            OrderedSet<T>.Node previous = node.previous;
            int num = this._comparison(node.value, previous.value);
            if (num > 0 || (num == 0 && this._processOrder != OrderedSet<T>.ProcessOrder.Lifo))
            {
                while (node.next != null)
                {
                    OrderedSet<T>.Node next = node.next;
                    int num2 = this._comparison(node.value, next.value);
                    if (num2 < 0 || (num2 == 0 && this._processOrder != OrderedSet<T>.ProcessOrder.Fifo))
                    {
                        break;
                    }
                    this.SwapNodes(node, next);
                }
                return;
            }
            this.SwapNodes(previous, node);
        }
        while (node.next != null)
        {
            OrderedSet<T>.Node next = node.next;
            int num2 = this._comparison(node.value, next.value);
            if (num2 < 0 || (num2 == 0 && this._processOrder != OrderedSet<T>.ProcessOrder.Fifo))
            {
                break;
            }
            this.SwapNodes(node, next);
        }
    }

    // Token: 0x0600049E RID: 1182 RVA: 0x0000C14B File Offset: 0x0000A34B
    public IEnumerator<T> GetEnumerator()
    {
        OrderedSet<T>.Node node = this._head;
        while (node != null && node.clearCount == this._clearCount)
        {
            OrderedSet<T>.Node next = node.next;
            if (!node.isRemoved)
            {
                yield return node.value;
            }
            node = next;
            next = null;
        }
        yield break;
    }

    // Token: 0x0600049F RID: 1183 RVA: 0x0000C15A File Offset: 0x0000A35A
    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    // Token: 0x060004A0 RID: 1184 RVA: 0x0000C164 File Offset: 0x0000A364
    public T GetFirstOrDefault()
    {
        if (this._head == null)
        {
            return default(T);
        }
        return this._head.value;
    }

    // Token: 0x060004A1 RID: 1185 RVA: 0x0000C18E File Offset: 0x0000A38E
    public bool TryGetFirst(out T value)
    {
        value = this.GetFirstOrDefault();
        return this._head != null;
    }

    // Token: 0x040001BB RID: 443
    private readonly Comparison<T> _comparison;

    // Token: 0x040001BC RID: 444
    private readonly OrderedSet<T>.ProcessOrder _processOrder;

    // Token: 0x040001BD RID: 445
    private readonly Dictionary<T, OrderedSet<T>.Node> _sortIndices = new Dictionary<T, OrderedSet<T>.Node>();

    // Token: 0x040001BE RID: 446
    private OrderedSet<T>.Node _head;

    // Token: 0x040001BF RID: 447
    private OrderedSet<T>.Node _tail;

    // Token: 0x040001C0 RID: 448
    private int _clearCount;

    // Token: 0x0200013B RID: 315
    public enum ProcessOrder
    {
        // Token: 0x04000409 RID: 1033
        Lifo,
        // Token: 0x0400040A RID: 1034
        Fifo,
        // Token: 0x0400040B RID: 1035
        DontCare
    }

    // Token: 0x0200013C RID: 316
    private class Node
    {
        // Token: 0x0600080F RID: 2063 RVA: 0x00014F40 File Offset: 0x00013140
        public Node(T value, int clearCount)
        {
            this.value = value;
            this.clearCount = clearCount;
        }

        // Token: 0x0400040C RID: 1036
        public readonly T value;

        // Token: 0x0400040D RID: 1037
        public OrderedSet<T>.Node previous;

        // Token: 0x0400040E RID: 1038
        public OrderedSet<T>.Node next;

        // Token: 0x0400040F RID: 1039
        public bool isRemoved;

        // Token: 0x04000410 RID: 1040
        public int clearCount;
    }
}
