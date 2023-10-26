// Decompiled with JetBrains decompiler
// Type: SimpleVRNodeRecorder
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.XR;
using Zenject;

public class SimpleVRNodeRecorder : MonoBehaviour
{
  [SerializeField]
  protected FloatSO _songTime;
  [SerializeField]
  protected string _saveFilename = "VRControllersRecording.dat";
  [SerializeField]
  protected SimpleVRNodeRecorder.RecordMode _mode = SimpleVRNodeRecorder.RecordMode.Off;
  [SerializeField]
  protected XRNode _node;
  [SerializeField]
  protected Transform _playbackTransform;
  [SerializeField]
  protected float _smooth = 4f;
  [SerializeField]
  protected float _forwardOffset;
  [Inject]
  protected IVRPlatformHelper _vrPlatformHelper;
  protected List<SimpleVRNodeRecorder.SavedData.NodeKeyframe> _keyframes;
  protected int _keyframeIndex;
  protected Vector3 _prevPos;
  protected Quaternion _prevRot;

  public virtual void Awake()
  {
    if (this._saveFilename == "")
      this._saveFilename = "VRControllersRecording.dat";
    this._keyframes = new List<SimpleVRNodeRecorder.SavedData.NodeKeyframe>();
    if (this._mode == SimpleVRNodeRecorder.RecordMode.Playback)
    {
      this.Load();
    }
    else
    {
      if (this._mode != SimpleVRNodeRecorder.RecordMode.Record)
        return;
      Debug.LogWarning((object) "Recording performance.");
    }
  }

  public virtual void OnDestroy()
  {
    if (this._mode != SimpleVRNodeRecorder.RecordMode.Record)
      return;
    this.Save();
  }

  public virtual void RecordNewKeyFrame()
  {
    Vector3 pos;
    Quaternion rot;
    this._vrPlatformHelper.GetNodePose(this._node, 0, out pos, out rot);
    this._keyframes.Add(new SimpleVRNodeRecorder.SavedData.NodeKeyframe(pos, rot, this._songTime.value));
  }

    public virtual void Update()
    {
        if (this._mode == SimpleVRNodeRecorder.RecordMode.Record)
        {
            this.RecordNewKeyFrame();
        }
        if (this._mode != SimpleVRNodeRecorder.RecordMode.Playback || this._keyframes.Count < 2)
        {
            return;
        }
        float value = this._songTime.value;
        while (this._keyframeIndex < this._keyframes.Count - 2 && this._keyframes[this._keyframeIndex + 1].time < value)
        {
            this._keyframeIndex++;
        }
        SimpleVRNodeRecorder.SavedData.NodeKeyframe nodeKeyframe = this._keyframes[this._keyframeIndex];
        SimpleVRNodeRecorder.SavedData.NodeKeyframe nodeKeyframe2 = this._keyframes[this._keyframeIndex + 1];
        float t = (value - nodeKeyframe.time) / Mathf.Max(1E-06f, nodeKeyframe2.time - nodeKeyframe.time);
        Vector3 pos = nodeKeyframe.pos;
        Vector3 pos2 = nodeKeyframe2.pos;
        Quaternion rot = nodeKeyframe.rot;
        Quaternion rot2 = nodeKeyframe2.rot;
        if (this._smooth < 0f)
        {
            this._playbackTransform.localPosition = Vector3.Lerp(pos, pos2, t);
            this._playbackTransform.localRotation = Quaternion.Slerp(rot, rot2, t);
            return;
        }
        Vector3 vector = Vector3.Lerp(this._prevPos, Vector3.Lerp(pos, pos2, t), Time.deltaTime * this._smooth);
        Quaternion quaternion = Quaternion.Slerp(this._prevRot, Quaternion.Slerp(rot, rot2, t), Time.deltaTime * this._smooth);
        Vector3 eulerAngles = quaternion.eulerAngles;
        eulerAngles.z = 0f;
        quaternion.eulerAngles = eulerAngles;
        this._playbackTransform.localPosition = vector;
        this._playbackTransform.localRotation = quaternion;
        this._playbackTransform.localPosition += this._playbackTransform.forward * this._forwardOffset;
        this._prevPos = vector;
        this._prevRot = quaternion;
    }

    public virtual void Save()
  {
    BinaryFormatter binaryFormatter = new BinaryFormatter();
    FileStream fileStream = File.Open(this._saveFilename, FileMode.OpenOrCreate);
    SimpleVRNodeRecorder.SavedData savedData = new SimpleVRNodeRecorder.SavedData();
    savedData.keyframes = this._keyframes.ToArray();
    FileStream serializationStream = fileStream;
    SimpleVRNodeRecorder.SavedData graph = savedData;
    binaryFormatter.Serialize((Stream) serializationStream, (object) graph);
    fileStream.Close();
    Debug.Log((object) ("Performance saved to file " + this._saveFilename));
  }

  public virtual void Load()
  {
    BinaryFormatter binaryFormatter = new BinaryFormatter();
    FileStream serializationStream = (FileStream) null;
    SimpleVRNodeRecorder.SavedData savedData = (SimpleVRNodeRecorder.SavedData) null;
    try
    {
      serializationStream = File.Open(this._saveFilename, FileMode.Open);
      savedData = (SimpleVRNodeRecorder.SavedData) binaryFormatter.Deserialize((Stream) serializationStream);
    }
    catch
    {
      savedData = (SimpleVRNodeRecorder.SavedData) null;
    }
    finally
    {
      serializationStream?.Close();
    }
    if (savedData != null)
    {
      this._keyframes = new List<SimpleVRNodeRecorder.SavedData.NodeKeyframe>((IEnumerable<SimpleVRNodeRecorder.SavedData.NodeKeyframe>) savedData.keyframes);
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
    public SimpleVRNodeRecorder.SavedData.NodeKeyframe[] keyframes;

    [Serializable]
    public class NodeKeyframe
    {
      public float posX;
      public float posY;
      public float posZ;
      public float rotX;
      public float rotY;
      public float rotZ;
      public float rotW;
      public float time;

      public Vector3 pos => new Vector3(this.posX, this.posY, this.posZ);

      public Quaternion rot => new Quaternion(this.rotX, this.rotY, this.rotZ, this.rotW);

      public NodeKeyframe(Vector3 pos, Quaternion rot, float time)
      {
        this.posX = pos.x;
        this.posY = pos.y;
        this.posZ = pos.z;
        this.rotX = rot.x;
        this.rotY = rot.y;
        this.rotZ = rot.z;
        this.rotW = rot.w;
        this.time = time;
      }
    }
  }

  public enum RecordMode
  {
    Record,
    Playback,
    Off,
  }
}
