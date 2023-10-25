// Decompiled with JetBrains decompiler
// Type: ReleaseInfoViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using UnityEngine;

public class ReleaseInfoViewController : ViewController
{
  [SerializeField]
  protected MainSettingsModelSO _mainSettingsModel;
  [SerializeField]
  protected TextPageScrollView _textPageScrollView;
  [SerializeField]
  protected TextAsset _releaseNotesTextAsset;
  [SerializeField]
  protected TextAsset _firstTextAsset;

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (!firstActivation)
      return;
    if (this._mainSettingsModel.playingForTheFirstTime)
      this._textPageScrollView.SetText(this._firstTextAsset.text);
    else
      this._textPageScrollView.SetText(this._releaseNotesTextAsset.text);
  }
}
