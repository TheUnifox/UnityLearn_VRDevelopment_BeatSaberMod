// Decompiled with JetBrains decompiler
// Type: MultiplayerLeadPlayerProvider
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class MultiplayerLeadPlayerProvider : MonoBehaviour
{
  [SerializeField]
  protected float _timeToGainFirstLead = 2f;
  [SerializeField]
  protected float _timeToLooseLead = 0.5f;
  [Inject]
  protected readonly MultiplayerScoreProvider _scoreProvider;
  [Inject]
  protected readonly MultiplayerController _multiplayerController;
  protected float _currentLeadingPlayerStartTime;
  protected MultiplayerScoreProvider.RankedPlayer _currentlyDisplayedUser;
  protected MultiplayerScoreProvider.RankedPlayer _currentlyLeadingUser;

  public event System.Action<string> newLeaderWasSelectedEvent;

  public virtual void Start()
  {
    this._multiplayerController.stateChangedEvent += new System.Action<MultiplayerController.State>(this.HandleStateChanged);
    this.HandleStateChanged(this._multiplayerController.state);
  }

  public virtual void OnDestroy()
  {
    if ((UnityEngine.Object) this._multiplayerController != (UnityEngine.Object) null)
      this._multiplayerController.stateChangedEvent -= new System.Action<MultiplayerController.State>(this.HandleStateChanged);
    if (!((UnityEngine.Object) this._scoreProvider != (UnityEngine.Object) null))
      return;
    this._scoreProvider.firstPlayerDidChangeEvent -= new System.Action<MultiplayerScoreProvider.RankedPlayer>(this.HandleFirstPlayerDidChange);
  }

  public virtual void Update()
  {
    if ((double) Time.time - (double) this._currentLeadingPlayerStartTime <= (this._currentlyDisplayedUser == null ? (double) this._timeToGainFirstLead : (double) this._timeToLooseLead))
      return;
    this._currentlyDisplayedUser = this._currentlyLeadingUser;
    System.Action<string> wasSelectedEvent = this.newLeaderWasSelectedEvent;
    if (wasSelectedEvent != null)
      wasSelectedEvent(this._currentlyDisplayedUser.userId);
    this.enabled = false;
  }

  public virtual void StopProviding()
  {
    this._scoreProvider.firstPlayerDidChangeEvent -= new System.Action<MultiplayerScoreProvider.RankedPlayer>(this.HandleFirstPlayerDidChange);
    this.enabled = false;
  }

  public virtual void StartProviding()
  {
    this.HandleFirstPlayerDidChange(this._scoreProvider.firstPlayer);
    this._scoreProvider.firstPlayerDidChangeEvent += new System.Action<MultiplayerScoreProvider.RankedPlayer>(this.HandleFirstPlayerDidChange);
  }

  public virtual void HandleStateChanged(MultiplayerController.State state)
  {
    if (state == MultiplayerController.State.Gameplay)
      this.StartProviding();
    else
      this.StopProviding();
  }

  public virtual void HandleFirstPlayerDidChange(MultiplayerScoreProvider.RankedPlayer firstPlayer)
  {
    if (firstPlayer == null || !firstPlayer.isActiveOrFinished)
    {
      System.Action<string> wasSelectedEvent = this.newLeaderWasSelectedEvent;
      if (wasSelectedEvent == null)
        return;
      wasSelectedEvent((string) null);
    }
    else if (firstPlayer != this._currentlyDisplayedUser)
    {
      this._currentlyLeadingUser = firstPlayer;
      if (this._currentlyDisplayedUser != null && this._currentlyDisplayedUser.isFailed)
      {
        this._currentlyDisplayedUser = this._currentlyLeadingUser;
        System.Action<string> wasSelectedEvent = this.newLeaderWasSelectedEvent;
        if (wasSelectedEvent != null)
          wasSelectedEvent(this._currentlyDisplayedUser.userId);
        this.enabled = false;
      }
      else
      {
        if (this.enabled && this._currentlyDisplayedUser != null)
          return;
        this._currentLeadingPlayerStartTime = Time.time;
        this.enabled = true;
      }
    }
    else
      this.enabled = false;
  }
}
