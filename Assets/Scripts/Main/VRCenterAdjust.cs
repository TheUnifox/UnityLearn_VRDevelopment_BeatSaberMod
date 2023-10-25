// Decompiled with JetBrains decompiler
// Type: VRCenterAdjust
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using UnityEngine.XR;

public class VRCenterAdjust : MonoBehaviour
{
  [SerializeField]
  protected Vector3SO _roomCenter;
  [SerializeField]
  protected FloatSO _roomRotation;
  [SerializeField]
  protected MainSettingsModelSO _mainSettingsModel;

  public virtual void Awake() => XRDevice.SetTrackingSpaceType(TrackingSpaceType.RoomScale);

  public virtual void Start()
  {
    if ((double) this._roomCenter.value.magnitude > 5.0)
      this.ResetRoom();
    this.transform.localPosition = (Vector3) (ObservableVariableSO<Vector3>) this._roomCenter;
    this.transform.localEulerAngles = new Vector3(0.0f, (float) (ObservableVariableSO<float>) this._roomRotation, 0.0f);
  }

  public virtual void OnEnable()
  {
    this._roomCenter.didChangeEvent += new System.Action(this.HandleRoomCenterDidChange);
    this._roomRotation.didChangeEvent += new System.Action(this.HandleRoomRotationDidChange);
  }

  public virtual void OnDisable()
  {
    this._roomCenter.didChangeEvent -= new System.Action(this.HandleRoomCenterDidChange);
    this._roomRotation.didChangeEvent -= new System.Action(this.HandleRoomRotationDidChange);
  }

  public virtual void HandleRoomCenterDidChange() => this.transform.localPosition = (Vector3) (ObservableVariableSO<Vector3>) this._roomCenter;

  public virtual void HandleRoomRotationDidChange() => this.transform.localEulerAngles = new Vector3(0.0f, (float) (ObservableVariableSO<float>) this._roomRotation, 0.0f);

  public virtual void Update()
  {
    if (!Input.GetKey(KeyCode.Delete))
      return;
    this.ResetRoom();
  }

  public virtual void ResetRoom()
  {
    this._roomCenter.value = new Vector3(0.0f, 0.0f, 0.0f);
    this._roomRotation.value = 0.0f;
  }
}
