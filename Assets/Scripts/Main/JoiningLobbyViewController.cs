// Decompiled with JetBrains decompiler
// Type: JoiningLobbyViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class JoiningLobbyViewController : ViewController
{
  [SerializeField]
  protected Button _cancelJoiningButton;
  [SerializeField]
  protected LoadingControl _loadingControl;
  protected string _text;

  public event System.Action didCancelEvent;

  public virtual void Init(string text)
  {
    this._text = text;
    this._cancelJoiningButton.gameObject.SetActive(true);
  }

  public virtual void HideLoading()
  {
    this._loadingControl.Hide();
    this._cancelJoiningButton.gameObject.SetActive(false);
  }

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    this._loadingControl.ShowLoading(this._text);
    if (!firstActivation)
      return;
    this.buttonBinder.AddBinding(this._cancelJoiningButton, (System.Action) (() =>
    {
      System.Action didCancelEvent = this.didCancelEvent;
      if (didCancelEvent == null)
        return;
      didCancelEvent();
    }));
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__8_0()
  {
    System.Action didCancelEvent = this.didCancelEvent;
    if (didCancelEvent == null)
      return;
    didCancelEvent();
  }
}
