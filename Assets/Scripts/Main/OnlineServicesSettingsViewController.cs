// Decompiled with JetBrains decompiler
// Type: OnlineServicesSettingsViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using UnityEngine;
using UnityEngine.UI;

public class OnlineServicesSettingsViewController : ViewController
{
  [SerializeField]
  protected BoolSO _onlineServicesEnabled;
  [Space]
  [SerializeField]
  protected Toggle _enableOnlineServicesToggle;
  protected ToggleBinder _toggleBinder;

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (firstActivation)
    {
      this._toggleBinder = new ToggleBinder();
      this._toggleBinder.AddBinding(this._enableOnlineServicesToggle, new System.Action<bool>(this.HandleEnableOnlineServicesToggleValueChanged));
    }
    this._enableOnlineServicesToggle.isOn = (bool) (ObservableVariableSO<bool>) this._onlineServicesEnabled;
  }

  public virtual void HandleEnableOnlineServicesToggleValueChanged(bool value)
  {
  }
}
