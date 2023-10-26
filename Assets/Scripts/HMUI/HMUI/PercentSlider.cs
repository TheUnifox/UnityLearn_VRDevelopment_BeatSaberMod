// Decompiled with JetBrains decompiler
// Type: HMUI.PercentSlider
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

namespace HMUI
{
  public class PercentSlider : RangeValuesTextSlider
  {
    protected override string TextForValue(float value) => string.Format("{0:F1}%", (object) (float) ((double) value * 100.0));
  }
}
