// Decompiled with JetBrains decompiler
// Type: IBeatmapObjectSpawner
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public interface IBeatmapObjectSpawner
{
  void ProcessObstacleData(
    ObstacleData obstacleData,
    in BeatmapObjectSpawnMovementData.ObstacleSpawnData obstacleSpawnData,
    float rotation);

  void ProcessNoteData(
    NoteData noteData,
    in BeatmapObjectSpawnMovementData.NoteSpawnData noteSpawnData,
    float rotation,
    bool forceIsFirstNoteBehaviour);

  void ProcessSliderData(
    SliderData sliderData,
    in BeatmapObjectSpawnMovementData.SliderSpawnData sliderSpawnData,
    float rotation);
}
