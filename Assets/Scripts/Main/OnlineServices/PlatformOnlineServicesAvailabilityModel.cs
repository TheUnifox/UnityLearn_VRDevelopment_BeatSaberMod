// Decompiled with JetBrains decompiler
// Type: OnlineServices.PlatformOnlineServicesAvailabilityModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineServices
{
  public class PlatformOnlineServicesAvailabilityModel
  {
    public event Action<PlatformServicesAvailabilityInfo> platformServicesAvailabilityInfoChangedEvent;

    public virtual async Task<PlatformServicesAvailabilityInfo> GetPlatformServicesAvailabilityInfo(
      CancellationToken cancellationToken)
    {
      return await Task.FromResult<PlatformServicesAvailabilityInfo>(PlatformServicesAvailabilityInfo.everythingOK);
    }
  }
}
