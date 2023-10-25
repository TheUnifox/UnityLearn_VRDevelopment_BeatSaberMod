// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapFlowCoordinator
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands;
using BeatmapEditor3D.Controller;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using BeatmapEditor3D.Types;
using HMUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class BeatmapFlowCoordinator : BeatmapEditorFlowCoordinator
  {
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly IReadonlyBeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly BeatmapEditorSettingsDataModel _beatmapEditorSettingsDataModel;
    [Inject]
    private readonly BeatmapLevelDataModel _beatmapLevelDataModel;
    [Inject]
    private readonly BeatmapBasicEventsDataModel _beatmapBasicEventsDataModel;
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;
    [Inject]
    private readonly BeatmapState _beatmapState;
    [Inject]
    private readonly IBasicEventsState _basicEventsState;
    [Inject]
    private readonly IReadonlyBeatmapObjectsState _readonlyBeatmapObjectsState;
    [Inject]
    private readonly EventBoxGroupsState _eventBoxGroupsState;
    [Inject]
    private readonly BeatmapEditorScenesManager _beatmapEditorScenesManager;
    [Inject]
    private readonly BeatmapLevelStarterController _beatmapLevelStarterController;
    [Inject]
    private readonly ScrollInputController _scrollInputController;
    [Inject]
    private readonly ISongPreviewController _songPreviewController;
    [Inject]
    private readonly AudioManagerSO _audioManager;
    [Inject]
    private readonly SimpleEditorDialogViewController _simpleMessageViewController;
    [Inject]
    private readonly LoadingViewController _loadingViewController;
    [Inject]
    private readonly EditBeatmapViewController _editBeatmapViewController;
    [Inject]
    private readonly EditBeatmapNavigationViewController _editBeatmapNavigationViewController;
    [Inject]
    private readonly CopyBeatmapObjectsViewController _copyBeatmapObjectsViewController;
    private readonly KeyboardBinder _keyboardBinder = new KeyboardBinder();
    private BeatmapCharacteristicSO _beatmapCharacteristic;
    private BeatmapDifficulty _beatmapDifficulty;
    private EnvironmentInfoSO _currentEnvironment;

    public event Action<BeatmapFlowCoordinator.SaveType, BeatmapCharacteristicSO, BeatmapDifficulty> didFinishEvent;

    private bool anyDataModelDirty => this._beatmapLevelDataModel.isDirty || this._beatmapBasicEventsDataModel.isDirty || this._beatmapEventBoxGroupsDataModel.isDirty;

    protected void Update() => this._keyboardBinder.ManualUpdate();

    public void Setup(
      BeatmapCharacteristicSO beatmapCharacteristic,
      BeatmapDifficulty beatmapDifficulty)
    {
      this._beatmapCharacteristic = beatmapCharacteristic;
      this._beatmapDifficulty = beatmapDifficulty;
    }

    protected override void DidActivate(
      bool firstActivation,
      bool addedToHierarchy,
      bool screenSystemEnabling)
    {
      if (!addedToHierarchy)
        return;
      BeatmapEditorObjectId.Reset();
      this._editBeatmapNavigationViewController.buttonPressedEvent += new Action<EditBeatmapNavigationViewController.ButtonType>(this.HandleEditBeatmapNavigationViewControllerButtonPressed);
      this._editBeatmapNavigationViewController.toggleChangedEvent += new Action<EditBeatmapNavigationViewController.ToggleType, bool>(this.HandleEditBeatmapNavigationViewControllerToggleValueChanged);
      this._editBeatmapViewController.buttonPressedEvent += new Action<EditBeatmapViewController.ButtonType>(this.HandleEditBeatmapViewControllerButtonPressed);
      this._copyBeatmapObjectsViewController.didFinishEvent += new Action(this.HandleCopyBeatmapObjectViewControllerDidFinish);
      this._signalBus.Fire<BeatmapProjectManagerSignals.LoadBeatmapLevelSignal>(new BeatmapProjectManagerSignals.LoadBeatmapLevelSignal(this._beatmapCharacteristic, this._beatmapDifficulty));
      this._signalBus.Fire<SetAllTracksSignal>(new SetAllTracksSignal(((IEnumerable<EnvironmentTracksDefinitionSO.BasicEventTrackInfo>) this._beatmapDataModel.environmentTrackDefinition.basicEventTrackInfos).Select<EnvironmentTracksDefinitionSO.BasicEventTrackInfo, BasicBeatmapEventType>((Func<EnvironmentTracksDefinitionSO.BasicEventTrackInfo, BasicBeatmapEventType>) (info => info.basicBeatmapEventType)).ToList<BasicBeatmapEventType>()));
      this._currentEnvironment = this._beatmapDataModel.environment;
      this._signalBus.Subscribe<BeatmapLevelUpdatedSignal>(new Action(this.HandleBeatmapLevelUpdatedSignal));
      this._signalBus.Subscribe<BeatmapProjectManagerSignals.BeatmapSaveFailedSignal>(new Action<BeatmapProjectManagerSignals.BeatmapSaveFailedSignal>(this.HandleBeatmapLevelSaveFailed));
      this.showBackButton = true;
      this.SetBackButtonIconType(BackButtonView.BackButtonType.Back);
      this.SetBackButtonIsDirtyNotification(this.anyDataModelDirty);
      this._loadingViewController.Init("Loading Beatmap", "Loading Beatmap", true);
      this.ProvideInitialViewControllers((BeatmapEditorViewController) this._editBeatmapViewController, (BeatmapEditorViewController) this._editBeatmapNavigationViewController, (BeatmapEditorViewController) this._loadingViewController);
      this.BindCommonKeyboardBindings();
      this.StartCoroutine(this.OpenEnvironmentAndPresentEditor());
    }

    protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
    {
      if (!removedFromHierarchy)
        return;
      this._editBeatmapNavigationViewController.buttonPressedEvent -= new Action<EditBeatmapNavigationViewController.ButtonType>(this.HandleEditBeatmapNavigationViewControllerButtonPressed);
      this._editBeatmapNavigationViewController.toggleChangedEvent -= new Action<EditBeatmapNavigationViewController.ToggleType, bool>(this.HandleEditBeatmapNavigationViewControllerToggleValueChanged);
      this._editBeatmapViewController.buttonPressedEvent -= new Action<EditBeatmapViewController.ButtonType>(this.HandleEditBeatmapViewControllerButtonPressed);
      this._copyBeatmapObjectsViewController.didFinishEvent -= new Action(this.HandleCopyBeatmapObjectViewControllerDidFinish);
      this._audioManager.musicSpeed = 1f;
      this._scrollInputController.Disable();
      this._beatmapEditorScenesManager.CloseEnvironment(this._currentEnvironment);
      this._currentEnvironment = (EnvironmentInfoSO) null;
      this._keyboardBinder.ClearBindings();
      this._signalBus.TryUnsubscribe<BeatmapLevelUpdatedSignal>(new Action(this.HandleBeatmapLevelUpdatedSignal));
      this._signalBus.TryUnsubscribe<BeatmapProjectManagerSignals.BeatmapSaveFailedSignal>(new Action<BeatmapProjectManagerSignals.BeatmapSaveFailedSignal>(this.HandleBeatmapLevelSaveFailed));
      this._signalBus.Fire<BeatmapProjectManagerSignals.SaveSettingsSignal>();
      this._signalBus.Fire<ClearSelectionSignal>();
      this._signalBus.Fire<ClearEditorHistorySignal>();
      this._basicEventsState.Reset();
      this._beatmapState.Reset();
      this._readonlyBeatmapObjectsState.Reset();
      this._eventBoxGroupsState.Reset();
    }

    protected override void BackButtonWasPressed(BeatmapEditorViewController topViewController)
    {
      if ((UnityEngine.Object) topViewController != (UnityEngine.Object) this._editBeatmapViewController)
        return;
      if (this.anyDataModelDirty)
        this._simpleMessageViewController.Init("Quit Editing", "You have unsaved changes, do you want to save them?", "Save and quit", "No and quit", "No and stay", (Action<int>) (buttonIdx =>
        {
          this.SetDialogScreenViewController((BeatmapEditorViewController) null);
          if (buttonIdx == 2)
            return;
          Action<BeatmapFlowCoordinator.SaveType, BeatmapCharacteristicSO, BeatmapDifficulty> didFinishEvent = this.didFinishEvent;
          if (didFinishEvent == null)
            return;
          didFinishEvent(buttonIdx == 0 ? BeatmapFlowCoordinator.SaveType.Save : BeatmapFlowCoordinator.SaveType.DontSave, this._beatmapCharacteristic, this._beatmapDifficulty);
        }));
      else
        this._simpleMessageViewController.Init("Quit Editing", "Do you want to quit?", "Yes", "No", (Action<int>) (buttonIdx =>
        {
          this.SetDialogScreenViewController((BeatmapEditorViewController) null);
          if (buttonIdx != 0)
            return;
          Action<BeatmapFlowCoordinator.SaveType, BeatmapCharacteristicSO, BeatmapDifficulty> didFinishEvent = this.didFinishEvent;
          if (didFinishEvent == null)
            return;
          didFinishEvent(BeatmapFlowCoordinator.SaveType.DontSave, this._beatmapCharacteristic, this._beatmapDifficulty);
        }));
      this.SetDialogScreenViewController((BeatmapEditorViewController) this._simpleMessageViewController);
    }

    private void HandleEditBeatmapNavigationViewControllerButtonPressed(
      EditBeatmapNavigationViewController.ButtonType buttonType)
    {
      switch (buttonType)
      {
        case EditBeatmapNavigationViewController.ButtonType.Objects:
          this._signalBus.Fire<SwitchBeatmapEditingModeSignal>(new SwitchBeatmapEditingModeSignal(BeatmapEditingMode.Objects));
          break;
        case EditBeatmapNavigationViewController.ButtonType.EventGroups:
          this._signalBus.Fire<SwitchBeatmapEditingModeSignal>(new SwitchBeatmapEditingModeSignal(BeatmapEditingMode.EventBoxGroups));
          break;
        case EditBeatmapNavigationViewController.ButtonType.Events:
          this._signalBus.Fire<SwitchBeatmapEditingModeSignal>(new SwitchBeatmapEditingModeSignal(BeatmapEditingMode.BasicEvents));
          break;
        case EditBeatmapNavigationViewController.ButtonType.TestLevel:
          this.StartCoroutine(this.StartTestLevel());
          break;
        case EditBeatmapNavigationViewController.ButtonType.CopyFromDifficulty:
          this.SetDialogScreenViewController((BeatmapEditorViewController) this._copyBeatmapObjectsViewController);
          break;
      }
    }

    private void HandleEditBeatmapNavigationViewControllerToggleValueChanged(
      EditBeatmapNavigationViewController.ToggleType toggleType,
      bool isOn)
    {
      if (toggleType != EditBeatmapNavigationViewController.ToggleType.EnableFPFC)
        return;
      this._signalBus.Fire<ToggleFpfcSignal>(new ToggleFpfcSignal()
      {
        enableFpfc = isOn
      });
    }

    private void HandleBeatmapLevelUpdatedSignal()
    {
      this.SetBackButtonIsDirtyNotification(this.anyDataModelDirty);
      if (this._beatmapState.editingMode == BeatmapEditingMode.EventBoxes && this._beatmapEventBoxGroupsDataModel.GetEventBoxGroupById(this._eventBoxGroupsState.eventBoxGroupContext.id) == (EventBoxGroupEditorData) null)
        this._signalBus.Fire<ExitEditEventBoxGroupSignal>();
      this._signalBus.Fire<BeatmapProjectManagerSignals.SaveBeatmapLevelToTempSignal>();
    }

    private void HandleBeatmapLevelSaveFailed(
      BeatmapProjectManagerSignals.BeatmapSaveFailedSignal signal)
    {
      this._simpleMessageViewController.Init("Save Failed", "Unable to save beatmap\nReason: " + signal.reason + "\n(You can loose progress if you decide to ignore it)", "Ignore", "Quit", (Action<int>) (buttonIdx =>
      {
        this.SetDialogScreenViewController((BeatmapEditorViewController) null);
        if (buttonIdx != 1)
          return;
        Action<BeatmapFlowCoordinator.SaveType, BeatmapCharacteristicSO, BeatmapDifficulty> didFinishEvent = this.didFinishEvent;
        if (didFinishEvent == null)
          return;
        didFinishEvent(BeatmapFlowCoordinator.SaveType.Error, this._beatmapCharacteristic, this._beatmapDifficulty);
      }));
      this.SetDialogScreenViewController((BeatmapEditorViewController) this._simpleMessageViewController);
    }

    private void HandleBeatmapLevelStarterControllerLevelFinished(
      BeatmapEditorStandardLevelScenesTransitionSetupDataSO scenesTransitionSetupData,
      LevelCompletionResults levelCompletionResults)
    {
      if (levelCompletionResults.levelEndAction == LevelCompletionResults.LevelEndAction.Restart)
      {
        this.StartCoroutine(this.StartTestLevel(false));
      }
      else
      {
        if (this._beatmapEditorSettingsDataModel.preserveTime)
          this._signalBus.Fire<UpdatePlayHeadSignal>(new UpdatePlayHeadSignal(AudioTimeHelper.SecondsToSamples(levelCompletionResults.endSongTime, this._beatmapDataModel.audioClip.frequency), UpdatePlayHeadSignal.SnapType.CurrentSubdivision, false));
        this._audioManager.musicSpeed = this._beatmapEditorSettingsDataModel.playbackSpeed;
        this._beatmapEditorScenesManager.OpenEnvironment(this._currentEnvironment);
      }
    }

    private void HandleEditBeatmapViewControllerButtonPressed(
      EditBeatmapViewController.ButtonType type)
    {
      if (type != EditBeatmapViewController.ButtonType.CopyFromDifficulty)
        return;
      this.SetDialogScreenViewController((BeatmapEditorViewController) this._copyBeatmapObjectsViewController);
    }

    private void HandleCopyBeatmapObjectViewControllerDidFinish() => this.SetDialogScreenViewController((BeatmapEditorViewController) null);

    private void HandleKeyboardBinderSpace(bool pressed)
    {
      if (this._beatmapState.cameraMoving)
        return;
      this.TogglePlayPause();
    }

    private void HandleKeyboardBinderReplay(bool pressed)
    {
      if (!Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.LeftAlt) || !this._songPreviewController.isPlaying)
        return;
      this._signalBus.Fire<TimeControlsReplaySignal>();
    }

    private void HandleKeyboardBinderSave(bool pressed)
    {
      if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
        return;
      this._signalBus.Fire<BeatmapProjectManagerSignals.SaveBeatmapLevelSignal>();
      this.SetBackButtonIsDirtyNotification(this.anyDataModelDirty);
    }

    private void BindCommonKeyboardBindings()
    {
      this._keyboardBinder.AddBinding(KeyCode.Space, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandleKeyboardBinderSpace));
      this._keyboardBinder.AddBinding(KeyCode.R, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandleKeyboardBinderReplay));
      this._keyboardBinder.AddBinding(KeyCode.S, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandleKeyboardBinderSave));
      this._keyboardBinder.AddBinding(KeyCode.F1, KeyboardBinder.KeyBindingType.KeyDown, this.GetChangeLevelEditModeHandler(BeatmapEditingMode.Objects));
      this._keyboardBinder.AddBinding(KeyCode.F2, KeyboardBinder.KeyBindingType.KeyDown, this.GetChangeLevelEditModeHandler(BeatmapEditingMode.EventBoxGroups));
      this._keyboardBinder.AddBinding(KeyCode.F3, KeyboardBinder.KeyBindingType.KeyDown, this.GetChangeLevelEditModeHandler(BeatmapEditingMode.BasicEvents));
    }

    private void TogglePlayPause()
    {
      if (this._songPreviewController.isPlaying)
        this._signalBus.Fire<TimeControlsPauseSignal>();
      else
        this._signalBus.Fire<TimeControlsPlaySignal>(new TimeControlsPlaySignal(this._beatmapState.beat));
    }

    private Action<bool> GetChangeLevelEditModeHandler(BeatmapEditingMode levelEditMode) => (Action<bool>) (pressed => this._signalBus.Fire<SwitchBeatmapEditingModeSignal>(new SwitchBeatmapEditingModeSignal(levelEditMode)));

    private IEnumerator OpenEnvironmentAndPresentEditor()
    {
      // ISSUE: reference to a compiler-generated field
      int num = this.m_Cm_E1__state;
      BeatmapFlowCoordinator beatmapFlowCoordinator = this;
      if (num != 0)
      {
        if (num != 1)
          return false;
        // ISSUE: reference to a compiler-generated field
        this.m_Cm_E1__state = -1;
        beatmapFlowCoordinator._scrollInputController.Enable(beatmapFlowCoordinator._beatmapDataModel.bpmData.BeatToSample(beatmapFlowCoordinator._beatmapState.beat + 16f) - beatmapFlowCoordinator._beatmapDataModel.bpmData.BeatToSample(beatmapFlowCoordinator._beatmapState.beat));
        beatmapFlowCoordinator._signalBus.Fire<UpdatePlayHeadSignal>(new UpdatePlayHeadSignal(beatmapFlowCoordinator._beatmapState.beat, UpdatePlayHeadSignal.SnapType.SmallestSubdivision));
        beatmapFlowCoordinator.SetDialogScreenViewController((BeatmapEditorViewController) null);
        return false;
      }
      // ISSUE: reference to a compiler-generated field
      this.m_Cm_E1__state = -1;
      // ISSUE: reference to a compiler-generated field
      this.m_Cm_E2__current = (object) beatmapFlowCoordinator._beatmapEditorScenesManager.OpenEnvironmentDelayed(beatmapFlowCoordinator._currentEnvironment);
      // ISSUE: reference to a compiler-generated field
      this.m_Cm_E1__state = 1;
      return true;
    }

    private IEnumerator StartTestLevel(bool closeEnvironment = true)
    {
      BeatmapFlowCoordinator beatmapFlowCoordinator = this;
      if (closeEnvironment)
        yield return (object) beatmapFlowCoordinator._beatmapEditorScenesManager.CloseEnvironmentDelayed(beatmapFlowCoordinator._currentEnvironment);
      beatmapFlowCoordinator._audioManager.musicSpeed = 1f;
      beatmapFlowCoordinator._beatmapLevelStarterController.TestBeatmap(new Action<BeatmapEditorStandardLevelScenesTransitionSetupDataSO, LevelCompletionResults>(beatmapFlowCoordinator.HandleBeatmapLevelStarterControllerLevelFinished));
    }

    public enum SaveType
    {
      DontSave,
      Save,
      Error,
    }
  }
}
