// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.OpenTemporaryProjectViewController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BeatmapEditor3D
{
  public class OpenTemporaryProjectViewController : BeatmapEditorViewController
  {
    [SerializeField]
    private TextMeshProUGUI _lastOpenedDateTimeText;
    [SerializeField]
    private TextMeshProUGUI _createdDateTimeText;
    [SerializeField]
    private Button _yesButton;
    [SerializeField]
    private Button _noButton;
    private Action<bool> _buttonAction;

    public void Init(
      DateTime lastOpenedDateTime,
      DateTime createdDateTime,
      Action<bool> buttonAction)
    {
      this._lastOpenedDateTimeText.text = string.Format("{0:yy MM. dd. HH:mm:ss}", (object) lastOpenedDateTime);
      this._createdDateTimeText.text = string.Format("{0:yy MM. dd. HH:mm:ss}", (object) createdDateTime);
      this._buttonAction = buttonAction;
    }

    protected override void DidActivate(
      bool firstActivation,
      bool addedToHierarchy,
      bool screenSystemEnabling)
    {
      if (!firstActivation)
        return;
      this.buttonBinder.AddBinding(this._yesButton, new Action(this.HandleYesButtonClicked));
      this.buttonBinder.AddBinding(this._noButton, new Action(this.HandleNoButtonClicked));
    }

    protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
    {
      if (!removedFromHierarchy)
        return;
      this._buttonAction = (Action<bool>) null;
    }

    private void HandleYesButtonClicked()
    {
      Action<bool> buttonAction = this._buttonAction;
      if (buttonAction == null)
        return;
      buttonAction(true);
    }

    private void HandleNoButtonClicked()
    {
      Action<bool> buttonAction = this._buttonAction;
      if (buttonAction == null)
        return;
      buttonAction(false);
    }
  }
}
