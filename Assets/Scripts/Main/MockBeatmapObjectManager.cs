// Decompiled with JetBrains decompiler
// Type: MockBeatmapObjectManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;

public class MockBeatmapObjectManager : BeatmapObjectManager
{
  public override List<ObstacleController> activeObstacleControllers => (List<ObstacleController>) null;

  public override void ProcessObstacleData(
    ObstacleData obstacleData,
    in BeatmapObjectSpawnMovementData.ObstacleSpawnData obstacleSpawnData,
    float rotation)
  {
  }

  public override void ProcessNoteData(
    NoteData noteData,
    in BeatmapObjectSpawnMovementData.NoteSpawnData noteSpawnData,
    float rotation,
    bool forceIsFirstNoteBehaviour)
  {
  }

  public override void ProcessSliderData(
    SliderData sliderData,
    in BeatmapObjectSpawnMovementData.SliderSpawnData sliderSpawnData,
    float rotation)
  {
  }

  protected override void DespawnInternal(NoteController noteController)
  {
  }

  protected override void DespawnInternal(ObstacleController obstacleController)
  {
  }

  protected override void DespawnInternal(SliderController sliderNoteController)
  {
  }
}
