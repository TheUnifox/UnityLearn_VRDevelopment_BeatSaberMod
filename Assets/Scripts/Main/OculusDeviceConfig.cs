// Decompiled with JetBrains decompiler
// Type: OculusDeviceConfig
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public abstract class OculusDeviceConfig
{
  public static void Init()
  {
  }

  public static OculusDeviceConfig.State GetCurrentState() => OculusDeviceConfig.State.Failed;

  public static bool DidPrefetchParamName(string key) => false;

  public static string GetError() => "Cannot Use DeviceConfig on Non-Quest Platforms";

  public static bool GetBoolean(string key) => false;

  public static long GetLong(string key) => 0;

  public static double GetDouble(string key) => 0.0;

  public static string GetString(string key) => (string) null;

  public enum State
  {
    Uninitialized,
    Initialized,
    Prefetched,
    Failed,
  }
}
