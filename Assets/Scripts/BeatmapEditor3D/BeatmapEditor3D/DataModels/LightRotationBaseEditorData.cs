// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.LightRotationBaseEditorData
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using UnityEngine;

namespace BeatmapEditor3D.DataModels
{
  public class LightRotationBaseEditorData : BaseEditorData, IEquatable<LightRotationBaseEditorData>
  {
    public readonly EaseType easeType;
    public readonly int loopsCount;
    public readonly float rotation;
    public readonly bool usePreviousEventRotationValue;
    public readonly LightRotationDirection rotationDirection;

    public static LightRotationBaseEditorData CreateNew(
      float beat,
      EaseType easeType,
      int loopsCount,
      float rotation,
      bool usePreviousEventRotationValue,
      LightRotationDirection rotationDirection)
    {
      return LightRotationBaseEditorData.CreateNewWithId(BeatmapEditorObjectId.NewId(), beat, easeType, loopsCount, rotation, usePreviousEventRotationValue, rotationDirection);
    }

    public static LightRotationBaseEditorData CreateNewWithId(
      BeatmapEditorObjectId id,
      float beat,
      EaseType easeType,
      int loopsCount,
      float rotation,
      bool usePreviousEventRotationValue,
      LightRotationDirection rotationDirection)
    {
      return new LightRotationBaseEditorData(id, beat, easeType, loopsCount, rotation, usePreviousEventRotationValue, rotationDirection);
    }

    public static LightRotationBaseEditorData CopyWithoutId(LightRotationBaseEditorData original) => LightRotationBaseEditorData.CreateNew(original.beat, original.easeType, original.loopsCount, original.rotation, original.usePreviousEventRotationValue, original.rotationDirection);

    public static LightRotationBaseEditorData CopyWithModifications(
      LightRotationBaseEditorData original,
      BeatmapEditorObjectId? id = null,
      float? beat = null,
      EaseType? easeType = null,
      int? loopsCount = null,
      float? rotation = null,
      bool? usePreviousEventRotationValue = null,
      LightRotationDirection? rotationDirection = null)
    {
      return LightRotationBaseEditorData.CreateNewWithId(id ?? original.id, (float) ((double) beat ?? (double) original.beat), (EaseType) ((int) easeType ?? (int) original.easeType), loopsCount ?? original.loopsCount, (float) ((double) rotation ?? (double) original.rotation), ((int) usePreviousEventRotationValue ?? (original.usePreviousEventRotationValue ? 1 : 0)) != 0, (LightRotationDirection) ((int) rotationDirection ?? (int) original.rotationDirection));
    }

    public static LightRotationBaseEditorData CreateExtension(float beat) => LightRotationBaseEditorData.CreateNew(beat, EaseType.None, 0, 0.0f, true, LightRotationDirection.Automatic);

    private LightRotationBaseEditorData(
      BeatmapEditorObjectId id,
      float beat,
      EaseType easeType,
      int loopsCount,
      float rotation,
      bool usePreviousEventRotationValue,
      LightRotationDirection rotationDirection)
      : base(id, beat)
    {
      this.easeType = easeType;
      this.loopsCount = loopsCount;
      this.rotation = rotation;
      this.usePreviousEventRotationValue = usePreviousEventRotationValue;
      this.rotationDirection = rotationDirection;
    }

    public bool PositionEquals(LightRotationBaseEditorData other) => AudioTimeHelper.IsBeatSame(this.beat, other.beat);

    public bool ValueEquals(LightRotationBaseEditorData other) => this.PositionEquals(other) && this.easeType == other.easeType && this.loopsCount == other.loopsCount && Mathf.Approximately(this.rotation, other.rotation) && this.rotationDirection == other.rotationDirection && this.usePreviousEventRotationValue == other.usePreviousEventRotationValue;

    public bool Equals(LightRotationBaseEditorData other)
    {
      if ((object) other == null)
        return false;
      if ((object) this == (object) other)
        return true;
      return !(this.GetType() != other.GetType()) && this.InstanceEquals((BaseEditorData) other) && this.ValueEquals(other);
    }

    public override bool Equals(object obj) => this.Equals(obj as LightRotationBaseEditorData);

    public override int GetHashCode() => (this.beat, this.easeType, this.loopsCount, this.rotation, this.rotationDirection, this.usePreviousEventRotationValue).GetHashCode();

    public static bool operator ==(LightRotationBaseEditorData lhs, LightRotationBaseEditorData rhs) => (object) lhs != null ? lhs.Equals(rhs) : (object) rhs == null;

    public static bool operator !=(LightRotationBaseEditorData lhs, LightRotationBaseEditorData rhs) => !(lhs == rhs);
  }
}
