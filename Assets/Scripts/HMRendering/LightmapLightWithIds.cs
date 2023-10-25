// Decompiled with JetBrains decompiler
// Type: LightmapLightWithIds
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class LightmapLightWithIds : LightWithIds
{
  [SerializeField]
  protected LightConstants.BakeId _bakeId;
  [SerializeField]
  protected float _intensity = 1f;
  [SerializeField]
  protected float _probeIntensity = 1f;
  [SerializeField]
  protected LightmapLightWithIds.LightIntensitiesWithId[] _lightIntensityData;
  [SerializeField]
  protected LightmapLightWithIds.MixType _mixType;
  [SerializeField]
  protected float _normalizerWeight = 1f;
  protected BakedLightsNormalizer _bakedLightsNormalizer;
  protected int _lightmapLightIdColorPropertyId;
  protected int _lightProbeLightIdColorPropertyId;
  protected bool _initializedPropertyIds;
  protected bool _initializedNormalizer;
  protected bool _isNormalizerInScene;
  protected Color _calculatedColorPreNormalization;

  public float intensity
  {
    get => this._intensity;
    set => this._intensity = value;
  }

  public float normalizerWeight
  {
    get => this._normalizerWeight;
    set => this._normalizerWeight = value;
  }

  public Color calculatedColorPreNormalization => this._calculatedColorPreNormalization;

  public LightConstants.BakeId bakeId => this._bakeId;

  protected override void Awake()
  {
    base.Awake();
    this.GetBakedLightsNormalizer();
    this.SetShaderProperties();
    this.SetDataToShaders(Color.clear, Color.clear);
  }

  protected override void ProcessNewColorData()
  {
    if (!this._initializedPropertyIds)
      this.SetShaderProperties();
    if (!this._initializedNormalizer)
      this.GetBakedLightsNormalizer();
    Color color1 = new Color();
    Color color2 = new Color();
    float num1 = this._isNormalizerInScene ? this._bakedLightsNormalizer.GetNormalizationMultiplier() : 1f;
    foreach (LightmapLightWithIds.LightIntensitiesWithId intensitiesWithId in this._lightIntensityData)
    {
      float intensity = intensitiesWithId.intensity;
      Color color3 = intensitiesWithId.color;
      Color color4 = color3;
      float num2 = intensity * color3.a;
      color3.r *= num2;
      color3.g *= num2;
      color3.b *= num2;
      float num3 = Mathf.LinearToGammaSpace(color4.a) * intensity;
      color4.r *= num3;
      color4.g *= num3;
      color4.b *= num3;
      color4.a *= 2f * intensity * intensitiesWithId.probeHighlightsIntensityMultiplier;
      switch (this._mixType)
      {
        case LightmapLightWithIds.MixType.Maximum:
          if ((double) color1.r < (double) color3.r)
            color1.r = color3.r;
          if ((double) color1.g < (double) color3.g)
            color1.g = color3.g;
          if ((double) color1.b < (double) color3.b)
            color1.b = color3.b;
          if ((double) color2.r < (double) color4.r)
            color2.r = color4.r;
          if ((double) color2.g < (double) color4.g)
            color2.g = color4.g;
          if ((double) color2.b < (double) color4.b)
            color2.b = color4.b;
          if ((double) color2.a < (double) color4.a)
          {
            color2.a = color4.a;
            break;
          }
          break;
        case LightmapLightWithIds.MixType.Sum:
          color1.r += color3.r;
          color1.g += color3.g;
          color1.b += color3.b;
          color1.a += color3.a;
          color2.r += color4.r;
          color2.g += color4.g;
          color2.b += color4.b;
          color2.a += color4.a;
          break;
      }
    }
    color1 *= this._intensity;
    color2 *= this._probeIntensity;
    this._calculatedColorPreNormalization = color2.linear;
    this.SetDataToShaders(color1.linear, num1 * color2.linear);
  }

  protected override IEnumerable<LightWithIds.LightWithId> GetLightWithIds() => (IEnumerable<LightWithIds.LightWithId>) this._lightIntensityData;

  public virtual void SetDataToShaders(Color lightmapColor, Color probeColor)
  {
    Shader.SetGlobalColor(this._lightmapLightIdColorPropertyId, lightmapColor);
    Shader.SetGlobalColor(this._lightProbeLightIdColorPropertyId, probeColor);
  }

  public virtual void SetShaderProperties()
  {
    this._lightmapLightIdColorPropertyId = Shader.PropertyToID(string.Format("_LightmapLightBakeId{0}", (object) this._bakeId));
    this._lightProbeLightIdColorPropertyId = Shader.PropertyToID(string.Format("_LightProbeLightBakeId{0}", (object) this._bakeId));
    this._initializedPropertyIds = true;
  }

  public virtual void GetBakedLightsNormalizer()
  {
    this._bakedLightsNormalizer = UnityEngine.Object.FindObjectOfType<BakedLightsNormalizer>();
    this._isNormalizerInScene = (UnityEngine.Object) this._bakedLightsNormalizer != (UnityEngine.Object) null;
    this._initializedNormalizer = true;
  }

  public enum MixType
  {
    Maximum,
    Sum,
  }

  [Serializable]
  public class LightIntensitiesWithId : LightWithIds.LightWithId
  {
    [SerializeField]
    protected float _intensity;
    [SerializeField]
    protected float _probeHighlightsIntensityMultiplier = 1f;

    public float intensity => this._intensity;

    public float probeHighlightsIntensityMultiplier => this._probeHighlightsIntensityMultiplier;

    public LightIntensitiesWithId(int lightId, float lightIntensity, float probeMultiplier)
      : base(lightId)
    {
      this._intensity = lightIntensity;
      this._probeHighlightsIntensityMultiplier = probeMultiplier;
    }
  }
}
