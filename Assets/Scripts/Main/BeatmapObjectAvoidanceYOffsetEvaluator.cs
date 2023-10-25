// Decompiled with JetBrains decompiler
// Type: BeatmapObjectAvoidanceYOffsetEvaluator
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class BeatmapObjectAvoidanceYOffsetEvaluator
{
  protected const int kYJumpOffsetBufferLength = 2000;
  protected const float kYJumpOffsetBufferSongTimeInitValue = -1000f;
  protected int _currentYJumpOffsetBufferEndIndex;
  protected readonly float _jumpDurationToDesiredZPosition;
  protected readonly BeatmapObjectAvoidanceYOffsetEvaluator.BufferData[] _yJumpOffsetBuffer = new BeatmapObjectAvoidanceYOffsetEvaluator.BufferData[2000];
  protected readonly IAudioTimeSource _audioTimeSource;
  protected readonly IBeatmapObjectSpawnController _beatmapObjectSpawnController;

  public BeatmapObjectAvoidanceYOffsetEvaluator(
    IAudioTimeSource audioTimeSource,
    IBeatmapObjectSpawnController beatmapObjectSpawnController,
    float moveToPlayerHeadTParam,
    BeatmapObjectSpawnMovementData.NoteSpawnData noteSpawnData)
  {
    this._audioTimeSource = audioTimeSource;
    this._beatmapObjectSpawnController = beatmapObjectSpawnController;
    this._jumpDurationToDesiredZPosition = moveToPlayerHeadTParam * noteSpawnData.jumpDuration;
    for (int index = 0; index < this._yJumpOffsetBuffer.Length; ++index)
      this._yJumpOffsetBuffer[index] = new BeatmapObjectAvoidanceYOffsetEvaluator.BufferData(-1000f, this._beatmapObjectSpawnController.jumpOffsetY);
  }

  public virtual void ManualUpdate()
  {
    float songTime = this._audioTimeSource.songTime;
    float jumpOffsetY = this._beatmapObjectSpawnController.jumpOffsetY;
    this._currentYJumpOffsetBufferEndIndex = (this._currentYJumpOffsetBufferEndIndex + 1) % 2000;
    this._yJumpOffsetBuffer[this._currentYJumpOffsetBufferEndIndex] = new BeatmapObjectAvoidanceYOffsetEvaluator.BufferData(songTime, jumpOffsetY);
  }

  public virtual float GetJumpOffsetYAtJumpStartSongTime(float lastDeltaTime)
  {
    double songTime1 = (double) this._audioTimeSource.songTime;
    lastDeltaTime = Mathf.Max(lastDeltaTime, 1f / 1000f);
    int index1 = (this._currentYJumpOffsetBufferEndIndex - Mathf.Min(Mathf.CeilToInt(1f / lastDeltaTime), 2000) + 2000) % 2000;
    double desiredZposition = (double) this._jumpDurationToDesiredZPosition;
    float num = (float) (songTime1 - desiredZposition);
    float jumpStartSongTime = this._yJumpOffsetBuffer[0].yOffset;
    float songTime2 = this._yJumpOffsetBuffer[this._currentYJumpOffsetBufferEndIndex].songTime;
    if ((double) num < (double) songTime2 && (double) num > -1000.0)
    {
      while ((double) this._yJumpOffsetBuffer[index1].songTime > (double) num)
        index1 = (index1 - 1 + 2000) % 2000;
      while ((double) this._yJumpOffsetBuffer[index1].songTime <= (double) num)
        index1 = (index1 + 1) % 2000;
      int index2 = (index1 - 1 + 2000) % 2000;
      int index3 = (index2 + 1) % 2000;
      BeatmapObjectAvoidanceYOffsetEvaluator.BufferData bufferData1 = this._yJumpOffsetBuffer[index2];
      BeatmapObjectAvoidanceYOffsetEvaluator.BufferData bufferData2 = this._yJumpOffsetBuffer[index3];
      float t = 1f;
      if (!Mathf.Approximately(bufferData1.songTime, bufferData2.songTime))
        t = (float) (((double) num - (double) bufferData1.songTime) / ((double) bufferData2.songTime - (double) bufferData1.songTime));
      jumpStartSongTime = Mathf.LerpUnclamped(bufferData1.yOffset, bufferData2.yOffset, t);
    }
    return jumpStartSongTime;
  }

  public readonly struct BufferData
  {
    public readonly float songTime;
    public readonly float yOffset;

    public BufferData(float songTime, float yOffset)
    {
      this.songTime = songTime;
      this.yOffset = yOffset;
    }
  }
}
