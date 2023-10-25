// Decompiled with JetBrains decompiler
// Type: VRControllersRecorder
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Zenject;

public class VRControllersRecorder : MonoBehaviour
{
  [SerializeField]
  [NullAllowed]
  protected TextAsset _recordingTextAsset;
  [SerializeField]
  protected string _recordingFileName = "VRControllersRecording.dat";
  [SerializeField]
  protected VRControllersRecorder.Mode _mode = VRControllersRecorder.Mode.Off;
  [Space]
  [SerializeField]
  protected bool _dontMoveHead;
  [SerializeField]
  protected bool _changeToNonVRCamera;
  [SerializeField]
  protected bool _adjustSabersPositionBasedOnHeadPosition;
  [SerializeField]
  protected Vector3 _headRotationOffset;
  [SerializeField]
  protected Vector3 _headPositionOffset;
  [SerializeField]
  protected float _headSmooth = 8f;
  [SerializeField]
  protected float _cameraFOV = 65f;
  [SerializeField]
  protected float _controllersTimeOffset;
  [SerializeField]
  protected float _controllersSmooth = 8f;
  [Space]
  [SerializeField]
  protected VRController _controller0;
  [SerializeField]
  protected VRController _controller1;
  [SerializeField]
  protected Transform _headTransform;
  [SerializeField]
  protected Camera _camera;
  [Space]
  [SerializeField]
  protected Camera _recorderCamera;
  [SerializeField]
  protected Transform _spawnRotationTransform;
  [Space]
  [SerializeField]
  protected Transform _originTransform;
  [Inject]
  protected readonly AudioTimeSyncController _audioTimeSyncController;
  protected List<VRControllersRecorder.Keyframe> _keyframes;
  protected int _keyframeIndex;

  public VRControllersRecorder.Mode mode
  {
    set => this._mode = value;
    get => this._mode;
  }

  public TextAsset recordingTextAsset
  {
    set => this._recordingTextAsset = value;
    get => this._recordingTextAsset;
  }

  public string recordingFileName
  {
    set => this._recordingFileName = value;
    get => this._recordingFileName;
  }

  public bool changeToNonVRCamera
  {
    set => this._changeToNonVRCamera = value;
    get => this._changeToNonVRCamera;
  }

  public virtual void Start()
  {
    if (this._recordingFileName == "")
      this._recordingFileName = "Recordings/VRControllersRecording.rd";
    if (this._mode == VRControllersRecorder.Mode.Off)
      this.enabled = false;
    this._recorderCamera.gameObject.SetActive(false);
    if (this._mode == VRControllersRecorder.Mode.Playback)
    {
      this._controller0.enabled = false;
      this._controller1.enabled = false;
      if (this._changeToNonVRCamera)
      {
        Camera camera = new GameObject("TempVRCamera").AddComponent<Camera>();
        camera.CopyFrom(this._camera);
        camera.targetDisplay = 2;
        this._camera.enabled = false;
        this._recorderCamera.fieldOfView = this._cameraFOV;
        this._recorderCamera.gameObject.SetActive(true);
      }
    }
    this._keyframes = new List<VRControllersRecorder.Keyframe>();
    if (this._mode == VRControllersRecorder.Mode.Playback)
    {
      this.Load();
      if (this._keyframes.Count <= 0 || !((UnityEngine.Object) this._headTransform != (UnityEngine.Object) null) || this._dontMoveHead)
        return;
      this._headTransform.transform.SetPositionAndRotation(this._keyframes[0]._pos3, this._keyframes[0]._rot3);
    }
    else
    {
      if (this._mode != VRControllersRecorder.Mode.Record)
        return;
      this._originTransform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
      Debug.LogWarning((object) "Recording performance.");
    }
  }

  public virtual void OnDestroy()
  {
    if (this._mode != VRControllersRecorder.Mode.Record)
      return;
    this.Save();
  }

  public virtual void SetDefaultSettings()
  {
    this._dontMoveHead = false;
    this._changeToNonVRCamera = true;
    this._adjustSabersPositionBasedOnHeadPosition = false;
    this._headRotationOffset = Vector3.zero;
    this._headPositionOffset = Vector3.zero;
    this._headSmooth = 0.0f;
    this._cameraFOV = 65f;
    this._controllersTimeOffset = 0.0f;
    this._controllersSmooth = 0.0f;
  }

