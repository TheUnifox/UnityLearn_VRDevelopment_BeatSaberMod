// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.EventBoxGroupEditorData
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;

namespace BeatmapEditor3D.DataModels
{
  public class EventBoxGroupEditorData : BaseEditorData, IEquatable<EventBoxGroupEditorData>
  {
    public readonly int groupId;
    public readonly EventBoxGroupEditorData.EventBoxGroupType type;

    public static EventBoxGroupEditorData CreateNew(
      float beat,
      int groupId,
      EventBoxGroupEditorData.EventBoxGroupType type)
    {
      return EventBoxGroupEditorData.CreateNewWithId(BeatmapEditorObjectId.NewId(), beat, groupId, type);
    }

    public static EventBoxGroupEditorData CreateNewWithId(
      BeatmapEditorObjectId id,
      float beat,
      int groupId,
      EventBoxGroupEditorData.EventBoxGroupType type)
    {
      return new EventBoxGroupEditorData(id, beat, groupId, type);
    }

    private EventBoxGroupEditorData(
      BeatmapEditorObjectId id,
      float beat,
      int groupId,
      EventBoxGroupEditorData.EventBoxGroupType type)
      : base(id, beat)
    {
      this.groupId = groupId;
      this.type = type;
    }

    public bool IsEventValidAtTime(float beat) => AudioTimeHelper.IsBeatSame(this.beat, beat);

    public bool PositionEquals(EventBoxGroupEditorData other) => AudioTimeHelper.IsBeatSame(this.beat, other.beat) && this.groupId == other.groupId;

    public bool ValueEquals(EventBoxGroupEditorData other) => this.groupId == other.groupId && this.type == other.type;

    public bool Equals(EventBoxGroupEditorData other)
    {
      if ((object) other == null)
        return false;
      if ((object) this == (object) other)
        return true;
      return !(other.GetType() != this.GetType()) && this.InstanceEquals((BaseEditorData) other) && this.ValueEquals(other);
    }

    public override bool Equals(object obj) => this.Equals(obj as EventBoxGroupEditorData);

    public override int GetHashCode() => (this.groupId, this.beat).GetHashCode();

    public static bool operator ==(EventBoxGroupEditorData lhs, EventBoxGroupEditorData rhs) => (object) lhs != null ? lhs.Equals(rhs) : (object) rhs == null;

    public static bool operator !=(EventBoxGroupEditorData lhs, EventBoxGroupEditorData rhs) => !(lhs == rhs);

    public enum EventBoxGroupType
    {
      Color,
      Rotation,
      Translation,
    }
  }
}
