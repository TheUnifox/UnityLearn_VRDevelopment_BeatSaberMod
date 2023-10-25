// Decompiled with JetBrains decompiler
// Type: OnlineServices.PlatformServicesAvailabilityInfo
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Polyglot;

namespace OnlineServices
{
  public class PlatformServicesAvailabilityInfo
  {
    public readonly PlatformServicesAvailabilityInfo.OnlineServicesAvailability availability;
    public readonly string localizedMessage;

    private PlatformServicesAvailabilityInfo(
      PlatformServicesAvailabilityInfo.OnlineServicesAvailability availability,
      string localizedMessage)
    {
      this.availability = availability;
      this.localizedMessage = localizedMessage;
    }

    public static PlatformServicesAvailabilityInfo everythingOK => new PlatformServicesAvailabilityInfo(PlatformServicesAvailabilityInfo.OnlineServicesAvailability.Available, (string) null);

    public static PlatformServicesAvailabilityInfo onlineServicesUnavailableError => new PlatformServicesAvailabilityInfo(PlatformServicesAvailabilityInfo.OnlineServicesAvailability.Unavailable, Localization.Get("LEADERBOARDS_PLATFORM_SERVICES_ERROR"));

    public enum OnlineServicesAvailability
    {
      Available,
      Unavailable,
    }
  }
}
