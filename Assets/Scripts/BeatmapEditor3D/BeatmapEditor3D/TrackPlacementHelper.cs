// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.TrackPlacementHelper
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BeatmapEditor3D
{
  public static class TrackPlacementHelper
  {
    public const float kTrackWidth = 0.5f;
    public const float kEventBoxGroupTrackGap = 0.7f;
    public const float kEventBoxGroupTrackWidth = 0.6f;
    public const float kEventBoxGroupTrackPadding = 0.05f;

    public static float TrackCountToScale(int trackCount, float trackWidth = 0.5f) => (float) (trackCount - 1) * trackWidth;

    public static float TrackToPosition(int trackIdx, int trackCount, float trackWidth = 0.5f) => (float) ((double) -(trackCount - 1) * (double) trackWidth * 0.5 + (double) trackIdx * (double) trackWidth);

    public static int PositionToTrackIndex(float position, int trackCount, float trackWidth = 0.5f)
    {
      double num = (double) (trackCount - 1) * (double) trackWidth;
      float min = (float) (-num * 0.5);
      float max = (float) (num * 0.5);
      return Mathf.RoundToInt((Mathf.Clamp(position, min, max) + max) / trackWidth);
    }

    public static float GetPageWidth(
      IReadOnlyCollection<EnvironmentTracksDefinitionSO.SpawnableEventBoxGroupTrack> pageGroupTracks)
    {
      return pageGroupTracks.Sum<EnvironmentTracksDefinitionSO.SpawnableEventBoxGroupTrack>((Func<EnvironmentTracksDefinitionSO.SpawnableEventBoxGroupTrack, float>) (track => (float) track.tracksCount * 0.6f)) + (float) (pageGroupTracks.Count - 1) * 0.7f;
    }

    public static float GetGroupWidth(int tracksCount) => (float) tracksCount * 0.6f;

    public static Vector3 GetGroupPosition(float startPosition, float width) => Vector3.right * (startPosition + width * 0.5f);

    public static float GetTrackPositionInGroup(int indexInGroup) => (float) ((double) indexInGroup * 0.60000002384185791 + 0.30000001192092896);

    public static (int, float) GetPageIndexAndStartPosition(
      float position,
      IReadOnlyList<EnvironmentTracksDefinitionSO.SpawnableEventBoxGroupTrack> pageGroupTracks)
    {
      float num1 = (float) (-(double) TrackPlacementHelper.GetPageWidth((IReadOnlyCollection<EnvironmentTracksDefinitionSO.SpawnableEventBoxGroupTrack>) pageGroupTracks) * 0.5);
      if ((double) position < (double) num1)
        return (-1, 0.0f);
      for (int index = 0; index < pageGroupTracks.Count; ++index)
      {
        float groupWidth = TrackPlacementHelper.GetGroupWidth(pageGroupTracks[index].tracksCount);
        float num2 = (float) ((double) num1 + (double) groupWidth + 0.699999988079071);
        if ((double) position >= (double) num1 && (double) position < (double) num2)
          return (index, num1);
        num1 = num2;
      }
      return (-1, 0.0f);
    }

    public static int GetTrackIndex(float position, float startPosition, float trackCount) => Mathf.FloorToInt(Mathf.Clamp((float) ((double) position - (double) startPosition), 0.0f, trackCount * 0.6f) / 0.6f);
  }
}
