// Decompiled with JetBrains decompiler
// Type: BakedLightTexturePacking
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;

public abstract class BakedLightTexturePacking
{
  private const string kBakedLightTexturePackingShaderName = "Hidden/BakedLightTexturePacking";
  [DoesNotRequireDomainReloadInit]
  private static readonly int[] _texPropertyIds = new int[3]
  {
    Shader.PropertyToID("_Tex1"),
    Shader.PropertyToID("_Tex2"),
    Shader.PropertyToID("_Tex3")
  };
  [DoesNotRequireDomainReloadInit]
  private static Material _material;

  public static RenderTexture PackTextures(
    IReadOnlyList<RenderTexture> textures,
    RenderTextureDescriptor descriptor)
  {
    if ((Object) BakedLightTexturePacking._material == (Object) null)
      BakedLightTexturePacking._material = new Material(Shader.Find("Hidden/BakedLightTexturePacking"));
    RenderTexture dest = new RenderTexture(descriptor);
    for (int index = 0; index < BakedLightTexturePacking._texPropertyIds.Length; ++index)
      BakedLightTexturePacking._material.SetTexture(BakedLightTexturePacking._texPropertyIds[index], index < textures.Count ? (Texture) textures[index] : (Texture) null);
    Graphics.Blit((Texture) null, dest, BakedLightTexturePacking._material);
    return dest;
  }
}
