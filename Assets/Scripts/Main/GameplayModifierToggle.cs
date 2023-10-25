// Decompiled with JetBrains decompiler
// Type: GameplayModifierToggle
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayModifierToggle : MonoBehaviour
{
  [SerializeField]
  protected GameplayModifierParamsSO _gameplayModifier;
  [SerializeField]
  protected TextMeshProUGUI _nameText;
  [SerializeField]
  protected TextMeshProUGUI _multiplierText;
  [SerializeField]
  protected HoverTextSetter _hoverTextSetter;
  [SerializeField]
  protected Image _icon;
  [SerializeField]
  protected Toggle _toggle;
  [Space]
  [SerializeField]
  protected Color _positiveColor;

  public Toggle toggle => this._toggle;

  public GameplayModifierParamsSO gameplayModifier => this._gameplayModifier;

  public virtual void Start()
  {
    float num = Mathf.Clamp(this._gameplayModifier.multiplier, -1f, 1f);
    bool flag = (double) num > 0.0;
    string str = flag ? string.Format((IFormatProvider) Localization.Instance.SelectedCultureInfo, "+{0:P0}", (object) num) : string.Format((IFormatProvider) Localization.Instance.SelectedCultureInfo, "{0:P0}", (object) num);
    if (this._gameplayModifier.multiplierConditionallyValid)
      str = string.Format((IFormatProvider) Localization.Instance.SelectedCultureInfo, "+{0:P0} / {1}", (object) 0, (object) str);
    if (this._gameplayModifier.isInBeta)
      this._nameText.text = Localization.Get(this._gameplayModifier.modifierNameLocalizationKey) + " (beta)";
    else
      this._nameText.text = Localization.Get(this._gameplayModifier.modifierNameLocalizationKey);
    this._multiplierText.gameObject.SetActive(!Mathf.Approximately(num, 0.0f));
    this._multiplierText.text = str;
    this._multiplierText.color = flag ? this._positiveColor : this._multiplierText.color;
    this._hoverTextSetter.text = Localization.GetFormat(this._gameplayModifier.descriptionLocalizationKey, (object) Mathf.Abs(num));
    this._icon.sprite = this._gameplayModifier.icon;
  }
}
