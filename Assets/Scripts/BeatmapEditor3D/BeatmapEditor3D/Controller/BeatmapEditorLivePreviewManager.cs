// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Controller.BeatmapEditorLivePreviewManager
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using BeatmapEditor3D.Types;
using System;
using Zenject;

namespace BeatmapEditor3D.Controller
{
  public class BeatmapEditorLivePreviewManager : IInitializable, IDisposable
  {
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly BeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly BeatmapCallbacksController _beatmapCallbacksController;
    [Inject]
    private readonly BeatmapEditorSettingsDataModel _beatmapEditorSettingsDataModel;
    private IBeatToTimeConvertor _timeConvertor;

    public void Initialize()
    {
      this._timeConvertor = (IBeatToTimeConvertor) new BpmDataBeatToTimeConvertor();
      this._signalBus.Subscribe<BeatmapProjectManagerSignals.BeatmapLevelDataModelLoaded>(new Action(this.HandleBeatmapLevelDataModelLoaded));
      this._signalBus.Subscribe<BeatmapLevelStateTimeUpdated>(new Action(this.HandleBeatmapLevelStateTimeUpdated));
      this._signalBus.Subscribe<LevelEditorStateStaticLightsUpdatedSignal>(new Action(this.HandleLevelEditorStateStaticLightsUpdated));
      this._signalBus.Subscribe<BeatmapEditingModeSwitched>(new Action(this.HandleLevelEditorModeSwitched));
      this._beatmapCallbacksController.sendCallbacksOnBeatmapDataChange = true;
      this.UpdateLivePreview(this._beatmapState.beat);
    }

    public void Dispose()
    {
      this._signalBus.TryUnsubscribe<BeatmapProjectManagerSignals.BeatmapLevelDataModelLoaded>(new Action(this.HandleBeatmapLevelDataModelLoaded));
      this._signalBus.TryUnsubscribe<BeatmapLevelStateTimeUpdated>(new Action(this.HandleBeatmapLevelStateTimeUpdated));
      this._signalBus.TryUnsubscribe<LevelEditorStateStaticLightsUpdatedSignal>(new Action(this.HandleLevelEditorStateStaticLightsUpdated));
    }

    private void HandleBeatmapLevelDataModelLoaded() => this.UpdateLivePreview(this._beatmapState.beat);

    private void HandleBeatmapLevelStateTimeUpdated() => this.UpdateLivePreview(this._beatmapState.beat);

    private void HandleLevelEditorStateStaticLightsUpdated()
    {
      if (this._beatmapState.editingMode == BeatmapEditingMode.Objects && this._beatmapEditorSettingsDataModel.staticLights)
      {
        this.ClearDisplayedEvents();
      }
      else
      {
        this._beatmapCallbacksController.ReplayState();
        this.UpdateLivePreview(this._beatmapState.beat);
      }
    }

    private void HandleLevelEditorModeSwitched() => this.UpdateLivePreview(this._beatmapState.beat);

    private void UpdateLivePreview(float beat)
    {
      if (this._beatmapState.editingMode == BeatmapEditingMode.Objects && this._beatmapEditorSettingsDataModel.staticLights)
        return;
      this._beatmapCallbacksController.ManualUpdate(this._timeConvertor.ConvertBeatToTime(beat));
    }

    private void ClearDisplayedEvents()
    {
      float beat = this._beatmapState.beat;
      foreach (EnvironmentTracksDefinitionSO.BasicEventTrackInfo basicEventTrackInfo in this._beatmapDataModel.environmentTrackDefinition.basicEventTrackInfos)
      {
        if (basicEventTrackInfo.basicBeatmapEventType == BasicBeatmapEventType.Event5)
          this._beatmapCallbacksController.TriggerBeatmapEvent((BeatmapEventData) new ColorBoostBeatmapEventData(beat, false));
        else
          this._beatmapCallbacksController.TriggerBeatmapEvent((BeatmapEventData) new BasicBeatmapEventData(beat, basicEventTrackInfo.basicBeatmapEventType, 0, 0.0f));
      }
      foreach (EnvironmentTracksDefinitionSO.EventBoxGroupPageInfo boxGroupPageInfo in this._beatmapDataModel.environmentTrackDefinition.eventBoxGroupPageInfos)
      {
        foreach (EnvironmentTracksDefinitionSO.EventBoxGroupTrackInfo boxGroupTrackInfo in boxGroupPageInfo.eventBoxGroupTrackInfos)
        {
          int groupId = boxGroupTrackInfo.lightGroup.groupId;
          for (int elementId = 0; elementId < boxGroupTrackInfo.lightGroup.numberOfElements; ++elementId)
          {
            this._beatmapCallbacksController.TriggerBeatmapEvent((BeatmapEventData) new LightColorBeatmapEventData(beat, groupId, elementId, BeatmapEventTransitionType.Instant, global::EnvironmentColorType.Color0, 0.0f, 0));
            this._beatmapCallbacksController.TriggerBeatmapEvent((BeatmapEventData) new LightRotationBeatmapEventData(beat, groupId, elementId, false, EaseType.None.FromEaseType(), LightAxis.X, 0.0f, 0, LightRotationDirection.Automatic));
            this._beatmapCallbacksController.TriggerBeatmapEvent((BeatmapEventData) new LightRotationBeatmapEventData(beat, groupId, elementId, false, EaseType.None.FromEaseType(), LightAxis.Y, 0.0f, 0, LightRotationDirection.Automatic));
            this._beatmapCallbacksController.TriggerBeatmapEvent((BeatmapEventData) new LightRotationBeatmapEventData(beat, groupId, elementId, false, EaseType.None.FromEaseType(), LightAxis.Z, 0.0f, 0, LightRotationDirection.Automatic));
            this._beatmapCallbacksController.TriggerBeatmapEvent((BeatmapEventData) new LightTranslationBeatmapEventData(beat, groupId, elementId, false, EaseType.None.FromEaseType(), LightAxis.X, 0.0f, 0.0f));
            this._beatmapCallbacksController.TriggerBeatmapEvent((BeatmapEventData) new LightTranslationBeatmapEventData(beat, groupId, elementId, false, EaseType.None.FromEaseType(), LightAxis.Y, 0.0f, 0.0f));
            this._beatmapCallbacksController.TriggerBeatmapEvent((BeatmapEventData) new LightTranslationBeatmapEventData(beat, groupId, elementId, false, EaseType.None.FromEaseType(), LightAxis.Z, 0.0f, 0.0f));
          }
        }
      }
      this._beatmapCallbacksController.ManualUpdate(beat);
    }
  }
}
