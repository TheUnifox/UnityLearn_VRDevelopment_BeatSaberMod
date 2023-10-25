// Decompiled with JetBrains decompiler
// Type: Priority_Queue.GenericPriorityQueueNode`1
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;

namespace Priority_Queue
{
  public class GenericPriorityQueueNode<TPriority>
  {
    [CompilerGenerated]
    protected TPriority m_CPriority;
    [CompilerGenerated]
    protected int m_CQueueIndex;
    [CompilerGenerated]
    protected long m_CInsertionIndex;

    public TPriority Priority
    {
      get => this.m_CPriority;
      protected internal set => this.m_CPriority = value;
    }

    public int QueueIndex
    {
      get => this.m_CQueueIndex;
      internal set => this.m_CQueueIndex = value;
    }

    public long InsertionIndex
    {
      get => this.m_CInsertionIndex;
      internal set => this.m_CInsertionIndex = value;
    }
  }
}
