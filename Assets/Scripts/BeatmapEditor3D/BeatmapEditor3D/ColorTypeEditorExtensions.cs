// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.ColorTypeEditorExtensions
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using UnityEngine;

namespace BeatmapEditor3D
{
  public static class ColorTypeEditorExtensions
  {
    public static Color ToColor(this ColorType colorType)
    {
      if (colorType == ColorType.ColorA)
        return Color.red;
      return colorType == ColorType.ColorB ? Color.blue : new Color(0.25f, 0.25f, 0.25f, 1f);
    }
  }
}