  public virtual void SetInGamePlaybackDefaultSettings()
  {
    this._dontMoveHead = true;
    this._changeToNonVRCamera = false;
    this._adjustSabersPositionBasedOnHeadPosition = true;
    this._headRotationOffset = Vector3.zero;
    this._headPositionOffset = Vector3.zero;
    this._headSmooth = 0.0f;
    this._cameraFOV = 65f;
    this._controllersTimeOffset = 0.0f;
    this._controllersSmooth = 0.0f;
    this._mode = VRControllersRecorder.Mode.Playback;
  }

  public virtual void PlaybackTick()
  {
    float num = this._audioTimeSyncController.songTime + this._controllersTimeOffset;
    while (this._keyframeIndex < this._keyframes.Count - 2 && (double) this._keyframes[this._keyframeIndex + 1]._time < (double) num)
      ++this._keyframeIndex;
    VRControllersRecorder.Keyframe keyframe1 = this._keyframes[this._keyframeIndex];
    VRControllersRecorder.Keyframe keyframe2 = this._keyframes[this._keyframeIndex + 1];
    float t1 = (num - keyframe1._time) / Mathf.Max(1E-06f, keyframe2._time - keyframe1._time);
    float t2 = (double) this._controllersSmooth == 0.0 ? 1f : Time.deltaTime * this._controllersSmooth;
    Vector3 vector3_1 = Vector3.Lerp(keyframe1._pos3, keyframe2._pos3, t1);
    if ((UnityEngine.Object) this._controller0.transform != (UnityEngine.Object) null)
    {
      if (!this._controller0.transform.gameObject.activeSelf)
        this._controller0.transform.gameObject.SetActive(true);
      Vector3 targetPos = Vector3.Lerp(keyframe1._pos1, keyframe2._pos1, t1);
      if (this._adjustSabersPositionBasedOnHeadPosition)
        targetPos.z += this._headTransform.position.z - vector3_1.z;
      Quaternion targetRot = Quaternion.Lerp(keyframe1._rot1, keyframe2._rot1, t1);
      this.SetPositionAndRotation(this._controller0.transform.transform, targetPos, targetRot, t2);
    }
    if ((UnityEngine.Object) this._controller1.transform != (UnityEngine.Object) null)
    {
      if (!this._controller1.transform.gameObject.activeSelf)
        this._controller1.transform.gameObject.SetActive(true);
      Vector3 targetPos = Vector3.Lerp(keyframe1._pos2, keyframe2._pos2, t1);
      if (this._adjustSabersPositionBasedOnHeadPosition)
        targetPos.z += this._headTransform.position.z - vector3_1.z;
      Quaternion targetRot = Quaternion.Lerp(keyframe1._rot2, keyframe2._rot2, t1);
      this.SetPositionAndRotation(this._controller1.transform.transform, targetPos, targetRot, t2);
    }
    if (!((UnityEngine.Object) this._headTransform != (UnityEngine.Object) null))
      return;
    Vector3 position = vector3_1;
    if (this._dontMoveHead)
      return;
    Quaternion quaternion = Quaternion.Lerp(keyframe1._rot3, keyframe2._rot3, t1);
    this._headTransform.SetPositionAndRotation(position, quaternion);
    Vector3 vector3_2 = quaternion.eulerAngles + this._headRotationOffset;
    quaternion.eulerAngles = vector3_2;
    Vector3 vector3_3 = position + this._spawnRotationTransform.TransformPoint(this._headPositionOffset);
    if ((double) this._headSmooth == 0.0)
    {
      this._recorderCamera.transform.SetPositionAndRotation(vector3_3, quaternion);
    }
    else
    {
      float t3 = (double) this._headSmooth == 0.0 ? 1f : Time.deltaTime * this._headSmooth;
      this._recorderCamera.transform.SetPositionAndRotation(Vector3.Lerp(this._recorderCamera.transform.position, vector3_3, t3), Quaternion.Lerp(this._recorderCamera.transform.rotation, quaternion, t3));
    }
  }

