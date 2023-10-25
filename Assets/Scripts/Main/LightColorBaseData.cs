// Decompiled with JetBrains decompiler
// Type: LightColorBaseData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public class LightColorBaseData
{
  public readonly float beat;
  public readonly BeatmapEventTransitionType transitionType;
  public readonly EnvironmentColorType colorType;
  public readonly float brightness;
  public readonly int strobeBeatFrequency;

  public LightColorBaseData(
    float beat,
    BeatmapEventTransitionType transitionType,
    EnvironmentColorType colorType,
    float brightness,
    int strobeBeatFrequency)
  {
    this.beat = beat;
    this.transitionType = transitionType;
    this.colorType = colorType;
    this.brightness = brightness;
    this.strobeBeatFrequency = strobeBeatFrequency;
  }
}
