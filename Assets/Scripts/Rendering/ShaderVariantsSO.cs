// Decompiled with JetBrains decompiler
// Type: ShaderVariantsSO
// Assembly: Rendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B7D31E1-1F9E-4BE6-B735-D2EB47EFDA46
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Rendering.dll

using System;
using UnityEngine;
using UnityEngine.Rendering;

public class ShaderVariantsSO : PersistentScriptableObject
{
  [SerializeField]
  [Reorderable]
  private ShaderVariantsSO.ShaderVariant[] _shaderVariants;

  public ShaderVariantsSO.ShaderVariant[] shaderVariants => this._shaderVariants;

  public void Init(ShaderVariantsSO.ShaderVariant[] shaderVariants) => this._shaderVariants = shaderVariants;

  [Serializable]
  public class ShaderVariant
  {
    [SerializeField]
    private ShaderVariantsSO.ShaderVariant.Variant[] _variants;
    [SerializeField]
    private Shader _shader;

    public ShaderVariantsSO.ShaderVariant.Variant[] variants => this._variants;

    public Shader shader => this._shader;

    public ShaderVariant(Shader shader, ShaderVariantsSO.ShaderVariant.Variant[] variants)
    {
      this._shader = shader;
      this._variants = variants;
    }

    [Serializable]
    public class Variant
    {
      [SerializeField]
      private PassType _passType;
      [SerializeField]
      private string _keywords;

      public PassType passType => this._passType;

      public string keywords => this._keywords;

      public Variant(PassType passType, string keywords)
      {
        this._passType = passType;
        this._keywords = keywords;
      }
    }
  }
}
