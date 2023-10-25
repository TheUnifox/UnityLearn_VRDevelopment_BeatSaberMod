// Decompiled with JetBrains decompiler
// Type: KawaseBlurRendererSO
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using System;
using UnityEngine;
using UnityEngine.Rendering;

public class KawaseBlurRendererSO : PersistentScriptableObject
{
  [SerializeField]
  protected Shader _kawaseBlurShader;
  [SerializeField]
  protected Shader _additiveShader;
  [SerializeField]
  protected Shader _tintShader;
  protected Material _kawaseBlurMaterial;
  protected Material _additiveMaterial;
  protected Material _tintMaterial;
  protected Material _commandBuffersMaterial;
  protected int[][] _kernels;
  protected KawaseBlurRendererSO.BloomKernel[] _bloomKernels;
  protected RenderTexture[] _blurTextures;
  protected const int kMaxBloomIterations = 5;
  [DoesNotRequireDomainReloadInit]
  protected static readonly float[][] kBloomIterationWeights = new float[5][]
  {
    new float[1]{ 1f },
    new float[2]{ 0.3875f, 0.61125f },
    new float[3]{ 0.5f, 0.3f, 0.3f },
    new float[4]{ 0.5f, 0.3f, 0.3f, 0.5f },
    new float[5]{ 0.5f, 0.3f, 0.3f, 0.3f, 0.5f }
  };
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _offsetID = Shader.PropertyToID("_Offset");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _boostID = Shader.PropertyToID("_Boost");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _additiveAlphaID = Shader.PropertyToID("_AdditiveAlpha");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _alphaID = Shader.PropertyToID("_Alpha");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _tintColorID = Shader.PropertyToID("_TintColor");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _alphaWeightsID = Shader.PropertyToID("_AlphaWeights");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _tempTexture0ID = Shader.PropertyToID("_TempTexture0");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _tempTexture1ID = Shader.PropertyToID("_TempTexture1");

  public virtual int[] GetBlurKernel(KawaseBlurRendererSO.KernelSize kernelSize)
  {
    int index = (int) kernelSize;
    if (this._kernels[index] != null)
      return this._kernels[index];
    int[] blurKernel;
    switch (kernelSize)
    {
      case KawaseBlurRendererSO.KernelSize.Kernel7:
        blurKernel = new int[2];
        break;
      case KawaseBlurRendererSO.KernelSize.Kernel15:
        blurKernel = new int[3]{ 0, 1, 1 };
        break;
      case KawaseBlurRendererSO.KernelSize.Kernel23:
        blurKernel = new int[4]{ 0, 1, 1, 2 };
        break;
      case KawaseBlurRendererSO.KernelSize.Kernel35:
        blurKernel = new int[5]{ 0, 1, 2, 2, 3 };
        break;
      case KawaseBlurRendererSO.KernelSize.Kernel63:
        blurKernel = new int[7]{ 0, 1, 2, 3, 4, 4, 5 };
        break;
      case KawaseBlurRendererSO.KernelSize.Kernel127:
        blurKernel = new int[10]
        {
          0,
          1,
          2,
          3,
          4,
          5,
          7,
          8,
          9,
          10
        };
        break;
      case KawaseBlurRendererSO.KernelSize.Kernel135:
        blurKernel = new int[11]
        {
          0,
          1,
          2,
          3,
          4,
          5,
          7,
          8,
          9,
          10,
          11
        };
        break;
      case KawaseBlurRendererSO.KernelSize.Kernel143:
        blurKernel = new int[12]
        {
          0,
          1,
          2,
          3,
          4,
          5,
          7,
          8,
          9,
          10,
          11,
          12
        };
        break;
      default:
        blurKernel = new int[2];
        break;
    }
    this._kernels[index] = blurKernel;
    return blurKernel;
  }

