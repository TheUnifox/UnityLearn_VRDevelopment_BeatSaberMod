// Decompiled with JetBrains decompiler
// Type: HMUI.TimeSlider
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;
using UnityEngine;

namespace HMUI
{
  public class TimeSlider : RangeValuesTextSlider
  {
    [SerializeField]
    protected TimeSlider.TimeType _timeType;

    protected override string TextForValue(float value)
    {
      if (this._timeType == TimeSlider.TimeType.Miliseconds)
        return string.Format("{0} ms", (object) Mathf.RoundToInt(value * 1000f));
      TimeSpan timeSpan = TimeSpan.FromSeconds((double) value);
      return timeSpan.Minutes > 0 ? string.Format("{0} m {1} s", (object) timeSpan.Minutes, (object) timeSpan.Seconds) : string.Format("{0} s", (object) timeSpan.Seconds);
    }

    public enum TimeType
    {
      Default,
      Miliseconds,
    }
  }
}
