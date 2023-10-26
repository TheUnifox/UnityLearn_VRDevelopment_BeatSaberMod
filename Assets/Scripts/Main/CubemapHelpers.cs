// Decompiled with JetBrains decompiler
// Type: CubemapHelpers
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class CubemapHelpers
{
  private const string kCubemapHelpersShaderName = "Hidden/CubemapHelpers";
  [DoesNotRequireDomainReloadInit]
  private static Material _cubemapHelpersMaterial;
  [DoesNotRequireDomainReloadInit]
  private static readonly int _cubePropertyId = Shader.PropertyToID("_Cube");
  [DoesNotRequireDomainReloadInit]
  private static readonly int _cubeFaceNumberId = Shader.PropertyToID("_CubeFaceNumber");
  private const int kCubemapDownsamplePass = 0;
  private const int kCubemapTo2DTexturePass = 1;
  [DoesNotRequireDomainReloadInit]
  private static readonly Dictionary<CubemapFace, int> _cubemapFaceToCubeFaceNumberDict = new Dictionary<CubemapFace, int>()
  {
    {
      CubemapFace.PositiveX,
      0
    },
    {
      CubemapFace.NegativeX,
      1
    },
    {
      CubemapFace.PositiveY,
      2
    },
    {
      CubemapFace.NegativeY,
      3
    },
    {
      CubemapFace.PositiveZ,
      4
    },
    {
      CubemapFace.NegativeZ,
      5
    },
    {
      CubemapFace.Unknown,
      0
    }
  };
  [DoesNotRequireDomainReloadInit]
  private static readonly CubemapFace[] _cubemapFaces = new CubemapFace[6]
  {
    CubemapFace.PositiveX,
    CubemapFace.PositiveY,
    CubemapFace.PositiveZ,
    CubemapFace.NegativeX,
    CubemapFace.NegativeY,
    CubemapFace.NegativeZ
  };

  private static Material cubemapHelpersMaterial
  {
    get
    {
      if ((Object) CubemapHelpers._cubemapHelpersMaterial == (Object) null)
        CubemapHelpers._cubemapHelpersMaterial = new Material(Shader.Find("Hidden/CubemapHelpers"));
      return CubemapHelpers._cubemapHelpersMaterial;
    }
  }

  private static void Downsample(Texture src, RenderTexture dest)
  {
    foreach (CubemapFace cubemapFace in CubemapHelpers._cubemapFaces)
    {
      Graphics.SetRenderTarget(dest, 0, cubemapFace);
      GL.Clear(true, true, Color.clear);
      CubemapHelpers.DrawCubemapFace(src, cubemapFace);
    }
  }

    public static RenderTexture CreateDownsampledCubemap(RenderTexture src, int count)
    {
        RenderTexture renderTexture = src;
        RenderTexture renderTexture2 = null;
        int num = src.height;
        RenderTextureDescriptor descriptor = src.descriptor;
        descriptor.msaaSamples = 1;
        for (int i = 0; i < count; i++)
        {
            num /= 2;
            descriptor.width = (descriptor.height = num);
            renderTexture2 = ((i == count - 1) ? new RenderTexture(descriptor) : RenderTexture.GetTemporary(descriptor));
            CubemapHelpers.Downsample(renderTexture, renderTexture2);
            RenderTexture renderTexture3 = renderTexture;
            renderTexture = renderTexture2;
            if (renderTexture3 != src)
            {
                RenderTexture.ReleaseTemporary(renderTexture3);
            }
        }
        return renderTexture2;
    }

    public static RenderTexture Create2DTextureFromCubemap(RenderTexture src)
    {
        RenderTextureDescriptor descriptor = src.descriptor;
        descriptor.dimension = TextureDimension.Tex2D;
        descriptor.width *= 6;
        descriptor.msaaSamples = 1;
        RenderTexture renderTexture = new RenderTexture(descriptor);
        CubemapHelpers.cubemapHelpersMaterial.SetTexture(CubemapHelpers._cubePropertyId, src);
        Graphics.Blit(null, renderTexture, CubemapHelpers.cubemapHelpersMaterial, 1);
        return renderTexture;
    }

    private static void DrawCubemapFace(Texture cubemap, CubemapFace cubemapFace)
  {
    CubemapHelpers.cubemapHelpersMaterial.SetTexture(CubemapHelpers._cubePropertyId, cubemap);
    CubemapHelpers.cubemapHelpersMaterial.SetInt(CubemapHelpers._cubeFaceNumberId, CubemapHelpers._cubemapFaceToCubeFaceNumberDict[cubemapFace]);
    CubemapHelpers.cubemapHelpersMaterial.SetPass(0);
    GL.PushMatrix();
    GL.LoadOrtho();
    GL.Begin(7);
    GL.TexCoord2(0.0f, 0.0f);
    GL.Vertex3(0.0f, 0.0f, 0.0f);
    GL.TexCoord2(1f, 0.0f);
    GL.Vertex3(1f, 0.0f, 0.0f);
    GL.TexCoord2(1f, 1f);
    GL.Vertex3(1f, 1f, 0.0f);
    GL.TexCoord2(0.0f, 1f);
    GL.Vertex3(0.0f, 1f, 0.0f);
    GL.End();
    GL.PopMatrix();
  }
}