  protected override void OnEnable()
  {
    base.OnEnable();
    this._kernels = new int[Enum.GetNames(typeof (KawaseBlurRendererSO.KernelSize)).Length][];
    this._bloomKernels = new KawaseBlurRendererSO.BloomKernel[5]
    {
      new KawaseBlurRendererSO.BloomKernel()
      {
        kernelSize = KawaseBlurRendererSO.KernelSize.Kernel7,
        sharedPartWithNext = 1
      },
      new KawaseBlurRendererSO.BloomKernel()
      {
        kernelSize = KawaseBlurRendererSO.KernelSize.Kernel15,
        sharedPartWithNext = 2
      },
      new KawaseBlurRendererSO.BloomKernel()
      {
        kernelSize = KawaseBlurRendererSO.KernelSize.Kernel35,
        sharedPartWithNext = 3
      },
      new KawaseBlurRendererSO.BloomKernel()
      {
        kernelSize = KawaseBlurRendererSO.KernelSize.Kernel63,
        sharedPartWithNext = 5
      },
      new KawaseBlurRendererSO.BloomKernel()
      {
        kernelSize = KawaseBlurRendererSO.KernelSize.Kernel127,
        sharedPartWithNext = 9
      }
    };
    this._blurTextures = new RenderTexture[5];
    this._kawaseBlurMaterial = new Material(this._kawaseBlurShader);
    this._kawaseBlurMaterial.hideFlags = HideFlags.HideAndDontSave;
    this._commandBuffersMaterial = new Material(this._kawaseBlurShader);
    this._commandBuffersMaterial.hideFlags = HideFlags.HideAndDontSave;
    this._additiveMaterial = new Material(this._additiveShader);
    this._additiveMaterial.hideFlags = HideFlags.HideAndDontSave;
    this._tintMaterial = new Material(this._tintShader);
    this._tintMaterial.hideFlags = HideFlags.HideAndDontSave;
  }

  public virtual void OnDisable()
  {
    EssentialHelpers.SafeDestroy((UnityEngine.Object) this._kawaseBlurMaterial);
    EssentialHelpers.SafeDestroy((UnityEngine.Object) this._commandBuffersMaterial);
    EssentialHelpers.SafeDestroy((UnityEngine.Object) this._additiveMaterial);
    EssentialHelpers.SafeDestroy((UnityEngine.Object) this._tintMaterial);
  }

  public virtual void Bloom(
    RenderTexture src,
    RenderTexture dest,
    int iterationsStart,
    int iterations,
    float boost,
    float alphaWeights,
    KawaseBlurRendererSO.WeightsType blurStartWeightsType,
    float[] bloomIterationWeights)
  {
    iterations = Mathf.Clamp(iterations, 1, Mathf.Min(this._bloomKernels.Length, 5));
    RenderTextureDescriptor descriptor = dest.descriptor with
    {
      depthBufferBits = 0,
      msaaSamples = 1
    };
    RenderTexture renderTexture = src;
    int startIdx = 0;
    for (int index = iterationsStart; index < iterationsStart + iterations; ++index)
    {
      KawaseBlurRendererSO.BloomKernel bloomKernel = this._bloomKernels[index];
      int[] blurKernel = this.GetBlurKernel(bloomKernel.kernelSize);
      RenderTexture temporary = RenderTexture.GetTemporary(descriptor);
      this.Blur((Texture) renderTexture, temporary, blurKernel, boost, 0, startIdx, bloomKernel.sharedPartWithNext - startIdx, alphaWeights, 1f, false, false, index == iterationsStart ? blurStartWeightsType : KawaseBlurRendererSO.WeightsType.None);
      startIdx = bloomKernel.sharedPartWithNext;
      this._blurTextures[index - iterationsStart] = RenderTexture.GetTemporary(descriptor);
      this.Blur((Texture) temporary, this._blurTextures[index - iterationsStart], blurKernel, boost, 0, bloomKernel.sharedPartWithNext, blurKernel.Length - bloomKernel.sharedPartWithNext, 0.0f, 1f, false, false, KawaseBlurRendererSO.WeightsType.None);
      if (index > iterationsStart)
        RenderTexture.ReleaseTemporary(renderTexture);
      renderTexture = temporary;
    }
    RenderTexture.ReleaseTemporary(renderTexture);
    if (bloomIterationWeights == null)
      bloomIterationWeights = KawaseBlurRendererSO.kBloomIterationWeights[iterations - 1];
    for (int index = iterationsStart; index < iterationsStart + iterations; ++index)
    {
      float bloomIterationWeight = bloomIterationWeights[index - iterationsStart];
      if (index == iterationsStart)
      {
        this._tintMaterial.SetColor(KawaseBlurRendererSO._tintColorID, new Color(bloomIterationWeight, bloomIterationWeight, bloomIterationWeight, bloomIterationWeight));
        Graphics.Blit((Texture) this._blurTextures[index - iterationsStart], dest, this._tintMaterial);
      }
      else
      {
        this._additiveMaterial.SetFloat(KawaseBlurRendererSO._alphaID, bloomIterationWeight);
        Graphics.Blit((Texture) this._blurTextures[index - iterationsStart], dest, this._additiveMaterial);
      }
      RenderTexture.ReleaseTemporary(this._blurTextures[index - iterationsStart]);
    }
  }

