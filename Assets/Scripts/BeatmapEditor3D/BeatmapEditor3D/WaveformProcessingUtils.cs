// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.WaveformProcessingUtils
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

namespace BeatmapEditor3D
{
  public static class WaveformProcessingUtils
  {
    public static (int startSampleIdx, int endSampleIdx, int bracketSize) GetWaveformSizing(
      int textureWidth,
      int startSample,
      int endSample)
    {
      int num1 = endSample - startSample;
      int num2 = startSample;
      int num3 = endSample;
      int num4 = 1;
      for (int index = 0; index < 16; ++index)
      {
        if (num1 <= textureWidth)
          return (num2, num3, num4);
        num1 /= 2;
        num2 /= 2;
        num3 /= 2;
        num4 *= 2;
      }
      return (startSample, endSample, 1);
    }
  }
}
