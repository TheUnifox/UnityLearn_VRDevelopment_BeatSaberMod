// Decompiled with JetBrains decompiler
// Type: IVRPlatformHelper
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;
using UnityEngine.XR;

public interface IVRPlatformHelper
{
  event System.Action inputFocusWasCapturedEvent;

  event System.Action inputFocusWasReleasedEvent;

  event System.Action vrFocusWasCapturedEvent;

  event System.Action vrFocusWasReleasedEvent;

  event System.Action hmdUnmountedEvent;

  event System.Action hmdMountedEvent;

  event System.Action joystickWasCenteredThisFrameEvent;

  event System.Action<Vector2> joystickWasNotCenteredThisFrameEvent;

  bool hasInputFocus { get; }

  bool hasVrFocus { get; }

  bool isAlwaysWireless { get; }

  VRPlatformSDK vrPlatformSDK { get; }

  XRDeviceModel currentXRDeviceModel { get; }

  void TriggerHapticPulse(XRNode node, float duration, float strength, float frequency);

  void StopHaptics(XRNode node);

  void AdjustControllerTransform(
    XRNode node,
    Transform transform,
    Vector3 position,
    Vector3 rotation);

  bool GetNodePose(XRNode nodeType, int idx, out Vector3 pos, out Quaternion rot);
}
