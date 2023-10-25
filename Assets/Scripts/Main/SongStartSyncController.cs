// Decompiled with JetBrains decompiler
// Type: SongStartSyncController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class SongStartSyncController : MonoBehaviour
{
  protected const float kAudioLoadTimeout = 15f;
  [Inject]
  protected readonly IMultiplayerSessionManager _multiplayerSessionManager;
  [Inject]
  protected readonly IGameplayRpcManager _gameplayRpcManager;
  protected float _waitStartTime;
  protected bool _songStarted;
  protected float _startTime;
  protected string _sessionGameId;
  protected SongStartHandler _songStartHandler;

  public bool isSongStarted => this._songStarted;

  public float songStartSyncTime => this._songStartHandler.songStartSyncTime;

  public event System.Action syncStartFailedEvent;

  public event System.Action<float> syncStartSuccessEvent;

  public event System.Action<float> syncResumeEvent;

  public virtual void Start() => this.enabled = false;

  public virtual void OnDestroy() => this._songStartHandler?.Dispose();

  public virtual void Update()
  {
    if (this._songStarted || (double) Time.realtimeSinceStartup - 15.0 <= (double) this._waitStartTime)
      return;
    System.Action startFailedEvent = this.syncStartFailedEvent;
    if (startFailedEvent != null)
      startFailedEvent();
    this.enabled = false;
  }

  public virtual void OnApplicationPause(bool pauseStatus)
  {
    if (pauseStatus || !this._songStarted)
      return;
    System.Action<float> syncResumeEvent = this.syncResumeEvent;
    if (syncResumeEvent == null)
      return;
    syncResumeEvent(this._songStartHandler.songStartSyncTime);
  }

  public virtual void StartSong(
    PlayersSpecificSettingsAtGameStartModel playersSpecificSettingsAtGameStartModel,
    string sessionGameId)
  {
    this._sessionGameId = sessionGameId;
    if (!this._songStarted)
    {
      this._songStartHandler = new SongStartHandler(this._multiplayerSessionManager, this._gameplayRpcManager, playersSpecificSettingsAtGameStartModel);
      this._songStartHandler.setSongStartSyncTimeEvent += new System.Action<float>(this.HandleSetSongStartSyncTime);
      this._waitStartTime = Time.realtimeSinceStartup;
      this._songStartHandler.GetLevelStartTimeOffset();
      this.enabled = true;
    }
    else
    {
      System.Action<float> startSuccessEvent = this.syncStartSuccessEvent;
      if (startSuccessEvent == null)
        return;
      startSuccessEvent(this._songStartHandler.songStartSyncTime);
    }
  }

  public virtual void HandleSetSongStartSyncTime(float songStartSyncTime)
  {
    if (this._songStarted)
      return;
    this._songStarted = true;
    System.Action<float> startSuccessEvent = this.syncStartSuccessEvent;
    if (startSuccessEvent == null)
      return;
    startSuccessEvent(songStartSyncTime);
  }
}
