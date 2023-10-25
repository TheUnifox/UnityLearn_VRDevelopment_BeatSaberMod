// Decompiled with JetBrains decompiler
// Type: ImmediateRankUIPanel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Polyglot;
using System;
using System.Text;
using TMPro;
using UnityEngine;
using Zenject;

public class ImmediateRankUIPanel : MonoBehaviour
{
  [SerializeField]
  protected TextMeshProUGUI _rankText;
  [SerializeField]
  protected TextMeshProUGUI _relativeScoreText;
  [Inject]
  protected readonly RelativeScoreAndImmediateRankCounter _relativeScoreAndImmediateRankCounter;
  protected StringBuilder _stringBuilder;
  protected float _prevRelativeScore = -1f;
  protected RankModel.Rank _prevImmediateRank = RankModel.Rank.SSS;

  public virtual void Start()
  {
    this._stringBuilder = new StringBuilder(16);
    this.RefreshUI();
    this._relativeScoreAndImmediateRankCounter.relativeScoreOrImmediateRankDidChangeEvent += new System.Action(this.HandleRelativeScoreAndImmediateRankCounterRelativeScoreOrImmediateRankDidChange);
  }

  public virtual void HandleRelativeScoreAndImmediateRankCounterRelativeScoreOrImmediateRankDidChange() => this.RefreshUI();

  public virtual void RefreshUI()
  {
    RankModel.Rank immediateRank = this._relativeScoreAndImmediateRankCounter.immediateRank;
    if (immediateRank != this._prevImmediateRank)
    {
      this._rankText.text = RankModel.GetRankName(immediateRank);
      this._prevImmediateRank = immediateRank;
    }
    float relativeScore = this._relativeScoreAndImmediateRankCounter.relativeScore;
    if ((double) Mathf.Abs(this._prevRelativeScore - relativeScore) < 1.0 / 1000.0)
      return;
    this._stringBuilder.Remove(0, this._stringBuilder.Length);
    this._stringBuilder.AppendFormat((IFormatProvider) Localization.Instance.SelectedCultureInfo, "{0:P1}", (object) relativeScore);
    this._relativeScoreText.text = this._stringBuilder.ToString();
    this._prevRelativeScore = relativeScore;
  }
}
