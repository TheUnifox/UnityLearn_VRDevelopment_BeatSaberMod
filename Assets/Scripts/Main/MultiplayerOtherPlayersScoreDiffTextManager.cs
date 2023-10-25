// Decompiled with JetBrains decompiler
// Type: MultiplayerOtherPlayersScoreDiffTextManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class MultiplayerOtherPlayersScoreDiffTextManager : MonoBehaviour
{
  [Inject]
  protected readonly MultiplayerController _multiplayerController;
  [Inject]
  protected readonly MultiplayerPlayersManager _playersManager;
  [Inject]
  protected readonly MultiplayerScoreProvider _scoreProvider;
  [Inject]
  protected readonly IMultiplayerSessionManager _multiplayerSessionManager;
  [Inject]
  protected readonly MultiplayerLayoutProvider _layoutProvider;
  [Inject]
  protected readonly CoreGameHUDController.InitData _initData;
  protected float _timeToNextUpdate;
  protected const float kUpdateInterval = 0.5f;

  public virtual void Start()
  {
    if (!this._initData.advancedHUD || this._initData.hide)
      this.enabled = false;
    else
      this._multiplayerController.stateChangedEvent += new System.Action<MultiplayerController.State>(this.HandleStateChanged);
  }

  public virtual void OnDestroy()
  {
    if (!((UnityEngine.Object) this._multiplayerController != (UnityEngine.Object) null))
      return;
    this._multiplayerController.stateChangedEvent -= new System.Action<MultiplayerController.State>(this.HandleStateChanged);
  }

  public virtual void Update()
  {
    IConnectedPlayer localPlayer = this._multiplayerSessionManager.localPlayer;
    if (localPlayer.IsFailed() || !localPlayer.WasActiveAtLevelStart())
    {
      this.HideAll();
      this.enabled = false;
    }
    else
    {
      this._timeToNextUpdate -= Time.deltaTime;
      if ((double) this._timeToNextUpdate > 0.0)
        return;
      this._timeToNextUpdate += 0.5f;
      int score = this._scoreProvider.rankedPlayers.First<MultiplayerScoreProvider.RankedPlayer>((Func<MultiplayerScoreProvider.RankedPlayer, bool>) (p => p.isMe)).score;
      for (int index = 0; index < this._scoreProvider.rankedPlayers.Count; ++index)
      {
        MultiplayerScoreProvider.RankedPlayer rankedPlayer = this._scoreProvider.rankedPlayers[index];
        if (!rankedPlayer.isMe)
        {
          if (rankedPlayer.isFailed)
          {
            MultiplayerConnectedPlayerFacade connectedPlayerController;
            if (this._playersManager.TryGetConnectedPlayerController(rankedPlayer.userId, out connectedPlayerController))
              connectedPlayerController.scoreDiffText.AnimateHide();
          }
          else
          {
            int scoreDiff = rankedPlayer.score - score;
            MultiplayerConnectedPlayerFacade connectedPlayerController;
            if (this._playersManager.TryGetConnectedPlayerController(rankedPlayer.userId, out connectedPlayerController))
              connectedPlayerController.scoreDiffText.AnimateScoreDiff(scoreDiff);
          }
        }
      }
    }
  }

  public virtual void InitLeftRightPositions()
  {
    IReadOnlyList<IConnectedPlayer> gameStartPlayers = this._playersManager.allActiveAtGameStartPlayers;
    int count = gameStartPlayers.Count;
    int num1 = gameStartPlayers.IndexOf<IConnectedPlayer>(this._multiplayerSessionManager.localPlayer);
    int num2 = 1;
    int index1;
    if (num1 == -1)
    {
      num1 = gameStartPlayers.Count - 1;
      index1 = (num1 + count) % count;
      num2 = 0;
    }
    else
      index1 = (num1 - 1 + count) % count;
    int index2 = (num1 + 1) % count;
    bool flag = num1 > (count - 1) / 2;
    for (; num2 < gameStartPlayers.Count; ++num2)
    {
      if (flag)
      {
        MultiplayerConnectedPlayerFacade connectedPlayerController;
        if (this._playersManager.TryGetConnectedPlayerController(gameStartPlayers[index1].userId, out connectedPlayerController))
          connectedPlayerController.scoreDiffText.SetHorizontalPositionRelativeToLocalPlayer(MultiplayerScoreDiffText.HorizontalPosition.Left);
        index1 = (index1 - 1 + count) % count;
      }
      else
      {
        MultiplayerConnectedPlayerFacade connectedPlayerController;
        if (this._playersManager.TryGetConnectedPlayerController(gameStartPlayers[index2].userId, out connectedPlayerController))
          connectedPlayerController.scoreDiffText.SetHorizontalPositionRelativeToLocalPlayer(MultiplayerScoreDiffText.HorizontalPosition.Right);
        index2 = (index2 + 1) % count;
      }
      flag = !flag;
    }
  }

  public virtual void HideAll()
  {
    if (!this._playersManager.playerSpawningFinished)
      return;
    foreach (IConnectedPlayer atGameStartPlayer in (IEnumerable<IConnectedPlayer>) this._playersManager.allActiveAtGameStartPlayers)
    {
      MultiplayerConnectedPlayerFacade connectedPlayerController;
      if (this._playersManager.TryGetConnectedPlayerController(atGameStartPlayer.userId, out connectedPlayerController))
        connectedPlayerController.scoreDiffText.AnimateHide();
    }
  }

  public virtual void HandleStateChanged(MultiplayerController.State newState)
  {
    if (newState == MultiplayerController.State.Gameplay)
    {
      if (!this._multiplayerSessionManager.localPlayer.IsActiveOrFinished() || this._layoutProvider.layout == MultiplayerPlayerLayout.Duel)
        return;
      this.InitLeftRightPositions();
      this.enabled = true;
    }
    else
    {
      this.HideAll();
      this.enabled = false;
    }
  }
}
