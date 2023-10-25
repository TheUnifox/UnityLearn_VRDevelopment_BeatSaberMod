// Decompiled with JetBrains decompiler
// Type: MaterialPropertyBlockController
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using System.Collections.Generic;
using UnityEngine;

public class MaterialPropertyBlockController : MonoBehaviour
{
  [SerializeField]
  protected Renderer[] _renderers;
  protected MaterialPropertyBlock _materialPropertyBlock;
  protected List<int> _numberOfMaterialsInRenderers;
  protected bool _isInitialized;

  public Renderer[] renderers => this._renderers;

  public MaterialPropertyBlock materialPropertyBlock => this._materialPropertyBlock ?? (this._materialPropertyBlock = new MaterialPropertyBlock());

  public virtual void ApplyChanges()
  {
    if (!this._isInitialized)
    {
      this._numberOfMaterialsInRenderers = new List<int>(this._renderers.Length);
      for (int index = 0; index < this._renderers.Length; ++index)
        this._numberOfMaterialsInRenderers.Add(this._renderers[index].sharedMaterials.Length);
      this._isInitialized = true;
    }
    for (int index = 0; index < this._renderers.Length; ++index)
    {
      for (int materialIndex = 0; materialIndex < this._numberOfMaterialsInRenderers[index]; ++materialIndex)
        this._renderers[index].SetPropertyBlock(this._materialPropertyBlock, materialIndex);
    }
  }
}
