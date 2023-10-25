// Decompiled with JetBrains decompiler
// Type: SavWav
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.IO;
using System.Text;
using UnityEngine;

public abstract class SavWav
{
  private const uint HeaderSize = 44;
  private const float RescaleFactor = 32767f;

  public static void Save(string filepath, AudioClip clip, float start, float duration)
  {
    if (!filepath.ToLower().EndsWith(".wav"))
      filepath += ".wav";
    Directory.CreateDirectory(Path.GetDirectoryName(filepath));
    using (FileStream output = new FileStream(filepath, FileMode.Create))
    {
      using (BinaryWriter binaryWriter = new BinaryWriter((Stream) output))
      {
        uint length;
        byte[] wav = SavWav.GetWav(clip, out length, start, duration);
        binaryWriter.Write(wav, 0, (int) length);
      }
    }
  }

  public static byte[] GetWav(AudioClip clip, out uint length, float start, float duration)
  {
    uint samplesAfterTrimming;
    byte[] stream = SavWav.ConvertAndWrite(clip, out length, out samplesAfterTrimming, start, duration);
    SavWav.WriteHeader(stream, clip, length, samplesAfterTrimming);
    return stream;
  }

  private static byte[] ConvertAndWrite(
    AudioClip clip,
    out uint length,
    out uint samplesAfterTrimming,
    float start,
    float duration)
  {
    float[] data = new float[clip.samples * clip.channels];
    clip.GetData(data, 0);
    int length1 = data.Length;
    int num1 = (int) ((double) start * (double) clip.frequency * (double) clip.channels);
    int num2 = Math.Min(data.Length - 1, num1 + (int) ((double) duration * (double) clip.frequency * (double) clip.channels));
    byte[] numArray1 = new byte[(long) (length1 * 2) + 44L];
    uint num3 = 44;
    for (int index1 = num1; index1 <= num2; ++index1)
    {
      short num4 = (short) ((double) data[index1] * (double) short.MaxValue);
      byte[] numArray2 = numArray1;
      int index2 = (int) num3;
      uint num5 = (uint) (index2 + 1);
      int num6 = (int) (byte) num4;
      numArray2[index2] = (byte) num6;
      byte[] numArray3 = numArray1;
      int index3 = (int) num5;
      num3 = (uint) (index3 + 1);
      int num7 = (int) (byte) ((uint) num4 >> 8);
      numArray3[index3] = (byte) num7;
    }
    length = num3;
    samplesAfterTrimming = (uint) (num2 - num1 + 1);
    return numArray1;
  }

  private static void AddDataToBuffer(byte[] buffer, ref uint offset, byte[] addBytes)
  {
    foreach (byte addByte in addBytes)
      buffer[(int) offset++] = addByte;
  }

  private static void WriteHeader(byte[] stream, AudioClip clip, uint length, uint samples)
  {
    int frequency = clip.frequency;
    ushort channels = (ushort) clip.channels;
    uint offset = 0;
    byte[] bytes1 = Encoding.UTF8.GetBytes("RIFF");
    SavWav.AddDataToBuffer(stream, ref offset, bytes1);
    byte[] bytes2 = BitConverter.GetBytes(length - 8U);
    SavWav.AddDataToBuffer(stream, ref offset, bytes2);
    byte[] bytes3 = Encoding.UTF8.GetBytes("WAVE");
    SavWav.AddDataToBuffer(stream, ref offset, bytes3);
    byte[] bytes4 = Encoding.UTF8.GetBytes("fmt ");
    SavWav.AddDataToBuffer(stream, ref offset, bytes4);
    byte[] bytes5 = BitConverter.GetBytes(16U);
    SavWav.AddDataToBuffer(stream, ref offset, bytes5);
    byte[] bytes6 = BitConverter.GetBytes((ushort) 1);
    SavWav.AddDataToBuffer(stream, ref offset, bytes6);
    byte[] bytes7 = BitConverter.GetBytes(channels);
    SavWav.AddDataToBuffer(stream, ref offset, bytes7);
    byte[] bytes8 = BitConverter.GetBytes((uint) frequency);
    SavWav.AddDataToBuffer(stream, ref offset, bytes8);
    byte[] bytes9 = BitConverter.GetBytes((uint) (frequency * (int) channels * 2));
    SavWav.AddDataToBuffer(stream, ref offset, bytes9);
    ushort num = (ushort) ((uint) channels * 2U);
    SavWav.AddDataToBuffer(stream, ref offset, BitConverter.GetBytes(num));
    byte[] bytes10 = BitConverter.GetBytes((ushort) 16);
    SavWav.AddDataToBuffer(stream, ref offset, bytes10);
    byte[] bytes11 = Encoding.UTF8.GetBytes("data");
    SavWav.AddDataToBuffer(stream, ref offset, bytes11);
    byte[] bytes12 = BitConverter.GetBytes(samples * 2U);
    SavWav.AddDataToBuffer(stream, ref offset, bytes12);
  }
}
