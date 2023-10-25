// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BpmEditor.BpmEditorSongPreviewController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.BpmEditor
{
  public class BpmEditorSongPreviewController : ITickable
  {
    public const float kMinPlaybackSpeed = 0.3f;
    public const float kMaxPlaybackSpeed = 2f;
    private const float kMinPitchShift = 0.5f;
    private const float kMaxPitchShift = 2f;
    [Inject]
    private readonly AudioManagerSO _audioManager;
    [Inject]
    private readonly AudioSource _audioSource;
    public bool correctPitchOnPlaybackSpeedChange = true;
    private AudioClip _audioClip;

    public event Action<int> playHeadPositionChangedEvent;

    public event Action<int> playbackStartedEvent;

    public event Action playbackStoppedEvent;

    public bool isPlaying { get; private set; }

    public int sample => this._audioSource.timeSamples;

    public int samplesCount => this._audioClip.samples;

    public AudioClip audioClip => this._audioClip;

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
        Action<int> positionChangedEvent = this.playHeadPositionChangedEvent;
        if (positionChangedEvent == null)
          return;
        positionChangedEvent(this._audioSource.timeSamples);
      }
    }

    public void Initialize(AudioClip audioClip)
    {
      this._audioClip = audioClip;
      this._audioSource.clip = audioClip;
    }

    public void PlayFrom(int sample)
    {
      if (this.isPlaying)
        return;
      this.isPlaying = true;
      this._audioSource.timeSamples = Mathf.Min(sample, this._audioClip.samples - 1);
      this._audioSource.Play();
      Action<int> playbackStartedEvent = this.playbackStartedEvent;
      if (playbackStartedEvent == null)
        return;
      playbackStartedEvent(sample);
    }

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

    public void SetVolume(float volume) => this._audioManager.musicVolume = AudioHelpers.NormalizedVolumeToDB(volume);

    public void SetSpeed(float speed)
    {
      float num1 = Mathf.Clamp(speed, 0.3f, 2f);
      float num2 = this.correctPitchOnPlaybackSpeedChange ? Mathf.Clamp(1f / num1, 0.5f, 2f) : 1f;
      this._audioManager.musicSpeed = num1;
      this._audioManager.musicPitch = num2;
    }

    public void SetSfxVolume(float volume) => this._audioManager.sfxVolume = AudioHelpers.NormalizedVolumeToDB(volume);
  }
}
