// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapEditorMainNavigationViewController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using HMUI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BeatmapEditor3D
{
  public class BeatmapEditorMainNavigationViewController : BeatmapEditorViewController
  {
    [SerializeField]
    private Button _beatmapsListButton;
    [SerializeField]
    private Button _newBeatmapButton;
    [SerializeField]
    private Button _openBeatmapButton;
    [SerializeField]
    private Button _settingsButton;
    [SerializeField]
    private Button _refreshButton;
    [Inject]
    private readonly SignalBus _signalBus;

    public event Action<BeatmapEditorMainNavigationViewController.EditorControlsButtonType> buttonWasPressed;

    public void Setup(
      BeatmapEditorMainNavigationViewController.EditorControlsButtonType displayedViewControllerType)
    {
      this.SetActiveNavButtons(displayedViewControllerType);
    }

    protected override void DidActivate(
      bool firstActivation,
      bool addedToHierarchy,
      bool screenSystemEnabling)
    {
      if (!firstActivation)
        return;
      ButtonBinder buttonBinder = this.buttonBinder;
      List<Tuple<Button, Action>> tupleList = new List<Tuple<Button, Action>>();
      tupleList.Add<Button, Action>(this._beatmapsListButton, new Action(this.HandleBeatmapListButtonPressed));
      tupleList.Add<Button, Action>(this._newBeatmapButton, new Action(this.HandleNewBeatmapButtonPressed));
      tupleList.Add<Button, Action>(this._openBeatmapButton, new Action(this.HandleOpenBeatmapButtonPressed));
      tupleList.Add<Button, Action>(this._settingsButton, new Action(this.HandleSettingsButtonPressed));
      tupleList.Add<Button, Action>(this._refreshButton, new Action(this.HandleRefreshButtonPressed));
      buttonBinder.AddBindings(tupleList);
    }

    private void HandleRefreshButtonPressed() => this._signalBus.Fire<BeatmapsCollectionSignals.RefreshSignal>();

    private void HandleBeatmapListButtonPressed()
    {
      this.SetActiveNavButtons(BeatmapEditorMainNavigationViewController.EditorControlsButtonType.BeatmapList);
      Action<BeatmapEditorMainNavigationViewController.EditorControlsButtonType> buttonWasPressed = this.buttonWasPressed;
      if (buttonWasPressed == null)
        return;
      buttonWasPressed(BeatmapEditorMainNavigationViewController.EditorControlsButtonType.BeatmapList);
    }

    private void HandleNewBeatmapButtonPressed()
    {
      this.SetActiveNavButtons(BeatmapEditorMainNavigationViewController.EditorControlsButtonType.NewBeatmap);
      Action<BeatmapEditorMainNavigationViewController.EditorControlsButtonType> buttonWasPressed = this.buttonWasPressed;
      if (buttonWasPressed == null)
        return;
      buttonWasPressed(BeatmapEditorMainNavigationViewController.EditorControlsButtonType.NewBeatmap);
    }

    private void HandleOpenBeatmapButtonPressed()
    {
      this.SetActiveNavButtons(BeatmapEditorMainNavigationViewController.EditorControlsButtonType.OpenBeatmap);
      Action<BeatmapEditorMainNavigationViewController.EditorControlsButtonType> buttonWasPressed = this.buttonWasPressed;
      if (buttonWasPressed == null)
        return;
      buttonWasPressed(BeatmapEditorMainNavigationViewController.EditorControlsButtonType.OpenBeatmap);
    }

    private void HandleSettingsButtonPressed()
    {
      this.SetActiveNavButtons(BeatmapEditorMainNavigationViewController.EditorControlsButtonType.Settings);
      Action<BeatmapEditorMainNavigationViewController.EditorControlsButtonType> buttonWasPressed = this.buttonWasPressed;
      if (buttonWasPressed == null)
        return;
      buttonWasPressed(BeatmapEditorMainNavigationViewController.EditorControlsButtonType.Settings);
    }

    private void SetActiveNavButtons(
      BeatmapEditorMainNavigationViewController.EditorControlsButtonType displayedViewControllerType)
    {
      this._beatmapsListButton.enabled = displayedViewControllerType != 0;
      this._newBeatmapButton.enabled = displayedViewControllerType != BeatmapEditorMainNavigationViewController.EditorControlsButtonType.NewBeatmap;
      this._openBeatmapButton.enabled = displayedViewControllerType != BeatmapEditorMainNavigationViewController.EditorControlsButtonType.OpenBeatmap;
      this._settingsButton.enabled = displayedViewControllerType != BeatmapEditorMainNavigationViewController.EditorControlsButtonType.Settings;
    }

    public enum EditorControlsButtonType
    {
      BeatmapList,
      NewBeatmap,
      OpenBeatmap,
      Settings,
    }
  }
}
