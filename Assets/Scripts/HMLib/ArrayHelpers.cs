// Decompiled with JetBrains decompiler
// Type: ArrayHelpers
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System;

public abstract class ArrayHelpers
{
  public static T[] CreateOrEnlargeArray<T>(T[] array, int minimumCapacity)
  {
    if (array == null)
      return new T[minimumCapacity + 1];
    if (array.Length > minimumCapacity)
      return array;
    T[] destinationArray = new T[minimumCapacity + 1];
    Array.Copy((Array) array, (Array) destinationArray, array.Length);
    return destinationArray;
  }
}
