// Decompiled with JetBrains decompiler
// Type: PresetsSettingsController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Polyglot;
using UnityEngine;

public class PresetsSettingsController : ListSettingsController
{
  [SerializeField]
  protected IntSO _settingsValue;
  [SerializeField]
  protected NamedPresetsSO _presets;
  [SerializeField]
  protected bool _limitNumberOfElements;
  [SerializeField]
  [DrawIf("_limitNumberOfElements", true, DrawIfAttribute.DisablingType.DontDraw)]
  protected int _numberOfElementsLimit;

  protected override bool GetInitValues(out int idx, out int numberOfElements)
  {
    idx = (int) (ObservableVariableSO<int>) this._settingsValue;
    idx = Mathf.Clamp(idx, 0, this._presets.namedPresets.Length - 1);
    numberOfElements = this._presets.namedPresets.Length;
    if (this._limitNumberOfElements)
      numberOfElements = Mathf.Min(this._numberOfElementsLimit, numberOfElements);
    return true;
  }

  protected override void ApplyValue(int idx) => this._settingsValue.value = idx;

  protected override string TextForValue(int idx) => Localization.Get(this._presets.namedPresets[idx].presetNameLocalizationKey);
}
