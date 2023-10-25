// Decompiled with JetBrains decompiler
// Type: SimpleOpenVrOpenVrHaptics
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System;
using UnityEngine.XR;
using Valve.VR;

public class SimpleOpenVrOpenVrHaptics : IOpenVRHaptics
{
  public virtual void TriggerHapticPulse(
    XRNode node,
    float duration,
    float strength,
    float frequency)
  {
    CVRSystem system = OpenVR.System;
    if (system == null)
      return;
    float usDurationMicroSec = Math.Min(duration * 1000000f, 4000f * strength);
    ETrackedControllerRole unDeviceType = node == XRNode.LeftHand ? ETrackedControllerRole.LeftHand : ETrackedControllerRole.RightHand;
    uint forControllerRole = system.GetTrackedDeviceIndexForControllerRole(unDeviceType);
    system.TriggerHapticPulse(forControllerRole, 0U, (char) usDurationMicroSec);
  }

  public virtual void Destroy()
  {
  }
}
