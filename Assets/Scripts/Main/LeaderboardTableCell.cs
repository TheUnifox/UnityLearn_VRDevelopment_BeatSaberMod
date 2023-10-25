// Decompiled with JetBrains decompiler
// Type: LeaderboardTableCell
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardTableCell : TableCell
{
  [SerializeField]
  protected TextMeshProUGUI _rankText;
  [SerializeField]
  protected TextMeshProUGUI _playerNameText;
  [SerializeField]
  protected TextMeshProUGUI _scoreText;
  [SerializeField]
  protected TextMeshProUGUI _fullComboText;
  [SerializeField]
  protected Color _normalColor;
  [SerializeField]
  protected Color _specialScoreColor;
  [SerializeField]
  protected Image _separatorImage;

  public int rank
  {
    set => this._rankText.text = value.ToString();
  }

  public string playerName
  {
    set => this._playerNameText.text = value;
  }

  public int score
  {
    set => this._scoreText.text = value >= 0 ? ScoreFormatter.Format(value) : "";
  }

  public bool showSeparator
  {
    set => this._separatorImage.enabled = value;
  }

  public bool showFullCombo
  {
    set => this._fullComboText.enabled = value;
  }

  public bool specialScore
  {
    set
    {
      Color color = value ? this._specialScoreColor : this._normalColor;
      this._scoreText.color = color;
      this._playerNameText.color = color;
      this._rankText.color = color;
      this._fullComboText.color = color;
    }
  }
}
