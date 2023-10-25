// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.BrightnessDistributionView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BeatmapEditor3D.Views
{
  public class BrightnessDistributionView : MonoBehaviour
  {
    [SerializeField]
    private BeatmapEditorToggleGroupView _brightnessDistributionTypeToggle;
    [SerializeField]
    private FloatInputFieldValidator _brightnessDistributionParamInput;
    [SerializeField]
    private Toggle _brightnessAffectFirstEventToggle;
    [SerializeField]
    private SimpleTextEditorDropdownView _brightnessDistributionEaseTypeDropdown;

    public BeatmapEventDataBox.DistributionParamType distributionParamType => (BeatmapEventDataBox.DistributionParamType) (this._brightnessDistributionTypeToggle.value + 1);

    public float distributionParam => MathfExtra.Round(this._brightnessDistributionParamInput.value / 100f, 2);

    public bool affectFirst => this._brightnessAffectFirstEventToggle.isOn;

    public EaseType easeType => EventBoxView.availableEasings[this._brightnessDistributionEaseTypeDropdown.selectedIndex];

    public event Action distributionChangedEvent;

    public void Initialize()
    {
      this._brightnessDistributionTypeToggle.onValueChanged += new Action<int>(this.HandleToggleGroupValueChanged);
      this._brightnessDistributionParamInput.onInputValidated += new Action<float>(this.HandleInputOnValueChanged);
      this._brightnessAffectFirstEventToggle.onValueChanged.AddListener(new UnityAction<bool>(this.HandleToggleValueChanged));
      this._brightnessDistributionEaseTypeDropdown.didSelectCellWithIdxEvent += new Action<DropdownEditorView, int>(this.HandleDropdownValueChanged);
      this._brightnessDistributionEaseTypeDropdown.SetTexts((IReadOnlyList<string>) EventBoxView.availableEasingsNames);
    }

    public void SetData(LightColorEventBoxEditorData lightColorEventBox)
    {
      this._brightnessDistributionTypeToggle.SetValueWithoutNotify((int) (lightColorEventBox.brightnessDistributionParamType - 1));
      this._brightnessDistributionParamInput.value = (float) Mathf.RoundToInt(lightColorEventBox.brightnessDistributionParam * 100f);
      this._brightnessAffectFirstEventToggle.SetIsOnWithoutNotify(lightColorEventBox.brightnessDistributionShouldAffectFirstBaseEvent);
      this._brightnessDistributionEaseTypeDropdown.SelectCellWithIdx(((IReadOnlyList<EaseType>) EventBoxView.availableEasings).IndexOf<EaseType>(lightColorEventBox.brightnessDistributionEaseType));
    }

    private void HandleToggleGroupValueChanged(int _) => this.TriggerDidChange();

    private void HandleInputOnValueChanged(float _) => this.TriggerDidChange();

    private void HandleToggleValueChanged(bool _) => this.TriggerDidChange();

    private void HandleDropdownValueChanged(DropdownEditorView _, int idx) => this.TriggerDidChange();

    private void TriggerDidChange()
    {
      Action distributionChangedEvent = this.distributionChangedEvent;
      if (distributionChangedEvent == null)
        return;
      distributionChangedEvent();
    }
  }
}
