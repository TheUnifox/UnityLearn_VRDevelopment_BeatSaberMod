// Decompiled with JetBrains decompiler
// Type: EmptyDifficultyBeatmap
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;
using System.Threading.Tasks;

public class EmptyDifficultyBeatmap : IDifficultyBeatmap
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
  protected BeatmapData _beatmapData;

  public IBeatmapLevel level => this.m_Clevel;

  public IDifficultyBeatmapSet parentDifficultyBeatmapSet => this.m_CparentDifficultyBeatmapSet;

  public BeatmapDifficulty difficulty => this.m_Cdifficulty;

  public int difficultyRank => this.m_CdifficultyRank;

  public float noteJumpMovementSpeed => this.m_CnoteJumpMovementSpeed;

  public float noteJumpStartBeatOffset => this.m_CnoteJumpStartBeatOffset;

  public IBeatmapDataBasicInfo beatmapDataBasicInfo => (IBeatmapDataBasicInfo) this._beatmapData;

  public virtual async Task<IBeatmapDataBasicInfo> GetBeatmapDataBasicInfoAsync() => (IBeatmapDataBasicInfo) await Task.FromResult<BeatmapData>(this._beatmapData);

  public virtual async Task<IReadonlyBeatmapData> GetBeatmapDataAsync(
    EnvironmentInfoSO environmentInfo,
    PlayerSpecificSettings playerSpecificSettings)
  {
    return await Task.FromResult<IReadonlyBeatmapData>((IReadonlyBeatmapData) this._beatmapData);
  }

  public EmptyDifficultyBeatmap()
  {
    this.m_Clevel = (IBeatmapLevel) new EmptyBeatmapLevel();
    this._beatmapData = new BeatmapData(0);
  }
}
