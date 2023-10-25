// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.WaveformDataModel
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class WaveformDataModel : ITickable, IDisposable
  {
    [Inject]
    private readonly TickableManager _tickableManager;
    [Inject]
    private readonly DisposableManager _disposableManager;
    [Inject]
    private readonly SignalBus _signalBus;
    private NativeArray<float> _multiChannelSamples;
    private NativeArray<float> _monoSamples;
    private JobHandle _handle;
    private bool _disposeSubscribed;
    private bool _updateSubscribed;

    public bool waveformDataCreated { get; private set; }

    public ComputeBuffer computeBuffer { get; private set; }

    public void Tick()
    {
      if (this.waveformDataCreated || !this._handle.IsCompleted)
        return;
      this._tickableManager.Remove((ITickable) this);
      this._updateSubscribed = false;
      this._handle.Complete();
      if (this._multiChannelSamples.IsCreated)
      {
        this._multiChannelSamples.Dispose();
        this._multiChannelSamples = new NativeArray<float>();
      }
      this.computeBuffer = new ComputeBuffer(this._monoSamples.Length, 4);
      this.computeBuffer.SetData<float>(this._monoSamples);
      this._monoSamples.Dispose();
      this._monoSamples = new NativeArray<float>();
      this.waveformDataCreated = true;
      this._signalBus.Fire<BeatmapDataModelSignals.WaveformDataProcessedSignal>();
    }

    public void Dispose()
    {
      if (this._multiChannelSamples.IsCreated)
      {
        this._multiChannelSamples.Dispose();
        this._multiChannelSamples = new NativeArray<float>();
      }
      this.Close();
    }

    public void PrepareWaveformData(AudioClip clip)
    {
      ComputeMonoSamplesJob computeMonoSamplesJob;
      (computeMonoSamplesJob, this._multiChannelSamples) = WaveformProcessingJobs.CreateComputeMonoSamplesJob(clip);
      JobHandle jobHandle = computeMonoSamplesJob.Schedule<ComputeMonoSamplesJob>(this._multiChannelSamples.Length / clip.channels, 32);
      this._monoSamples = computeMonoSamplesJob.monoSamples;
      this._handle = jobHandle;
      this._tickableManager.Add((ITickable) this);
      this._updateSubscribed = true;
      if (this._disposeSubscribed)
        return;
      this._disposableManager.Add((IDisposable) this);
      this._disposeSubscribed = true;
    }

    public void Close()
    {
      if (this._updateSubscribed)
      {
        this._tickableManager.Remove((ITickable) this);
        this._updateSubscribed = false;
      }
      this._handle.Complete();
      if (!this.waveformDataCreated)
        return;
      this.waveformDataCreated = false;
      if (this.computeBuffer != null)
      {
        this.computeBuffer.Dispose();
        this.computeBuffer = (ComputeBuffer) null;
      }
      if (this._monoSamples != new NativeArray<float>() && this._monoSamples.IsCreated)
      {
        this._monoSamples.Dispose();
        this._monoSamples = new NativeArray<float>();
      }
      if (!(this._multiChannelSamples != new NativeArray<float>()) || !this._multiChannelSamples.IsCreated)
        return;
      this._multiChannelSamples.Dispose();
      this._multiChannelSamples = new NativeArray<float>();
    }
  }
}
