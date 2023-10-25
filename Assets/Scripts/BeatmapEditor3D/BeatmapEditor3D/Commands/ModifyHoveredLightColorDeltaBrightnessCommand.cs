// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.ModifyHoveredLightColorDeltaBrightnessCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class ModifyHoveredLightColorDeltaBrightnessCommand : ModifyHoveredLightColorEventCommand
  {
    private const float kBrightnessStep = 0.01f;
    [Inject]
    private readonly ModifyHoveredLightColorDeltaBrightnessSignal _signal;

    protected override LightColorBaseEditorData GetModifiedEventData(
      LightColorBaseEditorData originalData)
    {
      return LightColorBaseEditorData.CopyWithModifications(originalData, brightness: new float?(Mathf.Round(Mathf.Max(0.0f, originalData.brightness + Mathf.Sign(this._signal.deltaBrightness) * 0.01f) * 100f) / 100f));
    }

    protected override bool NeedsModification(LightColorBaseEditorData originalData) => true;
  }
}
