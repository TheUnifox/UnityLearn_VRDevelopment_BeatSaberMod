// Decompiled with JetBrains decompiler
// Type: MockBeatmapDataConverter
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Linq;
using UnityEngine;

public static class MockBeatmapDataConverter
{
  public static MockBeatmapData ToMockBeatmapData(this IReadonlyBeatmapData beatmapData)
  {
    MockBeatmapData mockBeatmapData1 = new MockBeatmapData();
    mockBeatmapData1.numberOfLines = beatmapData.numberOfLines;
    MockBeatmapData mockBeatmapData2 = mockBeatmapData1;
    float[] numArray = new float[1];
    BeatmapDataItem beatmapDataItem = beatmapData.allBeatmapDataItems.LastOrDefault<BeatmapDataItem>();
    numArray[0] = (float) (beatmapDataItem != null ? (double) beatmapDataItem.time : 0.0);
    double num = (double) Mathf.Max(numArray);
    mockBeatmapData2.songEndTime = (float) num;
    mockBeatmapData1.leftNotes = beatmapData.GetBeatmapDataItems<NoteData>(0).Where<NoteData>((Func<NoteData, bool>) (nd => nd.colorType == ColorType.ColorA)).Select<NoteData, MockNoteData>((Func<NoteData, MockNoteData>) (nd => nd.ToMockNoteData())).ToArray<MockNoteData>();
    mockBeatmapData1.rightNotes = beatmapData.GetBeatmapDataItems<NoteData>(0).Where<NoteData>((Func<NoteData, bool>) (nd => nd.colorType == ColorType.ColorB)).Select<NoteData, MockNoteData>((Func<NoteData, MockNoteData>) (nd => nd.ToMockNoteData())).ToArray<MockNoteData>();
    mockBeatmapData1.bombNotes = beatmapData.GetBeatmapDataItems<NoteData>(0).Where<NoteData>((Func<NoteData, bool>) (nd => nd.colorType == ColorType.None)).Select<NoteData, MockNoteData>((Func<NoteData, MockNoteData>) (nd => nd.ToMockNoteData())).ToArray<MockNoteData>();
    mockBeatmapData1.obstacles = beatmapData.GetBeatmapDataItems<ObstacleData>(0).Select<ObstacleData, MockObstacleData>((Func<ObstacleData, MockObstacleData>) (od => od.ToMockObstacleData())).ToArray<MockObstacleData>();
    return mockBeatmapData1;
  }

  public static MockNoteData ToMockNoteData(this NoteData noteData) => new MockNoteData()
  {
    time = noteData.time,
    colorType = noteData.colorType,
    cutDirection = noteData.cutDirection,
    lineIndex = noteData.lineIndex,
    noteLineLayer = noteData.noteLineLayer
  };

  public static MockObstacleData ToMockObstacleData(this ObstacleData obstacleData) => new MockObstacleData()
  {
    time = obstacleData.time,
    duration = obstacleData.duration,
    lineIndex = obstacleData.lineIndex,
    width = obstacleData.width,
    lineLayer = obstacleData.lineLayer
  };
}
