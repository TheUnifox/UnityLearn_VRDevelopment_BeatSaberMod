// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Controller.SongPreviewController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Controller
{
  public class SongPreviewController : ISongPreviewController, ITickable
  {
    [Inject]
    private readonly AudioManagerSO _audioManagerSo;
    [Inject]
    private readonly AudioSource _audioSource;
    [Inject]
    private readonly BeatmapDataModel _beatmapDataModel;
    public const float kMinPlaybackSpeed = 0.3f;
    public const float kMaxPlaybackSpeed = 2f;
    private const float kMinPitchShift = 0.5f;
    private const float kMaxPitchShift = 2f;
    public bool correctPitchOnPlaybackSpeedChange;

    public event Action<int> playheadPositionChangedEvent;

    public event Action<float> playbackStartedEvent;

    public event Action playbackStoppedEvent;

    public bool isPlaying { get; private set; }

    public int currentSample => this._audioSource.timeSamples;

    public int sampleCount => this._audioSource.clip.samples;

    public float currentTime => this._audioSource.time;

    public float volume => AudioHelpers.DBToNormalizedVolume(this._audioSource.volume);

    public void Tick()
    {
      if (!this.isPlaying)
        return;
      if (this._audioSource.timeSamples >= this._audioSource.clip.samples)
      {
        this.Stop();
      }
      else
      {
        Action<int> positionChangedEvent = this.playheadPositionChangedEvent;
        if (positionChangedEvent == null)
          return;
        positionChangedEvent(this._audioSource.timeSamples);
      }
    }

    public void PlayFrom(float beat)
    {
      if (this.isPlaying)
        return;
      this.isPlaying = true;
      this._audioSource.clip = this._beatmapDataModel.audioClip;
      this._audioSource.timeSamples = Mathf.Min(this._beatmapDataModel.bpmData.BeatToSample(beat), this._beatmapDataModel.audioClip.samples - 1);
      this._audioSource.Play();
      Action<float> playbackStartedEvent = this.playbackStartedEvent;
      if (playbackStartedEvent == null)
        return;
      playbackStartedEvent(beat);
    }

    public void SetPlaybackSpeed(float playbackSpeed)
    {
      float num = Mathf.Clamp(playbackSpeed, 0.3f, 2f);
      this._audioManagerSo.musicSpeed = num;
      this._audioManagerSo.musicPitch = this.correctPitchOnPlaybackSpeedChange ? Mathf.Clamp(1f / num, 0.5f, 2f) : 1f;
    }

    public void SetVolume(float volume) => this._audioManagerSo.musicVolume = AudioHelpers.NormalizedVolumeToDB(volume);

    public void Stop()
    {
      if (!this.isPlaying)
        return;
      this.isPlaying = false;
      this._audioSource.Stop();
      Action playbackStoppedEvent = this.playbackStoppedEvent;
      if (playbackStoppedEvent == null)
        return;
      playbackStoppedEvent();
    }
  }
}
