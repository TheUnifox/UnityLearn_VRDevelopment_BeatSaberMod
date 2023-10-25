// Decompiled with JetBrains decompiler
// Type: MirroredBeatmapObjectManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using Zenject;

public class MirroredBeatmapObjectManager
{
  protected BeatmapObjectManager _beatmapObjectManager;
  protected MemoryPoolContainer<MirroredGameNoteController> _mirroredBasicGameNotePoolContainer;
  protected MemoryPoolContainer<MirroredGameNoteController> _mirroredBurstSliderHeadGameNotePoolContainer;
  protected MemoryPoolContainer<MirroredGameNoteController> _mirroredBurstSliderGameNotePoolContainer;
  protected MemoryPoolContainer<MirroredGameNoteController> _mirroredBurstSliderFillPoolContainer;
  protected MemoryPoolContainer<MirroredBombNoteController> _mirroredBombNotePoolContainer;
  protected MemoryPoolContainer<MirroredObstacleController> _mirroredObstaclePoolContainer;
  protected MemoryPoolContainer<MirroredSliderController> _mirroredSlidersPoolContainer;
  protected readonly Dictionary<IGameNoteMirrorable, (MirroredGameNoteController, MemoryPoolContainer<MirroredGameNoteController>)> _gameNoteControllersToMirroredGameNoteControllers = new Dictionary<IGameNoteMirrorable, (MirroredGameNoteController, MemoryPoolContainer<MirroredGameNoteController>)>();
  protected readonly Dictionary<INoteMirrorable, MirroredBombNoteController> _bombNoteControllersToMirroredBombNoteControllers = new Dictionary<INoteMirrorable, MirroredBombNoteController>();
  protected readonly Dictionary<ObstacleController, MirroredObstacleController> _obstacleControllersToMirroredObstacleControllers = new Dictionary<ObstacleController, MirroredObstacleController>();
  protected readonly Dictionary<SliderController, MirroredSliderController> _sliderControllersToMirroredSliderControllers = new Dictionary<SliderController, MirroredSliderController>();

  [Inject]
  public virtual void Init(
    BeatmapObjectManager beatmapObjectManager,
    [Inject(Id = NoteData.GameplayType.Normal)] MirroredGameNoteController.Pool mirroredBasicGameNotePool,
    [Inject(Id = NoteData.GameplayType.BurstSliderHead)] MirroredGameNoteController.Pool burstSliderHeadGameNotePool,
    [Inject(Id = NoteData.GameplayType.BurstSliderElement)] MirroredGameNoteController.Pool burstSliderGameNotePool,
    [Inject(Id = NoteData.GameplayType.BurstSliderElementFill)] MirroredGameNoteController.Pool burstSliderFillPool,
    MirroredBombNoteController.Pool mirroredBombNotePool,
    MirroredObstacleController.Pool mirroredObstaclePool,
    MirroredSliderController.Pool mirroredSlidersPool)
  {
    this._beatmapObjectManager = beatmapObjectManager;
    this._mirroredBasicGameNotePoolContainer = new MemoryPoolContainer<MirroredGameNoteController>((IMemoryPool<MirroredGameNoteController>) mirroredBasicGameNotePool);
    this._mirroredBurstSliderHeadGameNotePoolContainer = new MemoryPoolContainer<MirroredGameNoteController>((IMemoryPool<MirroredGameNoteController>) burstSliderHeadGameNotePool);
    this._mirroredBurstSliderGameNotePoolContainer = new MemoryPoolContainer<MirroredGameNoteController>((IMemoryPool<MirroredGameNoteController>) burstSliderGameNotePool);
    this._mirroredBurstSliderFillPoolContainer = new MemoryPoolContainer<MirroredGameNoteController>((IMemoryPool<MirroredGameNoteController>) burstSliderFillPool);
    this._mirroredBombNotePoolContainer = new MemoryPoolContainer<MirroredBombNoteController>((IMemoryPool<MirroredBombNoteController>) mirroredBombNotePool);
    this._mirroredObstaclePoolContainer = new MemoryPoolContainer<MirroredObstacleController>((IMemoryPool<MirroredObstacleController>) mirroredObstaclePool);
    this._mirroredSlidersPoolContainer = new MemoryPoolContainer<MirroredSliderController>((IMemoryPool<MirroredSliderController>) mirroredSlidersPool);
    this._beatmapObjectManager.noteWasSpawnedEvent += new System.Action<NoteController>(this.HandleNoteWasSpawned);
    this._beatmapObjectManager.noteWasDespawnedEvent += new System.Action<NoteController>(this.HandleNoteWasDespawned);
    this._beatmapObjectManager.obstacleWasSpawnedEvent += new System.Action<ObstacleController>(this.HandleObstacleWasSpawned);
    this._beatmapObjectManager.obstacleWasDespawnedEvent += new System.Action<ObstacleController>(this.HandleObstacleWasDespawned);
    this._beatmapObjectManager.sliderWasSpawnedEvent += new System.Action<SliderController>(this.HandleSliderWasSpawned);
    this._beatmapObjectManager.sliderWasDespawnedEvent += new System.Action<SliderController>(this.HandleSliderWasDespawned);
    this._beatmapObjectManager.didHideAllBeatmapObjectsEvent += new System.Action<bool>(this.HandleDidHideAllBeatmapObjects);
  }

