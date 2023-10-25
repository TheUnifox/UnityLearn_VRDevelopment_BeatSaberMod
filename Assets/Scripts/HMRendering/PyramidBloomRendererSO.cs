// Decompiled with JetBrains decompiler
// Type: PyramidBloomRendererSO
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using UnityEngine;

public class PyramidBloomRendererSO : PersistentScriptableObject
{
  [SerializeField]
  protected Shader _shader;
  protected Material _material;
  protected PyramidBloomRendererSO.Level[] _pyramid;
  protected const int kMaxPyramidSize = 16;
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _bloomTexID = Shader.PropertyToID("_BloomTex");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _globalIntensityTex = Shader.PropertyToID("_GlobalIntensityTex");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _autoExposureLimitID = Shader.PropertyToID("_AutoExposureLimit");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _sampleScaleID = Shader.PropertyToID("_SampleScale");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _combineSrcID = Shader.PropertyToID("_CombineSrc");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _combineDstID = Shader.PropertyToID("_CombineDst");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _alphaWeightsID = Shader.PropertyToID("_AlphaWeights");
  protected bool _initialized;

  protected override void OnEnable()
  {
    base.OnEnable();
    if (this._initialized)
      return;
    this._initialized = true;
    this._material = new Material(this._shader);
    this._material.hideFlags = HideFlags.HideAndDontSave;
    this._pyramid = new PyramidBloomRendererSO.Level[16];
    for (int index = 0; index < 16; ++index)
    {
      this._pyramid[index] = new PyramidBloomRendererSO.Level();
      this._pyramid[index].down = (RenderTexture) null;
      this._pyramid[index].up = (RenderTexture) null;
    }
  }

  public virtual void OnDisable()
  {
    this._initialized = false;
    EssentialHelpers.SafeDestroy((Object) this._material);
    this._pyramid = (PyramidBloomRendererSO.Level[]) null;
  }

  public virtual void RenderBloom(
    RenderTexture src,
    RenderTexture dest,
    float radius,
    bool alphaWeights,
    bool betterQuality,
    bool gammaCorrection)
  {
    int num = betterQuality ? 0 : 1;
    PyramidBloomRendererSO.Pass preFilterPass = (PyramidBloomRendererSO.Pass) ((alphaWeights ? 0 : 2) + num);
    PyramidBloomRendererSO.Pass downsamplePass = (PyramidBloomRendererSO.Pass) (2 + num);
    PyramidBloomRendererSO.Pass upsamplePass = (PyramidBloomRendererSO.Pass) (5 + num);
    PyramidBloomRendererSO.Pass finalUpsamplePass = (PyramidBloomRendererSO.Pass) ((gammaCorrection ? 7 : 5) + num);
    this.RenderBloom(src, dest, radius, 1f, 1000f, 1f, true, true, 1f, 1f, 1f, 1f, preFilterPass, downsamplePass, upsamplePass, finalUpsamplePass);
  }

