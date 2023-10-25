// Decompiled with JetBrains decompiler
// Type: BeatmapDataNoArrowsTransform
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;

public abstract class BeatmapDataNoArrowsTransform
{
  public static IReadonlyBeatmapData CreateTransformedData(IReadonlyBeatmapData beatmapData)
  {
    return (IReadonlyBeatmapData) beatmapData.GetFilteredCopy(new Func<BeatmapDataItem, BeatmapDataItem>(ProcessData));

    BeatmapDataItem ProcessData(BeatmapDataItem beatmapDataItem)
    {
      switch (beatmapDataItem)
      {
        case NoteData noteData when noteData.cutDirection != NoteCutDirection.None:
          noteData.SetNoteToAnyCutDirection();
          if (noteData.gameplayType == NoteData.GameplayType.BurstSliderHead)
          {
            noteData.ChangeToGameNote();
            break;
          }
          break;
        case SliderData _:
          return (BeatmapDataItem) null;
      }
      return beatmapDataItem;
    }
  }
}
