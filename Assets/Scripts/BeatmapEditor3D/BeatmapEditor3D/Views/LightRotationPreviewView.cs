// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.LightRotationPreviewView
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
  public class LightRotationPreviewView : EventTrackPreviewView
  {
    [SerializeField]
    private LightRotationEventBoxTrackView _trackView;
    [SerializeField]
    private TextOnlyEventMarkerObject _textOnlyEventMarkerObject;
    [Inject]
    private readonly EventBoxGroupsState _eventBoxGroupsState;
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;

    protected override void OnEnable()
    {
      base.OnEnable();
      this._signalBus.Subscribe<LightRotationEventChangedSignal>(new Action(this.HandleLightRotationEventChanged));
    }

    protected override void OnDisable()
    {
      base.OnDisable();
      this._signalBus.TryUnsubscribe<LightRotationEventChangedSignal>(new Action(this.HandleLightRotationEventChanged));
    }

    protected override bool CanShowPreview(float beat) => this._beatmapEventBoxGroupsDataModel.GetBaseEditorDataAt(this._trackView.id, beat - this.beatmapState.beatOffset) == null && this._beatmapState.interactionMode == InteractionMode.Place;

    protected override void ToggleVisibility()
    {
      if (this._previewIsVisible)
        this.UpdatePreview();
      this._textOnlyEventMarkerObject.gameObject.SetActive(this._previewIsVisible);
    }

    private void HandleLightRotationEventChanged() => this.UpdatePreview();

    private void UpdatePreview()
    {
      this._textOnlyEventMarkerObject.Init(this._eventBoxGroupsState.eventBoxGroupExtension ? "" : string.Format("{0}, {1}, {2}", (object) this._eventBoxGroupsState.lightRotationEaseType, (object) this._eventBoxGroupsState.lightRotationLoopCount, (object) this._eventBoxGroupsState.lightRotation));
      this._textOnlyEventMarkerObject.SetState(false, false, false);
      this._textOnlyEventMarkerObject.SetHighlight(false);
    }
  }
}
