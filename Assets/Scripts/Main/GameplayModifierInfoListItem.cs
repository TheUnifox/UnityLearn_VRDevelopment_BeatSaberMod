// Decompiled with JetBrains decompiler
// Type: GameplayModifierInfoListItem
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using UnityEngine;
using UnityEngine.UI;

public class GameplayModifierInfoListItem : MonoBehaviour
{
  [SerializeField]
  protected HoverHint _hoverHint;
  [SerializeField]
  protected Image _iconImage;

  public virtual void SetModifier(GameplayModifierParamsSO modifierParam, bool showName = false)
  {
    this._iconImage.sprite = modifierParam.icon;
    HoverHint hoverHint = this._hoverHint;
    string str;
    if (!showName)
      str = Localization.GetFormat(modifierParam.descriptionLocalizationKey, (object) (float) ((double) Mathf.Clamp(modifierParam.multiplier, -1f, 1f) * 100.0)) ?? "";
    else
      str = Localization.Get(modifierParam.modifierNameLocalizationKey) + " - " + Localization.GetFormat(modifierParam.descriptionLocalizationKey, (object) (float) ((double) Mathf.Clamp(modifierParam.multiplier, -1f, 1f) * 100.0));
    hoverHint.text = str;
  }
}
