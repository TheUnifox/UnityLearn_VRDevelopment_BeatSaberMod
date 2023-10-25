// Decompiled with JetBrains decompiler
// Type: ScoreFormatter
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Globalization;

public class ScoreFormatter
{
  [DoesNotRequireDomainReloadInit]
  protected static readonly NumberFormatInfo _numberFormatInfo = (NumberFormatInfo) CultureInfo.InvariantCulture.NumberFormat.Clone();

  static ScoreFormatter() => ScoreFormatter._numberFormatInfo.NumberGroupSeparator = " ";

  public static string Format(int score) => score.ToString("#,0", (IFormatProvider) ScoreFormatter._numberFormatInfo);
}
