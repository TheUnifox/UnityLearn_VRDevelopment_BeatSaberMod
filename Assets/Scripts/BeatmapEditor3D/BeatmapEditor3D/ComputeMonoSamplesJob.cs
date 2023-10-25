// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.ComputeMonoSamplesJob
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace BeatmapEditor3D
{
  [BurstCompile]
  public struct ComputeMonoSamplesJob : IJobParallelFor
  {
    [WriteOnly]
    public NativeArray<float> monoSamples;
    [ReadOnly]
    private readonly NativeArray<float> _multiChannelSamples;
    [ReadOnly]
    private readonly int _channelCount;

    public ComputeMonoSamplesJob(
      NativeArray<float> monoSamples,
      NativeArray<float> multiChannelSamples,
      int channelCount)
    {
      this.monoSamples = monoSamples;
      this._multiChannelSamples = multiChannelSamples;
      this._channelCount = channelCount;
    }

    public void Execute(int i)
    {
      float num1 = 0.0f;
      for (int index = 0; index < this._channelCount; ++index)
      {
        float multiChannelSample = this._multiChannelSamples[i * this._channelCount + index];
        num1 += multiChannelSample;
      }
      float num2 = num1 / (float) this._channelCount;
      this.monoSamples[i] = num2;
    }
  }
}
