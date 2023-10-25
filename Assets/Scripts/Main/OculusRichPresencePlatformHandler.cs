// Decompiled with JetBrains decompiler
// Type: OculusRichPresencePlatformHandler
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Oculus.Platform;

public class OculusRichPresencePlatformHandler : IRichPresencePlatformHandler
{
  public virtual void SetPresence(IRichPresenceData richPresenceData)
  {
    GroupPresenceOptions groupPresenceOptions = new GroupPresenceOptions();
    groupPresenceOptions.SetDestinationApiName(richPresenceData.apiName);
    if (richPresenceData is IMultiplayerRichPresenceData richPresenceData1)
    {
      groupPresenceOptions.SetIsJoinable(richPresenceData1.isJoinable);
      if (!string.IsNullOrEmpty(richPresenceData1.multiplayerSecret))
        groupPresenceOptions.SetLobbySessionId(richPresenceData1.multiplayerSecret);
    }
    else
      groupPresenceOptions.SetIsJoinable(false);
    GroupPresence.Set(groupPresenceOptions);
  }

  public virtual void Clear() => RichPresence.Clear();
}
