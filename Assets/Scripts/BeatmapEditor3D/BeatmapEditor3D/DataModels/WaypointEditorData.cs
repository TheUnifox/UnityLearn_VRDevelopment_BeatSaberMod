// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.WaypointEditorData
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;

namespace BeatmapEditor3D.DataModels
{
  public sealed class WaypointEditorData : 
    BaseBeatmapObjectEditorData,
    IEquatable<WaypointEditorData>
  {
    public readonly OffsetDirection offsetDirection;

    public static WaypointEditorData Copy(WaypointEditorData waypointData) => new WaypointEditorData(waypointData);

    public static WaypointEditorData CopyWithoutId(WaypointEditorData waypointData) => new WaypointEditorData(BeatmapEditorObjectId.NewId(), waypointData.beat, waypointData.column, waypointData.row, waypointData.offsetDirection);

    public static WaypointEditorData CopyWithModifications(
      WaypointEditorData waypointData,
      BeatmapEditorObjectId? id = null,
      float? beat = null,
      int? column = null,
      int? row = null,
      OffsetDirection? offsetDirection = null)
    {
      return WaypointEditorData.CreateNewWithId(id ?? waypointData.id, (float) ((double) beat ?? (double) waypointData.beat), column ?? waypointData.column, row ?? waypointData.row, (OffsetDirection) ((int) offsetDirection ?? (int) waypointData.offsetDirection));
    }

    public static WaypointEditorData CreateNew(
      float beat,
      int column,
      int row,
      OffsetDirection offsetDirection)
    {
      return new WaypointEditorData(BeatmapEditorObjectId.NewId(), beat, column, row, offsetDirection);
    }

    public static WaypointEditorData CreateNewWithId(
      BeatmapEditorObjectId waypointId,
      float beat,
      int column,
      int row,
      OffsetDirection offsetDirection)
    {
      return new WaypointEditorData(waypointId, beat, column, row, offsetDirection);
    }

    private WaypointEditorData(WaypointEditorData other)
      : base((BaseBeatmapObjectEditorData) other)
    {
      this.offsetDirection = other.offsetDirection;
    }

    private WaypointEditorData(
      BeatmapEditorObjectId id,
      float beat,
      int column,
      int row,
      OffsetDirection offsetDirection)
      : base(id, beat, column, row)
    {
      this.offsetDirection = offsetDirection;
    }

    public bool ValueEquals(WaypointEditorData other) => this.PositionEquals((BaseBeatmapObjectEditorData) other) && this.offsetDirection == other.offsetDirection;

    public bool Equals(WaypointEditorData other)
    {
      if ((object) other == null)
        return false;
      if ((object) this == (object) other)
        return true;
      return !(other.GetType() != this.GetType()) && this.InstanceEquals((BaseEditorData) other) && this.ValueEquals(other);
    }

    public override bool Equals(object obj) => this.Equals(obj as WaypointEditorData);

    public override int GetHashCode() => (this.beat, this.column, this.row, this.offsetDirection).GetHashCode();

    public static bool operator ==(WaypointEditorData lhs, WaypointEditorData rhs) => (object) lhs != null ? lhs.Equals(rhs) : (object) rhs == null;

    public static bool operator !=(WaypointEditorData lhs, WaypointEditorData rhs) => !(lhs == rhs);
  }
}
