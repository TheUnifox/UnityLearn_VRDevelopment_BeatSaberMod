// Decompiled with JetBrains decompiler
// Type: DevicelessVRHelper
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;
using UnityEngine.XR;

public class DevicelessVRHelper : MonoBehaviour, IVRPlatformHelper
{
  protected bool _hasInputFocus = true;
  protected bool _hasVrFocus = true;
  protected bool _scrollingLastFrame;

  public event System.Action inputFocusWasCapturedEvent;

  public event System.Action inputFocusWasReleasedEvent;

  public event System.Action vrFocusWasCapturedEvent;

  public event System.Action vrFocusWasReleasedEvent;

  public event System.Action hmdUnmountedEvent;

  public event System.Action hmdMountedEvent;

  public event System.Action joystickWasCenteredThisFrameEvent;

  public event System.Action<Vector2> joystickWasNotCenteredThisFrameEvent;

  public bool hasInputFocus => this._hasInputFocus;

  public bool hasVrFocus => this._hasVrFocus;

  public bool isAlwaysWireless => true;

  public VRPlatformSDK vrPlatformSDK => VRPlatformSDK.Unknown;

  public XRDeviceModel currentXRDeviceModel => XRDeviceModel.Unknown;

  public virtual void Update()
  {
    Vector2 vector2 = new Vector2(Input.GetAxis("Mouse ScrollWheel"), Input.GetAxis("Mouse ScrollWheel"));
    if ((double) vector2.sqrMagnitude > 0.0099999997764825821)
    {
      this._scrollingLastFrame = true;
      System.Action<Vector2> centeredThisFrameEvent = this.joystickWasNotCenteredThisFrameEvent;
      if (centeredThisFrameEvent != null)
        centeredThisFrameEvent(vector2);
    }
    else if (this._scrollingLastFrame)
    {
      this._scrollingLastFrame = false;
      System.Action centeredThisFrameEvent = this.joystickWasCenteredThisFrameEvent;
      if (centeredThisFrameEvent != null)
        centeredThisFrameEvent();
    }
    if (Input.GetKeyDown(KeyCode.U))
    {
      System.Action hmdUnmountedEvent = this.hmdUnmountedEvent;
      if (hmdUnmountedEvent != null)
        hmdUnmountedEvent();
    }
    if (Input.GetKeyDown(KeyCode.M))
    {
      System.Action hmdMountedEvent = this.hmdMountedEvent;
      if (hmdMountedEvent != null)
        hmdMountedEvent();
    }
    if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.I))
    {
      this._hasInputFocus = false;
      System.Action wasCapturedEvent = this.inputFocusWasCapturedEvent;
      if (wasCapturedEvent != null)
        wasCapturedEvent();
    }
    if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.O))
    {
      this._hasInputFocus = true;
      System.Action wasReleasedEvent = this.inputFocusWasReleasedEvent;
      if (wasReleasedEvent != null)
        wasReleasedEvent();
    }
    if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.V))
    {
      this._hasVrFocus = false;
      System.Action wasCapturedEvent = this.vrFocusWasCapturedEvent;
      if (wasCapturedEvent != null)
        wasCapturedEvent();
    }
    if (!Input.GetKey(KeyCode.LeftShift) || !Input.GetKeyDown(KeyCode.B))
      return;
    this._hasVrFocus = true;
    System.Action wasReleasedEvent1 = this.vrFocusWasReleasedEvent;
    if (wasReleasedEvent1 == null)
      return;
    wasReleasedEvent1();
  }

  public virtual void TriggerHapticPulse(
    XRNode node,
    float duration,
    float strength,
    float frequency)
  {
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
    return true;
  }
}
