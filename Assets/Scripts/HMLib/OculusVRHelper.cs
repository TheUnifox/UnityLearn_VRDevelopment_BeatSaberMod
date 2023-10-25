// Decompiled with JetBrains decompiler
// Type: OculusVRHelper
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;

public class OculusVRHelper : MonoBehaviour, IVRPlatformHelper
{
  protected bool _hasInputFocus;
  protected bool _hasVrFocus;
  protected bool _userPresent;
  protected bool _scrollingLastFrame;
  protected EventSystem _disabledEventSystem;

  public event System.Action inputFocusWasCapturedEvent;

  public event System.Action inputFocusWasReleasedEvent;

  public event System.Action vrFocusWasCapturedEvent;

  public event System.Action vrFocusWasReleasedEvent;

  public event System.Action hmdUnmountedEvent;

  public event System.Action hmdMountedEvent;

  public event System.Action joystickWasCenteredThisFrameEvent;

  public event System.Action<Vector2> joystickWasNotCenteredThisFrameEvent;

  public bool hasInputFocus => OVRPlugin.hasInputFocus;

  public bool hasVrFocus => OVRPlugin.hasVrFocus;

  public bool isAlwaysWireless => this.currentXRDeviceModel == XRDeviceModel.OculusQuest;

  public VRPlatformSDK vrPlatformSDK => VRPlatformSDK.Oculus;

  public XRDeviceModel currentXRDeviceModel => VRPlatformUtils.GetXRDeviceModel();

  private bool _isOVRManagerPresent => (UnityEngine.Object) OVRManager.instance != (UnityEngine.Object) null;

  public virtual void Update()
  {
    if (OVRPlugin.shouldQuit)
      Application.Quit();
    if (OVRPlugin.shouldRecenter)
      OVRPlugin.RecenterTrackingOrigin(OVRPlugin.RecenterFlags.Default);
    bool userPresent = OVRPlugin.userPresent;
    if (this._userPresent && !userPresent)
    {
      System.Action hmdUnmountedEvent = this.hmdUnmountedEvent;
      if (hmdUnmountedEvent != null)
        hmdUnmountedEvent();
    }
    else if (!this._userPresent & userPresent)
    {
      System.Action hmdMountedEvent = this.hmdMountedEvent;
      if (hmdMountedEvent != null)
        hmdMountedEvent();
    }
    this._userPresent = userPresent;
    bool hasInputFocus = OVRPlugin.hasInputFocus;
    if (this._hasInputFocus && !hasInputFocus)
    {
      System.Action wasCapturedEvent = this.inputFocusWasCapturedEvent;
      if (wasCapturedEvent != null)
        wasCapturedEvent();
      this.DisableEventSystem();
    }
    else if (!this._hasInputFocus & hasInputFocus)
    {
      System.Action wasReleasedEvent = this.inputFocusWasReleasedEvent;
      if (wasReleasedEvent != null)
        wasReleasedEvent();
      this.EnableEventSystem();
    }
    this._hasInputFocus = hasInputFocus;
    bool hasVrFocus = OVRPlugin.hasVrFocus;
    if (this._hasVrFocus && !hasVrFocus)
    {
      System.Action wasCapturedEvent = this.vrFocusWasCapturedEvent;
      if (wasCapturedEvent != null)
        wasCapturedEvent();
    }
    else if (!this._hasVrFocus & hasVrFocus)
    {
      System.Action wasReleasedEvent = this.vrFocusWasReleasedEvent;
      if (wasReleasedEvent != null)
        wasReleasedEvent();
    }
    this._hasVrFocus = hasVrFocus;
    if (!this._isOVRManagerPresent)
      OVRInput.Update();
    float axis1 = Input.GetAxis("Oculus_CrossPlatform_PrimaryThumbstickHorizontal");
    float axis2 = Input.GetAxis("Oculus_CrossPlatform_PrimaryThumbstickVertical");
    float axis3 = Input.GetAxis("Oculus_CrossPlatform_SecondaryThumbstickHorizontal");
    float axis4 = Input.GetAxis("Oculus_CrossPlatform_SecondaryThumbstickVertical");
    Vector2 vector2 = new Vector2((double) Mathf.Abs(axis1) > (double) Mathf.Abs(axis3) ? axis1 : axis3, (double) Mathf.Abs(axis2) > (double) Mathf.Abs(axis4) ? axis2 : axis4);
    if (this.hasInputFocus)
    {
      if ((double) vector2.sqrMagnitude > 0.0099999997764825821)
      {
        this._scrollingLastFrame = true;
        System.Action<Vector2> centeredThisFrameEvent = this.joystickWasNotCenteredThisFrameEvent;
        if (centeredThisFrameEvent == null)
          return;
        centeredThisFrameEvent(vector2);
      }
      else
      {
        if (!this._scrollingLastFrame)
          return;
        this._scrollingLastFrame = false;
        System.Action centeredThisFrameEvent = this.joystickWasCenteredThisFrameEvent;
        if (centeredThisFrameEvent == null)
          return;
        centeredThisFrameEvent();
      }
    }
    else
    {
      if (!this._scrollingLastFrame)
        return;
      System.Action centeredThisFrameEvent = this.joystickWasCenteredThisFrameEvent;
      if (centeredThisFrameEvent == null)
        return;
      centeredThisFrameEvent();
    }
  }

  public virtual void FixedUpdate()
  {
    if (this._isOVRManagerPresent)
      return;
    OVRInput.FixedUpdate();
  }

  public virtual void LateUpdate()
  {
    if (this._isOVRManagerPresent)
      return;
    OVRHaptics.Process();
  }

  public virtual void TriggerHapticPulse(
    XRNode node,
    float duration,
    float strength,
    float frequency)
  {
    OVRPlugin.SetControllerVibration(node == XRNode.LeftHand ? 1U : 2U, frequency, strength);
  }

  public virtual void StopHaptics(XRNode node) => OVRPlugin.SetControllerVibration(node == XRNode.LeftHand ? 1U : 2U, 0.0f, 0.0f);

  public virtual void AdjustControllerTransform(
    XRNode node,
    Transform transform,
    Vector3 position,
    Vector3 rotation)
  {
    if (node != XRNode.LeftHand && node != XRNode.RightHand)
      return;
    rotation += new Vector3(-40f, 0.0f, 0.0f);
    position += new Vector3(0.0f, 0.0f, 0.055f);
    transform.Rotate(rotation);
    transform.Translate(position);
  }

  public virtual bool GetNodePose(XRNode nodeType, int idx, out Vector3 pos, out Quaternion rot)
  {
    OVRPose ovrPose = OVRPlugin.GetNodePose(nodeType.OculusNode(), OVRPlugin.Step.Render).ToOVRPose();
    pos = ovrPose.position;
    rot = ovrPose.orientation;
    return pos != Vector3.zero;
  }

  public virtual void EnableEventSystem()
  {
    if (!((UnityEngine.Object) this._disabledEventSystem != (UnityEngine.Object) null))
      return;
    this._disabledEventSystem.enabled = true;
    this._disabledEventSystem = (EventSystem) null;
  }

  public virtual void DisableEventSystem()
  {
    if (!((UnityEngine.Object) this._disabledEventSystem == (UnityEngine.Object) null))
      return;
    this._disabledEventSystem = EventSystem.current;
    if (!((UnityEngine.Object) this._disabledEventSystem != (UnityEngine.Object) null))
      return;
    if (!this._disabledEventSystem.enabled)
      this._disabledEventSystem = (EventSystem) null;
    else
      this._disabledEventSystem.enabled = false;
  }
}
