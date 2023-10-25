// Decompiled with JetBrains decompiler
// Type: ColorSchemeConverter
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public abstract class ColorSchemeConverter
{
  public static ColorScheme FromNetSerializable(ColorSchemeNetSerializable serialized) => new ColorScheme("deserialized", "deserialized", false, string.Empty, false, (Color) serialized.saberAColor, (Color) serialized.saberBColor, (Color) serialized.environmentColor0, (Color) serialized.environmentColor1, true, (Color) serialized.environmentColor0Boost, (Color) serialized.environmentColor1Boost, (Color) serialized.obstaclesColor);
}
