// Decompiled with JetBrains decompiler
// Type: DisconnectedReasonMethods
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public static class DisconnectedReasonMethods
{
  [LocalizationKey]
  private const string kDisconnectedUnknown = "DISCONNECTED_UNKNOWN";
  [LocalizationKey]
  private const string kDisconnectedUserInitiated = "DISCONNECTED_USER_INITIATED";
  [LocalizationKey]
  private const string kDisconnectedTimeout = "DISCONNECTED_TIMEOUT";
  [LocalizationKey]
  private const string kDisconnectedKicked = "DISCONNECTED_KICKED";
  [LocalizationKey]
  private const string kDisconnectedServerAtCapacity = "DISCONNECTED_SERVER_AT_CAPACITY";
  [LocalizationKey]
  private const string kDisconnectedServerConnectionClosed = "DISCONNECTED_SERVER_SHUT_DOWN";
  [LocalizationKey]
  private const string kDisconnectedMasterServerUnreachable = "DISCONNECTED_MASTER_SERVER_UNREACHABLE";
  [LocalizationKey]
  private const string kDisconnectedServerTerminated = "DISCONNECTED_SERVER_SHUT_DOWN";

  public static string LocalizedKey(this DisconnectedReason connectionFailedReason)
  {
    switch (connectionFailedReason)
    {
      case DisconnectedReason.Unknown:
        return "DISCONNECTED_UNKNOWN";
      case DisconnectedReason.UserInitiated:
        return "DISCONNECTED_USER_INITIATED";
      case DisconnectedReason.Timeout:
        return "DISCONNECTED_TIMEOUT";
      case DisconnectedReason.Kicked:
        return "DISCONNECTED_KICKED";
      case DisconnectedReason.ServerAtCapacity:
        return "DISCONNECTED_SERVER_AT_CAPACITY";
      case DisconnectedReason.ServerConnectionClosed:
        return "DISCONNECTED_SERVER_SHUT_DOWN";
      case DisconnectedReason.MasterServerUnreachable:
        return "DISCONNECTED_MASTER_SERVER_UNREACHABLE";
      case DisconnectedReason.ServerTerminated:
        return "DISCONNECTED_SERVER_SHUT_DOWN";
      default:
        return "DISCONNECTED_UNKNOWN";
    }
  }

  public static string ErrorCode(this DisconnectedReason disconnectedReason) => string.Format("DCR-{0}", (object) (int) disconnectedReason);
}
