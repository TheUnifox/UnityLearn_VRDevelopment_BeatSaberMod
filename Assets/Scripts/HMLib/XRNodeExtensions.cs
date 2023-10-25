// Decompiled with JetBrains decompiler
// Type: XRNodeExtensions
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;
using UnityEngine.XR;

public static class XRNodeExtensions
{
  public static OVRPlugin.Node OculusNode(this XRNode node)
  {
    switch (node)
    {
      case XRNode.Head:
        return OVRPlugin.Node.Head;
      case XRNode.LeftHand:
        return OVRPlugin.Node.HandLeft;
      case XRNode.RightHand:
        return OVRPlugin.Node.HandRight;
      default:
        Debug.LogError((object) string.Format("Can not convert XRNode ({0}) to OVRPlugin.Node.", (object) node));
        return OVRPlugin.Node.None;
    }
  }

  public static int PSMoveDeviceIndex(this XRNode node)
  {
    if (node == XRNode.LeftHand)
      return 1;
    if (node == XRNode.RightHand)
      return 0;
    Debug.LogError((object) string.Format("Can not convert XRNode ({0}) to PSMoveDeviceIndex.", (object) node));
    return -1;
  }
}
