// Decompiled with JetBrains decompiler
// Type: AlphabetScrollbarInfoBeatmapLevelHelper
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using System.Linq;

public abstract class AlphabetScrollbarInfoBeatmapLevelHelper
{
  private const string kFirstAlphabet = "A";
  private const char kNonAlphabetChar = '#';
  private const int kMaxCharactersCount = 28;

  public static IReadOnlyList<AlphabetScrollInfo.Data> CreateData(
    IReadOnlyList<IPreviewBeatmapLevel> previewBeatmapLevels,
    out IReadOnlyList<IPreviewBeatmapLevel> sortedPreviewBeatmapLevels)
  {
    List<AlphabetScrollInfo.Data> data = new List<AlphabetScrollInfo.Data>();
    if (previewBeatmapLevels == null || previewBeatmapLevels.Count == 0)
    {
      sortedPreviewBeatmapLevels = (IReadOnlyList<IPreviewBeatmapLevel>) null;
      return (IReadOnlyList<AlphabetScrollInfo.Data>) null;
    }
    sortedPreviewBeatmapLevels = (IReadOnlyList<IPreviewBeatmapLevel>) previewBeatmapLevels.OrderBy<IPreviewBeatmapLevel, string>((Func<IPreviewBeatmapLevel, string>) (x => x.songName.ToUpperInvariant())).ToArray<IPreviewBeatmapLevel>();
    string strA1 = sortedPreviewBeatmapLevels[0].songName.ToUpperInvariant().Substring(0, 1);
    if (string.CompareOrdinal(strA1, "A") < 0)
      data.Add(new AlphabetScrollInfo.Data('#', 0));
    else
      data.Add(new AlphabetScrollInfo.Data(strA1[0], 0));
    for (int index = 1; index < sortedPreviewBeatmapLevels.Count; ++index)
    {
      string strA2 = sortedPreviewBeatmapLevels[index].songName.ToUpperInvariant().Substring(0, 1);
      if (string.CompareOrdinal(strA2, "A") >= 0 && strA1 != strA2)
      {
        strA1 = strA2;
        if (data.Count < 27)
        {
          data.Add(new AlphabetScrollInfo.Data(strA2[0], index));
        }
        else
        {
          data.Add(new AlphabetScrollInfo.Data(strA2[0], index));
          break;
        }
      }
    }
    return (IReadOnlyList<AlphabetScrollInfo.Data>) data;
  }
}