  public virtual void SetPositionAndRotation(
    Transform transf,
    Vector3 targetPos,
    Quaternion targetRot,
    float t)
  {
    Vector3 position1 = transf.position;
    Quaternion rotation1 = transf.rotation;
    Vector3 position2 = Vector3.Lerp(position1, targetPos, t);
    Quaternion rotation2 = Quaternion.Lerp(rotation1, targetRot, t);
    transf.SetPositionAndRotation(position2, rotation2);
  }

  public virtual void RecordTick()
  {
    if ((double) this._audioTimeSyncController.songTime == 0.0)
      return;
    VRControllersRecorder.Keyframe keyframe = new VRControllersRecorder.Keyframe();
    if ((UnityEngine.Object) this._controller0.transform != (UnityEngine.Object) null)
    {
      keyframe._pos1 = this._controller0.transform.position;
      keyframe._rot1 = this._controller0.transform.rotation;
    }
    if ((UnityEngine.Object) this._controller1.transform != (UnityEngine.Object) null)
    {
      keyframe._pos2 = this._controller1.transform.position;
      keyframe._rot2 = this._controller1.transform.rotation;
    }
    if ((UnityEngine.Object) this._headTransform != (UnityEngine.Object) null)
    {
      keyframe._pos3 = this._headTransform.position;
      keyframe._rot3 = this._headTransform.rotation;
    }
    keyframe._time = this._audioTimeSyncController.songTime;
    this._keyframes.Add(keyframe);
  }

  public virtual void Update()
  {
    if (this._mode != VRControllersRecorder.Mode.Playback || this._keyframes.Count <= 1)
      return;
    this.PlaybackTick();
    this._recorderCamera.fieldOfView = this._cameraFOV;
  }

  public virtual void LateUpdate()
  {
    if (this._mode != VRControllersRecorder.Mode.Record)
      return;
    this.RecordTick();
  }

  public virtual void Save()
  {
    if (this._keyframes == null || this._keyframes.Count == 0)
      return;
    BinaryFormatter binaryFormatter = new BinaryFormatter();
    binaryFormatter.Binder = (SerializationBinder) new VRControllersRecorder.TypeSerializationBinder();
    FileStream serializationStream = File.Open(this._recordingFileName, FileMode.OpenOrCreate);
    VRControllersRecorder.SavedData graph = new VRControllersRecorder.SavedData();
    graph._keyframes = new VRControllersRecorder.SavedData.KeyframeSerializable[this._keyframes.Count];
    for (int index = 0; index < this._keyframes.Count; ++index)
    {
      VRControllersRecorder.Keyframe keyframe = this._keyframes[index];
      graph._keyframes[index] = new VRControllersRecorder.SavedData.KeyframeSerializable()
      {
        _xPos1 = keyframe._pos1.x,
        _yPos1 = keyframe._pos1.y,
        _zPos1 = keyframe._pos1.z,
        _xPos2 = keyframe._pos2.x,
        _yPos2 = keyframe._pos2.y,
        _zPos2 = keyframe._pos2.z,
        _xPos3 = keyframe._pos3.x,
        _yPos3 = keyframe._pos3.y,
        _zPos3 = keyframe._pos3.z,
        _xRot1 = keyframe._rot1.x,
        _yRot1 = keyframe._rot1.y,
        _zRot1 = keyframe._rot1.z,
        _wRot1 = keyframe._rot1.w,
        _xRot2 = keyframe._rot2.x,
        _yRot2 = keyframe._rot2.y,
        _zRot2 = keyframe._rot2.z,
        _wRot2 = keyframe._rot2.w,
        _xRot3 = keyframe._rot3.x,
        _yRot3 = keyframe._rot3.y,
        _zRot3 = keyframe._rot3.z,
        _wRot3 = keyframe._rot3.w,
        _time = keyframe._time
      };
    }
    binaryFormatter.Serialize((Stream) serializationStream, (object) graph);
    serializationStream.Close();
    Debug.Log((object) ("Performance saved to file " + this._recordingFileName));
  }

