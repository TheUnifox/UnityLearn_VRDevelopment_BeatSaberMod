// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.RotationDistributionView
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
  public class RotationDistributionView : MonoBehaviour
  {
    [SerializeField]
    private BeatmapEditorToggleGroupView _rotationAxisToggleGroupView;
    [SerializeField]
    private BeatmapEditorToggleGroupView _rotationDistributionToggleGroupView;
    [SerializeField]
    private Toggle _flipRotationToggle;
    [SerializeField]
    private FloatInputFieldValidator _rotationDistributionParamInput;
    [SerializeField]
    private Toggle _rotationAffectFirstEventToggle;
    [SerializeField]
    private SimpleTextEditorDropdownView _rotationDistributionEaseTypeDropdown;

    public BeatmapEventDataBox.DistributionParamType distributionParamType => (BeatmapEventDataBox.DistributionParamType) (this._rotationDistributionToggleGroupView.value + 1);

    public LightAxis axis => (LightAxis) this._rotationAxisToggleGroupView.value;

    public float distributionParam => this._rotationDistributionParamInput.value;

    public bool affectFirst => this._rotationAffectFirstEventToggle.isOn;

    public bool flip => this._flipRotationToggle.isOn;

    public EaseType easeType => EventBoxView.availableEasings[this._rotationDistributionEaseTypeDropdown.selectedIndex];

    public event Action distributionChangedEvent;

    public void Initialize()
    {
      this._rotationAxisToggleGroupView.onValueChanged += new Action<int>(this.HandleToggleGroupValueChanged);
      this._rotationDistributionToggleGroupView.onValueChanged += new Action<int>(this.HandleToggleGroupValueChanged);
      this._rotationDistributionParamInput.onInputValidated += new Action<float>(this.HandleInputOnValueChanged);
      this._rotationAffectFirstEventToggle.onValueChanged.AddListener(new UnityAction<bool>(this.HandleToggleValueChanged));
      this._flipRotationToggle.onValueChanged.AddListener(new UnityAction<bool>(this.HandleToggleValueChanged));
      this._rotationDistributionEaseTypeDropdown.didSelectCellWithIdxEvent += new Action<DropdownEditorView, int>(this.HandleDropdownValueChanged);
      this._rotationDistributionEaseTypeDropdown.SetTexts((IReadOnlyList<string>) EventBoxView.availableEasingsNames);
    }

    public void SetData(
      LightRotationEventBoxEditorData lightRotationEventBox,
      EnvironmentTracksDefinitionSO.EventBoxGroupTrackInfo groupTrackInfo)
    {
      this._rotationAxisToggleGroupView.SetInteractable(0, groupTrackInfo.showRotationXTrack);
      this._rotationAxisToggleGroupView.SetInteractable(1, groupTrackInfo.showRotationYTrack);
      this._rotationAxisToggleGroupView.SetInteractable(2, groupTrackInfo.showRotationZTrack);
      this._rotationAxisToggleGroupView.SetValueWithoutNotify((int) lightRotationEventBox.axis);
      this._flipRotationToggle.SetIsOnWithoutNotify(lightRotationEventBox.flipRotation);
      this._rotationDistributionToggleGroupView.SetValueWithoutNotify((int) (lightRotationEventBox.rotationDistributionParamType - 1));
      this._rotationDistributionParamInput.value = lightRotationEventBox.rotationDistributionParam;
      this._rotationAffectFirstEventToggle.SetIsOnWithoutNotify(lightRotationEventBox.rotationDistributionShouldAffectFirstBaseEvent);
      this._rotationDistributionEaseTypeDropdown.SelectCellWithIdx(((IReadOnlyList<EaseType>) EventBoxView.availableEasings).IndexOf<EaseType>(lightRotationEventBox.rotationDistributionEaseType));
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
