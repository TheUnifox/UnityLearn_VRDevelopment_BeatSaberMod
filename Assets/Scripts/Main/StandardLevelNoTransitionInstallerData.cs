// Decompiled with JetBrains decompiler
// Type: StandardLevelNoTransitionInstallerData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class StandardLevelNoTransitionInstallerData : ScriptableObject
{
  [SerializeField]
  protected BeatmapLevelSO _beatmapLevel;
  [SerializeField]
  protected BeatmapCharacteristicSO _beatmapCharacteristic;
  [SerializeField]
  protected BeatmapDifficulty _beatmapDifficulty;
  [SerializeField]
  protected ColorSchemeSO _colorScheme;
  [SerializeField]
  protected EnvironmentInfoSO _environmentInfo;
  [SerializeField]
  protected GameplayModifiers _gameplayModifiers;
  [SerializeField]
  protected PlayerSpecificSettings _playerSpecificSettings;
  [SerializeField]
  protected PracticeSettings _practiceSettings;
  [SerializeField]
  protected string _backButtonText;
  [SerializeField]
  protected bool _useTestNoteCutSoundEffects;

  public BeatmapLevelSO beatmapLevel
  {
    get => this._beatmapLevel;
    set => this._beatmapLevel = value;
  }

  public BeatmapCharacteristicSO beatmapCharacteristic
  {
    get => this._beatmapCharacteristic;
    set => this._beatmapCharacteristic = value;
  }

  public BeatmapDifficulty beatmapDifficulty
  {
    get => this._beatmapDifficulty;
    set => this._beatmapDifficulty = value;
  }

  public ColorSchemeSO colorScheme
  {
    get => this._colorScheme;
    set => this._colorScheme = value;
  }

  public EnvironmentInfoSO environmentInfo
  {
    get => this._environmentInfo;
    set => this._environmentInfo = value;
  }

  public GameplayModifiers gameplayModifiers
  {
    get => this._gameplayModifiers;
    set => this._gameplayModifiers = value;
  }

  public PlayerSpecificSettings playerSpecificSettings
  {
    get => this._playerSpecificSettings;
    set => this._playerSpecificSettings = value;
  }

  public PracticeSettings practiceSettings
  {
    get => this._practiceSettings;
    set => this._practiceSettings = value;
  }

  public string backButtonText
  {
    get => this._backButtonText;
    set => this._backButtonText = value;
  }

  public bool useTestNoteCutSoundEffects
  {
    get => this._useTestNoteCutSoundEffects;
    set => this._useTestNoteCutSoundEffects = value;
  }
}
