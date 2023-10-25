// Decompiled with JetBrains decompiler
// Type: BeatmapObjectSpawnMovementData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;

[Serializable]
public class BeatmapObjectSpawnMovementData
{
  [Header("Global")]
  [SerializeField]
  protected Vector3 _centerPos = new Vector3(0.0f, 0.0f, 0.65f);
  [Header("Jump")]
  [Tooltip("If half jump distance computed using halfJumpDurationInBeats is longer than this value, it is divided by two until it's smaller.")]
  [SerializeField]
  protected float _maxHalfJumpDistance = 18f;
  [SerializeField]
  protected float _startHalfJumpDurationInBeats = 4f;
  [SerializeField]
  protected float _baseLinesHighestJumpPosY = 0.85f;
  [SerializeField]
  protected float _upperLinesHighestJumpPosY = 1.4f;
  [SerializeField]
  protected float _topLinesHighestJumpPosY = 1.9f;
  [Header("Arrival")]
  [SerializeField]
  protected float _moveSpeed = 200f;
  [SerializeField]
  protected float _moveDuration = 1f;
  [Header("Obstacles")]
  [SerializeField]
  protected float _verticalObstaclePosY = 0.1f;
  [SerializeField]
  protected float _obstacleTopPosY = 3.1f;
  protected float _spawnAheadTime;
  protected float _jumpDuration;
  protected float _noteJumpStartBeatOffset;
  protected float _noteJumpMovementSpeed;
  protected float _jumpDistance;
  protected float _moveDistance;
  protected Vector3 _moveStartPos;
  protected Vector3 _moveEndPos;
  protected Vector3 _jumpEndPos;
  protected int _noteLinesCount = 4;
  protected IJumpOffsetYProvider _jumpOffsetYProvider;
  protected Vector3 _rightVec;
  protected Vector3 _forwardVec;
  public const float kDefaultMaxHalfJumpDistance = 18f;
  public const float kDefaultStartHalfJumpDurationInBeats = 4f;

  public float spawnAheadTime => this._spawnAheadTime;

  public float moveDuration => this._moveDuration;

  public float jumpDuration => this._jumpDuration;

  public float noteLinesDistance => 0.6f;

  public float verticalLayersDistance => 0.6f;

  public float jumpDistance => this._jumpDistance;

  public float noteJumpMovementSpeed => this._noteJumpMovementSpeed;

  public int noteLinesCount => this._noteLinesCount;

  public Vector3 centerPos => this._centerPos;

  public float jumpOffsetY => this._jumpOffsetYProvider.jumpOffsetY;

  public virtual void Init(
    int noteLinesCount,
    float startNoteJumpMovementSpeed,
    float startBpm,
    BeatmapObjectSpawnMovementData.NoteJumpValueType noteJumpValueType,
    float noteJumpValue,
    IJumpOffsetYProvider jumpOffsetYProvider,
    Vector3 rightVec,
    Vector3 forwardVec)
  {
    this._noteLinesCount = noteLinesCount;
    this._noteJumpMovementSpeed = startNoteJumpMovementSpeed;
    switch (noteJumpValueType)
    {
      case BeatmapObjectSpawnMovementData.NoteJumpValueType.BeatOffset:
        this._noteJumpStartBeatOffset = noteJumpValue;
        float oneBeatDuration = startBpm.OneBeatDuration();
        float num = CoreMathUtils.CalculateHalfJumpDurationInBeats(this._startHalfJumpDurationInBeats, this._maxHalfJumpDistance, this.noteJumpMovementSpeed, oneBeatDuration, this._noteJumpStartBeatOffset) * 2f;
        this._jumpDuration = oneBeatDuration * num;
        break;
      case BeatmapObjectSpawnMovementData.NoteJumpValueType.JumpDuration:
        this._jumpDuration = noteJumpValue * 2f;
        break;
    }
    this._rightVec = rightVec;
    this._forwardVec = forwardVec;
    this._jumpOffsetYProvider = jumpOffsetYProvider;
    this._moveDistance = this._moveSpeed * this._moveDuration;
    this._jumpDistance = this._noteJumpMovementSpeed * this._jumpDuration;
    this._moveEndPos = this._centerPos + this._forwardVec * (this._jumpDistance * 0.5f);
    this._jumpEndPos = this._centerPos - this._forwardVec * (this._jumpDistance * 0.5f);
    this._moveStartPos = this._centerPos + this._forwardVec * (this._moveDistance + this._jumpDistance * 0.5f);
    this._spawnAheadTime = this._moveDuration + this._jumpDuration * 0.5f;
  }

