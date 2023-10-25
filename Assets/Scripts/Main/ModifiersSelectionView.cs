// Decompiled with JetBrains decompiler
// Type: ModifiersSelectionView
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ModifiersSelectionView : MonoBehaviour
{
  [SerializeField]
  protected GameplayModifierInfoListItemsList _modifierInfoList;
  [SerializeField]
  protected TextMeshProUGUI _noModifiersText;
  [Space]
  [SerializeField]
  protected GameplayModifiersModelSO _gameplayModifiersModel;

  public virtual void SetGameplayModifiers(GameplayModifiers gameplayModifiers)
  {
    if (gameplayModifiers == null)
    {
      this._modifierInfoList.SetData(0, (UIItemsList<GameplayModifierInfoListItem>.DataCallback) null);
      this._noModifiersText.enabled = true;
    }
    else
    {
      List<GameplayModifierParamsSO> modifierParams = this._gameplayModifiersModel.CreateModifierParamsList(gameplayModifiers);
      this._noModifiersText.enabled = modifierParams.Count == 0;
      this._modifierInfoList.SetData(modifierParams.Count, (UIItemsList<GameplayModifierInfoListItem>.DataCallback) ((idx, item) => item.SetModifier(modifierParams[idx])));
    }
  }
}
