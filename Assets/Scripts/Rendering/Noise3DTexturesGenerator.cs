// Decompiled with JetBrains decompiler
// Type: Noise3DTexturesGenerator
// Assembly: Rendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B7D31E1-1F9E-4BE6-B735-D2EB47EFDA46
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Rendering.dll

using System;
using UnityEngine;

[ExecuteInEditMode]
public class Noise3DTexturesGenerator : MonoBehaviour
{
  [SerializeField]
  private Noise3DTexturesGenerator.MaterialTextureParamsCouple[] _data;
  private static Texture3D _texture;

  protected void Awake()
  {
    for (int index1 = 0; index1 < this._data.Length; ++index1)
    {
      Noise3DTexturesGenerator.MaterialTextureParamsCouple textureParamsCouple = this._data[index1];
      if ((UnityEngine.Object) Noise3DTexturesGenerator._texture == (UnityEngine.Object) null)
      {
        Color32[] noisePixels = Noise3DTexturesGenerator.CreateNoisePixels(16, 16, 16, 6f, 6, 1.8f);
        Noise3DTexturesGenerator._texture = new Texture3D(16, 16, 16, TextureFormat.Alpha8, false);
        Noise3DTexturesGenerator._texture.SetPixels32(noisePixels);
        Noise3DTexturesGenerator._texture.Apply();
      }
      if (!string.IsNullOrEmpty(textureParamsCouple.globalPropertyName))
        Shader.SetGlobalTexture(textureParamsCouple.globalPropertyName, (Texture) Noise3DTexturesGenerator._texture);
      for (int index2 = 0; index2 < textureParamsCouple.materialPropertyNameCouples.Length; ++index2)
      {
        Noise3DTexturesGenerator.MaterialPropertyNameCouple propertyNameCouple = textureParamsCouple.materialPropertyNameCouples[index2];
        propertyNameCouple.material.SetTexture(propertyNameCouple.texturePropertyName, (Texture) Noise3DTexturesGenerator._texture);
      }
    }
  }

  private static Color32[] CreateNoisePixels(
    int width,
    int height,
    int depth,
    float scale,
    int repeat,
    float contrast)
  {
    Color32[] noisePixels = new Color32[width * height * depth];
    for (int index1 = 0; index1 < depth; ++index1)
    {
      for (int index2 = 0; index2 < height; ++index2)
      {
        for (int index3 = 0; index3 < width; ++index3)
        {
          byte num = (byte) Mathf.RoundToInt(Mathf.Clamp01((float) (((double) PerlinNoise.Perlin3D(scale * (float) index3 / (float) width, scale * (float) index2 / (float) height, scale * (float) index1 / (float) depth, repeat) - 0.5) * (double) contrast + 0.5)) * (float) byte.MaxValue);
          noisePixels[index3 + index2 * height + index1 * height * depth].r = num;
          noisePixels[index3 + index2 * height + index1 * height * depth].g = num;
          noisePixels[index3 + index2 * height + index1 * height * depth].b = num;
          noisePixels[index3 + index2 * height + index1 * height * depth].a = num;
        }
      }
    }
    return noisePixels;
  }

  [Serializable]
  public struct MaterialTextureParamsCouple
  {
    public string globalPropertyName;
    public Noise3DTexturesGenerator.MaterialPropertyNameCouple[] materialPropertyNameCouples;
  }

  [Serializable]
  public struct MaterialPropertyNameCouple
  {
    public string texturePropertyName;
    public Material material;
  }
}
