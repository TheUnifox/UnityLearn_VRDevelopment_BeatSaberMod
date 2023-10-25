// Decompiled with JetBrains decompiler
// Type: IPreviewBeatmapLevel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public interface IPreviewBeatmapLevel
{
  string levelID { get; }

  string songName { get; }

  string songSubName { get; }

  string songAuthorName { get; }

  string levelAuthorName { get; }

  float beatsPerMinute { get; }

  float songTimeOffset { get; }

  float shuffle { get; }

  float shufflePeriod { get; }

  float previewStartTime { get; }

  float previewDuration { get; }

  float songDuration { get; }

  EnvironmentInfoSO environmentInfo { get; }

  EnvironmentInfoSO allDirectionsEnvironmentInfo { get; }

  IReadOnlyList<PreviewDifficultyBeatmapSet> previewDifficultyBeatmapSets { get; }

  Task<Sprite> GetCoverImageAsync(CancellationToken cancellationToken);
}
