// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BpmEditor.UI.WaveformPlacementHelper
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using UnityEngine;

namespace BeatmapEditor3D.BpmEditor.UI
{
  public static class WaveformPlacementHelper
  {
    public static float CalculateRegionPosition(
      RectTransform containerTransform,
      int sample,
      int startSample,
      int endSample)
    {
      return Mathf.InverseLerp((float) startSample, (float) endSample, (float) sample) * containerTransform.rect.width;
    }

    public static float CalculateRegionWidth(
      RectTransform containerTransform,
      int regionStartSample,
      int regionEndSample,
      int startSample,
      int endSample)
    {
      float num = Mathf.InverseLerp((float) startSample, (float) endSample, (float) regionStartSample) * containerTransform.rect.width;
      return Mathf.InverseLerp((float) startSample, (float) endSample, (float) regionEndSample) * containerTransform.rect.width - num;
    }
  }
}
