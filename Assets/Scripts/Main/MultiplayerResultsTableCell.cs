// Decompiled with JetBrains decompiler
// Type: MultiplayerResultsTableCell
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerResultsTableCell : TableCell
{
  [SerializeField]
  protected TextMeshProUGUI _positionText;
  [SerializeField]
  protected TextMeshProUGUI _nameText;
  [SerializeField]
  protected TextMeshProUGUI _scoreText;
  [SerializeField]
  protected TextMeshProUGUI _rankText;
  [SerializeField]
  protected TubeBloomPrePassLight[] _lights;
  [SerializeField]
  protected Image _backgroundImage;
  [SerializeField]
  protected CanvasGroup _canvasGroup;
  [SerializeField]
  protected MultiplayerResultsAvatarController _multiplayerResultsAvatarController;
  [Space]
  [SerializeField]
  protected float _avatarScale = 1f;
  [SerializeField]
  protected Color _normalSecondPlayerColor;
  [SerializeField]
  protected Color _normalLastPlayerColor;
  [SerializeField]
  protected Color _localPlayerColor;
  [SerializeField]
  protected Color _winnerColor;
  [SerializeField]
  protected Color _lightColor;

  public float alpha
  {
    set
    {
      this._canvasGroup.alpha = value;
      foreach (TubeBloomPrePassLight light in this._lights)
        light.color = light.color.ColorWithAlpha(value);
      this._multiplayerResultsAvatarController.SetScale(this._avatarScale * value);
    }
  }

  public virtual void SetData(
    IConnectedPlayer connectedPlayer,
    int position,
    string playerName,
    LevelCompletionResults levelCompletionResults,
    bool isLocalPlayer,
    int numberOfPlayers)
  {
    this._positionText.text = position.ToString();
    this._nameText.text = playerName;
    this._scoreText.text = ScoreFormatter.Format(levelCompletionResults.modifiedScore);
    if (levelCompletionResults.levelEndStateType == LevelCompletionResults.LevelEndStateType.Cleared)
    {
      this._rankText.text = RankModel.GetRankName(levelCompletionResults.rank);
      this._positionText.color = this._rankText.color = this._scoreText.color = this._nameText.color = Color.white;
    }
    else
    {
      this._rankText.text = "F";
      TextMeshProUGUI positionText = this._positionText;
      TextMeshProUGUI rankText = this._rankText;
      TextMeshProUGUI scoreText = this._scoreText;
      TextMeshProUGUI nameText = this._nameText;
      Color color1 = new Color(1f, 1f, 1f, 0.25f);
      Color color2 = color1;
      nameText.color = color2;
      Color color3;
      Color color4 = color3 = color1;
      scoreText.color = color3;
      Color color5;
      Color color6 = color5 = color4;
      rankText.color = color5;
      Color color7 = color6;
      positionText.color = color7;
    }
    if (position == 1)
      this._backgroundImage.color = this._winnerColor;
    else if (isLocalPlayer)
      this._backgroundImage.color = this._localPlayerColor;
    else
      this._backgroundImage.color = Color.Lerp(this._normalSecondPlayerColor, this._normalLastPlayerColor, ((float) position - 2f) / Mathf.Max(1f, (float) numberOfPlayers - 2f));
    foreach (TubeBloomPrePassLight light in this._lights)
      light.color = this._lightColor;
    this._multiplayerResultsAvatarController.Setup(connectedPlayer);
  }
}
