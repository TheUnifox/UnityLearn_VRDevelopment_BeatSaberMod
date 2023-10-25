// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BpmEditorFlowCoordinator
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.BpmEditor;
using BeatmapEditor3D.BpmEditor.Commands;
using BeatmapEditor3D.BpmEditor.UI;
using BeatmapEditor3D.DataModels;
using HMUI;
using System;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class BpmEditorFlowCoordinator : BeatmapEditorFlowCoordinator
  {
    [Inject]
    private readonly BpmEditorAudioScrollController _bpmEditorAudioScrollController;
    [Inject]
    private readonly BpmEditorSongPreviewController _bpmEditorSongPreviewController;
    [Inject]
    private readonly EditBpmGridViewController _editBpmGridViewController;
    [Inject]
    private readonly EditBpmGridNavigationViewController _editBpmGridNavigationViewController;
    [Inject]
    private readonly SimpleEditorDialogViewController _simpleEditorDialogViewController;
    [Inject]
    private readonly BeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly BpmEditorState _bpmEditorState;
    [Inject]
    private readonly BpmEditorDataModel _bpmEditorDataModel;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly BpmRegionView.Pool _bpmRegionViewPool;
    [Inject]
    private readonly BpmRegionDividerView.Pool _bpmRegionDividerViewPool;
    [Inject]
    private readonly BpmBeatMarkerView.Pool _bpmBeatMarkerViewPool;
    private readonly KeyboardBinder _keyboardBinder = new KeyboardBinder();

    public event Action<bool> didFinishEvent;

    protected void Update() => this._keyboardBinder.ManualUpdate();

    protected override void DidActivate(
      bool firstActivation,
      bool addedToHierarchy,
      bool screenSystemEnabling)
    {
      this.showBackButton = true;
      this.SetBackButtonIconType(BackButtonView.BackButtonType.Back);
      this.SetBackButtonIsDirtyNotification(this._bpmEditorDataModel.isDirty);
      this._bpmEditorDataModel.UpdateWith(this._beatmapDataModel.bpmData);
      this._bpmEditorSongPreviewController.Initialize(this._beatmapDataModel.audioClip);
      this._bpmEditorSongPreviewController.SetSpeed(this._bpmEditorState.playbackSpeed);
      this._bpmEditorSongPreviewController.SetVolume(this._bpmEditorState.musicVolume);
      this._bpmEditorSongPreviewController.SetSfxVolume(this._bpmEditorState.metronomeVolume);
      this._bpmEditorAudioScrollController.Enable();
      this._signalBus.Fire<SetPlayHeadSignal>(new SetPlayHeadSignal(this._bpmEditorState.sample));
      this._signalBus.Subscribe<BpmEditorDataUpdatedSignal>(new Action(this.HandleBpmEditorDataUpdated));
      this._signalBus.Subscribe<BeatmapProjectManagerSignals.BeatmapSaveFailedSignal>(new Action<BeatmapProjectManagerSignals.BeatmapSaveFailedSignal>(this.HandleBeatmapSaveFailed));
      this._keyboardBinder.AddBinding(KeyCode.S, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandleKeyboardBinderSave));
      this.ProvideInitialViewControllers((BeatmapEditorViewController) this._editBpmGridViewController, (BeatmapEditorViewController) this._editBpmGridNavigationViewController);
    }

    protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
    {
      this._bpmEditorState.Reset();
      this._bpmRegionViewPool.Clear();
      this._bpmRegionDividerViewPool.Clear();
      this._bpmBeatMarkerViewPool.Clear();
      this._signalBus.TryUnsubscribe<BpmEditorDataUpdatedSignal>(new Action(this.HandleBpmEditorDataUpdated));
      this._signalBus.TryUnsubscribe<BeatmapProjectManagerSignals.BeatmapSaveFailedSignal>(new Action<BeatmapProjectManagerSignals.BeatmapSaveFailedSignal>(this.HandleBeatmapSaveFailed));
      this._bpmEditorAudioScrollController.Disable();
      this._bpmEditorSongPreviewController.Stop();
    }

    protected override void BackButtonWasPressed(BeatmapEditorViewController topViewController)
    {
      if (this._bpmEditorDataModel.isDirty)
        this._simpleEditorDialogViewController.Init("Quit Editing", "You have unsaved changes, do you want to save them?", "Save and quit", "No and quit", "No and stay", (Action<int>) (buttonIdx =>
        {
          this.SetDialogScreenViewController((BeatmapEditorViewController) null);
          if (buttonIdx == 2)
            return;
          Action<bool> didFinishEvent = this.didFinishEvent;
          if (didFinishEvent == null)
            return;
          didFinishEvent(buttonIdx == 0);
        }));
      else
        this._simpleEditorDialogViewController.Init("Quit Editing", "Do you want to quit?", "Yes", "No", (Action<int>) (buttonIdx =>
        {
          this.SetDialogScreenViewController((BeatmapEditorViewController) null);
          if (buttonIdx != 0)
            return;
          Action<bool> didFinishEvent = this.didFinishEvent;
          if (didFinishEvent == null)
            return;
          didFinishEvent(false);
        }));
      this.SetDialogScreenViewController((BeatmapEditorViewController) this._simpleEditorDialogViewController);
    }

    private void HandleKeyboardBinderSave(bool _)
    {
      if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
        return;
      this._signalBus.Fire<BeatmapProjectManagerSignals.SaveBpmInfoSignal>();
    }

    private void HandleBpmEditorDataUpdated()
    {
      this.SetBackButtonIsDirtyNotification(this._bpmEditorDataModel.isDirty);
      this._signalBus.Fire<BeatmapProjectManagerSignals.SaveBpmInfoToTempSignal>();
    }

    private void HandleBeatmapSaveFailed(
      BeatmapProjectManagerSignals.BeatmapSaveFailedSignal signal)
    {
      this._simpleEditorDialogViewController.Init("Save Failed", "Unable to save beatmap\nReason: " + signal.reason + "\n(You can loose progress if you decide to ignore it)", "Ignore", "Quit", (Action<int>) (buttonIdx =>
      {
        this.SetDialogScreenViewController((BeatmapEditorViewController) null);
        if (buttonIdx != 1)
          return;
        Action<bool> didFinishEvent = this.didFinishEvent;
        if (didFinishEvent == null)
          return;
        didFinishEvent(false);
      }));
      this.SetDialogScreenViewController((BeatmapEditorViewController) this._simpleEditorDialogViewController);
    }
  }
}
