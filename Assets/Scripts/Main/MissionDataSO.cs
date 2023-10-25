// Decompiled with JetBrains decompiler
// Type: MissionDataSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class MissionDataSO : PersistentScriptableObject
{
  [SerializeField]
  protected BeatmapLevelSO _level;
  [SerializeField]
  protected BeatmapCharacteristicSO _beatmapCharacteristic;
  [SerializeField]
  protected BeatmapDifficulty _beatmapDifficulty;
  [SerializeField]
  protected MissionObjective[] _missionObjectives;
  [SerializeField]
  protected GameplayModifiers _gameplayModifiers;
  [Space]
  [SerializeField]
  [NullAllowed]
  protected MissionHelpSO _missionHelp;

  public MissionObjective[] missionObjectives => this._missionObjectives;

  public BeatmapLevelSO level => this._level;

  public BeatmapCharacteristicSO beatmapCharacteristic => this._beatmapCharacteristic;

  public BeatmapDifficulty beatmapDifficulty => this._beatmapDifficulty;

  public GameplayModifiers gameplayModifiers => this._gameplayModifiers;

  public MissionHelpSO missionHelp => this._missionHelp;

  public virtual void OnValidate()
  {
    if (!((Object) this._level != (Object) null))
      return;
    this._level.beatmapLevelData.GetDifficultyBeatmap(this._beatmapCharacteristic, this._beatmapDifficulty);
  }
}
