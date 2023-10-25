// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Controller.ISongPreviewController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;

namespace BeatmapEditor3D.Controller
{
  public interface ISongPreviewController
  {
    event Action<int> playheadPositionChangedEvent;

    event Action<float> playbackStartedEvent;

    event Action playbackStoppedEvent;

    bool isPlaying { get; }

    int currentSample { get; }

    int sampleCount { get; }

    float currentTime { get; }

    float volume { get; }

    void PlayFrom(float beat);

    void Stop();

    void SetPlaybackSpeed(float playbackSpeed);

    void SetVolume(float volume);
  }
}
