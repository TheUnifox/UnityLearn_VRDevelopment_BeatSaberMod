// Decompiled with JetBrains decompiler
// Type: Priority_Queue.IFixedSizePriorityQueue`2
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace Priority_Queue
{
  internal interface IFixedSizePriorityQueue<TItem, in TPriority> : 
    IPriorityQueue<TItem, TPriority>,
    IEnumerable<TItem>,
    IEnumerable
    where TPriority : IComparable<TPriority>
  {
    void Resize(int maxNodes);

    int MaxSize { get; }

    void ResetNode(TItem node);
  }
}
