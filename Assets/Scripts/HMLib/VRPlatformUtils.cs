// Decompiled with JetBrains decompiler
// Type: VRPlatformUtils
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System;

public abstract class VRPlatformUtils
{
  public static XRDeviceModel GetXRDeviceModel()
  {
    if (UnityEngine.XR.XRDevice.model.IndexOf("oculus", StringComparison.OrdinalIgnoreCase) >= 0)
      return XRDeviceModel.OculusRift;
    return UnityEngine.XR.XRDevice.model.IndexOf("quest", StringComparison.OrdinalIgnoreCase) >= 0 ? XRDeviceModel.OculusQuestLink : XRDeviceModel.Other;
  }
}
