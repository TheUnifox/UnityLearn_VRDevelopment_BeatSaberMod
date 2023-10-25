// Decompiled with JetBrains decompiler
// Type: IDifficultyBeatmap
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Threading.Tasks;

public interface IDifficultyBeatmap
{
  IBeatmapLevel level { get; }

  IDifficultyBeatmapSet parentDifficultyBeatmapSet { get; }

  BeatmapDifficulty difficulty { get; }

  int difficultyRank { get; }

  float noteJumpMovementSpeed { get; }

  float noteJumpStartBeatOffset { get; }

  Task<IBeatmapDataBasicInfo> GetBeatmapDataBasicInfoAsync();

  Task<IReadonlyBeatmapData> GetBeatmapDataAsync(
    EnvironmentInfoSO environmentInfo,
    PlayerSpecificSettings playerSpecificSettings);
}
