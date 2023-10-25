// Decompiled with JetBrains decompiler
// Type: SwitchSettingsController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class SwitchSettingsController : MonoBehaviour
{
  [SerializeField]
  private Toggle _toggle;
  private bool _on;

  protected abstract bool GetInitValue();

  protected abstract void ApplyValue(bool value);

  protected void Awake() => this._toggle.onValueChanged.AddListener(new UnityAction<bool>(this.HandleToggleValueDidChange));

  protected void OnDestroy() => this._toggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.HandleToggleValueDidChange));

  protected void OnEnable()
  {
    this._on = this.GetInitValue();
    this.RefreshUI();
    this.ApplyValue(this._on);
  }

  private void RefreshUI() => this._toggle.isOn = this._on;

  private void HandleToggleValueDidChange(bool value) => this.ApplyValue(value);
}