  public virtual void Load()
  {
    BinaryFormatter binaryFormatter = new BinaryFormatter();
    binaryFormatter.Binder = (SerializationBinder) new VRControllersRecorder.TypeSerializationBinder();
    savedData = (VRControllersRecorder.SavedData) null;
    if ((bool) (UnityEngine.Object) this._recordingTextAsset)
    {
      Stream serializationStream = (Stream) new MemoryStream(this._recordingTextAsset.bytes);
      if (binaryFormatter.Deserialize(serializationStream) is VRControllersRecorder.SavedData savedData)
        Debug.Log((object) ("Performance loaded from text asset " + (object) this._recordingTextAsset));
    }
    else
    {
      FileStream serializationStream = (FileStream) null;
      try
      {
        serializationStream = File.Open(this._recordingFileName, FileMode.Open);
        savedData = (VRControllersRecorder.SavedData) binaryFormatter.Deserialize((Stream) serializationStream);
      }
      catch
      {
        savedData = (VRControllersRecorder.SavedData) null;
      }
      finally
      {
        serializationStream?.Close();
      }
      if (savedData != null)
        Debug.Log((object) ("Performance loaded from file " + this._recordingFileName));
    }
    if (savedData != null)
    {
      this._keyframes = new List<VRControllersRecorder.Keyframe>(savedData._keyframes.Length);
      for (int index = 0; index < savedData._keyframes.Length; ++index)
      {
        VRControllersRecorder.SavedData.KeyframeSerializable keyframe = savedData._keyframes[index];
        this._keyframes.Add(new VRControllersRecorder.Keyframe()
        {
          _pos1 = new Vector3(keyframe._xPos1, keyframe._yPos1, keyframe._zPos1),
          _pos2 = new Vector3(keyframe._xPos2, keyframe._yPos2, keyframe._zPos2),
          _pos3 = new Vector3(keyframe._xPos3, keyframe._yPos3, keyframe._zPos3),
          _rot1 = new Quaternion(keyframe._xRot1, keyframe._yRot1, keyframe._zRot1, keyframe._wRot1),
          _rot2 = new Quaternion(keyframe._xRot2, keyframe._yRot2, keyframe._zRot2, keyframe._wRot2),
          _rot3 = new Quaternion(keyframe._xRot3, keyframe._yRot3, keyframe._zRot3, keyframe._wRot3),
          _time = keyframe._time
        });
      }
    }
    else
    {
      Debug.LogWarning((object) ("Loading performance file failed (" + this._recordingFileName + ")"));
      this.enabled = false;
    }
  }

