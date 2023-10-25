// Decompiled with JetBrains decompiler
// Type: UnityOpus.Decoder
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;

namespace UnityOpus
{
  public class Decoder : IDisposable
  {
    public const int maximumPacketDuration = 5760;
    protected IntPtr decoder;
    protected readonly NumChannels channels;
    protected readonly float[] softclipMem;
    protected bool disposedValue;

    public Decoder(SamplingFrequency samplingFrequency, NumChannels channels)
    {
      this.channels = channels;
      ErrorCode error;
      this.decoder = Library.OpusDecoderCreate(samplingFrequency, channels, out error);
      if (error != ErrorCode.OK)
      {
        Debug.LogError((object) ("[UnityOpus] Failed to create Decoder. Error code is " + error.ToString()));
        this.decoder = IntPtr.Zero;
      }
      this.softclipMem = new float[(int) channels];
    }

    public virtual int Decode(byte[] data, int dataLength, float[] pcm, int decodeFec = 0)
    {
      if (this.decoder == IntPtr.Zero)
        return 0;
      int num = Library.OpusDecodeFloat(this.decoder, data, dataLength, pcm, pcm.Length / (int) this.channels, decodeFec);
      Library.OpusPcmSoftClip(pcm, num / (int) this.channels, this.channels, this.softclipMem);
      return num;
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.disposedValue || this.decoder == IntPtr.Zero)
        return;
      Library.OpusDecoderDestroy(this.decoder);
      this.decoder = IntPtr.Zero;
      this.disposedValue = true;
    }

    ~Decoder() => this.Dispose(false);

    public virtual void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }
  }
}
