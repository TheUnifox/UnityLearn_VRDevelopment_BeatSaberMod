// Decompiled with JetBrains decompiler
// Type: MultiplayerConnectedPlayerBeatmapObjectEventManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MultiplayerConnectedPlayerBeatmapObjectEventManager : 
  MonoBehaviour,
  IConnectedPlayerBeatmapObjectEventManager
{
  [Inject]
  protected readonly IConnectedPlayer _connectedPlayer;
  [Inject]
  protected readonly IGameplayRpcManager _gameplayRpcManager;
  [Inject]
  protected readonly MultiplayerConnectedPlayerSongTimeSyncController _songTimeController;
  protected readonly Queue<MultiplayerConnectedPlayerBeatmapObjectEventManager.TimestampedBeatmapObjectEventData> _beatmapObjectEventQueue = new Queue<MultiplayerConnectedPlayerBeatmapObjectEventManager.TimestampedBeatmapObjectEventData>();
  protected bool _paused;

  public event System.Action<NoteSpawnInfoNetSerializable> connectedPlayerNoteWasSpawnedEvent;

  public event System.Action<ObstacleSpawnInfoNetSerializable> connectedPlayerObstacleWasSpawnedEvent;

  public event System.Action<SliderSpawnInfoNetSerializable> connectedPlayerSliderWasSpawnedEvent;

  public event System.Action<NoteCutInfoNetSerializable> connectedPlayerNoteWasCutEvent;

  public event System.Action<NoteMissInfoNetSerializable> connectedPlayerNoteWasMissedEvent;

  public virtual void Start()
  {
    this._gameplayRpcManager.obstacleWasSpawnedEvent += new Action<string, float, float, ObstacleSpawnInfoNetSerializable>(this.HandleBeatmapObjectEventData<ObstacleSpawnInfoNetSerializable>);
    this._gameplayRpcManager.noteWasSpawnedEvent += new Action<string, float, float, NoteSpawnInfoNetSerializable>(this.HandleBeatmapObjectEventData<NoteSpawnInfoNetSerializable>);
    this._gameplayRpcManager.sliderWasSpawnedEvent += new Action<string, float, float, SliderSpawnInfoNetSerializable>(this.HandleBeatmapObjectEventData<SliderSpawnInfoNetSerializable>);
    this._gameplayRpcManager.noteWasCutEvent += new Action<string, float, float, NoteCutInfoNetSerializable>(this.HandleBeatmapObjectEventData<NoteCutInfoNetSerializable>);
    this._gameplayRpcManager.noteWasMissedEvent += new Action<string, float, float, NoteMissInfoNetSerializable>(this.HandleBeatmapObjectEventData<NoteMissInfoNetSerializable>);
  }

  public virtual void OnDestroy()
  {
    if (this._gameplayRpcManager == null)
      return;
    this._gameplayRpcManager.obstacleWasSpawnedEvent -= new Action<string, float, float, ObstacleSpawnInfoNetSerializable>(this.HandleBeatmapObjectEventData<ObstacleSpawnInfoNetSerializable>);
    this._gameplayRpcManager.noteWasSpawnedEvent -= new Action<string, float, float, NoteSpawnInfoNetSerializable>(this.HandleBeatmapObjectEventData<NoteSpawnInfoNetSerializable>);
    this._gameplayRpcManager.sliderWasSpawnedEvent -= new Action<string, float, float, SliderSpawnInfoNetSerializable>(this.HandleBeatmapObjectEventData<SliderSpawnInfoNetSerializable>);
    this._gameplayRpcManager.noteWasCutEvent -= new Action<string, float, float, NoteCutInfoNetSerializable>(this.HandleBeatmapObjectEventData<NoteCutInfoNetSerializable>);
    this._gameplayRpcManager.noteWasMissedEvent -= new Action<string, float, float, NoteMissInfoNetSerializable>(this.HandleBeatmapObjectEventData<NoteMissInfoNetSerializable>);
  }

  public virtual void Update()
  {
    if (this._paused)
      return;
    while (this._beatmapObjectEventQueue.Count > 0 && (double) this._beatmapObjectEventQueue.Peek().time <= (double) this._songTimeController.songTime)
      this.InvokeCallback(this._beatmapObjectEventQueue.Dequeue().beatmapObjectEventData);
  }

  public virtual void Pause() => this._paused = true;

  public virtual void Resume() => this._paused = false;

  public virtual void HandleBeatmapObjectEventData<T>(
    string userId,
    float syncTime,
    float songTime,
    T beatmapObjectEventData)
    where T : IPoolableSerializable
  {
    if (userId != this._connectedPlayer.userId)
      return;
    this._songTimeController.SetConnectedPlayerSongTime(syncTime, songTime);
    beatmapObjectEventData.Retain();
    if ((double) this._songTimeController.songTime >= (double) songTime)
      this.InvokeCallback((IPoolableSerializable) beatmapObjectEventData);
    else
      this._beatmapObjectEventQueue.Enqueue(new MultiplayerConnectedPlayerBeatmapObjectEventManager.TimestampedBeatmapObjectEventData(songTime, (IPoolableSerializable) beatmapObjectEventData));
  }

  public virtual void InvokeCallback(IPoolableSerializable noteEventData)
  {
    switch (noteEventData)
    {
      case NoteSpawnInfoNetSerializable infoNetSerializable1:
        System.Action<NoteSpawnInfoNetSerializable> noteWasSpawnedEvent = this.connectedPlayerNoteWasSpawnedEvent;
        if (noteWasSpawnedEvent != null)
        {
          noteWasSpawnedEvent(infoNetSerializable1);
          break;
        }
        break;
      case ObstacleSpawnInfoNetSerializable infoNetSerializable2:
        System.Action<ObstacleSpawnInfoNetSerializable> obstacleWasSpawnedEvent = this.connectedPlayerObstacleWasSpawnedEvent;
        if (obstacleWasSpawnedEvent != null)
        {
          obstacleWasSpawnedEvent(infoNetSerializable2);
          break;
        }
        break;
      case SliderSpawnInfoNetSerializable infoNetSerializable3:
        System.Action<SliderSpawnInfoNetSerializable> sliderWasSpawnedEvent = this.connectedPlayerSliderWasSpawnedEvent;
        if (sliderWasSpawnedEvent != null)
        {
          sliderWasSpawnedEvent(infoNetSerializable3);
          break;
        }
        break;
      case NoteCutInfoNetSerializable infoNetSerializable4:
        System.Action<NoteCutInfoNetSerializable> playerNoteWasCutEvent = this.connectedPlayerNoteWasCutEvent;
        if (playerNoteWasCutEvent != null)
        {
          playerNoteWasCutEvent(infoNetSerializable4);
          break;
        }
        break;
      case NoteMissInfoNetSerializable infoNetSerializable5:
        System.Action<NoteMissInfoNetSerializable> noteWasMissedEvent = this.connectedPlayerNoteWasMissedEvent;
        if (noteWasMissedEvent != null)
        {
          noteWasMissedEvent(infoNetSerializable5);
          break;
        }
        break;
    }
    noteEventData.Release();
  }

  public readonly struct TimestampedBeatmapObjectEventData
  {
    public readonly float time;
    public readonly IPoolableSerializable beatmapObjectEventData;

    public TimestampedBeatmapObjectEventData(
      float time,
      IPoolableSerializable beatmapObjectEventData)
    {
      this.time = time;
      this.beatmapObjectEventData = beatmapObjectEventData;
    }
  }
}
