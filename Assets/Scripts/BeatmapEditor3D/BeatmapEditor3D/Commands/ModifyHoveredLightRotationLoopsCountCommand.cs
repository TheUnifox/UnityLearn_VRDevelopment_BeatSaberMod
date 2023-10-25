// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.ModifyHoveredLightRotationLoopsCountCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class ModifyHoveredLightRotationLoopsCountCommand : ModifyHoveredLightRotationEventCommand
  {
    [Inject]
    private readonly ModifyHoveredLightRotationLoopsCountSignal _signal;

    protected override LightRotationBaseEditorData GetModifiedEventData(
      LightRotationBaseEditorData originalData)
    {
      return LightRotationBaseEditorData.CopyWithModifications(originalData, loopsCount: new int?(this._signal.loopsCount));
    }

    protected override bool NeedsModification(LightRotationBaseEditorData originalData) => originalData.loopsCount != this._signal.loopsCount;
  }
}
