// Decompiled with JetBrains decompiler
// Type: ScreenshotRecorder
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System.IO;
using UnityEngine;
using UnityEngine.Rendering;

public class ScreenshotRecorder : MonoBehaviour
{
  [SerializeField]
  protected string _directory = "Screenshots";
  [SerializeField]
  [NullAllowed]
  protected Camera _camera;
  [SerializeField]
  protected int _frameRate = 60;
  [SerializeField]
  protected bool _forceFixedFramerate;
  [SerializeField]
  protected int _interval = 20;
  [SerializeField]
  protected ScreenshotRecorder.RecordingType _recordingType = ScreenshotRecorder.RecordingType.F10ForScreenshot;
  [SerializeField]
  protected bool _pauseWithPButton = true;
  [SerializeField]
  protected int _antiAlias = 8;
  [SerializeField]
  protected int _screenshotWidth = 1920;
  [SerializeField]
  protected int _screenshotHeight = 1080;
  protected int _counter;
  protected float _originalTimeScale;
  protected bool _paused;
  protected int _frameNum;
  protected RenderTexture _cubemapLeftEye;
  protected RenderTexture _cubemapRighEye;
  protected RenderTexture _equirectTexture;
  protected RenderTexture _cameraRenderTexture;

  public string directory
  {
    get => this._directory;
    set => this._directory = value;
  }

  public virtual void OnEnable()
  {
    if (this._recordingType == ScreenshotRecorder.RecordingType.Sequence || this._recordingType == ScreenshotRecorder.RecordingType.Stereo360Sequence || this._recordingType == ScreenshotRecorder.RecordingType.Mono360Sequence || this._forceFixedFramerate)
      Time.captureFramerate = this._frameRate;
    Directory.CreateDirectory(this._directory);
    this._counter = this._interval;
    this._cubemapLeftEye = new RenderTexture(1024, 1024, 24);
    this._cubemapLeftEye.dimension = TextureDimension.Cube;
    this._cubemapRighEye = new RenderTexture(1024, 1024, 24);
    this._cubemapRighEye.dimension = TextureDimension.Cube;
    this._equirectTexture = new RenderTexture(1920, 2160, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
    this._cameraRenderTexture = new RenderTexture(this._screenshotWidth, this._screenshotHeight, 24, RenderTextureFormat.ARGB32);
    this._cameraRenderTexture.antiAliasing = this._antiAlias;
    this._camera.targetTexture = this._cameraRenderTexture;
  }

  public virtual void OnDisable()
  {
    this._cubemapLeftEye.Release();
    this._cubemapRighEye.Release();
    this._equirectTexture.Release();
    this._cameraRenderTexture.Release();
    Object.Destroy((Object) this._cubemapLeftEye);
    Object.Destroy((Object) this._cubemapRighEye);
    Object.Destroy((Object) this._equirectTexture);
    Object.Destroy((Object) this._cameraRenderTexture);
  }

  public virtual void LateUpdate()
  {
    if (this._recordingType == ScreenshotRecorder.RecordingType.Sequence)
      this.SaveCameraScreenshot();
    else if (this._recordingType != ScreenshotRecorder.RecordingType.Stereo360Sequence && this._recordingType != ScreenshotRecorder.RecordingType.Mono360Sequence)
    {
      if (this._recordingType == ScreenshotRecorder.RecordingType.Interval && this._counter == 0)
      {
        this.SaveCameraScreenshot();
        this._counter = this._interval;
      }
      else if (this._recordingType == ScreenshotRecorder.RecordingType.F10ForScreenshot && Input.GetKeyDown(KeyCode.F10))
        this.SaveCameraScreenshot();
    }
    if (this._counter > 0)
      --this._counter;
    if (!this._pauseWithPButton || !Input.GetKeyDown(KeyCode.P))
      return;
    this._paused = !this._paused;
    if (this._paused)
    {
      this._originalTimeScale = Time.timeScale;
      Time.timeScale = 0.0f;
    }
    else
      Time.timeScale = this._originalTimeScale;
  }

  public virtual void OnApplicationFocus(bool hasFocus)
  {
    if (!(this._recordingType == ScreenshotRecorder.RecordingType.ScreenshotOnPause & hasFocus))
      return;
    this.SaveCameraScreenshot();
  }

  public virtual void SaveCameraScreenshot()
  {
    Texture2D tex = this.ConvertRenderTexture(this._camera.targetTexture);
    this.SaveTextureScreenshot(tex);
    Object.Destroy((Object) tex);
  }

  public virtual void SaveTextureScreenshot(Texture2D tex)
  {
    string path = string.Format("{0}/{1:D05}.png", (object) this._directory, (object) this._frameNum);
    byte[] png = tex.EncodeToPNG();
    File.WriteAllBytes(path, png);
    Debug.Log((object) ("Screenshot saved to \"" + path + "\""));
    ++this._frameNum;
  }

  public virtual Texture2D ConvertRenderTexture(RenderTexture renderTexture)
  {
    Texture2D texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
    RenderTexture.active = renderTexture;
    texture2D.ReadPixels(new Rect(0.0f, 0.0f, (float) renderTexture.width, (float) renderTexture.height), 0, 0);
    return texture2D;
  }

  public enum RecordingType
  {
    Sequence,
    Stereo360Sequence,
    Mono360Sequence,
    F10ForScreenshot,
    Interval,
    ScreenshotOnPause,
  }
}
