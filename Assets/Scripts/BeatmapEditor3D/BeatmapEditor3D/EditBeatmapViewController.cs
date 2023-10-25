// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.EditBeatmapViewController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands;
using BeatmapEditor3D.Controller;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.InputSignals;
using BeatmapEditor3D.LevelEditor;
using BeatmapEditor3D.Types;
using BeatmapEditor3D.Views;
using HMUI;
using Polyglot;
using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class EditBeatmapViewController : BeatmapEditorViewController
  {
    [SerializeField]
    private CurrentSubdivisionView _currentSubdivisionView;
    [SerializeField]
    private CurrentTimeView _currentTimeView;
    [SerializeField]
    private StaticAudioWaveformView _staticAudioWaveformView;
    [SerializeField]
    private InteractiveAudioWaveformView _interactiveAudioWaveformView;
    [SerializeField]
    private TextMeshProUGUI _difficultyCharacteristicText;
    [SerializeField]
    private ActiveSelectionView _activeSelectionView;
    [SerializeField]
    private EventBoxesView _eventBoxesView;
    [SerializeField]
    private StatusBarWaveformView _statusBarWaveformView;
    [SerializeField]
    private BeatmapEditorExtendedSettingsView _beatmapEditorExtendedSettingsView;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly BeatmapEditorSettingsDataModel _beatmapEditorSettingsDataModel;
    [Inject]
    private readonly IReadonlyBeatmapLevelDataModel _beatmapLevelDataModel;
    [Inject]
    private readonly IReadonlyBeatmapObjectsState _readonlyBeatmapObjectsState;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly BeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly ISongPreviewController _songPreviewController;
    private readonly KeyboardBinder _keyboardBinder = new KeyboardBinder();
    private EditBeatmapObjectsKeyboardBinder _editBeatmapObjectsKeyboardBinder;
    private EditBasicEventsKeyboardBinder _editBasicEventsKeyboardBinder;
    private EditEventBoxGroupsKeyboardBinder _editEventBoxGroupsKeyboardBinder;
    private EditEventBoxesKeyboardBinder _editEventBoxesKeyboardBinder;

    public event Action<EditBeatmapViewController.ButtonType> buttonPressedEvent;

    protected override void DidActivate(
      bool firstActivation,
      bool addedToHierarchy,
      bool screenSystemEnabling)
    {
      if (firstActivation)
      {
        this._editBeatmapObjectsKeyboardBinder = new EditBeatmapObjectsKeyboardBinder(this._keyboardBinder, this._signalBus, this._readonlyBeatmapObjectsState, this._beatmapState);
        this._editBasicEventsKeyboardBinder = new EditBasicEventsKeyboardBinder(this._keyboardBinder, this._signalBus, this._beatmapState);
        this._editEventBoxGroupsKeyboardBinder = new EditEventBoxGroupsKeyboardBinder(this._keyboardBinder, this._signalBus, this._beatmapState);
        this._editEventBoxesKeyboardBinder = new EditEventBoxesKeyboardBinder(this._keyboardBinder, this._signalBus, this._beatmapState);
      }
      this._difficultyCharacteristicText.text = this._beatmapLevelDataModel.beatmapDifficulty.SerializedName() + "/" + Localization.Get(this._beatmapLevelDataModel.beatmapCharacteristic.characteristicNameLocalizationKey);
      this._currentSubdivisionView.SetSubdivision(this._beatmapState.subdivision, this._beatmapState.prevSubdivision);
      this._statusBarWaveformView.SetType(this._beatmapEditorSettingsDataModel.waveformType);
      this._currentTimeView.SetAudioClip(this._beatmapDataModel.audioClip);
      this._currentTimeView.SetCurrentBeat(this._beatmapDataModel.bpmData.BeatToSample(this._beatmapState.beat), this._beatmapState.beat);
      this._staticAudioWaveformView.SetAudioClip(this._beatmapDataModel.audioClip);
      this._interactiveAudioWaveformView.SetAudioClip(this._beatmapDataModel.audioClip);
      this.SetCurrentKeyBindings();
      this._signalBus.Subscribe<BeatmapEditingModeSwitched>(new Action(this.HandleLevelEditorModeSwitched));
      this._signalBus.Subscribe<BeatmapLevelStateTimeUpdated>(new Action(this.HandleLevelEditorStateTimeUpdated));
      this._signalBus.Subscribe<SubdivisionChangedSignal>(new Action(this.HandleSubdivisionChanged));
      this._signalBus.Subscribe<WaveformTypeChangedSignal>(new Action(this.HandleWaveformTypeChanged));
      this._beatmapEditorExtendedSettingsView.copyBeatmapButtonClicked += new Action(this.HandleBeatmapEditorExtendedSettingsViewCopyBeatmapButtonClicked);
      this._activeSelectionView.mirrorSelectedBeatmapObjectsEvent += new Action(this.HandleActiveSelectionMirrorSelectedBeatmapObjects);
      this._activeSelectionView.deleteSelectedBeatmapObjectsEvent += new Action(this.HandleActiveSelectionDeleteSelectedBeatmapObjects);
      this._activeSelectionView.connectSelectedNotesWithSliderEvent += new Action(this.HandleActiveSelectionConnectSelectedNotesWithSlider);
      this._activeSelectionView.mirrorSelectedEventsEvent += new Action(this.HandleActiveSelectionMirrorSelectedEvents);
      this._activeSelectionView.deleteSelectedEventsEvent += new Action(this.HandleActiveSelectionDeleteSelectedEvents);
      this._activeSelectionView.mirrorSelectedEventBoxGroupsEvent += new Action(this.HandleActiveSelectionMirrorSelectedEventBoxGroups);
      this._activeSelectionView.deleteSelectedEventBoxGroupsEvent += new Action(this.HandleActiveSelectionDeleteSelectedEventBoxGroups);
      this._activeSelectionView.deleteSelectedEventBoxesEventsEvent += new Action(this.HandleActiveSelectionDeleteSelectedEventBoxesEvents);
      this._activeSelectionView.mirrorSelectedEventBoxesEventsEvent += new Action(this.HandleActiveSelectionMirrorSelectedEventBoxesEventsEvent);
    }

    protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
    {
      this._signalBus.TryUnsubscribe<BeatmapEditingModeSwitched>(new Action(this.HandleLevelEditorModeSwitched));
      this._signalBus.TryUnsubscribe<BeatmapLevelStateTimeUpdated>(new Action(this.HandleLevelEditorStateTimeUpdated));
      this._signalBus.TryUnsubscribe<SubdivisionChangedSignal>(new Action(this.HandleSubdivisionChanged));
      this._signalBus.TryUnsubscribe<WaveformTypeChangedSignal>(new Action(this.HandleWaveformTypeChanged));
      this._beatmapEditorExtendedSettingsView.copyBeatmapButtonClicked -= new Action(this.HandleBeatmapEditorExtendedSettingsViewCopyBeatmapButtonClicked);
      this._activeSelectionView.mirrorSelectedBeatmapObjectsEvent -= new Action(this.HandleActiveSelectionMirrorSelectedBeatmapObjects);
      this._activeSelectionView.deleteSelectedBeatmapObjectsEvent -= new Action(this.HandleActiveSelectionDeleteSelectedBeatmapObjects);
      this._activeSelectionView.connectSelectedNotesWithSliderEvent -= new Action(this.HandleActiveSelectionConnectSelectedNotesWithSlider);
      this._activeSelectionView.mirrorSelectedEventsEvent -= new Action(this.HandleActiveSelectionMirrorSelectedEvents);
      this._activeSelectionView.deleteSelectedEventsEvent -= new Action(this.HandleActiveSelectionDeleteSelectedEvents);
      this._activeSelectionView.mirrorSelectedEventBoxGroupsEvent -= new Action(this.HandleActiveSelectionMirrorSelectedEventBoxGroups);
      this._activeSelectionView.deleteSelectedEventBoxGroupsEvent -= new Action(this.HandleActiveSelectionDeleteSelectedEventBoxGroups);
      this._activeSelectionView.deleteSelectedEventBoxesEventsEvent -= new Action(this.HandleActiveSelectionDeleteSelectedEventBoxesEvents);
      this._activeSelectionView.mirrorSelectedEventBoxesEventsEvent -= new Action(this.HandleActiveSelectionMirrorSelectedEventBoxesEventsEvent);
      this._keyboardBinder.ClearBindings();
      this._songPreviewController.Stop();
    }

    protected void Update() => this._keyboardBinder.ManualUpdate();

    private void SetCurrentKeyBindings()
    {
      this._keyboardBinder.ClearBindings();
      this._activeSelectionView.SetMode(this._beatmapState.editingMode);
      this._eventBoxesView.gameObject.SetActive(this._beatmapState.editingMode == BeatmapEditingMode.EventBoxes);
      switch (this._beatmapState.editingMode)
      {
        case BeatmapEditingMode.Objects:
          this._editBeatmapObjectsKeyboardBinder.Enable();
          break;
        case BeatmapEditingMode.BasicEvents:
          this._editBasicEventsKeyboardBinder.Enable();
          break;
        case BeatmapEditingMode.EventBoxGroups:
          this._editEventBoxGroupsKeyboardBinder.Enable();
          break;
        case BeatmapEditingMode.EventBoxes:
          this._editEventBoxesKeyboardBinder.Enable();
          break;
      }
    }

    private void HandleLevelEditorModeSwitched() => this.SetCurrentKeyBindings();

    private void HandleActiveSelectionMirrorSelectedEventBoxGroups() => this._signalBus.Fire<MirrorSelectedEventBoxGroupsSignal>();

    private void HandleActiveSelectionDeleteSelectedEventBoxGroups() => this._signalBus.Fire<DeleteSelectedEventBoxGroupsSignal>();

    private void HandleActiveSelectionDeleteSelectedEventBoxesEvents() => this._signalBus.Fire<DeleteSelectedEventBoxEventsSignal>();

    private void HandleActiveSelectionMirrorSelectedEventBoxesEventsEvent() => this._signalBus.Fire<MirrorSelectedEventBoxEventsSignal>();

    private void HandleActiveSelectionDeleteSelectedEvents() => this._signalBus.Fire<DeleteSelectedEventsSignal>();

    private void HandleActiveSelectionMirrorSelectedEvents() => this._signalBus.Fire<MirrorLightEventsSignal>();

    private void HandleActiveSelectionDeleteSelectedBeatmapObjects() => this._signalBus.Fire<DeleteBeatmapObjectsSignal>();

    private void HandleActiveSelectionConnectSelectedNotesWithSlider() => this._signalBus.Fire<ConnectBeatmapObjectsWithArcSignal>();

    private void HandleActiveSelectionMirrorSelectedBeatmapObjects() => this._signalBus.Fire<MirrorSelectedBeatmapObjectsSignal>();

    private void HandleBeatmapEditorExtendedSettingsViewCopyBeatmapButtonClicked()
    {
      Action<EditBeatmapViewController.ButtonType> buttonPressedEvent = this.buttonPressedEvent;
      if (buttonPressedEvent == null)
        return;
      buttonPressedEvent(EditBeatmapViewController.ButtonType.CopyFromDifficulty);
    }

    private void HandleLevelEditorStateTimeUpdated() => this._currentTimeView.SetCurrentBeat(this._beatmapDataModel.bpmData.BeatToSample(this._beatmapState.beat), this._beatmapState.beat);

    private void HandleSubdivisionChanged() => this._currentSubdivisionView.SetSubdivision(this._beatmapState.subdivision, this._beatmapState.prevSubdivision);

    private void HandleWaveformTypeChanged() => this._statusBarWaveformView.SetType(this._beatmapEditorSettingsDataModel.waveformType);

    public enum ButtonType
    {
      CopyFromDifficulty,
    }
  }
}
