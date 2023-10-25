// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.BeatmapState
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Types;

namespace BeatmapEditor3D.DataModels
{
  public class BeatmapState : IReadonlyBeatmapState
  {
    private float _beat;

    public InteractionMode interactionMode { get; set; }

    public BeatmapEditingMode editingMode { get; set; }

    public float beatOffset { get; set; }

    public float prevBeat { get; private set; }

    public float beat
    {
      get => this._beat;
      set
      {
        this.prevBeat = this._beat;
        this._beat = value;
      }
    }

    public float offsetBeat => this.beat - this.beatOffset;

    public int rotation { get; set; }

    public BeatmapPreviewData previewData { get; set; } = new BeatmapPreviewData();

    public int subdivision => this.beatSubdivisionsModel.currentSubdivision;

    public int prevSubdivision => this.beatSubdivisionsModel.prevSubdivision;

    public BeatSubdivisionsModel beatSubdivisionsModel { get; } = new BeatSubdivisionsModel();

    public bool cameraMoving { get; set; }

    public bool autoRotation { get; set; } = true;

    public void Reset()
    {
      this.interactionMode = InteractionMode.Place;
      this.editingMode = BeatmapEditingMode.Objects;
      this.beatOffset = 0.0f;
      this.prevBeat = 0.0f;
      this.beat = 0.0f;
      this.rotation = 0;
      this.previewData = new BeatmapPreviewData();
      this.beatSubdivisionsModel.Reset();
      this.cameraMoving = false;
      this.autoRotation = true;
    }
  }
}
