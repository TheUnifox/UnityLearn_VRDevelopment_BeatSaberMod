// Decompiled with JetBrains decompiler
// Type: RecordingToolConfigurationProcessor
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Zenject;

public class RecordingToolConfigurationProcessor
{
  protected readonly ProgramArguments _programArguments;
  protected readonly IBeatSaberLogger _logger;
  protected readonly IPosesSerializer _posesSerializer;
  protected readonly RecordingToolResourceContainerSO _resourceContainer;
  public const string kRecordingToolCommandLineArgument = "--enable_recording_tool";
  protected const string kSoloMode = "Solo";
  protected const string kNormalEnvironmentType = "Normal";
  protected const string kNormalEnvironmentNameSuffix = "Environment";
  protected const string kEverythingLayerMask = "Everything";
  protected const string kNothingLayerMask = "Nothing";
  protected const string kDefaultMrcLayersMask = "DefaultMrcLayers";

  public RecordingToolConfigurationProcessor(
    [Inject] ProgramArguments programArguments,
    [Inject(Id = "RecordingTool")] IBeatSaberLogger logger,
    [Inject] IPosesSerializer posesSerializer,
    [Inject] RecordingToolResourceContainerSO resourceContainer)
  {
    this._programArguments = programArguments;
    this._logger = logger;
    this._posesSerializer = posesSerializer;
    this._resourceContainer = resourceContainer;
  }

  public virtual bool IsRecordingToolEnabled() => this._programArguments.arguments.Contains<string>("--enable_recording_tool");

  public virtual string GetConfigFilePath()
  {
    IReadOnlyList<string> arguments = this._programArguments.arguments;
    int index = 0;
    while (index < arguments.Count)
    {
      if (arguments[index++] == "--enable_recording_tool")
      {
        if (index == arguments.Count)
        {
          this._logger.LogError("Configuration json file for recording tool is not specified. Recording tool will be disabled.");
          return (string) null;
        }
        this._logger.Log("Recording tool is enabled by config file \"" + arguments[index] + "\".");
        return arguments[index];
      }
    }
    return (string) null;
  }

  public virtual string LoadConfigurationFile(string filePath)
  {
    if (filePath == null)
      return (string) null;
    try
    {
      return File.ReadAllText(filePath);
    }
    catch (Exception ex)
    {
      this._logger.LogError("Recording tool configuration file \"" + filePath + "\" cannot be loaded. Reason: " + ex.Message);
      return (string) null;
    }
  }

  public virtual RecordingToolConfigurationProcessor.RecordingToolConfiguration DeserializeConfigurationFile(
    string jsonData)
  {
    try
    {
      return JsonUtility.FromJson<RecordingToolConfigurationProcessor.RecordingToolConfiguration>(jsonData);
    }
    catch (Exception ex)
    {
      this._logger.LogError("Recording tool configuration file cannot be deserialized from json. Reason: " + ex.Message);
      return (RecordingToolConfigurationProcessor.RecordingToolConfiguration) null;
    }
  }

  public virtual RecordingToolConfigurationProcessor.RecordingToolConfiguration LoadConfiguration(
    string filePath)
  {
    return this.DeserializeConfigurationFile(this.LoadConfigurationFile(filePath));
  }

  public static LayerMask GetDefaultMrcLayersMask() => (LayerMask) ((int) (LayerMask) ((int) (LayerMask) ((int) (LayerMask) ((int) (LayerMask) ((int) (LayerMask)(-1) & -1025) & -65537) & -33554433) & -134217729) & int.MaxValue);

  public virtual LayerMask GetLayerMask(string layerName)
  {
    switch (layerName)
    {
      case "Everything":
        return (LayerMask)(-1);
      case "Nothing":
        return (LayerMask) 0;
      case "DefaultMrcLayers":
        return RecordingToolConfigurationProcessor.GetDefaultMrcLayersMask();
      default:
        int mask = LayerMask.GetMask(layerName);
        if (mask == 0)
          this._logger.LogWarning("Layer with name \"" + layerName + "\" does not exist. This layer will be skipped.");
        return (LayerMask) mask;
    }
  }

