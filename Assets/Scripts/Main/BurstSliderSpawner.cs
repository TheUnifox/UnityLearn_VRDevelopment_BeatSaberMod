// Decompiled with JetBrains decompiler
// Type: BurstSliderSpawner
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;

public abstract class BurstSliderSpawner
{
  public static void BezierCurve(
    Vector2 p0,
    Vector2 p1,
    Vector2 p2,
    float t,
    out Vector2 pos,
    out Vector2 tangent)
  {
    float num = 1f - t;
    pos = num * num * p0 + 2f * num * t * p1 + t * t * p2;
    tangent = (float) (2.0 * (1.0 - (double) t)) * (p1 - p0) + 2f * t * (p2 - p1);
  }

  public static void ProcessSliderData(
    SliderData sliderData,
    in BeatmapObjectSpawnMovementData.SliderSpawnData sliderSpawnData,
    float rotation,
    bool forceIsFirstNote,
    BurstSliderSpawner.ProcessNoteDataDelegate processNoteData)
  {
    float num1 = sliderSpawnData.jumpDuration * 0.5f;
    float time = sliderData.time;
    Vector3 headMoveStartPos = sliderSpawnData.headMoveStartPos;
    Vector3 headJumpStartPos = sliderSpawnData.headJumpStartPos;
    Vector3 headJumpEndPos = sliderSpawnData.headJumpEndPos;
    float headJumpGravity = sliderSpawnData.headJumpGravity;
    Vector3 vector3_1 = headJumpStartPos;
    vector3_1.y += (float) ((double) headJumpGravity * (double) num1 * (double) num1 * 0.5);
    float tailTime = sliderData.tailTime;
    Vector3 tailJumpStartPos = sliderSpawnData.tailJumpStartPos;
    float tailJumpGravity = sliderSpawnData.tailJumpGravity;
    Vector3 vector3_2 = tailJumpStartPos;
    vector3_2.y += (float) ((double) tailJumpGravity * (double) num1 * (double) num1 * 0.5);
    Vector2 p2 = new Vector2(vector3_2.x - vector3_1.x, vector3_2.y - vector3_1.y);
    float magnitude = p2.magnitude;
    float f = (float) (((double) sliderData.headCutDirection.RotationAngle() - 90.0 + (double) sliderData.headCutDirectionAngleOffset) * (Math.PI / 180.0));
    Vector3 p1 = (Vector3) (0.5f * magnitude * new Vector2(Mathf.Cos(f), Mathf.Sin(f)));
    int sliceCount = sliderData.sliceCount;
    float squishAmount = sliderData.squishAmount;
    float num2 = 0.5f * (tailTime - time);
    for (int index = 1; index < sliceCount; ++index)
    {
      float t = (float) index / (float) (sliceCount - 1);
      int lineIndex = index < sliceCount - 1 ? sliderData.headLineIndex : sliderData.tailLineIndex;
      NoteLineLayer noteLineLayer = index < sliceCount - 1 ? sliderData.headLineLayer : sliderData.tailLineLayer;
      NoteData burstSliderNoteData = NoteData.CreateBurstSliderNoteData(Mathf.LerpUnclamped(time, tailTime, t), lineIndex, noteLineLayer, sliderData.headBeforeJumpLineLayer, sliderData.colorType, NoteCutDirection.Any, 1f);
      burstSliderNoteData.timeToPrevColorNote = t * num2;
      Vector2 pos;
      Vector2 tangent;
      BurstSliderSpawner.BezierCurve(new Vector2(0.0f, 0.0f), (Vector2) p1, p2, t * squishAmount, out pos, out tangent);
      burstSliderNoteData.SetCutDirectionAngleOffset(Vector2.SignedAngle(new Vector2(0.0f, -1f), tangent));
      burstSliderNoteData.timeToNextColorNote = index == sliceCount - 1 ? 1f : 0.4f;
      BeatmapObjectSpawnMovementData.NoteSpawnData noteSpawnData = new BeatmapObjectSpawnMovementData.NoteSpawnData(new Vector3(headMoveStartPos.x + pos.x, headMoveStartPos.y, headMoveStartPos.z), new Vector3(headJumpStartPos.x + pos.x, headJumpStartPos.y, headJumpStartPos.z), new Vector3(headJumpEndPos.x + pos.x, headJumpEndPos.y, headJumpEndPos.z), (float) (2.0 * ((double) vector3_1.y + (double) pos.y - (double) headJumpStartPos.y) / ((double) num1 * (double) num1)), sliderSpawnData.moveDuration, sliderSpawnData.jumpDuration);
      processNoteData(burstSliderNoteData, in noteSpawnData, rotation, forceIsFirstNote);
    }
  }

  public delegate void ProcessNoteDataDelegate(
    NoteData noteData,
    in BeatmapObjectSpawnMovementData.NoteSpawnData noteSpawnData,
    float rotation,
    bool forceIsFirstNote);
}
