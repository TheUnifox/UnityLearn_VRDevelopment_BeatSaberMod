// Decompiled with JetBrains decompiler
// Type: InputFieldSettingsController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using UnityEngine;
using UnityEngine.Events;

public class InputFieldSettingsController : MonoBehaviour
{
  [SerializeField]
  protected InputFieldView _inputFieldView;
  [SerializeField]
  protected StringSO _settingsValue;

  public virtual void Awake() => this._inputFieldView.onValueChanged.AddListener(new UnityAction<InputFieldView>(this.HandleInputFieldDidChange));

  public virtual void OnDestroy() => this._inputFieldView.onValueChanged.RemoveListener(new UnityAction<InputFieldView>(this.HandleInputFieldDidChange));

  public virtual void OnEnable() => this._inputFieldView.SetText(this._settingsValue.value);

  protected virtual void HandleInputFieldDidChange(InputFieldView inputFieldView) => this._settingsValue.value = inputFieldView.text;
}
