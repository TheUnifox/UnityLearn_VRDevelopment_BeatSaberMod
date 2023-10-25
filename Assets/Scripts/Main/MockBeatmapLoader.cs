// Decompiled with JetBrains decompiler
// Type: MockBeatmapLoader
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class MockBeatmapLoader : IMockBeatmapDataProvider, IDisposable
{
  protected readonly BeatmapLevelsModel _beatmapLevelsModel;

  public MockBeatmapLoader(BeatmapLevelsModel beatmapLevelsModel) => this._beatmapLevelsModel = beatmapLevelsModel;

  public virtual async Task<MockBeatmapData> GetBeatmapData(
    BeatmapIdentifierNetSerializable beatmap,
    CancellationToken cancellationToken)
  {
    IDifficultyBeatmap difficultyBeatmap = (await this._beatmapLevelsModel.GetBeatmapLevelAsync(beatmap.levelID, cancellationToken)).beatmapLevel.beatmapLevelData.difficultyBeatmapSets.Where<IDifficultyBeatmapSet>((Func<IDifficultyBeatmapSet, bool>) (bds => bds.beatmapCharacteristic.serializedName == beatmap.beatmapCharacteristicSerializedName)).SelectMany<IDifficultyBeatmapSet, IDifficultyBeatmap>((Func<IDifficultyBeatmapSet, IEnumerable<IDifficultyBeatmap>>) (bds => (IEnumerable<IDifficultyBeatmap>) bds.difficultyBeatmaps)).FirstOrDefault<IDifficultyBeatmap>((Func<IDifficultyBeatmap, bool>) (dbm => dbm.difficulty == beatmap.difficulty));
    IReadonlyBeatmapData beatmapData = (IReadonlyBeatmapData) null;
    Task.Run((Func<Task>) (async () => beatmapData = await difficultyBeatmap.GetBeatmapDataAsync(difficultyBeatmap.level.environmentInfo, (PlayerSpecificSettings) null))).Wait();
    return beatmapData.ToMockBeatmapData();
  }

  public virtual void Dispose()
  {
  }
}
