// Decompiled with JetBrains decompiler
// Type: SelectRegionViewController
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
using UnityEngine.UI;

public class SelectRegionViewController : ViewController
{
  [SerializeField]
  protected Button _continueButton;
  [SerializeField]
  protected SimpleTextDropdown _regionSelectionDropdown;
  [Header("Regions")]
  [SerializeField]
  protected SelectRegionViewController.RegionToLocalizationKeyPair[] _regionLocalizationKeys;

  public event System.Action<SelectRegionViewController.Region> didPressContinueButtonEvent;

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (firstActivation)
    {
      this.buttonBinder.AddBinding(this._continueButton, (System.Action) (() =>
      {
        System.Action<SelectRegionViewController.Region> continueButtonEvent = this.didPressContinueButtonEvent;
        if (continueButtonEvent == null)
          return;
        continueButtonEvent(this._regionLocalizationKeys[this._regionSelectionDropdown.selectedIndex].region);
      }));
      this._regionSelectionDropdown.SetTexts((IReadOnlyList<string>) ((IEnumerable<SelectRegionViewController.RegionToLocalizationKeyPair>) this._regionLocalizationKeys).Select<SelectRegionViewController.RegionToLocalizationKeyPair, string>((Func<SelectRegionViewController.RegionToLocalizationKeyPair, string>) (p => Localization.Get(p.localizationKey))).ToList<string>());
      this._regionSelectionDropdown.didSelectCellWithIdxEvent += new System.Action<DropdownWithTableView, int>(this.HandleRegionSelectionDropdownDidSelectCell);
    }
    this._continueButton.interactable = false;
    this._regionSelectionDropdown.SelectCellWithIdx(0);
  }

  protected override void OnDestroy()
  {
    base.OnDestroy();
    if (!((UnityEngine.Object) this._regionSelectionDropdown != (UnityEngine.Object) null))
      return;
    this._regionSelectionDropdown.didSelectCellWithIdxEvent -= new System.Action<DropdownWithTableView, int>(this.HandleRegionSelectionDropdownDidSelectCell);
  }

  public virtual void HandleRegionSelectionDropdownDidSelectCell(
    DropdownWithTableView dropdown,
    int idx)
  {
    this._continueButton.interactable = this._regionLocalizationKeys[idx].region != 0;
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__8_0()
  {
    System.Action<SelectRegionViewController.Region> continueButtonEvent = this.didPressContinueButtonEvent;
    if (continueButtonEvent == null)
      return;
    continueButtonEvent(this._regionLocalizationKeys[this._regionSelectionDropdown.selectedIndex].region);
  }

  public enum Region
  {
    None,
    NorthAndSouthAmerica,
    Europe,
    SouthKorea,
    Japan,
    Other,
  }

  [Serializable]
  public struct RegionToLocalizationKeyPair
  {
    public SelectRegionViewController.Region region;
    public string localizationKey;

    public RegionToLocalizationKeyPair(
      SelectRegionViewController.Region region,
      string localizationKey)
    {
      this.region = region;
      this.localizationKey = localizationKey;
    }
  }
}
