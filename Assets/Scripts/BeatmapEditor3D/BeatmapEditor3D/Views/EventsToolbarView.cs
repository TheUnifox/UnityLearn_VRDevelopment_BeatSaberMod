// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.EventsToolbarView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using HMUI;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Views
{
  public class EventsToolbarView : MonoBehaviour
  {
    [SerializeField]
    private LightEventsV1Toolbar _lightEventsV1Toolbar;
    [SerializeField]
    private LightEventsV2Toolbar _lightEventsV2Toolbar;
    [SerializeField]
    private AbstractBeatmapEditorToolbar[] _toolbars;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly IBasicEventsState _basicEventsState;
    [Inject]
    private readonly BeatmapDataModel _beatmapDataModel;
    private AbstractBeatmapEditorToolbar _lightEventsToolbar;
    private readonly KeyboardBinder _keyboardBinder = new KeyboardBinder();

    public void SetLightViewVersion(LightEventsVersion lightEventsVersion)
    {
      this._lightEventsToolbar = lightEventsVersion == LightEventsVersion.Version1 ? (AbstractBeatmapEditorToolbar) this._lightEventsV1Toolbar : (AbstractBeatmapEditorToolbar) this._lightEventsV2Toolbar;
      this._lightEventsV1Toolbar.gameObject.SetActive(lightEventsVersion == LightEventsVersion.Version1);
      this._lightEventsV2Toolbar.gameObject.SetActive(lightEventsVersion == LightEventsVersion.Version2);
    }

    protected void OnEnable()
    {
      this._lightEventsToolbar.SetKeyBindings(this._keyboardBinder);
      foreach (AbstractBeatmapEditorToolbar toolbar in this._toolbars)
        toolbar.SetKeyBindings(this._keyboardBinder);
      this.SetCurrentData();
      this.SetEventsPageToolbars();
      this._signalBus.Subscribe<SelectedEventChangedSignal>(new Action(this.HandleSelectedEventChanged));
      this._signalBus.Subscribe<EventsPageChangedSignal>(new Action(this.HandleEventsPageChangedSignal));
    }

    protected void OnDisable()
    {
      this._keyboardBinder.ClearBindings();
      this._signalBus.TryUnsubscribe<SelectedEventChangedSignal>(new Action(this.HandleSelectedEventChanged));
      this._signalBus.TryUnsubscribe<EventsPageChangedSignal>(new Action(this.HandleEventsPageChangedSignal));
    }

    protected void Update() => this._keyboardBinder.ManualUpdate();

    private void HandleEventsPageChangedSignal() => this.SetEventsPageToolbars();

    private void HandleSelectedEventChanged() => this.SetCurrentData();

    private void SetEventsPageToolbars()
    {
      List<TrackToolbarType> toolbarTypesOnPage = this._beatmapDataModel.environmentTrackDefinition.GetToolbarTypesOnPage(this._basicEventsState.currentEventsPage);
      this._lightEventsToolbar.gameObject.SetActive(toolbarTypesOnPage.Contains(this._lightEventsToolbar.toolbarType));
      foreach (AbstractBeatmapEditorToolbar toolbar in this._toolbars)
        toolbar.gameObject.SetActive(toolbarTypesOnPage.Contains(toolbar.toolbarType));
    }

    private void SetCurrentData()
    {
      (int value, float floatValue) beatmapTypeValue = this._basicEventsState.GetSelectedBeatmapTypeValue(this._lightEventsToolbar.toolbarType);
      this._lightEventsToolbar.SetValue(beatmapTypeValue.value, beatmapTypeValue.floatValue, this._basicEventsState.GetSelectedBeatmapTypePayload(this._lightEventsToolbar.toolbarType));
      foreach (AbstractBeatmapEditorToolbar toolbar in this._toolbars)
      {
        (int value, float floatValue) = this._basicEventsState.GetSelectedBeatmapTypeValue(toolbar.toolbarType);
        object beatmapTypePayload = this._basicEventsState.GetSelectedBeatmapTypePayload(toolbar.toolbarType);
        toolbar.SetValue(value, floatValue, beatmapTypePayload);
      }
    }
  }
}
