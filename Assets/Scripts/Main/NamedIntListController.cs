// Decompiled with JetBrains decompiler
// Type: NamedIntListController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using System;
using UnityEngine;

public class NamedIntListController : ListSettingsController, IValueChanger<int>
{
  [SerializeField]
  protected NamedIntListController.TextValuePair[] _textValuePairs;
  [SerializeField]
  protected int _value;

  public event System.Action<int> valueChangedEvent;

  public virtual void InitValues(NamedIntListController.TextValuePair[] values)
  {
    this._textValuePairs = values;
    this.Refresh(false);
  }

  public virtual void SetValue(int value)
  {
    this._value = value;
    this.Refresh(false);
  }

  protected override bool GetInitValues(out int idx, out int numberOfElements)
  {
    numberOfElements = this._textValuePairs.Length;
    if (numberOfElements == 0)
    {
      idx = 0;
      return false;
    }
    idx = numberOfElements - 1;
    for (int index = 0; index < this._textValuePairs.Length; ++index)
    {
      if (this._value == this._textValuePairs[index].value)
      {
        idx = index;
        return true;
      }
    }
    return true;
  }

  protected override void ApplyValue(int idx)
  {
    this._value = idx;
    System.Action<int> valueChangedEvent = this.valueChangedEvent;
    if (valueChangedEvent == null)
      return;
    valueChangedEvent(idx);
  }

  protected override string TextForValue(int idx) => this._textValuePairs[idx].localizedText;

  [Serializable]
  public class TextValuePair
  {
    public string localizationKey;
    public int value;

    public string localizedText => Localization.Get(this.localizationKey);
  }
}
