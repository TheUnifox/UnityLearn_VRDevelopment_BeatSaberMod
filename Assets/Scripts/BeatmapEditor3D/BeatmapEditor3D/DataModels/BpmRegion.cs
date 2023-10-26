// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.BpmRegion
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

namespace BeatmapEditor3D.DataModels
{
  public class BpmRegion
  {
    public readonly int startSampleIndex;
    public readonly int endSampleIndex;
    public readonly float bpm;
    public readonly float beats;
    public readonly float startBeat;
    public readonly float endBeat;
    public readonly int sampleFrequency;

    public int sampleCount => this.endSampleIndex - this.startSampleIndex;

    public int samplesPerBeat => (int) ((double) this.sampleCount / (double) this.beats);

    public BpmRegion(
      int startSampleIndex,
      int endSampleIndex,
      float startBeat,
      float endBeat,
      int sampleFrequency)
    {
      this.startSampleIndex = startSampleIndex;
      this.endSampleIndex = endSampleIndex;
      this.startBeat = startBeat;
      this.endBeat = endBeat;
      this.beats = endBeat - startBeat;
      this.sampleFrequency = sampleFrequency;
      this.bpm = AudioTimeHelper.SamplesToBPM(this.sampleCount, sampleFrequency, this.beats);
    }

    public BpmRegion(
      BpmRegion other,
      int? startSampleIndex = null,
      int? endSampleIndex = null,
      float? startBeat = null,
      float? endBeat = null,
      int? sampleFrequency = null)
    {
      this.startSampleIndex = startSampleIndex ?? other.startSampleIndex;
      int? nullable1 = endSampleIndex;
      this.endSampleIndex = nullable1 ?? other.endSampleIndex;
      float? nullable2 = startBeat;
      this.startBeat = nullable2 ?? other.startBeat;
      nullable2 = endBeat;
      this.endBeat = nullable2 ?? other.endBeat;
      this.beats = this.endBeat - this.startBeat;
      nullable1 = sampleFrequency;
      this.sampleFrequency = nullable1 ?? other.sampleFrequency;
      this.bpm = AudioTimeHelper.SamplesToBPM(this.sampleCount, this.sampleFrequency, this.beats);
    }

    public int BeatToSample(float beat) => AudioTimeHelper.BeatToSample(this.startSampleIndex, this.endSampleIndex, this.startBeat, this.endBeat, beat);

    public float SampleToBeat(int sample) => AudioTimeHelper.SampleToBeat(this.startSampleIndex, this.endSampleIndex, this.startBeat, this.endBeat, sample);
  }
}
