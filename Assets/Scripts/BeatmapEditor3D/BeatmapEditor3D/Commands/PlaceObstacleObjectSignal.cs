// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.PlaceObstacleObjectSignal
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

namespace BeatmapEditor3D.Commands
{
  public class PlaceObstacleObjectSignal
  {
    public readonly float beat;
    public readonly int x;
    public readonly int y;
    public readonly int width;
    public readonly int height;

    public PlaceObstacleObjectSignal(float beat, int x, int y, int width, int height)
    {
      this.beat = beat;
      this.x = x;
      this.y = y;
      this.width = width;
      this.height = height;
    }
  }
}
