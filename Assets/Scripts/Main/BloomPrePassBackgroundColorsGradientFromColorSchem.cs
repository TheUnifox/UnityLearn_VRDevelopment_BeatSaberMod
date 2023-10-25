// Decompiled with JetBrains decompiler
// Type: BloomPrePassBackgroundColorsGradientFromColorSchemeColors
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;
using Zenject;

public class BloomPrePassBackgroundColorsGradientFromColorSchemeColors : MonoBehaviour
{
  [SerializeField]
  protected BloomPrePassBackgroundColorsGradient _bloomPrePassBackgroundColorsGradient;
  [SerializeField]
  protected BloomPrePassBackgroundColorsGradientFromColorSchemeColors.Element[] _elements;
  [Inject]
  protected readonly EnvironmentColorManager _colorManager;

  public BloomPrePassBackgroundColorsGradientFromColorSchemeColors()
  {
    this._elements = new BloomPrePassBackgroundColorsGradientFromColorSchemeColors.Element[7];
    this._elements[0] = new BloomPrePassBackgroundColorsGradientFromColorSchemeColors.Element()
    {
      loadFromColorScheme = true,
      environmentColor = BloomPrePassBackgroundColorsGradientFromColorSchemeColors.EnvironmentColor.Color0,
      intensity = 0.25f
    };
    this._elements[1] = new BloomPrePassBackgroundColorsGradientFromColorSchemeColors.Element()
    {
      loadFromColorScheme = true,
      environmentColor = BloomPrePassBackgroundColorsGradientFromColorSchemeColors.EnvironmentColor.Color0,
      intensity = 0.25f
    };
    this._elements[2] = new BloomPrePassBackgroundColorsGradientFromColorSchemeColors.Element()
    {
      loadFromColorScheme = false,
      color = Color.black
    };
    this._elements[3] = new BloomPrePassBackgroundColorsGradientFromColorSchemeColors.Element()
    {
      loadFromColorScheme = false,
      color = Color.black
    };
    this._elements[4] = new BloomPrePassBackgroundColorsGradientFromColorSchemeColors.Element()
    {
      loadFromColorScheme = false,
      color = Color.black
    };
    this._elements[5] = new BloomPrePassBackgroundColorsGradientFromColorSchemeColors.Element()
    {
      loadFromColorScheme = true,
      environmentColor = BloomPrePassBackgroundColorsGradientFromColorSchemeColors.EnvironmentColor.Color1,
      intensity = 0.1f
    };
    this._elements[6] = new BloomPrePassBackgroundColorsGradientFromColorSchemeColors.Element()
    {
      loadFromColorScheme = false,
      color = Color.black
    };
  }

  public virtual void Start()
  {
    for (int index = 0; index < this._bloomPrePassBackgroundColorsGradient.elements.Length && index < this._elements.Length; ++index)
    {
      if (this._elements[index].loadFromColorScheme)
      {
        switch (this._elements[index].environmentColor)
        {
          case BloomPrePassBackgroundColorsGradientFromColorSchemeColors.EnvironmentColor.Color0:
            this._elements[index].color = this._colorManager.environmentColor0 * this._elements[index].intensity;
            break;
          case BloomPrePassBackgroundColorsGradientFromColorSchemeColors.EnvironmentColor.Color1:
            this._elements[index].color = this._colorManager.environmentColor1 * this._elements[index].intensity;
            break;
          case BloomPrePassBackgroundColorsGradientFromColorSchemeColors.EnvironmentColor.Color0Boost:
            this._elements[index].color = this._colorManager.environmentColor0Boost * this._elements[index].intensity;
            break;
          case BloomPrePassBackgroundColorsGradientFromColorSchemeColors.EnvironmentColor.Color1Boost:
            this._elements[index].color = this._colorManager.environmentColor1Boost * this._elements[index].intensity;
            break;
        }
      }
      this._bloomPrePassBackgroundColorsGradient.elements[index].color = this._elements[index].color;
    }
    this._bloomPrePassBackgroundColorsGradient.UpdateGradientTexture();
  }

  [Serializable]
  public class Element
  {
    public bool loadFromColorScheme;
    [DrawIf("loadFromColorScheme", true, DrawIfAttribute.DisablingType.DontDraw)]
    public BloomPrePassBackgroundColorsGradientFromColorSchemeColors.EnvironmentColor environmentColor;
    [DrawIf("loadFromColorScheme", true, DrawIfAttribute.DisablingType.DontDraw)]
    public float intensity;
    [DrawIf("loadFromColorScheme", false, DrawIfAttribute.DisablingType.DontDraw)]
    public Color color;
  }

  public enum EnvironmentColor
  {
    Color0,
    Color1,
    Color0Boost,
    Color1Boost,
  }
}
