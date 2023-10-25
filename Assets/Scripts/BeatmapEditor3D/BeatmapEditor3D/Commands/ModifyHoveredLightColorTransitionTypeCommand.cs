// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.ModifyHoveredLightColorTransitionTypeCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class ModifyHoveredLightColorTransitionTypeCommand : ModifyHoveredLightColorEventCommand
  {
    [Inject]
    private readonly ModifyHoveredLightColorTransitionTypeSignal _signal;

    protected override LightColorBaseEditorData GetModifiedEventData(
      LightColorBaseEditorData originalData)
    {
      return LightColorBaseEditorData.CopyWithModifications(originalData, brightness: new float?(this._signal.brightness), transitionType: new LightColorBaseEditorData.TransitionType?(this._signal.transitionType));
    }

    protected override bool NeedsModification(LightColorBaseEditorData originalData) => originalData.transitionType != this._signal.transitionType || !Mathf.Approximately(originalData.brightness, this._signal.brightness);
  }
}
