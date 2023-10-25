// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BpmEditor.BpmEditorState
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

namespace BeatmapEditor3D.BpmEditor
{
  public class BpmEditorState
  {
    public const int kMinPreviewHalfSize = 5000;
    public const int kMaxPreviewHalfSize = 250000;
    public const float kDefaultPlaybackSpeed = 1f;
    public const float kDefaultMusicVolume = 1f;
    public const float kDefaultMetronomeVolume = 1f;

    public BpmEditorToolType bpmToolType { get; set; }

    public BpmEditorToolSnapType bpmToolSnapType { get; set; }

    public bool splitRegionSingleBeat { get; set; }

    public bool stretchRegion { get; set; }

    public int playStartSample { get; set; }

    public int sample { get; set; }

    public (int start, int end) previewRegionWindow { get; set; } = (0, 0);

    public int previewHalfSize { get; set; } = 44100;

    public int currentBpmRegionIdx { get; set; }

    public int hoverSample { get; set; }

    public int hoverBpmRegionIdx { get; set; }

    public float playbackSpeed { get; set; } = 1f;

    public float musicVolume { get; set; } = 1f;

    public float metronomeVolume { get; set; } = 1f;

    public BpmSubdivisionType bpmSubdivisionType { get; set; }

    public void Reset()
    {
      this.bpmToolType = BpmEditorToolType.Select;
      this.bpmToolSnapType = BpmEditorToolSnapType.No;
      this.splitRegionSingleBeat = false;
      this.stretchRegion = false;
      this.playStartSample = 0;
      this.sample = 0;
      this.previewRegionWindow = (0, 0);
      this.previewHalfSize = 44100;
      this.currentBpmRegionIdx = 0;
      this.hoverSample = 0;
      this.hoverBpmRegionIdx = 0;
      this.playbackSpeed = 1f;
      this.musicVolume = 1f;
      this.metronomeVolume = 1f;
      this.bpmSubdivisionType = BpmSubdivisionType.Normal;
    }
  }
}
