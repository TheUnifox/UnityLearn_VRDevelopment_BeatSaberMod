// Decompiled with JetBrains decompiler
// Type: UnityOpus.Encoder
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;

namespace UnityOpus
{
  public class Encoder : IDisposable
  {
    protected int bitrate;
    protected int complexity;
    protected OpusSignal signal;
    protected IntPtr encoder;
    protected NumChannels channels;
    protected bool disposedValue;

    public int Bitrate
    {
      get => this.bitrate;
      set
      {
        Library.OpusEncoderSetBitrate(this.encoder, value);
        this.bitrate = value;
      }
    }

    public int Complexity
    {
      get => this.complexity;
      set
      {
        Library.OpusEncoderSetComplexity(this.encoder, value);
        this.complexity = value;
      }
    }

    public OpusSignal Signal
    {
      get => this.signal;
      set
      {
        Library.OpusEncoderSetSignal(this.encoder, value);
        this.signal = value;
      }
    }

    public Encoder(
      SamplingFrequency samplingFrequency,
      NumChannels channels,
      OpusApplication application)
    {
      this.channels = channels;
      ErrorCode error;
      this.encoder = Library.OpusEncoderCreate(samplingFrequency, channels, application, out error);
      if (error == ErrorCode.OK)
        return;
      Debug.LogError((object) ("[UnityOpus] Failed to init encoder. Error code: " + error.ToString()));
      this.encoder = IntPtr.Zero;
    }

    public virtual int Encode(float[] pcm, int count, byte[] output) => this.encoder == IntPtr.Zero ? 0 : Library.OpusEncodeFloat(this.encoder, pcm, count / (int) this.channels, output, output.Length);

    protected virtual void Dispose(bool disposing)
    {
      if (this.disposedValue || this.encoder == IntPtr.Zero)
        return;
      Library.OpusEncoderDestroy(this.encoder);
      this.encoder = IntPtr.Zero;
      this.disposedValue = true;
    }

    ~Encoder() => this.Dispose(false);

    public virtual void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }
  }
}
