// Decompiled with JetBrains decompiler
// Type: VRTrackersRecorder
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;

public class VRTrackersRecorder : MonoBehaviour
{
  [SerializeField]
  protected FloatSO _songTime;
  [SerializeField]
  protected string _saveFilename = "VRControllersRecording.dat";
  [SerializeField]
  protected VRTrackersRecorder.RecordMode _mode = VRTrackersRecorder.RecordMode.Off;
  [SerializeField]
  protected Transform _originTransform;
  [SerializeField]
  protected Transform[] _playbackTransforms;
  protected List<VRTrackersRecorder.Keyframe> _keyframes;
  protected int _keyframeIndex;
  protected SteamVR_Events.Action _newPosesAction;
  protected Vector3 _loadedOriginPos;
  protected Quaternion _loadedOriginRot;

  public virtual void Awake()
  {
    if (this._saveFilename == "")
      this._saveFilename = "VRControllersRecording.dat";
    this._keyframes = new List<VRTrackersRecorder.Keyframe>();
    if (this._mode == VRTrackersRecorder.RecordMode.Playback)
      this.Load();
    else if (this._mode == VRTrackersRecorder.RecordMode.Record)
    {
      this._newPosesAction = SteamVR_Events.NewPosesAction(new UnityAction<TrackedDevicePose_t[]>(this.OnNewPoses));
      Debug.LogWarning((object) "Recording performance.");
    }
    if (this._playbackTransforms.Length != 0)
      return;
    Debug.LogWarning((object) "No playback transforms in VR trackers recorder.");
  }

  public virtual void OnDestroy()
  {
    if (this._mode != VRTrackersRecorder.RecordMode.Record)
      return;
    this.Save();
  }

  public virtual void OnEnable()
  {
    if (this._newPosesAction == null || this._mode != VRTrackersRecorder.RecordMode.Record)
      return;
    this._newPosesAction.enabled = true;
  }

  public virtual void OnDisable()
  {
    if (this._newPosesAction == null || this._mode != VRTrackersRecorder.RecordMode.Record)
      return;
    this._newPosesAction.enabled = false;
  }

  public virtual void OnNewPoses(TrackedDevicePose_t[] poses)
  {
    VRTrackersRecorder.Keyframe keyframe = new VRTrackersRecorder.Keyframe();
    keyframe._transforms = new VRTrackersRecorder.Keyframe.KeyframeTransform[poses.Length];
    for (int index = 0; index < poses.Length; ++index)
    {
      VRTrackersRecorder.Keyframe.KeyframeTransform keyframeTransform = keyframe._transforms[index] = new VRTrackersRecorder.Keyframe.KeyframeTransform();
      if (poses[index].bDeviceIsConnected && poses[index].bPoseIsValid)
      {
        SteamVR_Utils.RigidTransform rigidTransform = new SteamVR_Utils.RigidTransform(poses[index].mDeviceToAbsoluteTracking);
        keyframeTransform._pos = rigidTransform.pos;
        keyframeTransform._rot = rigidTransform.rot;
        keyframeTransform._valid = true;
      }
      else
      {
        keyframeTransform._pos = Vector3.zero;
        keyframeTransform._rot = Quaternion.identity;
        keyframeTransform._valid = false;
      }
    }
    keyframe._time = this._songTime.value;
    this._keyframes.Add(keyframe);
  }

  public virtual void Update()
  {
    if (this._mode != VRTrackersRecorder.RecordMode.Playback || this._keyframes.Count < 2)
      return;
    float num = this._songTime.value;
    while (this._keyframeIndex < this._keyframes.Count - 2 && (double) this._keyframes[this._keyframeIndex + 1]._time < (double) num)
      ++this._keyframeIndex;
    Vector3 position = this._originTransform.position;
    Quaternion rotation = this._originTransform.rotation;
    VRTrackersRecorder.Keyframe keyframe1 = this._keyframes[this._keyframeIndex];
    VRTrackersRecorder.Keyframe keyframe2 = this._keyframes[this._keyframeIndex + 1];
    float t = (num - keyframe1._time) / Mathf.Max(1E-06f, keyframe2._time - keyframe1._time);
    for (int index = 0; index < this._playbackTransforms.Length; ++index)
    {
      if (!((UnityEngine.Object) this._playbackTransforms[index] == (UnityEngine.Object) null) && index < keyframe1._transforms.Length && index < keyframe2._transforms.Length)
      {
        Vector3 pos1 = keyframe1._transforms[index]._pos;
        Vector3 pos2 = keyframe2._transforms[index]._pos;
        Quaternion rot1 = keyframe1._transforms[index]._rot;
        Quaternion rot2 = keyframe2._transforms[index]._rot;
        this._playbackTransforms[index].position = rotation * Vector3.Lerp(pos1, pos2, t) + position;
        this._playbackTransforms[index].rotation = rotation * Quaternion.Slerp(rot1, rot2, t);
      }
    }
  }

