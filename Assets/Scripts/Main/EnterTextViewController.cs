// Decompiled with JetBrains decompiler
// Type: EnterTextViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnterTextViewController : ViewController
{
  [SerializeField]
  protected VRTextEntryController _textEntryController;
  [SerializeField]
  protected TextMeshProUGUI _titleText;
  [SerializeField]
  protected Button _okButton;

  public event System.Action<EnterTextViewController, string> didFinishEvent;

  public virtual void Init(string titleText) => this._titleText.text = titleText;

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (firstActivation)
      this.buttonBinder.AddBinding(this._okButton, new System.Action(this.OkButtonPressed));
    if (!addedToHierarchy)
      return;
    this._textEntryController.text = "";
  }

  public virtual void OkButtonPressed()
  {
    if (this.didFinishEvent == null)
      return;
    string str = this._textEntryController.text;
    if (str.Length == 0)
      str = "PLAYER";
    this.didFinishEvent(this, str);
  }
}
