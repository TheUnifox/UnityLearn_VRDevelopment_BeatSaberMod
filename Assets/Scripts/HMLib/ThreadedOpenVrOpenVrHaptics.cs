// Decompiled with JetBrains decompiler
// Type: ThreadedOpenVrOpenVrHaptics
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System;
using System.Threading;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

public class ThreadedOpenVrOpenVrHaptics : IOpenVRHaptics
{
  protected float _currentTime;
  protected Thread _hapticThread;
  protected ThreadedOpenVrOpenVrHaptics.OpenVrHapticData _leftHandHaptics;
  protected ThreadedOpenVrOpenVrHaptics.OpenVrHapticData _rightHandHaptics;

  public ThreadedOpenVrOpenVrHaptics()
  {
    this._currentTime = Time.time;
    this._hapticThread = new Thread(new ThreadStart(this.UpdateHaptics));
    this._hapticThread.Start();
  }

  public virtual void TriggerHapticPulse(
    XRNode node,
    float duration,
    float strength,
    float frequency)
  {
    ThreadedOpenVrOpenVrHaptics.OpenVrHapticData openVrHapticData = new ThreadedOpenVrOpenVrHaptics.OpenVrHapticData(this._currentTime + duration, strength);
    if (node == XRNode.LeftHand)
      this._leftHandHaptics = openVrHapticData;
    else
      this._rightHandHaptics = openVrHapticData;
  }

  public virtual void UpdateHaptics()
  {
    long num = 0;
    while (true)
    {
      long ticks;
      do
      {
        ticks = DateTime.Now.Ticks;
      }
      while (ticks <= num);
      this.UpdateHandHaptics(XRNode.LeftHand);
      this.UpdateHandHaptics(XRNode.RightHand);
      this._currentTime += 0.004f;
      num = ticks + 40000L;
    }
  }

  public virtual void UpdateHandHaptics(XRNode node)
  {
    CVRSystem system = OpenVR.System;
    if (system == null)
      return;
    ThreadedOpenVrOpenVrHaptics.OpenVrHapticData openVrHapticData = node == XRNode.LeftHand ? this._leftHandHaptics : this._rightHandHaptics;
    float num = openVrHapticData.endTime - this._currentTime;
    if ((double) num < 0.0)
      return;
    float usDurationMicroSec = Math.Min(num * 1000000f, 4000f * openVrHapticData.strength);
    ETrackedControllerRole unDeviceType = node == XRNode.LeftHand ? ETrackedControllerRole.LeftHand : ETrackedControllerRole.RightHand;
    uint forControllerRole = system.GetTrackedDeviceIndexForControllerRole(unDeviceType);
    system.TriggerHapticPulse(forControllerRole, 0U, (char) usDurationMicroSec);
  }

  public virtual void Destroy() => this._hapticThread.Abort();

  public readonly struct OpenVrHapticData
  {
    public readonly float endTime;
    public readonly float strength;

    public OpenVrHapticData(float endTime, float strength)
    {
      this.endTime = endTime;
      this.strength = strength;
    }
  }
}
