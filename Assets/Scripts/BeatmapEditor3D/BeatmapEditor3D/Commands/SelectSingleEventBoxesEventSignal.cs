// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.SelectSingleEventBoxesEventSignal
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

namespace BeatmapEditor3D.Commands
{
  public class SelectSingleEventBoxesEventSignal
  {
    public readonly BeatmapEditorObjectId eventBoxId;
    public readonly BeatmapEditorObjectId eventId;
    public readonly bool addToSelection;

    public SelectSingleEventBoxesEventSignal(
      BeatmapEditorObjectId eventBoxId,
      BeatmapEditorObjectId eventId,
      bool addToSelection)
    {
      this.eventBoxId = eventBoxId;
      this.eventId = eventId;
      this.addToSelection = addToSelection;
    }
  }
}
