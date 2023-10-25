// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.TranslationEventBoxGroupPreviewView
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
  public class TranslationEventBoxGroupPreviewView : EventTrackPreviewView
  {
    [SerializeField]
    private TranslationEventBoxGroupTrackView _trackView;
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
      this._textOnlyEventMarkerObject.gameObject.SetActive(false);
    }

    protected override void OnEnable()
    {
      base.OnEnable();
      this.UpdatePreview();
      this._textOnlyEventMarkerObject.gameObject.SetActive(false);
      this._signalBus.Subscribe<LightTranslationEventChangedSignal>(new Action(this.HandleLightTranslationEventChanged));
      this._signalBus.Subscribe<EventBoxGroupExtensionChangedSignal>(new Action(this.HandleLightTranslationEventChanged));
    }

    protected override void OnDisable()
    {
      base.OnDisable();
      this._signalBus.TryUnsubscribe<LightTranslationEventChangedSignal>(new Action(this.HandleLightTranslationEventChanged));
      this._signalBus.TryUnsubscribe<EventBoxGroupExtensionChangedSignal>(new Action(this.HandleLightTranslationEventChanged));
    }

    protected override bool CanShowPreview(float beat) => this._beatmapEventBoxGroupsDataModel.GetEventBoxGroupAt(this._trackView.id.groupId, EventBoxGroupEditorData.EventBoxGroupType.Translation, beat) == (EventBoxGroupEditorData) null && this._beatmapState.interactionMode == InteractionMode.Place;

    protected override void ToggleVisibility() => this._textOnlyEventMarkerObject.gameObject.SetActive(this._previewIsVisible);

    private void HandleLightTranslationEventChanged() => this.UpdatePreview();

    private void UpdatePreview()
    {
      if (this._eventBoxGroupsState.eventBoxGroupExtension)
        this._textOnlyEventMarkerObject.Init("");
      else
        this._textOnlyEventMarkerObject.Init(string.Format("{0} / {1}", (object) (float) ((double) this._eventBoxGroupsState.lightTranslation * 100.0), (object) this._eventBoxGroupsState.lightTranslationEaseType));
    }
  }
}
