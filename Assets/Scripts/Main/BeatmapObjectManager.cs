// Decompiled with JetBrains decompiler
// Type: BeatmapObjectManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;

public abstract class BeatmapObjectManager : 
  IBeatmapObjectSpawner,
  INoteControllerNoteDidStartJumpEvent,
  INoteControllerNoteDidFinishJumpEvent,
  INoteControllerNoteWasCutEvent,
  INoteControllerNoteWasMissedEvent,
  INoteControllerNoteDidDissolveEvent,
  ISliderDidFinishJumpEvent,
  ISliderDidDissolveEvent
{
  private readonly List<IBeatmapObjectController> _allBeatmapObjects = new List<IBeatmapObjectController>();

  public event System.Action<NoteData, BeatmapObjectSpawnMovementData.NoteSpawnData, float> noteWasAddedEvent;

  public event System.Action<NoteController> noteWasSpawnedEvent;

  public event System.Action<NoteController> noteWasDespawnedEvent;

  public event System.Action<NoteController> noteWasMissedEvent;

  public event BeatmapObjectManager.NoteWasCutDelegate noteWasCutEvent;

  public event System.Action<NoteController> noteDidStartJumpEvent;

  public event System.Action<ObstacleData, BeatmapObjectSpawnMovementData.ObstacleSpawnData, float> obstacleWasAddedEvent;

  public event System.Action<ObstacleController> obstacleWasSpawnedEvent;

  public event System.Action<ObstacleController> obstacleWasDespawnedEvent;

  public event System.Action<ObstacleController> obstacleDidPassThreeQuartersOfMove2Event;

  public event System.Action<ObstacleController> obstacleDidPassAvoidedMarkEvent;

  public event System.Action<SliderData, BeatmapObjectSpawnMovementData.SliderSpawnData, float> sliderWasAddedEvent;

  public event System.Action<SliderController> sliderWasSpawnedEvent;

  public event System.Action<SliderController> sliderWasDespawnedEvent;

  public event System.Action<bool> didHideAllBeatmapObjectsEvent;

  public abstract List<ObstacleController> activeObstacleControllers { get; }

  public abstract void ProcessObstacleData(
    ObstacleData obstacleData,
    in BeatmapObjectSpawnMovementData.ObstacleSpawnData obstacleSpawnData,
    float rotation);

  public abstract void ProcessNoteData(
    NoteData noteData,
    in BeatmapObjectSpawnMovementData.NoteSpawnData noteSpawnData,
    float rotation,
    bool forceIsFirstNoteBehaviour);

  public abstract void ProcessSliderData(
    SliderData sliderData,
    in BeatmapObjectSpawnMovementData.SliderSpawnData sliderSpawnData,
    float rotation);

  protected abstract void DespawnInternal(NoteController noteController);

  protected abstract void DespawnInternal(ObstacleController obstacleController);

  protected abstract void DespawnInternal(SliderController sliderNoteController);

  public bool spawnHidden { get; set; }

  protected void AddSpawnedObstacleController(
    ObstacleController obstacleController,
    BeatmapObjectSpawnMovementData.ObstacleSpawnData obstacleSpawnData,
    float rotation)
  {
    if ((UnityEngine.Object) obstacleController == (UnityEngine.Object) null)
      return;
    this.SetObstacleEventCallbacks(obstacleController);
    System.Action<ObstacleController> obstacleWasSpawnedEvent = this.obstacleWasSpawnedEvent;
    if (obstacleWasSpawnedEvent != null)
      obstacleWasSpawnedEvent(obstacleController);
    System.Action<ObstacleData, BeatmapObjectSpawnMovementData.ObstacleSpawnData, float> obstacleWasAddedEvent = this.obstacleWasAddedEvent;
    if (obstacleWasAddedEvent != null)
      obstacleWasAddedEvent(obstacleController.obstacleData, obstacleSpawnData, rotation);
    obstacleController.ManualUpdate();
    obstacleController.Hide(this.spawnHidden);
    this._allBeatmapObjects.Add((IBeatmapObjectController) obstacleController);
  }

  protected void AddSpawnedNoteController(
    NoteController noteController,
    BeatmapObjectSpawnMovementData.NoteSpawnData noteSpawnData,
    float rotation)
  {
    if ((UnityEngine.Object) noteController == (UnityEngine.Object) null)
      return;
    this.SetNoteControllerEventCallbacks(noteController);
    System.Action<NoteController> noteWasSpawnedEvent = this.noteWasSpawnedEvent;
    if (noteWasSpawnedEvent != null)
      noteWasSpawnedEvent(noteController);
    System.Action<NoteData, BeatmapObjectSpawnMovementData.NoteSpawnData, float> noteWasAddedEvent = this.noteWasAddedEvent;
    if (noteWasAddedEvent != null)
      noteWasAddedEvent(noteController.noteData, noteSpawnData, rotation);
    noteController.ManualUpdate();
    noteController.Hide(this.spawnHidden);
    this._allBeatmapObjects.Add((IBeatmapObjectController) noteController);
  }

  protected void AddSpawnedSliderController(
    SliderController sliderController,
    BeatmapObjectSpawnMovementData.SliderSpawnData sliderSpawnData,
    float rotation)
  {
    if ((UnityEngine.Object) sliderController == (UnityEngine.Object) null)
      return;
    this.SetSliderNoteControllerEventCallbacks(sliderController);
    System.Action<SliderController> sliderWasSpawnedEvent = this.sliderWasSpawnedEvent;
    if (sliderWasSpawnedEvent != null)
      sliderWasSpawnedEvent(sliderController);
    System.Action<SliderData, BeatmapObjectSpawnMovementData.SliderSpawnData, float> sliderWasAddedEvent = this.sliderWasAddedEvent;
    if (sliderWasAddedEvent != null)
      sliderWasAddedEvent(sliderController.sliderData, sliderSpawnData, rotation);
    sliderController.ManualUpdate();
    sliderController.Hide(this.spawnHidden);
    this._allBeatmapObjects.Add((IBeatmapObjectController) sliderController);
  }

  private void SetNoteControllerEventCallbacks(NoteController noteController)
  {
    noteController.noteDidStartJumpEvent.Add((INoteControllerNoteDidStartJumpEvent) this);
    noteController.noteDidFinishJumpEvent.Add((INoteControllerNoteDidFinishJumpEvent) this);
    noteController.noteWasCutEvent.Add((INoteControllerNoteWasCutEvent) this);
    noteController.noteWasMissedEvent.Add((INoteControllerNoteWasMissedEvent) this);
    noteController.noteDidDissolveEvent.Add((INoteControllerNoteDidDissolveEvent) this);
  }

  private void RemoveNoteControllerEventCallbacks(NoteController noteController)
  {
    noteController.noteDidStartJumpEvent.Remove((INoteControllerNoteDidStartJumpEvent) this);
    noteController.noteDidFinishJumpEvent.Remove((INoteControllerNoteDidFinishJumpEvent) this);
    noteController.noteWasCutEvent.Remove((INoteControllerNoteWasCutEvent) this);
    noteController.noteWasMissedEvent.Remove((INoteControllerNoteWasMissedEvent) this);
    noteController.noteDidDissolveEvent.Remove((INoteControllerNoteDidDissolveEvent) this);
  }

  private void SetSliderNoteControllerEventCallbacks(SliderController sliderNoteController)
  {
    sliderNoteController.sliderDidFinishJumpEvent.Add((ISliderDidFinishJumpEvent) this);
    sliderNoteController.sliderDidDissolveEvent.Add((ISliderDidDissolveEvent) this);
  }

  private void RemoveSliderNoteControllerEventCallbacks(SliderController sliderNoteController)
  {
    sliderNoteController.sliderDidFinishJumpEvent.Remove((ISliderDidFinishJumpEvent) this);
    sliderNoteController.sliderDidDissolveEvent.Remove((ISliderDidDissolveEvent) this);
  }

  private void SetObstacleEventCallbacks(ObstacleController obstacleController)
  {
    obstacleController.finishedMovementEvent += new System.Action<ObstacleController>(this.HandleObstacleFinishedMovement);
    obstacleController.passedThreeQuartersOfMove2Event += new System.Action<ObstacleController>(this.HandleObstaclePassedThreeQuartersOfMove2);
    obstacleController.passedAvoidedMarkEvent += new System.Action<ObstacleController>(this.HandleObstaclePassedAvoidedMark);
    obstacleController.didDissolveEvent += new System.Action<ObstacleController>(this.HandleObstacleDidDissolve);
  }

  private void RemoveObstacleEventCallbacks(ObstacleController obstacleController)
  {
    obstacleController.finishedMovementEvent -= new System.Action<ObstacleController>(this.HandleObstacleFinishedMovement);
    obstacleController.passedThreeQuartersOfMove2Event -= new System.Action<ObstacleController>(this.HandleObstaclePassedThreeQuartersOfMove2);
    obstacleController.passedAvoidedMarkEvent -= new System.Action<ObstacleController>(this.HandleObstaclePassedAvoidedMark);
    obstacleController.didDissolveEvent -= new System.Action<ObstacleController>(this.HandleObstacleDidDissolve);
  }

  protected void Despawn(NoteController noteController)
  {
    this.RemoveNoteControllerEventCallbacks(noteController);
    this.DespawnInternal(noteController);
    this._allBeatmapObjects.Remove((IBeatmapObjectController) noteController);
    System.Action<NoteController> wasDespawnedEvent = this.noteWasDespawnedEvent;
    if (wasDespawnedEvent == null)
      return;
    wasDespawnedEvent(noteController);
  }

  private void Despawn(ObstacleController obstacleController)
  {
    this.RemoveObstacleEventCallbacks(obstacleController);
    this.DespawnInternal(obstacleController);
    this._allBeatmapObjects.Remove((IBeatmapObjectController) obstacleController);
    System.Action<ObstacleController> wasDespawnedEvent = this.obstacleWasDespawnedEvent;
    if (wasDespawnedEvent == null)
      return;
    wasDespawnedEvent(obstacleController);
  }

  private void Despawn(SliderController sliderNoteController)
  {
    this.RemoveSliderNoteControllerEventCallbacks(sliderNoteController);
    this._allBeatmapObjects.Remove((IBeatmapObjectController) sliderNoteController);
    this.DespawnInternal(sliderNoteController);
    System.Action<SliderController> wasDespawnedEvent = this.sliderWasDespawnedEvent;
    if (wasDespawnedEvent == null)
      return;
    wasDespawnedEvent(sliderNoteController);
  }

  public void HandleNoteControllerNoteDidStartJump(NoteController noteController)
  {
    System.Action<NoteController> didStartJumpEvent = this.noteDidStartJumpEvent;
    if (didStartJumpEvent == null)
      return;
    didStartJumpEvent(noteController);
  }

  public void HandleNoteControllerNoteWasMissed(NoteController noteController)
  {
    System.Action<NoteController> noteWasMissedEvent = this.noteWasMissedEvent;
    if (noteWasMissedEvent == null)
      return;
    noteWasMissedEvent(noteController);
  }

  public void HandleNoteControllerNoteDidFinishJump(NoteController noteController) => this.Despawn(noteController);

  public void HandleNoteControllerNoteDidDissolve(NoteController noteController) => this.Despawn(noteController);

  public void HandleNoteControllerNoteWasCut(
    NoteController noteController,
    in NoteCutInfo noteCutInfo)
  {
    BeatmapObjectManager.NoteWasCutDelegate noteWasCutEvent = this.noteWasCutEvent;
    if (noteWasCutEvent != null)
      noteWasCutEvent(noteController, in noteCutInfo);
    this.Despawn(noteController);
  }

  private void HandleObstaclePassedThreeQuartersOfMove2(ObstacleController obstacleController)
  {
    System.Action<ObstacleController> quartersOfMove2Event = this.obstacleDidPassThreeQuartersOfMove2Event;
    if (quartersOfMove2Event == null)
      return;
    quartersOfMove2Event(obstacleController);
  }

  private void HandleObstaclePassedAvoidedMark(ObstacleController obstacleController)
  {
    System.Action<ObstacleController> avoidedMarkEvent = this.obstacleDidPassAvoidedMarkEvent;
    if (avoidedMarkEvent == null)
      return;
    avoidedMarkEvent(obstacleController);
  }

  private void HandleObstacleFinishedMovement(ObstacleController obstacleController) => this.Despawn(obstacleController);

  private void HandleObstacleDidDissolve(ObstacleController obstacleController) => this.Despawn(obstacleController);

  public void HandleSliderDidFinishJump(SliderController sliderController) => this.Despawn(sliderController);

  public void HandleSliderDidDissolve(SliderController sliderController) => this.Despawn(sliderController);

  public void DissolveAllObjects()
  {
    foreach (IBeatmapObjectController allBeatmapObject in this._allBeatmapObjects)
      allBeatmapObject.Dissolve(1.4f);
  }

  public void HideAllBeatmapObjects(bool hide)
  {
    foreach (IBeatmapObjectController allBeatmapObject in this._allBeatmapObjects)
      allBeatmapObject.Hide(hide);
    System.Action<bool> beatmapObjectsEvent = this.didHideAllBeatmapObjectsEvent;
    if (beatmapObjectsEvent == null)
      return;
    beatmapObjectsEvent(hide);
  }

  public void PauseAllBeatmapObjects(bool pause)
  {
    foreach (IBeatmapObjectController allBeatmapObject in this._allBeatmapObjects)
      allBeatmapObject.Pause(pause);
  }

  void INoteControllerNoteWasCutEvent.HandleNoteControllerNoteWasCut(
    NoteController noteController,
    in NoteCutInfo noteCutInfo)
  {
    this.HandleNoteControllerNoteWasCut(noteController, in noteCutInfo);
  }

  public delegate void NoteWasCutDelegate(NoteController noteController, in NoteCutInfo noteCutInfo);
}
