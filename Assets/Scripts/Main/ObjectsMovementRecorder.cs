// Decompiled with JetBrains decompiler
// Type: ObjectsMovementRecorder
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Zenject;

[DefaultExecutionOrder(-4900)]
public class ObjectsMovementRecorder : MonoBehaviour
{
  [SerializeField]
  protected PoseObject[] _poseObjects;
  [Space]
  [SerializeField]
  protected PoseObjectIdSO _livPoseObjectId;
  [Space]
  [SerializeField]
  protected PosesRecorder _recorder;
  [SerializeField]
  protected PosesPlayback _playback;
  [SerializeField]
  protected PlaybackScreenshotRecorder _playbackScreenshotRecorder;
  [SerializeField]
  protected PlaybackRenderer _playbackRender;
  [Space]
  [SerializeField]
  protected Camera _externalCameraPrefab;
  [Inject]
  protected readonly AudioTimeSyncController _audioTimeSyncController;
  [Inject]
  protected readonly Camera _hmdCamera;
  [InjectOptional]
  protected readonly ObjectsMovementRecorder.InitData _initData;
  protected ObjectsMovementRecorder.Mode _mode = ObjectsMovementRecorder.Mode.Off;
  protected string _recordingPath;
  protected ObjectsMovementRecorder.CameraView _cameraView;
  protected Camera _externalCamera;
  protected PlaybackRenderer.PlaybackScreenshot[] _playbackScreenshots;
  protected IBeatSaberLogger _logger;
  protected IPosesSerializer _posesSerializer;

  [Inject]
  public virtual void Init()
  {
    if (this._initData == null || this._initData.mode == ObjectsMovementRecorder.Mode.Off)
    {
      this.enabled = false;
    }
    else
    {
      this._logger = this._initData.logger;
      this._posesSerializer = this._initData.posesSerializer;
      if (string.IsNullOrEmpty(this._initData.recordingPath))
      {
        this._logger.LogError("Recording path is null or empty. Recording tool will be disabled");
        this.enabled = false;
      }
      else
      {
        this.enabled = true;
        this._mode = this._initData.mode;
        this._cameraView = this._initData.cameraView;
        this._recordingPath = this._initData.recordingPath;
        string str = DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss");
        if (this._mode == ObjectsMovementRecorder.Mode.Record)
        {
          if (this._initData.addDateTimeSuffixToRecordingName)
            this._recordingPath = this._recordingPath + "-" + str;
          PosesRecordingData.ExternalCameraCalibration externalCameraCalibration = (PosesRecordingData.ExternalCameraCalibration) null;
          LivStaticWrapper.Activate();
          if (LivStaticWrapper.isActive)
          {
            List<PoseObject> list = ((IEnumerable<PoseObject>) this._poseObjects).ToList<PoseObject>();
            list.Add(new PoseObject(LivStaticWrapper.GetExternalCamera().transform, this._livPoseObjectId));
            this._poseObjects = list.ToArray();
            externalCameraCalibration = new PosesRecordingData.ExternalCameraCalibration(LivStaticWrapper.GetExternalCamera());
            this._logger.Log("LIV is active. External camera will be added to the recording.");
          }
          this._recorder.Init(this._poseObjects, externalCameraCalibration);
        }
        else if (this._mode == ObjectsMovementRecorder.Mode.Playback)
        {
          PosesRecordingData data = this._posesSerializer.LoadRecording(this._recordingPath);
          if (data == null)
          {
            this._logger.LogError("Recording " + this._recordingPath + " cannot be loaded. Recording tool will be disabled.");
            this.enabled = false;
            return;
          }
          if (this._cameraView == ObjectsMovementRecorder.CameraView.ThirdPerson && !data.Contains(this._livPoseObjectId.id))
          {
            this._logger.LogWarning("Third person view is selected for playback via command line parameter, but selected recording does not contain data for external camera. First person view will be used.");
            this._cameraView = ObjectsMovementRecorder.CameraView.FirstPerson;
          }
          Camera camera;
          PosesRecordingData.ExternalCameraCalibration cameraCalibration;
          if (this._cameraView == ObjectsMovementRecorder.CameraView.ThirdPerson)
          {
            this._externalCamera = UnityEngine.Object.Instantiate<Camera>(this._externalCameraPrefab, this._hmdCamera.transform.parent, true);
            this._externalCamera.enabled = true;
            camera = this._externalCamera;
            cameraCalibration = data.externalCameraCalibration;
            List<PoseObject> list = ((IEnumerable<PoseObject>) this._poseObjects).ToList<PoseObject>();
            list.Add(new PoseObject(this._externalCamera.transform, this._livPoseObjectId));
            this._poseObjects = list.ToArray();
          }
          else
          {
            camera = this._hmdCamera;
            cameraCalibration = new PosesRecordingData.ExternalCameraCalibration(this._hmdCamera);
          }
          this._playback.Init(this._poseObjects, data, this._logger);
          if (this._initData.screenshotRecording)
          {
            this._playbackRender.Setup(this._hmdCamera, camera, cameraCalibration, this._initData.screenshotWidth, this._initData.screenshotHeight, this._initData.playbackScreenshots);
            this._playbackScreenshotRecorder.Init(Path.Combine(this._playbackScreenshotRecorder.directory, this._initData.addDateTimeSuffixToRecordingName ? str : ""), this._initData.framerate, this._playbackRender);
          }
          this._playbackRender.enabled = this._initData.screenshotRecording;
          this._playbackScreenshotRecorder.enabled = this._initData.screenshotRecording;
        }
        this._audioTimeSyncController.stateChangedEvent += new System.Action(this.HandleGameStateChanged);
      }
    }
  }

