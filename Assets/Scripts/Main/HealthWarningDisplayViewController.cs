// Decompiled with JetBrains decompiler
// Type: HealthWarningDisplayViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using TMPro;
using UnityEngine;

public class HealthWarningDisplayViewController : ViewController
{
  [Header("Texts")]
  [SerializeField]
  protected TextMeshProUGUI _healthAndSafetyTextMesh;
  [SerializeField]
  [LocalizationKey]
  protected string _healthAndSafetyFullLocalizationKey;
  protected bool _showShortHealthAndSafety;

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (!firstActivation)
      return;
    this._healthAndSafetyTextMesh.SetText(Localization.Get(this._healthAndSafetyFullLocalizationKey));
  }
}