  public virtual LayerMask GetLayersMask(string[] layerNames)
  {
    LayerMask layersMask = (LayerMask) 0;
    if (layerNames == null)
      return layersMask;
    foreach (string layerName in layerNames)
      layersMask = (LayerMask) ((int) layersMask | (int) this.GetLayerMask(layerName));
    return layersMask;
  }

  private static (IBeatmapLevelPack, IPreviewBeatmapLevel) GetLevelPackAndLevelPreviewForLevelId(
    string packId,
    string levelId,
    IEnumerable<IBeatmapLevelPack> beatmapLevelPacks)
  {
    foreach (IBeatmapLevelPack beatmapLevelPack in beatmapLevelPacks)
    {
      if (packId == null || !(beatmapLevelPack.packID != packId))
      {
        foreach (IPreviewBeatmapLevel beatmapLevel in (IEnumerable<IPreviewBeatmapLevel>) beatmapLevelPack.beatmapLevelCollection.beatmapLevels)
        {
          if (beatmapLevel.levelID == levelId)
            return (beatmapLevelPack, beatmapLevel);
        }
        if (beatmapLevelPack.packID == packId)
          return (beatmapLevelPack, (IPreviewBeatmapLevel) null);
      }
    }
    return ((IBeatmapLevelPack) null, (IPreviewBeatmapLevel) null);
  }

