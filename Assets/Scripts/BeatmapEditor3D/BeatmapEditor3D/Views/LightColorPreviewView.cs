// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.LightColorPreviewView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Types;
using System;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Views
{
  public class LightColorPreviewView : EventTrackPreviewView
  {
    [SerializeField]
    private LightColorEventBoxTrackView _trackView;
    [SerializeField]
    private ColorEventMarkerObject _colorEventMarkerObject;
    [Inject]
    private readonly EventBoxGroupsState _eventBoxGroupsState;
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;

    protected override void OnEnable()
    {
      base.OnEnable();
      this._signalBus.Subscribe<LightColorEventChangedSignal>(new Action(this.HandleLightColorEventChangedSignal));
    }

    protected override void OnDisable()
    {
      base.OnDisable();
      this._signalBus.TryUnsubscribe<LightColorEventChangedSignal>(new Action(this.HandleLightColorEventChangedSignal));
    }

    protected override bool CanShowPreview(float beat) => this._beatmapEventBoxGroupsDataModel.GetBaseEditorDataAt(this._trackView.id, beat - this.beatmapState.beatOffset) == null && this._beatmapState.interactionMode == InteractionMode.Place;

    protected override void ToggleVisibility()
    {
      if (this._previewIsVisible)
        this.UpdatePreview();
      this._colorEventMarkerObject.gameObject.SetActive(this._previewIsVisible);
    }

    private void HandleLightColorEventChangedSignal() => this.UpdatePreview();

    private void UpdatePreview()
    {
      if (this._eventBoxGroupsState.eventBoxGroupExtension)
        this._colorEventMarkerObject.Init(ColorEventMarkerObject.EnvironmentColor.Default, ColorEventMarkerObject.InterpolationType.Instant, 0.0f, 0);
      else
        this._colorEventMarkerObject.Init(this._eventBoxGroupsState.lightColorType.ToObjectColor(), this._eventBoxGroupsState.lightColorTransitionType.ToObjectInterpolation(), this._eventBoxGroupsState.lightColorBrightness, this._eventBoxGroupsState.lightStrobeBeatFrequency);
      this._colorEventMarkerObject.SetState(false, false, false);
      this._colorEventMarkerObject.SetHighlight(false);
    }
  }
}
