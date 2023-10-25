// Decompiled with JetBrains decompiler
// Type: PrepareLevelCompletionResults
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class PrepareLevelCompletionResults : MonoBehaviour
{
  [SerializeField]
  protected GameplayModifiersModelSO _gameplayModifiersModelSO;
  [Inject]
  protected readonly SaberActivityCounter _saberActivityCounter;
  [Inject]
  protected readonly BeatmapObjectExecutionRatingsRecorder _beatmapObjectExecutionRatingsRecorder;
  [Inject]
  protected readonly IScoreController _scoreController;
  [Inject]
  protected readonly GameEnergyCounter _gameEnergyCounter;
  [Inject]
  protected readonly IReadonlyBeatmapData _beatmapData;
  [Inject]
  protected readonly AudioTimeSyncController _audioTimeSyncController;
  [Inject]
  protected readonly GameplayModifiers _gameplayModifiers;
  [Inject]
  protected readonly ComboController _comboController;

  public virtual LevelCompletionResults FillLevelCompletionResults(
    LevelCompletionResults.LevelEndStateType levelEndStateType,
    LevelCompletionResults.LevelEndAction levelEndAction)
  {
    BeatmapObjectExecutionRating[] array1 = this._beatmapObjectExecutionRatingsRecorder.beatmapObjectExecutionRatings.ToArray();
    int multipliedScore = this._scoreController.multipliedScore;
    int modifiedScore = this._scoreController.modifiedScore;
    int maxCombo = this._comboController.maxCombo;
    float[] array2 = this._saberActivityCounter.saberMovementAveragingValueRecorder.GetHistoryValues().ToArray();
    float movementDistance1 = this._saberActivityCounter.leftSaberMovementDistance;
    float movementDistance2 = this._saberActivityCounter.rightSaberMovementDistance;
    float[] array3 = this._saberActivityCounter.handMovementAveragingValueRecorder.GetHistoryValues().ToArray();
    float movementDistance3 = this._saberActivityCounter.leftHandMovementDistance;
    float movementDistance4 = this._saberActivityCounter.rightHandMovementDistance;
    float energy = this._gameEnergyCounter.energy;
    return LevelCompletionResultsHelper.Create(this._beatmapData, array1, this._gameplayModifiers, this._gameplayModifiersModelSO, multipliedScore, modifiedScore, maxCombo, array2, movementDistance1, movementDistance2, array3, movementDistance3, movementDistance4, levelEndStateType, levelEndAction, energy, this._audioTimeSyncController.songTime);
  }
}
