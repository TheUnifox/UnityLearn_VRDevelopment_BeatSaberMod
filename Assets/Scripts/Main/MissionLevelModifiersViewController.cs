// Decompiled with JetBrains decompiler
// Type: MissionLevelModifiersViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MissionLevelModifiersViewController : ViewController
{
  [SerializeField]
  protected GameplayModifiersModelSO _gameplayModifiersModel;
  [Space]
  [SerializeField]
  protected GameplayModifierInfoListItemsList _gameplayModifierInfoListItemsList;
  [SerializeField]
  protected GameObject _modifiersPanel;
  [SerializeField]
  protected TextMeshProUGUI _titleText;
  protected GameplayModifiers _gameplayModifiers;

  public virtual void Setup(GameplayModifiers gameplayModifiers)
  {
    this._gameplayModifiers = gameplayModifiers;
    if (!this.isInViewControllerHierarchy)
      return;
    this.RefreshContent();
  }

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    this.RefreshContent();
  }

  public virtual void RefreshContent()
  {
    List<GameplayModifierParamsSO> modifierParamsList = this._gameplayModifiersModel.CreateModifierParamsList(this._gameplayModifiers);
    this._gameplayModifierInfoListItemsList.SetData(modifierParamsList.Count, (UIItemsList<GameplayModifierInfoListItem>.DataCallback) ((idx, gameplayModifierInfoListItem) => gameplayModifierInfoListItem.SetModifier(modifierParamsList[idx], true)));
    if (modifierParamsList.Count == 0)
    {
      this._titleText.text = "No modifiers are active in this mission.";
      this._modifiersPanel.SetActive(false);
    }
    else
    {
      this._titleText.text = "These modifiers will affect the gemaplay of selected mission.";
      this._modifiersPanel.SetActive(true);
    }
  }
}
