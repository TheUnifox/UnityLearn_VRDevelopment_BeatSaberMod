// Decompiled with JetBrains decompiler
// Type: IGameEnergyCounter
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public interface IGameEnergyCounter
{
  event System.Action didInitEvent;

  event System.Action gameEnergyDidReach0Event;

  event System.Action<float> gameEnergyDidChangeEvent;

  bool isInitialized { get; }

  float energy { get; }

  int batteryEnergy { get; }

  int batteryLives { get; }

  GameplayModifiers.EnergyType energyType { get; }

  bool noFail { get; }

  bool instaFail { get; }

  bool failOnSaberClash { get; }
}
