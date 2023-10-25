// Decompiled with JetBrains decompiler
// Type: FirstPersonFlyingController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using VRUIControls;

public class FirstPersonFlyingController : MonoBehaviour
{
  [SerializeField]
  protected float _moveSensitivity = 0.1f;
  [SerializeField]
  [NullAllowed]
  protected Transform _transform;
  [SerializeField]
  [NullAllowed]
  protected Camera _camera;
  [SerializeField]
  protected float _cameraFov = 90f;
  [SerializeField]
  [NullAllowed]
  protected VRCenterAdjust _centerAdjust;
  [SerializeField]
  [NullAllowed]
  protected VRController _controller0;
  [SerializeField]
  [NullAllowed]
  protected VRController _controller1;
  [SerializeField]
  [NullAllowed]
  protected VRInputModule _vrInputModule;
  [SerializeField]
  [NullAllowed]
  protected GameObject[] _controllerModels;
  [SerializeField]
  protected MouseLook _mouseLook = new MouseLook();
  protected bool _shouldBeEnabled;
  protected Transform _cameraTransform;

  public virtual void Awake() => this.enabled = this._shouldBeEnabled;

  public virtual void Start()
  {
    this._cameraTransform = this._camera.transform;
    this._mouseLook.Init(this._transform, this._cameraTransform);
    this._controller0.enabled = false;
    this._controller1.enabled = false;
    this._camera.stereoTargetEye = StereoTargetEyeMask.None;
    this._camera.fieldOfView = this._cameraFov;
    this._camera.aspect = (float) Screen.width / (float) Screen.height;
    this._centerAdjust.ResetRoom();
    this._centerAdjust.enabled = false;
    this._transform.position = new Vector3(0.0f, 1.7f, 0.0f);
    foreach (GameObject controllerModel in this._controllerModels)
    {
      if ((Object) controllerModel != (Object) null)
        controllerModel.SetActive(false);
    }
  }

  public virtual void OnEnable()
  {
    if ((Object) this._vrInputModule != (Object) null)
      this._vrInputModule.useMouseForPressInput = true;
    this._mouseLook.SetCursorLock(true);
  }

  public virtual void OnDisable()
  {
    if ((Object) this._vrInputModule != (Object) null)
      this._vrInputModule.useMouseForPressInput = false;
    this._mouseLook.SetCursorLock(false);
  }

  public virtual void Update()
  {
    this._mouseLook.LookRotation(this._transform, this._cameraTransform);
    this._controller0.transform.SetPositionAndRotation(this._cameraTransform.position, this._cameraTransform.rotation);
    this._controller1.transform.SetPositionAndRotation(this._cameraTransform.position, this._cameraTransform.rotation);
    Vector3 position = this._transform.position;
    Vector3 vector3_1 = Vector3.zero;
    if (Input.GetKey(KeyCode.W))
      vector3_1 = this._cameraTransform.forward;
    if (Input.GetKey(KeyCode.S))
      vector3_1 = -this._cameraTransform.forward;
    Vector3 vector3_2 = Vector3.zero;
    if (Input.GetKey(KeyCode.D))
      vector3_2 = this._cameraTransform.right;
    if (Input.GetKey(KeyCode.A))
      vector3_2 = -this._cameraTransform.right;
    Vector3 vector3_3 = Vector3.zero;
    if (Input.GetKey(KeyCode.Space))
      vector3_3 = this._cameraTransform.up;
    if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
      vector3_3 = -this._cameraTransform.up;
    this._transform.position = position + (vector3_1 + vector3_2 + vector3_3) * this._moveSensitivity * Time.deltaTime;
  }

  public virtual void Inject(
    Camera camera,
    VRCenterAdjust centerAdjust,
    VRController controller0,
    VRController controller1,
    VRInputModule vrInputModule,
    bool shouldBeEnabled)
  {
    this._transform = centerAdjust.transform;
    this._camera = camera;
    this._centerAdjust = centerAdjust;
    this._controller0 = controller0;
    this._controller1 = controller1;
    this._vrInputModule = vrInputModule;
    this._controllerModels = new GameObject[2]
    {
      this._controller0.gameObject,
      this._controller1.gameObject
    };
    this._shouldBeEnabled = shouldBeEnabled;
  }
}
