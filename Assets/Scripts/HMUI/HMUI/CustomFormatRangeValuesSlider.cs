// Decompiled with JetBrains decompiler
// Type: HMUI.CustomFormatRangeValuesSlider
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using UnityEngine;

namespace HMUI
{
  public class CustomFormatRangeValuesSlider : RangeValuesTextSlider
  {
    [SerializeField]
    protected string _formatString;

    protected override string TextForValue(float value) => string.Format(this._formatString, (object) value);
  }
}
