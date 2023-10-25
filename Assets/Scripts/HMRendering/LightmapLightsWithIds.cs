// Decompiled with JetBrains decompiler
// Type: LightmapLightsWithIds
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class LightmapLightsWithIds : LightWithIds
{
  [SerializeField]
  protected float _maxTotalIntensity = 1f;
  [SerializeField]
  protected LightmapLightsWithIds.LightIntensitiesWithId[] _lightIntensityData;

  public float maxTotalIntensity
  {
    get => this._maxTotalIntensity;
    set => this._maxTotalIntensity = value;
  }

  protected override void ProcessNewColorData()
  {
    float num1 = 0.0f;
    foreach (LightmapLightsWithIds.LightIntensitiesWithId intensitiesWithId in this._lightIntensityData)
      num1 += intensitiesWithId.color.grayscale * intensitiesWithId.weight;
    float num2 = 1f;
    if ((double) num1 > (double) this._maxTotalIntensity)
      num2 = this._maxTotalIntensity / num1;
    float gammaSpace = Mathf.LinearToGammaSpace(num2);
    foreach (LightmapLightsWithIds.LightIntensitiesWithId intensitiesWithId in this._lightIntensityData)
    {
      Color color1 = intensitiesWithId.color;
      float num3 = intensitiesWithId.intensity * gammaSpace * color1.a;
      color1.r *= num3;
      color1.g *= num3;
      color1.b *= num3;
      Color color2 = intensitiesWithId.color;
      float num4 = Mathf.LinearToGammaSpace(color2.a) * gammaSpace;
      color2.r *= num4;
      color2.g *= num4;
      color2.b *= num4;
      color2.a *= 2f * gammaSpace;
      intensitiesWithId.SetDataToShaders(color1.linear, color2.linear);
    }
  }

  protected override IEnumerable<LightWithIds.LightWithId> GetLightWithIds() => (IEnumerable<LightWithIds.LightWithId>) this._lightIntensityData;

  [Serializable]
  public class LightIntensitiesWithId : LightWithIds.LightWithId
  {
    [SerializeField]
    protected LightConstants.BakeId _bakeId;
    [SerializeField]
    protected float _intensity;
    [SerializeField]
    protected float _weight;
    protected int _lightmapLightIdColorPropertyId;
    protected int _lightProbeLightIdColorPropertyId;
    protected bool _initializedPropertyIds;

    public LightConstants.BakeId bakeId => this._bakeId;

    public float intensity
    {
      get => this._intensity;
      set => this._intensity = value;
    }

    public float weight
    {
      get => this._weight;
      set => this._weight = value;
    }

    public virtual void SetDataToShaders(Color lightmapColor, Color probeColor)
    {
      if (!this._initializedPropertyIds)
      {
        this._lightmapLightIdColorPropertyId = Shader.PropertyToID(string.Format("_LightmapLightBakeId{0}", (object) this._bakeId));
        this._lightProbeLightIdColorPropertyId = Shader.PropertyToID(string.Format("_LightProbeLightBakeId{0}", (object) this._bakeId));
        this._initializedPropertyIds = true;
      }
      Shader.SetGlobalColor(this._lightmapLightIdColorPropertyId, lightmapColor);
      Shader.SetGlobalColor(this._lightProbeLightIdColorPropertyId, probeColor);
    }
  }
}
