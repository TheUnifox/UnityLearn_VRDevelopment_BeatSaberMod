// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.BpmData
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System.Collections.Generic;
using UnityEngine;

namespace BeatmapEditor3D.DataModels
{
  public class BpmData
  {
    private readonly List<BpmRegion> _regions = new List<BpmRegion>();
    private readonly int _frequency;
    private readonly int _sampleCount;
    private readonly int _startOffset;
    private readonly float _lastBeat;
    private readonly int _lastSample;

    public List<BpmRegion> regions => this._regions;

    public float totalBeats => this._lastBeat;

    public int startOffset => this._startOffset;

    public int sampleCount => this._sampleCount;

    public int frequency => this._frequency;

    public BpmData(float bpm, int sampleCount, int frequency, int startOffset = 0)
    {
      this._startOffset = startOffset;
      this._frequency = frequency;
      this._sampleCount = sampleCount;
      float beats = AudioTimeHelper.SamplesToBeats(sampleCount, this._frequency, bpm);
      this._regions.Add(new BpmRegion(0, AudioTimeHelper.BeatsToSamples(beats, this._frequency, bpm) - 1, 0.0f, beats, frequency));
      this._lastBeat = this._regions[this._regions.Count - 1].endBeat;
      this._lastSample = this._regions[this._regions.Count - 1].endSampleIndex;
    }

    public BpmData(int frequency, int startOffset, List<BpmRegion> regions)
    {
      this._startOffset = startOffset;
      this._frequency = frequency;
      this._regions.AddRange((IEnumerable<BpmRegion>) regions);
      this._lastBeat = this._regions[this._regions.Count - 1].endBeat;
      this._lastSample = this._regions[this._regions.Count - 1].endSampleIndex;
      this._sampleCount = this._lastSample;
    }

    public BpmRegion GetRegionAtBeat(float beat)
    {
      beat = Mathf.Clamp(beat, 0.0f, this._lastBeat);
      return this._regions[this.SearchRegionsBinaryByBeat(0, this._regions.Count - 1, beat)];
    }

    public BpmRegion GetRegionAtSample(int sample)
    {
      sample = Mathf.Clamp(sample, 0, this._lastSample);
      return this._regions[this.SearchRegionsBinaryBySample(0, this._regions.Count - 1, sample)];
    }

    public float BeatToSeconds(float beat) => AudioTimeHelper.SamplesToSeconds(this.BeatToSample(beat), this._frequency);

    public float SecondsToBeat(float time) => this.SampleToBeat(AudioTimeHelper.SecondsToSamples(time, this._frequency));

    public float FadeEndBeat(float time, float fadeDuration) => this.SecondsToBeat(this.BeatToSeconds(time) + fadeDuration);

    public float SampleToBeat(int sampleIndex)
    {
      BpmRegion regionAtSample = this.GetRegionAtSample(sampleIndex);
      float num = (float) (sampleIndex - regionAtSample.startSampleIndex) / (float) regionAtSample.sampleCount * regionAtSample.beats;
      return regionAtSample.startBeat + num;
    }

    public int BeatToSample(float beat)
    {
      BpmRegion regionAtBeat = this.GetRegionAtBeat(beat);
      float num = (beat - regionAtBeat.startBeat) / regionAtBeat.beats * (float) regionAtBeat.sampleCount;
      return (int) ((double) regionAtBeat.startSampleIndex + (double) num);
    }

    private int SearchRegionsBinaryByBeat(int l, int r, float beat)
    {
      while (r >= l)
      {
        int index = l + (r - l) / 2;
        int num = BpmData.CompareRegionByBeat(this._regions[index], beat);
        if (num == 0)
          return index;
        if (num < 0)
          r = index - 1;
        else
          l = index + 1;
      }
      return this._regions.Count - 1;
    }

    private int SearchRegionsBinaryBySample(int l, int r, int sample)
    {
      while (r >= l)
      {
        int index = l + (r - l) / 2;
        int num = BpmData.CompareRegionBySampleIndex(this._regions[index], sample);
        if (num == 0)
          return index;
        if (num < 0)
          r = index - 1;
        else
          l = index + 1;
      }
      return this._regions.Count - 1;
    }

    private static int CompareRegionBySampleIndex(BpmRegion region, int sample)
    {
      if (sample < region.startSampleIndex)
        return -1;
      return region.endSampleIndex < sample ? 1 : 0;
    }

    private static int CompareRegionByBeat(BpmRegion region, float beat)
    {
      if ((double) region.startBeat <= (double) beat && (double) beat < (double) region.endBeat)
        return 0;
      return (double) beat < (double) region.startBeat ? -1 : 1;
    }
  }
}
