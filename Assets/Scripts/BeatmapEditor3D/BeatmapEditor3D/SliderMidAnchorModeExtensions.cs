// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.SliderMidAnchorModeExtensions
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using UnityEngine;

namespace BeatmapEditor3D
{
  public static class SliderMidAnchorModeExtensions
  {
    public static float RotationAngle(this SliderMidAnchorMode midAnchorMode)
    {
      if (midAnchorMode == SliderMidAnchorMode.Clockwise)
        return -30f;
      return midAnchorMode == SliderMidAnchorMode.CounterClockwise ? 30f : 0.0f;
    }

    public static Quaternion Rotation(this SliderMidAnchorMode midAnchorMode, float offset = 0.0f) => Quaternion.Euler(0.0f, 0.0f, midAnchorMode.RotationAngle() + offset);

    public static SliderMidAnchorMode SliderMidAnchorModeFromDirection(Vector3 direction)
    {
      float num = Vector2.Angle((Vector2) direction, Vector2.up);
      if ((double) direction.x > 0.0 && (double) num > 30.0 && (double) num <= 180.0)
        return SliderMidAnchorMode.Clockwise;
      return (double) direction.x < 0.0 && (double) num > 30.0 && (double) num <= 180.0 ? SliderMidAnchorMode.CounterClockwise : SliderMidAnchorMode.Straight;
    }
  }
}
