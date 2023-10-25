// Decompiled with JetBrains decompiler
// Type: LeaderboardEntry
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using TMPro;
using UnityEngine;

public class LeaderboardEntry : MonoBehaviour
{
  [SerializeField]
  protected TextMeshProUGUI _scoreText;
  [SerializeField]
  protected TextMeshProUGUI _playerNameText;
  [SerializeField]
  protected TextMeshProUGUI _rankText;
  [SerializeField]
  protected Color _color = Color.black;

  public virtual void SetScore(
    int score,
    string playerName,
    int rank,
    bool highlighted,
    bool showSeparator)
  {
    this._scoreText.text = ScoreFormatter.Format(score);
    this._playerNameText.text = playerName;
    this._rankText.text = rank.ToString();
    Color color = highlighted ? this._color : this._color * 0.5f;
    this._scoreText.color = color;
    this._playerNameText.color = color;
    this._rankText.color = color;
  }
}
