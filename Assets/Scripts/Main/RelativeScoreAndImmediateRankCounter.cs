// Decompiled with JetBrains decompiler
// Type: RelativeScoreAndImmediateRankCounter
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

public class RelativeScoreAndImmediateRankCounter : MonoBehaviour
{
  [SerializeField]
  protected GameplayModifiersModelSO _gameplayModifiersModel;
  [Inject]
  protected readonly IScoreController _scoreController;
  [Inject]
  protected readonly GameplayModifiers _gameplayModifiers;
  [CompilerGenerated]
  protected float m_CrelativeScore;
  [CompilerGenerated]
  protected RankModel.Rank m_CimmediateRank;

  public event System.Action relativeScoreOrImmediateRankDidChangeEvent;

  public float relativeScore
  {
    get => this.m_CrelativeScore;
    private set => this.m_CrelativeScore = value;
  }

  public RankModel.Rank immediateRank
  {
    get => this.m_CimmediateRank;
    private set => this.m_CimmediateRank = value;
  }

  public virtual void Start()
  {
    this.immediateRank = RankModelHelper.MaxRankForGameplayModifiers(this._gameplayModifiers, this._gameplayModifiersModel, 1f);
    if (this.immediateRank == RankModel.Rank.SSS)
      this.immediateRank = RankModel.Rank.SS;
    this.relativeScore = 1f;
    this._scoreController.scoreDidChangeEvent += new System.Action<int, int>(this.HandleScoreDidChange);
    System.Action rankDidChangeEvent = this.relativeScoreOrImmediateRankDidChangeEvent;
    if (rankDidChangeEvent == null)
      return;
    rankDidChangeEvent();
  }

  public virtual void OnDestroy()
  {
    if (this._scoreController == null)
      return;
    this._scoreController.scoreDidChangeEvent -= new System.Action<int, int>(this.HandleScoreDidChange);
  }

  public virtual void HandleScoreDidChange(int scoreWithoutModifiers, int scoreWithModifiers) => this.UpdateRelativeScoreAndImmediateRank(scoreWithoutModifiers, scoreWithModifiers, this._scoreController.immediateMaxPossibleMultipliedScore, this._scoreController.immediateMaxPossibleModifiedScore);

  public virtual void UpdateRelativeScoreAndImmediateRank(
    int score,
    int modifiedScore,
    int maxPossibleScore,
    int maxPossibleModifiedScore)
  {
    this.immediateRank = RankModel.GetRankForScore(score, modifiedScore, maxPossibleScore, maxPossibleModifiedScore);
    if (this.immediateRank == RankModel.Rank.SSS)
      this.immediateRank = RankModel.Rank.SS;
    this.relativeScore = maxPossibleScore <= 0 ? 1f : (float) score / (float) maxPossibleScore;
    System.Action rankDidChangeEvent = this.relativeScoreOrImmediateRankDidChangeEvent;
    if (rankDidChangeEvent == null)
      return;
    rankDidChangeEvent();
  }
}
