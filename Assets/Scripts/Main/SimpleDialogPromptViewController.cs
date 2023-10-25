// Decompiled with JetBrains decompiler
// Type: SimpleDialogPromptViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SimpleDialogPromptViewController : ViewController
{
  [SerializeField]
  protected TextMeshProUGUI _titleText;
  [SerializeField]
  protected TextMeshProUGUI _messageText;
  [SerializeField]
  protected Button[] _buttons;
  [SerializeField]
  protected TextMeshProUGUI[] _buttonTexts;
  protected System.Action<int> _didFinishAction;

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (!firstActivation)
      return;
    for (int index = 0; index < this._buttons.Length; ++index)
    {
      Button button = this._buttons[index];
      int buttonNum = index;
      this.buttonBinder.AddBinding(button, (System.Action) (() =>
      {
        System.Action<int> didFinishAction = this._didFinishAction;
        if (didFinishAction == null)
          return;
        didFinishAction(buttonNum);
      }));
    }
  }

  protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
  {
    if (!removedFromHierarchy)
      return;
    this._didFinishAction = (System.Action<int>) null;
  }

  public virtual void Init(
    string title,
    string message,
    string buttonText,
    System.Action<int> didFinishAction)
  {
    this.Init(title, message, buttonText, (string) null, (string) null, didFinishAction);
  }

  public virtual void Init(
    string title,
    string message,
    string firstButtonText,
    string secondButtonText,
    System.Action<int> didFinishAction)
  {
    this.Init(title, message, firstButtonText, secondButtonText, (string) null, didFinishAction);
  }

  public virtual void Init(
    string title,
    string message,
    string firstButtonText,
    string secondButtonText,
    string thirdButtonText,
    System.Action<int> didFinishAction)
  {
    this._didFinishAction = didFinishAction;
    this._titleText.text = title;
    this._messageText.SetText(message);
    this._buttonTexts[0].text = firstButtonText;
    this._buttons[0].gameObject.SetActive(!string.IsNullOrEmpty(firstButtonText));
    this._buttonTexts[1].text = secondButtonText;
    this._buttons[1].gameObject.SetActive(!string.IsNullOrEmpty(secondButtonText));
    this._buttonTexts[2].text = thirdButtonText;
    this._buttons[2].gameObject.SetActive(!string.IsNullOrEmpty(thirdButtonText));
  }
}
