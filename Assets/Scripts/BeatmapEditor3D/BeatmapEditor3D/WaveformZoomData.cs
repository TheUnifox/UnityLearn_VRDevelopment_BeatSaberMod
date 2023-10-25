// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.WaveformZoomData
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

namespace BeatmapEditor3D
{
  public readonly struct WaveformZoomData
  {
    public readonly int lod;
    public readonly int startWaveFormSample;
    public readonly int endWaveFormSample;

    public WaveformZoomData(int lod, int startWaveFormSample, int endWaveFormSample)
    {
      this.lod = lod;
      this.startWaveFormSample = startWaveFormSample;
      this.endWaveFormSample = endWaveFormSample;
    }
  }
}
