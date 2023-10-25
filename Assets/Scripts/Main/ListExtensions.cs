// Decompiled with JetBrains decompiler
// Type: ListExtensions
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;

public static class ListExtensions
{
  [DoesNotRequireDomainReloadInit]
  private static readonly Random _random = new Random();

  public static int IndexOf<T>(this IReadOnlyList<T> self, T item)
  {
    int num = 0;
    foreach (T objA in (IEnumerable<T>) self)
    {
      if (object.Equals((object) objA, (object) item))
        return num;
      ++num;
    }
    return -1;
  }

  public static void ShuffleInPlace<T>(this IList<T> list)
  {
    int count = list.Count;
    while (count > 1)
    {
      --count;
      int index1 = ListExtensions._random.Next(count + 1);
      IList<T> objList1 = list;
      int num = index1;
      IList<T> objList2 = list;
      int index2 = count;
      T obj1 = list[count];
      T obj2 = list[index1];
      int index3 = num;
      T obj3;
      T obj4 = obj3 = obj1;
      objList1[index3] = obj3;
      objList2[index2] = obj4 = obj2;
    }
  }

  public static void InsertIntoSortedListFromEnd<T>(this List<T> sortedList, T newItem) where T : IComparable<T>
  {
    if (sortedList.Count == 0)
    {
      sortedList.Add(newItem);
    }
    else
    {
      T sorted = sortedList[sortedList.Count - 1];
      if (sorted.CompareTo(newItem) < 0)
      {
        sortedList.Add(newItem);
      }
      else
      {
        for (int index = sortedList.Count - 2; index >= 0; --index)
        {
          sorted = sortedList[index];
          if (sorted.CompareTo(newItem) < 0)
          {
            sortedList.Insert(index + 1, newItem);
            return;
          }
        }
        sortedList.Insert(0, newItem);
      }
    }
  }
}
