// Decompiled with JetBrains decompiler
// Type: VRControllersRecorderSaveAndLoad
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public abstract class VRControllersRecorderSaveAndLoad
{
  private static VRControllersRecorderSaveData LoadSaveDataFromFile(string filePath)
  {
    BinaryFormatter binaryFormatter = new BinaryFormatter();
    FileStream serializationStream = (FileStream) null;
    try
    {
      serializationStream = File.Open(filePath, FileMode.Open);
      return (VRControllersRecorderSaveData) binaryFormatter.Deserialize((Stream) serializationStream);
    }
    catch
    {
      return (VRControllersRecorderSaveData) null;
    }
    finally
    {
      serializationStream?.Close();
    }
  }

  private static VRControllersRecorderSaveData LoadSaveDataFromTextAsset(TextAsset textAsset) => new BinaryFormatter().Deserialize((Stream) new MemoryStream(textAsset.bytes)) as VRControllersRecorderSaveData;

  public static void LoadFromFile(string filePath, VRControllersRecorderData data)
  {
    VRControllersRecorderSaveData recorderSaveData = VRControllersRecorderSaveAndLoad.LoadSaveDataFromFile(filePath);
    if (recorderSaveData == null)
      Debug.LogWarning((object) ("Loading performance file failed (" + filePath + ")."));
    for (int index1 = 0; index1 < recorderSaveData.keyframes.Length; ++index1)
    {
      VRControllersRecorderData.PositionAndRotation[] positionsAndRotations = new VRControllersRecorderData.PositionAndRotation[data.nodesInfo.Length];
      VRControllersRecorderSaveData.Keyframe keyframe = recorderSaveData.keyframes[index1];
      for (int index2 = 0; index2 < recorderSaveData.nodesInfo.Length; ++index2)
      {
        VRControllersRecorderSaveData.NodeInfo nodeInfo1 = recorderSaveData.nodesInfo[index2];
        for (int index3 = 0; index3 < data.nodesInfo.Length; ++index3)
        {
          VRControllersRecorderData.NodeInfo nodeInfo2 = data.nodesInfo[index3];
          if (nodeInfo2.nodeIdx == nodeInfo1.nodeIdx && nodeInfo2.nodeType == nodeInfo1.nodeType)
          {
            VRControllersRecorderSaveData.PositionAndRotation positionsAndRotation = keyframe.positionsAndRotations[index2];
            positionsAndRotations[index3] = new VRControllersRecorderData.PositionAndRotation(new Vector3(positionsAndRotation.posX, positionsAndRotation.posY, positionsAndRotation.posZ), new Quaternion(positionsAndRotation.rotX, positionsAndRotation.rotY, positionsAndRotation.rotZ, positionsAndRotation.rotW));
            break;
          }
        }
      }
      data.AddKeyFrame(positionsAndRotations, keyframe.time);
    }
  }

  public static void SaveToFile(string filePath, VRControllersRecorderData data)
  {
    VRControllersRecorderSaveData recorderSaveData = new VRControllersRecorderSaveData();
    List<VRControllersRecorderSaveData.NodeInfo> nodeInfoList = new List<VRControllersRecorderSaveData.NodeInfo>(data.nodesInfo.Length);
    foreach (VRControllersRecorderData.NodeInfo nodeInfo in data.nodesInfo)
      nodeInfoList.Add(new VRControllersRecorderSaveData.NodeInfo()
      {
        nodeType = nodeInfo.nodeType,
        nodeIdx = nodeInfo.nodeIdx
      });
    recorderSaveData.nodesInfo = nodeInfoList.ToArray();
    List<VRControllersRecorderSaveData.Keyframe> keyframeList = new List<VRControllersRecorderSaveData.Keyframe>(data.numberOfKeyframes);
    for (int frameIdx = 0; frameIdx < data.numberOfKeyframes; ++frameIdx)
    {
      VRControllersRecorderSaveData.Keyframe keyframe = new VRControllersRecorderSaveData.Keyframe();
      keyframe.time = data.GetFrameTime(frameIdx);
      keyframe.positionsAndRotations = new VRControllersRecorderSaveData.PositionAndRotation[data.nodesInfo.Length];
      for (int index = 0; index < data.nodesInfo.Length; ++index)
      {
        VRControllersRecorderData.NodeInfo nodeInfo = data.nodesInfo[index];
        VRControllersRecorderData.PositionAndRotation positionAndRotation = data.GetPositionAndRotation(frameIdx, nodeInfo.nodeType, nodeInfo.nodeIdx);
        keyframe.positionsAndRotations[index] = new VRControllersRecorderSaveData.PositionAndRotation()
        {
          posX = positionAndRotation.pos.x,
          posY = positionAndRotation.pos.y,
          posZ = positionAndRotation.pos.z,
          rotX = positionAndRotation.rot.x,
          rotY = positionAndRotation.rot.y,
          rotZ = positionAndRotation.rot.z,
          rotW = positionAndRotation.rot.w
        };
      }
      keyframeList.Add(keyframe);
    }
    recorderSaveData.keyframes = keyframeList.ToArray();
    BinaryFormatter binaryFormatter = new BinaryFormatter();
    FileStream fileStream = File.Open(filePath, FileMode.OpenOrCreate);
    FileStream serializationStream = fileStream;
    VRControllersRecorderSaveData graph = recorderSaveData;
    binaryFormatter.Serialize((Stream) serializationStream, (object) graph);
    fileStream.Close();
  }
}
