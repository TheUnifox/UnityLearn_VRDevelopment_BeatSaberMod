// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.GapDistributionView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BeatmapEditor3D.Views
{
  public class GapDistributionView : MonoBehaviour
  {
    [SerializeField]
    private BeatmapEditorToggleGroupView _translationAxisToggleGroupView;
    [SerializeField]
    private BeatmapEditorToggleGroupView _translationDistributionTypeToggleGroupView;
    [SerializeField]
    private Toggle _flipTranslationToggle;
    [SerializeField]
    private FloatInputFieldValidator _translationDistributionParamInput;
    [SerializeField]
    private Toggle _translationAffectFirstEventToggle;
    [SerializeField]
    private SimpleTextEditorDropdownView _translationDistributionEaseTypeDropdown;

    public BeatmapEventDataBox.DistributionParamType distributionParamType => (BeatmapEventDataBox.DistributionParamType) (this._translationDistributionTypeToggleGroupView.value + 1);

    public LightAxis axis => (LightAxis) this._translationAxisToggleGroupView.value;

    public float distributionParam => MathfExtra.Round(this._translationDistributionParamInput.value / 100f, 2);

    public bool affectFirst => this._translationAffectFirstEventToggle.isOn;

    public bool flip => this._flipTranslationToggle.isOn;

    public EaseType easeType => EventBoxView.availableEasings[this._translationDistributionEaseTypeDropdown.selectedIndex];

    public event Action distributionChangedEvent;

    public void Initialize()
    {
      this._translationAxisToggleGroupView.onValueChanged += new Action<int>(this.HandleToggleGroupValueChanged);
      this._translationDistributionTypeToggleGroupView.onValueChanged += new Action<int>(this.HandleToggleGroupValueChanged);
      this._translationDistributionParamInput.onInputValidated += new Action<float>(this.HandleInputOnValueChanged);
      this._translationAffectFirstEventToggle.onValueChanged.AddListener(new UnityAction<bool>(this.HandleToggleValueChanged));
      this._flipTranslationToggle.onValueChanged.AddListener(new UnityAction<bool>(this.HandleToggleValueChanged));
      this._translationDistributionEaseTypeDropdown.didSelectCellWithIdxEvent += new Action<DropdownEditorView, int>(this.HandleDropdownValueChanged);
      this._translationDistributionEaseTypeDropdown.SetTexts((IReadOnlyList<string>) EventBoxView.availableEasingsNames);
    }

    public void SetData(
      LightTranslationEventBoxEditorData lightTranslationEventBox,
      EnvironmentTracksDefinitionSO.EventBoxGroupTrackInfo groupTrackInfo)
    {
      this._translationAxisToggleGroupView.SetInteractable(0, groupTrackInfo.showTranslationXTrack);
      this._translationAxisToggleGroupView.SetInteractable(1, groupTrackInfo.showTranslationYTrack);
      this._translationAxisToggleGroupView.SetInteractable(2, groupTrackInfo.showTranslationZTrack);
      this._translationAxisToggleGroupView.SetValueWithoutNotify((int) lightTranslationEventBox.axis);
      this._flipTranslationToggle.SetIsOnWithoutNotify(lightTranslationEventBox.flipTranslation);
      this._translationDistributionTypeToggleGroupView.SetValueWithoutNotify((int) (lightTranslationEventBox.gapDistributionParamType - 1));
      this._translationDistributionParamInput.value = (float) Mathf.RoundToInt(lightTranslationEventBox.gapDistributionParam * 100f);
      this._translationAffectFirstEventToggle.SetIsOnWithoutNotify(lightTranslationEventBox.gapDistributionShouldAffectFirstBaseEvent);
      this._translationDistributionEaseTypeDropdown.SelectCellWithIdx(((IReadOnlyList<EaseType>) EventBoxView.availableEasings).IndexOf<EaseType>(lightTranslationEventBox.gapDistributionEaseType));
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
