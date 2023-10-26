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
        RenderTextureDescriptor descriptor = src.descriptor;
        descriptor.sRGB = true;
        RenderTexture renderTexture = new RenderTexture(descriptor);
        if (PseudoHDREncoding._material == null)
        {
            PseudoHDREncoding._material = new Material(Shader.Find("Hidden/PseudoHDREncoding"));
        }
        Graphics.Blit(src, renderTexture, PseudoHDREncoding._material);
        return renderTexture;
    }
}
