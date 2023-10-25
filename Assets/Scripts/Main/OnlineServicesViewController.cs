// Decompiled with JetBrains decompiler
// Type: OnlineServicesViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class OnlineServicesViewController : ViewController
{
  [SerializeField]
  protected Button _enableButton;
  [SerializeField]
  protected Button _dontEnableButton;

  public event System.Action<bool> didFinishEvent;

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (!firstActivation)
      return;
    this.buttonBinder.AddBinding(this._enableButton, (System.Action) (() =>
    {
      System.Action<bool> didFinishEvent = this.didFinishEvent;
      if (didFinishEvent == null)
        return;
      didFinishEvent(true);
    }));
    this.buttonBinder.AddBinding(this._dontEnableButton, (System.Action) (() =>
    {
      System.Action<bool> didFinishEvent = this.didFinishEvent;
      if (didFinishEvent == null)
        return;
      didFinishEvent(false);
    }));
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__5_0()
  {
    System.Action<bool> didFinishEvent = this.didFinishEvent;
    if (didFinishEvent == null)
      return;
    didFinishEvent(true);
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__5_1()
  {
    System.Action<bool> didFinishEvent = this.didFinishEvent;
    if (didFinishEvent == null)
      return;
    didFinishEvent(false);
  }
}
