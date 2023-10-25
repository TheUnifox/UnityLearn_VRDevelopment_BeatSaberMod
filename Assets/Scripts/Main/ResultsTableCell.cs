// Decompiled with JetBrains decompiler
// Type: ResultsTableCell
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using TMPro;
using UnityEngine;

public class ResultsTableCell : TableCellWithSeparator
{
  [SerializeField]
  protected GameObject _border;
  [SerializeField]
  protected TextMeshProUGUI _orderText;
  [SerializeField]
  protected TextMeshProUGUI _nameText;
  [SerializeField]
  protected TextMeshProUGUI _scoreText;
  [SerializeField]
  protected TextMeshProUGUI _rankText;

  public virtual void SetData(
    int order,
    IConnectedPlayer connectedPlayer,
    LevelCompletionResults levelCompletionResults)
  {
    this._border.SetActive(connectedPlayer.isMe);
    this._orderText.text = order.ToString();
    this._nameText.text = connectedPlayer?.userName;
    this._scoreText.text = ScoreFormatter.Format(levelCompletionResults.modifiedScore);
    if (levelCompletionResults.levelEndStateType == LevelCompletionResults.LevelEndStateType.Cleared)
    {
      this._rankText.text = RankModel.GetRankName(levelCompletionResults.rank);
      this._orderText.color = this._rankText.color = this._scoreText.color = this._nameText.color = Color.white;
    }
    else
    {
      this._rankText.text = "F";
      TextMeshProUGUI orderText = this._orderText;
      TextMeshProUGUI rankText = this._rankText;
      TextMeshProUGUI scoreText = this._scoreText;
      TextMeshProUGUI nameText = this._nameText;
      Color color1 = new Color(1f, 1f, 1f, 0.5f);
      Color color2 = color1;
      nameText.color = color2;
      Color color3;
      Color color4 = color3 = color1;
      scoreText.color = color3;
      Color color5;
      Color color6 = color5 = color4;
      rankText.color = color5;
      Color color7 = color6;
      orderText.color = color7;
    }
  }
}
