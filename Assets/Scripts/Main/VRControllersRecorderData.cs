// Decompiled with JetBrains decompiler
// Type: VRControllersRecorderData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class VRControllersRecorderData
{
  public readonly VRControllersRecorderData.NodeInfo[] nodesInfo;
  protected readonly List<VRControllersRecorderData.Keyframe> _keyframes;

  public VRControllersRecorderData(VRControllersRecorderData.NodeInfo[] nodesInfo)
  {
    this.nodesInfo = nodesInfo;
    this._keyframes = new List<VRControllersRecorderData.Keyframe>();
  }

  public virtual void AddKeyFrame(
    VRControllersRecorderData.PositionAndRotation[] positionsAndRotations,
    float time)
  {
    this._keyframes.Add(new VRControllersRecorderData.Keyframe(positionsAndRotations, time));
  }

  public virtual VRControllersRecorderData.PositionAndRotation GetPositionAndRotation(
    int frameIdx,
    XRNode nodeType,
    int nodeIdx)
  {
    for (int index = 0; index < this.nodesInfo.Length; ++index)
    {
      VRControllersRecorderData.NodeInfo nodeInfo = this.nodesInfo[index];
      if (nodeInfo.nodeType == nodeType && nodeInfo.nodeIdx == nodeIdx)
        return this._keyframes[frameIdx].positionsAndRotations[index];
    }
    return new VRControllersRecorderData.PositionAndRotation();
  }

  public virtual VRControllersRecorderData.PositionAndRotation GetLerpedPositionAndRotation(
    int frameIdx,
    float t,
    XRNode nodeType,
    int nodeIdx)
  {
    return VRControllersRecorderData.PositionAndRotation.Lerp(this.GetPositionAndRotation(frameIdx, nodeType, nodeIdx), this.GetPositionAndRotation(frameIdx + 1, nodeType, nodeIdx), t);
  }

  public virtual float GetFrameTime(int frameIdx) => this._keyframes[frameIdx].time;

  public int numberOfKeyframes => this._keyframes.Count;

  public struct PositionAndRotation
  {
    public readonly Vector3 pos;
    public readonly Quaternion rot;

    public PositionAndRotation(Vector3 pos, Quaternion rot)
    {
      this.pos = pos;
      this.rot = rot;
    }

    public static VRControllersRecorderData.PositionAndRotation Lerp(
      VRControllersRecorderData.PositionAndRotation a,
      VRControllersRecorderData.PositionAndRotation b,
      float t)
    {
      return new VRControllersRecorderData.PositionAndRotation(Vector3.Lerp(a.pos, b.pos, t), Quaternion.Slerp(a.rot, b.rot, t));
    }
  }

  public class Keyframe
  {
    public readonly VRControllersRecorderData.PositionAndRotation[] positionsAndRotations;
    public readonly float time;

    public Keyframe(
      VRControllersRecorderData.PositionAndRotation[] positionAndRotations,
      float time)
    {
      this.positionsAndRotations = positionAndRotations;
      this.time = time;
    }
  }

  public class NodeInfo
  {
    public readonly XRNode nodeType;
    public readonly int nodeIdx;

    public NodeInfo(XRNode nodeType, int nodeIdx)
    {
      this.nodeType = nodeType;
      this.nodeIdx = nodeIdx;
    }
  }
}