  public virtual BeatmapObjectSpawnMovementData.ObstacleSpawnData GetObstacleSpawnData(
    ObstacleData obstacleData)
  {
    Vector3 obstacleOffset = this.GetObstacleOffset(obstacleData.lineIndex, obstacleData.lineLayer);
    obstacleOffset.y += this._jumpOffsetYProvider.jumpOffsetY;
    obstacleOffset.y = Mathf.Max(obstacleOffset.y, this._verticalObstaclePosY);
    float num = Mathf.Min((float) obstacleData.height * StaticBeatmapObjectSpawnMovementData.layerHeight, this._obstacleTopPosY - obstacleOffset.y);
    Vector3 moveStartPos = this._moveStartPos + obstacleOffset;
    Vector3 vector3_1 = this._moveEndPos + obstacleOffset;
    Vector3 vector3_2 = this._jumpEndPos + obstacleOffset;
    Vector3 moveEndPos = vector3_1;
    Vector3 jumpEndPos = vector3_2;
    double obstacleHeight = (double) num;
    double moveDuration = (double) this.moveDuration;
    double jumpDuration = (double) this.jumpDuration;
    double noteLinesDistance = (double) this.noteLinesDistance;
    return new BeatmapObjectSpawnMovementData.ObstacleSpawnData(moveStartPos, moveEndPos, jumpEndPos, (float) obstacleHeight, (float) moveDuration, (float) jumpDuration, (float) noteLinesDistance);
  }

  public virtual BeatmapObjectSpawnMovementData.NoteSpawnData GetJumpingNoteSpawnData(
    NoteData noteData)
  {
    Vector3 noteOffset1 = this.GetNoteOffset(noteData.lineIndex, noteData.beforeJumpNoteLineLayer);
    float jumpGravity = this.NoteJumpGravityForLineLayer(noteData.noteLineLayer, noteData.beforeJumpNoteLineLayer);
    Vector3 jumpEndPos = this._jumpEndPos + noteOffset1;
    Vector3 moveStartPos;
    Vector3 moveEndPos;
    if (noteData.colorType != ColorType.None)
    {
      Vector3 noteOffset2 = this.GetNoteOffset(noteData.flipLineIndex, noteData.beforeJumpNoteLineLayer);
      moveStartPos = this._moveStartPos + noteOffset2;
      moveEndPos = this._moveEndPos + noteOffset2;
    }
    else
    {
      moveStartPos = this._moveStartPos + noteOffset1;
      moveEndPos = this._moveEndPos + noteOffset1;
    }
    return new BeatmapObjectSpawnMovementData.NoteSpawnData(moveStartPos, moveEndPos, jumpEndPos, jumpGravity, this.moveDuration, this.jumpDuration);
  }

