// Decompiled with JetBrains decompiler
// Type: LevelFilterParams
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class LevelFilterParams
{
  [CompilerGenerated]
  protected bool m_CfilterByLevelIds;
  [CompilerGenerated]
  protected HashSet<string> m_CbeatmapLevelIds;
  [CompilerGenerated]
  protected bool m_CfilterByOwned;
  [CompilerGenerated]
  protected bool m_CfilterByNotOwned;
  [CompilerGenerated]
  protected string m_CsearchText;
  [CompilerGenerated]
  protected bool m_CfilterByDifficulty;
  [CompilerGenerated]
  protected BeatmapDifficultyMask m_CfilteredDifficulty;
  [CompilerGenerated]
  protected bool m_CfilterBySongPacks;
  [CompilerGenerated]
  protected SongPackMask m_CfilteredSongPacks;
  [CompilerGenerated]
  protected bool m_CfilterByCharacteristic;
  [CompilerGenerated]
  protected BeatmapCharacteristicSO m_CfilteredCharacteristic;
  [CompilerGenerated]
  protected bool m_CfilterByNotPlayedYet;
  [CompilerGenerated]
  protected bool m_CfilterByMinBpm;
  [CompilerGenerated]
  protected float m_CfilteredMinBpm;
  [CompilerGenerated]
  protected bool m_CfilterByMaxBpm;
  [CompilerGenerated]
  protected float m_CfilteredMaxBpm;
  [DoesNotRequireDomainReloadInit]
  public static readonly float[] bpmValues = new float[28]
  {
    60f,
    70f,
    80f,
    90f,
    100f,
    110f,
    120f,
    130f,
    140f,
    150f,
    160f,
    170f,
    180f,
    190f,
    200f,
    210f,
    220f,
    230f,
    240f,
    250f,
    260f,
    270f,
    280f,
    290f,
    300f,
    310f,
    320f,
    330f
  };

  public bool filterByLevelIds
  {
    get => this.m_CfilterByLevelIds;
    set => this.m_CfilterByLevelIds = value;
  }

  public HashSet<string> beatmapLevelIds
  {
    get => this.m_CbeatmapLevelIds;
    set => this.m_CbeatmapLevelIds = value;
  }

  public bool filterByOwned
  {
    get => this.m_CfilterByOwned;
    private set => this.m_CfilterByOwned = value;
  }

  public bool filterByNotOwned
  {
    get => this.m_CfilterByNotOwned;
    private set => this.m_CfilterByNotOwned = value;
  }

  public string searchText
  {
    get => this.m_CsearchText;
    set => this.m_CsearchText = value;
  }

  public bool filterByDifficulty
  {
    get => this.m_CfilterByDifficulty;
    private set => this.m_CfilterByDifficulty = value;
  }

  public BeatmapDifficultyMask filteredDifficulty
  {
    get => this.m_CfilteredDifficulty;
    private set => this.m_CfilteredDifficulty = value;
  }

  public bool filterBySongPacks
  {
    get => this.m_CfilterBySongPacks;
    private set => this.m_CfilterBySongPacks = value;
  }

  public SongPackMask filteredSongPacks
  {
    get => this.m_CfilteredSongPacks;
    private set => this.m_CfilteredSongPacks = value;
  }

  public bool filterByCharacteristic
  {
    get => this.m_CfilterByCharacteristic;
    private set => this.m_CfilterByCharacteristic = value;
  }

  public BeatmapCharacteristicSO filteredCharacteristic
  {
    get => this.m_CfilteredCharacteristic;
    private set => this.m_CfilteredCharacteristic = value;
  }

  public bool filterByNotPlayedYet
  {
    get => this.m_CfilterByNotPlayedYet;
    private set => this.m_CfilterByNotPlayedYet = value;
  }

  public bool filterByMinBpm
  {
    get => this.m_CfilterByMinBpm;
    private set => this.m_CfilterByMinBpm = value;
  }

  public float filteredMinBpm
  {
    get => this.m_CfilteredMinBpm;
    private set => this.m_CfilteredMinBpm = value;
  }

  public bool filterByMaxBpm
  {
    get => this.m_CfilterByMaxBpm;
    private set => this.m_CfilterByMaxBpm = value;
  }

  public float filteredMaxBpm
  {
    get => this.m_CfilteredMaxBpm;
    private set => this.m_CfilteredMaxBpm = value;
  }

  private LevelFilterParams()
  {
    this.filterByLevelIds = false;
    this.searchText = (string) null;
    this.beatmapLevelIds = (HashSet<string>) null;
    this.filterByOwned = false;
    this.filterByNotOwned = false;
    this.filterByDifficulty = false;
    this.filteredDifficulty = BeatmapDifficultyMask.All;
    this.filterByCharacteristic = false;
    this.filteredCharacteristic = (BeatmapCharacteristicSO) null;
    this.filterBySongPacks = false;
    this.filteredSongPacks = SongPackMask.all;
    this.filterByNotPlayedYet = false;
    this.filterByMinBpm = false;
    this.filteredMinBpm = LevelFilterParams.bpmValues[0];
    this.filterByMaxBpm = false;
    this.filteredMaxBpm = LevelFilterParams.bpmValues[LevelFilterParams.bpmValues.Length - 1];
  }

  public LevelFilterParams(
    bool filterByLevelIds,
    HashSet<string> beatmapLevelIds,
    string searchText,
    bool filterByOwned,
    bool filterByNotOwned,
    bool filterByDifficulty,
    BeatmapDifficultyMask filteredDifficulty,
    bool filterByCharacteristic,
    BeatmapCharacteristicSO filteredCharacteristic,
    bool filterBySongPacks,
    SongPackMask filteredSongPacks,
    bool filterByNotPlayedYet,
    bool filterByMinBpm,
    float filteredMinBpm,
    bool filterByMaxBpm,
    float filteredMaxBpm)
  {
    this.filterByLevelIds = filterByLevelIds;
    this.beatmapLevelIds = beatmapLevelIds;
    this.searchText = searchText;
    this.filterByOwned = filterByOwned;
    this.filterByNotOwned = filterByNotOwned;
    this.filterByDifficulty = filterByDifficulty;
    this.filteredDifficulty = filteredDifficulty;
    this.filterByCharacteristic = filterByCharacteristic;
    this.filteredCharacteristic = filteredCharacteristic;
    this.filterBySongPacks = filterBySongPacks;
    this.filteredSongPacks = filteredSongPacks;
    this.filterByNotPlayedYet = filterByNotPlayedYet;
    this.filterByMinBpm = filterByMinBpm;
    this.filteredMinBpm = filteredMinBpm;
    this.filterByMaxBpm = filterByMaxBpm;
    this.filteredMaxBpm = filteredMaxBpm;
  }

  public virtual bool IsWithoutFilter(bool ignoreFilterBySongs) => (ignoreFilterBySongs || !this.filterByLevelIds) && !this.filterByNotPlayedYet && !this.filterByOwned && !this.filterByNotOwned && !this.filterByDifficulty && !this.filterByCharacteristic && !this.filterBySongPacks && !this.filterByMinBpm && !this.filterByMaxBpm;

  private LevelFilterParams(HashSet<string> beatmapLevelIds)
    : this()
  {
    this.filterByLevelIds = true;
    this.beatmapLevelIds = beatmapLevelIds;
  }

  private LevelFilterParams(BeatmapCharacteristicSO beatmapCharacteristic)
    : this()
  {
    this.filterByCharacteristic = true;
    this.filteredCharacteristic = beatmapCharacteristic;
  }

  public static LevelFilterParams NoFilter() => new LevelFilterParams();

  public static LevelFilterParams ByBeatmapLevelIds(HashSet<string> beatmapLevelIds) => new LevelFilterParams(beatmapLevelIds);

  public static LevelFilterParams ByBeatmapCharacteristic(
    BeatmapCharacteristicSO beatmapCharacteristic)
  {
    return new LevelFilterParams(beatmapCharacteristic);
  }
}
