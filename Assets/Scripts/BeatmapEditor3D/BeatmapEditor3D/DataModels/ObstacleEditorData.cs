// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.ObstacleEditorData
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using UnityEngine;

namespace BeatmapEditor3D.DataModels
{
  public sealed class ObstacleEditorData : 
    BaseBeatmapObjectEditorData,
    IEquatable<ObstacleEditorData>
  {
    public readonly float duration;
    public readonly int width;
    public readonly int height;

    public float endBeat => this.beat + this.duration;

    public int endColumn => Mathf.Max(0, this.column + this.width - 1);

    public int endRow => Mathf.Max(0, this.row + this.height - 1);

    public static ObstacleEditorData Copy(ObstacleEditorData obstacleData) => new ObstacleEditorData(obstacleData);

    public static ObstacleEditorData CopyWithoutId(ObstacleEditorData obstacleData) => ObstacleEditorData.CreateNew(obstacleData.beat, obstacleData.column, obstacleData.row, obstacleData.duration, obstacleData.width, obstacleData.height);

    public static ObstacleEditorData CopyWithModifications(
      ObstacleEditorData obstacleData,
      BeatmapEditorObjectId? id = null,
      float? beat = null,
      int? column = null,
      int? row = null,
      float? duration = null,
      int? width = null,
      int? height = null)
    {
      return ObstacleEditorData.CreateNewWithId(id ?? obstacleData.id, (float) ((double) beat ?? (double) obstacleData.beat), column ?? obstacleData.column, row ?? obstacleData.row, (float) ((double) duration ?? (double) obstacleData.duration), width ?? obstacleData.width, height ?? obstacleData.height);
    }

    public static ObstacleEditorData CreateNew(
      float beat,
      int column,
      int row,
      float duration,
      int width,
      int height)
    {
      return ObstacleEditorData.CreateNewWithId(BeatmapEditorObjectId.NewId(), beat, column, row, duration, width, height);
    }

    public static ObstacleEditorData CreateNewWithId(
      BeatmapEditorObjectId obstacleId,
      float beat,
      int column,
      int row,
      float duration,
      int width,
      int height)
    {
      return new ObstacleEditorData(obstacleId, beat, column, row, duration, width, height);
    }

    private ObstacleEditorData(ObstacleEditorData other)
      : base((BaseBeatmapObjectEditorData) other)
    {
      this.duration = other.duration;
      this.width = other.width;
      this.height = other.height;
    }

    private ObstacleEditorData(
      BeatmapEditorObjectId id,
      float beat,
      int column,
      int row,
      float duration,
      int width,
      int height)
      : base(id, beat, column, row)
    {
      this.duration = duration;
      this.width = width;
      this.height = height;
    }

    public bool ValueEquals(ObstacleEditorData other) => this.PositionEquals((BaseBeatmapObjectEditorData) other) && Mathf.Approximately(this.duration, other.duration) && this.width == other.width && this.height == other.height;

    public bool Equals(ObstacleEditorData other)
    {
      if ((object) other == null)
        return false;
      if ((object) this == (object) other)
        return true;
      return !(other.GetType() != this.GetType()) && this.InstanceEquals((BaseEditorData) other) && this.ValueEquals(other);
    }

    public override bool Equals(object obj) => this.Equals(obj as ObstacleEditorData);

    public override int GetHashCode() => (this.beat, this.column, this.row, this.duration, this.width, this.height).GetHashCode();

    public static bool operator ==(ObstacleEditorData lhs, ObstacleEditorData rhs) => (object) lhs != null ? lhs.Equals(rhs) : (object) rhs == null;

    public static bool operator !=(ObstacleEditorData lhs, ObstacleEditorData rhs) => !(lhs == rhs);
  }
}
