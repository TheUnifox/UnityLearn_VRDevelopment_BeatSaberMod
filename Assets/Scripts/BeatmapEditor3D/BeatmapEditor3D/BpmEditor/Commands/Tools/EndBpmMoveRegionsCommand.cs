// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BpmEditor.Commands.Tools.EndBpmMoveRegionsCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

namespace BeatmapEditor3D.BpmEditor.Commands.Tools
{
  public class EndBpmMoveRegionsCommand : UpdateAllRegionsBeatBoundsCommand
  {
    public override bool ShouldMergeWith(
      IBeatmapEditorCommandWithHistoryMergeable previousCommand)
    {
      return previousCommand is BpmMoveRegionsCommand;
    }

    public override void MergeWith(
      IBeatmapEditorCommandWithHistoryMergeable previousCommand)
    {
      BpmMoveRegionsCommand moveRegionsCommand = (BpmMoveRegionsCommand) previousCommand;
      this._oldRegions[moveRegionsCommand.regionAIndex] = moveRegionsCommand.oldLeftRegion;
      this._oldRegions[moveRegionsCommand.regionBIndex] = moveRegionsCommand.oldRightRegion;
    }
  }
}
