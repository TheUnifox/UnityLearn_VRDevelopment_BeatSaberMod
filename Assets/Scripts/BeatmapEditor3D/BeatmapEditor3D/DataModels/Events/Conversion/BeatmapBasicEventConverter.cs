// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.Events.Conversion.BeatmapBasicEventConverter
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

namespace BeatmapEditor3D.DataModels.Events.Conversion
{
  public static class BeatmapBasicEventConverter
  {
    public static BeatmapEventData ConvertBasicEvent(
      BasicEventEditorData e,
      IBeatToTimeConvertor timeConvertor)
    {
      return e.type == BasicBeatmapEventType.Event5 ? (BeatmapEventData) new ColorBoostBeatmapEventData(timeConvertor.ConvertBeatToTime(e.beat), e.value == 1) : (BeatmapEventData) new BasicBeatmapEventData(timeConvertor.ConvertBeatToTime(e.beat), e.type, e.value, e.floatValue);
    }
  }
}
