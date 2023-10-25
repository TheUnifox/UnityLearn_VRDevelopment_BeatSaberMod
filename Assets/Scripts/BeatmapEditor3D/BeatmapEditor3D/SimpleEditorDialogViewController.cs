// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.SimpleEditorDialogViewController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BeatmapEditor3D
{
  public class SimpleEditorDialogViewController : BeatmapEditorViewController
  {
    [SerializeField]
    private TextMeshProUGUI _titleText;
    [SerializeField]
    private TextMeshProUGUI _messageText;
    [SerializeField]
    private Button[] _buttons;
    [SerializeField]
    private TextMeshProUGUI[] _buttonTexts;
    private Action<int> _didFinishAction;

    public void Init(string title, string message, string buttonText, Action<int> didFinishAction) => this.Init(title, message, buttonText, (string) null, (string) null, didFinishAction);

    public void Init(
      string title,
      string message,
      string firstButtonText,
      string secondButtonText,
      Action<int> didFinishAction)
    {
      this.Init(title, message, firstButtonText, secondButtonText, (string) null, didFinishAction);
    }

    public void Init(
      string title,
      string message,
      string firstButtonText,
      string secondButtonText,
      string thirdButtonText,
      Action<int> didFinishAction)
    {
      this._didFinishAction = didFinishAction;
      this._titleText.text = title;
      this._messageText.text = message;
      if (this._buttonTexts.Length != 0 && this._buttons.Length != 0)
      {
        this._buttonTexts[0].text = firstButtonText;
        this._buttons[0].gameObject.SetActive(!string.IsNullOrEmpty(firstButtonText));
      }
      if (this._buttonTexts.Length > 1 && this._buttons.Length > 1)
      {
        this._buttonTexts[1].text = secondButtonText;
        this._buttons[1].gameObject.SetActive(!string.IsNullOrEmpty(secondButtonText));
      }
      if (this._buttonTexts.Length <= 2 || this._buttons.Length <= 2)
        return;
      this._buttonTexts[2].text = thirdButtonText;
      this._buttons[2].gameObject.SetActive(!string.IsNullOrEmpty(thirdButtonText));
    }

    protected override void DidActivate(
      bool firstActivation,
      bool addedToHierarchy,
      bool screenSystemEnabling)
    {
      if (!firstActivation)
        return;
      this.buttonBinder.AddBindings(((IEnumerable<Button>) this._buttons).Select<Button, Tuple<Button, Action>>((Func<Button, int, Tuple<Button, Action>>) ((button, i) => new Tuple<Button, Action>(button, (Action) (() =>
      {
        Action<int> didFinishAction = this._didFinishAction;
        if (didFinishAction == null)
          return;
        didFinishAction(i);
      })))).ToList<Tuple<Button, Action>>());
    }

    protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
    {
      if (!removedFromHierarchy)
        return;
      this._didFinishAction = (Action<int>) null;
    }
  }
}
