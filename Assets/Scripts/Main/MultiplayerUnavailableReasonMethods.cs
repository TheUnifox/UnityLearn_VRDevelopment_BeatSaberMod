// Decompiled with JetBrains decompiler
// Type: MultiplayerUnavailableReasonMethods
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Polyglot;
using System;
using UnityEngine;

public static class MultiplayerUnavailableReasonMethods
{
  [LocalizationKey]
  private const string kMultiplayerUnavailableServerOffline = "MULTIPLAYER_UNAVAILABLE_SERVER_OFFLINE";
  [LocalizationKey]
  private const string kMultiplayerUnavailableUpdateRequired = "MULTIPLAYER_UNAVAILABLE_UPDATE_REQUIRED";
  [LocalizationKey]
  private const string kMultiplayerUnavailableMaintenanceMode = "MULTIPLAYER_UNAVAILABLE_MAINTENANCE_MODE";
  [LocalizationKey]
  private const string kMultiplayerUnavailableTryAgain = "MULTIPLAYER_UNAVAILABLE_TRY_AGAIN";

  public static string LocalizedKey(
    this MultiplayerUnavailableReason multiplayerUnavailableReason)
  {
    switch (multiplayerUnavailableReason)
    {
      case MultiplayerUnavailableReason.UpdateRequired:
        return "MULTIPLAYER_UNAVAILABLE_UPDATE_REQUIRED";
      case MultiplayerUnavailableReason.ServerOffline:
        return "MULTIPLAYER_UNAVAILABLE_SERVER_OFFLINE";
      case MultiplayerUnavailableReason.MaintenanceMode:
        return "MULTIPLAYER_UNAVAILABLE_MAINTENANCE_MODE";
      default:
        return "MULTIPLAYER_UNAVAILABLE_TRY_AGAIN";
    }
  }

  public static string ErrorCode(
    this MultiplayerUnavailableReason multiplayerUnavailableReason)
  {
    return string.Format("MUR-{0}", (object) (int) multiplayerUnavailableReason);
  }

  public static bool TryGetMultiplayerUnavailableReason(
    MultiplayerStatusData data,
    out MultiplayerUnavailableReason reason)
  {
    reason = (MultiplayerUnavailableReason) 0;
    if (data == null)
    {
      reason = MultiplayerUnavailableReason.NetworkUnreachable;
      return true;
    }
    if (data.status == MultiplayerStatusData.AvailabilityStatus.Offline)
    {
      reason = MultiplayerUnavailableReason.ServerOffline;
      return true;
    }
    if (MultiplayerUnavailableReasonMethods.VersionLessThan(Application.version, data.minimumAppVersion))
    {
      reason = MultiplayerUnavailableReason.UpdateRequired;
      return true;
    }
    if (data.status != MultiplayerStatusData.AvailabilityStatus.MaintenanceUpcoming || data.maintenanceStartTime >= DateTime.UtcNow.ToUnixTime() || DateTime.UtcNow.ToUnixTime() >= data.maintenanceEndTime)
      return false;
    reason = MultiplayerUnavailableReason.MaintenanceMode;
    return true;
  }

  public static string GetLocalizedMessage(this MultiplayerStatusData data, Language language)
  {
    if (data?.userMessage?.localizations == null)
      return (string) null;
    string localizedMessage = (string) null;
    for (int index = 0; index < data.userMessage.localizations.Length; ++index)
    {
      MultiplayerStatusData.UserMessage.LocalizedMessage localization = data.userMessage.localizations[index];
      if (localization.language.ToLanguage() == language)
        return localization.message;
      if (localization.language.ToLanguage() == Language.English)
        localizedMessage = localization.message;
    }
    return localizedMessage;
  }

  private static bool VersionLessThan(string currentVersion, string minVersion)
  {
    if (string.IsNullOrEmpty(minVersion))
      return false;
    string[] strArray1 = currentVersion.Split('_')[0].Split('b');
    string[] strArray2 = minVersion.Split('b');
    string[] strArray3 = strArray1[0].Split('.');
    string[] strArray4 = strArray2[0].Split('.');
    for (int index = 0; index < strArray3.Length || index < strArray4.Length; ++index)
    {
      int result1 = 0;
      if (index < strArray3.Length && !int.TryParse(strArray3[index], out result1))
        return true;
      int result2 = 0;
      if (index < strArray4.Length && !int.TryParse(strArray4[index], out result2) || result1 < result2)
        return true;
      if (result2 < result1)
        return false;
    }
    int result3 = int.MaxValue;
    if (strArray1.Length > 1 && !int.TryParse(strArray1[1], out result3))
      return true;
    int result4 = int.MaxValue;
    return strArray2.Length > 1 && !int.TryParse(strArray2[1], out result4) || result3 < result4;
  }
}
