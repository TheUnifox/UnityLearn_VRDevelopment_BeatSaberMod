// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.StaticAudioWaveformView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Views;

namespace BeatmapEditor3D
{
  public class StaticAudioWaveformView : AudioWaveformView
  {
    protected override void SetupWaveform()
    {
      base.SetupWaveform();
      this.RenderWaveform(0, this._audioClip.samples);
    }
  }
}
