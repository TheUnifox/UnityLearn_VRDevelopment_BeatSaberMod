// Decompiled with JetBrains decompiler
// Type: Priority_Queue.StablePriorityQueueNode
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;

namespace Priority_Queue
{
  public class StablePriorityQueueNode : FastPriorityQueueNode
  {
    [CompilerGenerated]
    protected long m_CInsertionIndex;

    public long InsertionIndex
    {
      get => this.m_CInsertionIndex;
      internal set => this.m_CInsertionIndex = value;
    }
  }
}
