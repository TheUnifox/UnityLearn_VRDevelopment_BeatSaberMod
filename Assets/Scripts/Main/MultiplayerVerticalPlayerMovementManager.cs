// Decompiled with JetBrains decompiler
// Type: MultiplayerVerticalPlayerMovementManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MultiplayerVerticalPlayerMovementManager : MonoBehaviour
{
  [Tooltip("Local player is 0, range is <-MovementRange/2, MovementRange/2>")]
  [SerializeField]
  protected float _movementRange = 20f;
  [SerializeField]
  protected float _duelMovementRange = 6f;
  [SerializeField]
  protected float _maxMoveSpeedMetersPerSecond = 0.5f;
  [SerializeField]
  protected float _accelerationMetersPerSecondSquared = 0.1f;
  [SerializeField]
  protected float _decelerationMetersPerSecondSquared = 0.1f;
  [SerializeField]
  protected float _minScoreDifference = 4500f;
  [Inject]
  protected readonly MultiplayerPlayersManager _multiplayerPlayersManager;
  [Inject]
  protected readonly MultiplayerScoreProvider _scoreProvider;
  [Inject]
  protected readonly MultiplayerLayoutProvider _layoutProvider;
  [Inject]
  protected readonly MultiplayerController _multiplayerController;
  protected readonly List<MultiplayerScoreProvider.RankedPlayer> _reusablePlayersList = new List<MultiplayerScoreProvider.RankedPlayer>();
  protected readonly Dictionary<MultiplayerConnectedPlayerFacade, float> _currentSpeedsDictionary = new Dictionary<MultiplayerConnectedPlayerFacade, float>();
  protected float _lastFrameBaseScore;

  public virtual void Start()
  {
    this._multiplayerController.stateChangedEvent += new System.Action<MultiplayerController.State>(this.HandleStateChanged);
    this.HandleStateChanged(this._multiplayerController.state);
  }

  public virtual void OnDestroy()
  {
    if (!((UnityEngine.Object) this._multiplayerController != (UnityEngine.Object) null))
      return;
    this._multiplayerController.stateChangedEvent -= new System.Action<MultiplayerController.State>(this.HandleStateChanged);
  }

  public virtual void Update()
  {
    this._reusablePlayersList.Clear();
    MultiplayerScoreProvider.RankedPlayer rankedPlayer1 = (MultiplayerScoreProvider.RankedPlayer) null;
    IReadOnlyList<MultiplayerScoreProvider.RankedPlayer> rankedPlayers = this._scoreProvider.rankedPlayers;
    if (rankedPlayers.Count == 0 || rankedPlayers[0].score <= 0)
      return;
    float a1 = 0.0f;
    float a2 = 0.0f;
    bool flag1 = true;
    for (int index = 0; index < rankedPlayers.Count; ++index)
    {
      MultiplayerScoreProvider.RankedPlayer rankedPlayer2 = rankedPlayers[index];
      if (rankedPlayer2.isMe && rankedPlayer2.wasActiveAtLevelStart)
        rankedPlayer1 = rankedPlayer2;
      if (rankedPlayer2.isActiveOrFinished)
      {
        a1 = Mathf.Max(a1, (float) rankedPlayer2.score);
        a2 = Mathf.Min(a2, (float) rankedPlayer2.score);
        this._reusablePlayersList.Add(rankedPlayer2);
      }
    }
    float num1 = this._lastFrameBaseScore;
    if (rankedPlayer1 != null)
    {
      if (rankedPlayer1.isActiveOrFinished)
        num1 = (float) rankedPlayer1.score;
      else if (this._reusablePlayersList.Count > 0)
      {
        if ((double) rankedPlayer1.score > (double) a2)
        {
          num1 = (float) rankedPlayer1.score;
        }
        else
        {
          num1 = (float) this._reusablePlayersList[this._reusablePlayersList.Count / 2].score;
          flag1 = false;
        }
      }
    }
    else if (this._reusablePlayersList.Count > 0)
      num1 = (float) this._reusablePlayersList[this._reusablePlayersList.Count / 2].score;
    this._lastFrameBaseScore = num1;
    float num2 = Mathf.Max(a1 - a2, this._minScoreDifference);
    float num3 = this._layoutProvider.layout == MultiplayerPlayerLayout.Duel ? this._duelMovementRange : this._movementRange;
    float max = num3 * 0.5f;
    for (int index = 0; index < rankedPlayers.Count; ++index)
    {
      MultiplayerScoreProvider.RankedPlayer rankedPlayer3 = rankedPlayers[index];
      MultiplayerConnectedPlayerFacade connectedPlayerController;
      if (!rankedPlayer3.isMe && this._multiplayerPlayersManager.TryGetConnectedPlayerController(rankedPlayer3.userId, out connectedPlayerController))
      {
        int num4 = rankedPlayer3.score;
        if (rankedPlayer3.isFailed)
          num4 = 0;
        float num5 = (float) (((double) num4 - (double) num1) / (double) num2 * (double) num3 * 0.5);
        bool flag2 = !rankedPlayer3.isActiveOrFinished;
        float min = flag1 | flag2 ? -max : max * 0.1f;
        float b = Mathf.Clamp(num5, min, max);
        Transform transform = connectedPlayerController.transform;
        Vector3 position = transform.position;
        if (!Mathf.Approximately(position.y, b))
        {
          float num6;
          if (!this._currentSpeedsDictionary.TryGetValue(connectedPlayerController, out num6))
            num6 = 0.0f;
          float num7 = Mathf.Abs(b - position.y);
          float num8 = (float) ((double) num6 * (double) num6 / (2.0 * (double) this._decelerationMetersPerSecondSquared));
          Vector3 vector3;
          if ((double) b > (double) position.y)
          {
            num6 = (double) num8 < (double) num7 ? Mathf.Min(this._maxMoveSpeedMetersPerSecond, num6 + this._accelerationMetersPerSecondSquared * Time.deltaTime) : Mathf.Max(0.0f, num6 - this._decelerationMetersPerSecondSquared * Time.deltaTime);
            vector3 = new Vector3(position.x, Mathf.Min(position.y + Time.deltaTime * num6, b), position.z);
          }
          else
          {
            num6 = (double) num8 < (double) num7 ? Mathf.Max(-this._maxMoveSpeedMetersPerSecond, num6 - this._accelerationMetersPerSecondSquared * Time.deltaTime) : Mathf.Min(0.0f, num6 + this._decelerationMetersPerSecondSquared * Time.deltaTime);
            vector3 = new Vector3(position.x, Mathf.Max(position.y + Time.deltaTime * num6, b), position.z);
          }
          this._currentSpeedsDictionary[connectedPlayerController] = num6;
          transform.position = vector3;
        }
      }
    }
  }

  public virtual void HandleStateChanged(MultiplayerController.State state) => this.enabled = state == MultiplayerController.State.Gameplay;
}
