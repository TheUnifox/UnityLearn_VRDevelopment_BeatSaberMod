// Decompiled with JetBrains decompiler
// Type: MultiplayerLocalPlayerScoreDiffTextManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class MultiplayerLocalPlayerScoreDiffTextManager : MonoBehaviour
{
  [SerializeField]
  protected MultiplayerScoreDiffText _scoreDiffText;
  [Inject]
  protected readonly MultiplayerController _multiplayerController;
  [Inject]
  protected readonly MultiplayerScoreProvider _scoreProvider;
  [Inject]
  protected readonly CoreGameHUDController.InitData _hudInitData;
  [Inject]
  protected readonly IMultiplayerSessionManager _multiplayerSessionManager;
  protected float _timeToNextUpdate;
  protected const float kUpdateInterval = 0.5f;
  protected bool? _wasLocalPlayerLeader;

  public virtual void Start()
  {
    if (this._hudInitData.hide)
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
      this._scoreDiffText.AnimateHide();
      this.enabled = false;
    }
    else
    {
      this._timeToNextUpdate -= Time.deltaTime;
      if ((double) this._timeToNextUpdate > 0.0)
        return;
      this._timeToNextUpdate += 0.5f;
      int num1 = 0;
      int a = 0;
      for (int index = 0; index < this._scoreProvider.rankedPlayers.Count; ++index)
      {
        MultiplayerScoreProvider.RankedPlayer rankedPlayer = this._scoreProvider.rankedPlayers[index];
        if (rankedPlayer.isMe)
          num1 = rankedPlayer.score;
        else
          a = Mathf.Max(a, rankedPlayer.score);
      }
      this._scoreDiffText.AnimateScoreDiff(num1 - a);
      bool isLeader = num1 > a;
      int num2;
      if (this._wasLocalPlayerLeader.HasValue)
      {
        int num3 = isLeader ? 1 : 0;
        bool? localPlayerLeader = this._wasLocalPlayerLeader;
        int num4 = localPlayerLeader.GetValueOrDefault() ? 1 : 0;
        num2 = !(num3 == num4 & localPlayerLeader.HasValue) ? 1 : 0;
      }
      else
        num2 = 1;
      if (num2 == 0)
        return;
      this._scoreDiffText.AnimateIsLeadPlayer(isLeader);
      this._wasLocalPlayerLeader = new bool?(isLeader);
    }
  }

  public virtual void HandleStateChanged(MultiplayerController.State newState)
  {
    if (newState == MultiplayerController.State.Gameplay)
    {
      this.enabled = true;
    }
    else
    {
      this.enabled = false;
      this._scoreDiffText.AnimateHide();
    }
  }
}
