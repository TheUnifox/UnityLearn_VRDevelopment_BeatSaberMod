// Decompiled with JetBrains decompiler
// Type: EmptyBeatmapLevel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class EmptyBeatmapLevel : IBeatmapLevel, IPreviewBeatmapLevel
{
  [CompilerGenerated]
  protected readonly IBeatmapLevelData m_CbeatmapLevelData;

  public string levelID => (string) null;

  public string songName => (string) null;

  public string songSubName => (string) null;

  public string songAuthorName => (string) null;

  public string levelAuthorName => (string) null;

  public float beatsPerMinute => 0.0f;

  public float songTimeOffset => 0.0f;

  public float shuffle => 0.0f;

  public float shufflePeriod => 0.0f;

  public float previewStartTime => 0.0f;

  public float previewDuration => 0.0f;

  public float songDuration => 0.0f;

  public EnvironmentInfoSO environmentInfo => (EnvironmentInfoSO) null;

  public EnvironmentInfoSO allDirectionsEnvironmentInfo => (EnvironmentInfoSO) null;

  public IReadOnlyList<PreviewDifficultyBeatmapSet> previewDifficultyBeatmapSets => (IReadOnlyList<PreviewDifficultyBeatmapSet>) null;

  public virtual Task<AudioClip> GetPreviewAudioClipAsync(CancellationToken cancellationToken) => throw new NotImplementedException();

  public virtual Task<Sprite> GetCoverImageAsync(CancellationToken cancellationToken) => throw new NotImplementedException();

  public IBeatmapLevelData beatmapLevelData => this.m_CbeatmapLevelData;

  public EmptyBeatmapLevel() => this.m_CbeatmapLevelData = (IBeatmapLevelData) new EmptyBeatmapLevelData();
}
