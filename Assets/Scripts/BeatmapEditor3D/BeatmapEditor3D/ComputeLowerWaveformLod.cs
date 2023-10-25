// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.ComputeLowerWaveformLod
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace BeatmapEditor3D
{
  [BurstCompile]
  public struct ComputeLowerWaveformLod : IJobParallelFor
  {
    [WriteOnly]
    public NativeArray<SamplesLod> outputSamples;
    [ReadOnly]
    private NativeArray<SamplesLod> _inputSamples;

    public ComputeLowerWaveformLod(
      NativeArray<SamplesLod> inputSamples,
      NativeArray<SamplesLod> outputSamples)
    {
      this._inputSamples = inputSamples;
      this.outputSamples = outputSamples;
    }

    public void Execute(int i) => this.outputSamples[i] = new SamplesLod()
    {
      outerMinSample = math.min(this._inputSamples[i * 2].outerMinSample, this._inputSamples[i * 2 + 1].outerMinSample),
      outerMaxSample = math.max(this._inputSamples[i * 2].outerMaxSample, this._inputSamples[i * 2 + 1].outerMaxSample)
    };
  }
}
