// Decompiled with JetBrains decompiler
// Type: OpenVRHelper
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.XR;
using Valve.VR;
using Zenject;

public class OpenVRHelper : MonoBehaviour, IVRPlatformHelper
{
  [Inject]
  protected readonly IOpenVRHaptics _openVrHaptics;
  public const float kMicroSecondsInSecond = 1000000f;
  public const int kUpdateInterval = 4;
  public const int kMaxDurationMs = 4000;
  public const float kUpdateIntervalSeconds = 0.004f;
  protected readonly TrackedDevicePose_t[] _gamePoses = new TrackedDevicePose_t[0];
  protected readonly List<XRNodeState> _nodeStates = new List<XRNodeState>(10);
  protected readonly TrackedDevicePose_t[] _poses = new TrackedDevicePose_t[64];
  protected bool _hasInputFocus = true;
  protected bool _didGetNodeStatesThisFrame;
  protected EventSystem _disabledEventSystem;
  protected OpenVRHelper.VRControllerManufacturerName _vrControllerManufacturerName = OpenVRHelper.VRControllerManufacturerName.Undefined;
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

  public bool hasVrFocus => true;

  public bool isAlwaysWireless => false;

  public VRPlatformSDK vrPlatformSDK => VRPlatformSDK.OpenVR;

  public XRDeviceModel currentXRDeviceModel => VRPlatformUtils.GetXRDeviceModel();

  private OpenVRHelper.VRControllerManufacturerName vrControllerManufacturerName
  {
    get
    {
      if (this._vrControllerManufacturerName == OpenVRHelper.VRControllerManufacturerName.Undefined)
      {
        CVRSystem system = OpenVR.System;
        if (system != null)
        {
          uint forControllerRole = system.GetTrackedDeviceIndexForControllerRole(ETrackedControllerRole.LeftHand);
          if (forControllerRole < 64U)
          {
            StringBuilder pchValue = new StringBuilder(100);
            ETrackedPropertyError pError = ETrackedPropertyError.TrackedProp_Success;
            int trackedDeviceProperty = (int) system.GetStringTrackedDeviceProperty(forControllerRole, ETrackedDeviceProperty.Prop_ManufacturerName_String, pchValue, 100U, ref pError);
            if (pError == ETrackedPropertyError.TrackedProp_Success)
            {
              string str = pchValue.ToString();
              this._vrControllerManufacturerName = str.IndexOf("htc", StringComparison.OrdinalIgnoreCase) < 0 ? (str.IndexOf("oculus", StringComparison.OrdinalIgnoreCase) < 0 ? (str.IndexOf("valve", StringComparison.OrdinalIgnoreCase) < 0 ? OpenVRHelper.VRControllerManufacturerName.Unknown : OpenVRHelper.VRControllerManufacturerName.Valve) : OpenVRHelper.VRControllerManufacturerName.Oculus) : OpenVRHelper.VRControllerManufacturerName.HTC;
            }
          }
        }
      }
      return this._vrControllerManufacturerName;
    }
  }

  public virtual void Awake() => Application.onBeforeRender += new UnityAction(this.OnBeforeRender);

  public virtual void TriggerHapticPulse(
    XRNode node,
    float duration,
    float strength,
    float frequency)
  {
    this._openVrHaptics.TriggerHapticPulse(node, duration, strength, frequency);
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
    if (node != XRNode.LeftHand && node != XRNode.RightHand)
      return;
    if (this.vrControllerManufacturerName == OpenVRHelper.VRControllerManufacturerName.Valve)
    {
      rotation += new Vector3(-16.3f, 0.0f, 0.0f);
      position += new Vector3(0.0f, 0.022f, -0.01f);
    }
    else
    {
      rotation += new Vector3(-4.3f, 0.0f, 0.0f);
      position += new Vector3(0.0f, -0.008f, 0.0f);
    }
    transform.Rotate(rotation);
    transform.Translate(position);
  }

  public virtual bool GetNodePose(XRNode nodeType, int idx, out Vector3 pos, out Quaternion rot)
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
    return pos != Vector3.zero;
  }

  public virtual void Update()
  {
    CVRSystem system = OpenVR.System;
    if (system != null)
    {
      VREvent_t pEvent = new VREvent_t();
      uint uncbVREvent = (uint) Marshal.SizeOf(typeof (VREvent_t));
      for (int index = 0; index < 64 && system.PollNextEvent(ref pEvent, uncbVREvent); ++index)
      {
        switch ((EVREventType) pEvent.eventType)
        {
          case EVREventType.VREvent_InputFocusCaptured:
            if (pEvent.data.process.oldPid == 0U)
            {
              this._hasInputFocus = false;
              System.Action wasCapturedEvent = this.inputFocusWasCapturedEvent;
              if (wasCapturedEvent != null)
                wasCapturedEvent();
              this.DisableEventSystem();
              break;
            }
            break;
          case EVREventType.VREvent_InputFocusReleased:
            if (pEvent.data.process.pid == 0U)
            {
              this._hasInputFocus = true;
              System.Action wasReleasedEvent = this.inputFocusWasReleasedEvent;
              if (wasReleasedEvent != null)
                wasReleasedEvent();
              this.EnableEventSystem();
              break;
            }
            break;
          case EVREventType.VREvent_DashboardActivated:
            this._hasInputFocus = false;
            System.Action wasCapturedEvent1 = this.inputFocusWasCapturedEvent;
            if (wasCapturedEvent1 != null)
              wasCapturedEvent1();
            this.DisableEventSystem();
            break;
          case EVREventType.VREvent_DashboardDeactivated:
            this._hasInputFocus = true;
            System.Action wasReleasedEvent1 = this.inputFocusWasReleasedEvent;
            if (wasReleasedEvent1 != null)
              wasReleasedEvent1();
            this.EnableEventSystem();
            break;
          case EVREventType.VREvent_Quit:
            Application.Quit();
            break;
          default:
            SteamVR_Events.System((EVREventType) pEvent.eventType).Send(pEvent);
            break;
        }
      }
    }
    float axis1 = Input.GetAxis("Oculus_CrossPlatform_PrimaryThumbstickHorizontal");
    float axis2 = Input.GetAxis("Oculus_CrossPlatform_PrimaryThumbstickVertical");
    float axis3 = Input.GetAxis("Oculus_CrossPlatform_SecondaryThumbstickHorizontal");
    float axis4 = Input.GetAxis("Oculus_CrossPlatform_SecondaryThumbstickVertical");
    Vector2 vector2 = new Vector2((double) Mathf.Abs(axis1) > (double) Mathf.Abs(axis3) ? axis1 : axis3, (double) Mathf.Abs(axis2) > (double) Mathf.Abs(axis4) ? axis2 : axis4);
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

  public virtual void LateUpdate() => this._didGetNodeStatesThisFrame = false;

  public virtual void OnDestroy() => this._openVrHaptics.Destroy();

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

  public virtual void EnableEventSystem()
  {
    if (!((UnityEngine.Object) this._disabledEventSystem != (UnityEngine.Object) null))
      return;
    this._disabledEventSystem.enabled = true;
    this._disabledEventSystem = (EventSystem) null;
  }

  public virtual void OnBeforeRender()
  {
    CVRCompositor compositor = OpenVR.Compositor;
    if (compositor == null)
      return;
    int lastPoses = (int) compositor.GetLastPoses(this._poses, this._gamePoses);
  }

  public enum VRControllerManufacturerName
  {
    HTC,
    Oculus,
    Valve,
    Unknown,
    Undefined,
  }
}
