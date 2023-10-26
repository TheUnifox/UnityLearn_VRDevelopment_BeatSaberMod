// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.LightColorBaseEditorData
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System;
using UnityEngine;

namespace BeatmapEditor3D
{
  public class LightColorBaseEditorData : BaseEditorData, IEquatable<LightColorBaseEditorData>
  {
    public readonly LightColorBaseEditorData.TransitionType transitionType;
    public readonly LightColorBaseEditorData.EnvironmentColorType colorType;
    public readonly float brightness;
    public readonly int strobeBeatFrequency;

    public static LightColorBaseEditorData CreateNew(
      float beat,
      float brightness,
      LightColorBaseEditorData.TransitionType transitionType,
      LightColorBaseEditorData.EnvironmentColorType colorType,
      int strobeFrequency)
    {
      return LightColorBaseEditorData.CreateNewWithId(BeatmapEditorObjectId.NewId(), beat, brightness, transitionType, colorType, strobeFrequency);
    }

    public static LightColorBaseEditorData CreateNewWithId(
      BeatmapEditorObjectId id,
      float beat,
      float brightness,
      LightColorBaseEditorData.TransitionType transitionType,
      LightColorBaseEditorData.EnvironmentColorType colorType,
      int strobeFrequency)
    {
      return new LightColorBaseEditorData(id, beat, brightness, transitionType, colorType, strobeFrequency);
    }

    public static LightColorBaseEditorData CopyWithoutId(LightColorBaseEditorData original) => LightColorBaseEditorData.CreateNew(original.beat, original.brightness, original.transitionType, original.colorType, original.strobeBeatFrequency);

    public static LightColorBaseEditorData CopyWithModifications(
      LightColorBaseEditorData original,
      BeatmapEditorObjectId? id = null,
      float? beat = null,
      float? brightness = null,
      LightColorBaseEditorData.TransitionType? transitionType = null,
      LightColorBaseEditorData.EnvironmentColorType? colorType = null,
      int? strobeFrequency = null)
    {
      return LightColorBaseEditorData.CreateNewWithId(id ?? original.id, beat ?? original.beat, brightness ?? original.brightness, transitionType ?? original.transitionType, colorType ?? original.colorType, strobeFrequency ?? original.strobeBeatFrequency);
    }

    public static LightColorBaseEditorData CreateExtension(float beat) => LightColorBaseEditorData.CreateNew(beat, 0.0f, LightColorBaseEditorData.TransitionType.Extension, LightColorBaseEditorData.EnvironmentColorType.None, 0);

    private LightColorBaseEditorData(
      BeatmapEditorObjectId id,
      float beat,
      float brightness,
      LightColorBaseEditorData.TransitionType transitionType,
      LightColorBaseEditorData.EnvironmentColorType colorType,
      int strobeBeatFrequency)
      : base(id, beat)
    {
      this.brightness = brightness;
      this.transitionType = transitionType;
      this.colorType = colorType;
      this.strobeBeatFrequency = strobeBeatFrequency;
    }

    public bool PositionEquals(LightColorBaseEditorData other) => AudioTimeHelper.IsBeatSame(this.beat, other.beat);

    public bool ValueEquals(LightColorBaseEditorData other) => this.PositionEquals(other) && this.colorType == other.colorType && this.transitionType == other.transitionType && Mathf.Approximately(this.brightness, other.brightness) && this.strobeBeatFrequency == other.strobeBeatFrequency;

    public bool Equals(LightColorBaseEditorData other)
    {
      if ((object) other == null)
        return false;
      if ((object) this == (object) other)
        return true;
      return !(this.GetType() != other.GetType()) && this.InstanceEquals((BaseEditorData) other) && this.ValueEquals(other);
    }

    public override bool Equals(object obj) => this.Equals(obj as LightColorBaseEditorData);

    public override int GetHashCode() => (this.beat, this.colorType, this.transitionType, this.brightness, this.strobeBeatFrequency).GetHashCode();

    public static bool operator ==(LightColorBaseEditorData lhs, LightColorBaseEditorData rhs) => (object) lhs != null ? lhs.Equals(rhs) : (object) rhs == null;

    public static bool operator !=(LightColorBaseEditorData lhs, LightColorBaseEditorData rhs) => !(lhs == rhs);

    public enum EnvironmentColorType
    {
      None = -1, // 0xFFFFFFFF
      Color0 = 0,
      Color1 = 1,
      ColorW = 2,
    }

    public enum TransitionType
    {
      Instant,
      Interpolate,
      Extension,
    }
  }
}