  public virtual BeatmapObjectSpawnMovementData.SliderSpawnData GetSliderSpawnData(
    SliderData sliderData)
  {
    Vector3 noteOffset1 = this.GetNoteOffset(sliderData.headLineIndex, sliderData.headBeforeJumpLineLayer);
    float num1 = this.NoteJumpGravityForLineLayer(sliderData.headLineLayer, sliderData.headBeforeJumpLineLayer);
    Vector3 headMoveStartPos = this._moveStartPos + noteOffset1;
    Vector3 vector3_1 = this._moveEndPos + noteOffset1;
    Vector3 vector3_2 = this._jumpEndPos + noteOffset1;
    Vector3 noteOffset2 = this.GetNoteOffset(sliderData.tailLineIndex, sliderData.tailBeforeJumpLineLayer);
    float num2 = this.NoteJumpGravityForLineLayer(sliderData.tailLineLayer, sliderData.tailBeforeJumpLineLayer);
    Vector3 vector3_3 = this._moveStartPos + noteOffset2;
    Vector3 vector3_4 = this._moveEndPos + noteOffset2;
    Vector3 vector3_5 = this._jumpEndPos + noteOffset2;
    Vector3 headJumpStartPos = vector3_1;
    Vector3 headJumpEndPos = vector3_2;
    double headJumpGravity = (double) num1;
    Vector3 tailMoveStartPos = vector3_3;
    Vector3 tailJumpStartPos = vector3_4;
    Vector3 tailJumpEndPos = vector3_5;
    double tailJumpGravity = (double) num2;
    double moveDuration = (double) this.moveDuration;
    double jumpDuration = (double) this.jumpDuration;
    return new BeatmapObjectSpawnMovementData.SliderSpawnData(headMoveStartPos, headJumpStartPos, headJumpEndPos, (float) headJumpGravity, tailMoveStartPos, tailJumpStartPos, tailJumpEndPos, (float) tailJumpGravity, (float) moveDuration, (float) jumpDuration);
  }

  public virtual Vector3 GetNoteOffset(int noteLineIndex, NoteLineLayer noteLineLayer) => this._rightVec * (float) (((double) ((float) -(this._noteLinesCount - 1) * 0.5f) + (double) noteLineIndex) * 0.60000002384185791) + new Vector3(0.0f, StaticBeatmapObjectSpawnMovementData.LineYPosForLineLayer(noteLineLayer), 0.0f);

  public virtual Vector3 GetObstacleOffset(int noteLineIndex, NoteLineLayer noteLineLayer) => this._rightVec * (float) (((double) ((float) -(this._noteLinesCount - 1) * 0.5f) + (double) noteLineIndex) * 0.60000002384185791) + new Vector3(0.0f, StaticBeatmapObjectSpawnMovementData.LineYPosForLineLayer(noteLineLayer) - 0.15f, 0.0f);

  public virtual Vector2 Get2DNoteOffset(int noteLineIndex, NoteLineLayer noteLineLayer) => StaticBeatmapObjectSpawnMovementData.Get2DNoteOffset(noteLineIndex, this._noteLinesCount, noteLineLayer);

  public virtual float JumpPosYForLineLayerAtDistanceFromPlayerWithoutJumpOffset(
    NoteLineLayer lineLayer,
    float distanceFromPlayer)
  {
    float num1 = (this._jumpDistance * 0.5f - distanceFromPlayer) / this._noteJumpMovementSpeed;
    float num2 = this.NoteJumpGravityForLineLayerWithoutJumpOffset(lineLayer, NoteLineLayer.Base);
    float num3 = (float) ((double) num2 * (double) this._jumpDuration * 0.5);
    return (float) ((double) StaticBeatmapObjectSpawnMovementData.LineYPosForLineLayer(NoteLineLayer.Base) + (double) num3 * (double) num1 - (double) num2 * (double) num1 * (double) num1 * 0.5);
  }

  public virtual float HighestJumpPosYForLineLayer(NoteLineLayer lineLayer)
  {
    if (lineLayer == NoteLineLayer.Base)
      return this._baseLinesHighestJumpPosY + this._jumpOffsetYProvider.jumpOffsetY;
    return lineLayer == NoteLineLayer.Upper ? this._upperLinesHighestJumpPosY + this._jumpOffsetYProvider.jumpOffsetY : this._topLinesHighestJumpPosY + this._jumpOffsetYProvider.jumpOffsetY;
  }

  public virtual float HighestJumpPosYForLineLayerWithoutJumpOffset(NoteLineLayer lineLayer)
  {
    if (lineLayer == NoteLineLayer.Base)
      return this._baseLinesHighestJumpPosY;
    return lineLayer == NoteLineLayer.Upper ? this._upperLinesHighestJumpPosY : this._topLinesHighestJumpPosY;
  }

  public virtual float NoteJumpGravityForLineLayer(
    NoteLineLayer lineLayer,
    NoteLineLayer beforeJumpLineLayer)
  {
    float num = (float) ((double) this._jumpDistance / (double) this._noteJumpMovementSpeed * 0.5);
    return (float) (2.0 * ((double) this.HighestJumpPosYForLineLayer(lineLayer) - (double) StaticBeatmapObjectSpawnMovementData.LineYPosForLineLayer(beforeJumpLineLayer)) / ((double) num * (double) num));
  }

