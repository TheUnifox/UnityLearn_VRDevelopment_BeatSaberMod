using System;
using System.Collections.Generic;

// Token: 0x02000091 RID: 145
public class SynchronizedActionQueue
{
    // Token: 0x060005DF RID: 1503 RVA: 0x000101F1 File Offset: 0x0000E3F1
    public void Clear()
    {
        this._synchronizedActionQueue.Clear();
    }

    // Token: 0x060005E0 RID: 1504 RVA: 0x000101FE File Offset: 0x0000E3FE
    public void EnqueueAction(float time, Action action)
    {
        this._synchronizedActionQueue.Add(new SynchronizedActionQueue.SynchronizedAction(time, action));
    }

    // Token: 0x060005E1 RID: 1505 RVA: 0x00010214 File Offset: 0x0000E414
    public void Update(float time)
    {
        for (int i = 0; i < this._synchronizedActionQueue.Count; i++)
        {
            if (this._synchronizedActionQueue[i].time <= time)
            {
                Action action = this._synchronizedActionQueue[i].action;
                this._synchronizedActionQueue.RemoveAt(i);
                i--;
                if (action != null)
                {
                    action();
                }
            }
        }
    }

    // Token: 0x0400024C RID: 588
    private readonly List<SynchronizedActionQueue.SynchronizedAction> _synchronizedActionQueue = new List<SynchronizedActionQueue.SynchronizedAction>();

    // Token: 0x02000161 RID: 353
    private readonly struct SynchronizedAction
    {
        // Token: 0x0600088D RID: 2189 RVA: 0x000167C0 File Offset: 0x000149C0
        public SynchronizedAction(float time, Action action)
        {
            this.time = time;
            this.action = action;
        }

        // Token: 0x04000478 RID: 1144
        public readonly float time;

        // Token: 0x04000479 RID: 1145
        public readonly Action action;
    }
}
