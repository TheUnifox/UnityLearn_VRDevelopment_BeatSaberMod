// Decompiled with JetBrains decompiler
// Type: AvatarPartCollection`1
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;

public class AvatarPartCollection<T> where T : Object, IAvatarPart
{
  protected readonly Dictionary<string, T> _partById = new Dictionary<string, T>();
  protected readonly Dictionary<string, int> _partIndexById = new Dictionary<string, int>();
  protected readonly T[] _parts;

  public int count => this._parts.Length;

  public T[] parts => this._parts;

  public AvatarPartCollection(T[] parts)
  {
    this._parts = parts;
    for (int index = 0; index < parts.Length; ++index)
    {
      T part = parts[index];
      this._partById[part.id] = part;
      this._partIndexById[part.id] = index;
    }
  }

  public virtual T GetById(string id)
  {
    if (string.IsNullOrEmpty(id))
      return default (T);
    T byId;
    this._partById.TryGetValue(id, out byId);
    return byId;
  }

  public virtual T GetRandom() => this._parts[Random.Range(0, this._parts.Length)];

  public virtual T GetByIndex(int index) => index >= 0 && index < this._parts.Length ? this._parts[index] : default (T);

  public virtual int GetIndexById(string id)
  {
    int num;
    return string.IsNullOrEmpty(id) || !this._partIndexById.TryGetValue(id, out num) ? 0 : num;
  }

  public virtual T GetDefault() => this.GetByIndex(0);
}
