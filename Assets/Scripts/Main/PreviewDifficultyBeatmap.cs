// Decompiled with JetBrains decompiler
// Type: PreviewDifficultyBeatmap
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Runtime.CompilerServices;

public class PreviewDifficultyBeatmap : IEquatable<PreviewDifficultyBeatmap>
{
  [CompilerGenerated]
  protected IPreviewBeatmapLevel m_CbeatmapLevel;
  [CompilerGenerated]
  protected BeatmapCharacteristicSO m_CbeatmapCharacteristic;
  [CompilerGenerated]
  protected BeatmapDifficulty m_CbeatmapDifficulty;

  public IPreviewBeatmapLevel beatmapLevel
  {
    get => this.m_CbeatmapLevel;
    set => this.m_CbeatmapLevel = value;
  }

  public BeatmapCharacteristicSO beatmapCharacteristic
  {
    get => this.m_CbeatmapCharacteristic;
    set => this.m_CbeatmapCharacteristic = value;
  }

  public BeatmapDifficulty beatmapDifficulty
  {
    get => this.m_CbeatmapDifficulty;
    set => this.m_CbeatmapDifficulty = value;
  }

  public PreviewDifficultyBeatmap(
    IPreviewBeatmapLevel beatmapLevel,
    BeatmapCharacteristicSO beatmapCharacteristic,
    BeatmapDifficulty beatmapDifficulty)
  {
    this.beatmapLevel = beatmapLevel;
    this.beatmapCharacteristic = beatmapCharacteristic;
    this.beatmapDifficulty = beatmapDifficulty;
  }

  public virtual bool Equals(PreviewDifficultyBeatmap other)
  {
    if ((object) other == null)
      return false;
    if ((object) this == (object) other)
      return true;
    return this.beatmapLevel == other.beatmapLevel && (UnityEngine.Object) this.beatmapCharacteristic == (UnityEngine.Object) other.beatmapCharacteristic && this.beatmapDifficulty == other.beatmapDifficulty;
  }

  public override bool Equals(object obj)
  {
    if (obj == null)
      return false;
    if ((object) this == obj)
      return true;
    return !(obj.GetType() != this.GetType()) && this.Equals((PreviewDifficultyBeatmap) obj);
  }

  public override int GetHashCode() => (int) ((BeatmapDifficulty) (((this.beatmapLevel != null ? this.beatmapLevel.GetHashCode() : 0) * 397 ^ ((UnityEngine.Object) this.beatmapCharacteristic != (UnityEngine.Object) null ? ((object) this.beatmapCharacteristic).GetHashCode() : 0)) * 397) ^ this.beatmapDifficulty);

  public static bool operator ==(PreviewDifficultyBeatmap a, PreviewDifficultyBeatmap b)
  {
    if ((object) a == (object) b)
      return true;
    return (object) a != null && a.Equals(b);
  }

  public static bool operator !=(PreviewDifficultyBeatmap a, PreviewDifficultyBeatmap b) => !(a == b);
}
