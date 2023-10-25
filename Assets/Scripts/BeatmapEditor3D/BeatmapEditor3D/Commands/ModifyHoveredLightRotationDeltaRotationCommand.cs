// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.ModifyHoveredLightRotationDeltaRotationCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class ModifyHoveredLightRotationDeltaRotationCommand : 
    ModifyHoveredLightRotationEventCommand
  {
    private const float kRotationStep = 5f;
    [Inject]
    private readonly ModifyHoveredLightRotationDeltaRotationSignal _signal;

    protected override LightRotationBaseEditorData GetModifiedEventData(
      LightRotationBaseEditorData originalData)
    {
      return LightRotationBaseEditorData.CopyWithModifications(originalData, rotation: new float?(MathfExtra.Mod(MathfExtra.Round(originalData.rotation + Mathf.Sign(this._signal.deltaRotation) * 5f * this.eventBoxGroupsState.scrollPrecision, 2), 360f)));
    }

    protected override bool NeedsModification(LightRotationBaseEditorData originalData) => true;
  }
}
