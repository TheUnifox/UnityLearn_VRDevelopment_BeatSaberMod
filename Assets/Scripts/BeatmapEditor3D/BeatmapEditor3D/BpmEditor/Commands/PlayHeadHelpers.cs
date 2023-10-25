// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BpmEditor.Commands.PlayHeadHelpers
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System.Collections.Generic;
using UnityEngine;

namespace BeatmapEditor3D.BpmEditor.Commands
{
  public static class PlayHeadHelpers
  {
    private static int FindIndexForward(IReadOnlyList<BpmRegion> regions, int index, int sample)
    {
      index = Mathf.Clamp(index, 0, regions.Count - 1);
      while (index > 0 && index < regions.Count && regions[index].startSampleIndex >= sample)
        --index;
      return index;
    }

    private static int FindIndexBackward(IReadOnlyList<BpmRegion> regions, int index, int sample)
    {
      while (index >= 0 && index < regions.Count - 1 && regions[index + 1].startSampleIndex < sample)
        ++index;
      return index;
    }

    public static int FindIndex(IReadOnlyList<BpmRegion> regions, int index, int sample) => PlayHeadHelpers.FindIndexBackward(regions, PlayHeadHelpers.FindIndexForward(regions, index, sample), sample);
  }
}