  public virtual void DoubleBlur(
    RenderTexture src,
    RenderTexture dest,
    KawaseBlurRendererSO.KernelSize kernelSize0,
    float boost0,
    KawaseBlurRendererSO.KernelSize kernelSize1,
    float boost1,
    float secondBlurAlpha,
    int downsample,
    bool gammaCorrection)
  {
    int[] blurKernel1 = this.GetBlurKernel(kernelSize0);
    int[] blurKernel2 = this.GetBlurKernel(kernelSize1);
    int index = 0;
    while (index < blurKernel1.Length && index < blurKernel2.Length && blurKernel1[index] == blurKernel2[index])
      ++index;
    int num1 = src.width >> downsample;
    int num2 = src.height >> downsample;
    RenderTexture temporary = RenderTexture.GetTemporary(src.descriptor with
    {
      depthBufferBits = 0,
      msaaSamples = 1,
      width = num1,
      height = num2
    });
    this.Blur((Texture) src, temporary, blurKernel1, 0.0f, downsample, 0, index, 0.0f, 1f, false, false, KawaseBlurRendererSO.WeightsType.None);
    this.Blur((Texture) temporary, dest, blurKernel1, boost0, 0, index, blurKernel1.Length - index, 0.0f, 1f, false, gammaCorrection, KawaseBlurRendererSO.WeightsType.None);
    this.Blur((Texture) temporary, dest, blurKernel2, boost1, 0, index, blurKernel2.Length - index, 0.0f, secondBlurAlpha, true, gammaCorrection, KawaseBlurRendererSO.WeightsType.None);
    RenderTexture.ReleaseTemporary(temporary);
  }

  public virtual Texture2D Blur(
    Texture src,
    KawaseBlurRendererSO.KernelSize kernelSize,
    int downsample = 0)
  {
    RenderTexture temporary = RenderTexture.GetTemporary(src.width >> downsample, src.height >> downsample);
    temporary.wrapMode = TextureWrapMode.Clamp;
    this.Blur(src, temporary, kernelSize, 0.0f, downsample);
    Texture2D texture2D = temporary.GetTexture2D();
    RenderTexture.ReleaseTemporary(temporary);
    return texture2D;
  }

  public virtual void Blur(
    Texture src,
    RenderTexture dest,
    KawaseBlurRendererSO.KernelSize kernelSize,
    float boost,
    int downsample)
  {
    int[] blurKernel = this.GetBlurKernel(kernelSize);
    this.Blur(src, dest, blurKernel, boost, downsample, 0, blurKernel.Length, 0.0f, 1f, false, false, KawaseBlurRendererSO.WeightsType.None);
  }

