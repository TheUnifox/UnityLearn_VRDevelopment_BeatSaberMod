// Decompiled with JetBrains decompiler
// Type: MultiplayerPositionHUDController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class MultiplayerPositionHUDController : MonoBehaviour
{
  [SerializeField]
  protected TextMeshProUGUI _playerCountText;
  [SerializeField]
  protected TextMeshProUGUI _positionText;
  [SerializeField]
  protected CanvasGroup _canvasGroup;
  [SerializeField]
  protected GameObject _firstPlayerAnimationGo;
  [Inject]
  protected readonly MultiplayerScoreProvider _scoreProvider;
  [Inject]
  protected readonly MultiplayerPlayersManager _playersManager;
  [Inject]
  protected readonly CoreGameHUDController.InitData _initData;
  protected int _prevPosition = -1;

  public float alpha
  {
    set => this._canvasGroup.alpha = value;
  }

  public virtual void Start()
  {
    if (this._initData.hide)
      this.gameObject.SetActive(false);
    else
      this._playerCountText.text = string.Format("/ {0}", (object) this._playersManager.allActiveAtGameStartPlayers.Count);
  }

  public virtual void Update()
  {
    IReadOnlyList<MultiplayerScoreProvider.RankedPlayer> rankedPlayers = this._scoreProvider.rankedPlayers;
    for (int index = 0; index < rankedPlayers.Count; ++index)
    {
      if (rankedPlayers[index].isMe)
      {
        if (this._prevPosition == index)
          break;
        this._positionText.text = (index + 1).ToString();
        this._prevPosition = index;
        this._firstPlayerAnimationGo.SetActive(index == 0);
        break;
      }
    }
  }
}
