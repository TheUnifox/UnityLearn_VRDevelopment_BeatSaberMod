// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.LightTranslationPreviewView
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
  public class LightTranslationPreviewView : EventTrackPreviewView
  {
    [SerializeField]
    private LightTranslationEventBoxTrackView _trackView;
    [SerializeField]
    private TextOnlyEventMarkerObject _textOnlyEventMarkerObject;
    [Inject]
    private readonly EventBoxGroupsState _eventBoxGroupsState;
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;

    protected void Awake()
    {
      this._textOnlyEventMarkerObject.SetState(false, false, false);
      this._textOnlyEventMarkerObject.SetHighlight(false);
    }

    protected override void OnEnable()
    {
      base.OnEnable();
      this._signalBus.Subscribe<LightTranslationEventChangedSignal>(new Action(this.HandleLightTranslationEventChangedSignal));
    }

    protected override void OnDisable()
    {
      base.OnDisable();
      this._signalBus.TryUnsubscribe<LightTranslationEventChangedSignal>(new Action(this.HandleLightTranslationEventChangedSignal));
    }

    protected override bool CanShowPreview(float beat) => this._beatmapEventBoxGroupsDataModel.GetBaseEditorDataAt(this._trackView.id, beat - this.beatmapState.beatOffset) == null && this._beatmapState.interactionMode == InteractionMode.Place;

    protected override void ToggleVisibility()
    {
      if (this._previewIsVisible)
        this.UpdatePreview();
      this._textOnlyEventMarkerObject.gameObject.SetActive(this._previewIsVisible);
    }

    private void HandleLightTranslationEventChangedSignal() => this.UpdatePreview();

    private void UpdatePreview()
    {
      if (this._eventBoxGroupsState.eventBoxGroupExtension)
        this._textOnlyEventMarkerObject.Init("");
      else
        this._textOnlyEventMarkerObject.Init(string.Format("{0} / {1}", (object) (float) ((double) this._eventBoxGroupsState.lightTranslation * 100.0), (object) this._eventBoxGroupsState.lightTranslationEaseType));
    }
  }
}
