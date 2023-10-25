// Decompiled with JetBrains decompiler
// Type: MultiplayerConnectedPlayerBeatmapObjectManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MultiplayerConnectedPlayerBeatmapObjectManager : BeatmapObjectManager, IDisposable
{
  protected float? _firstBasicNoteTime;
  protected readonly MemoryPoolContainer<MultiplayerConnectedPlayerGameNoteController> _gameNotePoolContainer;
  protected readonly MemoryPoolContainer<MultiplayerConnectedPlayerGameNoteController> _burstSliderHeadGameNotePoolContainer;
  protected readonly MemoryPoolContainer<MultiplayerConnectedPlayerGameNoteController> _burstSliderGameNotePoolContainer;
  protected readonly MemoryPoolContainer<MultiplayerConnectedPlayerGameNoteController> _burstSliderFillPoolContainer;
  protected readonly MemoryPoolContainer<MultiplayerConnectedPlayerBombNoteController> _bombNotePoolContainer;
  protected readonly MemoryPoolContainer<MultiplayerConnectedPlayerObstacleController, ObstacleController> _obstaclePoolContainer;
  protected readonly IConnectedPlayerBeatmapObjectEventManager _beatmapObjectEventManager;
  protected readonly MultiplayerConnectedPlayerBeatmapObjectManager.InitData _initData;

  public override List<ObstacleController> activeObstacleControllers => this._obstaclePoolContainer.activeItems;

  private MultiplayerConnectedPlayerBeatmapObjectManager(
    MultiplayerConnectedPlayerBeatmapObjectManager.InitData initData,
    IConnectedPlayerBeatmapObjectEventManager beatmapObjectEventManager,
    [Inject(Id = NoteData.GameplayType.Normal)] MultiplayerConnectedPlayerGameNoteController.Pool gameNotePool,
    [Inject(Id = NoteData.GameplayType.BurstSliderHead)] MultiplayerConnectedPlayerGameNoteController.Pool burstSliderHeadGameNotePool,
    [Inject(Id = NoteData.GameplayType.BurstSliderElement)] MultiplayerConnectedPlayerGameNoteController.Pool burstSliderGameNotePool,
    [Inject(Id = NoteData.GameplayType.BurstSliderElementFill)] MultiplayerConnectedPlayerGameNoteController.Pool burstSliderFillPool,
    MultiplayerConnectedPlayerBombNoteController.Pool bombNotePool,
    MultiplayerConnectedPlayerObstacleController.Pool obstaclePool)
  {
    this._initData = initData;
    this._beatmapObjectEventManager = beatmapObjectEventManager;
    this._beatmapObjectEventManager.connectedPlayerNoteWasSpawnedEvent += new System.Action<NoteSpawnInfoNetSerializable>(this.HandleMultiplayerBeatmapObjectEventManagerBeatmapObjectWasSpawned);
    this._beatmapObjectEventManager.connectedPlayerObstacleWasSpawnedEvent += new System.Action<ObstacleSpawnInfoNetSerializable>(this.HandleMultiplayerBeatmapObjectEventManagerObstacleWasSpawned);
    this._beatmapObjectEventManager.connectedPlayerSliderWasSpawnedEvent += new System.Action<SliderSpawnInfoNetSerializable>(this.HandleMultiplayerBeatmapObjectEventManagerSliderWasSpawned);
    this._beatmapObjectEventManager.connectedPlayerNoteWasCutEvent += new System.Action<NoteCutInfoNetSerializable>(this.HandleMultiplayerBeatmapObjectEventManagerBeatmapObjectWasCut);
    this._gameNotePoolContainer = new MemoryPoolContainer<MultiplayerConnectedPlayerGameNoteController>((IMemoryPool<MultiplayerConnectedPlayerGameNoteController>) gameNotePool);
    this._burstSliderHeadGameNotePoolContainer = new MemoryPoolContainer<MultiplayerConnectedPlayerGameNoteController>((IMemoryPool<MultiplayerConnectedPlayerGameNoteController>) burstSliderHeadGameNotePool);
    this._burstSliderGameNotePoolContainer = new MemoryPoolContainer<MultiplayerConnectedPlayerGameNoteController>((IMemoryPool<MultiplayerConnectedPlayerGameNoteController>) burstSliderGameNotePool);
    this._burstSliderFillPoolContainer = new MemoryPoolContainer<MultiplayerConnectedPlayerGameNoteController>((IMemoryPool<MultiplayerConnectedPlayerGameNoteController>) burstSliderFillPool);
    this._bombNotePoolContainer = new MemoryPoolContainer<MultiplayerConnectedPlayerBombNoteController>((IMemoryPool<MultiplayerConnectedPlayerBombNoteController>) bombNotePool);
    this._obstaclePoolContainer = new MemoryPoolContainer<MultiplayerConnectedPlayerObstacleController, ObstacleController>((IMemoryPool<MultiplayerConnectedPlayerObstacleController>) obstaclePool);
  }

  public virtual void Dispose()
  {
    if (this._beatmapObjectEventManager == null)
      return;
    this._beatmapObjectEventManager.connectedPlayerNoteWasSpawnedEvent -= new System.Action<NoteSpawnInfoNetSerializable>(this.HandleMultiplayerBeatmapObjectEventManagerBeatmapObjectWasSpawned);
    this._beatmapObjectEventManager.connectedPlayerObstacleWasSpawnedEvent -= new System.Action<ObstacleSpawnInfoNetSerializable>(this.HandleMultiplayerBeatmapObjectEventManagerObstacleWasSpawned);
    this._beatmapObjectEventManager.connectedPlayerSliderWasSpawnedEvent -= new System.Action<SliderSpawnInfoNetSerializable>(this.HandleMultiplayerBeatmapObjectEventManagerSliderWasSpawned);
    this._beatmapObjectEventManager.connectedPlayerNoteWasCutEvent -= new System.Action<NoteCutInfoNetSerializable>(this.HandleMultiplayerBeatmapObjectEventManagerBeatmapObjectWasCut);
  }

  public override void ProcessObstacleData(
    ObstacleData obstacleData,
    in BeatmapObjectSpawnMovementData.ObstacleSpawnData obstacleSpawnData,
    float rotation)
  {
    MultiplayerConnectedPlayerObstacleController obstacleController = this._obstaclePoolContainer.Spawn();
    obstacleController.Init(obstacleData, rotation, obstacleSpawnData.moveEndPos, obstacleSpawnData.moveEndPos, obstacleSpawnData.jumpEndPos, 0.0f, obstacleSpawnData.jumpDuration, obstacleSpawnData.noteLinesDistance, obstacleSpawnData.obstacleHeight);
    this.AddSpawnedObstacleController((ObstacleController) obstacleController, obstacleSpawnData, rotation);
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
      case NoteData.GameplayType.BurstSliderHead:
      case NoteData.GameplayType.BurstSliderElement:
      case NoteData.GameplayType.BurstSliderElementFill:
        if (!this._firstBasicNoteTime.HasValue)
          this._firstBasicNoteTime = new float?(noteData.time);
        NoteVisualModifierType noteVisualModifierType = NoteVisualModifierType.Normal;
        float? firstBasicNoteTime = this._firstBasicNoteTime;
        float time = noteData.time;
        bool flag = (double) firstBasicNoteTime.GetValueOrDefault() == (double) time & firstBasicNoteTime.HasValue;
        if (this._initData.ghostNotes && !flag)
          noteVisualModifierType = NoteVisualModifierType.Ghost;
        else if (this._initData.disappearingArrows)
          noteVisualModifierType = NoteVisualModifierType.DisappearingArrow;
        MultiplayerConnectedPlayerGameNoteController gameNoteController = (MultiplayerConnectedPlayerGameNoteController) null;
        switch (noteData.gameplayType)
        {
          case NoteData.GameplayType.Normal:
            gameNoteController = this._gameNotePoolContainer.Spawn();
            break;
          case NoteData.GameplayType.BurstSliderHead:
            gameNoteController = this._burstSliderHeadGameNotePoolContainer.Spawn();
            break;
          case NoteData.GameplayType.BurstSliderElement:
            gameNoteController = this._burstSliderGameNotePoolContainer.Spawn();
            break;
          case NoteData.GameplayType.BurstSliderElementFill:
            gameNoteController = this._burstSliderFillPoolContainer.Spawn();
            break;
        }
        if ((UnityEngine.Object) gameNoteController == (UnityEngine.Object) null)
        {
          Debug.LogError((object) string.Format("Unsupported note {0}", (object) noteData.gameplayType));
          break;
        }
        gameNoteController.Init(noteData, rotation, noteSpawnData.moveStartPos, noteSpawnData.moveEndPos, noteSpawnData.jumpEndPos, noteSpawnData.moveDuration, noteSpawnData.jumpDuration, noteSpawnData.jumpGravity, noteVisualModifierType, this._initData.notesUniformScale);
        this.AddSpawnedNoteController((NoteController) gameNoteController, noteSpawnData, rotation);
        break;
      case NoteData.GameplayType.Bomb:
        MultiplayerConnectedPlayerBombNoteController bombNoteController = this._bombNotePoolContainer.Spawn();
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
      case MultiplayerConnectedPlayerGameNoteController gameNoteController:
        switch (gameNoteController.gameplayType)
        {
          case NoteData.GameplayType.Normal:
            this._gameNotePoolContainer.Despawn(gameNoteController);
            return;
          case NoteData.GameplayType.Bomb:
            return;
          case NoteData.GameplayType.BurstSliderHead:
            this._burstSliderHeadGameNotePoolContainer.Despawn(gameNoteController);
            return;
          case NoteData.GameplayType.BurstSliderElement:
            this._burstSliderGameNotePoolContainer.Despawn(gameNoteController);
            return;
          case NoteData.GameplayType.BurstSliderElementFill:
            this._burstSliderFillPoolContainer.Despawn(gameNoteController);
            return;
          default:
            return;
        }
      case MultiplayerConnectedPlayerBombNoteController bombNoteController:
        this._bombNotePoolContainer.Despawn(bombNoteController);
        break;
    }
  }

  protected override void DespawnInternal(ObstacleController obstacleController)
  {
    if (!(obstacleController is MultiplayerConnectedPlayerObstacleController obstacleController1))
      return;
    this._obstaclePoolContainer.Despawn(obstacleController1);
  }

  protected override void DespawnInternal(SliderController sliderNoteController)
  {
  }

  public virtual void HandleMultiplayerBeatmapObjectEventManagerBeatmapObjectWasSpawned(
    NoteSpawnInfoNetSerializable noteSpawnInfo)
  {
    NoteDataFromNoteSpawnInfoNetSerializable infoNetSerializable = new NoteDataFromNoteSpawnInfoNetSerializable(noteSpawnInfo);
    BeatmapObjectSpawnMovementData.NoteSpawnData noteSpawnData = new BeatmapObjectSpawnMovementData.NoteSpawnData((Vector3) noteSpawnInfo.moveStartPos, (Vector3) noteSpawnInfo.moveEndPos, (Vector3) noteSpawnInfo.jumpEndPos, noteSpawnInfo.jumpGravity, noteSpawnInfo.moveDuration, noteSpawnInfo.jumpDuration);
    this.ProcessNoteData((NoteData) infoNetSerializable, in noteSpawnData, noteSpawnInfo.rotation, false);
  }

  public virtual void HandleMultiplayerBeatmapObjectEventManagerObstacleWasSpawned(
    ObstacleSpawnInfoNetSerializable obstacleSpawnInfo)
  {
    ObstacleData obstacleData = new ObstacleData(obstacleSpawnInfo.time, obstacleSpawnInfo.lineIndex, obstacleSpawnInfo.lineLayer, obstacleSpawnInfo.duration, obstacleSpawnInfo.width, obstacleSpawnInfo.height);
    BeatmapObjectSpawnMovementData.ObstacleSpawnData obstacleSpawnData = new BeatmapObjectSpawnMovementData.ObstacleSpawnData((Vector3) obstacleSpawnInfo.moveStartPos, (Vector3) obstacleSpawnInfo.moveEndPos, (Vector3) obstacleSpawnInfo.jumpEndPos, obstacleSpawnInfo.obstacleHeight, obstacleSpawnInfo.moveDuration, obstacleSpawnInfo.jumpDuration, obstacleSpawnInfo.noteLinesDistance);
    this.ProcessObstacleData(obstacleData, in obstacleSpawnData, obstacleSpawnInfo.rotation);
  }

  public virtual void HandleMultiplayerBeatmapObjectEventManagerSliderWasSpawned(
    SliderSpawnInfoNetSerializable sliderSpawnInfo)
  {
    SliderData sliderData = new SliderData(sliderSpawnInfo.sliderType, sliderSpawnInfo.colorType, sliderSpawnInfo.hasHeadNote, sliderSpawnInfo.headTime, sliderSpawnInfo.headLineIndex, sliderSpawnInfo.headLineLayer, sliderSpawnInfo.headBeforeJumpLineLayer, sliderSpawnInfo.headControlPointLengthMultiplier, sliderSpawnInfo.headCutDirection, sliderSpawnInfo.headCutDirectionAngleOffset, sliderSpawnInfo.hasTailNote, sliderSpawnInfo.tailTime, sliderSpawnInfo.tailLineIndex, sliderSpawnInfo.tailLineLayer, sliderSpawnInfo.tailBeforeJumpLineLayer, sliderSpawnInfo.tailControlPointLengthMultiplier, sliderSpawnInfo.tailCutDirection, sliderSpawnInfo.tailCutDirectionAngleOffset, sliderSpawnInfo.midAnchorMode, sliderSpawnInfo.sliceCount, sliderSpawnInfo.squishAmount);
    BeatmapObjectSpawnMovementData.SliderSpawnData sliderSpawnData = new BeatmapObjectSpawnMovementData.SliderSpawnData((Vector3) sliderSpawnInfo.headMoveStartPos, (Vector3) sliderSpawnInfo.headJumpStartPos, (Vector3) sliderSpawnInfo.headJumpEndPos, sliderSpawnInfo.headJumpGravity, (Vector3) sliderSpawnInfo.tailMoveStartPos, (Vector3) sliderSpawnInfo.tailJumpStartPos, (Vector3) sliderSpawnInfo.tailJumpEndPos, sliderSpawnInfo.tailJumpGravity, sliderSpawnInfo.moveDuration, sliderSpawnInfo.jumpDuration);
    this.ProcessSliderData(sliderData, in sliderSpawnData, sliderSpawnInfo.rotation);
  }

  public virtual void HandleMultiplayerBeatmapObjectEventManagerBeatmapObjectWasCut(
    NoteCutInfoNetSerializable noteCutInfo)
  {
    foreach (MultiplayerConnectedPlayerGameNoteController activeItem in this._gameNotePoolContainer.activeItems)
    {
      if (MultiplayerConnectedPlayerBeatmapObjectManager.AreNotesSame((NoteController) activeItem, noteCutInfo))
      {
        this.Despawn((NoteController) activeItem);
        return;
      }
    }
    foreach (MultiplayerConnectedPlayerGameNoteController activeItem in this._burstSliderHeadGameNotePoolContainer.activeItems)
    {
      if (MultiplayerConnectedPlayerBeatmapObjectManager.AreNotesSame((NoteController) activeItem, noteCutInfo))
      {
        this.Despawn((NoteController) activeItem);
        return;
      }
    }
    foreach (MultiplayerConnectedPlayerGameNoteController activeItem in this._burstSliderGameNotePoolContainer.activeItems)
    {
      if (MultiplayerConnectedPlayerBeatmapObjectManager.AreNotesSame((NoteController) activeItem, noteCutInfo))
      {
        this.Despawn((NoteController) activeItem);
        return;
      }
    }
    foreach (MultiplayerConnectedPlayerGameNoteController activeItem in this._burstSliderFillPoolContainer.activeItems)
    {
      if (MultiplayerConnectedPlayerBeatmapObjectManager.AreNotesSame((NoteController) activeItem, noteCutInfo))
      {
        this.Despawn((NoteController) activeItem);
        return;
      }
    }
    foreach (MultiplayerConnectedPlayerBombNoteController activeItem in this._bombNotePoolContainer.activeItems)
    {
      if (MultiplayerConnectedPlayerBeatmapObjectManager.AreNotesSame((NoteController) activeItem, noteCutInfo))
      {
        this.Despawn((NoteController) activeItem);
        break;
      }
    }
  }

  private static bool AreNotesSame(
    NoteController noteController,
    NoteCutInfoNetSerializable noteCutInfo)
  {
    NoteData noteData = noteController.noteData;
    return Mathf.Approximately(noteData.time, noteCutInfo.noteTime) && noteData.lineIndex == noteCutInfo.noteLineIndex && noteData.noteLineLayer == noteCutInfo.lineLayer;
  }

  public class InitData
  {
    public readonly bool disappearingArrows;
    public readonly bool ghostNotes;
    public readonly float notesUniformScale;

    public InitData(bool disappearingArrows, bool ghostNotes, float notesUniformScale)
    {
      this.disappearingArrows = disappearingArrows;
      this.ghostNotes = ghostNotes;
      this.notesUniformScale = notesUniformScale;
    }
  }
}
