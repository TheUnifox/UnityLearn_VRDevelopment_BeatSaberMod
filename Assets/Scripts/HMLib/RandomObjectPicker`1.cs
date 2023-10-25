// Decompiled with JetBrains decompiler
// Type: RandomObjectPicker`1
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System;
using UnityEngine;

public class RandomObjectPicker<T>
{
  protected T[] _objects;
  protected float _lastPickTime = -1000f;
  protected float _minimumPickInterval;

  public RandomObjectPicker(T obj, float minimumPickInterval)
  {
    this._objects = new T[1];
    this._objects[0] = obj;
    this._minimumPickInterval = minimumPickInterval;
  }

  public RandomObjectPicker(T[] objects, float minimumPickInterval)
  {
    this._objects = new T[objects.Length];
    Array.Copy((Array) objects, (Array) this._objects, objects.Length);
    this._minimumPickInterval = minimumPickInterval;
  }

  public virtual T PickRandomObject()
  {
    float timeSinceLevelLoad = Time.timeSinceLevelLoad;
    if ((double) timeSinceLevelLoad - (double) this._lastPickTime < (double) this._minimumPickInterval)
      return default (T);
    this._lastPickTime = timeSinceLevelLoad;
    if (this._objects.Length == 1)
      return this._objects[0];
    int index = UnityEngine.Random.Range(0, this._objects.Length - 1);
    T obj = this._objects[index];
    this._objects[index] = this._objects[this._objects.Length - 1];
    this._objects[this._objects.Length - 1] = obj;
    return obj;
  }
}
