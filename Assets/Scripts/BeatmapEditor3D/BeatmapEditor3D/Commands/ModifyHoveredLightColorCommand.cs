// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.ModifyHoveredLightColorCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class ModifyHoveredLightColorCommand : ModifyHoveredLightColorEventCommand
  {
    [Inject]
    private readonly ModifyHoveredLightColorSignal _signal;

    protected override LightColorBaseEditorData GetModifiedEventData(
      LightColorBaseEditorData originalData)
    {
      return LightColorBaseEditorData.CopyWithModifications(originalData, colorType: new LightColorBaseEditorData.EnvironmentColorType?(this._signal.newColor));
    }

    protected override bool NeedsModification(LightColorBaseEditorData originalData) => originalData.colorType != this._signal.newColor;
  }
}
