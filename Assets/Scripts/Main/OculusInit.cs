// Decompiled with JetBrains decompiler
// Type: OculusInit
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Oculus.Platform;
using Oculus.Platform.Models;
using System;
using UnityEngine;
using Zenject;

public class OculusInit : MonoBehaviour
{
  [InjectOptional]
  protected OculusDeeplinkManager _oculusDeeplinkManager;
  [Inject]
  protected DlcPromoPanelModel _dlcPromoPanelModel;
  public static bool __enabled = true;

  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
  private static void NoDomainReloadInit() => OculusInit.__enabled = true;

  public virtual void Init()
  {
    if (!OculusInit.__enabled)
      return;
    this.TryToInitialize();
  }

  public virtual void TryToInitialize()
  {
    try
    {
      Core.AsyncInitialize().OnComplete(new Message<PlatformInitialize>.Callback(this.InitCallback));
    }
    catch (UnityException ex)
    {
      Debug.LogError((object) "Oculus platform failed to initialize due to exception.");
      Debug.LogException((Exception) ex);
      UnityEngine.Application.Quit();
    }
  }

  public virtual void InitCallback(Message<PlatformInitialize> msg)
  {
    if (msg.IsError)
    {
      Debug.Log((object) ("Oculus PlatformInitialize Error: " + msg.GetError().Message));
      UnityEngine.Application.Quit();
    }
    else
    {
      this._dlcPromoPanelModel.InitAfterPlatformWasInitialized(true);
      this._oculusDeeplinkManager?.OculusPlatformWasInitialized();
      Entitlements.IsUserEntitledToApplication().OnComplete((Message.Callback) (message =>
      {
        if (!message.IsError)
          return;
        Debug.LogWarning((object) ("Oculus user entitlement error: " + message.GetError().Message));
        Debug.LogError((object) "Oculus user entitlement check failed. You are NOT entitled to use this app.");
        UnityEngine.Application.Quit();
      }));
    }
  }
}
