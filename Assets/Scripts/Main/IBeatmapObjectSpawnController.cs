// Decompiled with JetBrains decompiler
// Type: IBeatmapObjectSpawnController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public interface IBeatmapObjectSpawnController
{
  int noteLinesCount { get; }

  float jumpOffsetY { get; }

  float moveDuration { get; }

  float jumpDuration { get; }

  float jumpDistance { get; }

  float verticalLayerDistance { get; }

  float noteJumpMovementSpeed { get; }

  float noteLinesDistance { get; }

  BeatmapObjectSpawnMovementData beatmapObjectSpawnMovementData { get; }

  bool isInitialized { get; }

  event System.Action didInitEvent;

  Vector2 Get2DNoteOffset(int noteLineIndex, NoteLineLayer noteLineLayer);

  float JumpPosYForLineLayerAtDistanceFromPlayerWithoutJumpOffset(
    NoteLineLayer lineLayer,
    float distanceFromPlayer);
}
