// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.MainBeatmapEditorFlowCoordinator
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace BeatmapEditor3D
{
  public class MainBeatmapEditorFlowCoordinator : BeatmapEditorFlowCoordinator
  {
    [SerializeField]
    private MainSettingsModelSO _mainSettingsModel;
    [SerializeField]
    private BeatmapEditorScenesTransitionSetupDataSO _beatmapEditorScenesTransitionSetupData;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly BeatmapEditorSettingsDataModel _beatmapEditorSettingsDataModel;
    [Inject]
    private readonly BeatmapLevelFlowCoordinator _beatmapLevelFlowCoordinator;
    [Inject]
    private readonly BeatmapEditorMainNavigationViewController _beatmapEditorMainNavigationViewController;
    [Inject]
    private readonly BeatmapsListViewController _beatmapsListViewController;
    [Inject]
    private readonly NewBeatmapViewController _newBeatmapViewController;
    [Inject]
    private readonly BeatmapEditorSettingsViewController _settingsViewController;
    [Inject]
    private readonly SimpleEditorDialogViewController _simpleEditorDialogViewController;
    [Inject]
    private readonly IBeatmapCollectionDataModel _beatmapCollectionDataModel;
    [InjectOptional]
    private OpenBeatmapLevelDestination _openBeatmapLevelDestination;
    private GarbageCollector.Mode _prevGCMode;

    protected override void DidActivate(
      bool firstActivation,
      bool addedToHierarchy,
      bool screenSystemEnabling)
    {
      this.SetBackButtonIconType(this._beatmapEditorScenesTransitionSetupData.goStraightToEditor ? BackButtonView.BackButtonType.Quit : BackButtonView.BackButtonType.Back);
      this.SetBackButtonIsDirtyNotification(false);
      if (addedToHierarchy)
      {
        this._signalBus.Fire<BeatmapProjectManagerSignals.LoadSettingsSignal>();
        this._signalBus.Fire<BeatmapsCollectionSignals.RefreshSignal>();
        this.SetScreenResolution();
        this._prevGCMode = GarbageCollector.GCMode;
        GarbageCollector.GCMode = GarbageCollector.Mode.Enabled;
        GarbageCollector.GCModeChanged += new Action<GarbageCollector.Mode>(this.HandleGarbageCollectorGCModeChanged);
        this._beatmapLevelFlowCoordinator.didFinishEvent += new Action(this.HandleBeatmapLevelFlowCoordinatorDidFinish);
        this._beatmapEditorMainNavigationViewController.Setup(BeatmapEditorMainNavigationViewController.EditorControlsButtonType.BeatmapList);
        this._beatmapEditorMainNavigationViewController.buttonWasPressed += new Action<BeatmapEditorMainNavigationViewController.EditorControlsButtonType>(this.HandleBeatmapEditorMainNavigationViewControllerButtonWasPressed);
        this._newBeatmapViewController.saveBeatmapEvent += new Action<(string, string, float, string, string), bool>(this.HandleNewBeatmapViewControllerSaveBeatmap);
        this._beatmapsListViewController.openBeatmapEvent += new Action<IBeatmapInfoData>(this.HandleBeatmapsListViewControllerOpenBeatmap);
        this._signalBus.Subscribe<BeatmapsCollectionSignals.BeatmapAddedSignal>(new Action<BeatmapsCollectionSignals.BeatmapAddedSignal>(this.HandleBeatmapsCollectionBeatmapAddedSignal));
        this.showBackButton = true;
        this.ProvideInitialViewControllers((BeatmapEditorViewController) this._beatmapsListViewController, (BeatmapEditorViewController) this._beatmapEditorMainNavigationViewController);
      }
      if (!firstActivation || this._openBeatmapLevelDestination == null)
        return;
      this.StartCoroutine(this.ProcessMenuDestinationRequestAfterFrameCoroutine((MenuDestination) this._openBeatmapLevelDestination));
      this._openBeatmapLevelDestination = (OpenBeatmapLevelDestination) null;
    }

    protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
    {
      if (!removedFromHierarchy)
        return;
      this._beatmapLevelFlowCoordinator.didFinishEvent -= new Action(this.HandleBeatmapLevelFlowCoordinatorDidFinish);
      this._beatmapEditorMainNavigationViewController.buttonWasPressed -= new Action<BeatmapEditorMainNavigationViewController.EditorControlsButtonType>(this.HandleBeatmapEditorMainNavigationViewControllerButtonWasPressed);
      this._newBeatmapViewController.saveBeatmapEvent -= new Action<(string, string, float, string, string), bool>(this.HandleNewBeatmapViewControllerSaveBeatmap);
      this._beatmapsListViewController.openBeatmapEvent -= new Action<IBeatmapInfoData>(this.HandleBeatmapsListViewControllerOpenBeatmap);
      Vector2Int windowResolution = (Vector2Int) (ObservableVariableSO<Vector2Int>) this._mainSettingsModel.windowResolution;
      if (Screen.width != windowResolution.x || Screen.height != windowResolution.y)
        Screen.SetResolution(windowResolution.x, windowResolution.y, false);
      Screen.fullScreen = (bool) (ObservableVariableSO<bool>) this._mainSettingsModel.fullscreen;
      GarbageCollector.GCModeChanged -= new Action<GarbageCollector.Mode>(this.HandleGarbageCollectorGCModeChanged);
      GarbageCollector.GCMode = this._prevGCMode;
    }

    protected override void BackButtonWasPressed(BeatmapEditorViewController topViewController)
    {
      if (this._beatmapEditorScenesTransitionSetupData.goStraightToEditor)
        Application.Quit();
      else
        this._beatmapEditorScenesTransitionSetupData.Finish();
    }

    private void HandleBeatmapLevelFlowCoordinatorDidFinish() => this.DismissFlowCoordinator((BeatmapEditorFlowCoordinator) this._beatmapLevelFlowCoordinator);

    private void HandleNewBeatmapViewControllerSaveBeatmap(
      (string, string, float, string, string) data,
      bool shouldOpen)
    {
      (string songName, string customBeatmapName, float bpm, string coverImagePath, string songPath) = data;
      this._signalBus.Fire<BeatmapsCollectionSignals.AddNewBeatmapSignal>(new BeatmapsCollectionSignals.AddNewBeatmapSignal(songName, customBeatmapName, coverImagePath, songPath, bpm, shouldOpen));
    }

    private void HandleBeatmapsListViewControllerOpenBeatmap(IBeatmapInfoData beatmapInfoData) => this.OpenBeatmapProject(beatmapInfoData);

    private void HandleBeatmapEditorMainNavigationViewControllerButtonWasPressed(
      BeatmapEditorMainNavigationViewController.EditorControlsButtonType button)
    {
      BeatmapEditorViewController topViewController = this.topViewController;
      switch (button)
      {
        case BeatmapEditorMainNavigationViewController.EditorControlsButtonType.BeatmapList:
          this.ReplaceTopViewController((BeatmapEditorViewController) this._beatmapsListViewController);
          break;
        case BeatmapEditorMainNavigationViewController.EditorControlsButtonType.NewBeatmap:
          this.ReplaceTopViewController((BeatmapEditorViewController) this._newBeatmapViewController);
          break;
        case BeatmapEditorMainNavigationViewController.EditorControlsButtonType.Settings:
          this.ReplaceTopViewController((BeatmapEditorViewController) this._settingsViewController);
          break;
      }
      if (!((UnityEngine.Object) topViewController == (UnityEngine.Object) this._settingsViewController))
        return;
      this._signalBus.Fire<BeatmapProjectManagerSignals.SaveSettingsSignal>();
      this._signalBus.Fire<BeatmapsCollectionSignals.RefreshSignal>();
      this.SetScreenResolution();
    }

    private void HandleBeatmapsCollectionBeatmapAddedSignal(
      BeatmapsCollectionSignals.BeatmapAddedSignal signal)
    {
      this._beatmapEditorMainNavigationViewController.Setup(BeatmapEditorMainNavigationViewController.EditorControlsButtonType.BeatmapList);
      if (signal.shouldOpen)
      {
        this.ReplaceTopViewController((BeatmapEditorViewController) this._beatmapsListViewController);
        this._beatmapLevelFlowCoordinator.SetupProject(signal.beatmapInfoData);
        this.PresentFlowCoordinator((BeatmapEditorFlowCoordinator) this._beatmapLevelFlowCoordinator, false);
      }
      else
        this.ReplaceTopViewController((BeatmapEditorViewController) this._beatmapsListViewController);
    }

    private void HandleGarbageCollectorGCModeChanged(GarbageCollector.Mode mode)
    {
      if (mode == GarbageCollector.Mode.Enabled)
        return;
      GarbageCollector.GCMode = GarbageCollector.Mode.Enabled;
    }

    private void OpenBeatmapProject(IBeatmapInfoData beatmapInfoData)
    {
      if (!Directory.Exists(beatmapInfoData.beatmapFolderPath))
      {
        this._simpleEditorDialogViewController.Init("Unable to open beatmap", "Beatmap has been moved or deleted", "Close", (Action<int>) (_ => this.SetDialogScreenViewController((BeatmapEditorViewController) null)));
        this.SetDialogScreenViewController((BeatmapEditorViewController) this._simpleEditorDialogViewController);
        this._signalBus.Fire<BeatmapsCollectionSignals.RefreshSignal>();
      }
      else
      {
        this._signalBus.Fire<BeatmapsCollectionSignals.AddRecentlyOpenedBeatmapSignal>(new BeatmapsCollectionSignals.AddRecentlyOpenedBeatmapSignal()
        {
          beatmapFolderPath = beatmapInfoData.beatmapFolderPath
        });
        this._signalBus.Fire<BeatmapProjectManagerSignals.SaveSettingsSignal>();
        this._beatmapLevelFlowCoordinator.SetupProject(beatmapInfoData);
        this.PresentFlowCoordinator((BeatmapEditorFlowCoordinator) this._beatmapLevelFlowCoordinator, false);
      }
    }

    private void SetScreenResolution()
    {
      Vector2Int windowResolution = this._beatmapEditorSettingsDataModel.editorWindowResolution;
      Screen.SetResolution(windowResolution.x, windowResolution.y, false);
      Screen.fullScreen = (bool) (ObservableVariableSO<bool>) this._mainSettingsModel.fullscreen;
    }

    private IEnumerator ProcessMenuDestinationRequestAfterFrameCoroutine(MenuDestination destination)
    {
      yield return (object) null;
      MenuDestination menuDestination = destination;
      if (menuDestination != null && menuDestination is OpenBeatmapLevelDestination levelDestination1)
      {
        OpenBeatmapLevelDestination levelDestination = levelDestination1;
        IBeatmapInfoData beatmapInfoData = this._beatmapCollectionDataModel.beatmapInfos.FirstOrDefault<IBeatmapInfoData>((Func<IBeatmapInfoData, bool>) (beatmapInfo => beatmapInfo.beatmapFolderPath.Contains(levelDestination.projectPath)));
        if (beatmapInfoData != null)
          this.OpenBeatmapProject(beatmapInfoData);
      }
    }
  }
}
