// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.ChainEditorData
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using UnityEngine;

namespace BeatmapEditor3D.DataModels
{
  public class ChainEditorData : BaseSliderEditorData, IEquatable<ChainEditorData>
  {
    public readonly int sliceCount;
    public readonly float squishAmount;

    public static ChainEditorData CopyWithModifications(
      ChainEditorData data,
      BeatmapEditorObjectId? id = null,
      float? beat = null,
      ColorType? colorType = null,
      int? column = null,
      int? row = null,
      NoteCutDirection? cutDirection = null,
      float? tailBeat = null,
      int? tailColumn = null,
      int? tailRow = null,
      int? sliceCount = null,
      float? squishAmount = null)
    {
      return ChainEditorData.CreateNewWithId(id ?? data.id, beat ?? data.beat, colorType ?? data.colorType, column ?? data.column, row ?? data.row, cutDirection ?? data.cutDirection, tailBeat ?? data.tailBeat, tailColumn ?? data.tailColumn, tailRow ?? data.tailRow, sliceCount ?? data.sliceCount, squishAmount ?? data.squishAmount);
    }

    public static ChainEditorData CreateNew(
      float beat,
      ColorType colorType,
      int column,
      int row,
      NoteCutDirection cutDirection,
      float tailBeat,
      int tailColumn,
      int tailRow,
      int sliceCount,
      float squishAmount)
    {
      return ChainEditorData.CreateNewWithId(BeatmapEditorObjectId.NewId(), beat, colorType, column, row, cutDirection, tailBeat, tailColumn, tailRow, sliceCount, squishAmount);
    }

    public static ChainEditorData CreateNewWithId(
      BeatmapEditorObjectId id,
      float beat,
      ColorType colorType,
      int column,
      int row,
      NoteCutDirection cutDirection,
      float tailBeat,
      int tailColumn,
      int tailRow,
      int sliceCount,
      float squishAmount)
    {
      return new ChainEditorData(id, beat, colorType, column, row, cutDirection, tailBeat, tailColumn, tailRow, sliceCount, squishAmount);
    }

    private ChainEditorData(
      BeatmapEditorObjectId id,
      float beat,
      ColorType colorType,
      int column,
      int row,
      NoteCutDirection cutDirection,
      float tailBeat,
      int tailColumn,
      int tailRow,
      int sliceCount,
      float squishAmount)
      : base(id, beat, colorType, column, row, cutDirection, tailBeat, tailColumn, tailRow)
    {
      this.sliceCount = sliceCount;
      this.squishAmount = squishAmount;
    }

    private ChainEditorData(ChainEditorData other)
      : base((BaseSliderEditorData) other)
    {
      this.sliceCount = other.sliceCount;
      this.squishAmount = other.squishAmount;
    }

    public bool PositionEquals(ChainEditorData other) => this.PositionEquals((BaseBeatmapObjectEditorData) other) && AudioTimeHelper.IsBeatSame(this.tailBeat, other.tailBeat) && this.tailColumn == other.tailColumn && this.tailRow == other.tailRow;

    public bool ValueEquals(ChainEditorData other) => this.PositionEquals(other) && this.colorType == other.colorType && this.cutDirection == other.cutDirection && this.sliceCount == other.sliceCount && Mathf.Approximately(this.squishAmount, other.squishAmount);

    public bool Equals(ChainEditorData other)
    {
      if ((object) other == null)
        return false;
      if ((object) this == (object) other)
        return true;
      return !(this.GetType() != other.GetType()) && this.InstanceEquals((BaseEditorData) other) && this.ValueEquals(other);
    }

    public override bool Equals(object obj) => this.Equals(obj as ChainEditorData);

    public override int GetHashCode() => (this.beat, this.colorType, this.column, this.row, this.cutDirection, this.tailBeat, this.tailColumn, this.tailRow, this.sliceCount, this.squishAmount).GetHashCode();

    public static bool operator ==(ChainEditorData lhs, ChainEditorData rhs) => (object) lhs != null ? lhs.Equals(rhs) : (object) rhs == null;

    public static bool operator !=(ChainEditorData lhs, ChainEditorData rhs) => !(lhs == rhs);
  }
}