  public virtual RecordingToolSettings CreateRecordingToolSettingsFromConfiguration(
    RecordingToolConfigurationProcessor.RecordingToolConfiguration recordingToolConfiguration)
  {
    if (recordingToolConfiguration == null)
      return (RecordingToolSettings) null;
    string gameMode = recordingToolConfiguration.mode == "Solo" ? "Solo" : (string) null;
    (IBeatmapLevelPack pack, IPreviewBeatmapLevel level) = RecordingToolConfigurationProcessor.GetLevelPackAndLevelPreviewForLevelId(recordingToolConfiguration.packID, recordingToolConfiguration.levelID, (IEnumerable<IBeatmapLevelPack>) this._resourceContainer.beatmapLevelPacks);
    BeatmapDifficulty difficulty;
    recordingToolConfiguration.difficulty.BeatmapDifficultyFromSerializedName(out difficulty);
    BeatmapCharacteristicSO bySerializedName1 = this._resourceContainer.beatmapCharacteristicCollection.GetBeatmapCharacteristicBySerializedName(recordingToolConfiguration.characteristic);
    if (gameMode != "Solo")
      this._logger.LogWarning("Game mode \"" + recordingToolConfiguration.mode + "\" is not recognized or is not supported yet as a command line parameter.");
    if (level == null)
      this._logger.LogWarning("Cannot find level with id \"" + recordingToolConfiguration.levelID + "\" selected via command line parameter.");
    if (pack == null)
      this._logger.LogWarning("Cannot find music pack by level id \"" + recordingToolConfiguration.levelID + "\" selected via command line parameter.");
    if (difficulty.ToString() != recordingToolConfiguration.difficulty)
      this._logger.LogWarning("Cannot find \"" + recordingToolConfiguration.difficulty + "\" difficulty. \"" + difficulty.ToString() + "\" difficulty will be used instead.");
    if ((UnityEngine.Object) bySerializedName1 == (UnityEngine.Object) null)
      this._logger.LogWarning("Cannot find \"" + recordingToolConfiguration.characteristic + "\" characteristic selected via command line parameter.");
    ObjectsMovementRecorder.Mode mode;
    recordingToolConfiguration.recordingMode.ModeFromSerializedName(out mode);
    if (mode == ObjectsMovementRecorder.Mode.Off)
      this._logger.LogError("Recording tool mode \"" + recordingToolConfiguration.recordingMode + "\" is not set or is set incorrectly. Recording tool will not be used.");
    if (mode == ObjectsMovementRecorder.Mode.Playback && !this._posesSerializer.RecordingExists(recordingToolConfiguration.recordingPath))
    {
      this._logger.LogError("Playback will not be started as selected recording does not exist.");
      mode = ObjectsMovementRecorder.Mode.Off;
    }
    string path = recordingToolConfiguration.recordingPath;
    if (!string.IsNullOrWhiteSpace(path) && recordingToolConfiguration.addDateTimeSuffixToRecordingName)
      path = path + "-" + DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss");
    if (mode == ObjectsMovementRecorder.Mode.Record && !this._posesSerializer.RecordingCanBeCreated(path))
    {
      this._logger.LogError("Record will not be started as recording cannot be created.");
      mode = ObjectsMovementRecorder.Mode.Off;
    }
    ObjectsMovementRecorder.CameraView cameraView;
    recordingToolConfiguration.cameraView.CameraViewFromSerializedName(out cameraView);
    List<PlaybackRenderer.PlaybackScreenshot> playbackScreenshotList = new List<PlaybackRenderer.PlaybackScreenshot>();
    if (recordingToolConfiguration.playbackScreenshots != null)
    {
      foreach (RecordingToolConfigurationProcessor.PlaybackScreenshot playbackScreenshot in recordingToolConfiguration.playbackScreenshots)
      {
        PlaybackRenderer.PlaybackScreenshot.Type type;
        if (!playbackScreenshot.type.PlaybackScreenshotTypeFromSerializedName(out type))
          this._logger.LogWarning("Screenshot type \"" + playbackScreenshot.type + "\" is not recognized. Background type will be used.");
        LayerMask layerMask1 = (LayerMask) 0;
        if (playbackScreenshot.includedLayers != null && playbackScreenshot.includedLayers.Length != 0)
        {
          LayerMask layerMask2 = (LayerMask) ((int) this.GetLayersMask(playbackScreenshot.includedLayers) & ~(int) this.GetLayersMask(playbackScreenshot.excludedLayers));
          if ((int) layerMask2 == 0)
          {
            this._logger.LogWarning("Screenshot cannot have empty layer mask. Screenshot with empty layer will be skipped.");
          }
          else
          {
            if (type == PlaybackRenderer.PlaybackScreenshot.Type.Foreground)
              layerMask2 = (LayerMask) ((int) layerMask2 | LayerMask.GetMask("MRForegroundClipPlane"));
            Color backgroundColor = new Color(playbackScreenshot.backgroundColor.r, playbackScreenshot.backgroundColor.g, playbackScreenshot.backgroundColor.b, 0.0f);
            playbackScreenshotList.Add(new PlaybackRenderer.PlaybackScreenshot(playbackScreenshot.name, layerMask2, type, backgroundColor));
          }
        }
        else
          this._logger.LogWarning("Screenshot cannot have empty included layers list. Screenshot without included layers will be skipped.");
      }
    }
    EnvironmentInfoSO bySerializedName2 = this._resourceContainer.environmentsList.GetEnvironmentInfoBySerializedName(recordingToolConfiguration.environmentName + "Environment");
    EnvironmentTypeSO environmentType = (UnityEngine.Object) bySerializedName2 != (UnityEngine.Object) null ? bySerializedName2.environmentType : (EnvironmentTypeSO) null;
    if (recordingToolConfiguration.overrideEnvironments && (UnityEngine.Object) bySerializedName2 == (UnityEngine.Object) null)
      this._logger.Log("Override Environments is enabled, but environment name is not set or set incorrectly. Override Environments will be disabled.");
    return new RecordingToolSettings(gameMode, pack, level, difficulty, bySerializedName1, recordingToolConfiguration.runLevel, mode, recordingToolConfiguration.recordingPath, cameraView, recordingToolConfiguration.addDateTimeSuffixToRecordingName, recordingToolConfiguration.screenshotRecording, recordingToolConfiguration.screenshotWidth, recordingToolConfiguration.screenshotHeight, recordingToolConfiguration.framerate, playbackScreenshotList.ToArray(), recordingToolConfiguration.practice, recordingToolConfiguration.startSongTime, recordingToolConfiguration.songSpeedMultiplier, recordingToolConfiguration.overrideEnvironments, environmentType, bySerializedName2, recordingToolConfiguration.saveToOldFormat);
  }