  public virtual void OnDestroy()
  {
    if ((UnityEngine.Object) this._audioTimeSyncController != (UnityEngine.Object) null)
      this._audioTimeSyncController.stateChangedEvent -= new System.Action(this.HandleGameStateChanged);
    if (this._mode == ObjectsMovementRecorder.Mode.Record)
    {
      this._recorder.StopRecording();
      this._posesSerializer.SaveRecording(this._recordingPath, this._recorder.data, this._initData.saveToOldFormat);
    }
    else if (this._mode == ObjectsMovementRecorder.Mode.Playback)
      this._playback.StopPlayback();
    if (!((UnityEngine.Object) this._externalCamera != (UnityEngine.Object) null))
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this._externalCamera.gameObject);
  }

  public virtual void HandleGameStateChanged()
  {
    if (this._mode == ObjectsMovementRecorder.Mode.Record)
    {
      if (this._audioTimeSyncController.state == AudioTimeSyncController.State.Playing)
        this._recorder.StartRecording();
      else
        this._recorder.StopRecording();
    }
    else
    {
      if (this._mode != ObjectsMovementRecorder.Mode.Playback)
        return;
      if (this._audioTimeSyncController.state == AudioTimeSyncController.State.Playing)
        this._playback.StartPlayback();
      else
        this._playback.StopPlayback();
    }
  }

  public enum Mode
  {
    Record,
    Playback,
    Off,
  }

  public enum CameraView
  {
    FirstPerson,
    ThirdPerson,
  }

  public class InitData
  {
    public readonly ObjectsMovementRecorder.Mode mode;
    public readonly string recordingPath;
    public readonly ObjectsMovementRecorder.CameraView cameraView;
    public readonly bool addDateTimeSuffixToRecordingName;
    public readonly bool screenshotRecording;
    public readonly int screenshotWidth;
    public readonly int screenshotHeight;
    public readonly int framerate;
    public readonly PlaybackRenderer.PlaybackScreenshot[] playbackScreenshots;
    public readonly bool saveToOldFormat;
    public readonly IPosesSerializer posesSerializer;
    public readonly IBeatSaberLogger logger;

    public InitData(
      ObjectsMovementRecorder.Mode mode,
      string recordingPath,
      ObjectsMovementRecorder.CameraView cameraView,
      bool addDateTimeSuffixToRecordingName,
      bool screenshotRecording,
      int screenshotWidth,
      int screenshotHeight,
      int framerate,
      PlaybackRenderer.PlaybackScreenshot[] playbackScreenshots,
      bool saveToOldFormat,
      IBeatSaberLogger logger,
      IPosesSerializer posesSerializer)
    {
      this.mode = mode;
      this.recordingPath = recordingPath;
      this.cameraView = cameraView;
      this.addDateTimeSuffixToRecordingName = addDateTimeSuffixToRecordingName;
      this.screenshotRecording = screenshotRecording;
      this.screenshotWidth = screenshotWidth;
      this.screenshotHeight = screenshotHeight;
      this.framerate = framerate;
      this.playbackScreenshots = playbackScreenshots;
      this.saveToOldFormat = saveToOldFormat;
      this.logger = logger;
      this.posesSerializer = posesSerializer;
    }
  }
}
