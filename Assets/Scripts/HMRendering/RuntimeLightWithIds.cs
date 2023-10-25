// Decompiled with JetBrains decompiler
// Type: RuntimeLightWithIds
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class RuntimeLightWithIds : LightWithIds
{
  [SerializeField]
  private RuntimeLightWithIds.LightIntensitiesWithId[] _lightIntensityData;
  [Space]
  [SerializeField]
  private float _intensity = 1f;
  [SerializeField]
  private float _maxIntensity = 1f;
  [SerializeField]
  private bool _multiplyColorByAlpha = true;
  [SerializeField]
  private RuntimeLightWithIds.MixType _mixType;

  protected abstract void ColorWasSet(Color color);

  protected override void ProcessNewColorData()
  {
    Color color1 = new Color();
    foreach (RuntimeLightWithIds.LightIntensitiesWithId intensitiesWithId in this._lightIntensityData)
    {
      Color color2 = this.ProcessColor(intensitiesWithId.color, intensitiesWithId.intensity);
      switch (this._mixType)
      {
        case RuntimeLightWithIds.MixType.Maximum:
          if ((double) color1.r < (double) color2.r)
            color1.r = color2.r;
          if ((double) color1.g < (double) color2.g)
            color1.g = color2.g;
          if ((double) color1.b < (double) color2.b)
            color1.b = color2.b;
          if ((double) color1.a < (double) color2.a)
          {
            color1.a = color2.a;
            break;
          }
          break;
        case RuntimeLightWithIds.MixType.Sum:
          color1.r += color2.r;
          color1.g += color2.g;
          color1.b += color2.b;
          break;
      }
    }
    if (this._multiplyColorByAlpha)
    {
      color1 *= this._intensity;
      float grayscale = color1.grayscale;
      if ((double) grayscale > (double) this._maxIntensity)
        color1 /= grayscale / this._maxIntensity;
    }
    else
    {
      color1.a *= this._intensity;
      color1.a = Mathf.Min(this._maxIntensity, color1.a);
    }
    this.ColorWasSet(color1);
  }

  protected override IEnumerable<LightWithIds.LightWithId> GetLightWithIds() => (IEnumerable<LightWithIds.LightWithId>) this._lightIntensityData;

  private Color ProcessColor(Color color, float intensity)
  {
    switch (this._mixType)
    {
      case RuntimeLightWithIds.MixType.Maximum:
        color.a *= intensity;
        color.a = Mathf.Sqrt(color.a);
        break;
      case RuntimeLightWithIds.MixType.Sum:
        color.a *= intensity;
        break;
    }
    if (this._multiplyColorByAlpha)
    {
      color.r *= color.a;
      color.g *= color.a;
      color.b *= color.a;
    }
    return color;
  }

  private enum MixType
  {
    Maximum,
    Sum,
  }

  [Serializable]
  public class LightIntensitiesWithId : LightWithIds.LightWithId
  {
    [SerializeField]
    private float _intensity;

    public float intensity
    {
      get => this._intensity;
      set => this._intensity = value;
    }

    public LightIntensitiesWithId(int lightId, float lightIntensity)
      : base(lightId)
    {
      this._intensity = lightIntensity;
    }
  }
}
