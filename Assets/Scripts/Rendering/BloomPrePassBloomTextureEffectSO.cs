// Decompiled with JetBrains decompiler
// Type: BloomPrePassBloomTextureEffectSO
// Assembly: Rendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B7D31E1-1F9E-4BE6-B735-D2EB47EFDA46
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Rendering.dll

using UnityEngine;

public class BloomPrePassBloomTextureEffectSO : BloomPrePassEffectSO
{
  [Space]
  [SerializeField]
  private float _radius = 128f;
  [SerializeField]
  private float _intensity = 1f;
  [SerializeField]
  private float _downBloomIntensityOffset = 1f;
  [SerializeField]
  private bool _uniformPyramidWeights = true;
  [SerializeField]
  private float _pyramidWeightsParam = 1f;
  [SerializeField]
  private float _firstUpsampleBrightness = 1f;
  [SerializeField]
  private float _finalUpsampleBrightness = 1f;
  [SerializeField]
  private PyramidBloomRendererSO.Pass _prefilterPass = PyramidBloomRendererSO.Pass.Downsample4;
  [SerializeField]
  private PyramidBloomRendererSO.Pass _downsamplePass = PyramidBloomRendererSO.Pass.Downsample4;
  [SerializeField]
  private PyramidBloomRendererSO.Pass _upsamplePass = PyramidBloomRendererSO.Pass.UpsampleBox;
  [SerializeField]
  private PyramidBloomRendererSO.Pass _finalUpsamplePass = PyramidBloomRendererSO.Pass.UpsampleBoxGamma;
  [Space]
  [SerializeField]
  private PyramidBloomRendererSO _bloomRenderer;
  [SerializeField]
  private BloomFogSO _bloomFog;

  public override ToneMapping toneMapping
  {
    get
    {
      switch (this._finalUpsamplePass)
      {
        case PyramidBloomRendererSO.Pass.UpsampleTentAndACESToneMapping:
          return ToneMapping.Aces;
        case PyramidBloomRendererSO.Pass.UpsampleTentAndACESToneMappingGlobalIntensity:
          return ToneMapping.Aces;
        default:
          return ToneMapping.None;
      }
    }
  }

  public override void Render(RenderTexture src, RenderTexture dest) => this._bloomRenderer.RenderBloom(src, dest, this._radius, this._intensity, this._bloomFog.autoExposureLimit, this._downBloomIntensityOffset, this._uniformPyramidWeights, true, this._pyramidWeightsParam, 1f, this._firstUpsampleBrightness, this._finalUpsampleBrightness, this._prefilterPass, this._downsamplePass, this._upsamplePass, this._finalUpsamplePass);
}
