// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.ArcEditorData
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using UnityEngine;

namespace BeatmapEditor3D.DataModels
{
  public class ArcEditorData : BaseSliderEditorData, IEquatable<ArcEditorData>
  {
    public readonly float controlPointLengthMultiplier;
    public readonly NoteCutDirection tailCutDirection;
    public readonly float tailControlPointLengthMultiplier;
    public readonly SliderMidAnchorMode midAnchorMode;

    public static ArcEditorData CopyWithModifications(
      ArcEditorData data,
      BeatmapEditorObjectId? id = null,
      ColorType? colorType = null,
      float? beat = null,
      int? column = null,
      int? row = null,
      NoteCutDirection? cutDirection = null,
      float? controlPointLengthMultiplier = null,
      float? tailBeat = null,
      int? tailColumn = null,
      int? tailRow = null,
      NoteCutDirection? tailCutDirection = null,
      float? tailControlPointLengthMultiplier = null,
      SliderMidAnchorMode? midAnchorMode = null)
    {
      return ArcEditorData.CreateNewWithId(id ?? data.id, colorType ?? data.colorType, beat ?? data.beat, column ?? data.column, row ?? data.row, cutDirection ?? data.cutDirection, controlPointLengthMultiplier ?? data.controlPointLengthMultiplier, tailBeat ?? data.tailBeat, tailColumn ?? data.tailColumn, tailRow ?? data.tailRow, tailCutDirection ?? data.tailCutDirection, tailControlPointLengthMultiplier ?? data.tailControlPointLengthMultiplier, midAnchorMode ?? data.midAnchorMode);
    }

    public static ArcEditorData CreateNew(
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
      SliderMidAnchorMode midAnchorMode)
    {
      return ArcEditorData.CreateNewWithId(BeatmapEditorObjectId.NewId(), colorType, beat, column, row, cutDirection, controlPointLengthMultiplier, tailBeat, tailColumn, tailRow, tailCutDirection, tailControlPointLengthMultiplier, midAnchorMode);
    }

    public static ArcEditorData CreateNewWithId(
      BeatmapEditorObjectId id,
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
      SliderMidAnchorMode midAnchorMode)
    {
      return new ArcEditorData(id, colorType, beat, column, row, cutDirection, controlPointLengthMultiplier, tailBeat, tailColumn, tailRow, tailCutDirection, tailControlPointLengthMultiplier, midAnchorMode);
    }

    private ArcEditorData(
      BeatmapEditorObjectId id,
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
      SliderMidAnchorMode midAnchorMode)
      : base(id, beat, colorType, column, row, cutDirection, tailBeat, tailColumn, tailRow)
    {
      this.controlPointLengthMultiplier = controlPointLengthMultiplier;
      this.tailCutDirection = tailCutDirection;
      this.tailControlPointLengthMultiplier = tailControlPointLengthMultiplier;
      this.midAnchorMode = midAnchorMode;
    }

    private ArcEditorData(ArcEditorData other)
      : base((BaseSliderEditorData) other)
    {
      this.controlPointLengthMultiplier = other.controlPointLengthMultiplier;
      this.tailCutDirection = other.tailCutDirection;
      this.tailControlPointLengthMultiplier = other.tailControlPointLengthMultiplier;
      this.midAnchorMode = other.midAnchorMode;
    }

    public bool PositionEquals(ArcEditorData other) => this.PositionEquals((BaseBeatmapObjectEditorData) other) && AudioTimeHelper.IsBeatSame(this.tailBeat, other.tailBeat) && this.tailColumn == other.tailColumn && this.tailRow == other.tailRow;

    public bool ValueEquals(ArcEditorData other) => this.PositionEquals(other) && this.colorType == other.colorType && this.midAnchorMode == other.midAnchorMode && this.cutDirection == other.cutDirection && Mathf.Approximately(this.controlPointLengthMultiplier, other.controlPointLengthMultiplier) && this.tailCutDirection == other.tailCutDirection && Mathf.Approximately(this.tailControlPointLengthMultiplier, other.tailControlPointLengthMultiplier);

    public bool Equals(ArcEditorData other)
    {
      if ((object) other == null)
        return false;
      if ((object) this == (object) other)
        return true;
      return !(this.GetType() != other.GetType()) && this.InstanceEquals((BaseEditorData) other) && this.ValueEquals(other);
    }

    public override bool Equals(object obj) => this.Equals(obj as ArcEditorData);

    public override int GetHashCode() => (this.beat, this.colorType, this.column, this.row, this.cutDirection, this.controlPointLengthMultiplier, this.tailBeat, this.tailColumn, this.tailRow, this.tailCutDirection, this.tailControlPointLengthMultiplier, this.midAnchorMode).GetHashCode();

    public static bool operator ==(ArcEditorData lhs, ArcEditorData rhs) => (object) lhs != null ? lhs.Equals(rhs) : (object) rhs == null;

    public static bool operator !=(ArcEditorData lhs, ArcEditorData rhs) => !(lhs == rhs);
  }
}
