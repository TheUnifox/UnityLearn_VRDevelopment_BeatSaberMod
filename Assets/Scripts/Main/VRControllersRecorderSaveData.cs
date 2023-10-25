// Decompiled with JetBrains decompiler
// Type: VRControllersRecorderSaveData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine.XR;

[Serializable]
public class VRControllersRecorderSaveData
{
  public VRControllersRecorderSaveData.NodeInfo[] nodesInfo;
  public VRControllersRecorderSaveData.Keyframe[] keyframes;

  [Serializable]
  public class PositionAndRotation
  {
    public float posX;
    public float posY;
    public float posZ;
    public float rotX;
    public float rotY;
    public float rotZ;
    public float rotW;
  }

  [Serializable]
  public class Keyframe
  {
    public VRControllersRecorderSaveData.PositionAndRotation[] positionsAndRotations;
    public float time;
  }

  [Serializable]
  public class NodeInfo
  {
    public XRNode nodeType;
    public int nodeIdx;
  }
}
