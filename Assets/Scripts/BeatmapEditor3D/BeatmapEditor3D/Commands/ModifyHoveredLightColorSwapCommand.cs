// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.ModifyHoveredLightColorSwapCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

namespace BeatmapEditor3D.Commands
{
  public class ModifyHoveredLightColorSwapCommand : ModifyHoveredLightColorEventCommand
  {
    protected override LightColorBaseEditorData GetModifiedEventData(
      LightColorBaseEditorData originalData)
    {
      LightColorBaseEditorData.EnvironmentColorType environmentColorType = LightColorBaseEditorData.EnvironmentColorType.Color0;
      switch (originalData.colorType)
      {
        case LightColorBaseEditorData.EnvironmentColorType.Color0:
          environmentColorType = LightColorBaseEditorData.EnvironmentColorType.Color1;
          break;
        case LightColorBaseEditorData.EnvironmentColorType.Color1:
          environmentColorType = LightColorBaseEditorData.EnvironmentColorType.ColorW;
          break;
        case LightColorBaseEditorData.EnvironmentColorType.ColorW:
          environmentColorType = LightColorBaseEditorData.EnvironmentColorType.Color0;
          break;
      }
      return LightColorBaseEditorData.CopyWithModifications(originalData, colorType: new LightColorBaseEditorData.EnvironmentColorType?(environmentColorType));
    }

    protected override bool NeedsModification(LightColorBaseEditorData originalData) => true;
  }
}
