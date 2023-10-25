// Decompiled with JetBrains decompiler
// Type: OculusDeviceConfigExperimentModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

public class OculusDeviceConfigExperimentModel : IExperimentModel
{
  protected const int kInitTimeoutMs = 5000;
  protected const string kIsEmployeeKey = "deviceconfig_main_shared:is_employee";
  protected readonly Task _initializationTask;

  public OculusDeviceConfigExperimentModel() => this._initializationTask = this.Initialize();

  public virtual Task<bool> IsEmployee() => this.GetBooleanAsync("deviceconfig_main_shared:is_employee");

  public virtual Task<bool> IsInTest(IExperimentData data) => this.GetBooleanAsync(data.experimentPlatformKey);

  public virtual async Task Initialize()
  {
    OculusDeviceConfig.Init();
    Stopwatch stopwatch = new Stopwatch();
    stopwatch.Restart();
    while (stopwatch.ElapsedMilliseconds < 5000L)
    {
      switch (OculusDeviceConfig.GetCurrentState())
      {
        case OculusDeviceConfig.State.Prefetched:
        case OculusDeviceConfig.State.Failed:
          throw new Exception(OculusDeviceConfig.GetError());
        default:
          await Task.Delay(11);
          continue;
      }
    }
    throw new TimeoutException("Failed to initialize OculusDeviceConfig");
  }

  public virtual async Task<bool> GetBooleanAsync(string key)
  {
    await this._initializationTask;
    return OculusDeviceConfig.DidPrefetchParamName(key) ? OculusDeviceConfig.GetBoolean(key) : throw new KeyNotFoundException("Failed to Fetch ParamName " + key);
  }
}
