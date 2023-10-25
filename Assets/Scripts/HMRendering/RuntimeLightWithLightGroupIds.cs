// Decompiled with JetBrains decompiler
// Type: RuntimeLightWithLightGroupIds
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class RuntimeLightWithLightGroupIds : LightWithIds
{
  [SerializeField]
  private LightGroup[] _lightGroupList;
  [Space]
  [SerializeField]
  private float _intensity = 1f;
  [SerializeField]
  private float _maxIntensity = 1f;
  [SerializeField]
  private bool _multiplyColorByAlpha = true;
  private RuntimeLightWithLightGroupIds.LightIntensitiesWithId[] _lightIntensityData;

  protected abstract void ColorWasSet(Color color);

  protected override void Awake()
  {
    int length = 0;
    foreach (LightGroup lightGroup in this._lightGroupList)
      length += lightGroup.lightGroupSO.numberOfElements;
    this._lightIntensityData = new RuntimeLightWithLightGroupIds.LightIntensitiesWithId[length];
    int index1 = 0;
    foreach (LightGroup lightGroup in this._lightGroupList)
    {
      for (int index2 = 0; index2 < lightGroup.lightGroupSO.numberOfElements; ++index2)
      {
        this._lightIntensityData[index1] = new RuntimeLightWithLightGroupIds.LightIntensitiesWithId(index2 + lightGroup.lightGroupSO.startLightId, 1f);
        ++index1;
      }
    }
    base.Awake();
  }

  protected override void ProcessNewColorData()
  {
    Color color1 = new Color();
    for (int index = 0; index < this._lightIntensityData.Length; ++index)
    {
      RuntimeLightWithLightGroupIds.LightIntensitiesWithId intensitiesWithId = this._lightIntensityData[index];
      Color color2 = this.ProcessColor(intensitiesWithId.color, intensitiesWithId.intensity);
      if ((double) color1.r < (double) color2.r)
        color1.r = color2.r;
      if ((double) color1.g < (double) color2.g)
        color1.g = color2.g;
      if ((double) color1.b < (double) color2.b)
        color1.b = color2.b;
      if ((double) color1.a < (double) color2.a)
        color1.a = color2.a;
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
    color.a *= intensity;
    if (this._multiplyColorByAlpha)
    {
      color.a = Mathf.Sqrt(color.a);
      color.r *= color.a;
      color.g *= color.a;
      color.b *= color.a;
    }
    return color;
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

    public LightIntensitiesWithId(int lightId, float intensity)
      : base(lightId)
    {
      this._intensity = intensity;
    }
  }
}
