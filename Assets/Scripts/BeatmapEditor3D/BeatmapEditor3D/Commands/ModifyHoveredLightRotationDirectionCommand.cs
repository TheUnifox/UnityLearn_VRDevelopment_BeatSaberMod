// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.ModifyHoveredLightRotationDirectionCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class ModifyHoveredLightRotationDirectionCommand : ModifyHoveredLightRotationEventCommand
  {
    [Inject]
    private readonly ModifyHoveredLightRotationDirectionSignal _signal;

    protected override LightRotationBaseEditorData GetModifiedEventData(
      LightRotationBaseEditorData originalData)
    {
      return LightRotationBaseEditorData.CopyWithModifications(originalData, rotationDirection: new LightRotationDirection?(this._signal.rotationDirection));
    }

    protected override bool NeedsModification(LightRotationBaseEditorData originalData) => originalData.rotationDirection != this._signal.rotationDirection;
  }
}
