// Decompiled with JetBrains decompiler
// Type: MenuLightsPresetSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;

public class MenuLightsPresetSO : PersistentScriptableObject
{
  [SerializeField]
  protected ColorSO _playersPlaceNeonsColor;
  [SerializeField]
  [Range(0.0f, 1f)]
  protected float _playersPlaceNeonsIntensity = 1f;
  [SerializeField]
  protected MenuLightsPresetSO.LightIdColorPair[] _lightIdColorPairs;

  public ColorSO playersPlaceNeonsColor => this._playersPlaceNeonsColor;

  public float playersPlaceNeonsIntensity => this._playersPlaceNeonsIntensity;

  public MenuLightsPresetSO.LightIdColorPair[] lightIdColorPairs => this._lightIdColorPairs;

  [Serializable]
  public class LightIdColorPair
  {
    public int lightId;
    public ColorSO baseColor;
    [Range(0.0f, 1f)]
    public float intensity;

    public Color lightColor => this.baseColor.color.ColorWithAlpha(this.intensity);
  }
}
