// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.EndBeatmapObjectsSelectionSignal
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

namespace BeatmapEditor3D.Commands
{
  public class EndBeatmapObjectsSelectionSignal
  {
    public readonly float beat;
    public readonly bool commit;

    public EndBeatmapObjectsSelectionSignal(float beat, bool commit)
    {
      this.beat = beat;
      this.commit = commit;
    }
  }
}
