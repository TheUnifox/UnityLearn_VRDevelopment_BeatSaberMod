// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.SliderEditorData
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using UnityEngine;

namespace BeatmapEditor3D.DataModels
{
  public class SliderEditorData : BaseEditorData, IEquatable<SliderEditorData>
  {
    public readonly SliderData.Type type;
    public readonly ColorType colorType;
    public readonly int column;
    public readonly int row;
    public readonly NoteCutDirection cutDirection;
    public readonly float controlPointLengthMultiplier;
    public readonly float tailBeat;
    public readonly int tailColumn;
    public readonly int tailRow;
    public readonly NoteCutDirection tailCutDirection;
    public readonly float tailControlPointLengthMultiplier;
    public readonly SliderMidAnchorMode midAnchorMode;
    public readonly int sliceCount;
    public readonly float squishAmount;

    public static SliderEditorData Copy(SliderEditorData sliderData) => new SliderEditorData(sliderData);

    public static SliderEditorData CopyWithoutId(SliderEditorData sliderData) => SliderEditorData.CreateNew(sliderData.type, sliderData.colorType, sliderData.beat, sliderData.column, sliderData.row, sliderData.cutDirection, sliderData.controlPointLengthMultiplier, sliderData.tailBeat, sliderData.tailColumn, sliderData.tailRow, sliderData.tailCutDirection, sliderData.tailControlPointLengthMultiplier, sliderData.midAnchorMode, sliderData.sliceCount, sliderData.squishAmount);

    public static SliderEditorData CopyWithModifications(
      SliderEditorData sliderData,
      BeatmapEditorObjectId? id = null,
      SliderData.Type? type = null,
      ColorType? colorType = null,
      float? headBeat = null,
      int? headColumn = null,
      int? headRow = null,
      NoteCutDirection? cutDirection = null,
      float? controlPointLengthMultiplier = null,
      float? tailBeat = null,
      int? tailColumn = null,
      int? tailRow = null,
      NoteCutDirection? tailCutDirection = null,
      float? tailControlPointLengthMultiplier = null,
      SliderMidAnchorMode? midAnchorMode = null,
      int? sliceCount = null,
      float? squishAmount = null)
    {
      return SliderEditorData.CreateNewWithId(id ?? sliderData.id, (SliderData.Type) ((int) type ?? (int) sliderData.type), (ColorType) ((int) colorType ?? (int) sliderData.colorType), (float) ((double) headBeat ?? (double) sliderData.beat), headColumn ?? sliderData.column, headRow ?? sliderData.row, (NoteCutDirection) ((int) cutDirection ?? (int) sliderData.cutDirection), (float) ((double) controlPointLengthMultiplier ?? (double) sliderData.controlPointLengthMultiplier), (float) ((double) tailBeat ?? (double) sliderData.tailBeat), tailColumn ?? sliderData.tailColumn, tailRow ?? sliderData.tailRow, (NoteCutDirection) ((int) tailCutDirection ?? (int) sliderData.tailCutDirection), (float) ((double) tailControlPointLengthMultiplier ?? (double) sliderData.tailControlPointLengthMultiplier), (SliderMidAnchorMode) ((int) midAnchorMode ?? (int) sliderData.midAnchorMode), sliceCount ?? sliderData.sliceCount, (float) ((double) squishAmount ?? (double) sliderData.squishAmount));
    }

    public static SliderEditorData CreateNew(
      SliderData.Type type,
      ColorType colorType,
      float beat,
      int column,
      int row,
      NoteCutDirection cutDirection,
      float controlPointLengthMultiplier,
      float tailBeat,
      int tailColumn,
      int tailRow,
      NoteCutDirection tailCutDirection,
      float tailControlPointLengthMultiplier,
      SliderMidAnchorMode midAnchorMode,
      int sliceCount,
      float squishAmount)
    {
      return SliderEditorData.CreateNewWithId(BeatmapEditorObjectId.NewId(), type, colorType, beat, column, row, cutDirection, controlPointLengthMultiplier, tailBeat, tailColumn, tailRow, tailCutDirection, tailControlPointLengthMultiplier, midAnchorMode, sliceCount, squishAmount);
    }

