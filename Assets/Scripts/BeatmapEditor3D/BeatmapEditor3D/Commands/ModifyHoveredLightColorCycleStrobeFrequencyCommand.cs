// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.ModifyHoveredLightColorCycleStrobeFrequencyCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using UnityEngine;

namespace BeatmapEditor3D.Commands
{
  public class ModifyHoveredLightColorCycleStrobeFrequencyCommand : 
    ModifyHoveredLightColorEventCommand
  {
    private const float kMaxStrobePower = 5f;

    protected override LightColorBaseEditorData GetModifiedEventData(
      LightColorBaseEditorData originalData)
    {
      float num = 1f;
      if (originalData.strobeBeatFrequency != 0)
        num = Mathf.Pow(2f, (float) (((double) Mathf.Log((float) Mathf.ClosestPowerOfTwo(originalData.strobeBeatFrequency), 2f) + 1.0) % 5.0));
      return LightColorBaseEditorData.CopyWithModifications(originalData, strobeFrequency: new int?((int) num));
    }

    protected override bool NeedsModification(LightColorBaseEditorData originalData) => true;
  }
}
