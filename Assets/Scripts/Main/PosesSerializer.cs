// Decompiled with JetBrains decompiler
// Type: PosesSerializer
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Zenject;

public class PosesSerializer : IPosesSerializer
{
  protected const string kInfoFileName = "Info.json";
  protected const string kDataFileName = "Data.rcd";
  protected readonly IBeatSaberLogger _logger;
  protected readonly RecordingConverter _recordingConverter;

  public PosesSerializer([Inject(Id = "RecordingTool")] IBeatSaberLogger logger)
  {
    this._logger = logger;
    this._recordingConverter = new RecordingConverter(this._logger);
  }

  private static void SaveInfoFile(string filePath, PosesRecordingData data)
  {
    PosesRecordingInfoSaveData.ExternalCameraCalibrationSaveData externalCameraCalibration = (PosesRecordingInfoSaveData.ExternalCameraCalibrationSaveData) null;
    if (data.externalCameraCalibration != null)
      externalCameraCalibration = new PosesRecordingInfoSaveData.ExternalCameraCalibrationSaveData(data.externalCameraCalibration.fieldOfVision, data.externalCameraCalibration.nearClip, data.externalCameraCalibration.farClip, data.externalCameraCalibration.hmdOffset, data.externalCameraCalibration.nearOffset);
    PosesRecordingInfoSaveData recordingInfoSaveData = new PosesRecordingInfoSaveData(data.objectIds, externalCameraCalibration, "Data.rcd");
    File.WriteAllText(filePath, JsonUtility.ToJson((object) recordingInfoSaveData, true));
  }

  private static void SaveDataFile(string filePath, PosesRecordingData data)
  {
    FileStream output = File.Open(filePath, FileMode.Create);
    BinaryWriter binaryWriter = new BinaryWriter((Stream) output, Encoding.UTF8, false);
    binaryWriter.Write(data.keyframes.Count);
    binaryWriter.Write(data.objectIds.Length);
    foreach (PosesRecordingData.TransformsKeyframe keyframe in data.keyframes)
    {
      binaryWriter.Write(keyframe.time);
      foreach (Pose pose in keyframe.poses)
      {
        binaryWriter.Write(pose.position.x);
        binaryWriter.Write(pose.position.y);
        binaryWriter.Write(pose.position.z);
        binaryWriter.Write(pose.rotation.x);
        binaryWriter.Write(pose.rotation.y);
        binaryWriter.Write(pose.rotation.z);
        binaryWriter.Write(pose.rotation.w);
      }
    }
    output.Close();
  }

  public virtual void SaveRecordingIntoDirectory(string path, PosesRecordingData data)
  {
    if (data?.keyframes == null || data.keyframes.Count == 0)
      this._logger.LogWarning("Recording was not saved. Data is null or empty.");
    else if (string.IsNullOrEmpty(path))
    {
      this._logger.LogWarning("Recording was not saved to files. Path is null or empty.");
    }
    else
    {
      if (!string.IsNullOrEmpty(path))
        Directory.CreateDirectory(path);
      PosesSerializer.SaveInfoFile(Path.Combine(path, "Info.json"), data);
      PosesSerializer.SaveDataFile(Path.Combine(path, "Data.rcd"), data);
      this._logger.Log("Recording was saved to " + path + ".");
    }
  }

  public virtual PosesRecordingInfoSaveData LoadInfoFile(string filePath)
  {
    try
    {
      return JsonUtility.FromJson<PosesRecordingInfoSaveData>(File.ReadAllText(filePath));
    }
    catch (Exception ex)
    {
      this._logger.LogWarning("Recording info file " + filePath + " cannot be loaded. Exception: " + ex.Message);
      return (PosesRecordingInfoSaveData) null;
    }
  }

