// Decompiled with JetBrains decompiler
// Type: ConnectionFailedReasonMethods
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public static class ConnectionFailedReasonMethods
{
  [LocalizationKey]
  private const string kConnectionFailedUnknown = "SERVER_CONNECTION_FAILED_TRY_AGAIN";
  [LocalizationKey]
  private const string kConnectionFailedConnectionCanceled = "SERVER_CONNECTION_FAILED_TRY_AGAIN";
  [LocalizationKey]
  private const string kConnectionFailedServerUnreachable = "SERVER_CONNECTION_FAILED_TRY_AGAIN";
  [LocalizationKey]
  private const string kConnectionFailedServerDoesNotExist = "CONNECTION_FAILED_SERVER_DOES_NOT_EXIST";
  [LocalizationKey]
  private const string kConnectionFailedServerAtCapacity = "CONNECTION_FAILED_SERVER_AT_CAPACITY";
  [LocalizationKey]
  private const string kConnectionFailedVersionMismatch = "CONNECTION_FAILED_VERSION_MISMATCH";
  [LocalizationKey]
  private const string kConnectionFailedInvalidPassword = "CONNECTION_FAILED_INVALID_PASSWORD";
  [LocalizationKey]
  private const string kConnectionFailedMasterServerUnreachable = "SERVER_CONNECTION_FAILED_TRY_AGAIN";
  [LocalizationKey]
  private const string kConnectionFailedMasterServerNotAuthenticated = "SERVER_CONNECTION_FAILED_TRY_AGAIN";
  [LocalizationKey]
  private const string kConnectionFailedNetworkNotConnected = "CONNECTION_FAILED_NETWORK_NOT_CONNECTED";
  [LocalizationKey]
  private const string kConnectionFailedMasterServerCertificateValidationFailed = "CONNECTION_FAILED_NETWORK_NOT_CONNECTED";
  [LocalizationKey]
  private const string kConnectionFailedServerIsTerminating = "CONNECTION_FAILED_SERVER_DOES_NOT_EXIST";
  [LocalizationKey]
  private const string kConnectionFailedTimeout = "CONNECTION_FAILED_TIMEOUT";
  [LocalizationKey]
  private const string kConnectionFailedFailedToFindMatch = "CONNECTION_FAILED_FAILED_TO_FIND_MATCH";

  public static string LocalizedKey(this ConnectionFailedReason connectionFailedReason)
  {
    switch (connectionFailedReason)
    {
      case ConnectionFailedReason.Unknown:
        return "SERVER_CONNECTION_FAILED_TRY_AGAIN";
      case ConnectionFailedReason.ConnectionCanceled:
        return "SERVER_CONNECTION_FAILED_TRY_AGAIN";
      case ConnectionFailedReason.ServerUnreachable:
        return "SERVER_CONNECTION_FAILED_TRY_AGAIN";
      case ConnectionFailedReason.ServerDoesNotExist:
        return "CONNECTION_FAILED_SERVER_DOES_NOT_EXIST";
      case ConnectionFailedReason.ServerAtCapacity:
        return "CONNECTION_FAILED_SERVER_AT_CAPACITY";
      case ConnectionFailedReason.VersionMismatch:
        return "CONNECTION_FAILED_VERSION_MISMATCH";
      case ConnectionFailedReason.InvalidPassword:
        return "CONNECTION_FAILED_INVALID_PASSWORD";
      case ConnectionFailedReason.MultiplayerApiUnreachable:
        return "SERVER_CONNECTION_FAILED_TRY_AGAIN";
      case ConnectionFailedReason.AuthenticationFailed:
        return "SERVER_CONNECTION_FAILED_TRY_AGAIN";
      case ConnectionFailedReason.NetworkNotConnected:
        return "CONNECTION_FAILED_NETWORK_NOT_CONNECTED";
      case ConnectionFailedReason.CertificateValidationFailed:
        return "CONNECTION_FAILED_NETWORK_NOT_CONNECTED";
      case ConnectionFailedReason.ServerIsTerminating:
        return "CONNECTION_FAILED_SERVER_DOES_NOT_EXIST";
      case ConnectionFailedReason.Timeout:
        return "CONNECTION_FAILED_TIMEOUT";
      case ConnectionFailedReason.FailedToFindMatch:
        return "CONNECTION_FAILED_FAILED_TO_FIND_MATCH";
      default:
        return "SERVER_CONNECTION_FAILED_TRY_AGAIN";
    }
  }

  public static string ErrorCode(this ConnectionFailedReason connectionFailedReason) => string.Format("CFR-{0}", (object) (int) connectionFailedReason);
}
