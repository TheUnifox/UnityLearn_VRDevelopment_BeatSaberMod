// Decompiled with JetBrains decompiler
// Type: GameplayModifiersDropdown
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameplayModifiersDropdown : MonoBehaviour
{
  [SerializeField]
  protected SimpleTextDropdown _simpleTextDropdown;
  [SerializeField]
  protected GameplayModifiersModelSO _gameplayModifiersModel;
  protected IReadOnlyList<Tuple<GameplayModifierMask, string>> _gameplayModifiersData;

  public event System.Action<int> didSelectCellWithIdxEvent;

  private IReadOnlyList<Tuple<GameplayModifierMask, string>> gameplayModifiersData
  {
    get
    {
      if (this._gameplayModifiersData == null)
        this._gameplayModifiersData = (IReadOnlyList<Tuple<GameplayModifierMask, string>>) new List<Tuple<GameplayModifierMask, string>>(((IEnumerable<GameplayModifierMask>) Enum.GetValues(typeof (GameplayModifierMask))).Select<GameplayModifierMask, Tuple<GameplayModifierMask, string>>((Func<GameplayModifierMask, Tuple<GameplayModifierMask, string>>) (value =>
        {
          string str;
          switch (value)
          {
            case GameplayModifierMask.None:
              str = Localization.Get("MODIFIER_NONE");
              break;
            case GameplayModifierMask.All:
              str = Localization.Get("MODIFIER_ALL");
              break;
            default:
              GameplayModifierParamsSO gameplayModifierParams = this._gameplayModifiersModel.GetGameplayModifierParams(value);
              str = (UnityEngine.Object) gameplayModifierParams != (UnityEngine.Object) null ? Localization.Get(gameplayModifierParams.modifierNameLocalizationKey) : string.Empty;
              break;
          }
          return new Tuple<GameplayModifierMask, string>(value, str);
        })));
      return this._gameplayModifiersData;
    }
  }

  public virtual void Start()
  {
    this._simpleTextDropdown.didSelectCellWithIdxEvent += new System.Action<DropdownWithTableView, int>(this.HandleSimpleTextDropdownDidSelectCellWithIdx);
    this._simpleTextDropdown.SetTexts((IReadOnlyList<string>) this.gameplayModifiersData.Select<Tuple<GameplayModifierMask, string>, string>((Func<Tuple<GameplayModifierMask, string>, string>) (x => x.Item2)).ToArray<string>());
  }

  public virtual void OnDestroy()
  {
    if ((UnityEngine.Object) this._simpleTextDropdown == (UnityEngine.Object) null)
      return;
    this._simpleTextDropdown.didSelectCellWithIdxEvent -= new System.Action<DropdownWithTableView, int>(this.HandleSimpleTextDropdownDidSelectCellWithIdx);
  }

  public virtual GameplayModifierMask GetSelectedGameplayModifierMask() => this.gameplayModifiersData[this._simpleTextDropdown.selectedIndex].Item1;

  public virtual void SelectCellWithGameplayModifierMask(GameplayModifierMask gameplayModifierMask) => this._simpleTextDropdown.SelectCellWithIdx(this.GetIdxForGameplayModifierMask(gameplayModifierMask));

  public virtual int GetIdxForGameplayModifierMask(GameplayModifierMask gameplayModifierMask)
  {
    for (int index = 0; index < this.gameplayModifiersData.Count; ++index)
    {
      if (this.gameplayModifiersData[index].Item1 == gameplayModifierMask)
        return index;
    }
    return 0;
  }

  public virtual void HandleSimpleTextDropdownDidSelectCellWithIdx(
    DropdownWithTableView dropdownWithTableView,
    int idx)
  {
    System.Action<int> cellWithIdxEvent = this.didSelectCellWithIdxEvent;
    if (cellWithIdxEvent == null)
      return;
    cellWithIdxEvent(idx);
  }

  [CompilerGenerated]
  public virtual Tuple<GameplayModifierMask, string> m_Cget_gameplayModifiersDatam_Eb__7_0(
    GameplayModifierMask value)
  {
    string str;
    switch (value)
    {
      case GameplayModifierMask.None:
        str = Localization.Get("MODIFIER_NONE");
        break;
      case GameplayModifierMask.All:
        str = Localization.Get("MODIFIER_ALL");
        break;
      default:
        GameplayModifierParamsSO gameplayModifierParams = this._gameplayModifiersModel.GetGameplayModifierParams(value);
        str = (UnityEngine.Object) gameplayModifierParams != (UnityEngine.Object) null ? Localization.Get(gameplayModifierParams.modifierNameLocalizationKey) : string.Empty;
        break;
    }
    return new Tuple<GameplayModifierMask, string>(value, str);
  }
}
