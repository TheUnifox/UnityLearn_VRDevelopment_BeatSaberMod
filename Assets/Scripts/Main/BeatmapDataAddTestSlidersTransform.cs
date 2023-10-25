// Decompiled with JetBrains decompiler
// Type: BeatmapDataAddTestSlidersTransform
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using System.Linq;

public class BeatmapDataAddTestSlidersTransform
{
  protected const float kMaxTimeDiff = 2f;

  public static IReadonlyBeatmapData CreateTransformedData(IReadonlyBeatmapData beatmapData)
  {
    BeatmapData transformedData = new BeatmapData(beatmapData.numberOfLines);
    List<BeatmapDataItem> list = beatmapData.allBeatmapDataItems.ToList<BeatmapDataItem>();
    for (int index = 0; index < list.Count; ++index)
    {
      switch (list[index])
      {
        case BeatmapEventData beatmapEventData:
          transformedData.InsertBeatmapEventData(beatmapEventData);
          break;
        case NoteData noteData:
          if (noteData.colorType != ColorType.None)
          {
            transformedData.AddBeatmapObjectData((BeatmapObjectData) noteData);
            NoteData sameColorTypeNote = BeatmapDataAddTestSlidersTransform.FindNextSameColorTypeNote((IReadOnlyList<BeatmapDataItem>) list, index, noteData.colorType);
            if (sameColorTypeNote != null && (double) sameColorTypeNote.time - (double) noteData.time < 2.0)
            {
              SliderData sliderData = SliderData.CreateSliderData(noteData.colorType, noteData.time, noteData.lineIndex, noteData.noteLineLayer, noteData.beforeJumpNoteLineLayer, 1f, noteData.cutDirection, sameColorTypeNote.time, sameColorTypeNote.lineIndex, sameColorTypeNote.noteLineLayer, sameColorTypeNote.beforeJumpNoteLineLayer, 1f, sameColorTypeNote.cutDirection, SliderMidAnchorMode.Clockwise);
              noteData.ResetNoteFlip();
              sameColorTypeNote.ResetNoteFlip();
              transformedData.AddBeatmapObjectData((BeatmapObjectData) sliderData);
              break;
            }
            break;
          }
          break;
      }
    }
    return (IReadonlyBeatmapData) transformedData;
  }

  private static NoteData FindNextSameColorTypeNote(
    IReadOnlyList<BeatmapDataItem> beatmapDataItems,
    int startIndex,
    ColorType colorType)
  {
    double time = (double) beatmapDataItems[startIndex].time;
    for (int index = startIndex + 1; index < beatmapDataItems.Count; ++index)
    {
      if (beatmapDataItems[index] is NoteData beatmapDataItem && beatmapDataItem.colorType == colorType)
        return beatmapDataItem;
    }
    return (NoteData) null;
  }
}
