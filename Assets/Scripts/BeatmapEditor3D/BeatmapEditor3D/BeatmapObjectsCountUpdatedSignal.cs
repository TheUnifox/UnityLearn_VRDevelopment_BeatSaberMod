// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapObjectsCountUpdatedSignal
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

namespace BeatmapEditor3D
{
  public class BeatmapObjectsCountUpdatedSignal
  {
    public readonly int notesCount;
    public readonly int bombsCount;
    public readonly int obstaclesCount;
    public readonly int chainsCount;
    public readonly int arcsCount;
    public readonly int eventsCount;
    public readonly int groupsCount;
    public readonly int groupsEventsCount;

    public BeatmapObjectsCountUpdatedSignal(
      int notesCount,
      int bombsCount,
      int obstaclesCount,
      int chainsCount,
      int arcsCount,
      int eventsCount,
      int groupsCount,
      int groupsEventsCount)
    {
      this.notesCount = notesCount;
      this.bombsCount = bombsCount;
      this.obstaclesCount = obstaclesCount;
      this.chainsCount = chainsCount;
      this.arcsCount = arcsCount;
      this.eventsCount = eventsCount;
      this.groupsCount = groupsCount;
      this.groupsEventsCount = groupsEventsCount;
    }
  }
}