    public static SliderEditorData CreateNewWithId(
      BeatmapEditorObjectId sliderId,
      SliderData.Type type,
      ColorType colorType,
      float beat,
      int column,
      int row,
      NoteCutDirection cutDirection,
      float controlPointLengthMultiplier,
      float tailBeat,
      int tailColumn,
      int tailRow,
      NoteCutDirection tailCutDirection,
      float tailControlPointLengthMultiplier,
      SliderMidAnchorMode midAnchorMode,
      int sliceCount,
      float squishAmount)
    {
      return new SliderEditorData(sliderId, type, colorType, beat, column, row, cutDirection, controlPointLengthMultiplier, tailBeat, tailColumn, tailRow, tailCutDirection, tailControlPointLengthMultiplier, midAnchorMode, sliceCount, squishAmount);
    }

    private SliderEditorData(SliderEditorData other)
      : base(other.id, other.beat)
    {
      this.type = other.type;
      this.colorType = other.colorType;
      this.column = other.column;
      this.row = other.row;
      this.cutDirection = other.cutDirection;
      this.controlPointLengthMultiplier = other.controlPointLengthMultiplier;
      this.tailBeat = other.tailBeat;
      this.tailColumn = other.tailColumn;
      this.tailRow = other.tailRow;
      this.tailCutDirection = other.tailCutDirection;
      this.tailControlPointLengthMultiplier = other.tailControlPointLengthMultiplier;
      this.midAnchorMode = other.midAnchorMode;
      this.sliceCount = other.sliceCount;
      this.squishAmount = other.squishAmount;
    }

    private SliderEditorData(
      BeatmapEditorObjectId id,
      SliderData.Type type,
      ColorType colorType,
      float beat,
      int column,
      int row,
      NoteCutDirection cutDirection,
      float controlPointLengthMultiplier,
      float tailBeat,
      int tailColumn,
      int tailRow,
      NoteCutDirection tailCutDirection,
      float tailControlPointLengthMultiplier,
      SliderMidAnchorMode midAnchorMode,
      int sliceCount,
      float squishAmount)
      : base(id, beat)
    {
      this.type = type;
      this.colorType = colorType;
      this.column = column;
      this.row = row;
      this.cutDirection = cutDirection;
      this.controlPointLengthMultiplier = controlPointLengthMultiplier;
      this.tailBeat = tailBeat;
      this.tailColumn = tailColumn;
      this.tailRow = tailRow;
      this.tailCutDirection = tailCutDirection;
      this.tailControlPointLengthMultiplier = tailControlPointLengthMultiplier;
      this.midAnchorMode = midAnchorMode;
      this.sliceCount = sliceCount;
      this.squishAmount = squishAmount;
    }

    public bool PositionEquals(SliderEditorData other) => AudioTimeHelper.IsBeatSame(this.beat, other.beat) && this.column == other.column && this.row == other.row && AudioTimeHelper.IsBeatSame(this.tailBeat, other.tailBeat) && this.tailColumn == other.tailColumn && this.tailRow == other.tailRow;

    public bool ValueEquals(SliderEditorData other) => this.PositionEquals(other) && this.type == other.type && this.colorType == other.colorType && this.cutDirection == other.cutDirection && Mathf.Approximately(this.controlPointLengthMultiplier, other.controlPointLengthMultiplier) && this.tailCutDirection == other.tailCutDirection && Mathf.Approximately(this.tailControlPointLengthMultiplier, other.tailControlPointLengthMultiplier) && this.midAnchorMode == other.midAnchorMode && this.sliceCount == other.sliceCount && Mathf.Approximately(this.squishAmount, other.squishAmount);

    public bool Equals(SliderEditorData other)
    {
      if ((object) other == null)
        return false;
      if ((object) this == (object) other)
        return true;
      return !(this.GetType() != other.GetType()) && this.InstanceEquals((BaseEditorData) other) && this.ValueEquals(other);
    }

    public override bool Equals(object obj) => this.Equals(obj as SliderEditorData);

    public override int GetHashCode() => (this.type, this.colorType, this.beat, this.column, this.row, this.cutDirection, this.controlPointLengthMultiplier, this.tailBeat, this.tailColumn, this.tailRow, this.tailCutDirection, this.tailControlPointLengthMultiplier, this.midAnchorMode, this.sliceCount, this.squishAmount).GetHashCode();

    public static bool operator ==(SliderEditorData lhs, SliderEditorData rhs) => (object) lhs != null ? lhs.Equals(rhs) : (object) rhs == null;

    public static bool operator !=(SliderEditorData lhs, SliderEditorData rhs) => !(lhs == rhs);
  }
}
