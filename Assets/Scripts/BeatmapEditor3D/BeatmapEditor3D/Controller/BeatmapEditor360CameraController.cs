// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Controller.BeatmapEditor360CameraController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Controller
{
  public class BeatmapEditor360CameraController : MonoBehaviour
  {
    [Space]
    [SerializeField]
    private Transform _uiCameraMovementTransform;
    [SerializeField]
    private Transform _uiCameraTransform;
    [Space]
    [SerializeField]
    private float _slowMoveIntensity;
    [SerializeField]
    private float _defaultMoveIntensity;
    [SerializeField]
    private float _fastMoveIntensity;
    [Space]
    [SerializeField]
    private MouseLook _mouseLook = new MouseLook();
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    private float _currentMoveIntensity;

    private void Start()
    {
      this._mouseLook.Init(this._uiCameraMovementTransform, this._uiCameraTransform);
      this._mouseLook.SetRotations(this._uiCameraMovementTransform.transform.rotation, this._uiCameraTransform.transform.rotation);
      this._mouseLook.SetCursorLock(false);
      this._currentMoveIntensity = this._defaultMoveIntensity;
    }

    private void OnDestroy()
    {
    }

    private void Update()
    {
      if (!this._beatmapState.cameraMoving)
        return;
      this._mouseLook.LookRotation(this._uiCameraMovementTransform, this._uiCameraTransform);
      Vector3 position = this._uiCameraMovementTransform.position;
      Vector3 vector3_1 = Vector3.zero;
      if (Input.GetKey(KeyCode.W))
        vector3_1 = this._uiCameraTransform.forward;
      if (Input.GetKey(KeyCode.S))
        vector3_1 = -this._uiCameraTransform.forward;
      Vector3 vector3_2 = Vector3.zero;
      if (Input.GetKey(KeyCode.D))
        vector3_2 = this._uiCameraTransform.right;
      if (Input.GetKey(KeyCode.A))
        vector3_2 = -this._uiCameraTransform.right;
      Vector3 vector3_3 = Vector3.zero;
      if (Input.GetKey(KeyCode.Space))
        vector3_3 = this.transform.up;
      if (Input.GetKey(KeyCode.LeftControl))
        vector3_3 = -this.transform.up;
      if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        this._currentMoveIntensity = this._fastMoveIntensity;
      if (Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.RightAlt))
        this._currentMoveIntensity = this._slowMoveIntensity;
      if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift) || Input.GetKeyUp(KeyCode.LeftAlt) || Input.GetKeyUp(KeyCode.RightAlt))
        this._currentMoveIntensity = this._defaultMoveIntensity;
      this._uiCameraMovementTransform.position = position + (vector3_1 + vector3_2 + vector3_3) * (this._currentMoveIntensity * Time.deltaTime);
    }

    private void HandleViewRotated()
    {
    }
  }
}
