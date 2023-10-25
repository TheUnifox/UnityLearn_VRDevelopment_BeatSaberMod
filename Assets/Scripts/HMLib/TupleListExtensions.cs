// Decompiled with JetBrains decompiler
// Type: TupleListExtensions
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System;
using System.Collections.Generic;

public static class TupleListExtensions
{
  public static void Add<T1, T2>(this IList<Tuple<T1, T2>> list, T1 item1, T2 item2) => list.Add(Tuple.Create<T1, T2>(item1, item2));

  public static void Add<T1, T2, T3>(
    this IList<Tuple<T1, T2, T3>> list,
    T1 item1,
    T2 item2,
    T3 item3)
  {
    list.Add(Tuple.Create<T1, T2, T3>(item1, item2, item3));
  }

  public static void Add<T1, T2, T3, T4>(
    this IList<Tuple<T1, T2, T3, T4>> list,
    T1 item1,
    T2 item2,
    T3 item3,
    T4 item4)
  {
    list.Add(Tuple.Create<T1, T2, T3, T4>(item1, item2, item3, item4));
  }
}
