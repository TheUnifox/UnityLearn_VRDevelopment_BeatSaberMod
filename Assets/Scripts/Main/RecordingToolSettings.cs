// Decompiled with JetBrains decompiler
// Type: RecordingToolSettings
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Text;
using UnityEngine;

public class RecordingToolSettings
{
  public string gameMode;
  public IBeatmapLevelPack pack;
  public IPreviewBeatmapLevel level;
  public BeatmapDifficulty difficulty;
  public BeatmapCharacteristicSO characteristic;
  public bool runLevel;
  public ObjectsMovementRecorder.Mode recordingMode;
  public string recordingPath;
  public ObjectsMovementRecorder.CameraView cameraView;
  public bool addDateTimeSuffixToRecordingName;
  public bool screenshotRecording;
  public int screenshotWidth;
  public int screenshotHeight;
  public int framerate;
  public PlaybackRenderer.PlaybackScreenshot[] playbackScreenshots;
  public bool practice;
  public float startSongTime;
  public float songSpeedMultiplier;
  public bool overrideEnvironments;
  public EnvironmentTypeSO environmentType;
  public EnvironmentInfoSO environmentInfo;
  public bool saveToOldFormat;

  public RecordingToolSettings(
    string gameMode,
    IBeatmapLevelPack pack,
    IPreviewBeatmapLevel level,
    BeatmapDifficulty difficulty,
    BeatmapCharacteristicSO characteristic,
    bool runLevel,
    ObjectsMovementRecorder.Mode recordingMode,
    string recordingPath,
    ObjectsMovementRecorder.CameraView cameraView,
    bool addDateTimeSuffixToRecordingName,
    bool screenshotRecording,
    int screenshotWidth,
    int screenshotHeight,
    int framerate,
    PlaybackRenderer.PlaybackScreenshot[] playbackScreenshots,
    bool practice,
    float startSongTime,
    float songSpeedMultiplier,
    bool overrideEnvironments,
    EnvironmentTypeSO environmentType,
    EnvironmentInfoSO environmentInfo,
    bool saveToOldFormat)
  {
    this.gameMode = gameMode;
    this.pack = pack;
    this.level = level;
    this.difficulty = difficulty;
    this.characteristic = characteristic;
    this.runLevel = runLevel;
    this.recordingMode = recordingMode;
    this.recordingPath = recordingPath;
    this.cameraView = cameraView;
    this.addDateTimeSuffixToRecordingName = addDateTimeSuffixToRecordingName;
    this.screenshotRecording = screenshotRecording;
    this.screenshotWidth = screenshotWidth;
    this.screenshotHeight = screenshotHeight;
    this.framerate = framerate;
    this.playbackScreenshots = playbackScreenshots;
    this.practice = practice;
    this.startSongTime = startSongTime;
    this.songSpeedMultiplier = songSpeedMultiplier;
    this.overrideEnvironments = overrideEnvironments;
    this.environmentType = environmentType;
    this.environmentInfo = environmentInfo;
    this.saveToOldFormat = saveToOldFormat;
  }

  public override string ToString()
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("gameMode: " + this.gameMode + "\n");
    string str1 = this.pack != null ? this.pack.packID : "null";
    stringBuilder.Append("pack: " + str1 + "\n");
    string str2 = this.level != null ? this.level.levelID : "null";
    stringBuilder.Append("level: " + str2 + "\n");
    stringBuilder.Append(string.Format("difficulty: {0}\n", (object) this.difficulty));
    string str3 = (Object) this.characteristic != (Object) null ? this.characteristic.serializedName : "null";
    stringBuilder.Append("characteristic: " + str3 + "\n");
    stringBuilder.Append(string.Format("runLevel: {0}\n", (object) this.runLevel));
    stringBuilder.Append(string.Format("recordingMode: {0}\n", (object) this.recordingMode));
    stringBuilder.Append("recordingPath: " + this.recordingPath + "\n");
    stringBuilder.Append(string.Format("cameraView: {0}\n", (object) this.cameraView));
    stringBuilder.Append(string.Format("addDateTimeSuffixToRecordingName: {0}\n", (object) this.addDateTimeSuffixToRecordingName));
    stringBuilder.Append(string.Format("screenshotRecording: {0}\n", (object) this.screenshotRecording));
    stringBuilder.Append(string.Format("screenshotWidth: {0}\n", (object) this.screenshotWidth));
    stringBuilder.Append(string.Format("screenshotHeight: {0}\n", (object) this.screenshotHeight));
    stringBuilder.Append(string.Format("framerate: {0}\n", (object) this.framerate));
    stringBuilder.Append("playbackScreenshots: [\n");
    foreach (PlaybackRenderer.PlaybackScreenshot playbackScreenshot in this.playbackScreenshots)
    {
      stringBuilder.Append("    {\n");
      stringBuilder.Append("        name: " + playbackScreenshot.name + "\n");
      stringBuilder.Append(string.Format("        type: {0}\n", (object) playbackScreenshot.type));
      stringBuilder.Append(string.Format("        mask: {0}\n", (object) playbackScreenshot.layerMask.value));
      stringBuilder.Append(string.Format("        color: ({0}, {1}, {2})\n", (object) playbackScreenshot.backgroundColor.r, (object) playbackScreenshot.backgroundColor.g, (object) playbackScreenshot.backgroundColor.b));
      stringBuilder.Append("    }\n");
    }
    stringBuilder.Append("]\n");
    stringBuilder.Append(string.Format("practice: {0}\n", (object) this.practice));
    stringBuilder.Append(string.Format("startSongTime: {0}\n", (object) this.startSongTime));
    stringBuilder.Append(string.Format("songSpeedMultiplier: {0}\n", (object) this.songSpeedMultiplier));
    stringBuilder.Append(string.Format("overrideEnvironments: {0}\n", (object) this.overrideEnvironments));
    string str4 = (Object) this.environmentType != (Object) null ? this.environmentType.name : "null";
    stringBuilder.Append("environmentType: " + str4 + "\n");
    string str5 = (Object) this.environmentInfo != (Object) null ? this.environmentInfo.environmentName : "null";
    stringBuilder.Append("environmentName: " + str5 + "\n");
    stringBuilder.Append(string.Format("saveToOldFormat: {0}\n", (object) this.saveToOldFormat));
    return stringBuilder.ToString();
  }
}