  public virtual ObjectsMovementRecorder.InitData CreateObjectsMovementRecorderInitDataFromConfiguration(
    RecordingToolSettings recordingToolSettings)
  {
    if (recordingToolSettings == null || recordingToolSettings.recordingMode == ObjectsMovementRecorder.Mode.Off)
      return (ObjectsMovementRecorder.InitData) null;
    int recordingMode = (int) recordingToolSettings.recordingMode;
    string recordingPath = recordingToolSettings.recordingPath;
    int cameraView = (int) recordingToolSettings.cameraView;
    int num1 = recordingToolSettings.addDateTimeSuffixToRecordingName ? 1 : 0;
    int num2 = recordingToolSettings.screenshotRecording ? 1 : 0;
    int screenshotWidth = recordingToolSettings.screenshotWidth;
    int screenshotHeight = recordingToolSettings.screenshotHeight;
    int framerate = recordingToolSettings.framerate;
    PlaybackRenderer.PlaybackScreenshot[] playbackScreenshots = recordingToolSettings.playbackScreenshots;
    int num3 = recordingToolSettings.saveToOldFormat ? 1 : 0;
    IPosesSerializer posesSerializer1 = this._posesSerializer;
    IBeatSaberLogger logger = this._logger;
    IPosesSerializer posesSerializer2 = posesSerializer1;
    return new ObjectsMovementRecorder.InitData((ObjectsMovementRecorder.Mode) recordingMode, recordingPath, (ObjectsMovementRecorder.CameraView) cameraView, num1 != 0, num2 != 0, screenshotWidth, screenshotHeight, framerate, playbackScreenshots, num3 != 0, logger, posesSerializer2);
  }

  public virtual MenuDestination CreateMenuDestinationFromConfiguration(
    RecordingToolSettings recordingToolSettings)
  {
    MenuDestination fromConfiguration = (MenuDestination) null;
    if (recordingToolSettings.gameMode == "Solo")
    {
      if (recordingToolSettings.level != null && recordingToolSettings.pack != null && (UnityEngine.Object) recordingToolSettings.characteristic != (UnityEngine.Object) null)
      {
        if (recordingToolSettings.runLevel)
        {
          bool practice = recordingToolSettings.practice || recordingToolSettings.recordingMode == ObjectsMovementRecorder.Mode.Playback;
          fromConfiguration = (MenuDestination) new RunLevelMenuDestination(recordingToolSettings.pack, recordingToolSettings.level, recordingToolSettings.difficulty, recordingToolSettings.characteristic, practice, recordingToolSettings.startSongTime, recordingToolSettings.songSpeedMultiplier, recordingToolSettings.overrideEnvironments && (UnityEngine.Object) recordingToolSettings.environmentInfo != (UnityEngine.Object) null, (UnityEngine.Object) recordingToolSettings.environmentType != (UnityEngine.Object) null ? recordingToolSettings.environmentType.name : (string) null, (UnityEngine.Object) recordingToolSettings.environmentInfo != (UnityEngine.Object) null ? recordingToolSettings.environmentInfo.serializedName : (string) null);
        }
        else
          fromConfiguration = (MenuDestination) new SelectLevelDestination(recordingToolSettings.pack, recordingToolSettings.level, recordingToolSettings.difficulty, recordingToolSettings.characteristic);
      }
      else
        fromConfiguration = recordingToolSettings.pack == null ? (MenuDestination) new SelectSubMenuDestination(SelectSubMenuDestination.Destination.SoloFreePlay) : (MenuDestination) new SelectLevelPackDestination(recordingToolSettings.pack);
    }
    return fromConfiguration;
  }

  [Serializable]
  public class ColorSaveData
  {
    public float r;
    public float g;
    public float b;
  }

  [Serializable]
  public class PlaybackScreenshot
  {
    public string name;
    public string type;
    public string[] includedLayers;
    public string[] excludedLayers;
    public RecordingToolConfigurationProcessor.ColorSaveData backgroundColor;
  }

  [Serializable]
  public class RecordingToolConfiguration
  {
    public string mode = "Solo";
    public string packID;
    public string levelID;
    public string difficulty;
    public string characteristic;
    public bool runLevel;
    public string recordingMode;
    public string recordingPath;
    public string cameraView = ObjectsMovementRecorder.CameraView.FirstPerson.SerializedName();
    public bool addDateTimeSuffixToRecordingName;
    public bool screenshotRecording;
    public int screenshotWidth = 1920;
    public int screenshotHeight = 1080;
    public int framerate = 30;
    public RecordingToolConfigurationProcessor.PlaybackScreenshot[] playbackScreenshots;
    public bool practice;
    public float startSongTime;
    public float songSpeedMultiplier = 1f;
    public bool overrideEnvironments;
    public string environmentType = "Normal";
    public string environmentName;
    public bool saveToOldFormat;
  }
}
