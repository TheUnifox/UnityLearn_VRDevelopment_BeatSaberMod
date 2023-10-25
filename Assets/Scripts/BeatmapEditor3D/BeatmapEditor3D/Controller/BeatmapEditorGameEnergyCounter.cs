// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Controller.BeatmapEditorGameEnergyCounter
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;

namespace BeatmapEditor3D.Controller
{
  public class BeatmapEditorGameEnergyCounter : IGameEnergyCounter
  {
    public event Action didInitEvent;

    public event Action gameEnergyDidReach0Event;

    public event Action<float> gameEnergyDidChangeEvent;

    public bool isInitialized => true;

    public float energy => 1f;

    public int batteryEnergy => 1;

    public int batteryLives => 3;

    public GameplayModifiers.EnergyType energyType => GameplayModifiers.EnergyType.Battery;

    public bool noFail => false;

    public bool instaFail => false;

    public bool failOnSaberClash => false;
  }
}