  ~MirroredBeatmapObjectManager()
  {
    if (this._beatmapObjectManager == null)
      return;
    this._beatmapObjectManager.noteWasSpawnedEvent -= new System.Action<NoteController>(this.HandleNoteWasSpawned);
    this._beatmapObjectManager.noteWasDespawnedEvent -= new System.Action<NoteController>(this.HandleNoteWasDespawned);
    this._beatmapObjectManager.obstacleWasSpawnedEvent -= new System.Action<ObstacleController>(this.HandleObstacleWasSpawned);
    this._beatmapObjectManager.obstacleWasDespawnedEvent -= new System.Action<ObstacleController>(this.HandleObstacleWasDespawned);
    this._beatmapObjectManager.sliderWasSpawnedEvent -= new System.Action<SliderController>(this.HandleSliderWasSpawned);
    this._beatmapObjectManager.sliderWasDespawnedEvent -= new System.Action<SliderController>(this.HandleSliderWasDespawned);
    this._beatmapObjectManager.didHideAllBeatmapObjectsEvent -= new System.Action<bool>(this.HandleDidHideAllBeatmapObjects);
  }

  public virtual void HandleNoteWasSpawned(NoteController noteController)
  {
    if (noteController is IGameNoteMirrorable gameNoteMirrorable)
    {
      MemoryPoolContainer<MirroredGameNoteController> memoryPoolContainer = (MemoryPoolContainer<MirroredGameNoteController>) null;
      switch (gameNoteMirrorable.gameplayType)
      {
        case NoteData.GameplayType.Normal:
          memoryPoolContainer = this._mirroredBasicGameNotePoolContainer;
          break;
        case NoteData.GameplayType.BurstSliderHead:
          memoryPoolContainer = this._mirroredBurstSliderHeadGameNotePoolContainer;
          break;
        case NoteData.GameplayType.BurstSliderElement:
          memoryPoolContainer = this._mirroredBurstSliderGameNotePoolContainer;
          break;
        case NoteData.GameplayType.BurstSliderElementFill:
          memoryPoolContainer = this._mirroredBurstSliderFillPoolContainer;
          break;
      }
      MirroredGameNoteController gameNoteController = memoryPoolContainer.Spawn();
      this._gameNoteControllersToMirroredGameNoteControllers[gameNoteMirrorable] = (gameNoteController, memoryPoolContainer);
      gameNoteController.Mirror(gameNoteMirrorable);
    }
    else
    {
      INoteMirrorable noteMirrorable;
      if ((NoteController) (noteMirrorable = (INoteMirrorable) noteController) == null)
        return;
      MirroredBombNoteController bombNoteController = this._mirroredBombNotePoolContainer.Spawn();
      this._bombNoteControllersToMirroredBombNoteControllers[noteMirrorable] = bombNoteController;
      bombNoteController.Mirror(noteMirrorable);
    }
  }

