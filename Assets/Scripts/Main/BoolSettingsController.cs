// Decompiled with JetBrains decompiler
// Type: BoolSettingsController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class BoolSettingsController : SwitchSettingsController
{
  [SerializeField]
  protected BoolSO _settingsValue;

  protected override bool GetInitValue() => (bool) (ObservableVariableSO<bool>) this._settingsValue;

  protected override void ApplyValue(bool value) => this._settingsValue.value = value;
}
