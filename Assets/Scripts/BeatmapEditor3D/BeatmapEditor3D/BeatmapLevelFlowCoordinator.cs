// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapLevelFlowCoordinator
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using HMUI;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class BeatmapLevelFlowCoordinator : BeatmapEditorFlowCoordinator
  {
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly BeatmapFlowCoordinator _beatmapFlowCoordinator;
    [Inject]
    private readonly BpmEditorFlowCoordinator _bpmEditorFlowCoordinator;
    [Inject]
    private readonly EditBeatmapLevelNavigationViewController _editBeatmapLevelNavigationViewController;
    [Inject]
    private readonly EditBeatmapLevelViewController _editBeatmapViewController;
    [Inject]
    private readonly LoadingViewController _loadingViewController;
    [Inject]
    private readonly OpenTemporaryProjectViewController _openTemporaryProjectViewController;
    [Inject]
    private readonly SimpleEditorDialogViewController _dialogViewController;
    [Inject]
    private readonly BeatmapProjectManager _beatmapProjectManager;
    [Inject]
    private readonly BeatmapDataModel _beatmapDataModel;
    [InjectOptional]
    private OpenBeatmapLevelDestination _openBeatmapLevelDestination;
    private readonly KeyboardBinder _keyboardBinder = new KeyboardBinder();
    private IBeatmapInfoData _beatmapInfoData;

    public event Action didFinishEvent;

    protected void Update() => this._keyboardBinder.ManualUpdate();

    public void SetupProject(IBeatmapInfoData beatmapInfoData) => this._beatmapInfoData = beatmapInfoData;

    protected override void DidActivate(
      bool firstActivation,
      bool addedToHierarchy,
      bool screenSystemEnabling)
    {
      OpenBeatmapLevelDestination levelDestination = this._openBeatmapLevelDestination;
      bool flag = levelDestination != null && levelDestination.ignoreTemp;
      if (addedToHierarchy)
      {
        this._beatmapFlowCoordinator.didFinishEvent += new Action<BeatmapFlowCoordinator.SaveType, BeatmapCharacteristicSO, BeatmapDifficulty>(this.HandleBeatmapFlowCoordinatorDidFinish);
        this._bpmEditorFlowCoordinator.didFinishEvent += new Action<bool>(this.HandleBpmEditorFlowCoordinatorDidFinish);
        int num = !this._beatmapProjectManager.ProjectTempExists(this._beatmapInfoData.beatmapFolderPath) ? 0 : (!flag ? 1 : 0);
        this._loadingViewController.Init("Loading Beatmap", this._beatmapInfoData.songName + "\n\n" + this._beatmapInfoData.beatmapFolderPath, true);
        BeatmapEditorViewController dialogScreenViewController = (BeatmapEditorViewController) this._loadingViewController;
        if (num != 0)
        {
          this._openTemporaryProjectViewController.Init(BeatmapFileUtils.GetDirectoryModifiedDateTime(BeatmapFileUtils.GetTempBeatmapDirectoryPath(this._beatmapInfoData.beatmapFolderPath)), BeatmapFileUtils.GetDirectoryModifiedDateTime(this._beatmapInfoData.beatmapFolderPath), new Action<bool>(this.HandleOpenTemporaryProjectViewControllerAction));
          dialogScreenViewController = (BeatmapEditorViewController) this._openTemporaryProjectViewController;
        }
        this._editBeatmapLevelNavigationViewController.SetDeleteMode(false);
        this._editBeatmapViewController.SetDeleteMode(false);
        this._editBeatmapViewController.deleteDifficultyBeatmapEvent += new Action<BeatmapCharacteristicSO, BeatmapDifficulty>(this.HandleDeleteDifficultyBeatmapEvent);
        this._keyboardBinder.AddBinding(KeyCode.S, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandleKeyboardBinderSave));
        this.showBackButton = true;
        this.SetBackButtonIconType(BackButtonView.BackButtonType.Back);
        this.ProvideInitialViewControllers((BeatmapEditorViewController) this._editBeatmapViewController, (BeatmapEditorViewController) this._editBeatmapLevelNavigationViewController, dialogScreenViewController);
      }
      this._signalBus.Subscribe<BeatmapDataModelSignals.BeatmapLoadedSignal>(new Action(this.HandleBeatmapLoaded));
      this._signalBus.Subscribe<BeatmapDataModelSignals.BeatmapLoadErrorSignal>(new Action<BeatmapDataModelSignals.BeatmapLoadErrorSignal>(this.HandleBeatmapLoadError));
      this._signalBus.Subscribe<BeatmapDataModelSignals.DifficultyBeatmapAddedSignal>(new Action<BeatmapDataModelSignals.DifficultyBeatmapAddedSignal>(this.HandleBeatmapDataModelDifficultyBeatmapAdded));
      this._signalBus.Subscribe<BeatmapDataModelSignals.BeatmapUpdatedSignal>(new Action(this.HandleBeatmapUpdated));
      this._signalBus.Subscribe<BeatmapProjectManagerSignals.BeatmapSaveFailedSignal>(new Action<BeatmapProjectManagerSignals.BeatmapSaveFailedSignal>(this.HandleBeatmapSaveFailed));
      this._editBeatmapViewController.openBeatmapLevelEvent += new Action<BeatmapCharacteristicSO, BeatmapDifficulty>(this.HandleEditBeatmapViewControllerOpenBeatmapLevel);
      this._editBeatmapViewController.editBpmGridEvent += new Action(this.HandleEditBeatmapViewControllerEditBpmGrid);
      this._editBeatmapLevelNavigationViewController.deleteModeChangedEvent += new Action<bool>(this.HandleEditBeatmapLevelNavigationViewControllerDeleteModeChanged);
      this._editBeatmapLevelNavigationViewController.SetDeleteMode(false);
      this._editBeatmapViewController.SetDeleteMode(false);
      this._keyboardBinder.AddBinding(KeyCode.S, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandleKeyboardBinderSave));
      this.SetBackButtonIsDirtyNotification(this._beatmapDataModel.isDirty);
      if (!firstActivation || this._openBeatmapLevelDestination == null || !this._beatmapDataModel.beatmapDataLoaded)
        return;
      this.StartCoroutine(this.ProcessMenuDestinationRequestAfterFrameCoroutine(this._openBeatmapLevelDestination));
      this._openBeatmapLevelDestination = (OpenBeatmapLevelDestination) null;
    }

    protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
    {
      this._signalBus.TryUnsubscribe<BeatmapProjectManagerSignals.BeatmapSaveFailedSignal>(new Action<BeatmapProjectManagerSignals.BeatmapSaveFailedSignal>(this.HandleBeatmapSaveFailed));
      this._signalBus.TryUnsubscribe<BeatmapDataModelSignals.BeatmapLoadedSignal>(new Action(this.HandleBeatmapLoaded));
      this._signalBus.TryUnsubscribe<BeatmapDataModelSignals.BeatmapLoadErrorSignal>(new Action<BeatmapDataModelSignals.BeatmapLoadErrorSignal>(this.HandleBeatmapLoadError));
      this._signalBus.TryUnsubscribe<BeatmapDataModelSignals.DifficultyBeatmapAddedSignal>(new Action<BeatmapDataModelSignals.DifficultyBeatmapAddedSignal>(this.HandleBeatmapDataModelDifficultyBeatmapAdded));
      this._signalBus.TryUnsubscribe<BeatmapDataModelSignals.BeatmapUpdatedSignal>(new Action(this.HandleBeatmapUpdated));
      this._editBeatmapLevelNavigationViewController.deleteModeChangedEvent -= new Action<bool>(this.HandleEditBeatmapLevelNavigationViewControllerDeleteModeChanged);
      this._editBeatmapViewController.openBeatmapLevelEvent -= new Action<BeatmapCharacteristicSO, BeatmapDifficulty>(this.HandleEditBeatmapViewControllerOpenBeatmapLevel);
      this._editBeatmapViewController.editBpmGridEvent -= new Action(this.HandleEditBeatmapViewControllerEditBpmGrid);
      this._editBeatmapViewController.deleteDifficultyBeatmapEvent -= new Action<BeatmapCharacteristicSO, BeatmapDifficulty>(this.HandleDeleteDifficultyBeatmapEvent);
      if (!removedFromHierarchy)
        return;
      this._beatmapFlowCoordinator.didFinishEvent -= new Action<BeatmapFlowCoordinator.SaveType, BeatmapCharacteristicSO, BeatmapDifficulty>(this.HandleBeatmapFlowCoordinatorDidFinish);
      this._bpmEditorFlowCoordinator.didFinishEvent -= new Action<bool>(this.HandleBpmEditorFlowCoordinatorDidFinish);
      this._signalBus.Fire<BeatmapProjectManagerSignals.CloseBeatmapProjectSignal>();
    }

    protected override void InitialViewControllerWasPresented()
    {
      OpenBeatmapLevelDestination levelDestination = this._openBeatmapLevelDestination;
      bool flag = levelDestination != null && levelDestination.ignoreTemp;
      if ((!this._beatmapProjectManager.ProjectTempExists(this._beatmapInfoData.beatmapFolderPath) ? 0 : (!flag ? 1 : 0)) != 0)
        return;
      this._signalBus.Fire<BeatmapProjectManagerSignals.LoadBeatmapProjectSignal>(new BeatmapProjectManagerSignals.LoadBeatmapProjectSignal(this._beatmapInfoData.beatmapFolderPath, false));
    }

    protected override void BackButtonWasPressed(BeatmapEditorViewController topViewController)
    {
      if ((UnityEngine.Object) topViewController != (UnityEngine.Object) this._editBeatmapViewController)
        this.DismissViewController(topViewController);
      else if (this._beatmapDataModel.isDirty)
      {
        this._dialogViewController.Init("Quit Editing", "You have unsaved changes, do you want to save them?", "Save and quit", "No and quit", "No and stay", (Action<int>) (buttonIdx =>
        {
          this.SetDialogScreenViewController((BeatmapEditorViewController) null);
          switch (buttonIdx)
          {
            case 0:
              this._signalBus.Fire<BeatmapProjectManagerSignals.SaveBeatmapProjectSignal>();
              break;
            case 1:
              this._signalBus.Fire<BeatmapProjectManagerSignals.LoadBeatmapProjectFromLastSaveSignal>();
              break;
          }
          this._signalBus.Fire<BeatmapsCollectionSignals.RefreshSignal>();
          if (buttonIdx == 2)
            return;
          Action didFinishEvent = this.didFinishEvent;
          if (didFinishEvent == null)
            return;
          didFinishEvent();
        }));
        this.SetDialogScreenViewController((BeatmapEditorViewController) this._dialogViewController);
      }
      else
      {
        Action didFinishEvent = this.didFinishEvent;
        if (didFinishEvent == null)
          return;
        didFinishEvent();
      }
    }

    private void HandleBeatmapLoaded()
    {
      this.SetDialogScreenViewController((BeatmapEditorViewController) null);
      if (this._beatmapDataModel.beatmapDataLoaded && this._openBeatmapLevelDestination != null)
      {
        this.StartCoroutine(this.ProcessMenuDestinationRequestAfterFrameCoroutine(this._openBeatmapLevelDestination));
        this._openBeatmapLevelDestination = (OpenBeatmapLevelDestination) null;
      }
      this._signalBus.TryUnsubscribe<BeatmapDataModelSignals.BeatmapLoadedSignal>(new Action(this.HandleBeatmapLoaded));
    }

    private void HandleBeatmapLoadError(
      BeatmapDataModelSignals.BeatmapLoadErrorSignal signal)
    {
      string str = signal.errorType.ToString();
      if (signal.errorType == BeatmapDataModelSignals.BeatmapLoadErrorSignal.ErrorType.UnableToLoadAudio)
        str = "Unable to load audio";
      this._dialogViewController.Init("Unable to open Beatmap Level", str ?? "", "Close", (Action<int>) (_ =>
      {
        Action didFinishEvent = this.didFinishEvent;
        if (didFinishEvent == null)
          return;
        didFinishEvent();
      }));
      this.SetDialogScreenViewController((BeatmapEditorViewController) this._dialogViewController);
    }

    private void HandleBeatmapDataModelDifficultyBeatmapAdded(
      BeatmapDataModelSignals.DifficultyBeatmapAddedSignal signal)
    {
      if (this._beatmapDataModel.isDirty)
      {
        this.SetDialogViewControllerUnsavedChanges((Action) (() => this.OpenBeatmapLevel(signal.beatmapCharacteristic, signal.difficulty)));
        this.SetDialogScreenViewController((BeatmapEditorViewController) this._dialogViewController);
      }
      else
        this.OpenBeatmapLevel(signal.beatmapCharacteristic, signal.difficulty);
    }

    private void HandleEditBeatmapViewControllerOpenBeatmapLevel(
      BeatmapCharacteristicSO beatmapCharacteristic,
      BeatmapDifficulty beatmapDifficulty)
    {
      if (this._beatmapDataModel.isDirty)
      {
        this.SetDialogViewControllerUnsavedChanges((Action) (() => this.OpenBeatmapLevel(beatmapCharacteristic, beatmapDifficulty)));
        this.SetDialogScreenViewController((BeatmapEditorViewController) this._dialogViewController);
      }
      else
        this.OpenBeatmapLevel(beatmapCharacteristic, beatmapDifficulty);
    }

    private void HandleEditBeatmapViewControllerEditBpmGrid()
    {
      if (this._beatmapDataModel.isDirty)
      {
        this.SetDialogViewControllerUnsavedChanges((Action) (() => this.PresentFlowCoordinator((BeatmapEditorFlowCoordinator) this._bpmEditorFlowCoordinator, false)));
        this.SetDialogScreenViewController((BeatmapEditorViewController) this._dialogViewController);
      }
      else
        this.PresentFlowCoordinator((BeatmapEditorFlowCoordinator) this._bpmEditorFlowCoordinator, false);
    }

    private void HandleBeatmapFlowCoordinatorDidFinish(
      BeatmapFlowCoordinator.SaveType saveType,
      BeatmapCharacteristicSO beatmapCharacteristic,
      BeatmapDifficulty beatmapDifficulty)
    {
      switch (saveType)
      {
        case BeatmapFlowCoordinator.SaveType.DontSave:
          this._signalBus.Fire<BeatmapProjectManagerSignals.LoadBeatmapLevelFromLastSaveSignal>(new BeatmapProjectManagerSignals.LoadBeatmapLevelFromLastSaveSignal(beatmapCharacteristic, beatmapDifficulty));
          break;
        case BeatmapFlowCoordinator.SaveType.Save:
          this._signalBus.Fire<BeatmapProjectManagerSignals.SaveBeatmapLevelSignal>();
          break;
      }
      this._signalBus.Fire<BeatmapProjectManagerSignals.CloseBeatmapLevelSignal>();
      this.DismissFlowCoordinator((BeatmapEditorFlowCoordinator) this._beatmapFlowCoordinator);
    }

    private void HandleBpmEditorFlowCoordinatorDidFinish(bool saveDataModel)
    {
      if (saveDataModel)
        this._signalBus.Fire<BeatmapProjectManagerSignals.SaveBpmInfoSignal>();
      this._signalBus.Fire<BeatmapProjectManagerSignals.CloseBpmInfoSignal>();
      this._signalBus.Fire<BeatmapProjectManagerSignals.SaveBeatmapProjectSignal>();
      this.DismissFlowCoordinator((BeatmapEditorFlowCoordinator) this._bpmEditorFlowCoordinator);
    }

    private void HandleOpenTemporaryProjectViewControllerAction(bool yes)
    {
      this.SetDialogScreenViewController((BeatmapEditorViewController) this._loadingViewController);
      this._signalBus.Fire<BeatmapProjectManagerSignals.LoadBeatmapProjectSignal>(new BeatmapProjectManagerSignals.LoadBeatmapProjectSignal(this._beatmapInfoData.beatmapFolderPath, yes));
    }

    private void HandleEditBeatmapLevelNavigationViewControllerDeleteModeChanged(bool deleteEnabled) => this._editBeatmapViewController.SetDeleteMode(deleteEnabled);

    private void HandleBeatmapUpdated()
    {
      this.SetBackButtonIsDirtyNotification(this._beatmapDataModel.isDirty);
      this._signalBus.Fire<BeatmapProjectManagerSignals.SaveBeatmapProjectToTempSignal>();
    }

    private void HandleBeatmapSaveFailed(
      BeatmapProjectManagerSignals.BeatmapSaveFailedSignal signal)
    {
      this._dialogViewController.Init("Save Failed", "Unable to save beatmap\nReason: " + signal.reason + "\n(You can loose progress if you decide to ignore it)", "Ignore", "Quit", (Action<int>) (buttonIdx =>
      {
        this.SetDialogScreenViewController((BeatmapEditorViewController) null);
        if (buttonIdx != 1)
          return;
        Action didFinishEvent = this.didFinishEvent;
        if (didFinishEvent == null)
          return;
        didFinishEvent();
      }));
      this.SetDialogScreenViewController((BeatmapEditorViewController) this._dialogViewController);
    }

    private void HandleKeyboardBinderSave(bool _)
    {
      if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
        return;
      this._signalBus.Fire<BeatmapProjectManagerSignals.SaveBeatmapProjectSignal>();
      this.SetBackButtonIsDirtyNotification(false);
    }

    private void HandleDeleteDifficultyBeatmapEvent(
      BeatmapCharacteristicSO beatmapCharacteristic,
      BeatmapDifficulty beatmapDifficulty)
    {
      this._dialogViewController.Init("Delete Beatmap Difficulty", "Are you sure you want to delete beatmap difficulty?", "Yes", "No", (Action<int>) (buttonIdx =>
      {
        this.SetDialogScreenViewController((BeatmapEditorViewController) null);
        if (buttonIdx != 0)
          return;
        this._signalBus.Fire<BeatmapDataModelSignals.RemoveDifficultyBeatmapSignal>(new BeatmapDataModelSignals.RemoveDifficultyBeatmapSignal(beatmapCharacteristic, beatmapDifficulty));
      }));
      this.SetDialogScreenViewController((BeatmapEditorViewController) this._dialogViewController);
    }

    private void SetDialogViewControllerUnsavedChanges(Action continueAction) => this._dialogViewController.Init("Save Changes", "You have unsaved changes, do you want to save them?", "Yes and continue", "No and stay", "Dismiss and stay", (Action<int>) (buttonIdx =>
    {
      this.SetDialogScreenViewController((BeatmapEditorViewController) null);
      switch (buttonIdx)
      {
        case 0:
          this._signalBus.Fire<BeatmapProjectManagerSignals.SaveBeatmapProjectSignal>();
          Action action = continueAction;
          if (action == null)
            break;
          action();
          break;
        case 2:
          this._signalBus.Fire<BeatmapProjectManagerSignals.LoadBeatmapProjectFromLastSaveSignal>();
          break;
      }
    }));

    private void OpenBeatmapLevel(
      BeatmapCharacteristicSO beatmapCharacteristic,
      BeatmapDifficulty beatmapDifficulty)
    {
      this._beatmapFlowCoordinator.Setup(beatmapCharacteristic, beatmapDifficulty);
      this.PresentFlowCoordinator((BeatmapEditorFlowCoordinator) this._beatmapFlowCoordinator, false);
    }

    private IEnumerator ProcessMenuDestinationRequestAfterFrameCoroutine(
      OpenBeatmapLevelDestination destination)
    {
      yield return (object) null;
      this.OpenBeatmapLevel(destination.beatmapCharacteristic, destination.beatmapDifficulty);
    }
  }
}
