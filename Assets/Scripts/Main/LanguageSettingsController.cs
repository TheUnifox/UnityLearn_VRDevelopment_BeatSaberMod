// Decompiled with JetBrains decompiler
// Type: LanguageSettingsController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Polyglot;
using UnityEngine;

public class LanguageSettingsController : DropdownSettingsController
{
  [SerializeField]
  protected LanguageSO _settingsValue;

  protected override bool GetInitValues(out int idx, out int numberOfElements)
  {
    idx = Localization.Instance.SupportedLanguages.IndexOf(Localization.Instance.SelectedLanguage);
    numberOfElements = Localization.Instance.SupportedLanguages.Count;
    return true;
  }

  protected override void ApplyValue(int idx) => this._settingsValue.value = Localization.Instance.SupportedLanguages[idx];

  protected override string TextForValue(int idx) => Localization.Instance.LocalizedLanguageNames[idx];
}
