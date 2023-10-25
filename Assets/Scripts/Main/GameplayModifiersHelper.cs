// Decompiled with JetBrains decompiler
// Type: GameplayModifiersHelper
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using BeatSaberAPI.DataTransferObjects;
using System.Collections.Generic;

public abstract class GameplayModifiersHelper
{
  public static LevelScoreResult.GameplayModifiers[] ToDTO(GameplayModifiers gameplayModifiers)
  {
    if (gameplayModifiers == null)
      return (LevelScoreResult.GameplayModifiers[]) null;
    List<LevelScoreResult.GameplayModifiers> gameplayModifiersList = new List<LevelScoreResult.GameplayModifiers>();
    if (gameplayModifiers.noFailOn0Energy)
      gameplayModifiersList.Add(LevelScoreResult.GameplayModifiers.NoFailOn0Energy);
    if (gameplayModifiers.instaFail)
      gameplayModifiersList.Add(LevelScoreResult.GameplayModifiers.InstaFail);
    if (gameplayModifiers.failOnSaberClash)
      gameplayModifiersList.Add(LevelScoreResult.GameplayModifiers.FailOnSaberClash);
    if (gameplayModifiers.fastNotes)
      gameplayModifiersList.Add(LevelScoreResult.GameplayModifiers.FastNotes);
    if (gameplayModifiers.disappearingArrows)
      gameplayModifiersList.Add(LevelScoreResult.GameplayModifiers.DisappearingArrows);
    if (gameplayModifiers.noBombs)
      gameplayModifiersList.Add(LevelScoreResult.GameplayModifiers.NoBombs);
    if (gameplayModifiers.songSpeed == GameplayModifiers.SongSpeed.Faster)
      gameplayModifiersList.Add(LevelScoreResult.GameplayModifiers.SongSpeedFaster);
    else if (gameplayModifiers.songSpeed == GameplayModifiers.SongSpeed.Slower)
      gameplayModifiersList.Add(LevelScoreResult.GameplayModifiers.SongSpeedSlower);
    else if (gameplayModifiers.songSpeed == GameplayModifiers.SongSpeed.SuperFast)
      gameplayModifiersList.Add(LevelScoreResult.GameplayModifiers.SongSpeedSuperFast);
    if (gameplayModifiers.enabledObstacleType == GameplayModifiers.EnabledObstacleType.FullHeightOnly)
      gameplayModifiersList.Add(LevelScoreResult.GameplayModifiers.EnabledObstacleTypeFullHeightOnly);
    else if (gameplayModifiers.enabledObstacleType == GameplayModifiers.EnabledObstacleType.NoObstacles)
      gameplayModifiersList.Add(LevelScoreResult.GameplayModifiers.EnabledObstacleTypeNoObstacles);
    if (gameplayModifiers.energyType == GameplayModifiers.EnergyType.Battery)
      gameplayModifiersList.Add(LevelScoreResult.GameplayModifiers.EnergyTypeBattery);
    if (gameplayModifiers.strictAngles)
      gameplayModifiersList.Add(LevelScoreResult.GameplayModifiers.StrictAngles);
    if (gameplayModifiers.noArrows)
      gameplayModifiersList.Add(LevelScoreResult.GameplayModifiers.NoArrows);
    if (gameplayModifiers.ghostNotes)
      gameplayModifiersList.Add(LevelScoreResult.GameplayModifiers.GhostNotes);
    if (gameplayModifiers.proMode)
      gameplayModifiersList.Add(LevelScoreResult.GameplayModifiers.ProMode);
    if (gameplayModifiers.zenMode)
      gameplayModifiersList.Add(LevelScoreResult.GameplayModifiers.ZenMode);
    if (gameplayModifiers.smallCubes)
      gameplayModifiersList.Add(LevelScoreResult.GameplayModifiers.SmallCubes);
    return gameplayModifiersList.ToArray();
  }

  public static GameplayModifiers FromDTO(
    LevelScoreResult.GameplayModifiers[] gameplayModifiersDTOs)
  {
    GameplayModifiers noModifiers = GameplayModifiers.noModifiers;
    HashSet<LevelScoreResult.GameplayModifiers> gameplayModifiersSet = new HashSet<LevelScoreResult.GameplayModifiers>((IEnumerable<LevelScoreResult.GameplayModifiers>) gameplayModifiersDTOs);
    GameplayModifiers.SongSpeed songSpeed1 = GameplayModifiers.SongSpeed.Normal;
    if (gameplayModifiersSet.Contains(LevelScoreResult.GameplayModifiers.SongSpeedFaster))
      songSpeed1 = GameplayModifiers.SongSpeed.Faster;
    else if (gameplayModifiersSet.Contains(LevelScoreResult.GameplayModifiers.SongSpeedSlower))
      songSpeed1 = GameplayModifiers.SongSpeed.Slower;
    else if (gameplayModifiersSet.Contains(LevelScoreResult.GameplayModifiers.SongSpeedSuperFast))
      songSpeed1 = GameplayModifiers.SongSpeed.SuperFast;
    bool noFailOn0Energy = gameplayModifiersSet.Contains(LevelScoreResult.GameplayModifiers.NoFailOn0Energy);
    bool instaFail = gameplayModifiersSet.Contains(LevelScoreResult.GameplayModifiers.InstaFail);
    bool failOnSaberClash = gameplayModifiersSet.Contains(LevelScoreResult.GameplayModifiers.FailOnSaberClash);
    bool noBombs = gameplayModifiersSet.Contains(LevelScoreResult.GameplayModifiers.NoBombs);
    bool fastNotes = gameplayModifiersSet.Contains(LevelScoreResult.GameplayModifiers.FastNotes);
    bool disappearingArrows = gameplayModifiersSet.Contains(LevelScoreResult.GameplayModifiers.DisappearingArrows);
    GameplayModifiers.SongSpeed songSpeed2 = songSpeed1;
    GameplayModifiers.EnabledObstacleType enabledObstacleType = gameplayModifiersSet.Contains(LevelScoreResult.GameplayModifiers.EnabledObstacleTypeFullHeightOnly) ? GameplayModifiers.EnabledObstacleType.FullHeightOnly : (gameplayModifiersSet.Contains(LevelScoreResult.GameplayModifiers.EnabledObstacleTypeNoObstacles) ? GameplayModifiers.EnabledObstacleType.NoObstacles : GameplayModifiers.EnabledObstacleType.All);
    return new GameplayModifiers(gameplayModifiersSet.Contains(LevelScoreResult.GameplayModifiers.EnergyTypeBattery) ? GameplayModifiers.EnergyType.Battery : GameplayModifiers.EnergyType.Bar, noFailOn0Energy, instaFail, failOnSaberClash, enabledObstacleType, noBombs, fastNotes, gameplayModifiersSet.Contains(LevelScoreResult.GameplayModifiers.StrictAngles), disappearingArrows, songSpeed2, gameplayModifiersSet.Contains(LevelScoreResult.GameplayModifiers.NoArrows), gameplayModifiersSet.Contains(LevelScoreResult.GameplayModifiers.GhostNotes), gameplayModifiersSet.Contains(LevelScoreResult.GameplayModifiers.ProMode), gameplayModifiersSet.Contains(LevelScoreResult.GameplayModifiers.ZenMode), gameplayModifiersSet.Contains(LevelScoreResult.GameplayModifiers.SmallCubes));
  }
}
