// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.AudioTimeHelper
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using UnityEngine;

namespace BeatmapEditor3D.DataModels
{
  public static class AudioTimeHelper
  {
    public const float kMaxBeat = 3000f;
    public const int kBeatsPerWholeBar = 4;
    public const int kMaxBeatSubdivision = 256;
    public const float kMinBeatDifference = 0.00190624991f;
    public const int kSubdivisionMultiplier = 128;
    private const float kSecondsPerMinute = 60f;

    public static bool IsBeatSame(float a, float b) => (double) Mathf.Abs(a - b) < 0.0019062499050050974;

    public static int SecondsToSamples(float seconds, int frequency) => Mathf.FloorToInt(seconds * (float) frequency);

    public static float SecondsToBPM(float seconds, float beats) => (float) ((double) beats / (double) seconds * 60.0);

    public static float SecondsToBeats(float seconds, float bpm = 1f) => seconds / 60f * bpm;

    public static float SamplesToSeconds(int samples, int frequency) => (float) samples / (float) frequency;

    public static float SamplesToBPM(int samples, int frequency, float beats) => AudioTimeHelper.SecondsToBPM(AudioTimeHelper.SamplesToSeconds(samples, frequency), beats);

    public static float SamplesToBeats(int samples, int frequency, float bpm) => AudioTimeHelper.SecondsToBeats(AudioTimeHelper.SamplesToSeconds(samples, frequency), bpm);

    public static float BeatsToSeconds(float beats, float bpm) => (float) ((double) beats / (double) bpm * 60.0);

    public static int BeatsToSamples(float beats, int frequency, float bpm) => Mathf.FloorToInt(AudioTimeHelper.BeatsToSeconds(beats, bpm) * (float) frequency);

    public static float ChangeBeatBySubdivision(
      float currentBeat,
      int subdivision,
      int subdivisionDelta,
      int minValue = 0)
    {
      return (float) Math.Max(Mathf.RoundToInt(currentBeat / 4f * (float) subdivision + (float) subdivisionDelta), minValue) * (4f / (float) subdivision);
    }

    public static float RoundToBeat(float beat, int subdivision) => (float) Mathf.Max(Mathf.RoundToInt(beat / 4f * (float) subdivision), 0) * (4f / (float) subdivision);

    public static float RoundToBeatRelative(float beat, float relativeBeat, int subdivision) => relativeBeat + AudioTimeHelper.RoundToBeat(beat - relativeBeat, subdivision);

    public static float RoundDownToBeat(float beat, int subdivision) => Mathf.Max(Mathf.Floor(beat / 4f * (float) subdivision), 0.0f) * (4f / (float) subdivision);

    public static float RoundUpToBeat(float beat, int subdivision) => Mathf.Max(Mathf.Ceil(beat / 4f * (float) subdivision), 0.0f) * (4f / (float) subdivision);

    public static int BeatToSample(
      int startSampleIndex,
      int endSampleIndex,
      float startBeat,
      float endBeat,
      float beat)
    {
      return (int) Mathf.Lerp((float) startSampleIndex, (float) endSampleIndex, Mathf.InverseLerp(startBeat, endBeat, beat));
    }

    public static float SampleToBeat(
      int startSampleIndex,
      int endSampleIndex,
      float startBeat,
      float endBeat,
      int sample)
    {
      return Mathf.Lerp(startBeat, endBeat, Mathf.InverseLerp((float) startSampleIndex, (float) endSampleIndex, (float) sample));
    }
  }
}
