// Decompiled with JetBrains decompiler
// Type: OverridePlayerTransformsFromFullVRControllersRecorder
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using UnityEngine.XR;
using Zenject;

public class OverridePlayerTransformsFromFullVRControllersRecorder : MonoBehaviour
{
  [SerializeField]
  protected FullVRControllersRecorder _fullVRControllersRecorder;
  [Inject]
  protected readonly PlayerTransforms _playerTransforms;
  [Inject]
  protected readonly SaberManager _saberManager;
  [Inject]
  protected readonly PlayerVRControllersManager _playerVRControllersManager;

  public virtual void Start()
  {
    this._playerVRControllersManager.DisableAllVRControllers();
    this._fullVRControllersRecorder.didSetControllerTransformEvent += new System.Action<VRController>(this.HandleFullVRControllersRecorderDidSetControllerTransform);
  }

  public virtual void OnDestroy()
  {
    if (!((UnityEngine.Object) this._fullVRControllersRecorder != (UnityEngine.Object) null))
      return;
    this._fullVRControllersRecorder.didSetControllerTransformEvent -= new System.Action<VRController>(this.HandleFullVRControllersRecorderDidSetControllerTransform);
  }

  public virtual void HandleFullVRControllersRecorderDidSetControllerTransform(
    VRController controller)
  {
    if (controller.node == XRNode.Head)
      this._playerTransforms.OverrideHeadPos(controller.transform.position);
    else if (controller.node == XRNode.LeftHand)
    {
      this._saberManager.leftSaber.OverridePositionAndRotation(controller.position, controller.rotation);
    }
    else
    {
      if (controller.node != XRNode.RightHand)
        return;
      this._saberManager.rightSaber.OverridePositionAndRotation(controller.position, controller.rotation);
    }
  }
}
