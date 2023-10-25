// Decompiled with JetBrains decompiler
// Type: BTSCharacterMaterialSwitcher
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class BTSCharacterMaterialSwitcher : MonoBehaviour
{
  [SerializeField]
  protected BTSCharacterMaterialSwitcher.RendererMaterialsPairs[] _rendererMaterialsPairs;

  public virtual void SwapMaterials(bool alternative)
  {
    foreach (BTSCharacterMaterialSwitcher.RendererMaterialsPairs rendererMaterialsPair in this._rendererMaterialsPairs)
    {
      Material[] materials = rendererMaterialsPair.renderer.materials;
      foreach (BTSCharacterMaterialSwitcher.MaterialPairs materialPair in rendererMaterialsPair.materialPairs)
        materials[materialPair.materialIndex] = alternative ? materialPair.alternativeMaterial : materialPair.defaultMaterial;
      rendererMaterialsPair.renderer.materials = materials;
    }
  }

  [Serializable]
  public class RendererMaterialsPairs
  {
    [SerializeField]
    protected Renderer _renderer;
    [SerializeField]
    protected List<BTSCharacterMaterialSwitcher.MaterialPairs> _materialPairs;

    public Renderer renderer => this._renderer;

    public List<BTSCharacterMaterialSwitcher.MaterialPairs> materialPairs => this._materialPairs;

    public RendererMaterialsPairs(Renderer renderer)
    {
      this._renderer = renderer;
      this._materialPairs = new List<BTSCharacterMaterialSwitcher.MaterialPairs>();
    }
  }

  [Serializable]
  public class MaterialPairs
  {
    public int materialIndex;
    public Material defaultMaterial;
    public Material alternativeMaterial;
  }
}
