// Decompiled with JetBrains decompiler
// Type: BasicBeatmapObjectManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using Zenject;

public class BasicBeatmapObjectManager : BeatmapObjectManager
{
  protected float? _firstBasicNoteTime;
  protected BasicBeatmapObjectManager.InitData _initData;
  protected MemoryPoolContainer<GameNoteController> _basicGameNotePoolContainer;
  protected MemoryPoolContainer<GameNoteController> _burstSliderHeadGameNotePoolContainer;
  protected MemoryPoolContainer<BurstSliderGameNoteController> _burstSliderGameNotePoolContainer;
  protected MemoryPoolContainer<BurstSliderGameNoteController> _burstSliderFillPoolContainer;
  protected MemoryPoolContainer<BombNoteController> _bombNotePoolContainer;
  protected MemoryPoolContainer<ObstacleController> _obstaclePoolContainer;
  protected Dictionary<SliderController.LengthType, MemoryPoolContainer<SliderController>> _sliderNotePoolContainersDictionary;

  public override List<ObstacleController> activeObstacleControllers => this._obstaclePoolContainer.activeItems;

  [Inject]
  public virtual void Init(
    BasicBeatmapObjectManager.InitData initData,
    [Inject(Id = NoteData.GameplayType.Normal)] GameNoteController.Pool basicGameNotePool,
    [Inject(Id = NoteData.GameplayType.BurstSliderHead)] GameNoteController.Pool burstSliderHeadGameNotePool,
    [Inject(Id = NoteData.GameplayType.BurstSliderElement)] BurstSliderGameNoteController.Pool burstSliderGameNotePool,
    [Inject(Id = NoteData.GameplayType.BurstSliderElementFill)] BurstSliderGameNoteController.Pool burstSliderFillPool,
    BombNoteController.Pool bombNotePool,
    ObstacleController.Pool obstaclePool,
    SliderController.Pool sliderPools)
  {
    this._initData = initData;
    this._basicGameNotePoolContainer = new MemoryPoolContainer<GameNoteController>((IMemoryPool<GameNoteController>) basicGameNotePool);
    this._burstSliderHeadGameNotePoolContainer = new MemoryPoolContainer<GameNoteController>((IMemoryPool<GameNoteController>) burstSliderHeadGameNotePool);
    this._burstSliderGameNotePoolContainer = new MemoryPoolContainer<BurstSliderGameNoteController>((IMemoryPool<BurstSliderGameNoteController>) burstSliderGameNotePool);
    this._burstSliderFillPoolContainer = new MemoryPoolContainer<BurstSliderGameNoteController>((IMemoryPool<BurstSliderGameNoteController>) burstSliderFillPool);
    this._bombNotePoolContainer = new MemoryPoolContainer<BombNoteController>((IMemoryPool<BombNoteController>) bombNotePool);
    this._obstaclePoolContainer = new MemoryPoolContainer<ObstacleController>((IMemoryPool<ObstacleController>) obstaclePool);
    this._sliderNotePoolContainersDictionary = new Dictionary<SliderController.LengthType, MemoryPoolContainer<SliderController>>();
    foreach (SliderController.LengthType lengthType in Enum.GetValues(typeof (SliderController.LengthType)))
      this._sliderNotePoolContainersDictionary[lengthType] = new MemoryPoolContainer<SliderController>((IMemoryPool<SliderController>) sliderPools.GetPool(lengthType));
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

  protected override void DespawnInternal(ObstacleController obstacleController) => this._obstaclePoolContainer.Despawn(obstacleController);

  public override void ProcessNoteData(
    NoteData noteData,
    in BeatmapObjectSpawnMovementData.NoteSpawnData noteSpawnData,
    float rotation,
    bool forceIsFirstNoteBehaviour)
  {
    if (!this._firstBasicNoteTime.HasValue)
      this._firstBasicNoteTime = new float?(noteData.time);
    int num;
    if (!forceIsFirstNoteBehaviour)
    {
      float? firstBasicNoteTime = this._firstBasicNoteTime;
      float time = noteData.time;
      num = (double) firstBasicNoteTime.GetValueOrDefault() == (double) time & firstBasicNoteTime.HasValue ? 1 : 0;
    }
    else
      num = 1;
    bool flag = num != 0;
    NoteVisualModifierType noteVisualModifierType = NoteVisualModifierType.Normal;
    if (this._initData.ghostNotes && !flag)
      noteVisualModifierType = NoteVisualModifierType.Ghost;
    else if (this._initData.disappearingArrows)
      noteVisualModifierType = NoteVisualModifierType.DisappearingArrow;
    switch (noteData.gameplayType)
    {
      case NoteData.GameplayType.Normal:
        GameNoteController gameNoteController1 = this._basicGameNotePoolContainer.Spawn();
        gameNoteController1.Init(noteData, rotation, noteSpawnData.moveStartPos, noteSpawnData.moveEndPos, noteSpawnData.jumpEndPos, noteSpawnData.moveDuration, noteSpawnData.jumpDuration, noteSpawnData.jumpGravity, noteVisualModifierType, this._initData.cutAngleTolerance, this._initData.notesUniformScale);
        this.AddSpawnedNoteController((NoteController) gameNoteController1, noteSpawnData, rotation);
        break;
      case NoteData.GameplayType.Bomb:
        BombNoteController bombNoteController = this._bombNotePoolContainer.Spawn();
        bombNoteController.Init(noteData, rotation, noteSpawnData.moveStartPos, noteSpawnData.moveEndPos, noteSpawnData.jumpEndPos, noteSpawnData.moveDuration, noteSpawnData.jumpDuration, noteSpawnData.jumpGravity);
        this.AddSpawnedNoteController((NoteController) bombNoteController, noteSpawnData, rotation);
        break;
      case NoteData.GameplayType.BurstSliderHead:
        GameNoteController gameNoteController2 = this._burstSliderHeadGameNotePoolContainer.Spawn();
        gameNoteController2.Init(noteData, rotation, noteSpawnData.moveStartPos, noteSpawnData.moveEndPos, noteSpawnData.jumpEndPos, noteSpawnData.moveDuration, noteSpawnData.jumpDuration, noteSpawnData.jumpGravity, noteVisualModifierType, this._initData.cutAngleTolerance, this._initData.notesUniformScale);
        this.AddSpawnedNoteController((NoteController) gameNoteController2, noteSpawnData, rotation);
        break;
      case NoteData.GameplayType.BurstSliderElement:
        BurstSliderGameNoteController gameNoteController3 = this._burstSliderGameNotePoolContainer.Spawn();
        gameNoteController3.Init(noteData, rotation, noteSpawnData.moveStartPos, noteSpawnData.moveEndPos, noteSpawnData.jumpEndPos, noteSpawnData.moveDuration, noteSpawnData.jumpDuration, noteSpawnData.jumpGravity, noteVisualModifierType, this._initData.notesUniformScale);
        this.AddSpawnedNoteController((NoteController) gameNoteController3, noteSpawnData, rotation);
        break;
      case NoteData.GameplayType.BurstSliderElementFill:
        BurstSliderGameNoteController gameNoteController4 = this._burstSliderFillPoolContainer.Spawn();
        gameNoteController4.Init(noteData, rotation, noteSpawnData.moveStartPos, noteSpawnData.moveEndPos, noteSpawnData.jumpEndPos, noteSpawnData.moveDuration, noteSpawnData.jumpDuration, noteSpawnData.jumpGravity, noteVisualModifierType, this._initData.notesUniformScale);
        this.AddSpawnedNoteController((NoteController) gameNoteController4, noteSpawnData, rotation);
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  protected override void DespawnInternal(NoteController noteController)
  {
    switch (noteController.noteData.gameplayType)
    {
      case NoteData.GameplayType.Normal:
        this._basicGameNotePoolContainer.Despawn((GameNoteController) noteController);
        break;
      case NoteData.GameplayType.Bomb:
        this._bombNotePoolContainer.Despawn((BombNoteController) noteController);
        break;
      case NoteData.GameplayType.BurstSliderHead:
        this._burstSliderHeadGameNotePoolContainer.Despawn((GameNoteController) noteController);
        break;
      case NoteData.GameplayType.BurstSliderElement:
        this._burstSliderGameNotePoolContainer.Despawn((BurstSliderGameNoteController) noteController);
        break;
      case NoteData.GameplayType.BurstSliderElementFill:
        this._burstSliderFillPoolContainer.Despawn((BurstSliderGameNoteController) noteController);
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  public override void ProcessSliderData(
    SliderData sliderData,
    in BeatmapObjectSpawnMovementData.SliderSpawnData sliderSpawnData,
    float rotation)
  {
    if (sliderData.sliderType == SliderData.Type.Burst)
    {
      if (!this._firstBasicNoteTime.HasValue)
        this._firstBasicNoteTime = new float?(sliderData.time);
      float? firstBasicNoteTime = this._firstBasicNoteTime;
      float time = sliderData.time;
      bool forceIsFirstNote = (double) firstBasicNoteTime.GetValueOrDefault() == (double) time & firstBasicNoteTime.HasValue;
      BurstSliderSpawner.ProcessSliderData(sliderData, in sliderSpawnData, rotation, forceIsFirstNote, new BurstSliderSpawner.ProcessNoteDataDelegate(((BeatmapObjectManager) this).ProcessNoteData));
    }
    else
    {
      SliderController.LengthType lengthFromSliderData = SliderController.Pool.GetLengthFromSliderData(sliderData, sliderSpawnData);
      SliderController sliderController = this._sliderNotePoolContainersDictionary[lengthFromSliderData].Spawn();
      sliderController.Init(lengthFromSliderData, sliderData, rotation, sliderSpawnData.headJumpStartPos, sliderSpawnData.tailJumpStartPos, sliderSpawnData.headJumpEndPos, sliderSpawnData.tailJumpEndPos, sliderSpawnData.jumpDuration, sliderSpawnData.headJumpGravity, sliderSpawnData.tailJumpGravity, this._initData.notesUniformScale);
      this.AddSpawnedSliderController(sliderController, sliderSpawnData, rotation);
    }
  }

  protected override void DespawnInternal(SliderController sliderNoteController)
  {
    MemoryPoolContainer<SliderController> memoryPoolContainer;
    if (!this._sliderNotePoolContainersDictionary.TryGetValue(sliderNoteController.lengthType, out memoryPoolContainer))
      return;
    memoryPoolContainer.Despawn(sliderNoteController);
  }

  public class InitData
  {
    public readonly bool disappearingArrows;
    public readonly bool ghostNotes;
    public readonly float cutAngleTolerance;
    public readonly float notesUniformScale;

    public InitData(
      bool disappearingArrows,
      bool ghostNotes,
      float cutAngleTolerance,
      float notesUniformScale)
    {
      this.disappearingArrows = disappearingArrows;
      this.ghostNotes = ghostNotes;
      this.cutAngleTolerance = cutAngleTolerance;
      this.notesUniformScale = notesUniformScale;
    }
  }
}