  public virtual void Save()
  {
    BinaryFormatter binaryFormatter = new BinaryFormatter();
    FileStream serializationStream = File.Open(this._saveFilename, FileMode.OpenOrCreate);
    VRTrackersRecorder.SavedData graph = new VRTrackersRecorder.SavedData();
    graph._keyframes = new VRTrackersRecorder.SavedData.KeyframeSerializable[this._keyframes.Count];
    for (int index1 = 0; index1 < this._keyframes.Count; ++index1)
    {
      VRTrackersRecorder.Keyframe keyframe = this._keyframes[index1];
      VRTrackersRecorder.SavedData.KeyframeSerializable keyframeSerializable = new VRTrackersRecorder.SavedData.KeyframeSerializable();
      keyframeSerializable._transforms = new VRTrackersRecorder.SavedData.KeyframeSerializable.TransformSerializable[keyframe._transforms.Length];
      for (int index2 = 0; index2 < keyframe._transforms.Length; ++index2)
      {
        VRTrackersRecorder.SavedData.KeyframeSerializable.TransformSerializable transformSerializable = keyframeSerializable._transforms[index2] = new VRTrackersRecorder.SavedData.KeyframeSerializable.TransformSerializable();
        transformSerializable._valid = keyframeSerializable._transforms[index2]._valid;
        Vector3 pos = keyframe._transforms[index2]._pos;
        Quaternion rot = keyframe._transforms[index2]._rot;
        transformSerializable._xPos = pos.x;
        transformSerializable._yPos = pos.y;
        transformSerializable._zPos = pos.z;
        transformSerializable._xRot = rot.x;
        transformSerializable._yRot = rot.y;
        transformSerializable._zRot = rot.z;
        transformSerializable._wRot = rot.w;
      }
      keyframeSerializable._time = keyframe._time;
      graph._keyframes[index1] = keyframeSerializable;
    }
    binaryFormatter.Serialize((Stream) serializationStream, (object) graph);
    serializationStream.Close();
    Debug.Log((object) ("Performance saved to file " + this._saveFilename));
  }

  public virtual void Load()
  {
    BinaryFormatter binaryFormatter = new BinaryFormatter();
    FileStream serializationStream = (FileStream) null;
    VRTrackersRecorder.SavedData savedData = (VRTrackersRecorder.SavedData) null;
    try
    {
      serializationStream = File.Open(this._saveFilename, FileMode.Open);
      savedData = (VRTrackersRecorder.SavedData) binaryFormatter.Deserialize((Stream) serializationStream);
    }
    catch
    {
      savedData = (VRTrackersRecorder.SavedData) null;
    }
    finally
    {
      serializationStream?.Close();
    }
    if (savedData != null)
    {
      this._keyframes = new List<VRTrackersRecorder.Keyframe>(savedData._keyframes.Length);
      for (int index1 = 0; index1 < savedData._keyframes.Length; ++index1)
      {
        VRTrackersRecorder.SavedData.KeyframeSerializable keyframe1 = savedData._keyframes[index1];
        VRTrackersRecorder.Keyframe keyframe2 = new VRTrackersRecorder.Keyframe();
        keyframe2._transforms = new VRTrackersRecorder.Keyframe.KeyframeTransform[keyframe1._transforms.Length];
        for (int index2 = 0; index2 < keyframe2._transforms.Length; ++index2)
        {
          VRTrackersRecorder.SavedData.KeyframeSerializable.TransformSerializable transform = keyframe1._transforms[index2];
          keyframe2._transforms[index2] = new VRTrackersRecorder.Keyframe.KeyframeTransform();
          keyframe2._transforms[index2]._valid = transform._valid;
          keyframe2._transforms[index2]._pos = new Vector3(transform._xPos, transform._yPos, transform._zPos);
          keyframe2._transforms[index2]._rot = new Quaternion(transform._xRot, transform._yRot, transform._zRot, transform._wRot);
        }
        keyframe2._time = keyframe1._time;
        this._keyframes.Add(keyframe2);
      }
      Debug.Log((object) ("Performance loaded from file " + this._saveFilename));
    }
    else
    {
      Debug.Log((object) ("Loading performance file failed (" + this._saveFilename + ")"));
      this.enabled = false;
    }
  }

  [Serializable]
  public class SavedData
  {
    public VRTrackersRecorder.SavedData.KeyframeSerializable[] _keyframes;

    [Serializable]
    public class KeyframeSerializable
    {
      [SerializeField]
      public VRTrackersRecorder.SavedData.KeyframeSerializable.TransformSerializable[] _transforms;
      [SerializeField]
      public float _time;

      [Serializable]
      public class TransformSerializable
      {
        [SerializeField]
        public float _xPos;
        [SerializeField]
        public float _yPos;
        [SerializeField]
        public float _zPos;
        [SerializeField]
        public float _xRot;
        [SerializeField]
        public float _yRot;
        [SerializeField]
        public float _zRot;
        [SerializeField]
        public float _wRot;
        [SerializeField]
        public bool _valid;
      }
    }
  }

  public enum RecordMode
  {
    Record,
    Playback,
    Off,
  }

  public class Keyframe
  {
    public VRTrackersRecorder.Keyframe.KeyframeTransform[] _transforms;
    public float _time;

    public class KeyframeTransform
    {
      public Vector3 _pos;
      public Quaternion _rot;
      public bool _valid;
    }
  }
}
