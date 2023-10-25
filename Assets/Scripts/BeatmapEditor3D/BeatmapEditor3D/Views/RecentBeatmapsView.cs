// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.RecentBeatmapsView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BeatmapEditor3D.Views
{
  public class RecentBeatmapsView : MonoBehaviour
  {
    [SerializeField]
    private RecentBeatmapView[] _recentBeatmapViews;

    public event Action<string> openRecentBeatmapEvent;

    public void SetData(
      IReadOnlyList<string> recentlyOpenedBeatmaps,
      IReadOnlyList<IBeatmapInfoData> beatmapInfos)
    {
      if (beatmapInfos == null || recentlyOpenedBeatmaps == null)
        return;
      int index = 0;
      foreach (string recentlyOpenedBeatmap in (IEnumerable<string>) recentlyOpenedBeatmaps)
      {
        string recentBeatmap = recentlyOpenedBeatmap;
        IBeatmapInfoData beatmapInfoData = beatmapInfos.FirstOrDefault<IBeatmapInfoData>((Func<IBeatmapInfoData, bool>) (info => info.beatmapFolderPath == recentBeatmap));
        if (beatmapInfoData != null)
        {
          this._recentBeatmapViews[index].SetData(beatmapInfoData, (Action<string>) (path =>
          {
            Action<string> recentBeatmapEvent = this.openRecentBeatmapEvent;
            if (recentBeatmapEvent == null)
              return;
            recentBeatmapEvent(path);
          }));
          ++index;
        }
      }
    }
  }
}
