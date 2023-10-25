// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.EditBeatmapNavigationViewController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using BeatmapEditor3D.Types;
using BeatmapEditor3D.Views;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace BeatmapEditor3D
{
  public class EditBeatmapNavigationViewController : BeatmapEditorViewController
  {
    [Header("Mode Buttons")]
    [SerializeField]
    private Button _beatmapButton;
    [SerializeField]
    private Button _eventGroupsButton;
    [SerializeField]
    private Button _eventsButton;
    [Header("Waveform Type Buttons")]
    [SerializeField]
    private Button _expandedWaveformButton;
    [SerializeField]
    private Button _smallWaveformButton;
    [SerializeField]
    private Button _hiddenWaveformButton;
    [Header("Common Buttons")]
    [SerializeField]
    private Button _testLevelButton;
    [SerializeField]
    private Toggle _enableFPFCToggle;
    [Space]
    [SerializeField]
    private BeatmapObjectsToolbarView _beatmapObjectsToolbarView;
    [SerializeField]
    private EventsToolbarView _eventsToolbarView;
    [SerializeField]
    private EventBoxGroupsToolbarView _eventBoxGroupsToolbarView;
    [SerializeField]
    private EventBoxToolbarView _eventBoxToolbarView;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly BeatmapEditorSettingsDataModel _beatmapEditorSettingsDataModel;
    [Inject]
    private readonly IBeatmapEventsDataModel _beatmapEventsDataModel;
    [Inject]
    private readonly SignalBus _signalBus;

    public event Action<EditBeatmapNavigationViewController.ButtonType> buttonPressedEvent;

    public event Action<EditBeatmapNavigationViewController.ToggleType, bool> toggleChangedEvent;

    protected override void DidActivate(
      bool firstActivation,
      bool addedToHierarchy,
      bool screenSystemEnabling)
    {
      if (addedToHierarchy)
      {
        this._beatmapButton.onClick.AddListener((UnityAction) (() =>
        {
          Action<EditBeatmapNavigationViewController.ButtonType> buttonPressedEvent = this.buttonPressedEvent;
          if (buttonPressedEvent == null)
            return;
          buttonPressedEvent(EditBeatmapNavigationViewController.ButtonType.Objects);
        }));
        this._eventGroupsButton.onClick.AddListener((UnityAction) (() =>
        {
          Action<EditBeatmapNavigationViewController.ButtonType> buttonPressedEvent = this.buttonPressedEvent;
          if (buttonPressedEvent == null)
            return;
          buttonPressedEvent(EditBeatmapNavigationViewController.ButtonType.EventGroups);
        }));
        this._eventsButton.onClick.AddListener((UnityAction) (() =>
        {
          Action<EditBeatmapNavigationViewController.ButtonType> buttonPressedEvent = this.buttonPressedEvent;
          if (buttonPressedEvent == null)
            return;
          buttonPressedEvent(EditBeatmapNavigationViewController.ButtonType.Events);
        }));
        this._testLevelButton.onClick.AddListener((UnityAction) (() =>
        {
          Action<EditBeatmapNavigationViewController.ButtonType> buttonPressedEvent = this.buttonPressedEvent;
          if (buttonPressedEvent == null)
            return;
          buttonPressedEvent(EditBeatmapNavigationViewController.ButtonType.TestLevel);
        }));
        this._expandedWaveformButton.onClick.AddListener((UnityAction) (() => this.HandleWaveformTypeChangeButtonPressed(BeatmapEditor3D.Types.WaveformType.Expanded)));
        this._smallWaveformButton.onClick.AddListener((UnityAction) (() => this.HandleWaveformTypeChangeButtonPressed(BeatmapEditor3D.Types.WaveformType.Small)));
        this._hiddenWaveformButton.onClick.AddListener((UnityAction) (() => this.HandleWaveformTypeChangeButtonPressed(BeatmapEditor3D.Types.WaveformType.Hidden)));
        this._enableFPFCToggle.onValueChanged.AddListener((UnityAction<bool>) (isOn =>
        {
          Action<EditBeatmapNavigationViewController.ToggleType, bool> toggleChangedEvent = this.toggleChangedEvent;
          if (toggleChangedEvent == null)
            return;
          toggleChangedEvent(EditBeatmapNavigationViewController.ToggleType.EnableFPFC, isOn);
        }));
        this._eventsToolbarView.SetLightViewVersion(this._beatmapEventsDataModel.lightEventsVersion);
      }
      this._signalBus.Subscribe<BeatmapEditingModeSwitched>(new Action(this.HandleLevelEditorModeSwitchedSignal));
      this._signalBus.Subscribe<WaveformTypeChangedSignal>(new Action(this.HandleWaveformTypeChanged));
      this.UpdateToolbar();
    }

    protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
    {
      if (removedFromHierarchy)
      {
        this._beatmapButton.onClick.RemoveAllListeners();
        this._eventGroupsButton.onClick.RemoveAllListeners();
        this._eventsButton.onClick.RemoveAllListeners();
        this._testLevelButton.onClick.RemoveAllListeners();
        this._enableFPFCToggle.onValueChanged.RemoveAllListeners();
      }
      this._signalBus.TryUnsubscribe<BeatmapEditingModeSwitched>(new Action(this.HandleLevelEditorModeSwitchedSignal));
      this._signalBus.TryUnsubscribe<WaveformTypeChangedSignal>(new Action(this.HandleWaveformTypeChanged));
    }

    private void UpdateToolbar()
    {
      this._enableFPFCToggle.SetIsOnWithoutNotify(this._beatmapEditorSettingsDataModel.enableFpfc);
      this._beatmapButton.interactable = this._beatmapState.editingMode != 0;
      this._eventGroupsButton.interactable = this._beatmapState.editingMode != BeatmapEditingMode.EventBoxGroups;
      this._eventsButton.interactable = this._beatmapState.editingMode != BeatmapEditingMode.BasicEvents;
      this._expandedWaveformButton.interactable = this._beatmapEditorSettingsDataModel.waveformType != 0;
      this._smallWaveformButton.interactable = this._beatmapEditorSettingsDataModel.waveformType != BeatmapEditor3D.Types.WaveformType.Small;
      this._hiddenWaveformButton.interactable = this._beatmapEditorSettingsDataModel.waveformType != BeatmapEditor3D.Types.WaveformType.Hidden;
      this._beatmapObjectsToolbarView.gameObject.SetActive(this._beatmapState.editingMode == BeatmapEditingMode.Objects);
      this._eventsToolbarView.gameObject.SetActive(this._beatmapState.editingMode == BeatmapEditingMode.BasicEvents);
      this._eventBoxGroupsToolbarView.gameObject.SetActive(this._beatmapState.editingMode == BeatmapEditingMode.EventBoxGroups);
      this._eventBoxToolbarView.gameObject.SetActive(this._beatmapState.editingMode == BeatmapEditingMode.EventBoxes);
    }

    private void HandleLevelEditorModeSwitchedSignal() => this.UpdateToolbar();

    private void HandleWaveformTypeChanged() => this.UpdateToolbar();

    private void HandleWaveformTypeChangeButtonPressed(BeatmapEditor3D.Types.WaveformType waveformType) => this._signalBus.Fire<ChangeWaveformTypeSignal>(new ChangeWaveformTypeSignal(waveformType));

    public enum ButtonType
    {
      Objects,
      EventGroups,
      Events,
      TestLevel,
      CopyFromDifficulty,
    }

    public enum ToggleType
    {
      EnableFPFC,
    }
  }
}
