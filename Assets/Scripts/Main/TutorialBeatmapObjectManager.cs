// Decompiled with JetBrains decompiler
// Type: TutorialBeatmapObjectManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using Zenject;

public class TutorialBeatmapObjectManager : BeatmapObjectManager
{
  protected MemoryPoolContainer<TutorialNoteController> _tutorialNotePoolContainer;
  protected MemoryPoolContainer<BombNoteController> _bombNotePoolContainer;
  protected MemoryPoolContainer<ObstacleController> _obstaclePoolContainer;
  protected TutorialBeatmapObjectManager.InitData _initData;

  public override List<ObstacleController> activeObstacleControllers => this._obstaclePoolContainer.activeItems;

  [Inject]
  public virtual void Init(
    TutorialBeatmapObjectManager.InitData initData,
    TutorialNoteController.Pool tutorialNotePool,
    BombNoteController.Pool bombNotePool,
    ObstacleController.Pool obstaclePool)
  {
    this._initData = initData;
    this._tutorialNotePoolContainer = new MemoryPoolContainer<TutorialNoteController>((IMemoryPool<TutorialNoteController>) tutorialNotePool);
    this._bombNotePoolContainer = new MemoryPoolContainer<BombNoteController>((IMemoryPool<BombNoteController>) bombNotePool);
    this._obstaclePoolContainer = new MemoryPoolContainer<ObstacleController>((IMemoryPool<ObstacleController>) obstaclePool);
  }

  public override void ProcessObstacleData(
    ObstacleData obstacleData,
    in BeatmapObjectSpawnMovementData.ObstacleSpawnData obstacleSpawnData,
    float rotation)
  {
    ObstacleController obstacleController = this._obstaclePoolContainer.Spawn();
    obstacleController.Init(obstacleData, rotation, obstacleSpawnData.moveStartPos, obstacleSpawnData.moveEndPos, obstacleSpawnData.jumpEndPos, obstacleSpawnData.moveDuration, obstacleSpawnData.jumpDuration, obstacleSpawnData.noteLinesDistance, obstacleSpawnData.obstacleHeight);
    this.AddSpawnedObstacleController(obstacleController, obstacleSpawnData, rotation);
  }

  public override void ProcessNoteData(
    NoteData noteData,
    in BeatmapObjectSpawnMovementData.NoteSpawnData noteSpawnData,
    float rotation,
    bool forceIsFirstNoteBehaviour)
  {
    switch (noteData.gameplayType)
    {
      case NoteData.GameplayType.Normal:
        TutorialNoteController tutorialNoteController = this._tutorialNotePoolContainer.Spawn();
        tutorialNoteController.Init(noteData, rotation, noteSpawnData.moveStartPos, noteSpawnData.moveEndPos, noteSpawnData.jumpEndPos, noteSpawnData.moveDuration, noteSpawnData.jumpDuration, noteSpawnData.jumpGravity, this._initData.cutAngleTolerance, 1f);
        this.AddSpawnedNoteController((NoteController) tutorialNoteController, noteSpawnData, rotation);
        break;
      case NoteData.GameplayType.Bomb:
        BombNoteController bombNoteController = this._bombNotePoolContainer.Spawn();
        bombNoteController.Init(noteData, rotation, noteSpawnData.moveStartPos, noteSpawnData.moveEndPos, noteSpawnData.jumpEndPos, noteSpawnData.moveDuration, noteSpawnData.jumpDuration, noteSpawnData.jumpGravity);
        this.AddSpawnedNoteController((NoteController) bombNoteController, noteSpawnData, rotation);
        break;
    }
  }

  public override void ProcessSliderData(
    SliderData sliderData,
    in BeatmapObjectSpawnMovementData.SliderSpawnData sliderSpawnData,
    float rotation)
  {
  }

  protected override void DespawnInternal(NoteController noteController)
  {
    switch (noteController)
    {
      case TutorialNoteController tutorialNoteController:
        this._tutorialNotePoolContainer.Despawn(tutorialNoteController);
        break;
      case BombNoteController bombNoteController:
        this._bombNotePoolContainer.Despawn(bombNoteController);
        break;
    }
  }

  protected override void DespawnInternal(ObstacleController obstacleController) => this._obstaclePoolContainer.Despawn(obstacleController);

  protected override void DespawnInternal(SliderController sliderNoteController)
  {
  }

  public class InitData
  {
    public readonly float cutAngleTolerance;

    public InitData(float cutAngleTolerance) => this.cutAngleTolerance = cutAngleTolerance;
  }
}
