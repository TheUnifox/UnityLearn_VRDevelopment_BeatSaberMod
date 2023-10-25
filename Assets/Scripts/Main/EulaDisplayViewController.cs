// Decompiled with JetBrains decompiler
// Type: EulaDisplayViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using UnityEngine;

public class EulaDisplayViewController : ViewController
{
  [Header("Texts")]
  [SerializeField]
  protected TextPageScrollView _textPageScrollView;
  [SerializeField]
  protected LocalizedTextAsset _localizedTextAsset;

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (!firstActivation)
      return;
    this._textPageScrollView.SetText(this._localizedTextAsset.localizedText);
  }
}
