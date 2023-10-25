// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.ModifyHoveredLightTranslationTranslationCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class ModifyHoveredLightTranslationTranslationCommand : 
    ModifyHoveredLightTranslationEventCommand
  {
    [Inject]
    private readonly ModifyHoveredLightTranslationTranslationSignal _signal;

    protected override LightTranslationBaseEditorData GetModifiedEventData(
      LightTranslationBaseEditorData originalData)
    {
      return LightTranslationBaseEditorData.CopyWithModifications(originalData, translation: new float?(this._signal.translation));
    }

    protected override bool NeedsModification(LightTranslationBaseEditorData originalData) => !Mathf.Approximately(originalData.translation, this._signal.translation);
  }
}
