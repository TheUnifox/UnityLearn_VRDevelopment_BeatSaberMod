// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.StaticAudioWaveformBeatsView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System.Collections;

namespace BeatmapEditor3D.Views
{
  public class StaticAudioWaveformBeatsView : AudioWaveformBeatsView
  {
    protected override void DidActivate()
    {
      base.DidActivate();
      PersistentSingleton<SharedCoroutineStarter>.instance.StartCoroutine(this.DisplayBeatsDelayed());
    }

    private IEnumerator DisplayBeatsDelayed()
    {
      StaticAudioWaveformBeatsView waveformBeatsView = this;
      yield return (object) null;
      yield return (object) null;
      waveformBeatsView.DisplayBeats(0, waveformBeatsView._beatmapDataModel.audioClip.samples);
    }
  }
}
