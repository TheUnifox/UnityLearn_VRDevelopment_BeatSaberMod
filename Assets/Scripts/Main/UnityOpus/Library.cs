// Decompiled with JetBrains decompiler
// Type: UnityOpus.Library
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Runtime.InteropServices;

namespace UnityOpus
{
  public class Library
  {
    public const int maximumPacketDuration = 5760;
    protected const string dllName = "UnityOpus";

    [DllImport("UnityOpus")]
    public static extern IntPtr OpusEncoderCreate(
      SamplingFrequency samplingFrequency,
      NumChannels channels,
      OpusApplication application,
      out ErrorCode error);

    [DllImport("UnityOpus")]
    public static extern int OpusEncode(
      IntPtr encoder,
      short[] pcm,
      int frameSize,
      byte[] data,
      int maxDataBytes);

    [DllImport("UnityOpus")]
    public static extern int OpusEncodeFloat(
      IntPtr encoder,
      float[] pcm,
      int frameSize,
      byte[] data,
      int maxDataBytes);

    [DllImport("UnityOpus")]
    public static extern void OpusEncoderDestroy(IntPtr encoder);

    [DllImport("UnityOpus")]
    public static extern int OpusEncoderSetBitrate(IntPtr encoder, int bitrate);

    [DllImport("UnityOpus")]
    public static extern int OpusEncoderSetComplexity(IntPtr encoder, int complexity);

    [DllImport("UnityOpus")]
    public static extern int OpusEncoderSetSignal(IntPtr encoder, OpusSignal signal);

    [DllImport("UnityOpus")]
    public static extern IntPtr OpusDecoderCreate(
      SamplingFrequency samplingFrequency,
      NumChannels channels,
      out ErrorCode error);

    [DllImport("UnityOpus")]
    public static extern int OpusDecode(
      IntPtr decoder,
      byte[] data,
      int len,
      short[] pcm,
      int frameSize,
      int decodeFec);

    [DllImport("UnityOpus")]
    public static extern int OpusDecodeFloat(
      IntPtr decoder,
      byte[] data,
      int len,
      float[] pcm,
      int frameSize,
      int decodeFec);

    [DllImport("UnityOpus")]
    public static extern void OpusDecoderDestroy(IntPtr decoder);

    [DllImport("UnityOpus")]
    public static extern void OpusPcmSoftClip(
      float[] pcm,
      int frameSize,
      NumChannels channels,
      float[] softclipMem);
  }
}
