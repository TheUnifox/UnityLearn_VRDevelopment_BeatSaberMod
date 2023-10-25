// Decompiled with JetBrains decompiler
// Type: MouseLook
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;

[Serializable]
public class MouseLook
{
  [SerializeField]
  protected float _xSensitivity = 2f;
  [SerializeField]
  protected float _ySensitivity = 2f;
  [SerializeField]
  protected bool _clampVerticalRotation = true;
  [SerializeField]
  protected float _minimumX = -90f;
  [SerializeField]
  protected float _maximumX = 90f;
  [SerializeField]
  protected bool _smooth;
  [SerializeField]
  protected float _smoothTime = 5f;
  [SerializeField]
  protected bool _lockCursor = true;
  protected Quaternion _characterTargetRot;
  protected Quaternion _cameraTargetRot;
  protected bool _cursorIsLocked = true;

  public virtual void Init(Transform character, Transform camera)
  {
    this._characterTargetRot = character.localRotation;
    this._cameraTargetRot = Quaternion.identity;
  }

  public virtual void SetRotations(Quaternion characterRotation, Quaternion cameraRotation)
  {
    this._characterTargetRot = characterRotation;
    this._cameraTargetRot = cameraRotation;
  }

  public virtual void LookRotation(Transform character, Transform camera)
  {
    float y = Input.GetAxis("MouseX") * this._xSensitivity;
    float num = Input.GetAxis("MouseY") * this._ySensitivity;
    this._characterTargetRot *= Quaternion.Euler(0.0f, y, 0.0f);
    this._cameraTargetRot *= Quaternion.Euler(-num, 0.0f, 0.0f);
    if (this._clampVerticalRotation)
      this._cameraTargetRot = this.ClampRotationAroundXAxis(this._cameraTargetRot);
    if (this._smooth)
    {
      character.localRotation = Quaternion.Slerp(character.localRotation, this._characterTargetRot, this._smoothTime * Time.deltaTime);
      camera.localRotation = Quaternion.Slerp(camera.localRotation, this._cameraTargetRot, this._smoothTime * Time.deltaTime);
    }
    else
    {
      character.localRotation = this._characterTargetRot;
      camera.localRotation = this._cameraTargetRot;
    }
    this.UpdateCursorLock();
  }

  public virtual void SetCursorLock(bool value)
  {
    this._lockCursor = value;
    if (this._lockCursor)
      return;
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;
  }

  public virtual void UpdateCursorLock()
  {
    if (!this._lockCursor)
      return;
    this.InternalLockUpdate();
  }

  public virtual void InternalLockUpdate()
  {
    if (Input.GetKeyUp(KeyCode.Escape))
      this._cursorIsLocked = false;
    else if (Input.GetMouseButtonUp(0))
      this._cursorIsLocked = true;
    if (this._cursorIsLocked)
    {
      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;
    }
    else
    {
      if (this._cursorIsLocked)
        return;
      Cursor.lockState = CursorLockMode.None;
      Cursor.visible = true;
    }
  }

  public virtual Quaternion ClampRotationAroundXAxis(Quaternion q)
  {
    q.x /= q.w;
    q.y /= q.w;
    q.z /= q.w;
    q.w = 1f;
    float num = Mathf.Clamp(114.59156f * Mathf.Atan(q.x), this._minimumX, this._maximumX);
    q.x = Mathf.Tan((float) Math.PI / 360f * num);
    return q;
  }
}