  public virtual void Blur(
    Texture src,
    RenderTexture dest,
    int[] kernel,
    float boost,
    int downsample,
    int startIdx,
    int length,
    float alphaWeights,
    float additiveAlpha,
    bool additivelyBlendToDest,
    bool gammaCorrection,
    KawaseBlurRendererSO.WeightsType blurStartWeightsType)
  {
    if (length == 0)
    {
      Graphics.Blit(src, dest);
    }
    else
    {
      int num1 = src.width >> downsample;
      int num2 = src.height >> downsample;
      if (downsample == 0)
      {
        num1 = dest.width;
        num2 = dest.height;
      }
      RenderTextureDescriptor descriptor = dest.descriptor with
      {
        depthBufferBits = 0,
        width = num1,
        height = num2,
        msaaSamples = 1
      };
      this._kawaseBlurMaterial.SetFloat(KawaseBlurRendererSO._alphaWeightsID, alphaWeights);
      Texture texture;
      if (blurStartWeightsType == KawaseBlurRendererSO.WeightsType.AlphaAndDepthWeights)
      {
        texture = (Texture) RenderTexture.GetTemporary(descriptor);
        Graphics.Blit(src, (RenderTexture) texture, this._kawaseBlurMaterial, 4);
      }
      else
        texture = src;
      int num3 = startIdx + length;
      for (int index = startIdx; index < num3; ++index)
      {
        int pass = index != 0 || blurStartWeightsType != KawaseBlurRendererSO.WeightsType.AlphaWeights ? 1 : 3;
        RenderTexture dest1;
        if (index == num3 - 1)
        {
          dest1 = dest;
          if (additivelyBlendToDest)
            pass = gammaCorrection ? 6 : 2;
          else if (gammaCorrection)
            pass = 5;
        }
        else
          dest1 = RenderTexture.GetTemporary(descriptor);
        float num4 = (float) kernel[index] + 0.5f;
        this._kawaseBlurMaterial.SetVector(KawaseBlurRendererSO._offsetID, new Vector4(num4, num4, -num4, num4));
        this._kawaseBlurMaterial.SetFloat(KawaseBlurRendererSO._boostID, boost);
        this._kawaseBlurMaterial.SetFloat(KawaseBlurRendererSO._additiveAlphaID, additiveAlpha);
        Graphics.Blit(texture, dest1, this._kawaseBlurMaterial, pass);
        if ((UnityEngine.Object) texture != (UnityEngine.Object) src)
          RenderTexture.ReleaseTemporary((RenderTexture) texture);
        texture = (Texture) dest1;
      }
    }
  }

  public virtual void AlphaWeights(RenderTexture src, RenderTexture dest)
  {
    float num = 0.5f;
    this._kawaseBlurMaterial.SetVector(KawaseBlurRendererSO._offsetID, new Vector4(num, num, -num, num));
    Graphics.Blit((Texture) src, dest, this._kawaseBlurMaterial, 0);
  }

  public virtual CommandBuffer CreateBlurCommandBuffer(
    int width,
    int height,
    string globalTextureName,
    KawaseBlurRendererSO.KernelSize kernelSize,
    float boost)
  {
    int[] blurKernel = this.GetBlurKernel(kernelSize);
    CommandBuffer blurCommandBuffer = new CommandBuffer();
    int num1 = KawaseBlurRendererSO._tempTexture0ID;
    int num2 = KawaseBlurRendererSO._tempTexture1ID;
    blurCommandBuffer.GetTemporaryRT(num1, width, height, 0, FilterMode.Bilinear, RenderTextureFormat.RGB111110Float);
    blurCommandBuffer.GetTemporaryRT(num2, width, height, 0, FilterMode.Bilinear, RenderTextureFormat.RGB111110Float);
    blurCommandBuffer.Blit((RenderTargetIdentifier) BuiltinRenderTextureType.CurrentActive, (RenderTargetIdentifier) num1);
    for (int index = 0; index < blurKernel.Length; ++index)
    {
      float num3 = (float) blurKernel[index] + 0.5f;
      blurCommandBuffer.SetGlobalVector(KawaseBlurRendererSO._offsetID, new Vector4(num3, num3, -num3, num3));
      blurCommandBuffer.SetGlobalFloat(KawaseBlurRendererSO._boostID, boost);
      blurCommandBuffer.Blit((RenderTargetIdentifier) num1, (RenderTargetIdentifier) num2, this._commandBuffersMaterial, 1);
      int num4 = num2;
      num2 = num1;
      num1 = num4;
    }
    blurCommandBuffer.SetGlobalTexture(globalTextureName, (RenderTargetIdentifier) num1);
    blurCommandBuffer.ReleaseTemporaryRT(num1);
    blurCommandBuffer.ReleaseTemporaryRT(num2);
    return blurCommandBuffer;
  }

  public enum KernelSize
  {
    Kernel7,
    Kernel15,
    Kernel23,
    Kernel35,
    Kernel63,
    Kernel127,
    Kernel135,
    Kernel143,
  }

  public enum WeightsType
  {
    None,
    AlphaWeights,
    AlphaAndDepthWeights,
  }

  public class BloomKernel
  {
    public KawaseBlurRendererSO.KernelSize kernelSize;
    public int sharedPartWithNext;
  }

  public enum Pass
  {
    AlphaWeights,
    Blur,
    BlurAndAdd,
    BlurWithAlphaWeights,
    AlphaAndDepthWeights,
    BlurGamma,
    BlurGammaAndAdd,
  }
}
