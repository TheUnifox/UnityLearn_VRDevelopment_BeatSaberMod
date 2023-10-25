// Decompiled with JetBrains decompiler
// Type: PseudoHDREncoding
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public abstract class PseudoHDREncoding
{
  [DoesNotRequireDomainReloadInit]
  private const string kPseudoHDREncodingShaderName = "Hidden/PseudoHDREncoding";
  [DoesNotRequireDomainReloadInit]
  private static Material _material;

  public static RenderTexture CreatePseudoHDREncodedTexture(RenderTexture src)
  {
    RenderTexture dest = new RenderTexture(src.descriptor with
    {
      sRGB = true
    });
    if ((Object) PseudoHDREncoding._material == (Object) null)
      PseudoHDREncoding._material = new Material(Shader.Find("Hidden/PseudoHDREncoding"));
    Graphics.Blit((Texture) src, dest, PseudoHDREncoding._material);
    return dest;
  }
}