  public virtual List<PosesRecordingData.TransformsKeyframe> LoadDataFile(string filePath)
  {
    if (!File.Exists(filePath))
    {
      this._logger.LogWarning("Recording data file " + filePath + " is not found.");
      return (List<PosesRecordingData.TransformsKeyframe>) null;
    }
    FileStream input = File.Open(filePath, FileMode.Open);
    BinaryReader binaryReader = new BinaryReader((Stream) input, Encoding.UTF8, false);
    List<PosesRecordingData.TransformsKeyframe> transformsKeyframeList;
    try
    {
      int num = binaryReader.ReadInt32();
      int length = binaryReader.ReadInt32();
      transformsKeyframeList = new List<PosesRecordingData.TransformsKeyframe>();
      for (int index1 = 0; index1 < num; ++index1)
      {
        float time = binaryReader.ReadSingle();
        Pose[] poses = new Pose[length];
        for (int index2 = 0; index2 < length; ++index2)
          poses[index2] = new Pose(new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle()), new Quaternion(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle()));
        transformsKeyframeList.Add(new PosesRecordingData.TransformsKeyframe(poses, time));
      }
    }
    catch (Exception ex)
    {
      this._logger.LogWarning("Recording data file " + filePath + " cannot be loaded. Reason: " + ex.Message);
      transformsKeyframeList = (List<PosesRecordingData.TransformsKeyframe>) null;
    }
    input.Close();
    return transformsKeyframeList;
  }

  public virtual PosesRecordingData LoadRecordingFromDirectory(string path)
  {
    PosesRecordingInfoSaveData recordingInfoSaveData = this.LoadInfoFile(Path.Combine(path, "Info.json"));
    List<PosesRecordingData.TransformsKeyframe> keyframes = this.LoadDataFile(Path.Combine(path, recordingInfoSaveData?.dataFileName ?? "Data.rcd"));
    if (recordingInfoSaveData == null || keyframes == null)
    {
      this._logger.LogWarning("Cannot read info and/or data files for selected recording " + path + ".");
      return (PosesRecordingData) null;
    }
    string[] objectIds = new string[recordingInfoSaveData.objectIds.Length];
    for (int index = 0; index < recordingInfoSaveData.objectIds.Length; ++index)
      objectIds[index] = recordingInfoSaveData.objectIds[index];
    PosesRecordingData.ExternalCameraCalibration externalCameraCalibration = (PosesRecordingData.ExternalCameraCalibration) null;
    if (recordingInfoSaveData.externalCameraCalibration != null)
      externalCameraCalibration = new PosesRecordingData.ExternalCameraCalibration(recordingInfoSaveData.externalCameraCalibration.fieldOfVision, recordingInfoSaveData.externalCameraCalibration.nearClip, recordingInfoSaveData.externalCameraCalibration.farClip, recordingInfoSaveData.externalCameraCalibration.hmdOffset, recordingInfoSaveData.externalCameraCalibration.nearOffset);
    return new PosesRecordingData(objectIds, keyframes, externalCameraCalibration);
  }

  public virtual void SaveToOldFormat(string path, PosesRecordingData data) => this._recordingConverter.SaveToOldFormat(path, data);

  public virtual void SaveRecording(string path, PosesRecordingData data, bool saveToOldFormat)
  {
    this.SaveRecordingIntoDirectory(path, data);
    if (!saveToOldFormat)
      return;
    this.SaveToOldFormat(path, data);
  }

  public virtual PosesRecordingData LoadRecording(string path)
  {
    if (Directory.Exists(path))
      return this.LoadRecordingFromDirectory(path);
    this._logger.LogWarning("Selected recording " + path + " is not exist.");
    return (PosesRecordingData) null;
  }

  public virtual bool RecordingExists(string path)
  {
    if (string.IsNullOrWhiteSpace(path))
    {
      this._logger.Log("Recording path \"" + path + "\" is not set or is set incorrectly.");
      return false;
    }
    if (!Directory.Exists(path))
    {
      this._logger.Log("Recording \"" + path + "\" is not exist.");
      return false;
    }
    bool flag = true;
    string path1 = Path.Combine(path, "Info.json");
    if (!File.Exists(path1))
    {
      this._logger.Log("Recording info file \"" + path1 + "\" is not exist.");
      flag = false;
    }
    string path2 = Path.Combine(path, "Data.rcd");
    if (!File.Exists(path2))
    {
      this._logger.Log("Recording data file \"" + path2 + "\" is not exist.");
      flag = false;
    }
    return flag;
  }

  public virtual bool RecordingCanBeCreated(string path)
  {
    if (string.IsNullOrWhiteSpace(path))
    {
      this._logger.Log("Recording path \"" + path + "\" is not set or is set incorrectly.");
      return false;
    }
    bool flag = true;
    string path1 = Path.Combine(path, "Info.json");
    if (File.Exists(path1))
    {
      if (!FileSystemHelper.IsFileWritable(path1))
      {
        this._logger.Log("Recording info file \"" + path1 + "\" is not writable.");
        flag = false;
      }
    }
    string path2 = Path.Combine(path, "Data.rcd");
    if (File.Exists(path2))
    {
      if (!FileSystemHelper.IsFileWritable(path2))
      {
        this._logger.Log("Recording data file \"" + path2 + "\" is not writable.");
        flag = false;
      }
    }
    return flag;
  }
}
