// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.ChainUtils
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using UnityEngine;

namespace BeatmapEditor3D
{
  public static class ChainUtils
  {
    public static NoteCutDirection GetNewNoteCutDirection(
      int column,
      int row,
      int endColumn,
      int endRow,
      NoteCutDirection cutDirection,
      float squishAmount)
    {
      Vector2 p2 = new Vector2((float) (endColumn - column), (float) (endRow - row));
      float magnitude = p2.magnitude;
      float f = (float) (((double) cutDirection.RotationAngle() - 90.0) * (Math.PI / 180.0));
      Vector2 tangent;
      BurstSliderSpawner.BezierCurve(Vector2.zero, 0.5f * magnitude * new Vector2(Mathf.Cos(f), Mathf.Sign(f)), p2, squishAmount, out Vector2 _, out tangent);
      return NoteCutDirectionExtensions.NoteCutDirectionFromDirection((Vector3) tangent);
    }
  }
}
