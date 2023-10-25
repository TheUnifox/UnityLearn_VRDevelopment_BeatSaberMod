// Decompiled with JetBrains decompiler
// Type: GameplayModifiersModelSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class GameplayModifiersModelSO : PersistentScriptableObject
{
  [SerializeField]
  protected GameplayModifierParamsSO _batteryEnergy;
  [SerializeField]
  protected GameplayModifierParamsSO _instaFail;
  [SerializeField]
  protected GameplayModifierParamsSO _noObstacles;
  [SerializeField]
  protected GameplayModifierParamsSO _noBombs;
  [SerializeField]
  protected GameplayModifierParamsSO _fastNotes;
  [SerializeField]
  protected GameplayModifierParamsSO _strictAngles;
  [SerializeField]
  protected GameplayModifierParamsSO _disappearingArrows;
  [SerializeField]
  protected GameplayModifierParamsSO _fasterSong;
  [SerializeField]
  protected GameplayModifierParamsSO _slowerSong;
  [SerializeField]
  protected GameplayModifierParamsSO _noArrows;
  [SerializeField]
  protected GameplayModifierParamsSO _ghostNotes;
  [SerializeField]
  protected GameplayModifierParamsSO _noFailOn0Energy;
  [SerializeField]
  protected GameplayModifierParamsSO _superFastSong;
  [SerializeField]
  protected GameplayModifierParamsSO _proMode;
  [SerializeField]
  protected GameplayModifierParamsSO _zenMode;
  [SerializeField]
  protected GameplayModifierParamsSO _smallCubes;
  protected Dictionary<GameplayModifierParamsSO, GameplayModifiersModelSO.GameplayModifierBoolGetter> _gameplayModifierGetters;

  protected override void OnEnable()
  {
    base.OnEnable();
    this._gameplayModifierGetters = new Dictionary<GameplayModifierParamsSO, GameplayModifiersModelSO.GameplayModifierBoolGetter>()
    {
      {
        this._batteryEnergy,
        (GameplayModifiersModelSO.GameplayModifierBoolGetter) (gameplayModifiers => gameplayModifiers.energyType == GameplayModifiers.EnergyType.Battery)
      },
      {
        this._noFailOn0Energy,
        (GameplayModifiersModelSO.GameplayModifierBoolGetter) (gameplayModifiers => gameplayModifiers.noFailOn0Energy)
      },
      {
        this._instaFail,
        (GameplayModifiersModelSO.GameplayModifierBoolGetter) (gameplayModifiers => gameplayModifiers.instaFail)
      },
      {
        this._noObstacles,
        (GameplayModifiersModelSO.GameplayModifierBoolGetter) (gameplayModifiers => gameplayModifiers.enabledObstacleType == GameplayModifiers.EnabledObstacleType.NoObstacles)
      },
      {
        this._noBombs,
        (GameplayModifiersModelSO.GameplayModifierBoolGetter) (gameplayModifiers => gameplayModifiers.noBombs)
      },
      {
        this._fastNotes,
        (GameplayModifiersModelSO.GameplayModifierBoolGetter) (gameplayModifiers => gameplayModifiers.fastNotes)
      },
      {
        this._strictAngles,
        (GameplayModifiersModelSO.GameplayModifierBoolGetter) (gameplayModifiers => gameplayModifiers.strictAngles)
      },
      {
        this._disappearingArrows,
        (GameplayModifiersModelSO.GameplayModifierBoolGetter) (gameplayModifiers => gameplayModifiers.disappearingArrows)
      },
      {
        this._fasterSong,
        (GameplayModifiersModelSO.GameplayModifierBoolGetter) (gameplayModifiers => gameplayModifiers.songSpeed == GameplayModifiers.SongSpeed.Faster)
      },
      {
        this._slowerSong,
        (GameplayModifiersModelSO.GameplayModifierBoolGetter) (gameplayModifiers => gameplayModifiers.songSpeed == GameplayModifiers.SongSpeed.Slower)
      },
      {
        this._superFastSong,
        (GameplayModifiersModelSO.GameplayModifierBoolGetter) (gameplayModifiers => gameplayModifiers.songSpeed == GameplayModifiers.SongSpeed.SuperFast)
      },
      {
        this._noArrows,
        (GameplayModifiersModelSO.GameplayModifierBoolGetter) (gameplayModifiers => gameplayModifiers.noArrows)
      },
      {
        this._ghostNotes,
        (GameplayModifiersModelSO.GameplayModifierBoolGetter) (gameplayModifiers => gameplayModifiers.ghostNotes)
      },
      {
        this._proMode,
        (GameplayModifiersModelSO.GameplayModifierBoolGetter) (gameplayModifiers => gameplayModifiers.proMode)
      },
      {
        this._zenMode,
        (GameplayModifiersModelSO.GameplayModifierBoolGetter) (gameplayModifiers => gameplayModifiers.zenMode)
      },
      {
        this._smallCubes,
        (GameplayModifiersModelSO.GameplayModifierBoolGetter) (gameplayModifiers => gameplayModifiers.smallCubes)
      }
    };
  }

  public virtual GameplayModifiers CreateGameplayModifiers(
    Func<GameplayModifierParamsSO, bool> valueGetter)
  {
    return new GameplayModifiers(valueGetter(this._batteryEnergy) ? GameplayModifiers.EnergyType.Battery : GameplayModifiers.EnergyType.Bar, valueGetter(this._noFailOn0Energy), valueGetter(this._instaFail), false, valueGetter(this._noObstacles) ? GameplayModifiers.EnabledObstacleType.NoObstacles : GameplayModifiers.EnabledObstacleType.All, valueGetter(this._noBombs), valueGetter(this._fastNotes), valueGetter(this._strictAngles), valueGetter(this._disappearingArrows), this.GetSongSpeedFromValueGetter(valueGetter), valueGetter(this._noArrows), valueGetter(this._ghostNotes), valueGetter(this._proMode), valueGetter(this._zenMode), valueGetter(this._smallCubes));
  }

  public virtual bool GetModifierBoolValue(
    GameplayModifiers gameplayModifiers,
    GameplayModifierParamsSO gameplayModifierParams)
  {
    GameplayModifiersModelSO.GameplayModifierBoolGetter modifierBoolGetter;
    return this._gameplayModifierGetters.TryGetValue(gameplayModifierParams, out modifierBoolGetter) && modifierBoolGetter(gameplayModifiers);
  }

  public virtual List<GameplayModifierParamsSO> CreateModifierParamsList(
    GameplayModifiers gameplayModifiers)
  {
    List<GameplayModifierParamsSO> modifierParamsList = new List<GameplayModifierParamsSO>(12);
    foreach (KeyValuePair<GameplayModifierParamsSO, GameplayModifiersModelSO.GameplayModifierBoolGetter> gameplayModifierGetter in this._gameplayModifierGetters)
    {
      GameplayModifiersModelSO.GameplayModifierBoolGetter modifierBoolGetter;
      if (this._gameplayModifierGetters.TryGetValue(gameplayModifierGetter.Key, out modifierBoolGetter) && modifierBoolGetter(gameplayModifiers))
        modifierParamsList.Add(gameplayModifierGetter.Key);
    }
    return modifierParamsList;
  }

  public virtual float GetTotalMultiplier(
    List<GameplayModifierParamsSO> modifierParams,
    float energy)
  {
    float totalMultiplier = 1f;
    foreach (GameplayModifierParamsSO modifierParam in modifierParams)
    {
      if ((UnityEngine.Object) modifierParam == (UnityEngine.Object) this._noFailOn0Energy)
      {
        if ((double) energy <= 9.9999997473787516E-06)
          totalMultiplier += modifierParam.multiplier;
      }
      else
        totalMultiplier += modifierParam.multiplier;
    }
    if ((double) totalMultiplier < 0.0)
      totalMultiplier = 0.0f;
    return totalMultiplier;
  }

  public virtual int MaxModifiedScoreForMaxMultipliedScore(
    int maxMultipliedScore,
    List<GameplayModifierParamsSO> modifierParams,
    float energy)
  {
    return this.GetModifiedScoreForGameplayModifiers(maxMultipliedScore, modifierParams, energy);
  }

  public virtual int MaxModifiedScoreForMaxMultipliedScore(
    int maxMultipliedScore,
    List<GameplayModifierParamsSO> modifierParams,
    GameplayModifiersModelSO gameplayModifiersModel,
    float energy)
  {
    return this.GetModifiedScoreForGameplayModifiers(maxMultipliedScore, modifierParams, energy);
  }

  public virtual int GetModifiedScoreForGameplayModifiers(
    int multipliedScore,
    List<GameplayModifierParamsSO> modifierParams,
    float energy)
  {
    float totalMultiplier = this.GetTotalMultiplier(modifierParams, energy);
    return ScoreModel.GetModifiedScoreForGameplayModifiersScoreMultiplier(multipliedScore, totalMultiplier);
  }

  public virtual GameplayModifierParamsSO GetGameplayModifierParams(GameplayModifierMask modifier)
  {
    if ((uint) modifier <= 256U)
    {
      if ((uint) modifier <= 16U)
      {
        switch (modifier)
        {
          case GameplayModifierMask.BatteryEnergy:
            return this._batteryEnergy;
          case GameplayModifierMask.NoFail:
            return this._noFailOn0Energy;
          case GameplayModifierMask.InstaFail:
            return this._instaFail;
          case GameplayModifierMask.NoObstacles:
            return this._noObstacles;
          case GameplayModifierMask.NoBombs:
            return this._noBombs;
        }
      }
      else if ((uint) modifier <= 64U)
      {
        if (modifier == GameplayModifierMask.FastNotes)
          return this._fastNotes;
        if (modifier == GameplayModifierMask.StrictAngles)
          return this._strictAngles;
      }
      else
      {
        if (modifier == GameplayModifierMask.DisappearingArrows)
          return this._disappearingArrows;
        if (modifier == GameplayModifierMask.FasterSong)
          return this._fasterSong;
      }
    }
    else if ((uint) modifier <= 2048U)
    {
      switch (modifier)
      {
        case GameplayModifierMask.SlowerSong:
          return this._slowerSong;
        case GameplayModifierMask.NoArrows:
          return this._noArrows;
        case GameplayModifierMask.GhostNotes:
          return this._ghostNotes;
      }
    }
    else if ((uint) modifier <= 8192U)
    {
      if (modifier == GameplayModifierMask.SuperFastSong)
        return this._superFastSong;
      if (modifier == GameplayModifierMask.ProMode)
        return this._proMode;
    }
    else
    {
      if (modifier == GameplayModifierMask.ZenMode)
        return this._zenMode;
      if (modifier == GameplayModifierMask.SmallCubes)
        return this._smallCubes;
    }
    return (GameplayModifierParamsSO) null;
  }

  public virtual GameplayModifiers.SongSpeed GetSongSpeedFromValueGetter(
    Func<GameplayModifierParamsSO, bool> valueGetter)
  {
    if (valueGetter(this._superFastSong))
      return GameplayModifiers.SongSpeed.SuperFast;
    if (valueGetter(this._fasterSong))
      return GameplayModifiers.SongSpeed.Faster;
    return valueGetter(this._slowerSong) ? GameplayModifiers.SongSpeed.Slower : GameplayModifiers.SongSpeed.Normal;
  }

  public delegate bool GameplayModifierBoolGetter(GameplayModifiers gameplayModifiers);
}
