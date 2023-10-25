// Decompiled with JetBrains decompiler
// Type: GameSongController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

public class GameSongController : SongController, IStartSeekSongController
{
  [SerializeField]
  protected AudioTimeSyncController _audioTimeSyncController;
  [SerializeField]
  protected AudioPitchGainEffect _failAudioPitchGainEffect;
  [Inject]
  protected readonly BeatmapCallbacksController _beatmapCallbacksController;
  [Inject]
  protected readonly BeatmapCallbacksUpdater _beatmapCallbacksUpdater;
  protected bool _songDidFinish;

  public float songLength => this._audioTimeSyncController.songLength;

  public WaitUntil waitUntilIsReadyToStartTheSong => this._audioTimeSyncController.waitUntilAudioIsLoaded;

  public virtual void LateUpdate()
  {
    if (this._songDidFinish || (double) this._audioTimeSyncController.songTime < (double) this._audioTimeSyncController.songEndTime - 0.20000000298023224)
      return;
    this._songDidFinish = true;
    this.SendSongDidFinishEvent();
  }

  public virtual void StartSong(float songTimeOffset = 0.0f)
  {
    this._songDidFinish = false;
    this._audioTimeSyncController.StartSong(songTimeOffset);
  }

  public override void StopSong() => this._audioTimeSyncController.StopSong();

  public override void PauseSong()
  {
    this._audioTimeSyncController.Pause();
    this._beatmapCallbacksUpdater.Pause();
  }

  public override void ResumeSong()
  {
    this._audioTimeSyncController.Resume();
    this._beatmapCallbacksUpdater.Resume();
  }

  public virtual void FailStopSong()
  {
    for (int index = 0; index < 16; ++index)
      this._beatmapCallbacksController.TriggerBeatmapEvent((BeatmapEventData) new BasicBeatmapEventData(0.0f, (BasicBeatmapEventType) index, -1, 1f));
    this._beatmapCallbacksUpdater.Pause();
    this._audioTimeSyncController.forcedNoAudioSync = true;
    this._failAudioPitchGainEffect.StartEffect(1f, (System.Action) (() => this._audioTimeSyncController.StopSong()));
  }

  public virtual void SeekTo(float songTime) => this._audioTimeSyncController.SeekTo(songTime);

  [CompilerGenerated]
  public virtual void m_CFailStopSongm_Eb__14_0() => this._audioTimeSyncController.StopSong();
}
