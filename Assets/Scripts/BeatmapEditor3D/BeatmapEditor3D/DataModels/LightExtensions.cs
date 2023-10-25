// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.LightExtensions
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;

namespace BeatmapEditor3D.DataModels
{
  public static class LightExtensions
  {
    public static string ToName(this LightColor color) => color != LightColor.Blue ? "Red" : "Blue";

    public static string ToColorHex(this LightColor color) => color != LightColor.Blue ? "FF7F00" : "00B2FF";

    public static string ToName(this LightEventType type)
    {
      switch (type)
      {
        case LightEventType.Off:
          return "Off";
        case LightEventType.On:
          return "On";
        case LightEventType.Flash:
          return "Flash";
        case LightEventType.FadeOut:
          return "FadeOut";
        case LightEventType.Fade:
          return "Fade";
        default:
          throw new ArgumentOutOfRangeException(nameof (type), (object) type, (string) null);
      }
    }
  }
}
