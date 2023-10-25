// Decompiled with JetBrains decompiler
// Type: CustomDifficultyBeatmap
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using BeatmapSaveDataVersion3;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

public class CustomDifficultyBeatmap : IDifficultyBeatmap
{
  [CompilerGenerated]
  protected readonly IBeatmapLevel m_Clevel;
  [CompilerGenerated]
  protected readonly IDifficultyBeatmapSet m_CparentDifficultyBeatmapSet;
  [CompilerGenerated]
  protected readonly BeatmapDifficulty m_Cdifficulty;
  [CompilerGenerated]
  protected readonly int m_CdifficultyRank;
  [CompilerGenerated]
  protected readonly float m_CnoteJumpMovementSpeed;
  [CompilerGenerated]
  protected readonly float m_CnoteJumpStartBeatOffset;
  [CompilerGenerated]
  protected readonly IBeatmapDataBasicInfo m_CbeatmapDataBasicInfo;
  [CompilerGenerated]
  protected readonly float m_CbeatsPerMinute;
  [CompilerGenerated]
  protected readonly BeatmapSaveData m_CbeatmapSaveData;

  public IBeatmapLevel level => this.m_Clevel;

  public IDifficultyBeatmapSet parentDifficultyBeatmapSet => this.m_CparentDifficultyBeatmapSet;

  public BeatmapDifficulty difficulty => this.m_Cdifficulty;

  public int difficultyRank => this.m_CdifficultyRank;

  public float noteJumpMovementSpeed => this.m_CnoteJumpMovementSpeed;

  public float noteJumpStartBeatOffset => this.m_CnoteJumpStartBeatOffset;

  public IBeatmapDataBasicInfo beatmapDataBasicInfo => this.m_CbeatmapDataBasicInfo;

  public float beatsPerMinute => this.m_CbeatsPerMinute;

  public BeatmapSaveData beatmapSaveData => this.m_CbeatmapSaveData;

  public virtual async Task<IBeatmapDataBasicInfo> GetBeatmapDataBasicInfoAsync() => await Task.FromResult<IBeatmapDataBasicInfo>(this.beatmapDataBasicInfo);

  public virtual async Task<IReadonlyBeatmapData> GetBeatmapDataAsync(
    EnvironmentInfoSO environmentInfo,
    PlayerSpecificSettings playerSpecificSettings)
  {
    IReadonlyBeatmapData readonlyBeatmapData = (IReadonlyBeatmapData) null;
    await Task.Run((System.Action) (() => readonlyBeatmapData = (IReadonlyBeatmapData) BeatmapDataLoader.GetBeatmapDataFromSaveData(this.beatmapSaveData, this.difficulty, this.beatsPerMinute, this.level.environmentInfo.serializedName == environmentInfo.serializedName, environmentInfo, playerSpecificSettings)));
    return readonlyBeatmapData;
  }

  public CustomDifficultyBeatmap(
    IBeatmapLevel level,
    IDifficultyBeatmapSet parentDifficultyBeatmapSet,
    BeatmapDifficulty difficulty,
    int difficultyRank,
    float noteJumpMovementSpeed,
    float noteJumpStartBeatOffset,
    float beatsPerMinute,
    BeatmapSaveData beatmapSaveData,
    IBeatmapDataBasicInfo beatmapDataBasicInfo)
  {
    this.m_Clevel = level;
    this.m_CparentDifficultyBeatmapSet = parentDifficultyBeatmapSet;
    this.m_Cdifficulty = difficulty;
    this.m_CdifficultyRank = difficultyRank;
    this.m_CnoteJumpMovementSpeed = noteJumpMovementSpeed;
    this.m_CnoteJumpStartBeatOffset = noteJumpStartBeatOffset;
    this.m_CbeatsPerMinute = beatsPerMinute;
    this.m_CbeatmapSaveData = beatmapSaveData;
    this.m_CbeatmapDataBasicInfo = beatmapDataBasicInfo;
  }
}
