// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapEditorSettingsSerializedData
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BeatmapEditor3D
{
  [Serializable]
  public class BeatmapEditorSettingsSerializedData
  {
    public string version = "0.1.0";
    public string customLevelsFolder;
    public List<string> recentlyOpenedBeatmaps;
    public Vector2Int editorWindowResolution;
    public bool invertSubdivisionScroll;
    public bool invertTimelineScroll;
    public bool staticLights;
    public bool autoExposure;
    public bool enableFpfc;
    public bool preserveTime;
    public float playbackSpeed;
    public BeatmapEditorSettingsSerializedData.WaveformType waveformType;
    public BeatmapEditorSettingsSerializedData.GameplayUIState gameplayUIState;
    private const string kCurrentVersion = "0.1.0";

    public BeatmapEditorSettingsSerializedData()
    {
      this.customLevelsFolder = (string) null;
      this.recentlyOpenedBeatmaps = new List<string>();
      Resolution currentResolution = Screen.currentResolution;
      int width = currentResolution.width;
      currentResolution = Screen.currentResolution;
      int height = currentResolution.height;
      this.editorWindowResolution = new Vector2Int(width, height);
      this.invertSubdivisionScroll = false;
      this.invertTimelineScroll = false;
      this.staticLights = false;
      this.autoExposure = true;
      this.enableFpfc = true;
      this.preserveTime = true;
      this.playbackSpeed = 1f;
      this.waveformType = BeatmapEditorSettingsSerializedData.WaveformType.Expanded;
      this.gameplayUIState = BeatmapEditorSettingsSerializedData.GameplayUIState.Hidden;
    }

    public BeatmapEditorSettingsSerializedData(
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
      BeatmapEditorSettingsSerializedData.WaveformType waveformType,
      BeatmapEditorSettingsSerializedData.GameplayUIState gameplayUIState)
    {
      this.customLevelsFolder = customLevelsFolder;
      this.recentlyOpenedBeatmaps = recentlyOpenedBeatmaps;
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

    public enum WaveformType
    {
      Expanded,
      Small,
      Hidden,
    }

    public enum GameplayUIState
    {
      Hidden,
      Normal,
      Advanced,
    }
  }
}
