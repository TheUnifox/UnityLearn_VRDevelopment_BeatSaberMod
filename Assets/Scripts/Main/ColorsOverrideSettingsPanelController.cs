// Decompiled with JetBrains decompiler
// Type: ColorsOverrideSettingsPanelController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

public class ColorsOverrideSettingsPanelController : MonoBehaviour, IRefreshable
{
  [SerializeField]
  protected Toggle _overrideColorsToggle;
  [SerializeField]
  protected GameObject _detailsPanelGO;
  [SerializeField]
  protected ColorSchemeDropdown _colorSchemeDropDown;
  [SerializeField]
  protected EditColorSchemeController _editColorSchemeController;
  [SerializeField]
  protected ModalView _editColorSchemeModalView;
  [SerializeField]
  protected Button _editColorSchemeButton;
  [Space]
  [SerializeField]
  protected PanelAnimationSO _presentPanelAnimation;
  [SerializeField]
  protected PanelAnimationSO _dismissPanelAnimation;
  [Inject]
  protected readonly IAnalyticsModel _analyticsModel;
  protected ColorSchemesSettings _colorSchemesSettings;
  protected bool _initialized;
  protected ButtonBinder _buttonBinder;

  public ColorSchemesSettings colorSchemesSettings => this._colorSchemesSettings;

  public virtual void SetData(ColorSchemesSettings colorSchemesSettings)
  {
    if (!this._initialized)
    {
      this._buttonBinder = new ButtonBinder();
      this._buttonBinder.AddBinding(this._editColorSchemeButton, new System.Action(this.HandleEditColorSchemeButtonWasPressed));
      this._colorSchemeDropDown.didSelectCellWithIdxEvent += new System.Action<DropdownWithTableView, int>(this.HandleDropDownDidSelectCellWithIdx);
      this._editColorSchemeController.didFinishEvent += new System.Action(this.HandleEditColorSchemeControllerDidFinish);
      this._editColorSchemeController.didChangeColorSchemeEvent += new System.Action<ColorScheme>(this.HandleEditColorSchemeControllerDidChangeColorScheme);
      this._overrideColorsToggle.onValueChanged.AddListener(new UnityAction<bool>(this.HandleOverrideColorsToggleValueChanged));
      this._initialized = true;
    }
    this._colorSchemesSettings = colorSchemesSettings;
  }

  public virtual void OnDestroy()
  {
    if ((UnityEngine.Object) this._colorSchemeDropDown != (UnityEngine.Object) null)
      this._colorSchemeDropDown.didSelectCellWithIdxEvent -= new System.Action<DropdownWithTableView, int>(this.HandleDropDownDidSelectCellWithIdx);
    if ((UnityEngine.Object) this._editColorSchemeController != (UnityEngine.Object) null)
    {
      this._editColorSchemeController.didFinishEvent -= new System.Action(this.HandleEditColorSchemeControllerDidFinish);
      this._editColorSchemeController.didChangeColorSchemeEvent -= new System.Action<ColorScheme>(this.HandleEditColorSchemeControllerDidChangeColorScheme);
    }
    this._overrideColorsToggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.HandleOverrideColorsToggleValueChanged));
    this._buttonBinder.ClearBindings();
  }

  public virtual void OnDisable() => this._editColorSchemeModalView.Hide(false);

  public virtual void Refresh()
  {
    if (!this._initialized)
      return;
    this._overrideColorsToggle.isOn = this._colorSchemesSettings.overrideDefaultColors;
    this._detailsPanelGO.SetActive(this._colorSchemesSettings.overrideDefaultColors);
    int numberOfColorSchemes = this._colorSchemesSettings.GetNumberOfColorSchemes();
    List<ColorScheme> colorSchemes = new List<ColorScheme>(numberOfColorSchemes);
    for (int idx = 0; idx < numberOfColorSchemes; ++idx)
      colorSchemes.Add(this._colorSchemesSettings.GetColorSchemeForIdx(idx));
    ColorScheme selectedColorScheme = this._colorSchemesSettings.GetSelectedColorScheme();
    this._colorSchemeDropDown.SetData((IReadOnlyList<ColorScheme>) colorSchemes);
    this._colorSchemeDropDown.SelectCellWithIdx(this._colorSchemesSettings.GetSelectedColorSchemeIdx());
    this._editColorSchemeButton.interactable = selectedColorScheme.isEditable;
  }

  public virtual void HandleDropDownDidSelectCellWithIdx(
    DropdownWithTableView dropDownWithTableView,
    int idx)
  {
    ColorScheme colorSchemeForIdx = this._colorSchemesSettings.GetColorSchemeForIdx(idx);
    this._colorSchemesSettings.selectedColorSchemeId = colorSchemeForIdx.colorSchemeId;
    this._editColorSchemeButton.interactable = colorSchemeForIdx.isEditable;
    this._analyticsModel.LogClick("Colors Override DropDown", new Dictionary<string, string>()
    {
      ["color_scheme_id"] = colorSchemeForIdx.colorSchemeId
    });
  }

  public virtual void HandleOverrideColorsToggleValueChanged(bool isOn)
  {
    this._colorSchemesSettings.overrideDefaultColors = isOn;
    if (isOn && !this._detailsPanelGO.activeSelf)
    {
      this._detailsPanelGO.SetActive(true);
      this._presentPanelAnimation.ExecuteAnimation(this._detailsPanelGO);
    }
    else if (!isOn && this._detailsPanelGO.activeSelf)
      this._dismissPanelAnimation.ExecuteAnimation(this._detailsPanelGO, (System.Action) (() => this._detailsPanelGO.SetActive(false)));
    this._analyticsModel.LogClick("Colors Override Toggle", new Dictionary<string, string>()
    {
      ["override_default_colors"] = this._colorSchemesSettings.overrideDefaultColors.ToString()
    });
  }

  public virtual void HandleEditColorSchemeButtonWasPressed()
  {
    this._editColorSchemeController.gameObject.SetActive(true);
    this._editColorSchemeController.SetColorScheme(this._colorSchemesSettings.GetSelectedColorScheme());
    this._editColorSchemeModalView.Show(true, true);
  }

  public virtual void HandleEditColorSchemeControllerDidFinish()
  {
    if (this._editColorSchemeController.gameObject.activeSelf)
      this._editColorSchemeModalView.Hide(true);
    this.Refresh();
  }

  public virtual void HandleEditColorSchemeControllerDidChangeColorScheme(ColorScheme colorScheme) => this._colorSchemesSettings.SetColorSchemeForId(colorScheme);

  [CompilerGenerated]
  public virtual void m_CHandleOverrideColorsToggleValueChangedm_Eb__19_0() => this._detailsPanelGO.SetActive(false);
}
