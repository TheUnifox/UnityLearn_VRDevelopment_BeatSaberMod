// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.BeatmapEditorSettingsDataModel
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace BeatmapEditor3D.DataModels
{
  public class BeatmapEditorSettingsDataModel
  {
    public const string kDefaultCustomLevelsDirectoryName = "CustomLevels";
    public const int kMaxRecentBeatmaps = 6;
    [DoesNotRequireDomainReloadInit]
    public static readonly string baseProjectPath = Application.dataPath;
    [DoesNotRequireDomainReloadInit]
    public static readonly string defaultCustomLevelsDirectoryPath = Path.Combine(BeatmapEditorSettingsDataModel.baseProjectPath, "CustomLevels");
    private List<string> _recentlyOpenedBeatmaps;
    private string _customLevelsFolder;

    public string customLevelsFolder => !string.IsNullOrEmpty(this._customLevelsFolder) ? this._customLevelsFolder : BeatmapEditorSettingsDataModel.defaultCustomLevelsDirectoryPath;

    public IReadOnlyList<string> recentlyOpenedBeatmaps => (IReadOnlyList<string>) this._recentlyOpenedBeatmaps;

    public Vector2Int editorWindowResolution { get; private set; }

    public bool invertSubdivisionScroll { get; private set; }

    public bool invertTimelineScroll { get; private set; }

    public bool staticLights { get; private set; }

    public bool zenMode { get; private set; }

    public bool autoExposure { get; private set; }

    public bool enableFpfc { get; private set; }

    public bool preserveTime { get; private set; }

    public float playbackSpeed { get; private set; }

    public BeatmapEditor3D.Types.WaveformType waveformType { get; private set; }

    public BeatmapEditor3D.Types.GameplayUIState gameplayUIState { get; private set; }

    public void UpdateWith(
      string customLevelsFolder,
      List<string> recentlyOpenedBeatmaps,
      Vector2Int editorWindowResolution,
      bool invertSubdivisionScroll,
      bool invertTimelineScroll,
      bool staticLights,
      bool autoExposure,
      bool enableFpfc,
      bool preserveTime,
      float playbackSpeed,
      BeatmapEditor3D.Types.WaveformType waveformType,
      BeatmapEditor3D.Types.GameplayUIState gameplayUIState)
    {
      this._customLevelsFolder = customLevelsFolder;
      this._recentlyOpenedBeatmaps = recentlyOpenedBeatmaps;
      this.editorWindowResolution = editorWindowResolution;
      this.invertSubdivisionScroll = invertSubdivisionScroll;
      this.invertTimelineScroll = invertTimelineScroll;
      this.staticLights = staticLights;
      this.autoExposure = autoExposure;
      this.enableFpfc = enableFpfc;
      this.preserveTime = preserveTime;
      this.playbackSpeed = playbackSpeed;
      this.waveformType = waveformType;
      this.gameplayUIState = gameplayUIState;
    }

    public void UpdateWith(
      string customLevelsFolder,
      Vector2Int editorWindowResolution,
      bool invertSubdivisionScroll,
      bool invertTimelineScroll)
    {
      this._customLevelsFolder = customLevelsFolder;
      this.editorWindowResolution = editorWindowResolution;
      this.invertSubdivisionScroll = invertSubdivisionScroll;
      this.invertTimelineScroll = invertTimelineScroll;
    }

    public void AddRecentlyOpenedBeatmap(string beatmap)
    {
      if (this._recentlyOpenedBeatmaps.Contains(beatmap))
        this._recentlyOpenedBeatmaps.Remove(beatmap);
      this._recentlyOpenedBeatmaps.Insert(0, beatmap);
      if (this._recentlyOpenedBeatmaps.Count <= 6)
        return;
      this._recentlyOpenedBeatmaps.RemoveAt(this._recentlyOpenedBeatmaps.Count - 1);
    }

    public void ClearRecentlyOpenedBeatmaps() => this._recentlyOpenedBeatmaps.Clear();

    public void SetEditorResolution(Vector2Int editorWindowResolution) => this.editorWindowResolution = editorWindowResolution;

    public void SetInvertSubdivisionScroll(bool invertSubdivisionScroll) => this.invertSubdivisionScroll = invertSubdivisionScroll;

    public void SetInvertTimelineScroll(bool invertTimelineScroll) => this.invertTimelineScroll = invertTimelineScroll;

    public void SetStaticLights(bool staticLights) => this.staticLights = staticLights;

    public void SetZenMode(bool zenMode) => this.zenMode = zenMode;

    public void SetAutoExposure(bool autoExposure) => this.autoExposure = autoExposure;

    public void SetEnableFpfc(bool enableFpfc) => this.enableFpfc = enableFpfc;

    public void SetPreserveTime(bool preserveTime) => this.preserveTime = preserveTime;

    public void SetPlaybackSpeed(float playbackSpeed) => this.playbackSpeed = playbackSpeed;

    public void SetWaveformType(BeatmapEditor3D.Types.WaveformType waveformType) => this.waveformType = waveformType;

    public void SetGameplayUIState(BeatmapEditor3D.Types.GameplayUIState gameplayUIState) => this.gameplayUIState = gameplayUIState;
  }
}
