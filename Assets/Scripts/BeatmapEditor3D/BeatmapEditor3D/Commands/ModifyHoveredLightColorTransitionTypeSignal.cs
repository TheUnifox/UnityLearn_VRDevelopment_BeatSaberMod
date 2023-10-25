// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.ModifyHoveredLightColorTransitionTypeSignal
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

namespace BeatmapEditor3D.Commands
{
  public class ModifyHoveredLightColorTransitionTypeSignal
  {
    public readonly LightColorBaseEditorData.TransitionType transitionType;
    public readonly float brightness;

    public ModifyHoveredLightColorTransitionTypeSignal(
      LightColorBaseEditorData.TransitionType transitionType,
      float brightness)
    {
      this.transitionType = transitionType;
      this.brightness = brightness;
    }
  }
}
