// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.LightTranslationBaseEditorData
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using UnityEngine;

namespace BeatmapEditor3D.DataModels
{
  public class LightTranslationBaseEditorData : 
    BaseEditorData,
    IEquatable<LightTranslationBaseEditorData>
  {
    public readonly EaseType easeType;
    public readonly float translation;
    public readonly bool usePreviousEventTranslationValue;

    public static LightTranslationBaseEditorData CreateNew(
      float beat,
      EaseType easeType,
      float translation,
      bool usePreviousEventTranslationValue)
    {
      return LightTranslationBaseEditorData.CreateNewWithId(BeatmapEditorObjectId.NewId(), beat, easeType, translation, usePreviousEventTranslationValue);
    }

    public static LightTranslationBaseEditorData CreateNewWithId(
      BeatmapEditorObjectId id,
      float beat,
      EaseType easeType,
      float translation,
      bool usePreviousEventTranslationValue)
    {
      return new LightTranslationBaseEditorData(id, beat, easeType, translation, usePreviousEventTranslationValue);
    }

    public static LightTranslationBaseEditorData CopyWithoutId(
      LightTranslationBaseEditorData original)
    {
      return LightTranslationBaseEditorData.CreateNew(original.beat, original.easeType, original.translation, original.usePreviousEventTranslationValue);
    }

    public static LightTranslationBaseEditorData CopyWithModifications(
      LightTranslationBaseEditorData original,
      BeatmapEditorObjectId? id = null,
      float? beat = null,
      EaseType? easeType = null,
      float? translation = null,
      bool? usePreviousEventTranslationValue = null)
    {
      return LightTranslationBaseEditorData.CreateNewWithId(id ?? original.id, beat ?? original.beat, easeType ?? original.easeType, translation ?? original.translation, usePreviousEventTranslationValue ?? original.usePreviousEventTranslationValue);
    }

    public static LightTranslationBaseEditorData CreateExtension(float beat) => LightTranslationBaseEditorData.CreateNew(beat, EaseType.None, 0.0f, true);

    private LightTranslationBaseEditorData(
      BeatmapEditorObjectId id,
      float beat,
      EaseType easeType,
      float translation,
      bool usePreviousEventTranslationValue)
      : base(id, beat)
    {
      this.easeType = easeType;
      this.translation = translation;
      this.usePreviousEventTranslationValue = usePreviousEventTranslationValue;
    }

    public bool PositionEquals(LightTranslationBaseEditorData other) => AudioTimeHelper.IsBeatSame(this.beat, other.beat);

    public bool ValueEquals(LightTranslationBaseEditorData other) => this.PositionEquals(other) && this.easeType == other.easeType && Mathf.Approximately(this.translation, other.translation) && this.usePreviousEventTranslationValue == other.usePreviousEventTranslationValue;

    public bool Equals(LightTranslationBaseEditorData other)
    {
      if ((object) other == null)
        return false;
      if ((object) this == (object) other)
        return true;
      return !(this.GetType() != other.GetType()) && this.InstanceEquals((BaseEditorData) other) && this.ValueEquals(other);
    }

    public override bool Equals(object obj) => this.Equals(obj as LightTranslationBaseEditorData);

    public override int GetHashCode() => (this.beat, this.easeType, this.translation, this.usePreviousEventTranslationValue).GetHashCode();

    public static bool operator ==(
      LightTranslationBaseEditorData lhs,
      LightTranslationBaseEditorData rhs)
    {
      return (object) lhs != null ? lhs.Equals(rhs) : (object) rhs == null;
    }

    public static bool operator !=(
      LightTranslationBaseEditorData lhs,
      LightTranslationBaseEditorData rhs)
    {
      return !(lhs == rhs);
    }
  }
}
