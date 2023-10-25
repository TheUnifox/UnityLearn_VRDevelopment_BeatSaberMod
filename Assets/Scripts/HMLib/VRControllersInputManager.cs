// Decompiled with JetBrains decompiler
// Type: VRControllersInputManager
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;
using UnityEngine.XR;
using Zenject;

public class VRControllersInputManager
{
  [Inject]
  protected IVRPlatformHelper _vrPlatformHelper;
  protected const string kTriggerLeftHand = "TriggerLeftHand";
  protected const string kTriggerRightHand = "TriggerRightHand";
  protected const string kVerticalLeftHand = "VerticalLeftHand";
  protected const string kVerticalRightHand = "VerticalRightHand";
  protected const string kHorizontalLeftHand = "HorizontalLeftHand";
  protected const string kHorizontalRightHand = "HorizontalRightHand";
  protected const string kMenuButtonLeftHandOculusTouch = "MenuButtonLeftHandOculusTouch";
  protected const string kMenuButtonLeftHand = "MenuButtonLeftHand";
  protected const string kMenuButtonRightHandOculusTouch = "MenuButtonRightHandOculusTouch";
  protected const string kMenuButtonRightHand = "MenuButtonRightHand";
  protected const string kMenuButtonOculusTouch = "MenuButtonOculusTouch";

  public virtual float TriggerValue(XRNode node)
  {
    if (this._vrPlatformHelper.currentXRDeviceModel == XRDeviceModel.OculusQuest)
    {
      if (node == XRNode.LeftHand)
        return OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch);
      if (node == XRNode.RightHand)
        return OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
    }
    else
    {
      if (node == XRNode.LeftHand)
        return Input.GetAxis("TriggerLeftHand");
      if (node == XRNode.RightHand)
        return Input.GetAxis("TriggerRightHand");
    }
    return 0.0f;
  }

  public virtual float VerticalAxisValue(XRNode node)
  {
    if (node == XRNode.LeftHand)
      return Input.GetAxis("VerticalLeftHand");
    return node == XRNode.RightHand ? Input.GetAxis("VerticalRightHand") : 0.0f;
  }

  public virtual float HorizontalAxisValue(XRNode node)
  {
    if (node == XRNode.LeftHand)
      return Input.GetAxis("HorizontalLeftHand");
    return node == XRNode.RightHand ? Input.GetAxis("HorizontalRightHand") : 0.0f;
  }

  public virtual bool MenuButtonDown()
  {
    if (this._vrPlatformHelper.currentXRDeviceModel == XRDeviceModel.OculusRift || this._vrPlatformHelper.currentXRDeviceModel == XRDeviceModel.OculusQuestLink)
    {
      if (this._vrPlatformHelper.vrPlatformSDK == VRPlatformSDK.Oculus)
        return Input.GetButtonDown("MenuButtonOculusTouch");
      return Input.GetButtonDown("MenuButtonLeftHandOculusTouch") || Input.GetButtonDown("MenuButtonRightHandOculusTouch");
    }
    if (this._vrPlatformHelper.currentXRDeviceModel == XRDeviceModel.OculusQuest)
      return Input.GetButtonDown("MenuButtonOculusTouch");
    return Input.GetButtonDown("MenuButtonLeftHand") || Input.GetButtonDown("MenuButtonRightHand");
  }

  public virtual bool MenuButton()
  {
    if (this._vrPlatformHelper.currentXRDeviceModel == XRDeviceModel.OculusRift || this._vrPlatformHelper.currentXRDeviceModel == XRDeviceModel.OculusQuestLink)
    {
      if (this._vrPlatformHelper.vrPlatformSDK == VRPlatformSDK.Oculus)
        return Input.GetButton("MenuButtonOculusTouch");
      return Input.GetButton("MenuButtonLeftHandOculusTouch") || Input.GetButton("MenuButtonRightHandOculusTouch");
    }
    if (this._vrPlatformHelper.currentXRDeviceModel == XRDeviceModel.OculusQuest)
      return Input.GetButton("MenuButtonOculusTouch");
    return Input.GetButton("MenuButtonLeftHand") || Input.GetButton("MenuButtonRightHand");
  }
}
