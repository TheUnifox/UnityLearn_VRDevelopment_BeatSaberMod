// Decompiled with JetBrains decompiler
// Type: ColorExtensions
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System.Runtime.CompilerServices;
using UnityEngine;

public static class ColorExtensions
{
  public static Color SaturatedColor(this Color color, float saturation)
  {
    float H;
    float V;
    Color.RGBToHSV(color, out H, out float _, out V);
    float S = saturation;
    return Color.HSVToRGB(H, S, V);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Color ColorWithAlpha(this Color color, float alpha)
  {
    color.a = alpha;
    return color;
  }

  public static Color ColorWithR(this Color color, float r)
  {
    color.r = r;
    return color;
  }

  public static Color ColorWithG(this Color color, float g)
  {
    color.g = g;
    return color;
  }

  public static Color ColorWithB(this Color color, float b)
  {
    color.b = b;
    return color;
  }

  public static Color ColorWithValue(this Color color, float value)
  {
    float H;
    float S;
    Color.RGBToHSV(color, out H, out S, out float _);
    float V = value;
    return Color.HSVToRGB(H, S, V);
  }

  public static Color LerpRGBUnclamped(this Color a, Color b, float t) => new Color(a.r + (b.r - a.r) * t, a.g + (b.g - a.g) * t, a.b + (b.b - a.b) * t, a.a);
}
