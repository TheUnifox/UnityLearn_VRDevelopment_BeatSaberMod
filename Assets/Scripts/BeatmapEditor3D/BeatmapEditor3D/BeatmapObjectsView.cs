// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapObjectsView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands.LevelEditor;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using System;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class BeatmapObjectsView : BaseObjectsView
  {
    [SerializeField]
    private AbstractBeatmapObjectView[] _beatmapObjectViews;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;

    protected override void OnEnable()
    {
      this._signalBus.Subscribe<BeatmapLevelStateTimeUpdated>(new Action(this.HandleLevelEditorStateTimeUpdated));
      this._signalBus.Subscribe<BeatmapTimeScaleChangedSignal>(new Action(this.HandleBeatmapTimeScaleChanged));
      this._signalBus.Subscribe<BeatmapLevelUpdatedSignal>(new Action(this.HandleBeatmapLevelUpdated));
      this._signalBus.Subscribe<BeatmapObjectsSelectionStateUpdatedSignal>(new Action(this.HandleBeatmapObjectsSelectionStateUpdated));
      this._signalBus.Subscribe<InteractionModeChangedSignal>(new Action(this.HandleBeatmapObjectModeChanged));
      this.RefreshView(false);
      base.OnEnable();
    }

    protected override void OnDisable()
    {
      this._signalBus.TryUnsubscribe<BeatmapLevelStateTimeUpdated>(new Action(this.HandleLevelEditorStateTimeUpdated));
      this._signalBus.TryUnsubscribe<BeatmapTimeScaleChangedSignal>(new Action(this.HandleBeatmapTimeScaleChanged));
      this._signalBus.TryUnsubscribe<BeatmapLevelUpdatedSignal>(new Action(this.HandleBeatmapLevelUpdated));
      this._signalBus.TryUnsubscribe<BeatmapObjectsSelectionStateUpdatedSignal>(new Action(this.HandleBeatmapObjectsSelectionStateUpdated));
      this._signalBus.TryUnsubscribe<InteractionModeChangedSignal>(new Action(this.HandleBeatmapObjectModeChanged));
      this.ClearView();
      base.OnDisable();
    }

    private void RefreshView(bool clearView)
    {
      float startTime = this._beatmapState.beat - 5f;
      float endTime = this._beatmapState.beat + 16f;
      foreach (AbstractBeatmapObjectView beatmapObjectView in this._beatmapObjectViews)
        beatmapObjectView.RefreshView(startTime, endTime, clearView);
    }

    private void ClearView()
    {
      foreach (AbstractBeatmapObjectView beatmapObjectView in this._beatmapObjectViews)
        beatmapObjectView.ClearView();
    }

    private void UpdateTimeScale()
    {
      foreach (AbstractBeatmapObjectView beatmapObjectView in this._beatmapObjectViews)
        beatmapObjectView.UpdateTimeScale();
    }

    private void HandleLevelEditorStateTimeUpdated() => this.RefreshView(false);

    private void HandleBeatmapTimeScaleChanged()
    {
      this.RefreshView(false);
      this.UpdateTimeScale();
    }

    private void HandleBeatmapLevelUpdated() => this.RefreshView(true);

    private void HandleBeatmapObjectsSelectionStateUpdated() => this.RefreshView(false);

    private void HandleBeatmapObjectModeChanged() => this.RefreshView(true);
  }
}
