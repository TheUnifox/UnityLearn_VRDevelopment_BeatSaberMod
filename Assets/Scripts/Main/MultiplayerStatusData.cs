// Decompiled with JetBrains decompiler
// Type: MultiplayerStatusData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Newtonsoft.Json;
using System;
using UnityEngine.Scripting;

[Preserve]
[Serializable]
public class MultiplayerStatusData
{
  public string minimumAppVersion;
  public MultiplayerStatusData.AvailabilityStatus status;
  public long maintenanceStartTime;
  public long maintenanceEndTime;
  public MultiplayerStatusData.UserMessage userMessage;
  public bool useGamelift;

  [JsonProperty("minimum_app_version")]
  private string _minimumAppVersion
  {
    get => this.minimumAppVersion;
    set => this.minimumAppVersion = value;
  }

  [JsonProperty("maintenance_start_time")]
  private long _maintenanceStartTime
  {
    get => this.maintenanceStartTime;
    set => this.maintenanceStartTime = value;
  }

  [JsonProperty("user_message")]
  private MultiplayerStatusData.UserMessage _userMessage
  {
    get => this.userMessage;
    set => this.userMessage = value;
  }

  [JsonProperty("use_gamelift")]
  private bool _useGamelift
  {
    get => this.useGamelift;
    set => this.useGamelift = value;
  }

  public enum AvailabilityStatus
  {
    Online,
    MaintenanceUpcoming,
    Offline,
  }

  [Serializable]
  public class UserMessage
  {
    public MultiplayerStatusData.UserMessage.LocalizedMessage[] localizations;

    [Serializable]
    public class LocalizedMessage
    {
      public string language;
      public string message;
    }
  }
}
