// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.LevelEditor.ChangeEventSignal
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;

namespace BeatmapEditor3D.LevelEditor
{
  public class ChangeEventSignal
  {
    public TrackToolbarType group { get; }

    public int value { get; }

    public float floatValue { get; }

    public object payload { get; }

    public ChangeEventSignal(TrackToolbarType group, int value, float floatValue = 1f, object payload = null)
    {
      this.group = group;
      this.value = value;
      this.floatValue = floatValue;
      this.payload = payload;
    }
  }
}
