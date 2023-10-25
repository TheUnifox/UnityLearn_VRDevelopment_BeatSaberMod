// Decompiled with JetBrains decompiler
// Type: MultiplayerLeaderboardPanelItem
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerLeaderboardPanelItem : MonoBehaviour
{
  [SerializeField]
  protected TextMeshProUGUI _playerNameText;
  [SerializeField]
  protected TextMeshProUGUI _scoreText;
  [SerializeField]
  protected TextMeshProUGUI _positionText;
  [SerializeField]
  protected Image _backgroundImage;
  [SerializeField]
  protected Color _normalPlayerTextColor;
  [SerializeField]
  protected Color _failedPlayerTextColor;
  [SerializeField]
  protected Color _firstPlayerBackgroundColor;
  [SerializeField]
  protected Color _lastPlayerBackgroundColor;
  protected int _prevPosition = -1;
  protected string _prevPlayerName;
  protected int _prevScore = -1;
  protected bool _prevFailed;
  protected int _prevNumberOfPlayers;

  public virtual void SetData(
    int position,
    string playerName,
    int score,
    bool failed,
    int numberOfPlayers)
  {
    if (this._prevPosition != position)
      this._positionText.text = position.ToString();
    if (this._prevFailed != failed || this._prevPosition != position || this._prevNumberOfPlayers != numberOfPlayers)
    {
      this._backgroundImage.color = Color.Lerp(this._firstPlayerBackgroundColor, this._lastPlayerBackgroundColor, (float) position / Mathf.Max((float) (numberOfPlayers - 1), 1f));
      TextMeshProUGUI playerNameText = this._playerNameText;
      TextMeshProUGUI positionText = this._positionText;
      Color color1;
      this._scoreText.color = color1 = failed ? this._failedPlayerTextColor : this._normalPlayerTextColor;
      Color color2;
      Color color3 = color2 = color1;
      positionText.color = color2;
      Color color4 = color3;
      playerNameText.color = color4;
    }
    if (this._prevPlayerName != playerName)
      this._playerNameText.text = playerName;
    if (this._prevScore != score || this._prevFailed != failed)
      this._scoreText.text = failed ? "F" : ScoreFormatter.Format(score);
    this._prevPosition = position;
    this._prevPlayerName = playerName;
    this._prevScore = score;
    this._prevFailed = failed;
    this._prevNumberOfPlayers = numberOfPlayers;
  }

  public bool hide
  {
    set => this.gameObject.SetActive(!value);
  }
}
