// Decompiled with JetBrains decompiler
// Type: EnvironmentOverrideSettingsPanelController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

public class EnvironmentOverrideSettingsPanelController : MonoBehaviour, IRefreshable
{
  [SerializeField]
  protected Toggle _overrideEnvironmentsToggle;
  [SerializeField]
  protected GameObject _elementsGO;
  [SerializeField]
  protected EnvironmentOverrideSettingsPanelController.Elements[] _elements;
  [Space]
  [SerializeField]
  protected PanelAnimationSO _presentPanelAnimation;
  [SerializeField]
  protected PanelAnimationSO _dismissPanelAnimation;
  [Space]
  [SerializeField]
  protected EnvironmentsListSO _allEnvironments;
  [Inject]
  protected readonly IAnalyticsModel _analyticsModel;
  protected OverrideEnvironmentSettings _overrideEnvironmentSettings;
  protected bool _initialized;

  public OverrideEnvironmentSettings overrideEnvironmentSettings => this._overrideEnvironmentSettings;

  public virtual void SetData(
    OverrideEnvironmentSettings overrideEnvironmentSettings)
  {
    if (!this._initialized)
    {
      foreach (EnvironmentOverrideSettingsPanelController.Elements element in this._elements)
      {
        element.environmentInfos = this._allEnvironments.GetAllEnvironmentInfosWithType(element.environmentType);
        element.simpleTextDropdown.SetTexts((IReadOnlyList<string>) element.environmentInfos.Select<EnvironmentInfoSO, string>((Func<EnvironmentInfoSO, string>) (x => x.environmentName)).ToArray<string>());
        element.simpleTextDropdown.didSelectCellWithIdxEvent += new System.Action<DropdownWithTableView, int>(this.HandleDropDownDidSelectCellWithIdx);
      }
      this._overrideEnvironmentsToggle.onValueChanged.AddListener(new UnityAction<bool>(this.HandleOverrideEnvironmentsToggleValueChanged));
      this._initialized = true;
    }
    this._overrideEnvironmentSettings = overrideEnvironmentSettings;
  }

  public virtual void OnDestroy()
  {
    foreach (EnvironmentOverrideSettingsPanelController.Elements element in this._elements)
    {
      if ((UnityEngine.Object) element.simpleTextDropdown != (UnityEngine.Object) null)
        element.simpleTextDropdown.didSelectCellWithIdxEvent -= new System.Action<DropdownWithTableView, int>(this.HandleDropDownDidSelectCellWithIdx);
    }
    this._overrideEnvironmentsToggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.HandleOverrideEnvironmentsToggleValueChanged));
  }

  public virtual void Refresh()
  {
    if (!this._initialized)
      return;
    foreach (EnvironmentOverrideSettingsPanelController.Elements element in this._elements)
    {
      EnvironmentInfoSO environmentInfoForType = this._overrideEnvironmentSettings.GetOverrideEnvironmentInfoForType(element.environmentType);
      int idx = element.environmentInfos.IndexOf(environmentInfoForType);
      element.simpleTextDropdown.SelectCellWithIdx(idx);
      element.label.text = Localization.Get(element.environmentType.typeNameLocalizationKey);
    }
    this._overrideEnvironmentsToggle.isOn = this._overrideEnvironmentSettings.overrideEnvironments;
    this._elementsGO.SetActive(this._overrideEnvironmentSettings.overrideEnvironments);
  }

  public virtual void HandleDropDownDidSelectCellWithIdx(
    DropdownWithTableView dropDownWithTableView,
    int idx)
  {
    foreach (EnvironmentOverrideSettingsPanelController.Elements element in this._elements)
    {
      if ((UnityEngine.Object) element.simpleTextDropdown == (UnityEngine.Object) dropDownWithTableView)
      {
        this._overrideEnvironmentSettings.SetEnvironmentInfoForType(element.environmentType, element.environmentInfos[idx]);
        this._analyticsModel.LogClick("Environments Override DropDown", new Dictionary<string, string>()
        {
          ["environment_type"] = element.environmentType.typeNameLocalizationKey,
          ["environment"] = element.environmentInfos[idx].serializedName
        });
        break;
      }
    }
  }

  public virtual void HandleOverrideEnvironmentsToggleValueChanged(bool isOn)
  {
    this._overrideEnvironmentSettings.overrideEnvironments = isOn;
    if (isOn && !this._elementsGO.activeSelf)
    {
      this._elementsGO.SetActive(true);
      this._presentPanelAnimation.ExecuteAnimation(this._elementsGO);
    }
    else if (!isOn && this._elementsGO.activeSelf)
      this._dismissPanelAnimation.ExecuteAnimation(this._elementsGO, (System.Action) (() => this._elementsGO.SetActive(false)));
    this._analyticsModel.LogClick("Environments Override Toggle", new Dictionary<string, string>()
    {
      ["override_environments"] = this._overrideEnvironmentSettings.overrideEnvironments.ToString()
    });
  }

  [CompilerGenerated]
  public virtual void m_CHandleOverrideEnvironmentsToggleValueChangedm_Eb__16_0() => this._elementsGO.SetActive(false);

  [Serializable]
  public class Elements
  {
    public TextMeshProUGUI label;
    public SimpleTextDropdown simpleTextDropdown;
    public EnvironmentTypeSO environmentType;
    [CompilerGenerated]
    protected List<EnvironmentInfoSO> m_CenvironmentInfos;

    public List<EnvironmentInfoSO> environmentInfos
    {
      get => this.m_CenvironmentInfos;
      set => this.m_CenvironmentInfos = value;
    }
  }
}
