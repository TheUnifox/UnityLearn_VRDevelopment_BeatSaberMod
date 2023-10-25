// Decompiled with JetBrains decompiler
// Type: LightColorHelpers
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public abstract class LightColorHelpers
{
  public static Color AdjustColorWithStrobe(
    Color fromColor,
    Color toColor,
    Color offColor,
    float t,
    float fromStrobeFrequency,
    float toStrobeFrequency,
    float tweenDuration)
  {
    Color color = Color.LerpUnclamped(fromColor, toColor, t);
    if ((double) fromStrobeFrequency > 0.0 || (double) toStrobeFrequency > 0.0)
    {
      float num1 = t * tweenDuration;
      float num2 = (float) ((double) num1 * (double) num1 / (2.0 * (double) tweenDuration));
      if ((-(double) fromStrobeFrequency * (double) num2 + (double) fromStrobeFrequency * (double) num1 + (double) toStrobeFrequency * (double) num2) % 1.0 > 0.5)
        color = offColor;
    }
    return color;
  }
}
