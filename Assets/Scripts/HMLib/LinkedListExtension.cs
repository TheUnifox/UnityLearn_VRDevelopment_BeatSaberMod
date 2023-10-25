// Decompiled with JetBrains decompiler
// Type: LinkedListExtension
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System.Collections.Generic;

public static class LinkedListExtension
{
  public static int Index<T>(this LinkedListNode<T> searchNode)
  {
    LinkedList<T> list = searchNode.List;
    T obj = searchNode.Value;
    int num = 0;
    LinkedListNode<T> linkedListNode = list.First;
    while (linkedListNode != null)
    {
      if (obj.Equals((object) linkedListNode.Value))
        return num;
      linkedListNode = linkedListNode.Next;
      ++num;
    }
    return -1;
  }
}
