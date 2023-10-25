// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.ComputeWaveformBitmapJob
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace BeatmapEditor3D
{
  [BurstCompile]
  public struct ComputeWaveformBitmapJob : IJob
  {
    [WriteOnly]
    private NativeArray<Color32> _pixels;
    [ReadOnly]
    private readonly NativeArray<SamplesLod> _inputSamples;
    [ReadOnly]
    private readonly Color32 _waveformColor;
    [ReadOnly]
    private readonly int _start;
    [ReadOnly]
    private readonly int _end;
    [ReadOnly]
    private readonly int _halfThickness;
    [ReadOnly]
    private readonly int _width;
    [ReadOnly]
    private readonly int _height;

    public ComputeWaveformBitmapJob(
      NativeArray<Color32> pixels,
      NativeArray<SamplesLod> inputSamples,
      Color32 waveformColor,
      int start,
      int end,
      int halfThickness,
      int width,
      int height)
    {
      this._pixels = pixels;
      this._inputSamples = inputSamples;
      this._waveformColor = waveformColor;
      this._start = start;
      this._end = end;
      this._halfThickness = halfThickness;
      this._width = width;
      this._height = height;
    }

    public void Execute()
    {
      int x1 = int.MinValue;
      int x2 = int.MaxValue;
      for (int index1 = 0; index1 < this._width; ++index1)
      {
        int index2 = index1 + this._start;
        if (index2 >= 0)
        {
          int num1 = index2;
          NativeArray<SamplesLod> inputSamples = this._inputSamples;
          int length = inputSamples.Length;
          if (num1 >= length || index2 > this._end)
            break;
          inputSamples = this._inputSamples;
          int y1 = inputSamples[index2].outerMinSample - this._halfThickness;
          inputSamples = this._inputSamples;
          int y2 = inputSamples[index2].outerMaxSample + this._halfThickness;
          int num2 = math.max(0, math.min(x1, y1));
          int num3 = math.min(this._height - 1, math.max(x2, y2));
          x1 = y1;
          x2 = y2;
          for (int index3 = num2; index3 < num3; ++index3)
            this._pixels[index1 + this._width * index3] = this._waveformColor;
        }
      }
    }
  }
}
