// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.ChangeLightColorEventSignal
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

namespace BeatmapEditor3D.Commands
{
  public class ChangeLightColorEventSignal
  {
    public readonly LightColorBaseEditorData.EnvironmentColorType colorType;
    public readonly LightColorBaseEditorData.TransitionType transitionType;
    public readonly float brightness;
    public readonly int strobeBeatFrequency;
    public readonly bool extension;

    public ChangeLightColorEventSignal(
      LightColorBaseEditorData.EnvironmentColorType colorType,
      LightColorBaseEditorData.TransitionType transitionType,
      float brightness,
      int strobeBeatFrequency,
      bool extension)
    {
      this.colorType = colorType;
      this.transitionType = transitionType;
      this.brightness = brightness;
      this.strobeBeatFrequency = strobeBeatFrequency;
      this.extension = extension;
    }
  }
}
