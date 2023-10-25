// Decompiled with JetBrains decompiler
// Type: PatternFightSceneSetupData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Zenject;

[ZenjectAllowDuringValidation]
public class PatternFightSceneSetupData : SceneSetupData
{
  public readonly PlayerSpecificSettings playerSpecificSettings;
  public readonly ColorScheme colorScheme;

  public PatternFightSceneSetupData(
    PlayerSpecificSettings playerSpecificSettings,
    ColorScheme colorScheme)
  {
    this.playerSpecificSettings = playerSpecificSettings;
    this.colorScheme = colorScheme;
  }
}
