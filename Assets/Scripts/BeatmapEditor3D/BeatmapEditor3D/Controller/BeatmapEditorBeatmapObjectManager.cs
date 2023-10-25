// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Controller.BeatmapEditorBeatmapObjectManager
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System.Collections.Generic;

namespace BeatmapEditor3D.Controller
{
  public class BeatmapEditorBeatmapObjectManager : BeatmapObjectManager
  {
    public override List<ObstacleController> activeObstacleControllers => new List<ObstacleController>();

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

    public void TriggerNoteCut(NoteController noteController, NoteCutInfo noteCutInfo) => this.HandleNoteControllerNoteWasCut(noteController, in noteCutInfo);
  }
}
