// Decompiled with JetBrains decompiler
// Type: CommandBufferOwners
// Assembly: Rendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B7D31E1-1F9E-4BE6-B735-D2EB47EFDA46
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Rendering.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CommandBufferOwners
{
  private HashSet<Object> _owners;
  public CommandBuffer commandBuffer;

  public void AddOwner(Object owner)
  {
    if (this._owners == null)
      this._owners = new HashSet<Object>();
    this._owners.Add(owner);
  }

  public void RemoveOwner(Object owner)
  {
    if (this._owners == null)
      return;
    this._owners.Remove(owner);
  }

  public bool ContainsOwner(Object owner) => this._owners.Contains(owner);

  public int NumberOfOwners => this._owners.Count;
}
