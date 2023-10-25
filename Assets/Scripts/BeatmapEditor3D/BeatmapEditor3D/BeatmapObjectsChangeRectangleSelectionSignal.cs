// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapObjectsChangeRectangleSelectionSignal
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

namespace BeatmapEditor3D
{
  public class BeatmapObjectsChangeRectangleSelectionSignal
  {
    public readonly float beat;
    public readonly BeatmapObjectsChangeRectangleSelectionSignal.ChangeType changeType;

    public BeatmapObjectsChangeRectangleSelectionSignal(
      float beat,
      BeatmapObjectsChangeRectangleSelectionSignal.ChangeType changeType)
    {
      this.beat = beat;
      this.changeType = changeType;
    }

    public enum ChangeType
    {
      Start,
      Drag,
      End,
    }
  }
}