  public static AnimationClip CreateAnimationClipFromRecording(string recordingfilePath)
  {
    BinaryFormatter binaryFormatter = new BinaryFormatter();
    FileStream serializationStream = (FileStream) null;
    VRControllersRecorder.SavedData savedData = (VRControllersRecorder.SavedData) null;
    try
    {
      serializationStream = File.Open(recordingfilePath, FileMode.Open);
      savedData = (VRControllersRecorder.SavedData) binaryFormatter.Deserialize((Stream) serializationStream);
    }
    catch
    {
      savedData = (VRControllersRecorder.SavedData) null;
    }
    finally
    {
      serializationStream?.Close();
    }
    if (savedData != null)
    {
      AnimationClip clipFromRecording = new AnimationClip();
      AnimationCurve[] animationCurveArray1 = new AnimationCurve[3];
      AnimationCurve[] animationCurveArray2 = new AnimationCurve[3];
      AnimationCurve[] animationCurveArray3 = new AnimationCurve[3];
      AnimationCurve[] animationCurveArray4 = new AnimationCurve[3];
      AnimationCurve[] animationCurveArray5 = new AnimationCurve[3];
      AnimationCurve[] animationCurveArray6 = new AnimationCurve[3];
      for (int index = 0; index < 3; ++index)
      {
        animationCurveArray1[index] = new AnimationCurve();
        animationCurveArray2[index] = new AnimationCurve();
        animationCurveArray3[index] = new AnimationCurve();
        animationCurveArray4[index] = new AnimationCurve();
        animationCurveArray5[index] = new AnimationCurve();
        animationCurveArray6[index] = new AnimationCurve();
      }
      for (int index = 0; index < savedData._keyframes.Length; ++index)
      {
        VRControllersRecorder.SavedData.KeyframeSerializable keyframe = savedData._keyframes[index];
        animationCurveArray1[0].AddKey(keyframe._time, keyframe._xPos1);
        animationCurveArray2[0].AddKey(keyframe._time, keyframe._yPos1);
        animationCurveArray3[0].AddKey(keyframe._time, keyframe._zPos1);
        animationCurveArray4[0].AddKey(keyframe._time, keyframe._xRot1);
        animationCurveArray5[0].AddKey(keyframe._time, keyframe._yRot1);
        animationCurveArray6[0].AddKey(keyframe._time, keyframe._zRot1);
        animationCurveArray1[1].AddKey(keyframe._time, keyframe._xPos2);
        animationCurveArray2[1].AddKey(keyframe._time, keyframe._yPos2);
        animationCurveArray3[1].AddKey(keyframe._time, keyframe._zPos2);
        animationCurveArray4[1].AddKey(keyframe._time, keyframe._xRot2);
        animationCurveArray5[1].AddKey(keyframe._time, keyframe._yRot2);
        animationCurveArray6[1].AddKey(keyframe._time, keyframe._zRot2);
        animationCurveArray1[2].AddKey(keyframe._time, keyframe._xPos3);
        animationCurveArray2[2].AddKey(keyframe._time, keyframe._yPos3);
        animationCurveArray3[2].AddKey(keyframe._time, keyframe._zPos3);
        animationCurveArray4[2].AddKey(keyframe._time, keyframe._xRot3);
        animationCurveArray5[2].AddKey(keyframe._time, keyframe._yRot3);
        animationCurveArray6[2].AddKey(keyframe._time, keyframe._zRot3);
      }
      string[] strArray = new string[3]
      {
        "HandControllers/LeftSaber",
        "HandControllers/RightSaber",
        "MainCamera"
      };
      for (int index = 0; index < 3; ++index)
      {
        clipFromRecording.SetCurve(strArray[index], typeof (Transform), "localPosition.x", animationCurveArray1[index]);
        clipFromRecording.SetCurve(strArray[index], typeof (Transform), "localPosition.y", animationCurveArray2[index]);
        clipFromRecording.SetCurve(strArray[index], typeof (Transform), "localPosition.z", animationCurveArray3[index]);
        clipFromRecording.SetCurve(strArray[index], typeof (Transform), "localRotation.x", animationCurveArray4[index]);
        clipFromRecording.SetCurve(strArray[index], typeof (Transform), "localRotation.y", animationCurveArray5[index]);
        clipFromRecording.SetCurve(strArray[index], typeof (Transform), "localRotation.z", animationCurveArray6[index]);
      }
      Debug.Log((object) ("Performance loaded from file " + recordingfilePath));
      return clipFromRecording;
    }
    Debug.LogWarning((object) ("Loading performance file failed (" + recordingfilePath + ")"));
    return (AnimationClip) null;
  }

  public class TypeSerializationBinder : SerializationBinder
  {
    public override System.Type BindToType(string assemblyName, string typeName) => System.Type.GetType(typeName);
  }

  [Serializable]
  public class SavedData
  {
    public VRControllersRecorder.SavedData.KeyframeSerializable[] _keyframes;

    [Serializable]
    public class KeyframeSerializable
    {
      public float _xPos1;
      public float _yPos1;
      public float _zPos1;
      public float _xPos2;
      public float _yPos2;
      public float _zPos2;
      public float _xPos3;
      public float _yPos3;
      public float _zPos3;
      public float _xRot1;
      public float _yRot1;
      public float _zRot1;
      public float _wRot1;
      public float _xRot2;
      public float _yRot2;
      public float _zRot2;
      public float _wRot2;
      public float _xRot3;
      public float _yRot3;
      public float _zRot3;
      public float _wRot3;
      public float _time;
    }
  }

  public enum Mode
  {
    Record,
    Playback,
    Off,
  }

  public class Keyframe
  {
    public Vector3 _pos1;
    public Vector3 _pos2;
    public Vector3 _pos3;
    public Quaternion _rot1;
    public Quaternion _rot2;
    public Quaternion _rot3;
    public float _time;
  }
}
