// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.NoteEditorData
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Types;
using System;

namespace BeatmapEditor3D.DataModels
{
  public sealed class NoteEditorData : BaseBeatmapObjectEditorData, IEquatable<NoteEditorData>
  {
    public readonly NoteType noteType;
    public readonly ColorType type;
    public readonly NoteCutDirection cutDirection;
    public readonly int angle;

    public static NoteEditorData Copy(NoteEditorData noteData) => new NoteEditorData(noteData);

    public static NoteEditorData CopyWithoutId(NoteEditorData noteData) => NoteEditorData.CreateNew(noteData.beat, noteData.column, noteData.row, noteData.type, noteData.noteType, noteData.cutDirection, noteData.angle);

    public static NoteEditorData CopyWithModifications(
      NoteEditorData noteData,
      BeatmapEditorObjectId? id = null,
      float? beat = null,
      int? column = null,
      int? row = null,
      ColorType? type = null,
      NoteType? noteType = null,
      NoteCutDirection? cutDirection = null,
      int? angle = null)
    {
      return NoteEditorData.CreateNewWithId(id ?? noteData.id, (float) ((double) beat ?? (double) noteData.beat), column ?? noteData.column, row ?? noteData.row, (ColorType) ((int) type ?? (int) noteData.type), (NoteType) ((int) noteType ?? (int) noteData.noteType), (NoteCutDirection) ((int) cutDirection ?? (int) noteData.cutDirection), angle ?? noteData.angle);
    }

    public static NoteEditorData CreateNew(
      float beat,
      int column,
      int row,
      ColorType type,
      NoteType noteType,
      NoteCutDirection cutDirection,
      int angle)
    {
      return NoteEditorData.CreateNewWithId(BeatmapEditorObjectId.NewId(), beat, column, row, type, noteType, cutDirection, angle);
    }

    public static NoteEditorData CreateNewWithId(
      BeatmapEditorObjectId noteId,
      float beat,
      int column,
      int row,
      ColorType type,
      NoteType noteType,
      NoteCutDirection cutDirection,
      int angle)
    {
      return new NoteEditorData(noteId, beat, column, row, type, noteType, cutDirection, angle);
    }

    private NoteEditorData(NoteEditorData other)
      : base((BaseBeatmapObjectEditorData) other)
    {
      this.type = other.type;
      this.noteType = other.noteType;
      this.cutDirection = other.cutDirection;
      this.angle = other.angle;
    }

    private NoteEditorData(
      BeatmapEditorObjectId id,
      float beat,
      int column,
      int row,
      ColorType type,
      NoteType noteType,
      NoteCutDirection cutDirection,
      int angle)
      : base(id, beat, column, row)
    {
      this.type = type;
      this.noteType = noteType;
      this.cutDirection = cutDirection;
      this.angle = angle;
    }

    public bool ValueEquals(NoteEditorData other) => this.PositionEquals((BaseBeatmapObjectEditorData) other) && this.type == other.type && this.cutDirection == other.cutDirection && this.angle == other.angle && this.noteType == other.noteType;

    public bool Equals(NoteEditorData other)
    {
      if ((object) other == null)
        return false;
      if ((object) this == (object) other)
        return true;
      return !(other.GetType() != this.GetType()) && this.InstanceEquals((BaseEditorData) other) && this.ValueEquals(other);
    }

    public override bool Equals(object obj) => this.Equals(obj as NoteEditorData);

    public override int GetHashCode() => (this.beat, this.column, this.row, this.type, this.cutDirection, this.angle, this.noteType).GetHashCode();

    public static bool operator ==(NoteEditorData lhs, NoteEditorData rhs) => (object) lhs != null ? lhs.Equals(rhs) : (object) rhs == null;

    public static bool operator !=(NoteEditorData lhs, NoteEditorData rhs) => !(lhs == rhs);
  }
}
