// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.ColorTypeHelper
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using UnityEngine;

namespace BeatmapEditor3D
{
  public static class ColorTypeHelper
  {
    public static Color GetColorByColorType(ColorType type)
    {
      if (type == ColorType.ColorA)
        return Color.red;
      return type == ColorType.ColorB ? Color.blue : new Color(0.25f, 0.25f, 0.25f, 1f);
    }

    public static ColorEventMarkerObject.EnvironmentColor ToObjectColor(
      this LightColorBaseEditorData.EnvironmentColorType environmentColorType)
    {
      switch (environmentColorType)
      {
        case LightColorBaseEditorData.EnvironmentColorType.Color0:
          return ColorEventMarkerObject.EnvironmentColor.Color0;
        case LightColorBaseEditorData.EnvironmentColorType.Color1:
          return ColorEventMarkerObject.EnvironmentColor.Color1;
        case LightColorBaseEditorData.EnvironmentColorType.ColorW:
          return ColorEventMarkerObject.EnvironmentColor.ColorW;
        default:
          return ColorEventMarkerObject.EnvironmentColor.Default;
      }
    }

    public static LightColorBaseEditorData.EnvironmentColorType MirrorColor(
      this LightColorBaseEditorData.EnvironmentColorType color)
    {
      switch (color)
      {
        case LightColorBaseEditorData.EnvironmentColorType.None:
        case LightColorBaseEditorData.EnvironmentColorType.ColorW:
          return color;
        case LightColorBaseEditorData.EnvironmentColorType.Color0:
          return LightColorBaseEditorData.EnvironmentColorType.Color1;
        default:
          return LightColorBaseEditorData.EnvironmentColorType.Color0;
      }
    }

    public static LightColorBaseEditorData.EnvironmentColorType SwapColor(
      this LightColorBaseEditorData.EnvironmentColorType color)
    {
      if (color == LightColorBaseEditorData.EnvironmentColorType.Color0)
        return LightColorBaseEditorData.EnvironmentColorType.Color1;
      return color != LightColorBaseEditorData.EnvironmentColorType.Color1 ? LightColorBaseEditorData.EnvironmentColorType.Color0 : LightColorBaseEditorData.EnvironmentColorType.ColorW;
    }
  }
}
