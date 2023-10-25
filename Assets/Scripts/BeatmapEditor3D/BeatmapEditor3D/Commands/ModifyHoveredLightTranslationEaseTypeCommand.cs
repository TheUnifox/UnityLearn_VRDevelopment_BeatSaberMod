// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.ModifyHoveredLightTranslationEaseTypeCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class ModifyHoveredLightTranslationEaseTypeCommand : 
    ModifyHoveredLightTranslationEventCommand
  {
    [Inject]
    private readonly ModifyHoveredLightTranslationEaseTypeSignal _signal;

    protected override LightTranslationBaseEditorData GetModifiedEventData(
      LightTranslationBaseEditorData originalData)
    {
      return LightTranslationBaseEditorData.CopyWithModifications(originalData, easeType: new EaseType?(this._signal.easeType));
    }

    protected override bool NeedsModification(LightTranslationBaseEditorData originalData) => originalData.easeType != this._signal.easeType;
  }
}
