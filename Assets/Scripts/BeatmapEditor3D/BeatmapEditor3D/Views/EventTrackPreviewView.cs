// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.EventTrackPreviewView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands.LevelEditor;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.InputSignals;
using BeatmapEditor3D.LevelEditor;
using System;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Views
{
  public abstract class EventTrackPreviewView : MonoBehaviour
  {
    [SerializeField]
    private EventTrackInputMouseInputSource _eventTrackInputMouseInputSource;
    [Inject]
    protected readonly SignalBus _signalBus;
    [Inject]
    protected readonly IReadonlyBeatmapState beatmapState;
    protected bool _previewIsVisible;
    private bool _previewShouldBeVisible;

    protected virtual void OnEnable()
    {
      this._signalBus.Subscribe<BeatmapEditingModeSwitched>(new Action(this.HandleLevelEditorSwitched));
      this._signalBus.Subscribe<BeatmapLevelUpdatedSignal>(new Action(this.HandleBeatmapLevelUpdated));
      this._signalBus.Subscribe<BeatmapLevelStateTimeUpdated>(new Action(this.HandleBeatmapLevelStateTimeUpdated));
      this._signalBus.Subscribe<InteractionModeChangedSignal>(new Action(this.HandleInteractionModeChanged));
      this._eventTrackInputMouseInputSource.gridPointerHoverEvent += new Action<MouseInputType>(this.HandleEventTrackInputGridHover);
      this._previewShouldBeVisible = false;
      this.ResolvePreviewVisibility();
    }

    protected virtual void OnDisable()
    {
      this._signalBus.TryUnsubscribe<BeatmapEditingModeSwitched>(new Action(this.HandleLevelEditorSwitched));
      this._signalBus.TryUnsubscribe<BeatmapLevelUpdatedSignal>(new Action(this.HandleBeatmapLevelUpdated));
      this._signalBus.TryUnsubscribe<BeatmapLevelStateTimeUpdated>(new Action(this.HandleBeatmapLevelStateTimeUpdated));
      this._signalBus.TryUnsubscribe<InteractionModeChangedSignal>(new Action(this.HandleInteractionModeChanged));
      this._eventTrackInputMouseInputSource.gridPointerHoverEvent -= new Action<MouseInputType>(this.HandleEventTrackInputGridHover);
    }

    private void HandleLevelEditorSwitched()
    {
      this._previewShouldBeVisible = false;
      this.ResolvePreviewVisibility();
    }

    private void HandleBeatmapLevelUpdated() => this.ResolvePreviewVisibility();

    private void HandleBeatmapLevelStateTimeUpdated() => this.ResolvePreviewVisibility();

    private void HandleInteractionModeChanged() => this.ResolvePreviewVisibility();

    private void HandleEventTrackInputGridHover(MouseInputType inputType)
    {
      this._previewShouldBeVisible = inputType == MouseInputType.Enter;
      this.ResolvePreviewVisibility();
    }

    private void ResolvePreviewVisibility()
    {
      this._previewIsVisible = this._previewShouldBeVisible && this.CanShowPreview(this.beatmapState.beat);
      this.ToggleVisibility();
    }

    protected abstract bool CanShowPreview(float beat);

    protected abstract void ToggleVisibility();
  }
}
