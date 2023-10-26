// Decompiled with JetBrains decompiler
// Type: VRUIControls.VRPointer
// Assembly: VRUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3BA7334A-77F9-4425-988C-69CB2EE35CCF
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\VRUI.dll

using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace VRUIControls
{
  [RequireComponent(typeof (EventSystem))]
  public class VRPointer : MonoBehaviour
  {
    [SerializeField]
    protected VRController _leftVRController;
    [SerializeField]
    protected VRController _rightVRController;
    [SerializeField]
    protected VRLaserPointer _laserPointerPrefab;
    [SerializeField]
    protected Transform _cursorPrefab;
    [SerializeField]
    protected float _defaultLaserPointerLength = 10f;
    [SerializeField]
    protected float _laserPointerWidth = 0.01f;
    public const float kScrollMultiplier = 1f;
    protected VRLaserPointer _laserPointer;
    protected Transform _cursorTransform;
    protected EventSystem _eventSystem;
    protected VRController _vrController;
    protected static bool _lastControllerUsedWasRight = true;
    protected static bool _rightControllerWasReleased = true;
    protected static bool _leftControllerWasReleased = true;
    protected PointerEventData _pointerData;

    public VRController vrController => this._vrController;

    public Vector3 cursorPosition => !((Object) this._cursorTransform != (Object) null) ? Vector3.zero : this._cursorTransform.position;

    private EventSystem eventSystem => !((Object) this._eventSystem != (Object) null) ? (this._eventSystem = this.GetComponent<EventSystem>()) : this._eventSystem;

    public virtual void Awake()
    {
      if (VRPointer._lastControllerUsedWasRight || !this._leftVRController.active)
        this._vrController = this._rightVRController;
      else
        this._vrController = this._leftVRController;
    }

    public virtual void OnEnable() => this.CreateLaserPointerAndLaserHit();

    public virtual void OnDisable() => this.DestroyLaserAndHit();

    public virtual void LateUpdate()
    {
      if ((double) this._leftVRController.triggerValue < 0.10000000149011612)
        VRPointer._leftControllerWasReleased = true;
      if ((double) this._rightVRController.triggerValue < 0.10000000149011612)
        VRPointer._rightControllerWasReleased = true;
      if (this._leftVRController.active && (double) this._leftVRController.triggerValue > 0.10000000149011612 && (Object) this._vrController != (Object) this._leftVRController && VRPointer._leftControllerWasReleased)
      {
        VRPointer._lastControllerUsedWasRight = false;
        VRPointer._leftControllerWasReleased = false;
        this._vrController = this._leftVRController;
        this.DestroyLaserAndHit();
        this.CreateLaserPointerAndLaserHit();
      }
      else if (this._rightVRController.active && (double) this._rightVRController.triggerValue > 0.10000000149011612 && (Object) this._vrController != (Object) this._rightVRController && VRPointer._rightControllerWasReleased)
      {
        VRPointer._lastControllerUsedWasRight = true;
        VRPointer._rightControllerWasReleased = false;
        this._vrController = this._rightVRController;
        this.DestroyLaserAndHit();
        this.CreateLaserPointerAndLaserHit();
      }
      if ((double) this._leftVRController.triggerValue > 0.10000000149011612)
        VRPointer._leftControllerWasReleased = false;
      if ((double) this._rightVRController.triggerValue > 0.10000000149011612)
        VRPointer._rightControllerWasReleased = false;
      if (this.eventSystem.enabled && (Object) this._laserPointer == (Object) null)
      {
        this.CreateLaserPointerAndLaserHit();
      }
      else
      {
        if (this.eventSystem.enabled || !((Object) this._laserPointer != (Object) null))
          return;
        this.DestroyLaserAndHit();
      }
    }

    public virtual void CreateLaserPointerAndLaserHit()
    {
      if ((Object) this._laserPointer == (Object) null && (Object) this._laserPointerPrefab != (Object) null)
      {
        this._laserPointer = Object.Instantiate<VRLaserPointer>(this._laserPointerPrefab, this._vrController.transform, false);
        this._laserPointer.SetLocalPosition(new Vector3(0.0f, 0.0f, this._defaultLaserPointerLength / 2f));
        this._laserPointer.SetLocalScale(new Vector3(this._laserPointerWidth, this._laserPointerWidth, this._defaultLaserPointerLength));
        this._laserPointer.SetFadeDistance(1f);
      }
      if (!((Object) this._cursorTransform == (Object) null) || !((Object) this._cursorPrefab != (Object) null))
        return;
      this._cursorTransform = Object.Instantiate<Transform>(this._cursorPrefab);
      this._cursorTransform.gameObject.SetActive(false);
      this._cursorTransform.SetParent(this.transform, false);
    }

    public virtual void RefreshLaserPointerAndLaserHit(PointerEventData pointerData)
    {
      if (float.IsNaN(pointerData.pointerCurrentRaycast.worldPosition.x))
        return;
      if ((Object) pointerData.pointerCurrentRaycast.gameObject != (Object) null)
      {
        if ((Object) this._laserPointer != (Object) null)
        {
          this._laserPointer.SetLocalPosition(new Vector3(0.0f, 0.0f, pointerData.pointerCurrentRaycast.distance / 2f));
          this._laserPointer.SetLocalScale(new Vector3(this._laserPointerWidth, this._laserPointerWidth, pointerData.pointerCurrentRaycast.distance));
          this._laserPointer.SetFadeDistance(0.0f);
        }
        if (!((Object) this._cursorTransform != (Object) null))
          return;
        this._cursorTransform.gameObject.SetActive(true);
        this._cursorTransform.position = pointerData.pointerCurrentRaycast.worldPosition;
      }
      else
      {
        if ((Object) this._laserPointer != (Object) null)
        {
          this._laserPointer.SetLocalPosition(new Vector3(0.0f, 0.0f, this._defaultLaserPointerLength / 2f));
          this._laserPointer.SetLocalScale(new Vector3(this._laserPointerWidth, this._laserPointerWidth, this._defaultLaserPointerLength));
          this._laserPointer.SetFadeDistance(1f);
        }
        if (!((Object) this._cursorTransform != (Object) null))
          return;
        this._cursorTransform.gameObject.SetActive(false);
      }
    }

    public virtual void OnApplicationFocus(bool hasFocus)
    {
      if (hasFocus || !((Object) this._cursorTransform != (Object) null))
        return;
      this._cursorTransform.gameObject.SetActive(false);
    }

    public virtual void DestroyLaserAndHit()
    {
      if ((Object) this._laserPointer != (Object) null)
      {
        this._laserPointer.gameObject.SetActive(false);
        Object.Destroy((Object) this._laserPointer.gameObject);
        this._laserPointer = (VRLaserPointer) null;
      }
      if (!((Object) this._cursorTransform != (Object) null))
        return;
      this._cursorTransform.gameObject.SetActive(false);
      Object.Destroy((Object) this._cursorTransform.gameObject);
      this._cursorTransform = (Transform) null;
    }

    public string state
    {
      get
      {
        StringBuilder stringBuilder = new StringBuilder();
        if (this._pointerData != null)
        {
          stringBuilder.AppendFormat("\ndevice id: {0}", (object) this._vrController.node);
          stringBuilder.AppendFormat("\nentered: {0}", (Object) this._pointerData.pointerEnter == (Object) null ? (object) "none" : (object) this._pointerData.pointerEnter.name);
          stringBuilder.AppendFormat("\npressed: {0}", (Object) this._pointerData.pointerPress == (Object) null ? (object) "none" : (object) this._pointerData.pointerPress.name);
          stringBuilder.AppendFormat("\ndragged: {0}", (Object) this._pointerData.pointerDrag == (Object) null ? (object) "none" : (object) this._pointerData.pointerDrag.name);
          stringBuilder.AppendFormat("\nselected: {0}", (Object) this.eventSystem.currentSelectedGameObject == (Object) null ? (object) "none" : (object) this.eventSystem.currentSelectedGameObject.name);
        }
        return stringBuilder.ToString();
      }
    }

    public virtual void Process(PointerEventData pointerEventData)
    {
      if (!this._vrController.active)
      {
        this.enabled = false;
      }
      else
      {
        this.enabled = true;
        this._pointerData = pointerEventData;
        this.RefreshLaserPointerAndLaserHit(this._pointerData);
      }
    }
  }
}
