// Decompiled with JetBrains decompiler
// Type: ColorManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class ColorManager
{
  [Inject]
  protected readonly ColorScheme _colorScheme;

  public Color obstaclesColor => this._colorScheme.obstaclesColor;

  public virtual Color ColorForType(EnvironmentColorType type, bool boost)
  {
    switch (type)
    {
      case EnvironmentColorType.Color0:
        return !boost || !this._colorScheme.supportsEnvironmentColorBoost ? this._colorScheme.environmentColor0 : this._colorScheme.environmentColor0Boost;
      case EnvironmentColorType.Color1:
        return !boost || !this._colorScheme.supportsEnvironmentColorBoost ? this._colorScheme.environmentColor1 : this._colorScheme.environmentColor1Boost;
      case EnvironmentColorType.ColorW:
        return !boost || !this._colorScheme.supportsEnvironmentColorBoost ? this._colorScheme.environmentColorW : this._colorScheme.environmentColorWBoost;
      default:
        return Color.black;
    }
  }

  public virtual Color ColorForType(ColorType type)
  {
    if (type == ColorType.ColorB)
      return this._colorScheme.saberBColor;
    return type == ColorType.ColorA ? this._colorScheme.saberAColor : Color.black;
  }

  public virtual Color ColorForSaberType(SaberType type) => type == SaberType.SaberB ? this._colorScheme.saberBColor : this._colorScheme.saberAColor;

  public virtual Color EffectsColorForSaberType(SaberType type)
  {
    float H;
    float S;
    float V;
    Color.RGBToHSV(type == SaberType.SaberB ? this._colorScheme.saberBColor : this._colorScheme.saberAColor, out H, out S, out V);
    V = 1f;
    return Color.HSVToRGB(H, S, V);
  }

  public virtual Color GetObstacleEffectColor()
  {
    float H;
    float S;
    Color.RGBToHSV(this._colorScheme.obstaclesColor, out H, out S, out float _);
    float V = 1f;
    return Color.HSVToRGB(H, S, V);
  }
}
