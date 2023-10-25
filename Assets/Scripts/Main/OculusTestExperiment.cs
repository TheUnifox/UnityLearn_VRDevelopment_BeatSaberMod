// Decompiled with JetBrains decompiler
// Type: OculusTestExperiment
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;
using Zenject;

public class OculusTestExperiment
{
  protected const string kIsInTest1Key = "beatsaber_experiments:test_parameter";
  [Inject]
  protected readonly IExperimentModel _experimentModel;

  [Inject]
  public virtual async void Init()
  {
    try
    {
      int num1 = await this._experimentModel.IsEmployee() ? 1 : 0;
      int num2 = await this._experimentModel.IsInTest((IExperimentData) new OculusTestExperiment.ExperimentData("beatsaber_experiments:test_parameter")) ? 1 : 0;
    }
    catch (Exception ex)
    {
      Debug.LogWarning((object) ex.Message);
    }
  }

  public class ExperimentData : IExperimentData
  {
    protected readonly string _experimentPlatformKey;

    public string experimentPlatformKey => this._experimentPlatformKey;

    public ExperimentData(string experimentPlatformKey) => this._experimentPlatformKey = experimentPlatformKey;
  }
}
