// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.StartEventsSelectionSignal
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

namespace BeatmapEditor3D.Commands
{
  public class StartEventsSelectionSignal
  {
    public readonly float beat;
    public readonly BasicBeatmapEventType BasicBeatmapEventType;

    public StartEventsSelectionSignal(float beat, BasicBeatmapEventType BasicBeatmapEventType)
    {
      this.beat = beat;
      this.BasicBeatmapEventType = BasicBeatmapEventType;
    }
  }
}
