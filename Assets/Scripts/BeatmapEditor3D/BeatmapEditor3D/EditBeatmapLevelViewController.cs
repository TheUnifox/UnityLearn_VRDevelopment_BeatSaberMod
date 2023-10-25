// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.EditBeatmapLevelViewController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BeatmapEditor3D
{
  public class EditBeatmapLevelViewController : BeatmapEditorViewController
  {
    [Header("Beatmap Info")]
    [SerializeField]
    private StringInputFieldValidator _songNameInputValidator;
    [SerializeField]
    private StringInputFieldValidator _songSubNameInputValidator;
    [SerializeField]
    private StringInputFieldValidator _songAuthorNameInputValidator;
    [SerializeField]
    private StringInputFieldValidator _levelAuthorNameInputValidator;
    [SerializeField]
    private FloatInputFieldValidator _songTimeOffsetInputValidator;
    [SerializeField]
    private FloatInputFieldValidator _beatsPerMinuteInputValidator;
    [SerializeField]
    private Button _editBpmGridButton;
    [SerializeField]
    private FloatInputFieldValidator _previewStartTimeInputValidator;
    [SerializeField]
    private FloatInputFieldValidator _previewDurationInputValidator;
    [SerializeField]
    private SongInputView _songInputView;
    [SerializeField]
    private CoverImageInputView _coverImageInputView;
    [SerializeField]
    private SimpleTextEditorDropdownView _environmentTextDropdownView;
    [SerializeField]
    private GameObject _environmentTextDropdownDirtyStateGo;
    [SerializeField]
    private SimpleTextEditorDropdownView _allDirectionsEnvironmentTextDropdownView;
    [SerializeField]
    private GameObject _allDirectionsEnvironmentTextDropdownDirtyStateGo;
    [Header("Difficulty Beatmap Sets")]
    [SerializeField]
    private EditBeatmapLevelViewController.DifficultyBeatmapSetViewPair[] _difficultyBeatmapSetViewPairs;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly IReadonlyBeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly EnvironmentsListSO _environmentsList;
    [Inject(Id = "Normal")]
    private readonly EnvironmentTypeSO _normalEnvironmentType;
    [Inject(Id = "Circle")]
    private readonly EnvironmentTypeSO _circleEnvironmentType;
    private EnvironmentInfoSO[] _normalEnvironments;
    private EnvironmentInfoSO[] _allDirectionsEnvironments;
    private bool _deleteEnabled;

    public event Action<BeatmapCharacteristicSO, BeatmapDifficulty> openBeatmapLevelEvent;

    public event Action<BeatmapCharacteristicSO, BeatmapDifficulty> deleteDifficultyBeatmapEvent;

    public event Action editBpmGridEvent;

    public void SetDeleteMode(bool deleteEnabled)
    {
      this._deleteEnabled = deleteEnabled;
      if (!this.isInViewControllerHierarchy)
        return;
      foreach (EditBeatmapLevelViewController.DifficultyBeatmapSetViewPair beatmapSetViewPair in this._difficultyBeatmapSetViewPairs)
        beatmapSetViewPair.view.SetState(this._beatmapDataModel.beatmapDataLoaded, deleteEnabled);
    }

    protected override void DidActivate(
      bool firstActivation,
      bool addedToHierarchy,
      bool screenSystemEnabling)
    {
      if (firstActivation)
      {
        this._normalEnvironments = this._environmentsList.GetAllEnvironmentInfosWithType(this._normalEnvironmentType).ToArray();
        this._environmentTextDropdownView.SetTexts((IReadOnlyList<string>) ((IEnumerable<EnvironmentInfoSO>) this._normalEnvironments).Reverse<EnvironmentInfoSO>().Select<EnvironmentInfoSO, string>((Func<EnvironmentInfoSO, string>) (info => info.environmentName)).ToArray<string>());
        this._allDirectionsEnvironments = this._environmentsList.GetAllEnvironmentInfosWithType(this._circleEnvironmentType).ToArray();
        this._allDirectionsEnvironmentTextDropdownView.SetTexts((IReadOnlyList<string>) ((IEnumerable<EnvironmentInfoSO>) this._allDirectionsEnvironments).Select<EnvironmentInfoSO, string>((Func<EnvironmentInfoSO, string>) (info => info.environmentName)).ToArray<string>());
        this._songNameInputValidator.onInputValidated += (Action<string>) (input => this._signalBus.Fire<BeatmapDataModelSignals.UpdateBeatmapDataSignal>(new BeatmapDataModelSignals.UpdateBeatmapDataSignal(input)));
        this._songSubNameInputValidator.onInputValidated += (Action<string>) (input => this._signalBus.Fire<BeatmapDataModelSignals.UpdateBeatmapDataSignal>(new BeatmapDataModelSignals.UpdateBeatmapDataSignal(songSubName: input)));
        this._songAuthorNameInputValidator.onInputValidated += (Action<string>) (input => this._signalBus.Fire<BeatmapDataModelSignals.UpdateBeatmapDataSignal>(new BeatmapDataModelSignals.UpdateBeatmapDataSignal(songAuthorName: input)));
        this._levelAuthorNameInputValidator.onInputValidated += (Action<string>) (input => this._signalBus.Fire<BeatmapDataModelSignals.UpdateBeatmapDataSignal>(new BeatmapDataModelSignals.UpdateBeatmapDataSignal(levelAuthorName: input)));
        this._songTimeOffsetInputValidator.onInputValidated += (Action<float>) (input => this._signalBus.Fire<BeatmapDataModelSignals.UpdateBeatmapDataSignal>(new BeatmapDataModelSignals.UpdateBeatmapDataSignal(songTimeOffset: new float?(input))));
        this._beatsPerMinuteInputValidator.onInputValidated += (Action<float>) (input => this._signalBus.Fire<BeatmapDataModelSignals.UpdateBeatmapDataSignal>(new BeatmapDataModelSignals.UpdateBeatmapDataSignal(beatsPerMinute: new float?(input))));
        this._previewStartTimeInputValidator.onInputValidated += (Action<float>) (input => this._signalBus.Fire<BeatmapDataModelSignals.UpdateBeatmapDataSignal>(new BeatmapDataModelSignals.UpdateBeatmapDataSignal(previewStartTime: new float?(input))));
        this._previewDurationInputValidator.onInputValidated += (Action<float>) (input => this._signalBus.Fire<BeatmapDataModelSignals.UpdateBeatmapDataSignal>(new BeatmapDataModelSignals.UpdateBeatmapDataSignal(previewDuration: new float?(input))));
        this._songInputView.songLoadedEvent += (Action<string, AudioClip>) ((path, audioClip) => this._signalBus.Fire<BeatmapDataModelSignals.UpdateBeatmapSongSignal>(new BeatmapDataModelSignals.UpdateBeatmapSongSignal(path, audioClip)));
        this._coverImageInputView.coverImageLoadedEvent += (Action<string, Texture2D>) ((path, texture) => this._signalBus.Fire<BeatmapDataModelSignals.UpdateBeatmapCoverImageSignal>(new BeatmapDataModelSignals.UpdateBeatmapCoverImageSignal(path, texture)));
      }
      this._signalBus.Subscribe<BeatmapDataModelSignals.BeatmapLoadedSignal>(new Action(this.HandleBeatmapDataModelUpdated));
      this._signalBus.Subscribe<BeatmapProjectManagerSignals.BeatmapProjectSavedSignal>(new Action(this.HandleBeatmapProjectSaved));
      this._signalBus.Subscribe<BeatmapDataModelSignals.DifficultyBeatmapAddedSignal>(new Action(this.HandleBeatmapAdded));
      this._environmentTextDropdownView.didSelectCellWithIdxEvent += new Action<DropdownEditorView, int>(this.HandleEnvironmentTextDropdownViewDidSelectCellWithIdx);
      this._allDirectionsEnvironmentTextDropdownView.didSelectCellWithIdxEvent += new Action<DropdownEditorView, int>(this.HandleAllDirectionsEnvironmentTextDropdownViewDidSelectCellWithIdx);
      this.buttonBinder.AddBinding(this._editBpmGridButton, new Action(this.HandleEditBpmGridButtonOnClick));
      if (!this._beatmapDataModel.beatmapDataLoaded)
        return;
      this.RefreshData();
    }

    protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
    {
      this._signalBus.TryUnsubscribe<BeatmapDataModelSignals.BeatmapLoadedSignal>(new Action(this.HandleBeatmapDataModelUpdated));
      this._signalBus.TryUnsubscribe<BeatmapProjectManagerSignals.BeatmapProjectSavedSignal>(new Action(this.HandleBeatmapProjectSaved));
      this._signalBus.TryUnsubscribe<BeatmapDataModelSignals.DifficultyBeatmapAddedSignal>(new Action(this.HandleBeatmapAdded));
      this._environmentTextDropdownView.didSelectCellWithIdxEvent -= new Action<DropdownEditorView, int>(this.HandleEnvironmentTextDropdownViewDidSelectCellWithIdx);
      this._allDirectionsEnvironmentTextDropdownView.didSelectCellWithIdxEvent -= new Action<DropdownEditorView, int>(this.HandleAllDirectionsEnvironmentTextDropdownViewDidSelectCellWithIdx);
      this.buttonBinder.ClearBindings();
    }

    private void HandleDifficultyBeatmapSetViewNewBeatmap(
      BeatmapCharacteristicSO beatmapCharacteristic,
      BeatmapDifficulty beatmapDifficulty)
    {
      this._signalBus.Fire<BeatmapDataModelSignals.AddDifficultyBeatmapSignal>(new BeatmapDataModelSignals.AddDifficultyBeatmapSignal(beatmapCharacteristic, beatmapDifficulty));
    }

    private void HandleDifficultyBeatmapSetViewEditBeatmap(
      BeatmapCharacteristicSO beatmapCharacteristic,
      BeatmapDifficulty beatmapDifficulty)
    {
      if (!this._beatmapDataModel.beatmapDataLoaded)
        return;
      Action<BeatmapCharacteristicSO, BeatmapDifficulty> beatmapLevelEvent = this.openBeatmapLevelEvent;
      if (beatmapLevelEvent == null)
        return;
      beatmapLevelEvent(beatmapCharacteristic, beatmapDifficulty);
    }

    private void HandleDifficultyBeatmapSetViewDeleteBeatmap(
      BeatmapCharacteristicSO beatmapCharacteristic,
      BeatmapDifficulty beatmapDifficulty)
    {
      Action<BeatmapCharacteristicSO, BeatmapDifficulty> difficultyBeatmapEvent = this.deleteDifficultyBeatmapEvent;
      if (difficultyBeatmapEvent == null)
        return;
      difficultyBeatmapEvent(beatmapCharacteristic, beatmapDifficulty);
    }

    private void HandleDifficultyBeatmapSetViewDataChanged(
      BeatmapCharacteristicSO beatmapCharacteristic,
      BeatmapDifficulty beatmapDifficulty,
      float njs,
      float offset)
    {
      this._signalBus.Fire<BeatmapDataModelSignals.UpdateDifficultyBeatmapSignal>(new BeatmapDataModelSignals.UpdateDifficultyBeatmapSignal(beatmapCharacteristic, beatmapDifficulty, njs, offset));
    }

    private void HandleEditBpmGridButtonOnClick()
    {
      Action editBpmGridEvent = this.editBpmGridEvent;
      if (editBpmGridEvent == null)
        return;
      editBpmGridEvent();
    }

    private void HandleBeatmapDataModelUpdated() => this.RefreshData();

    private void HandleEnvironmentTextDropdownViewDidSelectCellWithIdx(
      DropdownEditorView dropdown,
      int idx)
    {
      EnvironmentInfoSO normalEnvironment = this._normalEnvironments[this._normalEnvironments.Length - 1 - idx];
      if ((UnityEngine.Object) normalEnvironment == (UnityEngine.Object) this._beatmapDataModel.environment)
        return;
      this._signalBus.Fire<BeatmapDataModelSignals.UpdateBeatmapDataSignal>(new BeatmapDataModelSignals.UpdateBeatmapDataSignal(environmentName: normalEnvironment.serializedName));
      this._environmentTextDropdownDirtyStateGo.SetActive(true);
    }

    private void HandleAllDirectionsEnvironmentTextDropdownViewDidSelectCellWithIdx(
      DropdownEditorView dropdown,
      int idx)
    {
      EnvironmentInfoSO directionsEnvironment = this._allDirectionsEnvironments[this._allDirectionsEnvironments.Length - 1 - idx];
      if ((UnityEngine.Object) directionsEnvironment == (UnityEngine.Object) this._beatmapDataModel.allDirectionsEnvironment)
        return;
      this._signalBus.Fire<BeatmapDataModelSignals.UpdateBeatmapDataSignal>(new BeatmapDataModelSignals.UpdateBeatmapDataSignal(allDirectionsEnvironmentName: directionsEnvironment.serializedName));
      this._allDirectionsEnvironmentTextDropdownDirtyStateGo.SetActive(true);
    }

    private void HandleBeatmapProjectSaved()
    {
      this._songNameInputValidator.ClearDirtyState();
      this._songSubNameInputValidator.ClearDirtyState();
      this._songAuthorNameInputValidator.ClearDirtyState();
      this._levelAuthorNameInputValidator.ClearDirtyState();
      this._songTimeOffsetInputValidator.ClearDirtyState();
      this._beatsPerMinuteInputValidator.ClearDirtyState();
      this._previewStartTimeInputValidator.ClearDirtyState();
      this._previewDurationInputValidator.ClearDirtyState();
      this._songInputView.ClearDirtyState();
      this._coverImageInputView.ClearDirtyState();
      foreach (EditBeatmapLevelViewController.DifficultyBeatmapSetViewPair beatmapSetViewPair in this._difficultyBeatmapSetViewPairs)
        beatmapSetViewPair.view.ClearDirtyState();
      this._environmentTextDropdownDirtyStateGo.SetActive(false);
      this._allDirectionsEnvironmentTextDropdownDirtyStateGo.SetActive(false);
    }

    private void HandleBeatmapAdded() => this.RefreshData();

    private void RefreshData()
    {
      this._environmentTextDropdownDirtyStateGo.SetActive(false);
      this._allDirectionsEnvironmentTextDropdownDirtyStateGo.SetActive(false);
      this._songNameInputValidator.SetValueWithoutNotify(this._beatmapDataModel.songName, true);
      this._songSubNameInputValidator.SetValueWithoutNotify(this._beatmapDataModel.songSubName, true);
      this._songAuthorNameInputValidator.SetValueWithoutNotify(this._beatmapDataModel.songAuthorName, true);
      this._levelAuthorNameInputValidator.SetValueWithoutNotify(this._beatmapDataModel.levelAuthorName, true);
      this._songTimeOffsetInputValidator.SetValueWithoutNotify(this._beatmapDataModel.songTimeOffset, true);
      this._beatsPerMinuteInputValidator.SetValueWithoutNotify(this._beatmapDataModel.beatsPerMinute, true);
      this._previewStartTimeInputValidator.SetValueWithoutNotify(this._beatmapDataModel.previewStartTime, true);
      this._previewDurationInputValidator.SetValueWithoutNotify(this._beatmapDataModel.previewDuration, true);
      this._songInputView.SetAudioPath(this._beatmapDataModel.songFilePath);
      this._coverImageInputView.SetCoverImagePath(this._beatmapDataModel.coverImageFilePath);
      this._environmentTextDropdownView.SelectCellWithIdx(this._normalEnvironments.Length - 1 - Mathf.Clamp(((IReadOnlyList<EnvironmentInfoSO>) this._normalEnvironments).IndexOf<EnvironmentInfoSO>(this._beatmapDataModel.environment), 0, this._normalEnvironments.Length));
      this._allDirectionsEnvironmentTextDropdownView.SelectCellWithIdx(Mathf.Clamp(((IReadOnlyList<EnvironmentInfoSO>) this._allDirectionsEnvironments).IndexOf<EnvironmentInfoSO>(this._beatmapDataModel.allDirectionsEnvironment), 0, this._allDirectionsEnvironments.Length));
      foreach (EditBeatmapLevelViewController.DifficultyBeatmapSetViewPair beatmapSetViewPair in this._difficultyBeatmapSetViewPairs)
      {
        IDifficultyBeatmapSetData difficultyBeatmapSetData;
        this._beatmapDataModel.difficultyBeatmapSets.TryGetValue(beatmapSetViewPair.beatmapCharacteristic, out difficultyBeatmapSetData);
        beatmapSetViewPair.view.SetData(difficultyBeatmapSetData);
        beatmapSetViewPair.view.SetState(this._beatmapDataModel.beatmapDataLoaded, this._deleteEnabled);
        beatmapSetViewPair.view.newBeatmapEvent -= new Action<BeatmapCharacteristicSO, BeatmapDifficulty>(this.HandleDifficultyBeatmapSetViewNewBeatmap);
        beatmapSetViewPair.view.newBeatmapEvent += new Action<BeatmapCharacteristicSO, BeatmapDifficulty>(this.HandleDifficultyBeatmapSetViewNewBeatmap);
        beatmapSetViewPair.view.editBeatmapEvent -= new Action<BeatmapCharacteristicSO, BeatmapDifficulty>(this.HandleDifficultyBeatmapSetViewEditBeatmap);
        beatmapSetViewPair.view.editBeatmapEvent += new Action<BeatmapCharacteristicSO, BeatmapDifficulty>(this.HandleDifficultyBeatmapSetViewEditBeatmap);
        beatmapSetViewPair.view.deleteBeatmapEvent -= new Action<BeatmapCharacteristicSO, BeatmapDifficulty>(this.HandleDifficultyBeatmapSetViewDeleteBeatmap);
        beatmapSetViewPair.view.deleteBeatmapEvent += new Action<BeatmapCharacteristicSO, BeatmapDifficulty>(this.HandleDifficultyBeatmapSetViewDeleteBeatmap);
        beatmapSetViewPair.view.beatmapDataChangedEvent -= new Action<BeatmapCharacteristicSO, BeatmapDifficulty, float, float>(this.HandleDifficultyBeatmapSetViewDataChanged);
        beatmapSetViewPair.view.beatmapDataChangedEvent += new Action<BeatmapCharacteristicSO, BeatmapDifficulty, float, float>(this.HandleDifficultyBeatmapSetViewDataChanged);
      }
    }

    [Serializable]
    public class DifficultyBeatmapSetViewPair
    {
      [SerializeField]
      private BeatmapCharacteristicSO _beatmapCharacteristic;
      [SerializeField]
      private DifficultyBeatmapSetView _view;

      public BeatmapCharacteristicSO beatmapCharacteristic => this._beatmapCharacteristic;

      public DifficultyBeatmapSetView view => this._view;
    }
  }
}
