// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.BasicEventEditorData
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using UnityEngine;

namespace BeatmapEditor3D.DataModels
{
  public sealed class BasicEventEditorData : BaseEditorData, IEquatable<BasicEventEditorData>
  {
    public readonly BasicBeatmapEventType type;
    public readonly int value;
    public readonly float floatValue;
    public readonly bool hasEndTime;
    public readonly float endBeat;
    public readonly int endValue;
    public readonly float endFloatValue;

    public static BasicEventEditorData Copy(BasicEventEditorData basicEventData) => new BasicEventEditorData(basicEventData);

    public static BasicEventEditorData CopyWithoutId(BasicEventEditorData basicEventData) => BasicEventEditorData.CreateNew(basicEventData.type, basicEventData.beat, basicEventData.value, basicEventData.floatValue, basicEventData.endBeat, basicEventData.endValue, basicEventData.endFloatValue);

    public static BasicEventEditorData CreateNew(
      BasicBeatmapEventType type,
      float beat,
      int value,
      float floatValue)
    {
      return BasicEventEditorData.CreateNewWithId(BeatmapEditorObjectId.NewId(), type, beat, value, floatValue);
    }

    public static BasicEventEditorData CreateNew(
      BasicBeatmapEventType type,
      float beat,
      int value,
      float floatValue,
      float endTime,
      int endValue,
      float endFloatValue)
    {
      return BasicEventEditorData.CreateNewWithId(BeatmapEditorObjectId.NewId(), type, beat, value, floatValue, endTime, endValue, endFloatValue);
    }

    public static BasicEventEditorData CreateNewWithId(
      BeatmapEditorObjectId id,
      BasicBeatmapEventType type,
      float beat,
      int value,
      float floatValue)
    {
      return new BasicEventEditorData(id, type, beat, value, floatValue);
    }

    public static BasicEventEditorData CreateNewWithId(
      BeatmapEditorObjectId id,
      BasicBeatmapEventType type,
      float time,
      int value,
      float floatValue,
      float endTime,
      int endValue,
      float endFloatValue)
    {
      return new BasicEventEditorData(id, type, time, value, floatValue, endTime, endValue, endFloatValue);
    }

    private BasicEventEditorData(BasicEventEditorData basicEventData)
      : base(basicEventData.id, basicEventData.beat)
    {
      this.type = basicEventData.type;
      this.value = basicEventData.value;
      this.floatValue = basicEventData.floatValue;
      this.hasEndTime = basicEventData.hasEndTime;
      this.endBeat = basicEventData.endBeat;
      this.endValue = basicEventData.endValue;
      this.endFloatValue = basicEventData.endFloatValue;
    }

    private BasicEventEditorData(
      BeatmapEditorObjectId id,
      BasicBeatmapEventType type,
      float beat,
      int value,
      float floatValue)
      : base(id, beat)
    {
      this.type = type;
      this.value = value;
      this.floatValue = floatValue;
      this.hasEndTime = false;
    }

    private BasicEventEditorData(
      BeatmapEditorObjectId id,
      BasicBeatmapEventType type,
      float beat,
      int value,
      float floatValue,
      float endBeat,
      int endValue,
      float endFloatValue)
      : this(id, type, beat, value, floatValue)
    {
      this.hasEndTime = true;
      this.endBeat = endBeat;
      this.endValue = endValue;
      this.endFloatValue = endFloatValue;
    }

    public bool IsAtEdge(float beat) => AudioTimeHelper.IsBeatSame(this.beat, beat);

    public float GetStartBeat() => this.beat;

    public float GetEndBeat() => !this.hasEndTime ? this.beat : this.endBeat;

    public int GetStartValue() => this.value;

    public bool IsEventValidAtBeat(float beat) => this.IsAtEdge(beat);

    public bool IsAtStart(float beat) => AudioTimeHelper.IsBeatSame(this.beat, beat);

    public bool PositionEquals(BasicEventEditorData other) => AudioTimeHelper.IsBeatSame(this.beat, other.beat) && this.type == other.type;

    public bool ValueEquals(BasicEventEditorData other)
    {
      bool flag1 = AudioTimeHelper.IsBeatSame(this.beat, other.beat) && this.value == other.value && Mathf.Approximately(this.floatValue, other.floatValue);
      bool flag2 = this.hasEndTime == other.hasEndTime;
      if (this.hasEndTime && other.hasEndTime)
        flag2 = AudioTimeHelper.IsBeatSame(this.endBeat, other.endBeat) && this.endValue == other.endValue && Mathf.Approximately(this.endFloatValue, other.endFloatValue);
      return this.type == other.type & flag1 & flag2;
    }

    public bool Equals(BasicEventEditorData other)
    {
      if ((object) other == null)
        return false;
      if ((object) this == (object) other)
        return true;
      return !(other.GetType() != this.GetType()) && this.InstanceEquals((BaseEditorData) other) && this.ValueEquals(other);
    }

    public override bool Equals(object obj) => this.Equals(obj as BasicEventEditorData);

    public override int GetHashCode() => (this.type, this.beat, this.value, this.floatValue).GetHashCode();

    public static bool operator ==(BasicEventEditorData lhs, BasicEventEditorData rhs) => (object) lhs != null ? lhs.Equals(rhs) : (object) rhs == null;

    public static bool operator !=(BasicEventEditorData lhs, BasicEventEditorData rhs) => !(lhs == rhs);
  }
}
