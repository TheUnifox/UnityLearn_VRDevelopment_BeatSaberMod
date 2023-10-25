// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapEditorFloorLightStreakTileEffectController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Controller;
using BeatmapEditor3D.DataModels;
using Ice;
using System;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class BeatmapEditorFloorLightStreakTileEffectController : MonoBehaviour
  {
    [Inject]
    private readonly ISongPreviewController _songPreviewController;
    [Inject]
    private readonly BeatmapEditorSettingsDataModel _beatmapEditorSettingsDataModel;
    [Inject]
    private readonly SignalBus _signalBus;
    private FloorLightStreakTileEffect[] _floorLightStreakTileEffects;
    private FloorLightTilesGrid[] _floorLightTilesGrids;

    protected void Start()
    {
      this._floorLightStreakTileEffects = UnityEngine.Object.FindObjectsOfType<FloorLightStreakTileEffect>();
      this._floorLightTilesGrids = UnityEngine.Object.FindObjectsOfType<FloorLightTilesGrid>();
      this._signalBus.Subscribe<LevelEditorStateStaticLightsUpdatedSignal>(new Action(this.HandleLevelEditorStateStaticLightsUpdated));
      this._songPreviewController.playbackStoppedEvent += new Action(this.HandleSongPreviewControllerPlaybackStopped);
    }

    protected void OnDestroy()
    {
      this._signalBus.TryUnsubscribe<LevelEditorStateStaticLightsUpdatedSignal>(new Action(this.HandleLevelEditorStateStaticLightsUpdated));
      this._songPreviewController.playbackStoppedEvent -= new Action(this.HandleSongPreviewControllerPlaybackStopped);
    }

    private void HandleLevelEditorStateStaticLightsUpdated()
    {
      if (!this._beatmapEditorSettingsDataModel.staticLights)
        return;
      this.DespawnFloorEffects();
    }

    private void HandleSongPreviewControllerPlaybackStopped() => this.DespawnFloorEffects();

    private void DespawnFloorEffects()
    {
      foreach (FloorLightStreakTileEffect streakTileEffect in this._floorLightStreakTileEffects)
        streakTileEffect.DespawnAllEffects();
      foreach (FloorLightTilesGrid floorLightTilesGrid in this._floorLightTilesGrids)
        floorLightTilesGrid.DespawnAllTiles();
    }
  }
}
