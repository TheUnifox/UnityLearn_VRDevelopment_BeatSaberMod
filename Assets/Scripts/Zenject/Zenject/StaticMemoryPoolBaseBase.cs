using System;
using System.Collections.Generic;
using ModestTree;

namespace Zenject
{
    // Token: 0x020001E3 RID: 483
    [NoReflectionBaking]
    public abstract class StaticMemoryPoolBaseBase<TValue> : IDespawnableMemoryPool<TValue>, IMemoryPool, IDisposable where TValue : class
    {
        // Token: 0x06000A04 RID: 2564 RVA: 0x0001A984 File Offset: 0x00018B84
        public StaticMemoryPoolBaseBase(Action<TValue> onDespawnedMethod)
        {
            this._onDespawnedMethod = onDespawnedMethod;
        }

        // Token: 0x17000082 RID: 130
        // (set) Token: 0x06000A05 RID: 2565 RVA: 0x0001A9A0 File Offset: 0x00018BA0
        public Action<TValue> OnDespawnedMethod
        {
            set
            {
                this._onDespawnedMethod = value;
            }
        }

        // Token: 0x17000083 RID: 131
        // (get) Token: 0x06000A06 RID: 2566 RVA: 0x0001A9AC File Offset: 0x00018BAC
        public int NumTotal
        {
            get
            {
                return this.NumInactive + this.NumActive;
            }
        }

        // Token: 0x17000084 RID: 132
        // (get) Token: 0x06000A07 RID: 2567 RVA: 0x0001A9BC File Offset: 0x00018BBC
        public int NumActive
        {
            get
            {
                return this._activeCount;
            }
        }

        // Token: 0x17000085 RID: 133
        // (get) Token: 0x06000A08 RID: 2568 RVA: 0x0001A9C4 File Offset: 0x00018BC4
        public int NumInactive
        {
            get
            {
                return this._stack.Count;
            }
        }

        // Token: 0x17000086 RID: 134
        // (get) Token: 0x06000A09 RID: 2569 RVA: 0x0001A9D4 File Offset: 0x00018BD4
        public Type ItemType
        {
            get
            {
                return typeof(TValue);
            }
        }

        // Token: 0x06000A0A RID: 2570 RVA: 0x0001A9E0 File Offset: 0x00018BE0
        public void Resize(int desiredPoolSize)
        {
            this.ResizeInternal(desiredPoolSize);
        }

        // Token: 0x06000A0B RID: 2571 RVA: 0x0001A9EC File Offset: 0x00018BEC
        private void ResizeInternal(int desiredPoolSize)
        {
            ModestTree.Assert.That(desiredPoolSize >= 0, "Attempted to resize the pool to a negative amount");
            while (this._stack.Count > desiredPoolSize)
            {
                this._stack.Pop();
            }
            while (desiredPoolSize > this._stack.Count)
            {
                this._stack.Push(this.Alloc());
            }
            ModestTree.Assert.IsEqual(this._stack.Count, desiredPoolSize);
        }

        // Token: 0x06000A0C RID: 2572 RVA: 0x0001AA64 File Offset: 0x00018C64
        public void Dispose()
        {
        }

        // Token: 0x06000A0D RID: 2573 RVA: 0x0001AA68 File Offset: 0x00018C68
        public void ClearActiveCount()
        {
            this._activeCount = 0;
        }

        // Token: 0x06000A0E RID: 2574 RVA: 0x0001AA74 File Offset: 0x00018C74
        public void Clear()
        {
            this.Resize(0);
        }

        // Token: 0x06000A0F RID: 2575 RVA: 0x0001AA80 File Offset: 0x00018C80
        public void ShrinkBy(int numToRemove)
        {
            this.ResizeInternal(this._stack.Count - numToRemove);
        }

        // Token: 0x06000A10 RID: 2576 RVA: 0x0001AA98 File Offset: 0x00018C98
        public void ExpandBy(int numToAdd)
        {
            this.ResizeInternal(this._stack.Count + numToAdd);
        }

        // Token: 0x06000A11 RID: 2577 RVA: 0x0001AAB0 File Offset: 0x00018CB0
        protected TValue SpawnInternal()
        {
            TValue result;
            if (this._stack.Count == 0)
            {
                result = this.Alloc();
            }
            else
            {
                result = this._stack.Pop();
            }
            this._activeCount++;
            return result;
        }

        // Token: 0x06000A12 RID: 2578 RVA: 0x0001AAF0 File Offset: 0x00018CF0
        void IMemoryPool.Despawn(object item)
        {
            this.Despawn((TValue)((object)item));
        }

        // Token: 0x06000A13 RID: 2579 RVA: 0x0001AB00 File Offset: 0x00018D00
        public void Despawn(TValue element)
        {
            if (this._onDespawnedMethod != null)
            {
                this._onDespawnedMethod(element);
            }
            ModestTree.Assert.That(!this._stack.Contains(element), "Attempted to despawn element twice!");
            this._activeCount--;
            this._stack.Push(element);
        }

        // Token: 0x06000A14 RID: 2580
        protected abstract TValue Alloc();

        // Token: 0x040002F9 RID: 761
        private readonly Stack<TValue> _stack = new Stack<TValue>();

        // Token: 0x040002FA RID: 762
        private Action<TValue> _onDespawnedMethod;

        // Token: 0x040002FB RID: 763
        private int _activeCount;
    }
}
