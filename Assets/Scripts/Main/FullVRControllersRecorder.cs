// Decompiled with JetBrains decompiler
// Type: FullVRControllersRecorder
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Zenject;

public class FullVRControllersRecorder : MonoBehaviour
{
  [SerializeField]
  protected string _recordingFilePath = "Recording.dat";
  [SerializeField]
  protected FullVRControllersRecorder.Mode _mode = FullVRControllersRecorder.Mode.Off;
  [SerializeField]
  protected float _timeOffset;
  [SerializeField]
  protected float _othersSmooth = 8f;
  [SerializeField]
  protected float _handsSmooth = 8f;
  [SerializeField]
  protected float _playbackFloorOffset;
  [Space]
  [SerializeField]
  protected VRController[] _controllers;
  [Inject]
  protected AudioTimeSyncController _audioTimeSyncController;
  protected int _keyframeIndex;
  protected VRControllersRecorderData _data;

  public event System.Action<VRController> didSetControllerTransformEvent;

  public virtual void Start()
  {
    if (this._recordingFilePath == "")
      this._recordingFilePath = "Recordings/Recording.rd";
    if (this._mode == FullVRControllersRecorder.Mode.Off)
    {
      this.enabled = false;
    }
    else
    {
      if (this._mode == FullVRControllersRecorder.Mode.Playback)
      {
        foreach (Behaviour controller in this._controllers)
          controller.enabled = false;
      }
      List<VRControllersRecorderData.NodeInfo> nodeInfoList = new List<VRControllersRecorderData.NodeInfo>(this._controllers.Length);
      foreach (VRController controller in this._controllers)
        nodeInfoList.Add(new VRControllersRecorderData.NodeInfo(controller.node, controller.nodeIdx));
      this._data = new VRControllersRecorderData(nodeInfoList.ToArray());
      if (this._mode == FullVRControllersRecorder.Mode.Playback)
      {
        VRControllersRecorderSaveAndLoad.LoadFromFile(this._recordingFilePath, this._data);
        if (this._data.numberOfKeyframes <= 0)
          return;
        foreach (VRController controller in this._controllers)
        {
          controller.gameObject.SetActive(true);
          VRControllersRecorderData.PositionAndRotation positionAndRotation = this._data.GetPositionAndRotation(0, controller.node, controller.nodeIdx);
          controller.transform.localPosition = positionAndRotation.pos;
          controller.transform.localRotation = positionAndRotation.rot;
          System.Action<VRController> controllerTransformEvent = this.didSetControllerTransformEvent;
          if (controllerTransformEvent != null)
            controllerTransformEvent(controller);
        }
      }
      else
      {
        if (this._mode != FullVRControllersRecorder.Mode.Record)
          return;
        Debug.LogWarning((object) "Recording performance.");
      }
    }
  }

  public virtual void OnDestroy()
  {
    if (this._mode != FullVRControllersRecorder.Mode.Record)
      return;
    VRControllersRecorderSaveAndLoad.SaveToFile(this._recordingFilePath, this._data);
    Debug.Log((object) ("Performance saved to file " + this._recordingFilePath));
  }

  public virtual void PlaybackTick()
  {
    float num = this._audioTimeSyncController.songTime + this._timeOffset;
    while (this._keyframeIndex < this._data.numberOfKeyframes - 2 && (double) this._data.GetFrameTime(this._keyframeIndex + 1) < (double) num)
      ++this._keyframeIndex;
    float t1 = (num - this._data.GetFrameTime(this._keyframeIndex)) / Mathf.Max(1E-06f, this._data.GetFrameTime(this._keyframeIndex + 1) - this._data.GetFrameTime(this._keyframeIndex));
    foreach (VRController controller in this._controllers)
    {
      VRControllersRecorderData.PositionAndRotation positionAndRotation = this._data.GetLerpedPositionAndRotation(this._keyframeIndex, t1, controller.node, controller.nodeIdx);
      Vector3 position = controller.position;
      Quaternion rotation = controller.rotation;
      float t2 = controller.node == XRNode.LeftHand || controller.node == XRNode.RightHand ? ((double) this._handsSmooth == 0.0 ? 1f : Time.deltaTime * this._handsSmooth) : ((double) this._othersSmooth == 0.0 ? 1f : Time.deltaTime * this._othersSmooth);
      Vector3 vector3 = Vector3.Lerp(position, positionAndRotation.pos + new Vector3(0.0f, this._playbackFloorOffset, 0.0f), t2);
      Quaternion quaternion = Quaternion.Lerp(rotation, positionAndRotation.rot, t2);
      controller.transform.position = vector3;
      controller.transform.rotation = quaternion;
      System.Action<VRController> controllerTransformEvent = this.didSetControllerTransformEvent;
      if (controllerTransformEvent != null)
        controllerTransformEvent(controller);
    }
  }

  public virtual void RecordTick()
  {
    if ((double) this._audioTimeSyncController.songTime == 0.0)
      return;
    VRControllersRecorderData.PositionAndRotation[] positionsAndRotations = new VRControllersRecorderData.PositionAndRotation[this._controllers.Length];
    for (int index = 0; index < this._controllers.Length; ++index)
    {
      VRController controller = this._controllers[index];
      Vector3 position = controller.position;
      Quaternion rotation = controller.rotation;
      positionsAndRotations[index] = new VRControllersRecorderData.PositionAndRotation(position, rotation);
    }
    this._data.AddKeyFrame(positionsAndRotations, this._audioTimeSyncController.songTime);
  }

  public virtual void Update()
  {
    if (this._mode != FullVRControllersRecorder.Mode.Playback || this._data == null || this._data.numberOfKeyframes <= 1)
      return;
    this.PlaybackTick();
  }

  public virtual void LateUpdate()
  {
    if (this._mode != FullVRControllersRecorder.Mode.Record)
      return;
    this.RecordTick();
  }

  public enum Mode
  {
    Record,
    Playback,
    Off,
  }
}
