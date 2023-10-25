// Decompiled with JetBrains decompiler
// Type: GameServersFilter
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public class GameServersFilter
{
  public bool filterByDifficulty;
  public BeatmapDifficultyMask filteredDifficulty;
  public bool filterByModifiers;
  public GameplayModifierMask filteredModifiers;
  public bool filterBySongPacks;
  public SongPackMask filteredSongPacks;
  public bool showFull;
  public bool showProtected;
  public bool showInternetGames;

  public GameServersFilter()
  {
    this.filterByDifficulty = false;
    this.filteredDifficulty = BeatmapDifficultyMask.Normal;
    this.filterByModifiers = false;
    this.filteredModifiers = GameplayModifierMask.All;
    this.filterBySongPacks = false;
    this.filteredSongPacks = new SongPackMask();
    this.showFull = false;
    this.showProtected = false;
    this.showInternetGames = true;
  }
}