  public virtual void HandleNoteWasDespawned(NoteController noteController)
  {
    if (noteController is IGameNoteMirrorable key1)
    {
      (MirroredGameNoteController, MemoryPoolContainer<MirroredGameNoteController>) tuple;
      if (!this._gameNoteControllersToMirroredGameNoteControllers.TryGetValue(key1, out tuple))
        return;
      MirroredGameNoteController gameNoteController = tuple.Item1;
      tuple.Item2.Despawn(gameNoteController);
      this._gameNoteControllersToMirroredGameNoteControllers.Remove(key1);
    }
    else
    {
      INoteMirrorable key;
      MirroredBombNoteController bombNoteController;
      if ((NoteController) (key = (INoteMirrorable) noteController) == null || !this._bombNoteControllersToMirroredBombNoteControllers.TryGetValue(key, out bombNoteController))
        return;
      this._mirroredBombNotePoolContainer.Despawn(bombNoteController);
      this._bombNoteControllersToMirroredBombNoteControllers.Remove(key);
    }
  }

  public virtual void HandleObstacleWasSpawned(ObstacleController obstacleController)
  {
    MirroredObstacleController obstacleController1 = this._mirroredObstaclePoolContainer.Spawn();
    this._obstacleControllersToMirroredObstacleControllers[obstacleController] = obstacleController1;
    obstacleController1.Mirror(obstacleController);
  }

  public virtual void HandleObstacleWasDespawned(ObstacleController obstacleController)
  {
    MirroredObstacleController obstacleController1;
    if (!this._obstacleControllersToMirroredObstacleControllers.TryGetValue(obstacleController, out obstacleController1))
      return;
    this._mirroredObstaclePoolContainer.Despawn(obstacleController1);
    this._obstacleControllersToMirroredObstacleControllers.Remove(obstacleController);
  }

  public virtual void HandleSliderWasSpawned(SliderController sliderController)
  {
    MirroredSliderController sliderController1 = this._mirroredSlidersPoolContainer.Spawn();
    this._sliderControllersToMirroredSliderControllers[sliderController] = sliderController1;
    sliderController1.Mirror(sliderController);
  }

  public virtual void HandleSliderWasDespawned(SliderController sliderController)
  {
    MirroredSliderController sliderController1;
    if (!this._sliderControllersToMirroredSliderControllers.TryGetValue(sliderController, out sliderController1))
      return;
    this._mirroredSlidersPoolContainer.Despawn(sliderController1);
    this._sliderControllersToMirroredSliderControllers.Remove(sliderController);
  }

  public virtual void HandleDidHideAllBeatmapObjects(bool hide)
  {
    foreach (MirroredNoteController<IGameNoteMirrorable> activeItem in this._mirroredBasicGameNotePoolContainer.activeItems)
      activeItem.Hide(hide);
    foreach (MirroredNoteController<IGameNoteMirrorable> activeItem in this._mirroredBasicGameNotePoolContainer.activeItems)
      activeItem.Hide(hide);
    foreach (MirroredNoteController<IGameNoteMirrorable> activeItem in this._mirroredBasicGameNotePoolContainer.activeItems)
      activeItem.Hide(hide);
    foreach (MirroredNoteController<IGameNoteMirrorable> activeItem in this._mirroredBasicGameNotePoolContainer.activeItems)
      activeItem.Hide(hide);
    foreach (MirroredNoteController<INoteMirrorable> activeItem in this._mirroredBombNotePoolContainer.activeItems)
      activeItem.Hide(hide);
    foreach (MirroredObstacleController activeItem in this._mirroredObstaclePoolContainer.activeItems)
      activeItem.hide = hide;
    foreach (MirroredSliderController activeItem in this._mirroredSlidersPoolContainer.activeItems)
      activeItem.hide = hide;
  }
}
