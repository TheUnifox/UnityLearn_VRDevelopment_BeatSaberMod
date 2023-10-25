// Decompiled with JetBrains decompiler
// Type: PSVRHelper
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PSVRHelper : MonoBehaviour, IVRPlatformHelper
{
  protected const float kContinuesRumbleImpulseStrength = 0.8f;
  protected bool _didGetNodeStatesThisFrame;
  protected readonly List<XRNodeState> _nodeStates = new List<XRNodeState>(10);
  protected bool _hasVrFocus = true;

  public event System.Action inputFocusWasCapturedEvent;

  public event System.Action inputFocusWasReleasedEvent;

  public event System.Action vrFocusWasCapturedEvent;

  public event System.Action vrFocusWasReleasedEvent;

  public event System.Action hmdUnmountedEvent;

  public event System.Action hmdMountedEvent;

  public event System.Action joystickWasCenteredThisFrameEvent;

  public event System.Action<Vector2> joystickWasNotCenteredThisFrameEvent;

  public bool hasInputFocus => true;

  public bool hasVrFocus => this._hasVrFocus;

  public bool isAlwaysWireless => false;

  public VRPlatformSDK vrPlatformSDK => VRPlatformSDK.Unknown;

  public XRDeviceModel currentXRDeviceModel => XRDeviceModel.Unknown;

  public virtual void Update()
  {
  }

  public virtual void LateUpdate() => this._didGetNodeStatesThisFrame = false;

  public virtual void TriggerHapticPulse(
    XRNode node,
    float duration,
    float strength,
    float frequency)
  {
    strength *= 0.8f;
  }

  public virtual void StopHaptics(XRNode node)
  {
  }

  public virtual void AdjustControllerTransform(
    XRNode node,
    Transform transform,
    Vector3 position,
    Vector3 rotation)
  {
  }

  public virtual bool GetNodePose(XRNode nodeType, int idx, out Vector3 pos, out Quaternion rot)
  {
    pos = Vector3.zero;
    rot = Quaternion.identity;
    if (nodeType == XRNode.LeftHand || nodeType == XRNode.RightHand)
    {
      nodeType.PSMoveDeviceIndex();
    }
    else
    {
      if (!this._didGetNodeStatesThisFrame)
      {
        InputTracking.GetNodeStates(this._nodeStates);
        this._didGetNodeStatesThisFrame = true;
      }
      pos = Vector3.zero;
      rot = Quaternion.identity;
      int num = 0;
      foreach (XRNodeState nodeState in this._nodeStates)
      {
        if (nodeState.nodeType == nodeType && idx == num)
          return nodeState.TryGetPosition(out pos) & nodeState.TryGetRotation(out rot);
        if (nodeState.nodeType == nodeType)
          ++num;
      }
    }
    return pos != Vector3.zero;
  }
}
