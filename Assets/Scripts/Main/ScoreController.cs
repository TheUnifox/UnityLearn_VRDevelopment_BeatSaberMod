// Decompiled with JetBrains decompiler
// Type: ScoreController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ScoreController : MonoBehaviour, IScoreController
{
  [SerializeField]
  protected GameplayModifiersModelSO _gameplayModifiersModel;
  [Inject]
  protected readonly GameplayModifiers _gameplayModifiers;
  [Inject]
  protected readonly BeatmapObjectManager _beatmapObjectManager;
  [Inject]
  protected readonly IGameEnergyCounter _gameEnergyCounter;
  [Inject]
  protected readonly AudioTimeSyncController _audioTimeSyncController;
  [Inject]
  protected readonly GoodCutScoringElement.Pool _goodCutScoringElementPool;
  [Inject]
  protected readonly BadCutScoringElement.Pool _badCutScoringElementPool;
  [Inject]
  protected readonly MissScoringElement.Pool _missScoringElementPool;
  [Inject]
  protected readonly PlayerHeadAndObstacleInteraction _playerHeadAndObstacleInteraction;
  protected List<GameplayModifierParamsSO> _gameplayModifierParams;
  protected int _modifiedScore;
  protected int _multipliedScore;
  protected int _immediateMaxPossibleMultipliedScore;
  protected int _immediateMaxPossibleModifiedScore;
  protected float _prevMultiplierFromModifiers;
  protected readonly ScoreMultiplierCounter _maxScoreMultiplierCounter = new ScoreMultiplierCounter();
  protected readonly ScoreMultiplierCounter _scoreMultiplierCounter = new ScoreMultiplierCounter();
  protected readonly List<float> _sortedNoteTimesWithoutScoringElements = new List<float>(50);
  protected readonly List<ScoringElement> _sortedScoringElementsWithoutMultiplier = new List<ScoringElement>(50);
  protected readonly List<ScoringElement> _scoringElementsWithMultiplier = new List<ScoringElement>(50);
  protected readonly List<ScoringElement> _scoringElementsToRemove = new List<ScoringElement>(50);

  public event System.Action<int, int> scoreDidChangeEvent;

  public event System.Action<int, float> multiplierDidChangeEvent;

  public event System.Action<ScoringElement> scoringForNoteStartedEvent;

  public event System.Action<ScoringElement> scoringForNoteFinishedEvent;

  public int multipliedScore => this._multipliedScore;

  public int modifiedScore => this._modifiedScore;

  public int immediateMaxPossibleMultipliedScore => this._immediateMaxPossibleMultipliedScore;

  public int immediateMaxPossibleModifiedScore => this._immediateMaxPossibleModifiedScore;

  public virtual void SetEnabled(bool enabled) => this.enabled = enabled;

  public virtual void Start()
  {
    this._gameplayModifierParams = this._gameplayModifiersModel.CreateModifierParamsList(this._gameplayModifiers);
    this._prevMultiplierFromModifiers = this._gameplayModifiersModel.GetTotalMultiplier(this._gameplayModifierParams, this._gameEnergyCounter.energy);
    this._playerHeadAndObstacleInteraction.headDidEnterObstaclesEvent += new System.Action(this.HandlePlayerHeadDidEnterObstacles);
    this._beatmapObjectManager.noteWasCutEvent += new BeatmapObjectManager.NoteWasCutDelegate(this.HandleNoteWasCut);
    this._beatmapObjectManager.noteWasMissedEvent += new System.Action<NoteController>(this.HandleNoteWasMissed);
    this._beatmapObjectManager.noteWasSpawnedEvent += new System.Action<NoteController>(this.HandleNoteWasSpawned);
  }

  public virtual void OnDestroy()
  {
    if ((UnityEngine.Object) this._playerHeadAndObstacleInteraction != (UnityEngine.Object) null)
      this._playerHeadAndObstacleInteraction.headDidEnterObstaclesEvent -= new System.Action(this.HandlePlayerHeadDidEnterObstacles);
    if (this._beatmapObjectManager == null)
      return;
    this._beatmapObjectManager.noteWasCutEvent -= new BeatmapObjectManager.NoteWasCutDelegate(this.HandleNoteWasCut);
    this._beatmapObjectManager.noteWasMissedEvent -= new System.Action<NoteController>(this.HandleNoteWasMissed);
    this._beatmapObjectManager.noteWasSpawnedEvent -= new System.Action<NoteController>(this.HandleNoteWasSpawned);
  }

  public virtual void LateUpdate()
  {
    float num1 = this._sortedNoteTimesWithoutScoringElements.Count > 0 ? this._sortedNoteTimesWithoutScoringElements[0] : float.MaxValue;
    float num2 = this._audioTimeSyncController.songTime + 0.15f;
    int count = 0;
    bool flag1 = false;
    foreach (ScoringElement scoringElement in this._sortedScoringElementsWithoutMultiplier)
    {
      if ((double) scoringElement.time >= (double) num2)
      {
        if ((double) scoringElement.time <= (double) num1)
          break;
      }
      flag1 |= this._scoreMultiplierCounter.ProcessMultiplierEvent(scoringElement.multiplierEventType);
      if (scoringElement.wouldBeCorrectCutBestPossibleMultiplierEventType == ScoreMultiplierCounter.MultiplierEventType.Positive)
        this._maxScoreMultiplierCounter.ProcessMultiplierEvent(ScoreMultiplierCounter.MultiplierEventType.Positive);
      scoringElement.SetMultipliers(this._scoreMultiplierCounter.multiplier, this._maxScoreMultiplierCounter.multiplier);
      this._scoringElementsWithMultiplier.Add(scoringElement);
      ++count;
    }
    this._sortedScoringElementsWithoutMultiplier.RemoveRange(0, count);
    if (flag1)
    {
      System.Action<int, float> multiplierDidChangeEvent = this.multiplierDidChangeEvent;
      if (multiplierDidChangeEvent != null)
        multiplierDidChangeEvent(this._scoreMultiplierCounter.multiplier, this._scoreMultiplierCounter.normalizedProgress);
    }
    bool flag2 = false;
    this._scoringElementsToRemove.Clear();
    foreach (ScoringElement scoringElement in this._scoringElementsWithMultiplier)
    {
      if (scoringElement.isFinished)
      {
        if ((double) scoringElement.maxPossibleCutScore > 0.0)
        {
          flag2 = true;
          this._multipliedScore += scoringElement.cutScore * scoringElement.multiplier;
          this._immediateMaxPossibleMultipliedScore += scoringElement.maxPossibleCutScore * scoringElement.maxMultiplier;
        }
        this._scoringElementsToRemove.Add(scoringElement);
        System.Action<ScoringElement> noteFinishedEvent = this.scoringForNoteFinishedEvent;
        if (noteFinishedEvent != null)
          noteFinishedEvent(scoringElement);
      }
    }
    foreach (ScoringElement scoringElement in this._scoringElementsToRemove)
    {
      this.DespawnScoringElement(scoringElement);
      this._scoringElementsWithMultiplier.Remove(scoringElement);
    }
    this._scoringElementsToRemove.Clear();
    float totalMultiplier = this._gameplayModifiersModel.GetTotalMultiplier(this._gameplayModifierParams, this._gameEnergyCounter.energy);
    if ((double) this._prevMultiplierFromModifiers != (double) totalMultiplier)
    {
      this._prevMultiplierFromModifiers = totalMultiplier;
      flag2 = true;
    }
    if (!flag2)
      return;
    this._modifiedScore = ScoreModel.GetModifiedScoreForGameplayModifiersScoreMultiplier(this._multipliedScore, totalMultiplier);
    this._immediateMaxPossibleModifiedScore = ScoreModel.GetModifiedScoreForGameplayModifiersScoreMultiplier(this._immediateMaxPossibleMultipliedScore, totalMultiplier);
    System.Action<int, int> scoreDidChangeEvent = this.scoreDidChangeEvent;
    if (scoreDidChangeEvent == null)
      return;
    scoreDidChangeEvent(this._multipliedScore, this._modifiedScore);
  }

  public virtual void HandleNoteWasSpawned(NoteController noteController)
  {
    if (noteController.noteData.scoringType == NoteData.ScoringType.Ignore)
      return;
    this._sortedNoteTimesWithoutScoringElements.InsertIntoSortedListFromEnd<float>(noteController.noteData.time);
  }

  public virtual void HandleNoteWasCut(NoteController noteController, in NoteCutInfo noteCutInfo)
  {
    if (noteCutInfo.noteData.scoringType == NoteData.ScoringType.Ignore)
      return;
    if (noteCutInfo.allIsOK)
    {
      GoodCutScoringElement newItem = this._goodCutScoringElementPool.Spawn();
      newItem.Init(noteCutInfo);
      this._sortedScoringElementsWithoutMultiplier.InsertIntoSortedListFromEnd<ScoringElement>((ScoringElement) newItem);
      System.Action<ScoringElement> noteStartedEvent = this.scoringForNoteStartedEvent;
      if (noteStartedEvent != null)
        noteStartedEvent((ScoringElement) newItem);
      this._sortedNoteTimesWithoutScoringElements.Remove(noteCutInfo.noteData.time);
    }
    else
    {
      BadCutScoringElement newItem = this._badCutScoringElementPool.Spawn();
      newItem.Init(noteCutInfo.noteData);
      this._sortedScoringElementsWithoutMultiplier.InsertIntoSortedListFromEnd<ScoringElement>((ScoringElement) newItem);
      System.Action<ScoringElement> noteStartedEvent = this.scoringForNoteStartedEvent;
      if (noteStartedEvent != null)
        noteStartedEvent((ScoringElement) newItem);
      this._sortedNoteTimesWithoutScoringElements.Remove(noteCutInfo.noteData.time);
    }
  }

  public virtual void HandleNoteWasMissed(NoteController noteController)
  {
    NoteData noteData = noteController.noteData;
    if (noteData.scoringType == NoteData.ScoringType.Ignore)
      return;
    MissScoringElement newItem = this._missScoringElementPool.Spawn();
    newItem.Init(noteData);
    this._sortedScoringElementsWithoutMultiplier.InsertIntoSortedListFromEnd<ScoringElement>((ScoringElement) newItem);
    System.Action<ScoringElement> noteStartedEvent = this.scoringForNoteStartedEvent;
    if (noteStartedEvent != null)
      noteStartedEvent((ScoringElement) newItem);
    this._sortedNoteTimesWithoutScoringElements.Remove(noteData.time);
  }

  public virtual void HandlePlayerHeadDidEnterObstacles()
  {
    if (!this._scoreMultiplierCounter.ProcessMultiplierEvent(ScoreMultiplierCounter.MultiplierEventType.Negative))
      return;
    System.Action<int, float> multiplierDidChangeEvent = this.multiplierDidChangeEvent;
    if (multiplierDidChangeEvent == null)
      return;
    multiplierDidChangeEvent(this._scoreMultiplierCounter.multiplier, this._scoreMultiplierCounter.normalizedProgress);
  }

  public virtual void DespawnScoringElement(ScoringElement scoringElement)
  {
    switch (scoringElement)
    {
      case GoodCutScoringElement cutScoringElement1:
        this._goodCutScoringElementPool.Despawn(cutScoringElement1);
        break;
      case BadCutScoringElement cutScoringElement2:
        this._badCutScoringElementPool.Despawn(cutScoringElement2);
        break;
      case MissScoringElement missScoringElement:
        this._missScoringElementPool.Despawn(missScoringElement);
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }
}
