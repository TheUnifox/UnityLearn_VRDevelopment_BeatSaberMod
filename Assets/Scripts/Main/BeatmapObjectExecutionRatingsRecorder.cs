// Decompiled with JetBrains decompiler
// Type: BeatmapObjectExecutionRatingsRecorder
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BeatmapObjectExecutionRatingsRecorder : MonoBehaviour
{
  [Inject]
  protected readonly IScoreController _scoreController;
  [Inject]
  protected readonly BeatmapObjectManager _beatmapObjectManager;
  [Inject]
  protected readonly PlayerHeadAndObstacleInteraction _playerHeadAndObstacleInteraction;
  [Inject]
  protected readonly AudioTimeSyncController _audioTimeSyncController;
  protected readonly List<BeatmapObjectExecutionRating> _beatmapObjectExecutionRatings = new List<BeatmapObjectExecutionRating>(3000);
  protected readonly HashSet<ObstacleController> _hitObstacles = new HashSet<ObstacleController>();

  public List<BeatmapObjectExecutionRating> beatmapObjectExecutionRatings => this._beatmapObjectExecutionRatings;

  public virtual void Start()
  {
    this._playerHeadAndObstacleInteraction.headDidEnterObstacleEvent += new System.Action<ObstacleController>(this.HandlePlayerHeadDidEnterObstacle);
    this._beatmapObjectManager.obstacleDidPassAvoidedMarkEvent += new System.Action<ObstacleController>(this.HandleObstacleDidPassAvoidedMark);
    this._scoreController.scoringForNoteFinishedEvent += new System.Action<ScoringElement>(this.HandleScoringForNoteDidFinish);
  }

  public virtual void OnDestroy()
  {
    if ((UnityEngine.Object) this._playerHeadAndObstacleInteraction != (UnityEngine.Object) null)
      this._playerHeadAndObstacleInteraction.headDidEnterObstacleEvent -= new System.Action<ObstacleController>(this.HandlePlayerHeadDidEnterObstacle);
    if (this._beatmapObjectManager != null)
      this._beatmapObjectManager.obstacleDidPassAvoidedMarkEvent -= new System.Action<ObstacleController>(this.HandleObstacleDidPassAvoidedMark);
    if (this._scoreController == null)
      return;
    this._scoreController.scoringForNoteFinishedEvent -= new System.Action<ScoringElement>(this.HandleScoringForNoteDidFinish);
  }

  public virtual void HandleScoringForNoteDidFinish(ScoringElement scoringElement)
  {
    NoteData noteData = scoringElement.noteData;
    if (noteData.colorType == ColorType.None)
    {
      BombExecutionRating.Rating rating = scoringElement is MissScoringElement ? BombExecutionRating.Rating.OK : BombExecutionRating.Rating.NotGood;
      this._beatmapObjectExecutionRatings.Add((BeatmapObjectExecutionRating) new BombExecutionRating(noteData.time, rating));
    }
    else
    {
      switch (scoringElement)
      {
        case GoodCutScoringElement cutScoringElement:
          IReadonlyCutScoreBuffer cutScoreBuffer = cutScoringElement.cutScoreBuffer;
          this._beatmapObjectExecutionRatings.Add((BeatmapObjectExecutionRating) new NoteExecutionRating(noteData.time, noteData.scoringType, NoteExecutionRating.Rating.GoodCut, scoringElement.cutScore, cutScoreBuffer.beforeCutScore, cutScoreBuffer.centerDistanceCutScore, cutScoreBuffer.afterCutScore));
          break;
        case BadCutScoringElement _:
          this._beatmapObjectExecutionRatings.Add((BeatmapObjectExecutionRating) new NoteExecutionRating(noteData.time, noteData.scoringType, NoteExecutionRating.Rating.BadCut, 0, 0, 0, 0));
          break;
        case MissScoringElement _:
          this._beatmapObjectExecutionRatings.Add((BeatmapObjectExecutionRating) new NoteExecutionRating(noteData.time, noteData.scoringType, NoteExecutionRating.Rating.Miss, 0, 0, 0, 0));
          break;
      }
    }
  }

  public virtual void HandlePlayerHeadDidEnterObstacle(ObstacleController obstacleController)
  {
    this._hitObstacles.Add(obstacleController);
    this._beatmapObjectExecutionRatings.Add((BeatmapObjectExecutionRating) new ObstacleExecutionRating(this._audioTimeSyncController.songTime, ObstacleExecutionRating.Rating.NotGood));
  }

  public virtual void HandleObstacleDidPassAvoidedMark(ObstacleController obstacleController)
  {
    if (this._hitObstacles.Contains(obstacleController))
      this._hitObstacles.Remove(obstacleController);
    else
      this._beatmapObjectExecutionRatings.Add((BeatmapObjectExecutionRating) new ObstacleExecutionRating(obstacleController.obstacleData.time + obstacleController.obstacleData.duration, ObstacleExecutionRating.Rating.OK));
  }
}
