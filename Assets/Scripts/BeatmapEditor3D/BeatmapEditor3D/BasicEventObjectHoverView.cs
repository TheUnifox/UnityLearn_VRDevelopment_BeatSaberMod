// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BasicEventObjectHoverView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands.LevelEditor;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using BeatmapEditor3D.Types;
using BeatmapEditor3D.Visuals;
using System;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class BasicEventObjectHoverView : MonoBehaviour
  {
    [SerializeField]
    private TextEventMarkerObject _textEventMarkerObject;
    [SerializeField]
    private LightEventMarkerObject _lightEventMarkerObject;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly IBasicEventsState _basicEventsState;
    [Inject]
    private readonly IBeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    private bool _hoverIsVisible;

    protected void Start()
    {
      this._textEventMarkerObject.gameObject.SetActive(false);
      this._lightEventMarkerObject.gameObject.SetActive(false);
      this._signalBus.Subscribe<EventHoverUpdatedSignal>(new Action(this.UpdatePreview));
      this._signalBus.Subscribe<SelectedEventChangedSignal>(new Action(this.UpdatePreview));
      this._signalBus.Subscribe<SwitchBeatmapEditingModeSignal>(new Action(this.HandleSwitchLevelEditorMode));
      this._signalBus.Subscribe<InteractionModeChangedSignal>(new Action(this.HandleInteractionModeChanged));
    }

    protected void OnDestroy()
    {
      this._signalBus.TryUnsubscribe<EventHoverUpdatedSignal>(new Action(this.UpdatePreview));
      this._signalBus.TryUnsubscribe<SelectedEventChangedSignal>(new Action(this.UpdatePreview));
      this._signalBus.TryUnsubscribe<SwitchBeatmapEditingModeSignal>(new Action(this.HandleSwitchLevelEditorMode));
      this._signalBus.TryUnsubscribe<InteractionModeChangedSignal>(new Action(this.HandleInteractionModeChanged));
    }

    private void UpdatePreview()
    {
      this._textEventMarkerObject.gameObject.SetActive(false);
      this._lightEventMarkerObject.gameObject.SetActive(false);
      if (this._beatmapState.editingMode != BeatmapEditingMode.BasicEvents || this._beatmapState.interactionMode != InteractionMode.Place)
        return;
      float position = TrackPlacementHelper.TrackToPosition(this._basicEventsState.currentHoverPageTrackId, this._beatmapDataModel.environmentTrackDefinition[this._basicEventsState.currentEventsPage].Count);
      if (this._beatmapDataModel.environmentTrackDefinition.visibleTrackInfos.Length == 0 || this._basicEventsState.currentHoverVisibleTrackId < 0 && this._basicEventsState.currentHoverVisibleTrackId >= this._beatmapDataModel.environmentTrackDefinition.visibleTrackInfos.Length)
        return;
      EnvironmentTracksDefinitionSO.BasicEventTrackInfo visibleTrackInfo = this._beatmapDataModel.environmentTrackDefinition.visibleTrackInfos[this._basicEventsState.currentHoverVisibleTrackId];
      (int value, float floatValue) = this._basicEventsState.GetSelectedBeatmapTypeValue(visibleTrackInfo.trackToolbarType);
      if (visibleTrackInfo.trackToolbarType == TrackToolbarType.Lights)
      {
        this._lightEventMarkerObject.Init((BasicEventEditorData) null, EventObjectViewColorHelper.GetLightEventObjectColor(value, floatValue).ColorWithAlpha(1f));
        this._lightEventMarkerObject.SetEventValue(value, floatValue);
        this._lightEventMarkerObject.SetState(false, false, false);
        this._lightEventMarkerObject.gameObject.SetActive(true);
        this._lightEventMarkerObject.transform.localPosition = new Vector3(position, 0.0f, 0.0f);
      }
      else
      {
        this._textEventMarkerObject.Init((BasicEventEditorData) null);
        this._textEventMarkerObject.SetText(string.Format("{0}/{1}", (object) value, (object) floatValue));
        this._textEventMarkerObject.SetState(false, false, false);
        this._textEventMarkerObject.gameObject.SetActive(true);
        this._textEventMarkerObject.transform.localPosition = new Vector3(position, 0.0f, 0.0f);
      }
    }

    public void ShowPreview()
    {
      this._hoverIsVisible = true;
      this.ToggleVisibility();
    }

    public void HidePreview()
    {
      this._hoverIsVisible = false;
      this.ToggleVisibility();
    }

    private void HandleSwitchLevelEditorMode() => this.UpdatePreview();

    private void HandleInteractionModeChanged() => this.UpdatePreview();

    private void ToggleVisibility() => this.gameObject.SetActive(this._hoverIsVisible);
  }
}
