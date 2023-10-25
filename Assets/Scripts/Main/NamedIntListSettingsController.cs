// Decompiled with JetBrains decompiler
// Type: NamedIntListSettingsController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Polyglot;
using System;
using UnityEngine;

public class NamedIntListSettingsController : ListSettingsController
{
  [SerializeField]
  protected IntSO _settingsValue;
  [SerializeField]
  protected NamedIntListSettingsController.TextValuePair[] _textValuePairs;

  protected override bool GetInitValues(out int idx, out int numberOfElements)
  {
    numberOfElements = this._textValuePairs.Length;
    idx = numberOfElements - 1;
    for (int index = 0; index < this._textValuePairs.Length; ++index)
    {
      if ((int) (ObservableVariableSO<int>) this._settingsValue == this._textValuePairs[index].value)
      {
        idx = index;
        return true;
      }
    }
    return true;
  }

  protected override void ApplyValue(int idx) => this._settingsValue.value = this._textValuePairs[idx].value;

  protected override string TextForValue(int idx) => this._textValuePairs[idx].localizedText;

  [Serializable]
  public class TextValuePair
  {
    public string text;
    public int value;

    public string localizedText => Localization.Get(this.text);
  }
}
