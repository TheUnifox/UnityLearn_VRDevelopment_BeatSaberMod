// Decompiled with JetBrains decompiler
// Type: VRController
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;
using UnityEngine.XR;
using Zenject;

public class VRController : MonoBehaviour
{
  [SerializeField]
  protected XRNode _node;
  [SerializeField]
  protected int _nodeIdx;
  [SerializeField]
  [NullAllowed]
  protected VRControllerTransformOffset _transformOffset;
  [Inject]
  protected readonly IVRPlatformHelper _vrPlatformHelper;
  [Inject]
  protected readonly VRControllersInputManager _vrControllersInputManager;
  protected Vector3 _lastTrackedPosition = Vector3.zero;

  public XRNode node
  {
    get => this._node;
    set => this._node = value;
  }

  public int nodeIdx
  {
    get => this._nodeIdx;
    set => this._nodeIdx = value;
  }

  public Vector3 position => this.transform.position;

  public Quaternion rotation => this.transform.rotation;

  public Vector3 forward => this.transform.forward;

  public float triggerValue => this._vrControllersInputManager.TriggerValue(this._node);

  public float verticalAxisValue => this._vrControllersInputManager.VerticalAxisValue(this._node);

  public float horizontalAxisValue => this._vrControllersInputManager.HorizontalAxisValue(this._node);

  public bool active => this.gameObject.activeInHierarchy;

  public virtual void Update()
  {
    Vector3 pos;
    Quaternion rot;
    if (!this._vrPlatformHelper.GetNodePose(this._node, this._nodeIdx, out pos, out rot))
    {
      if (this._lastTrackedPosition != Vector3.zero)
        pos = this._lastTrackedPosition;
      else if (this._node == XRNode.LeftHand)
        pos = new Vector3(-0.2f, 0.05f, 0.0f);
      else if (this._node == XRNode.RightHand)
        pos = new Vector3(0.2f, 0.05f, 0.0f);
    }
    else
      this._lastTrackedPosition = pos;
    this.transform.localPosition = pos;
    this.transform.localRotation = rot;
    if ((Object) this._transformOffset != (Object) null)
      this._vrPlatformHelper.AdjustControllerTransform(this.node, this.transform, this._transformOffset.positionOffset, this._transformOffset.rotationOffset);
    else
      this._vrPlatformHelper.AdjustControllerTransform(this.node, this.transform, Vector3.zero, Vector3.zero);
  }
}
