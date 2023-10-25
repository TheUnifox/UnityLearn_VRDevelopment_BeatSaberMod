// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.EventBoxView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Views
{
  public class EventBoxView : MonoBehaviour
  {
    [SerializeField]
    private GroupInfoView _groupInfoView;
    [SerializeField]
    private BeatmapEditorToggleGroupView _beatDistributionTypeToggleGroup;
    [SerializeField]
    private FloatInputFieldValidator _beatDistributionInput;
    [SerializeField]
    private IndexFilterView _indexFilterView;
    [Header("Color")]
    [SerializeField]
    private GameObject _lightColorWrapper;
    [SerializeField]
    private BrightnessDistributionView _brightnessDistributionView;
    [Header("Rotation")]
    [SerializeField]
    private GameObject _lightRotationWrapper;
    [SerializeField]
    private RotationDistributionView _rotationDistributionView;
    [Header("Translation")]
    [SerializeField]
    private GameObject _lightTranslationWrapper;
    [SerializeField]
    private GapDistributionView _gapDistributionView;
    [Inject]
    private readonly BeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly EventBoxGroupsState _eventBoxGroupsState;
    [DoesNotRequireDomainReloadInit]
    public static readonly EaseType[] availableEasings = new EaseType[4]
    {
      EaseType.Linear,
      EaseType.InQuad,
      EaseType.OutQuad,
      EaseType.InOutQuad
    };
    [DoesNotRequireDomainReloadInit]
    public static readonly string[] availableEasingsNames = new string[4]
    {
      "Linear",
      "In Quad",
      "Out Quad",
      "In Out Quad"
    };
    private EventBoxView.Type _type;
    private int _groupSize;
    private bool _initialized;
    private EventBoxEditorData _eventBox;

    public event Action<EventBoxEditorData> saveEventBoxEvent;

    public void SetData(
      float groupBeat,
      LightColorEventBoxEditorData lightColorEventBox,
      int groupSize)
    {
      this.Initialize();
      this._type = EventBoxView.Type.Color;
      this.SetCommonData(groupBeat, (EventBoxEditorData) lightColorEventBox, groupSize);
      this._brightnessDistributionView.SetData(lightColorEventBox);
      this._lightColorWrapper.SetActive(true);
      this._lightRotationWrapper.SetActive(false);
      this._lightTranslationWrapper.SetActive(false);
    }

    public void SetData(
      float groupBeat,
      LightRotationEventBoxEditorData lightRotationEventBox,
      int groupSize)
    {
      this.Initialize();
      this._type = EventBoxView.Type.Rotation;
      EnvironmentTracksDefinitionSO.EventBoxGroupTrackInfo groupTrackInfo = this._beatmapDataModel.environmentTrackDefinition.groupIdToTrackInfo[this._eventBoxGroupsState.eventBoxGroupContext.groupId];
      this.SetCommonData(groupBeat, (EventBoxEditorData) lightRotationEventBox, groupSize);
      this._rotationDistributionView.SetData(lightRotationEventBox, groupTrackInfo);
      this._lightColorWrapper.SetActive(false);
      this._lightRotationWrapper.SetActive(true);
      this._lightTranslationWrapper.SetActive(false);
    }

    public void SetData(
      float groupBeat,
      LightTranslationEventBoxEditorData lightTranslationEventBox,
      int groupSize)
    {
      this.Initialize();
      this._type = EventBoxView.Type.Translation;
      EnvironmentTracksDefinitionSO.EventBoxGroupTrackInfo groupTrackInfo = this._beatmapDataModel.environmentTrackDefinition.groupIdToTrackInfo[this._eventBoxGroupsState.eventBoxGroupContext.groupId];
      this.SetCommonData(groupBeat, (EventBoxEditorData) lightTranslationEventBox, groupSize);
      this._gapDistributionView.SetData(lightTranslationEventBox, groupTrackInfo);
      this._lightColorWrapper.SetActive(false);
      this._lightRotationWrapper.SetActive(false);
      this._lightTranslationWrapper.SetActive(true);
    }

    private void SetCommonData(float groupBeat, EventBoxEditorData eventBox, int groupSize)
    {
      this._eventBox = eventBox;
      this._groupSize = groupSize;
      this._beatDistributionTypeToggleGroup.SetValueWithoutNotify((int) (eventBox.beatDistributionParamType - 1));
      this._beatDistributionInput.value = eventBox.beatDistributionParam;
      this._indexFilterView.SetData(eventBox.indexFilter, groupBeat, groupSize);
      this._groupInfoView.SetIndexFilter(eventBox.indexFilter, groupSize);
    }

    private void Initialize()
    {
      if (this._initialized)
        return;
      this._initialized = true;
      this._brightnessDistributionView.Initialize();
      this._rotationDistributionView.Initialize();
      this._gapDistributionView.Initialize();
      this._beatDistributionInput.onInputValidated += new Action<float>(this.HandleInputOnValueChanged);
      this._beatDistributionTypeToggleGroup.onValueChanged += new Action<int>(this.HandleToggleValueChanged);
      this._indexFilterView.didChangeEvent += new Action<IndexFilterEditorData>(this.HandleIndexFilterDidChange);
      this._brightnessDistributionView.distributionChangedEvent += new Action(this.HandleDistributionChanged);
      this._rotationDistributionView.distributionChangedEvent += new Action(this.HandleDistributionChanged);
      this._gapDistributionView.distributionChangedEvent += new Action(this.HandleDistributionChanged);
    }

    private void HandleDistributionChanged() => this.TriggerDidChange();

    private void HandleInputOnValueChanged(float _) => this.TriggerDidChange();

    private void HandleToggleValueChanged(int _) => this.TriggerDidChange();

    private void HandleIndexFilterDidChange(IndexFilterEditorData indexFilterEditorData)
    {
      this._groupInfoView.SetIndexFilter(indexFilterEditorData, this._groupSize);
      this.TriggerDidChange();
    }

    private void TriggerDidChange()
    {
      BeatmapEventDataBox.DistributionParamType beatDistributionParamType = (BeatmapEventDataBox.DistributionParamType) (this._beatDistributionTypeToggleGroup.value + 1);
      IndexFilterEditorData indexFilter = this._indexFilterView.indexFilter;
      switch (this._type)
      {
        case EventBoxView.Type.Color:
          this._eventBox = (EventBoxEditorData) LightColorEventBoxEditorData.CreateNewWithId(this._eventBox.id, indexFilter, beatDistributionParamType, this._beatDistributionInput.value, this._brightnessDistributionView.distributionParamType, this._brightnessDistributionView.distributionParam, this._brightnessDistributionView.affectFirst, this._brightnessDistributionView.easeType);
          break;
        case EventBoxView.Type.Rotation:
          this._eventBox = (EventBoxEditorData) LightRotationEventBoxEditorData.CreateNewWithId(this._eventBox.id, indexFilter, beatDistributionParamType, this._beatDistributionInput.value, this._rotationDistributionView.distributionParamType, this._rotationDistributionView.distributionParam, this._rotationDistributionView.affectFirst, this._rotationDistributionView.easeType, this._rotationDistributionView.axis, this._rotationDistributionView.flip);
          break;
        case EventBoxView.Type.Translation:
          this._eventBox = (EventBoxEditorData) LightTranslationEventBoxEditorData.CreateNewWithId(this._eventBox.id, indexFilter, beatDistributionParamType, this._beatDistributionInput.value, this._gapDistributionView.distributionParamType, this._gapDistributionView.distributionParam, this._gapDistributionView.affectFirst, this._gapDistributionView.easeType, this._gapDistributionView.axis, this._gapDistributionView.flip);
          break;
      }
      Action<EventBoxEditorData> saveEventBoxEvent = this.saveEventBoxEvent;
      if (saveEventBoxEvent == null)
        return;
      saveEventBoxEvent(this._eventBox);
    }

    private enum Type
    {
      Color,
      Rotation,
      Translation,
    }
  }
}
