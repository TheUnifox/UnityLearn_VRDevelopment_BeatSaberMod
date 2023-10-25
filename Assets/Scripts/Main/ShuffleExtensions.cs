// Decompiled with JetBrains decompiler
// Type: ShuffleExtensions
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;

public static class ShuffleExtensions
{
  public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random random)
  {
    List<T> objList = new List<T>();
    foreach (T obj in source)
    {
      int index = random.Next(objList.Count + 1);
      if (index == objList.Count)
      {
        objList.Add(obj);
      }
      else
      {
        objList.Add(objList[index]);
        objList[index] = obj;
      }
    }
    return (IEnumerable<T>) objList;
  }

  public static IEnumerable<T> PickRandomElementsWithTombstone<T>(
    this IEnumerable<T> source,
    int limit,
    int count,
    Random random,
    T tombstone)
  {
    int index = 0;
    int picked = 0;
    foreach (T obj in source)
    {
      if (random.Next(count - index) < limit - picked)
      {
        ++picked;
        yield return obj;
      }
      else
        yield return tombstone;
      ++index;
    }
  }

  public static IEnumerable<T> TakeWithTombstone<T>(
    this IEnumerable<T> source,
    int limit,
    T tombstone)
  {
    using (IEnumerator<T> enumerator = source.GetEnumerator())
    {
      int index = 0;
      while (enumerator.MoveNext())
      {
        if (index < limit)
          yield return enumerator.Current;
        else
          yield return tombstone;
        ++index;
      }
    }
  }

  public static IEnumerable<(T1, T2)> ZipSkipTombstone<T1, T2>(
    this IEnumerable<T1> collection1,
    IEnumerable<T2> collection2,
    T2 collection2Tombstone)
  {
    using (IEnumerator<T1> enum1 = collection1.GetEnumerator())
    {
      using (IEnumerator<T2> enum2 = collection2.GetEnumerator())
      {
        while (enum1.MoveNext() && enum2.MoveNext())
        {
          if (!collection2Tombstone.Equals((object) enum2.Current))
            yield return (enum1.Current, enum2.Current);
        }
      }
    }
  }
}
