// Decompiled with JetBrains decompiler
// Type: ServerCodeView
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ServerCodeView : MonoBehaviour
{
  [SerializeField]
  protected TextMeshProUGUI _serverCodeText;
  [SerializeField]
  protected Button _button;
  protected readonly ButtonBinder _buttonBinder = new ButtonBinder();
  protected string _serverCode;
  protected bool _codeIsShown;

  public virtual void SetCode(string serverCode)
  {
    this._serverCode = serverCode;
    this.RefreshText(false);
  }

  public virtual void OnEnable() => this._buttonBinder.AddBinding(this._button, new System.Action(this.HandleShowServerCodeButtonPressed));

  public virtual void OnDisable() => this._buttonBinder.ClearBindings();

  public virtual void HandleShowServerCodeButtonPressed() => this.RefreshText(!this._codeIsShown);

  public virtual void RefreshText(bool showCode)
  {
    this._codeIsShown = showCode;
    this._serverCodeText.text = showCode ? this._serverCode ?? "" : "*****";
  }
}
