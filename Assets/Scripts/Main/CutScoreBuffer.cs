// Decompiled with JetBrains decompiler
// Type: CutScoreBuffer
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class CutScoreBuffer : 
  IReadonlyCutScoreBuffer,
  ISaberSwingRatingCounterDidChangeReceiver,
  ISaberSwingRatingCounterDidFinishReceiver
{
  protected readonly SaberSwingRatingCounter _saberSwingRatingCounter = new SaberSwingRatingCounter();
  protected NoteCutInfo _noteCutInfo;
  protected ScoreModel.NoteScoreDefinition _noteScoreDefinition;
  protected int _afterCutScore;
  protected int _beforeCutScore;
  protected int _centerDistanceCutScore;
  protected bool _initialized;
  protected bool _isFinished;
  protected readonly LazyCopyHashSet<ICutScoreBufferDidFinishReceiver> _didFinishEvent = new LazyCopyHashSet<ICutScoreBufferDidFinishReceiver>();
  protected readonly LazyCopyHashSet<ICutScoreBufferDidChangeReceiver> _didChangeEvent = new LazyCopyHashSet<ICutScoreBufferDidChangeReceiver>();

  public int executionOrder => this._noteScoreDefinition.executionOrder;

  public int maxPossibleCutScore => this._noteScoreDefinition.maxCutScore;

  public bool isFinished => this._isFinished;

  public int cutScore => this._afterCutScore + this._beforeCutScore + this._centerDistanceCutScore + this._noteScoreDefinition.fixedCutScore;

  public int beforeCutScore => this._beforeCutScore;

  public int centerDistanceCutScore => this._centerDistanceCutScore;

  public int afterCutScore => this._afterCutScore;

  public ScoreModel.NoteScoreDefinition noteScoreDefinition => this._noteScoreDefinition;

  public NoteCutInfo noteCutInfo => this._noteCutInfo;

  public float beforeCutSwingRating => this._saberSwingRatingCounter.beforeCutRating;

  public float afterCutSwingRating => this._saberSwingRatingCounter.afterCutRating;

  public virtual void RegisterDidChangeReceiver(ICutScoreBufferDidChangeReceiver receiver) => this._didChangeEvent.Add(receiver);

  public virtual void RegisterDidFinishReceiver(ICutScoreBufferDidFinishReceiver receiver) => this._didFinishEvent.Add(receiver);

  public virtual void UnregisterDidChangeReceiver(ICutScoreBufferDidChangeReceiver receiver) => this._didChangeEvent.Remove(receiver);

  public virtual void UnregisterDidFinishReceiver(ICutScoreBufferDidFinishReceiver receiver) => this._didFinishEvent.Remove(receiver);

  public virtual bool Init(in NoteCutInfo noteCutInfo)
  {
    this._initialized = true;
    this._isFinished = false;
    this._noteCutInfo = noteCutInfo;
    this._noteScoreDefinition = ScoreModel.GetNoteScoreDefinition(noteCutInfo.noteData.scoringType);
    this._centerDistanceCutScore = Mathf.RoundToInt((float) this._noteScoreDefinition.maxCenterDistanceCutScore * (1f - Mathf.Clamp01(noteCutInfo.cutDistanceToCenter / 0.3f)));
    bool rateBeforeCut = this._noteScoreDefinition.maxBeforeCutScore > 0 && this._noteScoreDefinition.minBeforeCutScore != this._noteScoreDefinition.maxBeforeCutScore;
    bool rateAfterCut = this._noteScoreDefinition.maxAfterCutScore > 0 && this._noteScoreDefinition.minAfterCutScore != this._noteScoreDefinition.maxAfterCutScore;
    this._saberSwingRatingCounter.Init(noteCutInfo.saberMovementData, noteCutInfo.notePosition, noteCutInfo.noteRotation, rateBeforeCut, rateAfterCut);
    this.RefreshScores();
    if (rateBeforeCut | rateAfterCut)
    {
      this._saberSwingRatingCounter.RegisterDidChangeReceiver((ISaberSwingRatingCounterDidChangeReceiver) this);
      this._saberSwingRatingCounter.RegisterDidFinishReceiver((ISaberSwingRatingCounterDidFinishReceiver) this);
      return true;
    }
    this._initialized = false;
    this._isFinished = true;
    this._saberSwingRatingCounter.Finish();
    return false;
  }

  public virtual void RefreshScores()
  {
    this._beforeCutScore = Mathf.RoundToInt(Mathf.LerpUnclamped((float) this._noteScoreDefinition.minBeforeCutScore, (float) this._noteScoreDefinition.maxBeforeCutScore, this._saberSwingRatingCounter.beforeCutRating));
    this._afterCutScore = Mathf.RoundToInt(Mathf.LerpUnclamped((float) this._noteScoreDefinition.minAfterCutScore, (float) this._noteScoreDefinition.maxAfterCutScore, this._saberSwingRatingCounter.afterCutRating));
  }

  public virtual void HandleSaberSwingRatingCounterDidChange(
    ISaberSwingRatingCounter swingRatingCounter,
    float rating)
  {
    this.RefreshScores();
    foreach (ICutScoreBufferDidChangeReceiver didChangeReceiver in this._didChangeEvent.items)
      didChangeReceiver.HandleCutScoreBufferDidChange(this);
  }

  public virtual void HandleSaberSwingRatingCounterDidFinish(
    ISaberSwingRatingCounter swingRatingCounter)
  {
    this.RefreshScores();
    this._initialized = false;
    this._isFinished = true;
    swingRatingCounter.UnregisterDidChangeReceiver((ISaberSwingRatingCounterDidChangeReceiver) this);
    swingRatingCounter.UnregisterDidFinishReceiver((ISaberSwingRatingCounterDidFinishReceiver) this);
    foreach (ICutScoreBufferDidFinishReceiver didFinishReceiver in this._didFinishEvent.items)
      didFinishReceiver.HandleCutScoreBufferDidFinish(this);
  }
}
