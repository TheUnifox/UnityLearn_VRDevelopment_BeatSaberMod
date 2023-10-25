// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.BeatmapObjectsState
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Types;
using UnityEngine;

namespace BeatmapEditor3D.DataModels
{
  public class BeatmapObjectsState : IReadonlyBeatmapObjectsState
  {
    public const int kMinSliderSliceCount = 2;
    public const int kMaxSliderSliceCount = 100;
    public const float kMinSliderSquishAmount = 0.5f;
    public const float kMaxSliderSquishAmount = 4f;
    private int _sliderSliceCount = 2;
    private float _sliderSquishAmount = 1f;

    public BeatmapObjectType beatmapObjectType { get; set; }

    public BeatmapEditorObjectId hoveredBeatmapObjectId { get; set; } = BeatmapEditorObjectId.invalid;

    public BeatmapObjectCellData hoveredCellData => this.hoveredGridCellData ?? this.hoveredNoteCellData;

    public BeatmapObjectCellData hoveredNoteCellData { get; set; }

    public BeatmapObjectCellData hoveredGridCellData { get; set; }

    public float playStartBeat { get; set; }

    public ColorType noteColorType { get; set; }

    public NoteCutDirection noteCutDirection { get; set; }

    public int noteAngle { get; set; }

    public float obstacleDuration { get; set; } = 2f;

    public int sliderSliceCount
    {
      get => this._sliderSliceCount;
      set => this._sliderSliceCount = Mathf.Clamp(value, 2, 100);
    }

    public float sliderSquishAmount
    {
      get => this._sliderSquishAmount;
      set => this._sliderSquishAmount = Mathf.Clamp(value, 0.5f, 4f);
    }

    public SliderMidAnchorMode arcMidAnchorMode { get; set; }

    public SliderData.Type sliderType { get; set; }

    public float arcControlPointLengthMultiplier { get; set; } = 1f;

    public float arcTailControlPointLengthMultiplier { get; set; } = 1f;

    public void Reset()
    {
      this.beatmapObjectType = BeatmapObjectType.Note;
      this.hoveredBeatmapObjectId = BeatmapEditorObjectId.invalid;
      this.hoveredNoteCellData = (BeatmapObjectCellData) null;
      this.hoveredGridCellData = (BeatmapObjectCellData) null;
      this.noteColorType = ColorType.ColorA;
      this.noteCutDirection = NoteCutDirection.Up;
      this.noteAngle = 0;
      this.obstacleDuration = 2f;
      this.sliderType = SliderData.Type.Normal;
      this.arcControlPointLengthMultiplier = 1f;
      this.arcTailControlPointLengthMultiplier = 1f;
      this.sliderSliceCount = 2;
      this.sliderSquishAmount = 1f;
    }
  }
}
