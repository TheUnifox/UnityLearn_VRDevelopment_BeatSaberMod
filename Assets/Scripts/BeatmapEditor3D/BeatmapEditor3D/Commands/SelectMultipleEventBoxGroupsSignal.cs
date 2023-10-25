// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.SelectMultipleEventBoxGroupsSignal
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

namespace BeatmapEditor3D.Commands
{
  public class SelectMultipleEventBoxGroupsSignal
  {
    public readonly bool commit;
    public readonly bool addToSelection;

    public SelectMultipleEventBoxGroupsSignal(bool commit, bool addToSelection)
    {
      this.commit = commit;
      this.addToSelection = addToSelection;
    }
  }
}