  public virtual void RenderBloom(
    RenderTexture src,
    RenderTexture dest,
    float radius,
    float intensity,
    float autoExposureLimit,
    float downIntensityOffset,
    bool uniformPyramidWeights,
    bool downsampleOnFirstPass,
    float pyramidWeightsParam,
    float alphaWeights,
    float firstUpsampleBrightness,
    float finalUpsampleBrightness,
    PyramidBloomRendererSO.Pass preFilterPass,
    PyramidBloomRendererSO.Pass downsamplePass,
    PyramidBloomRendererSO.Pass upsamplePass,
    PyramidBloomRendererSO.Pass finalUpsamplePass)
  {
    RenderTextureDescriptor descriptor = dest.descriptor with
    {
      depthBufferBits = 0,
      msaaSamples = 1,
      vrUsage = VRTextureUsage.None
    };
    if (downsampleOnFirstPass)
    {
      descriptor.width /= 2;
      descriptor.height /= 2;
    }
    else
    {
      descriptor.width = descriptor.width;
      descriptor.height = descriptor.height;
    }
    float f = (float) ((double) Mathf.Log((float) Mathf.Max(descriptor.width, descriptor.height), 2f) + (double) Mathf.Min(radius, 10f) - 10.0);
    int num1 = Mathf.FloorToInt(f);
    int num2 = Mathf.Clamp(num1, 1, 16);
    float num3 = 0.5f + f - (float) num1;
    this._material.SetFloat(PyramidBloomRendererSO._sampleScaleID, num3);
    this._material.SetFloat(PyramidBloomRendererSO._alphaWeightsID, alphaWeights);
    this._material.SetFloat(PyramidBloomRendererSO._combineDstID, 1f);
    this._material.SetFloat(PyramidBloomRendererSO._combineSrcID, intensity);
    RenderTexture source1 = src;
    for (int index = 0; index < num2; ++index)
    {
      PyramidBloomRendererSO.Pass pass = index == 0 ? preFilterPass : downsamplePass;
      this._pyramid[index].down = RenderTexture.GetTemporary(descriptor);
      if (index > 0)
        this._pyramid[index].up = RenderTexture.GetTemporary(descriptor);
      RenderTexture down = this._pyramid[index].down;
      Graphics.Blit((Texture) source1, down, this._material, (int) pass);
      source1 = down;
      descriptor.width = Mathf.Max(descriptor.width / 2, 1);
      descriptor.height = Mathf.Max(descriptor.height / 2, 1);
    }
    this._material.SetTexture(PyramidBloomRendererSO._globalIntensityTex, (Texture) source1);
    this._material.SetFloat(PyramidBloomRendererSO._autoExposureLimitID, autoExposureLimit);
    if (uniformPyramidWeights)
    {
      this._material.SetFloat(PyramidBloomRendererSO._combineDstID, 1f);
      this._material.SetFloat(PyramidBloomRendererSO._combineSrcID, intensity);
    }
    RenderTexture source2 = this._pyramid[num2 - 1].down;
    for (int index = num2 - 2; index >= 0; --index)
    {
      RenderTexture down = this._pyramid[index].down;
      RenderTexture dest1 = index == 0 ? dest : this._pyramid[index].up;
      if (!uniformPyramidWeights)
      {
        float num4 = Mathf.Min(1f, Mathf.Pow(intensity * (float) (index + 1) / (float) (num2 - 1), pyramidWeightsParam));
        float num5 = Mathf.Min(1f, 1f + downIntensityOffset - num4);
        float num6 = 1f;
        if (index == 0)
          num6 = finalUpsampleBrightness;
        else if (index == num2 - 2)
          num6 = firstUpsampleBrightness;
        this._material.SetFloat(PyramidBloomRendererSO._combineSrcID, num4 * num6);
        this._material.SetFloat(PyramidBloomRendererSO._combineDstID, num5 * num6);
      }
      this._material.SetTexture(PyramidBloomRendererSO._bloomTexID, (Texture) down);
      PyramidBloomRendererSO.Pass pass = index == 0 ? finalUpsamplePass : upsamplePass;
      Graphics.Blit((Texture) source2, dest1, this._material, (int) pass);
      source2 = dest1;
    }
    this._material.SetTexture(PyramidBloomRendererSO._bloomTexID, (Texture) null);
    this._material.SetTexture(PyramidBloomRendererSO._globalIntensityTex, (Texture) null);
    for (int index = 0; index < num2; ++index)
    {
      if ((Object) this._pyramid[index].down != (Object) null)
      {
        RenderTexture.ReleaseTemporary(this._pyramid[index].down);
        this._pyramid[index].down = (RenderTexture) null;
      }
      if ((Object) this._pyramid[index].up != (Object) null)
      {
        RenderTexture.ReleaseTemporary(this._pyramid[index].up);
        this._pyramid[index].up = (RenderTexture) null;
      }
    }
  }

  public enum Pass
  {
    Prefilter13,
    Prefilter4,
    Downsample13,
    Downsample4,
    DownsampleBilinearGamma,
    UpsampleTent,
    UpsampleBox,
    UpsampleTentGamma,
    UpsampleBoxGamma,
    Bilinear,
    BilinearGamma,
    UpsampleTentAndReinhardToneMapping,
    UpsampleTentAndACESToneMapping,
    UpsampleTentAndACESToneMappingGlobalIntensity,
  }

  public struct Level
  {
    internal RenderTexture down;
    internal RenderTexture up;
  }
}
