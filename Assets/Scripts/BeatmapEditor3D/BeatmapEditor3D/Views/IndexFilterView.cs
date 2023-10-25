// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.IndexFilterView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.SerializedData;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BeatmapEditor3D.Views
{
  public class IndexFilterView : MonoBehaviour
  {
    [DoesNotRequireDomainReloadInit]
    private static readonly Dictionary<IndexFilterEditorData.IndexFilterType, (int, int)> _defaultValues = new Dictionary<IndexFilterEditorData.IndexFilterType, (int, int)>()
    {
      {
        IndexFilterEditorData.IndexFilterType.Division,
        (1, 1)
      },
      {
        IndexFilterEditorData.IndexFilterType.StepAndOffset,
        (1, 1)
      }
    };
    [SerializeField]
    private BeatmapEditorToggleGroupView _typeToggleGroup;
    [SerializeField]
    private Toggle _reversedToggle;
    [SerializeField]
    private TextMeshProUGUI _param0LabelText;
    [SerializeField]
    private IntInputFieldValidator _param0Input;
    [SerializeField]
    private TextMeshProUGUI _param1LabelText;
    [SerializeField]
    private IntInputFieldValidator _param1Input;
    [SerializeField]
    private Toggle _randomToggle;
    [SerializeField]
    private Toggle _orderToggle;
    [SerializeField]
    private IntInputFieldValidator _randomSeedValidator;
    [SerializeField]
    private Button _newSeedButton;
    [SerializeField]
    private Button _resetSeedButton;
    [SerializeField]
    private IntInputFieldValidator _groupingValidator;
    [SerializeField]
    private IntInputFieldValidator _limitValidator;
    [SerializeField]
    private Toggle _durationLimitToggle;
    [SerializeField]
    private Toggle _distributionLimitToggle;
    private IndexFilterEditorData _indexFilter;
    private float _groupBeat;
    private int _groupSize;
    private bool _initialized;

    public event Action<IndexFilterEditorData> didChangeEvent;

    public IndexFilterEditorData indexFilter => this._indexFilter;

    public void SetData(IndexFilterEditorData indexFilter, float groupBeat, int groupSize)
    {
      this.Initialize();
      this._indexFilter = indexFilter;
      this._groupBeat = groupBeat;
      this._groupSize = groupSize;
      this._typeToggleGroup.SetValueWithoutNotify((int) (indexFilter.type - 1));
      this._reversedToggle.SetIsOnWithoutNotify(indexFilter.reversed);
      switch (indexFilter.type)
      {
        case IndexFilterEditorData.IndexFilterType.Division:
          this._param0Input.SetValueWithoutNotify(indexFilter.param0, true);
          this._param1Input.SetValueWithoutNotify(indexFilter.param1 + 1, true);
          break;
        case IndexFilterEditorData.IndexFilterType.StepAndOffset:
          this._param0Input.SetValueWithoutNotify(indexFilter.param0 + 1, true);
          this._param1Input.SetValueWithoutNotify(indexFilter.param1, true);
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      this._randomToggle.SetIsOnWithoutNotify(indexFilter.randomType.HasFlag((Enum) IndexFilter.IndexFilterRandomType.RandomElements));
      this._orderToggle.SetIsOnWithoutNotify(indexFilter.randomType.HasFlag((Enum) IndexFilter.IndexFilterRandomType.KeepOrder));
      this._randomSeedValidator.SetValueWithoutNotify(indexFilter.seed, false);
      this._randomSeedValidator.interactable = indexFilter.randomType.HasFlag((Enum) IndexFilter.IndexFilterRandomType.RandomElements);
      this._newSeedButton.interactable = indexFilter.randomType.HasFlag((Enum) IndexFilter.IndexFilterRandomType.RandomElements);
      this._resetSeedButton.interactable = indexFilter.randomType.HasFlag((Enum) IndexFilter.IndexFilterRandomType.RandomElements);
      this._orderToggle.interactable = indexFilter.randomType.HasFlag((Enum) IndexFilter.IndexFilterRandomType.RandomElements);
      this._groupingValidator.SetValueWithoutNotify(indexFilter.chunks, false);
      this._limitValidator.SetValueWithoutNotify(Mathf.CeilToInt(indexFilter.limit * 100f), false);
      this._durationLimitToggle.SetIsOnWithoutNotify(indexFilter.limitAlsoAffectType.HasFlag((Enum) IndexFilter.IndexFilterLimitAlsoAffectType.Duration));
      this._distributionLimitToggle.SetIsOnWithoutNotify(indexFilter.limitAlsoAffectType.HasFlag((Enum) IndexFilter.IndexFilterLimitAlsoAffectType.Distribution));
      this.SetLabelData(this._indexFilter.type);
    }

    private void Initialize()
    {
      if (this._initialized)
        return;
      this._initialized = true;
      this._typeToggleGroup.onValueChanged += new Action<int>(this.HandleTypeToggleGroupValueChanged);
      this._reversedToggle.onValueChanged.AddListener(new UnityAction<bool>(this.HandleDefaultToggleValidated));
      this._param0Input.onInputValidated += new Action<int>(this.HandleDefaultInputValidated);
      this._param1Input.onInputValidated += new Action<int>(this.HandleDefaultInputValidated);
      this._randomToggle.onValueChanged.AddListener(new UnityAction<bool>(this.HandleRandomToggleValueChanged));
      this._orderToggle.onValueChanged.AddListener(new UnityAction<bool>(this.HandleDefaultToggleValidated));
      this._randomSeedValidator.onInputValidated += new Action<int>(this.HandleDefaultInputValidated);
      this._newSeedButton.onClick.AddListener(new UnityAction(this.HandleNewSeedOnClick));
      this._resetSeedButton.onClick.AddListener(new UnityAction(this.HandleResetSeedButtonOnClick));
      this._groupingValidator.onInputValidated += new Action<int>(this.HandleDefaultInputValidated);
      this._limitValidator.onInputValidated += new Action<int>(this.HandleDefaultInputValidated);
      this._durationLimitToggle.onValueChanged.AddListener(new UnityAction<bool>(this.HandleDefaultToggleValidated));
      this._distributionLimitToggle.onValueChanged.AddListener(new UnityAction<bool>(this.HandleDefaultToggleValidated));
    }

    private void HandleTypeToggleGroupValueChanged(int idx)
    {
      IndexFilterEditorData.IndexFilterType key = (IndexFilterEditorData.IndexFilterType) (idx + 1);
      (int, int) defaultValue = IndexFilterView._defaultValues[key];
      this.SetParamsInputs(defaultValue.Item1, defaultValue.Item2);
    }

    private void HandleRandomToggleValueChanged(bool isOn)
    {
      this._randomSeedValidator.interactable = isOn;
      this._newSeedButton.interactable = isOn;
      this._resetSeedButton.interactable = isOn;
      this._orderToggle.interactable = isOn;
      this._randomSeedValidator.SetValueWithoutNotify(BitConverter.ToInt32(BitConverter.GetBytes(this._groupBeat), 0), false);
      if (!isOn)
        this._orderToggle.SetIsOnWithoutNotify(false);
      this.TriggerDidChange();
    }

    private void HandleNewSeedOnClick()
    {
      this._randomSeedValidator.SetValueWithoutNotify(UnityEngine.Random.Range(int.MinValue, int.MaxValue), false);
      this.TriggerDidChange();
    }

    private void HandleResetSeedButtonOnClick()
    {
      this._randomSeedValidator.SetValueWithoutNotify(BitConverter.ToInt32(BitConverter.GetBytes(this._groupBeat), 0), false);
      this.TriggerDidChange();
    }

    private void HandleDefaultInputValidated(int _) => this.TriggerDidChange();

    private void HandleDefaultToggleValidated(bool _) => this.TriggerDidChange();

    private void SetParamsInputs(int param0, int param1)
    {
      this._param0Input.value = param0;
      this._param1Input.value = param1;
      this.TriggerDidChange();
    }

    private void SetLabelData(IndexFilterEditorData.IndexFilterType type)
    {
      if (type != IndexFilterEditorData.IndexFilterType.Division)
      {
        if (type != IndexFilterEditorData.IndexFilterType.StepAndOffset)
          return;
        this._param0LabelText.text = "Light #";
        this._param1LabelText.text = "Step";
      }
      else
      {
        this._param0LabelText.text = "Sections";
        this._param1LabelText.text = "Id";
      }
    }

    private void TriggerDidChange()
    {
      int b = this._param0Input.value;
      int num1 = this._param1Input.value;
      IndexFilterEditorData.IndexFilterType type1 = (IndexFilterEditorData.IndexFilterType) (this._typeToggleGroup.value + 1);
      bool isOn = this._reversedToggle.isOn;
      if (type1 != IndexFilterEditorData.IndexFilterType.Division)
      {
        if (type1 != IndexFilterEditorData.IndexFilterType.StepAndOffset)
          throw new ArgumentOutOfRangeException();
        b = Mathf.Max(1, b) - 1;
      }
      else
        --num1;
      this.SetLabelData(type1);
      IndexFilter.IndexFilterRandomType filterRandomType1 = IndexFilter.IndexFilterRandomType.NoRandom;
      if (this._randomToggle.isOn)
        filterRandomType1 |= IndexFilter.IndexFilterRandomType.RandomElements;
      if (this._randomToggle.isOn && this._orderToggle.isOn)
        filterRandomType1 |= IndexFilter.IndexFilterRandomType.KeepOrder;
      IndexFilter.IndexFilterLimitAlsoAffectType limitAlsoAffectType = IndexFilter.IndexFilterLimitAlsoAffectType.None;
      if (this._durationLimitToggle.isOn)
        limitAlsoAffectType |= IndexFilter.IndexFilterLimitAlsoAffectType.Duration;
      if (this._distributionLimitToggle.isOn)
        limitAlsoAffectType |= IndexFilter.IndexFilterLimitAlsoAffectType.Distribution;
      int type2 = (int) type1;
      int num2 = b;
      int num3 = num1;
      int num4 = isOn ? 1 : 0;
      IndexFilter.IndexFilterRandomType filterRandomType2 = filterRandomType1;
      int num5 = this._randomSeedValidator.value;
      int chunks = this._groupingValidator.value;
      int randomType = (int) filterRandomType2;
      int seed = num5;
      double limit = (double) this._limitValidator.value / 100.0;
      int num6 = (int) limitAlsoAffectType;
      IndexFilterEditorData f = IndexFilterEditorData.CreateNew((IndexFilterEditorData.IndexFilterType) type2, num2, num3, num4 != 0, chunks, (IndexFilter.IndexFilterRandomType) randomType, seed, (float) limit, (IndexFilter.IndexFilterLimitAlsoAffectType) num6);
      if (!BeatmapDataLoader.IndexFilterConvertor.IsIndexFilterValid(BeatmapLevelDataModelSaver.CreateIndexFilter(f), this._groupSize))
        return;
      this._indexFilter = f;
      Action<IndexFilterEditorData> didChangeEvent = this.didChangeEvent;
      if (didChangeEvent == null)
        return;
      didChangeEvent(this._indexFilter);
    }
  }
}
