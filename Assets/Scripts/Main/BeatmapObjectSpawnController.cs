// Decompiled with JetBrains decompiler
// Type: BeatmapObjectSpawnController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class BeatmapObjectSpawnController : MonoBehaviour, IBeatmapObjectSpawnController
{
  [SerializeField]
  protected BeatmapObjectSpawnMovementData _beatmapObjectSpawnMovementData = new BeatmapObjectSpawnMovementData();
  [Inject]
  protected readonly BeatmapCallbacksController _beatmapCallbacksController;
  [Inject]
  protected readonly IBeatmapObjectSpawner _beatmapObjectSpawner;
  [Inject]
  protected readonly IJumpOffsetYProvider _jumpOffsetYProvider;
  [Inject]
  protected readonly BeatmapObjectSpawnController.InitData _initData;
  protected bool _disableSpawning;
  protected bool _isInitialized;
  protected BeatmapDataCallbackWrapper _obstacleDataCallbackWrapper;
  protected BeatmapDataCallbackWrapper _noteDataCallbackWrapper;
  protected BeatmapDataCallbackWrapper _sliderDataCallbackWrapper;
  protected BeatmapDataCallbackWrapper _spawnRotationCallbackWrapper;
  protected float _spawnRotation;

  public int noteLinesCount => this._beatmapObjectSpawnMovementData.noteLinesCount;

  public float jumpOffsetY => this._beatmapObjectSpawnMovementData.jumpOffsetY;

  public float moveDuration => this._beatmapObjectSpawnMovementData.moveDuration;

  public float jumpDuration => this._beatmapObjectSpawnMovementData.jumpDuration;

  public float jumpDistance => this._beatmapObjectSpawnMovementData.jumpDistance;

  public float verticalLayerDistance => this._beatmapObjectSpawnMovementData.verticalLayersDistance;

  public float noteJumpMovementSpeed => this._beatmapObjectSpawnMovementData.noteJumpMovementSpeed;

  public float noteLinesDistance => this._beatmapObjectSpawnMovementData.noteLinesDistance;

  public BeatmapObjectSpawnMovementData beatmapObjectSpawnMovementData => this._beatmapObjectSpawnMovementData;

  public bool isInitialized => this._isInitialized;

  public event System.Action didInitEvent;

  public virtual void Start()
  {
    this._beatmapObjectSpawnMovementData.Init(this._initData.noteLinesCount, this._initData.noteJumpMovementSpeed, this._initData.beatsPerMinute, this._initData.noteJumpValueType, this._initData.noteJumpValue, this._jumpOffsetYProvider, Vector3.right, Vector3.forward);
    this._spawnRotation = 0.0f;
    this._obstacleDataCallbackWrapper = this._beatmapCallbacksController.AddBeatmapCallback<ObstacleData>(this._beatmapObjectSpawnMovementData.spawnAheadTime, new BeatmapDataCallback<ObstacleData>(this.HandleObstacleDataCallback));
    this._noteDataCallbackWrapper = this._beatmapCallbacksController.AddBeatmapCallback<NoteData>(this._beatmapObjectSpawnMovementData.spawnAheadTime, new BeatmapDataCallback<NoteData>(this.HandleNoteDataCallback));
    this._sliderDataCallbackWrapper = this._beatmapCallbacksController.AddBeatmapCallback<SliderData>(this._beatmapObjectSpawnMovementData.spawnAheadTime, new BeatmapDataCallback<SliderData>(this.HandleSliderDataCallback));
    this._spawnRotationCallbackWrapper = this._beatmapCallbacksController.AddBeatmapCallback<SpawnRotationBeatmapEventData>(this._beatmapObjectSpawnMovementData.spawnAheadTime, new BeatmapDataCallback<SpawnRotationBeatmapEventData>(this.HandleSpawnRotationCallback));
    this._isInitialized = true;
    System.Action didInitEvent = this.didInitEvent;
    if (didInitEvent == null)
      return;
    didInitEvent();
  }

  public virtual void OnDestroy()
  {
    if (this._beatmapCallbacksController == null)
      return;
    this._beatmapCallbacksController.RemoveBeatmapCallback(this._obstacleDataCallbackWrapper);
    this._beatmapCallbacksController.RemoveBeatmapCallback(this._noteDataCallbackWrapper);
    this._beatmapCallbacksController.RemoveBeatmapCallback(this._sliderDataCallbackWrapper);
    this._beatmapCallbacksController.RemoveBeatmapCallback(this._spawnRotationCallbackWrapper);
  }

  public virtual void HandleObstacleDataCallback(ObstacleData obstacleData)
  {
    if (this._disableSpawning)
      return;
    BeatmapObjectSpawnMovementData.ObstacleSpawnData obstacleSpawnData = this._beatmapObjectSpawnMovementData.GetObstacleSpawnData(obstacleData);
    this._beatmapObjectSpawner.ProcessObstacleData(obstacleData, in obstacleSpawnData, this._spawnRotation);
  }

  public virtual void HandleNoteDataCallback(NoteData noteData)
  {
    if (this._disableSpawning)
      return;
    BeatmapObjectSpawnMovementData.NoteSpawnData noteSpawnData = this._beatmapObjectSpawnMovementData.GetJumpingNoteSpawnData(noteData);
    this._beatmapObjectSpawner.ProcessNoteData(noteData, in noteSpawnData, this._spawnRotation, false);
  }

  public virtual void HandleSliderDataCallback(SliderData sliderNoteData)
  {
    if (this._disableSpawning)
      return;
    BeatmapObjectSpawnMovementData.SliderSpawnData sliderSpawnData = this._beatmapObjectSpawnMovementData.GetSliderSpawnData(sliderNoteData);
    this._beatmapObjectSpawner.ProcessSliderData(sliderNoteData, in sliderSpawnData, this._spawnRotation);
  }

  public virtual void HandleSpawnRotationCallback(SpawnRotationBeatmapEventData beatmapEventData) => this._spawnRotation = beatmapEventData.rotation;

  public virtual void StopSpawning() => this._disableSpawning = true;

  public virtual Vector2 Get2DNoteOffset(int noteLineIndex, NoteLineLayer noteLineLayer) => this._beatmapObjectSpawnMovementData.Get2DNoteOffset(noteLineIndex, noteLineLayer);

  public virtual float JumpPosYForLineLayerAtDistanceFromPlayerWithoutJumpOffset(
    NoteLineLayer lineLayer,
    float distanceFromPlayer)
  {
    return this._beatmapObjectSpawnMovementData.JumpPosYForLineLayerAtDistanceFromPlayerWithoutJumpOffset(lineLayer, distanceFromPlayer);
  }

  public class InitData
  {
    public readonly float beatsPerMinute;
    public readonly int noteLinesCount;
    public readonly float noteJumpMovementSpeed;
    public readonly BeatmapObjectSpawnMovementData.NoteJumpValueType noteJumpValueType;
    public readonly float noteJumpValue;

    public InitData(
      float beatsPerMinute,
      int noteLinesCount,
      float noteJumpMovementSpeed,
      BeatmapObjectSpawnMovementData.NoteJumpValueType noteJumpValueType,
      float noteJumpValue)
    {
      this.beatsPerMinute = beatsPerMinute;
      this.noteLinesCount = noteLinesCount;
      this.noteJumpValueType = noteJumpValueType;
      this.noteJumpMovementSpeed = noteJumpMovementSpeed;
      this.noteJumpValue = noteJumpValue;
    }
  }
}
