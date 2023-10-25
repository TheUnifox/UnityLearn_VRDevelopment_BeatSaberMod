// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.DeleteLightEventSignal
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

namespace BeatmapEditor3D.Commands
{
  public class DeleteLightEventSignal
  {
    public readonly BeatmapEditorObjectId eventBoxId;
    public readonly BeatmapEditorObjectId lightColorEventId;

    public DeleteLightEventSignal(
      BeatmapEditorObjectId eventBoxId,
      BeatmapEditorObjectId lightColorEventId)
    {
      this.eventBoxId = eventBoxId;
      this.lightColorEventId = lightColorEventId;
    }
  }
}
