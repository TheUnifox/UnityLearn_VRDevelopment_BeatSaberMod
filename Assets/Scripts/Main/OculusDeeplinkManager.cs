// Decompiled with JetBrains decompiler
// Type: OculusDeeplinkManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Oculus.Platform;
using Oculus.Platform.Models;
using System;
using System.Diagnostics;
using UnityEngine;
using Zenject;

public class OculusDeeplinkManager : IDeeplinkManager
{
  protected Deeplink _currentDeeplink;
  protected bool _oculusPlatformWasInitialized;

  public event System.Action<Deeplink> didReceiveDeeplinkEvent;

  public Deeplink currentDeeplink => this._currentDeeplink;

  [Inject]
  public virtual void Init() => GroupPresence.SetJoinIntentReceivedNotificationCallback(new Message<GroupPresenceJoinIntent>.Callback(this.SetJoinIntentReceivedNotificationCallback));

  public virtual void OculusPlatformWasInitialized()
  {
    if (this._oculusPlatformWasInitialized)
      return;
    this._oculusPlatformWasInitialized = true;
  }

  public virtual void SetJoinIntentReceivedNotificationCallback(
    Message<GroupPresenceJoinIntent> message)
  {
    if (message.IsError)
      OculusDeeplinkManager.Log("[DeeplinkManager] Received Error: " + message.GetError().Message);
    else
      this.UpdateDeeplinkMessage(message.Data, ApplicationLifecycle.GetLaunchDetails());
  }

  public virtual void UpdateDeeplinkMessage(
    GroupPresenceJoinIntent joinIntent,
    LaunchDetails launchDetails)
  {
    Deeplink deeplink = (Deeplink) null;
    if (string.IsNullOrEmpty(joinIntent.DeeplinkMessage))
      return;
    try
    {
      OculusDeeplinkManager.Log("[DeeplinkManager] Received Deeplink: " + joinIntent.DeeplinkMessage);
      try
      {
        deeplink = JsonUtility.FromJson<Deeplink>(joinIntent.DeeplinkMessage);
      }
      catch (Exception ex)
      {
        OculusDeeplinkManager.Log(string.Format("[DeeplinkManager] Exception in Deeplink json loading:\n{0}", (object) ex));
      }
      if (deeplink == null || !this.IsAtLeastOneFieldPopulated(deeplink))
      {
        this._currentDeeplink = (Deeplink) null;
      }
      else
      {
        if (launchDetails.RoomID != 0UL)
          deeplink.MultiplayerRoomId = launchDetails.RoomID;
        if (!string.IsNullOrWhiteSpace(joinIntent.LobbySessionId))
          deeplink.MultiplayerSecret = joinIntent.LobbySessionId;
        this._currentDeeplink = deeplink;
        System.Action<Deeplink> receiveDeeplinkEvent = this.didReceiveDeeplinkEvent;
        if (receiveDeeplinkEvent == null)
          return;
        receiveDeeplinkEvent(this._currentDeeplink);
      }
    }
    catch (Exception ex)
    {
      OculusDeeplinkManager.Log("[DeeplinkManager] Failed To parse deeplink: " + ex.ToString());
    }
  }

  public virtual bool IsAtLeastOneFieldPopulated(Deeplink deeplink) => !string.IsNullOrEmpty(deeplink.Destination) || !string.IsNullOrEmpty(deeplink.LevelID) || !string.IsNullOrEmpty(deeplink.PackID) || !string.IsNullOrEmpty(deeplink.Difficulty) || !string.IsNullOrEmpty(deeplink.Characteristic);

  [Conditional("OculusDeeplinkManagerLogging")]
  public static void Log(string message) => UnityEngine.Debug.Log((object) message);
}