  public virtual float NoteJumpGravityForLineLayerWithoutJumpOffset(
    NoteLineLayer lineLayer,
    NoteLineLayer beforeJumpLineLayer)
  {
    float num = (float) ((double) this._jumpDistance / (double) this._noteJumpMovementSpeed * 0.5);
    return (float) (2.0 * ((double) this.HighestJumpPosYForLineLayerWithoutJumpOffset(lineLayer) - (double) StaticBeatmapObjectSpawnMovementData.LineYPosForLineLayer(beforeJumpLineLayer)) / ((double) num * (double) num));
  }

  public readonly struct ObstacleSpawnData
  {
    public readonly Vector3 moveStartPos;
    public readonly Vector3 moveEndPos;
    public readonly Vector3 jumpEndPos;
    public readonly float obstacleHeight;
    public readonly float moveDuration;
    public readonly float jumpDuration;
    public readonly float noteLinesDistance;

    public ObstacleSpawnData(
      Vector3 moveStartPos,
      Vector3 moveEndPos,
      Vector3 jumpEndPos,
      float obstacleHeight,
      float moveDuration,
      float jumpDuration,
      float noteLinesDistance)
    {
      this.moveStartPos = moveStartPos;
      this.moveEndPos = moveEndPos;
      this.jumpEndPos = jumpEndPos;
      this.obstacleHeight = obstacleHeight;
      this.moveDuration = moveDuration;
      this.jumpDuration = jumpDuration;
      this.noteLinesDistance = noteLinesDistance;
    }
  }

  public readonly struct NoteSpawnData
  {
    public readonly Vector3 moveStartPos;
    public readonly Vector3 moveEndPos;
    public readonly Vector3 jumpEndPos;
    public readonly float jumpGravity;
    public readonly float moveDuration;
    public readonly float jumpDuration;

    public NoteSpawnData(
      Vector3 moveStartPos,
      Vector3 moveEndPos,
      Vector3 jumpEndPos,
      float jumpGravity,
      float moveDuration,
      float jumpDuration)
    {
      this.moveStartPos = moveStartPos;
      this.moveEndPos = moveEndPos;
      this.jumpEndPos = jumpEndPos;
      this.jumpGravity = jumpGravity;
      this.moveDuration = moveDuration;
      this.jumpDuration = jumpDuration;
    }
  }

  public readonly struct SliderSpawnData
  {
    public readonly Vector3 headMoveStartPos;
    public readonly Vector3 headJumpStartPos;
    public readonly Vector3 headJumpEndPos;
    public readonly float headJumpGravity;
    public readonly Vector3 tailMoveStartPos;
    public readonly Vector3 tailJumpStartPos;
    public readonly Vector3 tailJumpEndPos;
    public readonly float tailJumpGravity;
    public readonly float moveDuration;
    public readonly float jumpDuration;

    public SliderSpawnData(
      Vector3 headMoveStartPos,
      Vector3 headJumpStartPos,
      Vector3 headJumpEndPos,
      float headJumpGravity,
      Vector3 tailMoveStartPos,
      Vector3 tailJumpStartPos,
      Vector3 tailJumpEndPos,
      float tailJumpGravity,
      float moveDuration,
      float jumpDuration)
    {
      this.headMoveStartPos = headMoveStartPos;
      this.tailMoveStartPos = tailMoveStartPos;
      this.headJumpStartPos = headJumpStartPos;
      this.headJumpEndPos = headJumpEndPos;
      this.headJumpGravity = headJumpGravity;
      this.tailJumpStartPos = tailJumpStartPos;
      this.tailJumpEndPos = tailJumpEndPos;
      this.tailJumpGravity = tailJumpGravity;
      this.moveDuration = moveDuration;
      this.jumpDuration = jumpDuration;
    }
  }

  public enum NoteJumpValueType
  {
    BeatOffset = 1,
    JumpDuration = 2,
  }
}
